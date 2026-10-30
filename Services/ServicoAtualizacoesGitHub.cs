using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BacklogManager.Configuracao;
using NuGet.Versioning;
namespace BacklogManager.Services;

public enum EstadoAtualizacao { Atualizado, Disponivel, SemPacote, Indisponivel, Inconclusivo }
public sealed record ResultadoAtualizacao(EstadoAtualizacao Estado,string Mensagem,string? Versao=null,Uri? Pagina=null);
public sealed class ServicoAtualizacoesGitHub
{
    private readonly HttpClient _cliente;
    private readonly string _repositorio;
    private readonly int _paginas;
    private readonly int _tamanhoPagina;
    private readonly TimeProvider _relogio;
    private DateTimeOffset _proximaConsulta;
    private readonly SemaphoreSlim _trava=new(1,1);
    public ServicoAtualizacoesGitHub(HttpClient cliente,string repositorio,int paginas=10,int tamanhoPagina=30,TimeProvider? relogio=null)
    {
        if(!Regex.IsMatch(repositorio,@"\A[A-Za-z0-9][A-Za-z0-9-]{0,38}/[A-Za-z0-9_.-]{1,100}\z") || repositorio.EndsWith("/.") || repositorio.EndsWith("/.."))throw new ArgumentException("Repositório oficial inválido.");
        if(paginas is <1 or >20 || tamanhoPagina is <1 or >100)throw new ArgumentOutOfRangeException(nameof(paginas));
        if(cliente.DefaultRequestHeaders.Authorization!=null || cliente.DefaultRequestHeaders.Contains("Client-ID"))throw new ArgumentException("Use um cliente anônimo exclusivo para atualizações.");
        _cliente=cliente;_repositorio=repositorio;_paginas=paginas;_tamanhoPagina=tamanhoPagina;_relogio=relogio??TimeProvider.System;
    }
    public bool PaginaOficial(Uri? uri)=>uri is {IsAbsoluteUri:true,Scheme:"https",Host:"github.com",IsDefaultPort:true} && string.IsNullOrEmpty(uri.UserInfo) &&
        string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(uri.Fragment) && uri.AbsolutePath.StartsWith("/"+_repositorio+"/releases/tag/",StringComparison.Ordinal) &&
        !uri.AbsolutePath.Contains("%",StringComparison.Ordinal) && uri.Segments.Length==6;

    public async Task<ResultadoAtualizacao> Consultar(string versaoAtual,string rid,CancellationToken cancelamento=default)
    {
        if(!VersaoAplicativo.TentarLer(versaoAtual,out var atual) || rid is not ("win-x64" or "linux-x64"))return new(EstadoAtualizacao.Inconclusivo,"Versão ou plataforma não reconhecida.");
        if(!await _trava.WaitAsync(0,cancelamento))return new(EstadoAtualizacao.Inconclusivo,"Já existe uma consulta em andamento.");
        using var prazo=CancellationTokenSource.CreateLinkedTokenSource(cancelamento);prazo.CancelAfter(TimeSpan.FromSeconds(15));
        try
        {
            if(_relogio.GetUtcNow()<_proximaConsulta)return new(EstadoAtualizacao.Indisponivel,"Aguarde antes de consultar novamente: limite temporário do GitHub.");
            NuGetVersion? maior=null;Uri? pagina=null;bool pacote=false;bool completa=false;
            for(var i=1;i<=_paginas;i++)
            {
                using var pedido=new HttpRequestMessage(HttpMethod.Get,$"https://api.github.com/repos/{_repositorio}/releases?per_page={_tamanhoPagina}&page={i}");
                pedido.Headers.UserAgent.ParseAdd("MinhasRealizacoes/"+versaoAtual);
                pedido.Headers.Accept.ParseAdd("application/vnd.github+json");pedido.Headers.Add("X-GitHub-Api-Version","2026-03-10");
                using var resposta=await _cliente.SendAsync(pedido,HttpCompletionOption.ResponseHeadersRead,prazo.Token);
                if(resposta.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.TooManyRequests)
                {
                    var espera=resposta.Headers.RetryAfter?.Delta ?? (resposta.Headers.RetryAfter?.Date-_relogio.GetUtcNow()) ?? TimeSpan.FromMinutes(1);
                    _proximaConsulta=_relogio.GetUtcNow()+ (espera>TimeSpan.Zero?espera:TimeSpan.FromMinutes(1));
                    return new(EstadoAtualizacao.Indisponivel,"Limite temporário do GitHub. Tente mais tarde.");
                }
                if(!resposta.IsSuccessStatusCode)return new(EstadoAtualizacao.Indisponivel,"Não foi possível consultar as releases públicas. O repositório pode estar privado ou indisponível.");
                if(resposta.Content.Headers.ContentLength>1024*1024)return new(EstadoAtualizacao.Inconclusivo,"Resposta de versões excedeu o limite.");
                using var stream=await resposta.Content.ReadAsStreamAsync(prazo.Token);using var memoria=new MemoryStream();
                var buffer=new byte[8192];int lidos;
                while((lidos=await stream.ReadAsync(buffer,prazo.Token))>0)
                {if(memoria.Length+lidos>1024*1024)return new(EstadoAtualizacao.Inconclusivo,"Resposta de versões excedeu o limite.");await memoria.WriteAsync(buffer.AsMemory(0,lidos),prazo.Token);}
                using var json=JsonDocument.Parse(memoria.ToArray(),new JsonDocumentOptions{MaxDepth=32});
                if(json.RootElement.ValueKind!=JsonValueKind.Array)throw new JsonException();
                foreach(var release in json.RootElement.EnumerateArray())
                {
                    if(release.GetProperty("draft").GetBoolean())continue;
                    if(!VersaoAplicativo.TentarLer(release.GetProperty("tag_name").GetString(),out var v))continue;
                    if(!atual!.IsPrerelease && v!.IsPrerelease)continue;
                    if(VersionComparer.VersionRelease.Compare(v,atual)<=0 || maior!=null && VersionComparer.VersionRelease.Compare(v,maior)<0)continue;
                    if(!Uri.TryCreate(release.GetProperty("html_url").GetString(),UriKind.Absolute,out var uri) || !PaginaOficial(uri))throw new JsonException();
                    var assets=release.GetProperty("assets");if(assets.ValueKind!=JsonValueKind.Array)throw new JsonException();
                    var prefixo=$"MinhasRealizacoes-{v!.ToNormalizedString()}-{rid}";
                    var temPacote=assets.EnumerateArray().Any(a=>a.GetProperty("state").GetString()=="uploaded" &&
                        a.GetProperty("name").GetString() is {} nome && (nome==prefixo+(rid=="win-x64"?".zip":".tar.gz") || rid=="win-x64" && nome==prefixo+"-setup.exe"));
                    if(maior==null || VersionComparer.VersionRelease.Compare(v,maior)>0){maior=v;pagina=uri;pacote=temPacote;}
                    else pacote|=temPacote;
                }
                if(json.RootElement.GetArrayLength()<_tamanhoPagina){completa=true;break;}
            }
            if(!completa)return new(EstadoAtualizacao.Inconclusivo,"A consulta de versões ficou incompleta. Tente novamente mais tarde.");
            if(maior==null)return new(EstadoAtualizacao.Atualizado,"Nenhuma versão mais recente foi encontrada nas releases consultadas.");
            return pacote?new(EstadoAtualizacao.Disponivel,"Nova versão disponível. Faça backup e instale a atualização manualmente.",maior.ToNormalizedString(),pagina):
                new(EstadoAtualizacao.SemPacote,"Existe uma versão mais recente, mas sem pacote compatível com este sistema.",maior.ToNormalizedString());
        }
        catch(OperationCanceledException) when(!cancelamento.IsCancellationRequested){return new(EstadoAtualizacao.Indisponivel,"A consulta demorou demais. Tente novamente.");}
        catch(Exception e) when(e is HttpRequestException or IOException or JsonException or InvalidOperationException or KeyNotFoundException or FormatException)
        {return new(EstadoAtualizacao.Inconclusivo,"Não foi possível concluir a consulta. Sua biblioteca continua disponível.");}
        finally{_trava.Release();}
    }
}

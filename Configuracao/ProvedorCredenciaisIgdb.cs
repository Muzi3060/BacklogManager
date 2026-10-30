using System;
using System.Reflection;
using System.Text.Json;
using BacklogManager.Services;
namespace BacklogManager.Configuracao;

internal sealed record CredenciaisIgdb(string ClientId,string AccessToken,bool DaBeta);
internal static class ProvedorCredenciaisIgdb
{
    internal static CredenciaisIgdb? Obter(Func<string,string?>? ambiente=null,Func<CredenciaisIgdb?>? beta=null)
    {
        ambiente??=Environment.GetEnvironmentVariable;
        var explicito=!string.IsNullOrWhiteSpace(ambiente("IGDB_CLIENT_ID")) ||
            !string.IsNullOrWhiteSpace(ambiente("IGDB_ACCESS_TOKEN")) || !string.IsNullOrWhiteSpace(ambiente("IGDB_ACESS_TOKEN"));
        if(explicito) return ConfiguracaoIgdb.TentarLer(out var id,out var token,ambiente)?new(id,token,false):null;
        var incorporadas=(beta??LerBeta)();
        return incorporadas!=null && ConfiguracaoIgdb.CredencialValida(incorporadas.ClientId) &&
            ConfiguracaoIgdb.CredencialValida(incorporadas.AccessToken)?incorporadas:null;
    }
    private static CredenciaisIgdb? LerBeta()
    {
        using var recurso=typeof(ProvedorCredenciaisIgdb).Assembly.GetManifestResourceStream("MinhasRealizacoes.Beta.Igdb");
        if(recurso==null)return null;
        try{return JsonSerializer.Deserialize<CredenciaisIgdb>(recurso);}
        catch(JsonException){return null;}
    }
}

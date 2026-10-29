using Xunit;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using BacklogManager.Aparencia;
using BacklogManager.Controls;
using BacklogManager.Data;
using BacklogManager.Models;
using System.Text.Json.Nodes;
namespace BacklogManager.Tests.Fase5;

public sealed class PreviaInterfaceTests
{
    [AvaloniaFact(DisplayName = "VP01 | Prévia Avalonia Moderno Claro | Janela controles e captura refletem a combinação em tempo real")]
    public void Cenario01() { using var t=new TelaTeste(new[]{PastaTeste.Jogo()});ApoioAparencia.Tema(t,TemaVisual.Moderno,ModoVisual.Claro);var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos;Assert.Equal(r["PageBackground"],ApoioAparencia.Cor(t.Janela.Background));Assert.Equal(r["ControlBackground"],ApoioAparencia.Cor(t.Campo<TextBox>("TxtTitulo").Background));Assert.Equal(ThemeVariant.Light,t.Janela.ActualThemeVariant);t.Clicar("NavInicio");ApoioAparencia.Capturar(t,"Moderno-Claro-inicio");t.Clicar("NavBacklog");ApoioAparencia.Capturar(t,"Moderno-Claro-biblioteca");t.Clicar("NavConfiguracoes");ApoioAparencia.Capturar(t,"Moderno-Claro-aparencia"); }
    [AvaloniaFact(DisplayName = "VP02 | Prévia Avalonia Moderno Escuro | Janela controles e captura refletem a combinação em tempo real")]
    public void Cenario02() { using var t=new TelaTeste(new[]{PastaTeste.Jogo()});ApoioAparencia.Tema(t,TemaVisual.Moderno,ModoVisual.Escuro);var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos;Assert.Equal(r["PageBackground"],ApoioAparencia.Cor(t.Janela.Background));Assert.Equal(r["ControlBackground"],ApoioAparencia.Cor(t.Campo<TextBox>("TxtTitulo").Background));Assert.Equal(ThemeVariant.Dark,t.Janela.ActualThemeVariant);t.Clicar("NavInicio");ApoioAparencia.Capturar(t,"Moderno-Escuro-inicio");t.Clicar("NavBacklog");ApoioAparencia.Capturar(t,"Moderno-Escuro-biblioteca");t.Clicar("NavConfiguracoes");ApoioAparencia.Capturar(t,"Moderno-Escuro-aparencia"); }
    [AvaloniaFact(DisplayName = "VP03 | Prévia Avalonia Tecnologico Claro | Janela controles e captura refletem a combinação em tempo real")]
    public void Cenario03() { using var t=new TelaTeste(new[]{PastaTeste.Jogo()});ApoioAparencia.Tema(t,TemaVisual.Tecnologico,ModoVisual.Claro);var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos;Assert.Equal(r["PageBackground"],ApoioAparencia.Cor(t.Janela.Background));Assert.Equal(r["ControlBackground"],ApoioAparencia.Cor(t.Campo<TextBox>("TxtTitulo").Background));Assert.Equal(ThemeVariant.Light,t.Janela.ActualThemeVariant);t.Clicar("NavInicio");ApoioAparencia.Capturar(t,"Tecnologico-Claro-inicio");t.Clicar("NavBacklog");ApoioAparencia.Capturar(t,"Tecnologico-Claro-biblioteca");t.Clicar("NavConfiguracoes");ApoioAparencia.Capturar(t,"Tecnologico-Claro-aparencia"); }
    [AvaloniaFact(DisplayName = "VP04 | Prévia Avalonia Tecnologico Escuro | Janela controles e captura refletem a combinação em tempo real")]
    public void Cenario04() { using var t=new TelaTeste(new[]{PastaTeste.Jogo()});ApoioAparencia.Tema(t,TemaVisual.Tecnologico,ModoVisual.Escuro);var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos;Assert.Equal(r["PageBackground"],ApoioAparencia.Cor(t.Janela.Background));Assert.Equal(r["ControlBackground"],ApoioAparencia.Cor(t.Campo<TextBox>("TxtTitulo").Background));Assert.Equal(ThemeVariant.Dark,t.Janela.ActualThemeVariant);t.Clicar("NavInicio");ApoioAparencia.Capturar(t,"Tecnologico-Escuro-inicio");t.Clicar("NavBacklog");ApoioAparencia.Capturar(t,"Tecnologico-Escuro-biblioteca");t.Clicar("NavConfiguracoes");ApoioAparencia.Capturar(t,"Tecnologico-Escuro-aparencia"); }
    [AvaloniaFact(DisplayName = "VP05 | Prévia Avalonia Acolhedor Claro | Janela controles e captura refletem a combinação em tempo real")]
    public void Cenario05() { using var t=new TelaTeste(new[]{PastaTeste.Jogo()});ApoioAparencia.Tema(t,TemaVisual.Acolhedor,ModoVisual.Claro);var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos;Assert.Equal(r["PageBackground"],ApoioAparencia.Cor(t.Janela.Background));Assert.Equal(r["ControlBackground"],ApoioAparencia.Cor(t.Campo<TextBox>("TxtTitulo").Background));Assert.Equal(ThemeVariant.Light,t.Janela.ActualThemeVariant);t.Clicar("NavInicio");ApoioAparencia.Capturar(t,"Acolhedor-Claro-inicio");t.Clicar("NavBacklog");ApoioAparencia.Capturar(t,"Acolhedor-Claro-biblioteca");t.Clicar("NavConfiguracoes");ApoioAparencia.Capturar(t,"Acolhedor-Claro-aparencia"); }
    [AvaloniaFact(DisplayName = "VP06 | Prévia Avalonia Acolhedor Escuro | Janela controles e captura refletem a combinação em tempo real")]
    public void Cenario06() { using var t=new TelaTeste(new[]{PastaTeste.Jogo()});ApoioAparencia.Tema(t,TemaVisual.Acolhedor,ModoVisual.Escuro);var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos;Assert.Equal(r["PageBackground"],ApoioAparencia.Cor(t.Janela.Background));Assert.Equal(r["ControlBackground"],ApoioAparencia.Cor(t.Campo<TextBox>("TxtTitulo").Background));Assert.Equal(ThemeVariant.Dark,t.Janela.ActualThemeVariant);t.Clicar("NavInicio");ApoioAparencia.Capturar(t,"Acolhedor-Escuro-inicio");t.Clicar("NavBacklog");ApoioAparencia.Capturar(t,"Acolhedor-Escuro-biblioteca");t.Clicar("NavConfiguracoes");ApoioAparencia.Capturar(t,"Acolhedor-Escuro-aparencia"); }





    [AvaloniaFact(DisplayName = "VP12 | Prévia com formulário preenchido | Título review e navegação não são perdidos")]
    public void Cenario12() { using var t=new TelaTeste(new[]{PastaTeste.Jogo()});t.Editar();t.Campo<TextBox>("TxtReview").Text="Rascunho pessoal";ApoioAparencia.Tema(t,TemaVisual.Acolhedor,ModoVisual.Escuro);t.Clicar("NavCadastro");Assert.Equal("Rascunho pessoal",t.Campo<TextBox>("TxtReview").Text);Assert.Equal("Jogo simulado",t.Campo<TextBox>("TxtTitulo").Text); }

    [AvaloniaFact(DisplayName = "VP14 | Janela já aberta atualiza recursos compartilhados | Troca em segunda janela chega à primeira janela")]
    public void Cenario14() { var a=new AparenciaSimulada();using var t=new TelaTeste(aparencia:a.Criar());using var b=new TelaTeste(aparencia:a.Criar());ApoioAparencia.Tema(b,TemaVisual.Tecnologico,ModoVisual.Escuro);Assert.Equal(ApoioAparencia.Cor(b.Janela.Background),ApoioAparencia.Cor(t.Janela.Background));Assert.Equal(ThemeVariant.Dark,t.Janela.ActualThemeVariant); }
    [AvaloniaFact(DisplayName = "VP15 | Falha ao salvar pela interface | Prévia funciona e mensagem não afirma sucesso")]
    public void Cenario15() { var a=new AparenciaSimulada{FalhaSalvar=new IOException()};using var t=new TelaTeste(aparencia:a.Criar());ApoioAparencia.Tema(t,TemaVisual.Acolhedor,ModoVisual.Escuro);Assert.Contains("não pôde ser salva",t.Campo<TextBlock>("TxtEstadoAparencia").Text!);Assert.True(t.Campo<Button>("BtnRepetirSalvarAparencia").IsVisible); }
    [AvaloniaFact(DisplayName = "VP16 | Retry de gravação pela interface | Grava preferências atuais e encerra estado pendente")]
    public void Cenario16() { var a=new AparenciaSimulada{FalhaSalvar=new IOException()};using var t=new TelaTeste(aparencia:a.Criar());ApoioAparencia.Tema(t,TemaVisual.Tecnologico,ModoVisual.Escuro);a.FalhaSalvar=null;t.Clicar("BtnRepetirSalvarAparencia");Assert.False(t.Campo<Button>("BtnRepetirSalvarAparencia").IsVisible);Assert.Equal(TemaVisual.Tecnologico,a.Preferencias.Tema);Assert.Equal(ModoVisual.Escuro,a.Preferencias.Modo); }




    [AvaloniaFact(DisplayName = "VP21 | Nome público e recursos técnicos | Interface usa nome aprovado sem slogan ou troca de namespace")]
    public void Cenario21() { using var t=new TelaTeste();Assert.Equal("Minhas Realizações",t.Janela.Title);Assert.Contains(t.Janela.GetVisualDescendants().OfType<TextBlock>(),x=>x.Text=="Minhas Realizações");Assert.DoesNotContain(t.Janela.GetVisualDescendants().OfType<TextBlock>(),x=>x.Text is "Backlog" or "MANAGER" or "Sua coleção, no seu ritmo");Assert.Equal("BacklogManager",typeof(MainWindow).Namespace); }


}

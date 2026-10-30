using BacklogManager.Backup;
using BacklogManager.Services;
// Modo da beta: todos os caminhos precisam pertencer exclusivamente às fixtures temporárias.
if(args.Length==5 && args[0]=="--migracao")
{
    var raiz=Path.Combine(Path.GetTempPath(),"MR.Beta.Tests")+Path.DirectorySeparatorChar;
    if(args.Skip(1).Take(3).Any(p=>!Path.GetFullPath(p).StartsWith(raiz,StringComparison.Ordinal)))return 2;
    var migracao=new ServicoMigracaoLocal(new CaminhosDadosLocais(Path.Combine(args[2],"backlog.json")),args[3]);
    var previaMigracao=migracao.Prever(args[1]);
    migracao.Etapa=etapa=>{if(etapa==args[4])Environment.Exit(73);};
    migracao.Migrar(previaMigracao,true,true);
    return 0;
}
// Executável de teste: somente aceita a raiz temporária criada pela fixture da Fase 7.
if (args.Length != 4 || !Path.GetFullPath(args[0]).StartsWith(Path.Combine(Path.GetTempPath(), "MinhasRealizacoes.Fase7") + Path.DirectorySeparatorChar))
    return 2;
var servico = new ServicoBackupLocal(new CaminhosDadosLocais(args[0]), args[1]);
var previa = servico.Prever(args[2]);
servico.Etapa = etapa => { if(etapa == args[3]) Environment.Exit(73); };
servico.Restaurar(previa,true,true);
return 0;

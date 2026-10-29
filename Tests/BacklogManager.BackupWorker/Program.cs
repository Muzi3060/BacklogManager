using BacklogManager.Backup;
// Executável de teste: somente aceita a raiz temporária criada pela fixture da Fase 7.
if (args.Length != 4 || !Path.GetFullPath(args[0]).StartsWith(Path.Combine(Path.GetTempPath(), "MinhasRealizacoes.Fase7") + Path.DirectorySeparatorChar))
    return 2;
var servico = new ServicoBackupLocal(new CaminhosDadosLocais(args[0]), args[1]);
var previa = servico.Prever(args[2]);
servico.Etapa = etapa => { if(etapa == args[3]) Environment.Exit(73); };
servico.Restaurar(previa,true,true);
return 0;

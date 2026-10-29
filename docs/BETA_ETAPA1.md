# Beta — Etapa 1: simplificação da interface

Implementação para revisão em `feat/beta-interface`, criada diretamente da `main` aprovada (`1771688`). A branch `feat/promocoes-ui` (`0aa420c`) e todas as branches anteriores foram preservadas. A Fase 8 não foi integrada. O escopo desta entrega é exclusivamente a simplificação autorizada; a validação manual e a aprovação desta etapa ainda estão pendentes.

## Resultado

- Configurações foi movida para o rodapé da barra lateral, substituindo o indicador de armazenamento local. Início, biblioteca, coleções e adicionar/editar permanecem na navegação principal.
- Aparência deixou de ser uma página independente. Tema e modo aparecem diretamente em Configurações e são aplicados/salvos automaticamente.
- São quatro temas fixos: Moderno, Tecnológico, Acolhedor e Dark Mode. Todos possuem Claro/Escuro, totalizando oito combinações. Moderno claro continua sendo o padrão sem preferência salva. Dark Mode usa superfícies e destaques neutros; mensagens de sucesso, aviso e erro conservam sua semântica.
- Editor, paleta, campos hexadecimais e restauração de cores foram removidos, inclusive os métodos de personalização. A biblioteca continua com seu seletor de grade/lista; o seletor duplicado das configurações foi retirado.
- O Dashboard preserva totais, distribuição por mídia/status, proporção de obras concluídas, atividade de 7/30 dias e acompanhamento das obras em andamento com acesso à edição. Foram retirados o convite “Qual será sua próxima obra?”, a reserva de promoções e toda a seção “Sem data de adição registrada”, além das explicações redundantes.
- Obras sem data permanecem na biblioteca e nos totais. Podem aparecer no acompanhamento quando estão em andamento, sem ganhar datas inventadas. A atividade usa exclusivamente datas reais. Nenhuma obra foi removida por não ter data.
- Promoções não aparecem na interface desta branch. Página, navegação e componente exclusivos da Fase 8 permanecem somente no histórico/branch anterior; na base aprovada havia apenas a reserva do Dashboard, agora removida.
- Exportação, prévia, validação, confirmação, restauração seletiva de preferências, backup preventivo, diário, rollback e recuperação da Fase 7 foram mantidos.

## Decisões e explicação em C#

`TemaVisual` ganhou `DarkMode` no final do enum: os três valores anteriores mantêm seus números. `TemasPredefinidos` continua sendo o catálogo central de paletas. `ResolvedorAparencia` deriva cores de contraste e estados a partir dessas paletas; `AplicadorAparencia` continua atualizando os recursos dinâmicos do Avalonia. As telas não receberam uma segunda implementação de temas.

`PreferenciasAparencia` agora representa apenas tema e modo. Em termos práticos, a janela informa a escolha ao serviço, o serviço salva e o adaptador visual atualiza os recursos consumidos pelos controles. Não é necessário recriar a janela ou reiniciar o aplicativo.

`ArmazenamentoAparenciaJson` continua guardando uma cópia do documento JSON original. Ele lê tema/modo de arquivos anteriores, mas não aplica `Personalizacoes`. Essa propriedade e extensões desconhecidas permanecem como conteúdo opaco: não aparecem no modelo ativo, não mudam as cores e não são apagadas ao selecionar um tema. A cópia com `DeepClone` permite atualizar apenas os campos conhecidos, preservando os demais.

Uma escolha explícita salva a preferência de aparência como **versão 2**, com o mesmo nome/caminho de arquivo. Isso permite reconhecer Dark Mode e faz a versão antiga do aplicativo bloquear sua gravação, em vez de sobrescrever uma preferência que não compreende. Arquivos versão 1 continuam legíveis; a leitura não realiza migração nem gravação automática. Versões futuras acima de 2 mantêm aviso e bloqueio de escrita. JSON inválido mantém a proteção de backup `.bak` antes de uma substituição explícita.

A persistência de biblioteca, coleções e grade/lista e o envelope de backup não mudaram de formato. O backup pode transportar preferências versão 1 ou 2. Cores antigas permanecem inativas também após restaurar um backup antigo. Preferências ausentes em um backup não são apagadas por inferência.

O método `ServicoAparencia.Carregar()` passou a limpar `PendenteSalvar` quando há uma recarga explícita. Um teste reproduziu o erro em que uma restauração bem-sucedida ainda deixava o botão de tentar salvar uma escolha anterior. Restaurar somente dados continua sem recarregar a aparência; QI18/QI19 verificam a preservação das escolhas pendentes nesses casos.

A simplificação do Dashboard ficou na apresentação. Sua projeção de leitura e os cálculos de datas não foram reescritos. Não houve novas APIs, regras de catálogo, serviços HTTP ou mudanças nos identificadores legados.

## Preservação e Git

Antes de alterar a persistência de aparência, foi criada e verificada uma cópia externa de `backlog.json` e do `.bak` existente em `~/Backups/BacklogManager/beta-interface-20261029-192701`. O manifesto privado contém hashes SHA-256; as duas cópias foram conferidas contra os originais. Os caminhos de armazenamento usados pelo aplicativo não foram alterados.

A comparação final confirmou os bytes dos dados pessoais presentes e a ausência dos arquivos que não existiam antes, os 19 arquivos de backups anteriores, o contrato, os arquivos pessoais não rastreados e os relatórios/documentos existentes na `main`. As 11 branches anteriores continuaram nos mesmos commits. Evidência portátil: [verificacoes-beta-etapa1.json](verificacoes-beta-etapa1.json). O manifesto completo e os dados do backup ficam fora do Git.

Antes de qualquer commit, `/FASE_BETA_INSTRUCOES.md` foi incluído em `.git/info/exclude`, somente no clone local. O arquivo foi preservado, permanece fora do índice e não integra os commits. Essa proteção **não acompanha novos clones**: mantenha esse documento privado e preserve a exclusão local. `qodana.yaml` e `skills/` continuam não rastreados e intactos.

Commit de implementação e testes: `f536428` — `feat(beta): simplifica interface e mantém temas fixos compatíveis`. A documentação inicial foi registrada em `11e528d`. A revisão final dos enunciados de três testes e a nova execução completa são registradas em um terceiro commit na mesma branch.

Não houve merge, push ou publicação. Distribuição, adaptação das credenciais IGDB, verificação de atualizações e a auditoria completa para publicação permanecem fora desta etapa.

## Build e testes

A solução compilou com **zero avisos e zero erros**. A referência anterior passou em **1551/1551** casos. A suíte final passou em **1541/1541**, sem falhas ou testes ignorados: **1446 regressões mantidas/adaptadas e 95 cenários novos**.

Os 105 cenários retirados exigiam recursos expressamente removidos. Estão individualmente mapeados, com motivo e cobertura substituta; não foram contados como aprovados na beta. Os relatórios das fases anteriores foram preservados.

- [Tabelas dos 95 novos cenários e histórico das correções](BETA_ETAPA1_TESTES.md).
- [Resultados individuais da suíte e hashes dos TRX](resultados-beta-etapa1.json).
- [Transição individual dos testes retirados](beta-etapa1-transicao-testes.json).

Comandos reais utilizados, com saídas em uma pasta temporária isolada:

```bash
dotnet test --logger 'trx;LogFileName=beta1-base.trx' --results-directory /tmp/minhas-realizacoes-beta1-resultados
# Depois das alterações e das correções:
dotnet build --no-restore
dotnet test --no-build --no-restore --logger 'trx;LogFileName=beta1-final.trx' --results-directory /tmp/minhas-realizacoes-beta1-resultados
# Reexecução após revisar os enunciados dos testes:
dotnet test --no-restore --logger 'trx;LogFileName=beta1-final-revisado.trx' --results-directory /tmp/minhas-realizacoes-beta1-resultados
python3 scripts/gerar_relatorio_beta_etapa1.py /tmp/minhas-realizacoes-beta1-resultados/beta1-base.trx /tmp/minhas-realizacoes-beta1-resultados/beta1-final-revisado.trx
```

O TRX de referência corresponde ao código anterior às alterações. Para reproduzi-lo futuramente, use uma cópia separada do commit base, preservando o diretório de trabalho atual. O gerador não executa testes: lê os TRX, confere IDs/cobertura/transições e produz documentação portátil.

As 40 capturas automatizadas cobrem as cinco telas nas oito combinações, em 900 × 660. Foram inspecionadas capturas de Configurações Dark Mode escuro, Dashboard Moderno claro e biblioteca Acolhedor escuro. Isso complementa as asserções e não constitui validação manual no Rider.

## Até cinco cenários para validação manual no Rider

Prepare uma **nova pasta de dados fictícios**, executando `python3 scripts/preparar_validacao_fase7.py`. Reutilizamos esse gerador existente, que não lê a biblioteca pessoal. Na configuração de execução do Rider, use como **Working directory** a pasta `dados` informada pelo script. Confira os caminhos na página Configurações antes de exportar/restaurar. Não aponte o aplicativo para a biblioteca original.

| ID | Procedimento consolidado | Resultado esperado | Situação |
|---|---|---|---|
| MB01 | Em 900 × 660 e em janela maior, navegar entre Início, biblioteca, coleções e adicionar/editar. Acessar Configurações no rodapé e testar Tab/teclado. | Links principais preservados, Configurações acessível no rodapé, sem link Aparência ou indicador antigo. | Não executado — aguardando proprietário |
| MB02 | Em Configurações, experimentar os quatro temas nos dois modos, alternando pelas telas; fechar e reabrir com o mesmo Working directory. | Oito combinações legíveis, mudança imediata, escolha persistida, sem editor de cores ou seletor redundante de grade/lista. | Não executado — aguardando proprietário |
| MB03 | Conferir o Dashboard com os dados fictícios datados e o jogo legado sem data; alternar 7/30 dias e abrir uma obra em andamento para edição. | Estatísticas corretas, sem blocos/explicações removidos, sem promoções. Registro sem data preservado na biblioteca. | Não executado — aguardando proprietário |
| MB04 | Na biblioteca, alternar grade/lista, filtros e mídia; editar review/progresso fictícios, conferir coleções e reiniciar. | Capas/cartões/filtros, dados pessoais e vínculos continuam funcionando e persistindo. | Não executado — aguardando proprietário |
| MB05 | Exportar backup fictício com preferências; alterar tema e uma review de teste; selecionar o backup, conferir a prévia e confirmar primeiro sem preferências, depois com elas. | Prévia/consentimento obrigatórios, backup preventivo verificado, restauração de dados preserva tema quando desmarcado e reaplica o tema do backup quando marcado; nenhum aviso de gravação antiga pendente. | Não executado — aguardando proprietário |

## Limitações e pendências

A substituição de biblioteca e coleções continua coordenada por diário e recuperação, **sem transação atômica conjunta entre arquivos**. Incluindo preferências, podem ser quatro arquivos. Não há promessa de proteção absoluta contra falhas físicas. Essa limitação permanece registrada para as futuras etapas de persistência/sincronização.

Ao abrir uma aparência versão 2 na versão anterior do aplicativo, a gravação fica bloqueada por proteção; não se promete compatibilidade reversa de edição. As cores personalizadas antigas ficam preservadas no documento, mas não são editáveis nem aplicadas na beta.

A validação automatizada utiliza Avalonia Headless/Skia e HTTP simulado. Não valida executáveis distribuídos Windows/Linux, disponibilidade real da IGDB ou todos os leitores de tela. Capas remotas continuam dependendo de acesso/cache conforme a implementação existente. Os cinco cenários manuais acima ainda estão pendentes; a aprovação definitiva desta etapa pertence ao proprietário.

A auditoria completa de código/histórico/documentos para tornar o repositório público e as etapas de distribuição/IGDB/atualizações não foram executadas nem aprovadas por esta entrega.

## Arquivos desta entrega


### Aplicativo

- `Aparencia/ArmazenamentoAparenciaJson.cs`
- `Aparencia/CoresAparencia.cs`
- `Aparencia/PreferenciasAparencia.cs`
- `Aparencia/ResolvedorAparencia.cs`
- `Aparencia/ServicoAparencia.cs`
- `Controls/EditorCor.cs` — removido
- `MainWindow.Aparencia.cs`
- `MainWindow.Biblioteca.cs`
- `MainWindow.Configuracoes.cs`
- `MainWindow.Dashboard.cs`
- `MainWindow.axaml`
- `MainWindow.axaml.cs`

### Testes novos e adaptados; arquivos removidos constam no diff

- `Tests/BacklogManager.Tests/AjustesFase3/TextosTests.cs`
- `Tests/BacklogManager.Tests/BetaInterface/ApoioBeta.cs`
- `Tests/BacklogManager.Tests/BetaInterface/CompatibilidadeBetaTests.cs`
- `Tests/BacklogManager.Tests/BetaInterface/ConfiguracoesBetaTests.cs`
- `Tests/BacklogManager.Tests/BetaInterface/DashboardBetaTests.cs`
- `Tests/BacklogManager.Tests/BetaInterface/NavegacaoBetaTests.cs`
- `Tests/BacklogManager.Tests/BetaInterface/SemPromocoesBetaTests.cs`
- `Tests/BacklogManager.Tests/BetaInterface/TemasFixosBetaTests.cs`
- `Tests/BacklogManager.Tests/Fase5/ApoioAparencia.cs`
- `Tests/BacklogManager.Tests/Fase5/CompatibilidadeVisualTests.cs` — removido
- `Tests/BacklogManager.Tests/Fase5/ContrasteTests.cs`
- `Tests/BacklogManager.Tests/Fase5/EstadosVisuaisTests.cs`
- `Tests/BacklogManager.Tests/Fase5/HexadecimalTests.cs`
- `Tests/BacklogManager.Tests/Fase5/PersistenciaAparenciaTests.cs`
- `Tests/BacklogManager.Tests/Fase5/PersonalizacaoTests.cs` — removido
- `Tests/BacklogManager.Tests/Fase5/PreviaInterfaceTests.cs`
- `Tests/BacklogManager.Tests/Fase5/RestauracaoAparenciaTests.cs` — removido
- `Tests/BacklogManager.Tests/Fase6/CapasDashboardTests.cs`
- `Tests/BacklogManager.Tests/Fase6/InterfaceDashboardTests.cs`
- `Tests/BacklogManager.Tests/Fase7/ApoioBackup.cs`
- `Tests/BacklogManager.Tests/Fase7/ConfiguracoesTests.cs`
- `Tests/BacklogManager.Tests/Fase7/ContrasteBackupTests.cs`
- `Tests/BacklogManager.Tests/Fase7/IntegracaoBackupTests.cs`
- `Tests/BacklogManager.Tests/Fase7/PreferenciasBackupTests.cs`
- `Tests/BacklogManager.Tests/Fase7/PreferenciasNaSessaoTests.cs`
- `Tests/BacklogManager.Tests/Revisao3/ApoioVisual.cs`
- `Tests/BacklogManager.Tests/Revisao3/ArredondamentoTests.cs`
- `Tests/BacklogManager.Tests/Revisao3/RecentesCapasTests.cs` — removido

### Documentação e geração do relatório

- `docs/BETA_ETAPA1.md`
- `docs/BETA_ETAPA1_TESTES.md`
- `docs/beta-etapa1-transicao-testes.json`
- `docs/resultados-beta-etapa1.json`
- `docs/verificacoes-beta-etapa1.json`
- `scripts/gerar_relatorio_beta_etapa1.py`

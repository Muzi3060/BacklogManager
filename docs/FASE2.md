# Fase 2 — Multimídia

## Situação para revisão

A Fase 1 foi aprovada oficialmente pelo proprietário. O relato geral de verificações no Rider, sem discriminação de cenários, foi registrado em [APROVACAO_FASE1.md](APROVACAO_FASE1.md), no commit `56bcd58`. Esse registro não aprova individualmente MF01–MF10 nem altera os resultados automatizados anteriores.

A integração de `refactor/arquitetura` à `main` ocorreu por fast-forward, de `10aab6a` para `56bcd58`, sem conflitos. A árvore integrada foi comparada à branch aprovada. Nenhuma branch foi excluída e não houve push.

A Fase 2 foi desenvolvida em **`feat/multimidia`**, criada dessa `main`. A implementação segue `Contrato.md` versão 1.1 e fica disponível para revisão. **Não houve merge da Fase 2 nem início da Fase 3.** O contrato não foi alterado.

## Implementação

- Seis categorias na biblioteca e no cadastro: jogos, animes, séries, filmes, mangás e livros. Cada categoria da biblioteca mostra apenas os registros daquele tipo, combinando a busca/status existentes.
- Jogos continuam com pesquisa IGDB, seleção por ID, credenciais locais, cancelamento, tratamento de falhas e persistência existentes.
- Cinco catálogos locais simulados, com três títulos fictícios cada, IDs próprios, gêneros e referências de capas SVG locais. Nenhum deles usa HTTP ou funciona como fallback da IGDB.
- Identificação explícita dos resultados simulados no título, no aviso do formulário, nas sugestões, na biblioteca e no arquivo salvo.
- Cadastro, edição e exclusão das novas mídias, com persistência e recarga. A edição preserva o tipo e o catálogo do registro.
- Progresso pessoal opcional: horas jogadas; episódios assistidos; temporada/episódio; minutos assistidos; capítulo/volume; página/capítulo.
- Validação de campos compatíveis com cada mídia, valores não negativos, frações apenas nas unidades decimais e distinção entre zero e ausência. Não há inferência de duração por APIs ou conclusão automática.
- Rótulos de status adequados a leitura e reprodução, mantendo os estados internos existentes e os textos legados desconhecidos.
- Extensão opcional e versionada do JSON para mídia, referência de catálogo, gêneros, capa, simulação e progresso, mantendo registros antigos sem migração automática.
- Cancelamento e limpeza da seleção ao trocar o tipo do cadastro. O tipo é conferido novamente antes de usar um resultado de busca.

A apresentação continua em lista. As referências de capas estão disponíveis e os recursos simulados estão empacotados, mas **a grade, a alternância de visualizações e a exibição de capas nas sugestões/biblioteca permanecem na Fase 3**. Não foram implementados novos catálogos HTTP, nuvem, preferências persistidas, exportação/importação de backup, coleções ou demais fases.

## Decisões arquiteturais

Os contratos e o serviço da Fase 1 foram mantidos. `RoteadorPesquisaCatalogo` escolhe o serviço por tipo; `CatalogoSimulado` satisfaz o mesmo `IPesquisaCatalogo` usado pela IGDB, permitindo substituir o provedor em uma fase futura autorizada.

`FormularioObra` prepara os dados de qualquer mídia. `FormularioJogo` permanece como entrada compatível para o fluxo específico de jogos, delegando a preparação comum. `ProgressoPessoal` fica nos dados pessoais; `RegrasProgresso` valida suas unidades no formulário, no serviço e na persistência.

`MainWindow.Multimidia.cs` separa os eventos e controles de tipo/progresso da janela existente. O layout segue Avalonia; nenhum framework ou pacote adicional foi introduzido.

Os campos antigos do JSON permanecem na mesma lista. `DadosMultimidiaJson`, com `Versao = 1`, é opcional por registro. Novos tipos, capas, gêneros ou progresso usam esse bloco; um jogo legado sem informação nova continua sem o bloco. `ConversorBibliotecaJson` concentra a tradução e `FileManager` preserva temporário, hash, bloqueio e `.bak`.

A [documentação da arquitetura da Fase 2](ARQUITETURA_FASE2.md) detalha o formato, a validação e os limites. Os documentos anteriores permanecem históricos. O proprietário informou que estudará os conceitos da Fase 1 posteriormente com o ChatGPT; nenhuma sessão teórica foi registrada como realizada.

## Backup e preservação da biblioteca

Antes de alterar modelos/persistência, foi criado e conferido byte a byte:

`${HOME}/Backups/BacklogManager/fase2-20261027-213204/backlog.json`

| Dado no início desta fase | Valor |
|---|---|
| Registros | 9 |
| Tamanho | 1535 bytes |
| SHA-256 | `d33b826a57148dc8c7017ae22e15f6898163ba7d5d5eea57a7ba64b6ddaac810` |

A pasta tem permissão 700 e o arquivo 600, com manifesto de verificação ao lado. Backups anteriores foram preservados. Não foi restaurado o backup histórico de 11 registros da Fase 0.

O arquivo pessoal original foi apenas lido para cópia/hash; **não foi gravado por testes ou pelo aplicativo nesta fase**. O caminho continua `backlog.json` no diretório de trabalho. A abertura não regrava ou migra dados.

A verificação X201 usou exclusivamente uma cópia temporária: confirmou igualdade semântica dos nove registros após roundtrip, acrescentou um livro simulado com página 12, recarregou e editou a página para 24. Os nove registros iniciais permaneceram iguais no arquivo misto de dez registros. O `.bak` inicial preservou os bytes anteriores, e original/backup externo permaneceram intactos.

`SteamAppId` permanece opaco, sem conversão para IGDB ou IDs simulados, e sem deduplicação. `IgdbId` continua reservado a jogos com referência IGDB confirmada. Origem e IDs das outras mídias ficam no bloco multimídia. Referências conflitantes são recusadas.

Versões, tipos ou campos desconhecidos dentro dos blocos novos bloqueiam leitura/gravação, preservando o arquivo; não são descartados silenciosamente. Extras desconhecidos da raiz dos registros antigos continuam sendo preservados.

## Arquivos

Modificados:

- `BacklogManager.csproj`: inclui os recursos locais de capas simuladas.
- `Data/ConversorBibliotecaJson.cs`: conversão do bloco opcional e validação de referências.
- `Data/FileManager.cs`: valida o bloco multimídia e protege seu nome reservado nos extras.
- `MainWindow.axaml`: seletores de mídia, indicação de simulação, progresso e bindings/rótulos adaptados.
- `MainWindow.axaml.cs`: pesquisa por tipo, preparação genérica do formulário, filtro de mídia e edição com progresso.
- `Models/BacklogItem.cs`: bloco opcional de persistência.
- `Models/DadosPessoaisBiblioteca.cs`: progresso opcional.
- `Models/ItemBiblioteca.cs`: textos de status/progresso para apresentação.
- `Models/ObraCatalogo.cs`: flag de simulação e texto de gêneros.
- `Models/TipoMidia.cs`: comentário atualizado para a disponibilidade das categorias.
- `Services/FormularioJogo.cs`: entrada compatível para preparação comum e progresso.
- `Services/ServicoBiblioteca.cs`: validação de progresso e filtro opcional por tipo.
- `Tests/BacklogManager.Tests/BuscaJanelaTests.cs`: artefatos visuais separados dos resultados históricos.
- `Tests/BacklogManager.Tests/Fase1/ArmazenamentoJsonTests.cs`: oito cenários ajustados ao suporte de formato aprovado.

Criados:

- `MainWindow.Multimidia.cs`.
- `Models/DadosMultimidiaJson.cs`, `Models/ProgressoPessoal.cs`, `Models/TextosMidia.cs`.
- `Services/CatalogoSimulado.cs`, `Services/FormularioObra.cs`, `Services/RoteadorPesquisaCatalogo.cs`.
- `Assets/CapasSimuladas/Anime.svg`, `Serie.svg`, `Filme.svg`, `Manga.svg`, `Livro.svg`.
- `Tests/BacklogManager.Tests/Fase2/ApoioMultimidia.cs`.
- `Tests/BacklogManager.Tests/Fase2/CatalogosSimuladosTests.cs`.
- `Tests/BacklogManager.Tests/Fase2/ProgressoTests.cs`.
- `Tests/BacklogManager.Tests/Fase2/PersistenciaMultimidiaTests.cs`.
- `Tests/BacklogManager.Tests/Fase2/FormularioMultimidiaTests.cs`.
- `Tests/BacklogManager.Tests/Fase2/RoteadorTests.cs`.
- `Tests/BacklogManager.Tests/Fase2/InterfaceMultimidiaTests.cs`.
- `Tests/BacklogManager.Tests/Fase2/CategoriasBibliotecaTests.cs`.
- `scripts/gerar_relatorio_fase2.py`.
- `docs/ARQUITETURA_FASE2.md`, `docs/FASE2.md`, `docs/FASE2_TESTES.md`, `docs/resultados-fase2.json`.

O registro `docs/APROVACAO_FASE1.md` foi criado e integrado na etapa de aprovação anterior, em `56bcd58`. `Contrato.md`, `qodana.yaml`, a biblioteca e os relatórios anteriores foram preservados. `IgdbService`, `PesquisaJogosIgdb`, `ConfiguracaoIgdb` e seus protocolos/credenciais não foram modificados. Nenhuma dependência nova foi adicionada.

## Testes e resultados

Ambiente: Linux CachyOS, .NET SDK 10.0.113, runtime 10.0.13, Avalonia 12.1.4, xUnit v3 3.2.2 e Avalonia.Headless.XUnit 12.1.4.

- Base antes das alterações: **323/323 aprovados**.
- Integração inicial do suporte multimídia: **323/323 aprovados**.
- Catálogos/progresso/formato/formulário/rotas: **493/495 aprovados**; duas expectativas de busca parcial dos testes foram corrigidas, sem mudar o comportamento de busca do aplicativo.
- Primeira execução com a interface: **513/515 aprovados**, ainda com as duas expectativas anteriores na assembly dessa execução.
- Suíte ampliada com filtros/status/fluxos adicionais: **533/533 aprovados**.
- Primeira execução final: **534/534 aprovados**.
- Inspeção visual encontrou rótulo de status antigo no campo fechado; UM28 reproduziu a falha em execução isolada e foi corrigido.
- **Compilação final: sucesso, zero erros e zero avisos.**
- **Suíte final: 535 aprovados, zero reprovados e zero não executados.**

São **212 cenários novos**, além dos **323 cenários anteriores executados novamente**. Os cenários AR09–AR15 passaram de recusa temporária para verificação de gravação/recarga de mídia/capa/gêneros. AR19 continua protegendo contra lote inválido, agora usando tipo desconhecido em vez de livro. Nenhum teste foi removido e os relatórios históricos não foram regenerados.

| Grupo novo | IDs | Cenários | Aprovados |
|---|---|---:|---:|
| Catálogos simulados | SM01–SM50 | 50: dez para cada uma das cinco mídias novas | 50 |
| Progresso pessoal | PG01–PG60 | 60: dez para cada uma das seis mídias | 60 |
| Formato e compatibilidade JSON | JM01–JM30 | 30 | 30 |
| Formulário multimídia | FM01–FM20 | 20 | 20 |
| Roteamento e recursos locais | RT01–RT12 | 12 | 12 |
| Interface das seis categorias | UM01–UM28 | 28 | 28 |
| Filtro de categorias e status | MT01–MT12 | 12 | 12 |
| Regressão das fases anteriores | P/I/C/H/Q/A/E/R/B/F/MP/AR/CA/AD/ED/RM/FI/FO/PE/CO/UI | 323 | 323 |
| **Total** | | **535** | **535** |

A [tabela completa dos testes](FASE2_TESTES.md) contém ID, cenário, resultado esperado, observado, tipo e status, gerados do TRX final real. A [evidência estruturada](resultados-fase2.json) contém os mesmos resultados/durações. Cada grupo satisfaz o mínimo de dez cenários relevantes; catálogos e progresso têm dez por mídia, incluindo entradas válidas/vazias, limites, cancelamento ou validação e persistência conforme aplicável.

| ID | Verificação adicional | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| X201 | Compatibilidade com cópia da biblioteca pessoal e inclusão de livro simulado | Nove registros preservados; novo registro/progresso recarregáveis; original intacto | Igualdade JSON dos nove registros confirmada; cópia mista com dez registros; página editada de 12 para 24; backup inicial exato; original e backup externo intactos | Automatizado adicional em cópia | Aprovado |
| X202 | Inicialização desktop nativa em pasta isolada sem credenciais | Processo aberto e nenhuma gravação automática | Aberto por oito segundos, encerrado pelo verificador; log vazio; cópia e original intactos | Automatizado adicional de inicialização | Aprovado |

**Planejados: 547 cenários** (535 da suíte, duas verificações adicionais e dez validações manuais). **Executados: 537; aprovados: 537; reprovados atuais: zero; não executados: dez manuais**, listados abaixo. A inicialização nativa não equivale a interação manual no Rider ou chamada autenticada à IGDB.

UM20 gera dezoito imagens das seis categorias na janela mínima (biblioteca, topo do formulário e região de progresso/ações). Amostras foram inspecionadas, incluindo os rótulos de livro e os campos de progresso/ações de animes, séries e mangás. Os artefatos e TRX desta execução estão em `/tmp/backlogmanager-fase2-resultados/`; a suíte pode regenerá-los.

### Falhas e ajustes encontrados

| Evidência | Problema | Correção/verificação |
|---|---|---|
| SM35/SM45 | Os testes usavam os artigos “O”/“A” como trecho e esperavam um resultado único, mas vários títulos corretamente correspondiam | Entradas de teste passaram a usar um trecho significativo e único; as buscas do aplicativo não foram alteradas; ambos passaram na suíte final |
| Inspeção visual e UM28 | O `ComboBox` fechado podia manter “Na Fila” após traduzir sua opção para “Quero ler” | Seleção reaplicada após atualizar os rótulos, refrescando o conteúdo visual; reprodução isolada falhou antes da correção; UM28 e suíte final passaram |
| UM27 | Aviso de falta de credenciais de jogos não deve contradizer a disponibilidade do catálogo local | Aviso específico ocultado ao selecionar mídia simulada; teste aprovado |
| Análise de entradas e UM10 | Frações em campos inteiros poderiam ser truncadas por conversão direta | Validação explícita antes da conversão, com aviso de progresso inválido; teste aprovado sem gravação |

Um aviso do analisador de testes sobre `Where` antes de `Assert.Single` foi corrigido usando a sobrecarga com predicado. Não há falhas críticas conhecidas abertas nas funcionalidades implementadas. A suíte completa foi repetida após a correção visual.

## Executar e validar no Rider

```bash
dotnet restore BacklogManager.slnx
dotnet build BacklogManager.slnx --no-restore -m:1 -warnaserror
dotnet test BacklogManager.slnx --no-build --no-restore -m:1 --logger 'trx;LogFileName=fase2.trx' --results-directory TestResults
python3 -B scripts/gerar_relatorio_fase2.py TestResults/fase2.trx
```

A suíte utiliza dados fictícios, arquivos temporários e HTTP simulado; não lê a biblioteca pessoal. O gerador não executa testes nem lê dados pessoais e atualiza somente relatórios da Fase 2 a partir do TRX indicado.

Para validar manualmente, use uma nova cópia isolada:

```bash
pasta_validacao_fase2=$(mktemp -d /tmp/backlogmanager-rider-fase2-XXXXXX)
cp -- backlog.json "$pasta_validacao_fase2/backlog.json"
printf '%s\n' "$pasta_validacao_fase2"
```

Abra `BacklogManager.slnx` no Rider e configure **Working directory** com essa pasta. Para as cinco categorias novas, escolha o tipo no cadastro e digite **simulado** para ver os exemplos. Selecione uma sugestão, informe progresso opcional e salve. Na biblioteca, escolha o mesmo tipo para ver os registros. Ao reiniciar, a categoria inicial continua Jogos; a seleção da categoria não é uma preferência persistida nesta fase.

Jogos continuam usando `IGDB_CLIENT_ID` e `IGDB_ACESS_TOKEN` ou `IGDB_ACCESS_TOKEN` na configuração local. Nunca inclua credenciais nos arquivos versionados. Nenhuma chamada autenticada real foi executada pelo agente nesta fase.

| ID | Cenário manual | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| M201 | Abrir cópia da biblioteca antiga no Rider | Registros e IDs preservados, sem gravação na abertura | Não realizado manualmente nesta fase | Manual | Não executado |
| M202 | Pesquisar/cadastrar jogo pela IGDB real | Sugestões e seleção por ID funcionam; `IgdbId` correto | Não realizado contra API autenticada nesta fase | Manual | Não executado |
| M203 | Cadastrar anime simulado com episódios | Indicação de simulação e progresso persistentes | Cobertura automatizada aprovada; interação no Rider pendente | Manual | Não executado |
| M204 | Cadastrar série com temporada/episódio | Campos próprios persistem e reaparecem na edição | Cobertura automatizada aprovada; interação no Rider pendente | Manual | Não executado |
| M205 | Cadastrar filme com minutos | Minutos opcionais, inclusive decimais, preservados | Cobertura automatizada aprovada; interação no Rider pendente | Manual | Não executado |
| M206 | Cadastrar mangá com capítulo/volume | Progresso próprio e origem simulada persistentes | Cobertura automatizada aprovada; interação no Rider pendente | Manual | Não executado |
| M207 | Cadastrar livro, editar página/capítulo e reiniciar | Progresso e rótulos de leitura corretos após recarga | Cobertura automatizada aprovada; interação no Rider pendente | Manual | Não executado |
| M208 | Alternar categorias, pesquisar rapidamente, limpar e filtrar status | Somente tipo/resultados atuais; registros de outras mídias preservados | Cobertura automatizada aprovada; interação no Rider pendente | Manual | Não executado |
| M209 | Reduzir janela e rolar formulários | Controles, progresso e ações acessíveis; simulação legível | Imagens Headless inspecionadas; interação no Rider pendente | Manual | Não executado |
| M210 | Simular falha de gravação somente na cópia e tentar novamente | Arquivo válido preservado; sem falso sucesso; retomada sem duplicação | Cobertura automatizada aprovada; interação no Rider pendente | Manual | Não executado |

O relato geral de testes manuais e a aprovação da Fase 1 permanecem no registro próprio, sem atribuição individual de cenários nem nova aprovação automática da Fase 2.

## Riscos e limites pendentes

- Executáveis anteriores não compreendem as novas mídias/progresso. Não os use para editar uma coleção ampliada. Para retorno de versão, feche o aplicativo, preserve o arquivo novo e restaure conscientemente uma cópia do backup externo anterior; não há downgrade automático.
- O caminho continua dependendo do diretório de trabalho; alterá-lo pode abrir outra coleção. Nenhum dado foi movido automaticamente.
- Os catálogos novos são demonstrações locais com três entradas por mídia. IDs e capas são simulados; não equivalem a integrações reais. A escolha das APIs e a implementação guiada permanecem reservadas ao proprietário.
- Capas têm referências/recursos preparados, mas sua apresentação em grade/lista/sugestões está reservada à Fase 3.
- `.bak` mantém uma versão anterior e o backup externo está no mesmo computador. Não houve teste de falha física de disco ou queda de energia.
- O hash/bloqueio continua sujeito a editores externos que ignorem o bloqueio entre a conferência e a substituição.
- Versões ou campos futuros desconhecidos dos blocos novos são recusados; recuperação deve preservar o arquivo e não apagar registros para forçar abertura.
- A origem dos IDs legados permanece indeterminada. Tokens locais podem expirar e continuam sem renovação automática.
- Não houve nova validação manual no Rider, chamada real autenticada IGDB nem teste em Windows/macOS pelo agente nesta fase. As ressalvas históricas aprovadas das fases anteriores foram preservadas.

## Estado Git e encerramento

A `main` permanece na Fase 1 aprovada em `56bcd58`, com as branches anteriores mantidas. A Fase 2 fica em commits próprios de `feat/multimidia`. `qodana.yaml` continua não rastreado e não foi incluído nos commits; contrato, biblioteca e relatórios anteriores foram conferidos por hash e preservados.

Não houve push, merge da Fase 2 ou início da Fase 3. Aguardar revisão e autorização explícita do proprietário antes de qualquer avanço.

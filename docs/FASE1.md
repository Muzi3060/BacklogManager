# Fase 1 — Arquitetura

## Situação para revisão

A Fase 0 foi oficialmente aprovada pelo proprietário, incluindo as ressalvas documentadas, e integrada na `main` por `git merge --ff-only chore/seguranca-estabilidade`, sem conflitos. A `main` passou de `7ce3009` para `10aab6a`, exatamente o conteúdo aprovado da branch da Fase 0. A consulta ao remoto confirmou a base antes da integração. Nenhuma branch foi excluída e não houve push.

A Fase 1 foi desenvolvida exclusivamente em `refactor/arquitetura`, criada a partir dessa `main`. **A Fase 1 não foi integrada na `main`; a Fase 2 não foi iniciada.** Este relatório apresenta a implementação para revisão do proprietário, conforme `Contrato.md` versão 1.1, seções 15, 16, 18, 19, 19.1 e 20.

## Resumo da implementação

- Separação entre `ObraCatalogo`, `DadosPessoaisBiblioteca` e `ItemBiblioteca`.
- `TipoMidia` preparado com os seis tipos previstos, sem acrescentar telas, cadastros ou integrações para outras mídias.
- Contratos `IArmazenamentoBiblioteca` e `IPesquisaCatalogo` para permitir substituição dos componentes sem acoplar a janela ao `FileManager` ou ao serviço HTTP específico.
- `ServicoBiblioteca` responsável pelo carregamento, cadastro, edição pessoal, remoção, filtros, resumo e recentes existentes. A memória só é confirmada após salvar.
- `FormularioJogo` responsável pela preparação/validação dos dados do formulário, sem acessar controles ou disco.
- Adaptadores JSON e IGDB em uso no fluxo real da janela. `BacklogItem` continua descrevendo o JSON legado; `IgdbGame` continua descrevendo a resposta da IGDB.
- Composição das dependências em `ComposicaoAplicativo`, incluindo configuração local e propriedade/descarte do cliente HTTP.
- Ajuste dos bindings da interface para `Catalogo.Titulo`, `Pessoal.Status`, `Pessoal.Nota` e demais campos separados, preservando layout, navegação e comportamento.
- Preservação da implementação de `FileManager`, `IgdbService` e `ConfiguracaoIgdb` da Fase 0.
- Correção da seleção após filtrar registros inteiramente iguais, identificada nos testes da refatoração.

Não foram adicionadas dependências de produção ou de testes. Não foram implementados MVVM, contêiner de injeção, armazenamento remoto, novas APIs, novos serviços simulados na aplicação, categorias visuais, progresso por mídia, coleções ou outras funcionalidades de fases posteriores. Os substitutos simulados utilizados nesta fase pertencem somente ao projeto de testes.

## Decisões arquiteturais e conceitos C#

O [documento de arquitetura](ARQUITETURA.md) descreve componentes, fluxo e limites, com diagrama de dependências e explicações didáticas.

- **Interfaces:** definem as poucas operações esperadas de armazenamento e pesquisa. O serviço da biblioteca depende do contrato; a composição escolhe o armazenamento JSON atual.
- **Injeção pelo construtor:** a janela recebe os serviços prontos. Isso permite exercitá-la com armazenamento em memória e HTTP simulado, sem acessar a biblioteca real.
- **Adaptadores:** convertem entre os modelos comuns e os formatos já utilizados no disco e na IGDB. A conversão não é uma migração do arquivo.
- **`record`, `init` e `with`:** permitem preparar uma edição sem modificar os dados originais antes de salvar. O serviço recebe somente o novo bloco pessoal, mantendo catálogo e metadados legados.
- **Coleção somente leitura:** a janela consulta os itens; as alterações passam pelo serviço, que coordena a gravação.
- **Identidade de registros:** duas instâncias com valores iguais continuam sendo dois registros. O serviço localiza por referência, e a seleção visual é restaurada pelo índice da instância.

O contrato de armazenamento permanece síncrono para acompanhar a persistência local atual. A busca mantém `Task` e `CancellationToken` porque realiza HTTP. A futura implementação remota deverá avaliar assincronia e conflitos no momento apropriado, sem criar esse backend agora.

Os metadados JSON desconhecidos são mantidos explicitamente em `ItemBiblioteca` para compatibilidade. Essa dependência transitória de `JsonElement` não é introduzida nos dados de catálogo ou pessoais.

## Proteção e compatibilidade dos dados

Antes das alterações nos modelos e no armazenamento, foi criado e verificado byte a byte um novo backup externo da biblioteca encontrada no início desta fase:

`${HOME}/Backups/BacklogManager/fase1-20261027-205329/backlog.json`

- Registros atuais: **9**.
- Tamanho atual: **1535 bytes**.
- SHA-256 do original e do backup desta fase:

```text
d33b826a57148dc8c7017ae22e15f6898163ba7d5d5eea57a7ba64b6ddaac810
```

Esses são os dados existentes no começo da Fase 1. Não foi restaurado o backup histórico de 11 registros da Fase 0. Os backups anteriores foram preservados.

O JSON pessoal não foi gravado nesta fase. A verificação adicional X101 fez leitura/gravação somente em uma cópia temporária, atravessando `ServicoBiblioteca`, modelos separados e `ArmazenamentoJsonBiblioteca`. Os nove registros e todos os valores JSON permaneceram semanticamente iguais; o `.bak` da cópia reteve os bytes anteriores. O hash do original permaneceu igual antes/depois dos testes e da integração Git.

O caminho continua sendo `backlog.json` no diretório de trabalho, resolvido por `FileManager` uma vez por sessão. Não houve migração para outra pasta. `SteamAppId` não foi convertido, reinterpretado ou deduplicado; apenas um `IgdbId` já explícito vira referência IGDB no modelo em memória e retorna ao mesmo campo ao salvar.

Capas, gêneros de catálogo, outros provedores e outras mídias ainda não cabem no formato atual. O adaptador JSON recusa o lote antes de gravar quando recebe esses dados, em vez de descartá-los silenciosamente. O modelo está preparado para evoluir; o suporte de armazenamento dessas informações exige uma futura alteração de formato com autorização e backup próprios.

## Arquivos criados e modificados

Modificados nesta fase:

| Arquivo | Alteração |
|---|---|
| `MainWindow.axaml` | Bindings e tipos dos templates ajustados aos modelos separados; layout preservado. |
| `MainWindow.axaml.cs` | Dependências recebidas por construtor; operações delegadas; seleção por instância. |
| `Models/BacklogItem.cs` | Comentário documenta seu papel de registro do formato legado. |
| `Tests/BacklogManager.Tests/TelaTeste.cs` | Fixture adaptada aos contratos e modelos novos, preservando os dados e asserções de regressão. |
| `Tests/BacklogManager.Tests/BuscaJanelaTests.cs` | Sugestões do teste usam o modelo comum; imagens desta fase ficam em diretório próprio. |

Criados:

- `Models/TipoMidia.cs`, `Models/ObraCatalogo.cs`, `Models/DadosPessoaisBiblioteca.cs`, `Models/ItemBiblioteca.cs`.
- `Contracts/IArmazenamentoBiblioteca.cs`, `Contracts/IPesquisaCatalogo.cs`.
- `Data/ArmazenamentoJsonBiblioteca.cs`, `Data/ConversorBibliotecaJson.cs`.
- `Services/ServicoBiblioteca.cs`, `Services/FormularioJogo.cs`, `Services/PesquisaJogosIgdb.cs`.
- `Configuracao/ComposicaoAplicativo.cs`.
- `Tests/BacklogManager.Tests/Fase1/ApoioArquitetura.cs`.
- `Tests/BacklogManager.Tests/Fase1/ServicoBibliotecaTests.cs`.
- `Tests/BacklogManager.Tests/Fase1/ConversorBibliotecaTests.cs`.
- `Tests/BacklogManager.Tests/Fase1/ArmazenamentoJsonTests.cs`.
- `Tests/BacklogManager.Tests/Fase1/FormularioJogoTests.cs`.
- `Tests/BacklogManager.Tests/Fase1/ConsultasBibliotecaTests.cs`.
- `Tests/BacklogManager.Tests/Fase1/PesquisaCatalogoTests.cs`.
- `Tests/BacklogManager.Tests/Fase1/ComposicaoTests.cs`.
- `Tests/BacklogManager.Tests/Fase1/JanelaContratosTests.cs`.
- `scripts/gerar_relatorio_fase1.py`.
- `docs/ARQUITETURA.md`, `docs/FASE1.md`, `docs/FASE1_TESTES.md`, `docs/resultados-fase1.json`.

`Contrato.md`, `qodana.yaml`, a biblioteca original e os relatórios/documentos da Fase 0 permaneceram preservados. O gerador novo lê metadados do gerador anterior, mas grava somente relatórios da Fase 1.

## Testes e resultados

Ambiente de execução: Linux CachyOS, .NET SDK 10.0.113, runtime 10.0.13 e Avalonia 12.1.4. Pacotes de testes existentes mantidos: xUnit v3 3.2.2 e Avalonia.Headless.XUnit 12.1.4.

- Base após integrar a Fase 0: **175/175 aprovados**.
- Primeira integração da janela com os novos modelos: **175/175 aprovados**.
- Suíte ampliada com serviço, conversão e armazenamento: **255/255 aprovados**.
- Suíte com formulário, pesquisa, consultas e composição: **320/320 aprovados**.
- Ampliação para identidade/ID não canônico: **321/323 aprovados**, com duas falhas identificadas e corrigidas.
- Regressão após as correções: **323/323 aprovados**.
- Compilação final com `-warnaserror`: **sucesso, zero erros e zero avisos**.
- Execução final após formatação/correções: **323/323 aprovados, zero reprovados, zero não executados**.

A suíte final contém **175 cenários de regressão e 148 cenários novos**, distribuídos nos grupos abaixo. Os grupos novos têm no mínimo dez cenários distintos, incluindo falhas, compatibilidade, limites e recuperação quando aplicáveis. Cadastro, edição e remoção possuem grupos próprios.

| Grupo da Fase 1 | IDs | Cenários | Aprovados |
|---|---|---:|---:|
| Modelos e conversão de compatibilidade | MP01–MP20 | 20 | 20 |
| Armazenamento pelo contrato | AR01–AR20 | 20 | 20 |
| Carregamento pelo serviço | CA01–CA10 | 10 | 10 |
| Cadastro pelo serviço | AD01–AD10 | 10 | 10 |
| Edição pessoal pelo serviço | ED01–ED10 | 10 | 10 |
| Remoção pelo serviço | RM01–RM10 | 10 | 10 |
| Filtros, resumo e recentes | FI01–FI13 | 13 | 13 |
| Formulário | FO01–FO21 | 21 | 21 |
| Pesquisa pelo contrato | PE01–PE12 | 12 | 12 |
| Composição e configuração | CO01–CO10 | 10 | 10 |
| Janela com contratos substituídos | UI01–UI12 | 12 | 12 |
| Regressão da Fase 0 | P/I/C/H/Q/A/E/R/B/F | 175 | 175 |
| **Total da suíte** | | **323** | **323** |

A [tabela individual](FASE1_TESTES.md) apresenta cenário, esperado, observado, tipo e status para todos os 323 testes, gerada do TRX real. A [evidência estruturada](resultados-fase1.json) preserva os mesmos resultados e durações. Os arquivos da Fase 0 não foram regenerados ou sobrescritos.

Verificações adicionais, fora da suíte repetível:

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| X101 | Roundtrip da cópia da biblioteca atual pelos novos modelos e adaptador | Nove registros e todos os valores preservados; backup exato; original intacto | Nove registros; igualdade semântica JSON confirmada; `.bak` byte a byte igual ao conteúdo anterior; original com hash preservado | Automatizado adicional em cópia temporária | Aprovado |
| X102 | Inicialização desktop nativa sem credenciais em pasta isolada | Aplicativo permanece aberto e não grava a coleção | Processo aberto por oito segundos, encerrado pelo verificador; log vazio; cópia e original intactos | Automatizado adicional de inicialização | Aprovado |

**Total planejado nesta fase: 335 cenários** (323 da suíte, duas verificações adicionais e dez validações manuais). **Executados: 325; aprovados: 325; reprovados na entrega: zero; não executados: dez manuais**, discriminados abaixo. A contagem não transforma a inicialização nativa em validação manual completa.

Três imagens Skia geradas com dados simulados foram inspecionadas: início sem credenciais, listagem da coleção e formulário com falha de gravação no tamanho mínimo. Os títulos, status, notas, reviews e avisos continuam visíveis com os bindings novos. As imagens e os resultados desta execução estão em `/tmp/backlogmanager-fase1-resultados/`; os artefatos temporários podem ser regenerados pela suíte.

### Bugs encontrados e corrigidos

| Evidência | Problema observado | Correção | Reexecução |
|---|---|---|---|
| UI12 | Após filtrar duas instâncias com valores iguais, o `ListBox` podia selecionar a primeira cópia | A seleção passa a ser restaurada pelo índice da mesma instância, usando `ReferenceEquals` | UI12 e toda a suíte final aprovados |
| FO21 | Formulário aceitava ID externo com zeros à esquerda, mas o adaptador o recusava | Validação alinhada à representação decimal canônica antes do armazenamento | FO21 e toda a suíte final aprovados |
| Compilação final intermediária | Remoção indevida de `using System.Linq` impediu o `OfType` do `ComboBox` existente de compilar | Importação restaurada | Compilação final sem erros/avisos e suíte final aprovada |

As duas falhas automatizadas foram reproduzidas com dados simulados e corrigidas sem acessar a biblioteca pessoal. Nenhuma falha relevante conhecida desses cenários ficou aberta. As pendências manuais e limitações continuam explícitas.

## Executar e validar no Rider

Na raiz do repositório:

```bash
dotnet restore BacklogManager.slnx
dotnet build BacklogManager.slnx --no-restore -m:1 -warnaserror
dotnet test BacklogManager.slnx --no-build --no-restore -m:1 --logger 'trx;LogFileName=fase1.trx' --results-directory TestResults
python3 -B scripts/gerar_relatorio_fase1.py TestResults/fase1.trx
```

Os testes da suíte utilizam somente arquivos temporários, dados simulados e HTTP simulado. Não exigem credenciais nem internet após a restauração dos pacotes. O script do relatório não lê a biblioteca pessoal e atualiza apenas os resultados da Fase 1 a partir da execução indicada.

Para validação manual, faça uma nova cópia, sem sobrescrever pastas de testes anteriores:

```bash
pasta_validacao_fase1=$(mktemp -d /tmp/backlogmanager-rider-fase1-XXXXXX)
cp -- backlog.json "$pasta_validacao_fase1/backlog.json"
printf '%s\n' "$pasta_validacao_fase1"
```

Abra a solução no Rider, selecione `BacklogManager` e configure **Working directory** com a pasta impressa. Para pesquisar realmente na IGDB, use suas variáveis locais `IGDB_CLIENT_ID` e `IGDB_ACESS_TOKEN` ou `IGDB_ACCESS_TOKEN`. Não coloque credenciais em arquivos versionados ou relatórios.

A interação manual no Rider e a pesquisa autenticada real não foram executadas pelo agente nesta fase. Os testes manuais aprovados pelo proprietário na Fase 0 permanecem históricos; não são registrados como nova aprovação automática da Fase 1.

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| MF01 | Abrir a cópia no Rider | Mesmos registros e textos da biblioteca, sem regravação na abertura | Não realizado manualmente no Rider nesta fase | Manual | Não executado |
| MF02 | Editar nota e review de legado e reiniciar | Dados pessoais persistem; título e identificadores preservados | Não realizado manualmente no Rider nesta fase | Manual | Não executado |
| MF03 | Pesquisar na IGDB real, selecionar e cadastrar | Sugestões, seleção por ID e persistência corretas | Não realizado contra a API real nesta fase | Manual | Não executado |
| MF04 | Adicionar duas cópias de um jogo, filtrar e editar a segunda | Seleção não muda para a primeira; registros continuam independentes | Versões automatizadas aprovadas; interação manual pendente | Manual | Não executado |
| MF05 | Remover um registro e reiniciar | Somente registro selecionado removido; demais valores preservados | Não realizado manualmente no Rider nesta fase | Manual | Não executado |
| MF06 | Executar sem credenciais e usar filtros/edição | Aviso e funcionamento local preservados | Inicialização nativa verificada; fluxo manual no Rider pendente | Manual | Não executado |
| MF07 | Pesquisar com token inválido | Aviso de autenticação sem encerrar aplicativo | Simulação automatizada aprovada; API real não consultada nesta fase | Manual | Não executado |
| MF08 | Digitar rapidamente, limpar e navegar durante busca | Somente sugestões atuais; cancelamento e navegação corretos | Testes Headless aprovados; interação manual pendente | Manual | Não executado |
| MF09 | Reduzir ao tamanho mínimo e conferir recentes/listagem/avisos | Layout e textos preservados com os bindings novos | Imagens Skia inspecionadas; interação manual no Rider pendente | Manual | Não executado |
| MF10 | Simular falha de gravação exclusivamente na cópia e restaurar a permissão | Operação não confirmada; arquivo válido preservado; nova tentativa salva | Simulações automatizadas aprovadas; interação manual pendente | Manual | Não executado |

## Riscos e limitações

- O JSON atual ainda suporta somente os dados de jogos existentes; a recusa de dados futuros é intencional para evitar perda de informação. Não há migração multimídia nesta fase.
- O caminho depende do diretório de trabalho, por compatibilidade. Uma configuração diferente do Rider pode abrir outra coleção; nenhum arquivo é movido automaticamente.
- IDs legados continuam com origem indeterminada. O modelo mantém esses valores separados da referência explícita IGDB.
- A seleção é preservada por identidade da instância na sessão. Não foi introduzido um identificador interno persistente ou uma nova versão de formato.
- Os metadados JSON de compatibilidade e o contrato síncrono de armazenamento são decisões transitórias documentadas, a revisar quando houver evolução real de formato ou backend remoto.
- O bloqueio e o hash da Fase 0 continuam sujeitos à limitação de um editor externo que ignore o bloqueio e altere o arquivo entre a conferência e a substituição.
- `.bak` guarda somente a versão anterior. O backup externo desta fase está no mesmo computador; falha física do dispositivo não foi simulada.
- As ressalvas aprovadas da Fase 0 continuam preservadas: M05 não executado e restauração manual pelo `.bak` não realizada em M10. A Fase 1 não as converte em aprovações.
- Pesquisa real autenticada e validação interativa no Rider precisam de revisão do proprietário para esta versão. Não houve teste em Windows/macOS nem queda de energia real.
- Tokens continuam locais, podem expirar e são lidos ao iniciar. Não foi implementada renovação com Client Secret.

## Estado Git e encerramento

A `main` contém a Fase 0 aprovada em `10aab6a`; a branch `chore/seguranca-estabilidade` foi mantida. A Fase 1 fica em commits próprios de `refactor/arquitetura`, sem merge na `main` e sem push.

`qodana.yaml` continua não rastreado e fora dos commits. Seu hash permanece `aabdb19b53fec2d5f02ef051fa2f505b69bc1fbe18d3e6c94564a05f3575f139`. O contrato, a biblioteca original e os relatórios históricos da Fase 0 foram verificados por hash e preservados.

A implementação e os relatórios ficam disponíveis para revisão. Aguardar aprovação explícita antes de integrar a Fase 1 ou iniciar a Fase 2.

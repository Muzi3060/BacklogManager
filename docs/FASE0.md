# Fase 0 — Estabilidade

Implementação limitada à Fase 0 de `Contrato.md` versão 1.1, incluindo a seção 19.1. Base: `7ce3009`. Branch: `chore/seguranca-estabilidade`. Não houve merge, exclusão de branch, push ou início da Fase 1.

Registro complementado com a validação manual realizada pelo proprietário no Rider e informada nesta conversa. Os testes manuais utilizaram uma cópia do `backlog.json` em pasta temporária, sem utilizar a biblioteca original. Esta atualização altera somente este documento; não acrescenta execuções de compilação ou testes automatizados nem representa aprovação de merge ou de outra fase.

## O que foi implementado

- A coleção local abre mesmo sem configuração IGDB. Cadastro continua exigindo uma seleção válida da pesquisa; edição, exclusão, filtros e consulta local funcionam sem internet.
- Tratamento de HTTP 401/403, 429, demais 4xx, 5xx, falha de conexão, timeout, JSON inválido e resultados inválidos. Erro não é convertido em lista vazia enganosa.
- Cancelamento desde a espera de 400 ms até o envio/leitura HTTP. Só a pesquisa atual pode atualizar sugestões ou avisos. Limpar, editar, navegar e fechar cancelam a pesquisa.
- Reutilização de um `HttpClient` por janela, timeout de 15 segundos, renovação das conexões após cinco minutos e descarte das mensagens HTTP e do cliente próprio ao fechar. Os cabeçalhos são aplicados em cada requisição, evitando interferência em clientes compartilhados.
- Escape de aspas e barras invertidas na consulta APICalypse. Termos vazios não são enviados; termos acima de 200 caracteres ou com controles internos são recusados.
- Suporte à variável atual `IGDB_ACESS_TOKEN` e à grafia corrigida `IGDB_ACCESS_TOKEN`. A corrigida não vazia tem precedência. Bordas são aparadas; credenciais com controles/espaços internos são recusadas. Não há renovação automática nem Client Secret.
- `IgdbId` opcional para novas seleções confirmadas. Os valores existentes de `SteamAppId` são preservados sem inferência, conversão ou deduplicação.
- Gravação primeiro em temporário, `Flush(true)`, substituição do principal e backup anterior em `.bak`. A interface só aplica cadastro, edição ou remoção depois que a gravação foi concluída.
- Leitura inválida bloqueia gravações. Arquivo ausente, coleção vazia válida e arquivo inválido são estados distintos. Erros não apagam registros nem fazem recuperação silenciosa.
- Hash do conteúdo e arquivo `.lock` para detectar alterações externas e coordenar gravações de instâncias do aplicativo.
- Preservação de campos JSON desconhecidos e de reviews antigas vazias, com espaços ou acima de 1000 caracteres quando não alteradas. Novas reviews continuam limitadas a 1000 caracteres.
- Mensagens de falha em português, sem respostas HTTP brutas ou credenciais. O texto completo dos avisos está disponível ao passar o mouse sobre eles.

## Decisões e conceitos C#

`FileManager` passou de classe estática para uma instância pequena que guarda o caminho e o hash da versão carregada. Isso permite detectar conflitos por janela e usar um arquivo temporário explícito nos testes. Não foram criadas interfaces de repositório, camadas multimídia, MVVM ou abstrações remotas.

- **`CancellationToken`:** sinal compartilhado que informa que uma pesquisa perdeu sua utilidade. O mesmo sinal chega ao atraso, ao HTTP e à leitura da resposta.
- **`async Task` e `async void`:** a operação de busca retorna uma tarefa que pode ser aguardada nos testes. O evento da interface continua `async void`, com tratamento das falhas da operação.
- **`try/catch/finally`:** distingue falhas esperadas, apresenta uma orientação e garante a liberação dos recursos, inclusive quando há cancelamento.
- **`using`:** descarta requisições, respostas, streams e bloqueios no fim de seu uso.
- **Lista candidata:** prepara uma possível alteração sem mudar a coleção atual; somente confirma a alteração depois de salvar. Não é uma transação de banco de dados.
- **`int?`:** permite um identificador numérico ou `null`. Um ID legado desconhecido não recebe uma origem inventada.
- **`JsonExtensionData`:** guarda propriedades JSON não conhecidas pelo modelo para não perdê-las na próxima gravação.
- **SHA-256:** uma impressão do conteúdo, usada para comparar versões e verificar o backup; não é criptografia dos dados.
- **`InternalsVisibleTo`:** permite que somente a assembly de testes acesse o construtor interno da janela e o estado necessário para aguardar buscas, sem alterar o construtor público usado pelo aplicativo.
- **Avalonia Headless/Skia:** executa os controles e eventos reais da janela sem interação com a sessão de trabalho do usuário. HTTP é simulado com `HttpMessageHandler`.

Referências técnicas consultadas: [ciclo de vida do HttpClient](https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines), [eventos assíncronos C#](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/async-return-types), [IGDB](https://api-docs.igdb.com/), [Avalonia Headless com xUnit](https://docs.avaloniaui.net/docs/testing/headless-xunit).

## Dados, caminho e backup

A regra de armazenamento continua sendo **`backlog.json` no diretório de trabalho**. O caminho absoluto é resolvido ao construir `FileManager` e mantido por toda a sessão. Não há migração para AppData, diretório do executável ou outro local.

Com o diretório de trabalho deste repositório, o caminho é:

`${PROJETO}/backlog.json`

O diretório configurado no Rider pode ser diferente. Confira **Working directory** antes de executar; uma pasta diferente continua representando outro arquivo. Nenhuma mudança automática tenta adivinhar qual coleção usar.

Antes de alterar a persistência, foi criado e verificado byte a byte o backup externo:

`${HOME}/Backups/BacklogManager/fase0-20261027-170624/backlog.json`

A pasta tem permissão 700 e o backup tem permissão 600. O manifesto `verificacao.json` registra origem, quantidade, tamanho e hash.

- Registros: **11**.
- Tamanho: **1817 bytes**.
- SHA-256 do original e do backup verificado durante a implementação inicial:

```text
4cfc6e877e370f4c154f22afa521f725d652c31f3ef0425fd4d5623138cfc25e
```

Os dados de quantidade, tamanho e hash acima são o registro histórico da verificação realizada na implementação inicial, não uma afirmação sobre o estado da biblioteca após eventual uso posterior. Nesta atualização documental, a biblioteca original não foi modificada.

O arquivo real não foi gravado pelos testes nem pelo aplicativo durante a implementação. Uma cópia temporária dos dados reais foi carregada e salva pelo novo `FileManager`: os 11 registros e todos os valores JSON permaneceram semanticamente iguais; seu `.bak` preservou os bytes anteriores. Ao concluir a implementação inicial, o original e o backup externo permaneceram idênticos.

Em gravações futuras, o JSON continua sendo uma lista, sem envelope ou versão nova. `IgdbId` só é escrito quando preenchido. Os registros legados não são migrados. A formatação e a codificação do arquivo salvo podem mudar para UTF-8 sem BOM, preservando seus valores; o `.bak` retém os bytes anteriores.

O `.bak` automático guarda a versão imediatamente anterior à última substituição. Ele não é um histórico de longo prazo e não substitui o backup externo. Nenhuma restauração é automática. Para recuperar: feche todas as janelas, preserve uma cópia do arquivo problemático, confira o backup e restaure conscientemente no mesmo caminho. Valide esse procedimento primeiro em cópias.

## Arquivos

Modificados:

- `.gitignore`: ignora o arquivo de bloqueio da coleção real.
- `BacklogManager.csproj`: exclui fontes/recursos de testes da compilação do aplicativo.
- `Data/FileManager.cs`: leitura validada, proteção de gravação, backup e conflito.
- `MainWindow.axaml.cs`: falhas, formulário, confirmação após salvar e busca cancelável.
- `Models/BacklogItem.cs`: `IgdbId` e preservação de propriedades desconhecidas.
- `Services/IgdbService.cs`: HTTP, entradas, cancelamento, mensagens e descarte.

Criados:

- `BacklogManager.slnx`: solução que reúne aplicativo e testes.
- `Services/ConfiguracaoIgdb.cs`: leitura compatível e validação das variáveis locais.
- `Properties/AssemblyInfo.cs`: acesso interno limitado aos testes.
- `Tests/BacklogManager.Tests/BacklogManager.Tests.csproj`.
- `Tests/BacklogManager.Tests/Apoio.cs` e `TelaTeste.cs`: isolamento, dados simulados, HTTP e janela de teste.
- `Tests/BacklogManager.Tests/PersistenciaTests.cs`.
- `Tests/BacklogManager.Tests/IdentificadoresTests.cs`.
- `Tests/BacklogManager.Tests/ConfiguracaoTests.cs`.
- `Tests/BacklogManager.Tests/IgdbTests.cs`.
- `Tests/BacklogManager.Tests/OperacoesTests.cs`.
- `Tests/BacklogManager.Tests/BuscaJanelaTests.cs`.
- `Tests/BacklogManager.Tests/FiltrosTests.cs`.
- `scripts/gerar_relatorio_testes.py`.
- `docs/FASE0.md`, `docs/FASE0_TESTES.md` e `docs/resultados-fase0.json`.

`Contrato.md`, os arquivos `.axaml`, `SteamCatalogService.cs`, o JSON pessoal e `qodana.yaml` foram preservados. Não foram adicionadas dependências de produção; os novos pacotes são exclusivos do projeto de testes.

## Validação automatizada executada durante a implementação

Ambiente: Linux CachyOS, .NET SDK 10.0.113, runtime 10.0.13, Avalonia 12.1.4. Testes com xUnit v3 3.2.2 e Avalonia.Headless.XUnit 12.1.4.

- Compilação inicial antes das mudanças: sucesso, zero erros/avisos.
- Compilação final da solução com `-warnaserror`: sucesso, zero erros/avisos.
- Suíte final: **175 aprovados, zero reprovados, zero ignorados/não executados**.
- Cada um dos dez grupos tem pelo menos dez cenários distintos. Cadastro, edição e remoção têm grupos próprios, incluindo suas falhas de persistência.
- Tabela individual com esperado, observado, tipo e status: [FASE0_TESTES.md](FASE0_TESTES.md).
- Evidência estruturada gerada do TRX: [resultados-fase0.json](resultados-fase0.json).

| Comportamento | IDs | Cenários | Aprovados |
|---|---|---:|---:|
| Persistência | P01–P34 | 34 | 34 |
| Identificadores | I01–I12 | 12 | 12 |
| Configuração | C01–C14 | 14 | 14 |
| HTTP e recursos | H01–H28 | 28 | 28 |
| Consulta e entradas | Q01–Q15 | 15 | 15 |
| Cadastro | A01–A13 | 13 | 13 |
| Edição | E01–E14 | 14 | 14 |
| Remoção | R01–R12 | 12 | 12 |
| Busca, janela e renderização | B01–B23 | 23 | 23 |
| Regressão dos filtros | F01–F10 | 10 | 10 |

Verificações adicionais executadas fora da suíte, com scripts temporários e cópias:

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| X01 | Leitura/gravação de uma cópia dos dados reais | 11 registros, equivalência de todos os valores JSON, backup e original intactos | 11 registros; `JsonNode.DeepEquals` verdadeiro; bytes do backup e do original iguais aos iniciais | Automatizado, execução adicional isolada | Aprovado |
| X02 | Inicialização desktop nativa sem credenciais | Processo permanece aberto, sem erro de inicialização nem gravação dos dados | Aberto por oito segundos; encerrado pelo verificador; log vazio; original e cópia intactos | Automatizado, teste de inicialização | Aprovado |

**Total planejado: 187 cenários**, sendo 175 da suíte, duas verificações adicionais e dez validações manuais no Rider. Na entrega inicial, havia 177 verificações aprovadas e dez testes manuais pendentes. As verificações adicionais não fazem parte da suíte repetível de 175 testes.

Após o registro dos resultados manuais informados pelo proprietário:

| Conjunto | Planejados | Aprovados sem ressalva | Aprovados com ressalva | Reprovados na entrega | Não executados |
|---|---:|---:|---:|---:|---:|
| Suíte automatizada anterior | 175 | 175 | 0 | 0 | 0 |
| Verificações adicionais anteriores (X01/X02) | 2 | 2 | 0 | 0 | 0 |
| Validação manual no Rider | 10 | 8 | 1 | 0 | 1 |
| **Total consolidado** | **187** | **185** | **1** | **0** | **1** |

Há **186 cenários com execução registrada**: 185 integralmente aprovados e M10 executado parcialmente, aprovado com ressalva. M05 permanece não executado por decisão do proprietário. A restauração pelo `.bak` é a parte não executada de M10 e não é contada como um cenário adicional nem como aprovada. Os números automatizados são históricos e não correspondem a novas execuções nesta atualização.

Foram capturadas e inspecionadas três imagens Skia com dados simulados: início sem credenciais, coleção local e aviso de falha ao gravar na janela mínima de 900×660. As imagens e o TRX desta execução estão em `/tmp/backlogmanager-resultados/`. O teste B23 regenera as imagens na pasta temporária do sistema. Isso não equivale a interação manual no Rider nem valida todas as plataformas.

### Falhas encontradas durante o desenvolvimento

Além dos riscos do diagnóstico inicial (exceções HTTP, ausência de credenciais, cancelamento parcial, IDs misturados, sobrescrita direta e memória alterada antes de salvar), os testes identificaram estas falhas concretas:

| Cenário | Falha observada | Correção e regressão |
|---|---|---|
| A12 | `InvalidDataException` escapava do filtro de erros na abertura | Exceção incluída no tratamento; janela abre com gravação bloqueada |
| E13 | Edição aparava espaços de uma review antiga inalterada | Texto original mantido quando não foi editado |
| E14 | Review antiga vazia virava texto padrão ao editar outro campo | Valor vazio original mantido |
| P33/P34 | Leitura de bytes rejeitava JSON com BOM aceito pela implementação anterior | `StreamReader` reconhece BOM antes da desserialização |
| B21 | Aviso de falha permanecia visível após sucesso | Sucesso limpa somente avisos anteriores relacionados à pesquisa |
| B22 | Texto alterado e clique imediato podiam usar seleção antiga antes do evento enfileirado | Texto atual é comparado ao texto no momento da seleção |
| Inspeção visual B23 | Exceção de arquivo em inglês e caminho longo ocultavam a orientação | Mensagem curta em português; tooltip com texto completo |

Histórico das execuções: 89/89 aprovados; 125/128 aprovados (três falhas); 170/170 aprovados após correções; 170/174 aprovados (quatro novos casos de limite); 174/174 aprovados após correções; 175/175 com renderização; 175/175 na execução final com compilação sem avisos. Os sete cenários que falharam foram executados novamente e passaram. A suíte inteira foi repetida após as últimas correções. Avisos de uso do token de cancelamento nos testes e de uma sobrecarga obsoleta de imagem também foram corrigidos.

## Como executar novamente

Na raiz do repositório:

```bash
dotnet restore BacklogManager.slnx
dotnet build BacklogManager.slnx --no-restore -m:1 -warnaserror
dotnet test BacklogManager.slnx --no-build --no-restore -m:1 --logger 'trx;LogFileName=fase0.trx' --results-directory TestResults
python3 scripts/gerar_relatorio_testes.py TestResults/fase0.trx
```

Os pacotes de testes precisam ser restaurados na primeira execução. Os testes não exigem variáveis IGDB, internet após a restauração, conta ou coleção real. O script de relatório usa somente Python padrão e os resultados TRX; não lê o JSON pessoal. Executá-lo atualiza os relatórios versionados com os resultados da nova execução. `TestResults` permanece ignorado pelo Git.

No ambiente restrito do agente, MSBuild/testhost precisaram de permissão para criar seus canais locais de comunicação; essa limitação do sandbox não foi tratada como falha funcional do aplicativo.

## Validação manual no Rider

Não use o JSON original para experimentar erros, exclusões ou restaurações. Prepare uma cópia em pasta temporária:

```bash
pasta_validacao_fase0=$(mktemp -d /tmp/backlogmanager-rider-XXXXXX)
cp -- backlog.json "$pasta_validacao_fase0/backlog.json"
printf '%s\n' "$pasta_validacao_fase0"
```

1. Abra `BacklogManager.slnx` no Rider e selecione o projeto executável `BacklogManager`.
2. Em **Run → Edit Configurations**, configure **Working directory** com a pasta temporária impressa pelo comando. Não mude o arquivo original.
3. Para testar pesquisa real, mantenha seu `IGDB_CLIENT_ID` e `IGDB_ACESS_TOKEN` somente nas variáveis da configuração local. O aplicativo também aceita `IGDB_ACCESS_TOKEN`, que tem preferência se preenchida. Não compartilhe tokens em logs, capturas, commits ou relatórios.
4. Use a janela de testes do Rider ou o terminal para executar a suíte.

### Resultados manuais informados pelo proprietário

Fonte: relato do proprietário nesta conversa, após testes no Rider com uma cópia da biblioteca em pasta temporária. O agente está registrando esses resultados; não executou novamente os testes nem inspecionou diretamente a sessão manual. Não foram fornecidas credenciais ou registradas evidências que contenham tokens.

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| M01 | Inicializar pelo Rider com a cópia da biblioteca | Aplicativo abre e carrega os registros da cópia | Aplicativo inicializou corretamente com os registros da biblioteca | Manual, informado pelo proprietário | Aprovado |
| M02 | Executar sem credenciais IGDB | Aviso esperado e uso local disponível, com edição e persistência | Aviso apresentado; uso local, edição e persistência verificados | Manual, informado pelo proprietário | Aprovado |
| M03 | Pesquisar na IGDB real, selecionar e cadastrar | Sugestões reais; cadastro persistente; novos `IgdbId` e IDs legados preservados | Pesquisa real, sugestões, cadastro e persistência após reiniciar confirmados; jogos novos receberam `IgdbId` e antigos preservaram `SteamAppId` | Manual, informado pelo proprietário | Aprovado |
| M04 | Pesquisar com token inválido | Mensagem de autenticação recusada sem encerrar o aplicativo | IGDB recusou a autenticação; mensagem adequada; sem encerramento inesperado | Manual, informado pelo proprietário | Aprovado |
| M05 | Simular falha de conexão durante pesquisa | Aviso de conexão/timeout e continuidade do uso local | Não executado por decisão do proprietário nesta rodada | Manual | Não executado |
| M06 | Pesquisar rapidamente, limpar o campo e navegar durante a busca | Somente resultados da consulta atual; sugestões removidas ao limpar; navegação funcional | Apenas resultados da última consulta; sugestões desapareceram ao limpar; navegação ocorreu normalmente | Manual, informado pelo proprietário | Aprovado |
| M07 | Editar nota e review de registro legado e reiniciar | Alterações persistem sem converter ou alterar o identificador legado | Nota e review persistiram; `SteamAppId` do LEGO Batman permaneceu `361855`, sem migração indevida para `IgdbId` | Manual, informado pelo proprietário | Aprovado |
| M08 | Adicionar, editar, excluir e reiniciar usando a cópia | Operações funcionam e persistem após reiniciar | Cadastro, edição e exclusão funcionaram corretamente, com persistência após reiniciar | Manual, informado pelo proprietário | Aprovado |
| M09 | Usar a janela no menor tamanho permitido | Interface, navegação e avisos utilizáveis e legíveis | Interface, navegação e avisos permaneceram utilizáveis e legíveis no tamanho mínimo | Manual, informado pelo proprietário | Aprovado |
| M10 | Simular falha de gravação, recuperar a capacidade de salvar e verificar restauração pelo `.bak` | Falha sem aplicar alterações; JSON válido preservado; retomada após corrigir o acesso; restauração pelo backup validada | Permissão de escrita da pasta de testes removida temporariamente: mensagem adequada, alteração não aplicada e JSON válido preservado; salvamento voltou a funcionar após restaurar as permissões. Restauração efetiva pelo `.bak` não testada manualmente | Manual, informado pelo proprietário | Aprovado com ressalva |

**Ressalva de M10:** restaurar a permissão e voltar a salvar confirma a recuperação da capacidade de gravação. Isso não equivale a restaurar dados a partir do arquivo `.bak`. Essa parte do cenário continua pendente de validação manual, embora a suíte anterior tenha verificado a geração e preservação do backup.

**Pendências manuais:** M05 foi dispensado nesta rodada por decisão do proprietário, sem receber aprovação. Se houver autorização futura, a simulação de falha de conexão deve usar a cópia em pasta temporária e verificar o aviso e a continuidade do uso local. Para concluir a parte de restauração de M10, use outra cópia isolada, feche o aplicativo, preserve o arquivo de teste, restaure conscientemente o `.bak` e confira os registros após reiniciar. Nenhum desses procedimentos foi executado nesta atualização.

### Consistência com o contrato e escopo desta atualização

O registro mantém a separação entre resultados esperados e observados exigida pela seção 19.1.3, identifica a origem dos resultados manuais, os testes não executados e a ressalva parcial de M10, conforme as seções 19 e 19.1.2. A seção 19.1.4 reserva a aprovação definitiva ao proprietário: o relatório não transforma pendências em aprovações nem autoriza merge ou avanço de fase.

`docs/FASE0_TESTES.md` e `docs/resultados-fase0.json` permanecem integralmente preservados, incluindo os 175 resultados e suas durações. As compilações, verificações adicionais e repetições de testes descritas anteriormente são registros da implementação inicial. Nesta atualização documental não houve nova compilação, execução da suíte, teste manual pelo agente ou mudança de comportamento do aplicativo. Foram realizadas somente verificações de consistência documental, integridade dos arquivos e estado Git. Não foram encontrados novos bugs no relato manual, e nenhuma correção de código foi realizada.

## Limitações e riscos restantes

- A pesquisa autenticada real e a recusa de token inválido foram aprovadas pelo proprietário no Rider (M03/M04). O agente não repetiu essas chamadas nesta atualização.
- A falha de conexão real permanece sem teste manual (M05), por decisão do proprietário. A cobertura automatizada simulada anterior foi preservada, sem substituí-la por uma aprovação manual.
- M10 foi aprovado com ressalva: falha por falta de permissão e retomada do salvamento verificadas; restauração efetiva dos dados pelo `.bak` ainda não testada manualmente.
- O caminho depende do diretório de trabalho por compatibilidade. Alterar a configuração do Rider pode abrir outra coleção. Nenhum arquivo é movido automaticamente.
- A origem dos IDs legados continua indeterminada. A correção impede novas misturas, sem converter o passado.
- O bloqueio coordena instâncias deste aplicativo. Um editor externo que ignore o bloqueio ainda pode alterar o arquivo entre a conferência do hash e a substituição; evite editar o JSON com o aplicativo aberto.
- `File.Replace` e o backup foram exercitados no sistema de arquivos Linux disponível. Não foi simulada uma queda de energia real, falha física do disco ou execução no Windows/macOS. A durabilidade depende do sistema de arquivos e do dispositivo.
- `.bak` protege somente a versão anterior; corrupção física ou perda da unidade pode afetar principal e backup. O backup externo criado nesta fase está no mesmo computador.
- JSON inválido bloqueia operações, sem tentar eliminar registros problemáticos. A recuperação precisa ser feita após fechar o aplicativo e preservar os arquivos.
- Tokens continuam locais e podem expirar. O aplicativo orienta revisar a configuração, sem Client Secret ou renovação automática.
- Não há atualização automática de credenciais durante a sessão: reinicie após alterar as variáveis.
- Não há funcionalidades de arquitetura multimídia, novas APIs, temas, nuvem, exportação/importação visual de backups ou demais fases.

## Estado Git da entrega

A branch da Fase 0 parte de `7ce3009`; `main` foi preservada. A implementação foi registrada em `97e7e82` e os testes e relatórios iniciais em `0c3fac3`. Esta atualização documental é registrada em um commit próprio na mesma branch, contendo somente `docs/FASE0.md`. `qodana.yaml` continua não rastreado e não faz parte dos commits; seu SHA-256 permaneceu `aabdb19b53fec2d5f02ef051fa2f505b69bc1fbe18d3e6c94564a05f3575f139`. `backlog.json` continua ignorado e intacto.

O registro da validação manual está finalizado com as pendências e ressalvas acima, disponível para revisão do proprietário. Merge e início da Fase 1 continuam dependendo de autorização explícita; nenhuma dessas ações foi realizada.

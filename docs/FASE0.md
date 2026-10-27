# Fase 0 — Estabilidade

Implementação limitada à Fase 0 de `Contrato.md` versão 1.1, incluindo a seção 19.1. Base: `7ce3009`. Branch: `chore/seguranca-estabilidade`. Não houve merge, exclusão de branch, push ou início da Fase 1.

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
- SHA-256 do original e do backup:

```text
4cfc6e877e370f4c154f22afa521f725d652c31f3ef0425fd4d5623138cfc25e
```

O arquivo real não foi gravado pelos testes nem pelo aplicativo durante a implementação. Uma cópia temporária dos dados reais foi carregada e salva pelo novo `FileManager`: os 11 registros e todos os valores JSON permaneceram semanticamente iguais; seu `.bak` preservou os bytes anteriores. O original e o backup externo permaneceram idênticos.

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

## Validação executada

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

**Total planejado: 187 cenários**, sendo 175 da suíte, duas verificações adicionais e dez validações manuais no Rider. **Executados: 177; aprovados: 177; reprovados atuais: zero; não executados: dez**, listados abaixo. As verificações adicionais não fazem parte da suíte repetível de 175 testes.

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

Os cenários abaixo estão pendentes porque exigem sua interação/configuração no Rider ou autenticação real. As versões equivalentes simuladas estão cobertas pela suíte; não foi alegado sucesso nestes testes manuais.

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| M01 | Abrir a solução e executar no Rider com a cópia | Janela abre e mostra 11 registros | Não realizado no Rider nesta sessão | Manual | Não executado |
| M02 | Executar sem variáveis IGDB | Aviso; coleção, edição e exclusão locais disponíveis | Não realizado no Rider nesta sessão | Manual | Não executado |
| M03 | Pesquisar com suas credenciais válidas usando o nome legado | Sugestões reais; seleção adiciona `IgdbId` na cópia | Não realizado; requer credenciais reais | Manual | Não executado |
| M04 | Executar com token simulado inválido | Falha de autenticação sem encerrar aplicativo | Não realizado contra a API real | Manual | Não executado |
| M05 | Interromper a conexão durante busca | Aviso de conexão/timeout; coleção continua disponível | Não realizado na sessão desktop do usuário | Manual | Não executado |
| M06 | Digitar rapidamente, limpar e navegar durante buscas | Somente resultados atuais; nenhum resultado antigo reaparece | Não realizado manualmente no Rider | Manual | Não executado |
| M07 | Editar nota/review de registro legado na cópia | `SteamAppId` preservado; nenhuma conversão ou deduplicação | Não realizado manualmente no Rider | Manual | Não executado |
| M08 | Cadastrar, editar, remover e reiniciar usando a cópia | Operações confirmadas persistem; contadores e filtros corretos | Não realizado manualmente no Rider | Manual | Não executado |
| M09 | Usar tamanho mínimo e passar o mouse nos avisos | Navegação/rolagem utilizáveis; mensagem completa no tooltip | Imagens Headless inspecionadas; interação nativa pendente | Manual | Não executado |
| M10 | Simular falha de gravação e restaurar backup exclusivamente na cópia | Sem falso sucesso; arquivo anterior intacto; recuperação consciente | Não realizado manualmente no Rider | Manual | Não executado |

## Limitações e riscos restantes

- A pesquisa autenticada real não foi executada nesta sessão. Headers, endpoint, consultas, respostas, falhas e cancelamento foram verificados por simulação; a validação real está em M03/M04.
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

A branch da Fase 0 parte de `7ce3009`; `main` foi preservada. As mudanças da fase estão em commits próprios. `qodana.yaml` continua não rastreado e não faz parte dos commits; seu SHA-256 permaneceu `aabdb19b53fec2d5f02ef051fa2f505b69bc1fbe18d3e6c94564a05f3575f139`. `backlog.json` continua ignorado e intacto.

A implementação fica disponível para revisão. Merge e início da Fase 1 dependem de autorização explícita do proprietário.

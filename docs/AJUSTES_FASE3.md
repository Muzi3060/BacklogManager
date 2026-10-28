# Ajustes da Fase 3 — Biblioteca

## Situação para revisão

Os ajustes foram implementados exclusivamente em **`fix/biblioteca-ajustes`**, criada do estado `bbcb99f` de `feat/biblioteca`. A branch original continua nesse commit; a `main` continua em `64fb4fb`. Não houve merge, push, exclusão de branches ou início da Fase 4. **A Fase 3 não está aprovada.**

O `Contrato.md` versão 1.1 foi relido e preservado. Foram encontrados dois arquivos não rastreados: `qodana.yaml` e `skills/system/run-workflow/SKILL.md`. Ambos tiveram seus hashes registrados e foram mantidos fora dos commits. Não havia alterações rastreadas pendentes que precisassem de stash. Os relatórios anteriores foram preservados; este documento complementa [FASE3.md](FASE3.md).

## Implementação e decisões

### Cadastro de jogos por catálogo

O fluxo já exigia uma sugestão nas seis mídias. A análise confirmou que a decisão específica de IGDB não exige restringir as outras cinco: animes, séries, filmes, mangás e livros continuam usando os catálogos simulados locais e a classificação pessoal existente, sem novas chamadas HTTP ou requisitos de autenticação.

Para jogos:

1. O campo recebe texto livre para pesquisa, mas o botão de adicionar permanece desabilitado até uma seleção válida.
2. A pesquisa usa o serviço IGDB existente, com IDs e metadados retornados pelo catálogo.
3. Selecionar uma sugestão preenche o título oficial e todos os gêneros disponíveis. O campo de gêneros é somente de leitura.
4. Alterar/limpar o texto ou desfazer a seleção invalida a seleção anterior e desabilita o botão. Uma busca antiga não substitui os resultados atuais.
5. Formulário e serviço verificam a referência IGDB. Não há cadastro de jogo por título arbitrário, ID digitado ou fallback simulado.
6. Sem credenciais, sem resultados ou com erro de conexão/autenticação, não há cadastro sem seleção válida; a interface mantém avisos claros.

A conexão é necessária para **obter a seleção IGDB**. Uma seleção válida já obtida na sessão é usada para salvar, sem uma segunda chamada HTTP no momento da gravação. Não foi acrescentada exigência de conexão para editar dados pessoais, listar, filtrar, ordenar ou excluir registros já existentes.

O título agora mostra o nome oficial da sugestão em todas as mídias. `TextChanged` é processado de forma enfileirada pelo Avalonia; o evento que resulta do preenchimento do título oficial não deve limpar a seleção nem iniciar outra pesquisa. Uma alteração real feita pelo usuário continua invalidando a seleção.

### Gêneros externos e categoria antiga

- Jogos novos usam `Catalogo.Generos` retornados pela IGDB. Não exigem o ComboBox antigo de categoria.
- O campo JSON `Categoria` continua existindo por compatibilidade estrutural. Para jogos novos, ele recebe uma **string vazia**, enquanto os gêneros ficam no catálogo. Nenhum gênero ou categoria é inventado para preencher a ausência da API.
- A validação aceita essa categoria vazia somente para jogos com referência IGDB válida; `null`, espaços em branco e vazio sem identificação IGDB continuam inválidos na persistência.
- Em jogos antigos, a categoria pessoal original é preservada ao editar nota, review, status ou progresso. Se faltarem gêneros externos, a interface identifica o valor como **categoria legada**; ele continua disponível nos filtros.
- Com gêneros externos disponíveis, a apresentação e o filtro de gênero usam esses gêneros, conservando a categoria antiga no arquivo.
- Sem gêneros e sem categoria, aparece “Gêneros não informados”; o filtro não oferece uma categoria fictícia.
- As outras cinco mídias mantêm a categoria pessoal e os metadados dos catálogos simulados existentes. A obrigatoriedade específica de IGDB não foi aplicada a elas.

`SteamAppId`, `IgdbId`, referências de catálogo, datas, capas, plataformas e extensões JSON antigas não são convertidos, deduplicados ou enriquecidos automaticamente.

### Página, capítulo e volume

Posições novas ou modificadas de página, capítulo e volume devem ser inteiras e **maiores ou iguais a 1**. Campos vazios significam progresso não informado e continuam opcionais. O formulário informa o erro e conserva a seleção para permitir corrigir o valor.

A mesma regra protege os comandos `Adicionar` e `Atualizar` do serviço. A leitura e a persistência de cópias legadas continuam aceitando zeros anteriormente válidos: editar apenas a nota ou review de um registro com zero não pode bloquear a biblioteca nem migrar seu progresso. Um zero antigo pode ser conservado sem alteração, removido explicitamente ou substituído por uma posição positiva. Não é permitido introduzir zero onde antes não existia ou substituir uma posição positiva por zero.

Contagens de episódios assistidos, horas e minutos mantêm o significado anterior, que inclui zero. Temporada/episódio de série não receberam novas restrições nesta revisão.

### Capas e textos

A grade usa `UniformToFill`, que preenche proporcionalmente a área da capa e pode recortar suas bordas, sem deformar a imagem. Lista e sugestões conservam `Uniform`. A alteração do modo de preenchimento não provoca outro download.

O template do item da biblioteca não usa a camada de hover que interferia na capa. O hover realça a borda do cartão; seleção e ações permanecem disponíveis. O teste HV10 comparou pixels no interior da capa antes e durante o hover, além das verificações de visibilidade, fonte e opacidade.

O placeholder da review passou a falar em “obra”. A página inicial usa biblioteca/obras em saudações, convites, estados vazios e tooltip dos recentes. Seus contadores continuam calculados pelos dados locais existentes. Não foi implementado o dashboard de uma fase futura nem alterada a identidade do aplicativo.

## Compatibilidade, proteção e backup

A biblioteca pessoal continua no mesmo caminho: `backlog.json`, relativo ao diretório de trabalho do aplicativo. Nesta sessão, o arquivo é `${PROJETO}/backlog.json`, com **9 registros e 1535 bytes**.

Antes dos ajustes na validação e persistência foi criado um backup externo verificável:

`${HOME}/Backups/BacklogManager/ajustes-fase3-20261028-150858/backlog.json`

Pasta com permissão 700, arquivo com permissão 600 e manifesto `verificacao.json` ao lado. Original e backup apresentam o SHA-256:

`d33b826a57148dc8c7017ae22e15f6898163ba7d5d5eea57a7ba64b6ddaac810`

Os hashes do contrato, arquivos não rastreados originais, relatórios e backups anteriores foram conferidos novamente. Nenhum teste modificou a biblioteca pessoal. A verificação XA01 trabalhou exclusivamente em cópia externa e confirmou a igualdade semântica dos nove registros antigos após salvar novos registros e recarregar.

Não houve migração de formato, renomeação de campos ou mudança automática de caminho. A alteração de validação permite `Categoria` vazia em novos jogos IGDB, mantendo os gêneros externos separados. **O executável anterior de `feat/biblioteca` rejeita categoria vazia.** Portanto, para executar aquela versão preservada depois de testar novos jogos nesta branch, use uma cópia compatível ou o backup; não remova dados automaticamente do arquivo novo. A branch preservada não implica compatibilidade de leitura de todo novo estado por seu executável antigo.

As limitações de retorno de versões relativas a `DataAdicao` e `Plataformas`, documentadas na entrega anterior, também permanecem. Registros sem identificador externo e categorias antigas continuam carregáveis e editáveis; não há exigência de recadastro.

## C# e organização

`MainWindow.AjustesBiblioteca.cs` é outra parte da mesma classe `partial MainWindow`. Ela organiza a disponibilidade do botão e a apresentação dos gêneros; não duplica a biblioteca nem cria outra camada arquitetural.

`GenerosExibidos` é uma propriedade calculada: deriva o texto do catálogo ou da categoria antiga, sem salvar uma cópia desse texto no JSON. `ValidarPreenchimento` compara o progresso desejado com o anterior para distinguir dado legado preservado de valor novo inválido. O teste `valor is <= 0` só é verdadeiro para um valor informado nesse intervalo; `null` continua significando ausência.

A validação de comandos é separada da validação de carregamento. Isso permite impedir novos cadastros inválidos sem recusar arquivos antigos válidos. Os `record` e `with` existentes continuam conservando catálogo, IDs e outros dados durante alterações pessoais. Nenhum pacote novo foi adicionado.

## Arquivos

Criados:

- `MainWindow.AjustesBiblioteca.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/CadastroIgdbTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/CapasGradeTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/ComandosTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/GenerosLegadosTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/OfflineTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/PosicoesJanelaTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/PosicoesTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/TextosTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/TitulosTests.cs`
- `scripts/gerar_relatorio_ajustes_fase3.py`
- `docs/AJUSTES_FASE3.md`
- `docs/AJUSTES_FASE3_TESTES.md`
- `docs/resultados-ajustes-fase3.json`
- `docs/verificacoes-ajustes-fase3.json`

Modificados:

- `Controls/CapaObra.cs`, `Data/FileManager.cs`
- `MainWindow.axaml`, `MainWindow.axaml.cs`
- `Models/ItemBiblioteca.cs`, `Models/ProgressoPessoal.cs`
- `Services/FormularioObra.cs`, `Services/ServicoBiblioteca.cs`
- `Tests/BacklogManager.Tests/OperacoesTests.cs`, `Tests/BacklogManager.Tests/TelaTeste.cs`
- `Tests/BacklogManager.Tests/Fase1/FormularioJogoTests.cs`, `Tests/BacklogManager.Tests/Fase1/JanelaContratosTests.cs`
- `Tests/BacklogManager.Tests/Fase2/InterfaceMultimidiaTests.cs`
- `Tests/BacklogManager.Tests/BuscaJanelaTests.cs`
- `Tests/BacklogManager.Tests/Fase3/InteracaoBibliotecaTests.cs`, `Tests/BacklogManager.Tests/Fase3/VisualizacoesTests.cs`

São **15 criados e 16 modificados**, em relação a `bbcb99f`. Parte dos testes visuais anteriores alterou somente o destino de imagens para `/tmp/backlogmanager-ajustes-fase3-resultados/`. `TelaTeste` passou a simular também o cliente de capas por padrão; as chamadas do catálogo IGDB e das imagens são independentes, evitando acesso à rede real pelos cenários de janela.

## Testes e evidências

Compilação final: **zero erros e zero avisos**. Suíte: **851 aprovados, zero reprovados e zero não executados**, incluindo todos os **715 cenários anteriores** e **136 novos**.

| Área | IDs | Novos cenários | Aprovados |
|---|---|---:|---:|
| Cadastro IGDB, metadados e seleção válida | GC01–GC20 | 20 | 20 |
| Gêneros e compatibilidade legada | GL01–GL12 | 12 | 12 |
| Posições de página | NP01–NP12 | 12 | 12 |
| Posições de capítulo | NC01–NC15 | 15 | 15 |
| Posições de volume | NV01–NV12 | 12 | 12 |
| Comandos de cadastro e atualização | CS01–CS10 | 10 | 10 |
| Operações locais sem IGDB | OL01–OL10 | 10 | 10 |
| Preenchimento e hover das capas | HV01–HV10 | 10 | 10 |
| Textos da página inicial e reviews | TX01–TX13 | 13 | 13 |
| Títulos oficiais nas seis mídias | TC01–TC10 | 10 | 10 |
| Posições na janela e recuperação | UP01–UP12 | 12 | 12 |
| Regressão anterior | Todos os cenários anteriores | 715 | 715 |
| **Total** | | **851** | **851** |

A cobertura dos textos da página inicial tem dez cenários específicos: TX01–TX05 e TX09–TX13. Para reviews, TX06–TX08 e a regressão FO11–FO16/A13 totalizam dez cenários relevantes, incluindo texto genérico, mídias diferentes, preservação, limites e valores vazios. Os três tipos de posição têm ao menos dez cenários próprios. As outras cinco mídias foram verificadas também por TC02–TC06, GL10 e por toda a regressão multimídia anterior.

A [tabela individual](AJUSTES_FASE3_TESTES.md) apresenta resultado esperado e observado, tipo e status dos 851 cenários. [resultados-ajustes-fase3.json](resultados-ajustes-fase3.json) contém os resultados/durações gerados do TRX real. [verificacoes-ajustes-fase3.json](verificacoes-ajustes-fase3.json) registra execuções e hashes de preservação. Relatórios anteriores não foram regenerados.

| ID | Verificação adicional | Esperado | Observado | Tipo | Status |
|---|---|---|---|---|---|
| XA01 | Cópia externa da biblioteca com novos registros | Preservar os nove registros, aceitar jogo de catálogo sem categoria manual e bloquear página zero | Nove registros preservados semanticamente; jogo de catálogo simulado de teste e livro com página 1 recarregados; edição pessoal correta; tentativa de página zero não mudou o arquivo; `.bak` inicial exato | Automatizado em cópia | Aprovado |
| XA02 | Inicialização desktop sem credenciais em pasta isolada | Permanecer aberto sem gravar na cópia | Processo aberto por oito segundos e encerrado pelo verificador; log vazio; cópia intacta; preferência não criada automaticamente | Automatizado de inicialização | Aprovado |

**Planejados: 863** — 851 da suíte, duas verificações adicionais e dez manuais. **Executados: 853; aprovados: 853; reprovados atuais: zero; não executados: dez manuais.** A inicialização nativa não equivale à validação interativa no Rider ou pesquisa autenticada real.

### Execuções reais e ajustes encontrados

| Execução | Resultado | Tratamento |
|---|---|---|
| Base preservada antes das alterações | 715/715 | Referência inicial. |
| Primeira regressão | 711/715 | A07/FO09 exigiam categoria manual; UI06 esperava consulta parcial; UM22 criava capítulo zero. Expectativas adaptadas às mudanças autorizadas, sem excluir cenários. |
| Novos testes iniciais | 71/71 | Cadastro, gêneros e posições aprovados. |
| Novos testes ampliados | 121/121 | Inclui hover, títulos, textos, comandos e offline. |
| Integração com posições na janela | 847/848 | Restava uma asserção de aviso de categoria antiga em A07; corrigida para verificar sucesso sem categoria inventada. |
| Suíte ampliada | 848/848 | Todos os cenários da rodada passaram. |
| Cobertura completa dos textos iniciais | 851/851 | Dez cenários específicos da página inicial. |
| Repetição final após mensagem de status | 851/851 | Confirma que o aviso de status não pede categoria manual para jogos. |

Problemas corrigidos: áreas não preenchidas nas capas; hover interferindo na apresentação; aceitação de novas posições zero; campo mantendo o termo parcial após seleção; evento de preenchimento podendo invalidar a própria seleção; categoria manual exigida para jogos; mensagens exclusivas de jogos em áreas multimídia. As mudanças foram cobertas por novas asserções e por toda a regressão. Não foram encontrados erros relevantes pendentes na suíte final.

## Validação manual no Rider — não executada

Crie uma pasta nova, no terminal da raiz do projeto:

```bash
pasta_ajustes=$(mktemp -d /tmp/backlogmanager-ajustes-rider.XXXXXX)
cp -- backlog.json "$pasta_ajustes/backlog.json"
printf '%s\n' "$pasta_ajustes"
```

No Rider, selecione `fix/biblioteca-ajustes` e configure **Run → Edit Configurations → Working directory** com a pasta impressa. Confira que a cópia contém os registros esperados antes de testar. O caminho da biblioteca permanece relativo ao diretório de trabalho; não utilize a biblioteca original para cadastros/exclusões de teste.

Para pesquisa real, use as variáveis locais existentes `IGDB_CLIENT_ID` e `IGDB_ACESS_TOKEN` ou `IGDB_ACCESS_TOKEN`. O aplicativo não precisa delas para editar informações pessoais ou operar a biblioteca local. Para voltar à branch original preservada, use outra cópia compatível com ela, pois jogos novos desta revisão podem ter `Categoria` vazia.

| ID | Cenário manual | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| MA01 | Digitar um jogo arbitrário sem selecionar | Botão desabilitado; nenhuma inclusão possível | Aguardando proprietário | Manual / Rider | Não executado |
| MA02 | Pesquisar na IGDB real e selecionar por termo parcial | Título oficial e gêneros disponíveis preenchidos; salva com ID/metadados corretos | Aguardando proprietário | Manual / Rider | Não executado |
| MA03 | Selecionar resultado sem gêneros e alterar/limpar a pesquisa | Ausência explícita de gêneros; alteração invalida seleção; nenhum gênero fictício | Aguardando proprietário | Manual / Rider | Não executado |
| MA04 | Usar biblioteca sem IGDB e tentar novo jogo | Local permanece utilizável; aviso claro na pesquisa; novo jogo sem seleção bloqueado | Aguardando proprietário | Manual / Rider | Não executado |
| MA05 | Editar nota/review de legado sem ID ou com Steam antigo | Salva offline, conserva categoria e IDs; não exige recadastro | Aguardando proprietário | Manual / Rider | Não executado |
| MA06 | Filtrar, ordenar e excluir em grade/lista offline | Operações locais funcionam sem requisição ao catálogo | Aguardando proprietário | Manual / Rider | Não executado |
| MA07 | Informar página, capítulo e volume como zero/negativo, vazio e um | Valores novos inválidos recusados; vazio permitido; um persistido | Aguardando proprietário | Manual / Rider | Não executado |
| MA08 | Editar cópia de registro antigo com posição zero | Nota/review podem mudar sem migrar posição; limpeza explícita e correção positiva funcionam | Aguardando proprietário | Manual / Rider | Não executado |
| MA09 | Verificar preenchimento/hover e seleção das capas em 900×660 e tamanho normal | Grade preenchida proporcionalmente; hover não escurece capa; ações e seleção mantidas | Aguardando proprietário | Manual / Rider | Não executado |
| MA10 | Cadastrar nas cinco mídias simuladas e conferir início/reviews | Fluxos anteriores funcionam; título oficial mostrado; textos genéricos | Aguardando proprietário | Manual / Rider | Não executado |

As aprovações manuais gerais das fases anteriores permanecem nos documentos próprios. Nenhum resultado manual desta revisão ou aprovação individual não relatada foi inventado. A Fase 3 continua pendente de aprovação.

## Limitações e riscos

- A nova seleção foi testada com HTTP controlado, sem credenciais reais. A pesquisa IGDB autenticada deve ser conferida no Rider.
- Há cache de capas **na sessão**, sem cache persistente em disco. Ao abrir offline em uma sessão nova, capas remotas podem aparecer como “Sem capa”; metadados e operações locais continuam disponíveis. Downloads de imagens usam cliente separado e não consultam o catálogo IGDB para editar ou excluir registros.
- Uma seleção IGDB válida da sessão não é revalidada por rede ao salvar. Alterar ou limpar o texto exige nova seleção.
- `UniformToFill` pode recortar parte da imagem para preencher o retângulo; lista/sugestões preservam a imagem inteira. Isso não altera a referência da capa ou os arquivos pessoais.
- A grade continua sem virtualização, conforme a limitação anterior. Não foi feito benchmark de bibliotecas grandes.
- Zeros de posição antigos são preservados se inalterados; não são corrigidos em massa. Valores negativos antigos já eram recusados pelo formato anterior.
- A revisão anterior pode recusar os novos jogos com categoria vazia. Preserve e use cópias adequadas para comparar as branches.
- As ressalvas históricas da Fase 0 sobre M05 e restauração manual do `.bak` continuam documentadas; testes novos não foram usados para inventar aprovação manual dessas etapas.

## Reproduzir e Git

```bash
dotnet build BacklogManager.slnx --no-restore -m:1
dotnet test BacklogManager.slnx --no-restore -m:1 --logger 'trx;LogFileName=ajustes-final.trx' --results-directory /tmp/backlogmanager-ajustes-fase3-resultados
python3 -B scripts/gerar_relatorio_ajustes_fase3.py /tmp/backlogmanager-ajustes-fase3-resultados/ajustes-final.trx
```

O gerador exige presença de todos os cenários da suíte, IDs únicos e mínimo de dez por grupo. As imagens e os TRX desta execução estão em `/tmp/backlogmanager-ajustes-fase3-resultados/`; foram inspecionadas amostras de hover/preenchimento e página inicial no tamanho mínimo. A tabela e os resultados estruturados permanecem no repositório.

Os commits da revisão são informados em `git log` e na entrega. A branch original permanece em `bbcb99f`, e a `main` em `64fb4fb`. Os únicos arquivos não rastreados originais são `qodana.yaml` e `skills/system/run-workflow/SKILL.md`, preservados. Não houve merge nem push. **Aguardando revisão e autorização explícita; Fase 4 não iniciada.**

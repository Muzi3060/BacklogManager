# Fase 3 — Biblioteca

## Entrega para revisão

Implementação concluída exclusivamente em **`feat/biblioteca`**, conforme `Contrato.md` versão 1.1, especialmente as seções 6.5, 8, 16 e 19.1. O contrato não foi alterado. A aprovação definitiva da Fase 3 depende do proprietário; a Fase 4 não foi iniciada.

A aprovação oficial da Fase 2 e o relato geral de validação no Rider foram registrados em [APROVACAO_FASE2.md](APROVACAO_FASE2.md), commit `64fb4fb`. O relato não discrimina cenários: nenhum resultado manual individual foi inventado. Os relatórios antigos permanecem como registros históricos das respectivas entregas.

A `main` foi atualizada por **fast-forward**, de `56bcd58` para `64fb4fb`, incorporando `feat/multimidia` sem conflitos. As árvores foram comparadas. A branch desta fase nasceu dessa `main`. Não houve push, exclusão de branches ou merge da Fase 3.

## O que foi implementado

- Grade de capas como visualização padrão, com quebra de linha conforme a largura da janela.
- Alternância para lista com capas menores, conservando a seleção, a consulta e os botões de edição/exclusão.
- Cadastro, edição, exclusão, status adaptados por mídia, notas, reviews e progresso disponíveis nas duas visualizações e nas seis categorias. Títulos/reviews abreviados têm tooltip e o formulário permite consultar/editar os dados completos.
- Busca por título/categoria e filtro de status existentes preservados; novos filtros por gênero, nota mínima inclusiva, intervalo de adição e plataforma de jogo quando disponível. Critérios se combinam por interseção. “Limpar filtros” conserva o tipo de mídia e a visualização.
- Gêneros do catálogo têm prioridade; registros sem gêneros usam a categoria pessoal como alternativa claramente indicada no filtro. Plataforma desconhecida não é inferida dos IDs ou da categoria.
- Ordenação por cadastro, título A–Z/Z–A, maior/menor nota e adição recente/antiga. Empates preservam a ordem relativa; não há deduplicação ou reordenação do arquivo em consequência de uma consulta.
- Data de adição em UTC nos cadastros novos preparados pelo formulário. Registros antigos continuam com data desconhecida; editar um registro antigo não cria uma data artificial. O filtro considera o dia no fuso local e inclui os limites do intervalo. Ordenações de data deixam os registros sem data ao final.
- Capas nas sugestões e na biblioteca. A IGDB passa a solicitar também `cover.image_id`, `cover.url`, `genres.name` e `platforms.name`, preservando POST, endpoint, autenticação, limite, escape de texto, cancelamento e erros controlados.
- Capas simuladas locais com rótulo explícito; nenhuma API nova foi integrada. As referências locais já existentes continuam aceitas.
- Carregamento assíncrono, imagem “Sem capa”, compartilhamento de pedidos, cache limitado à sessão e tratamento de URL/conteúdo inválido, falhas HTTP, timeout e cancelamentos.
- Preferência de visualização salva separadamente da biblioteca. Falha na preferência mantém o modo escolhido na sessão e não bloqueia operações da biblioteca.
- Cartões compactos em janelas menores que 740 pixels de altura; filtros adicionais em área expansível com rolagem e rodapé de ações disponível no tamanho mínimo de 900×660.

Não foram implementados coleções, temas, nova identidade, dashboard, configuração geral, exportação/importação de backups, nuvem ou novas APIs de catálogo. A preferência de grade/lista atende à biblioteca desta fase e não antecipa a página de configurações.

## Decisões arquiteturais e C#

A arquitetura das fases anteriores foi mantida, sem novos pacotes ou framework. `MainWindow.Biblioteca.cs` é outra parte da mesma classe `partial MainWindow`: organiza os eventos desta fase, sem criar uma segunda janela ou duplicar dados.

| Componente | Responsabilidade e motivo |
|---|---|
| `ConsultaBiblioteca` e `OrdenacaoBiblioteca` | Representam os critérios e as opções de ordenação com nomes definidos; evitam espalhar números ou consultas pela interface. |
| `ServicoBiblioteca.Consultar` | Aplica os filtros e ordena uma sequência para exibição. `Where`, `OrderBy` e `ThenBy` são operações LINQ; a coleção pessoal original continua na sua ordem de armazenamento. `Filtrar` permanece como entrada compatível. |
| Um único `ListaBacklog` | Alterna somente o template e o painel. Grade usa `WrapPanel`; lista usa `VirtualizingStackPanel`. Edição e exclusão continuam usando o mesmo serviço. A seleção é restaurada pela instância, preservando duplicatas iguais. |
| `CapaObra` | Controle visual reutilizado nas sugestões e nas duas visualizações. Cancela sua espera ao sair da tela ou mudar o endereço; uma resposta antiga não troca a nova imagem. |
| `CarregadorCapas` | Faz pedidos de imagens sem autenticação, compartilha tarefas e decodifica bitmaps fora da thread da interface. Desenhos nativos Avalonia são criados na thread da interface. A janela encerra seus recursos. |
| `ArmazenamentoPreferenciasBiblioteca` | Escreve somente a preferência de visualização, usando temporário, flush e substituição do arquivo. O caminho real é escolhido na composição; testes de janela usam memória ou pasta temporária. |
| Conversor JSON existente | Reconhece os dois novos campos opcionais do bloco multimídia e conserva IDs, progresso, campos legados e extensões anteriores. |

`Task` representa um trabalho ainda em andamento; `await` permite esperar sem travar a tela. `CancellationToken` comunica cancelamento. Cada cartão cancela sua própria espera; o download compartilhado continua enquanto a janela estiver aberta, respeitando o prazo de dez segundos. `SemaphoreSlim` limita a quatro downloads simultâneos. O prazo inclui fila e corpo da resposta, pois `ResponseHeadersRead` encerra o timeout do `HttpClient` nos cabeçalhos.

O cache mantém até 128 tarefas/referências. Quando precisa de espaço, remove uma entrada concluída; a imagem ainda exibida por um cartão permanece válida. Referências fracas permitem acompanhar os bitmaps sem mantê-los vivos apenas para descarte. A janela descarta os bitmaps vivos ao encerrar. Não há cache em disco.

A decodificação aceita até 5 MiB, usa largura de 320 pixels e rejeita resultado com altura superior a 4096 pixels. O cliente padrão não segue redirecionamentos. São aceitas imagens HTTPS do caminho de imagens de `images.igdb.com` e as cinco referências simuladas conhecidas. As capas simuladas são desenhos vetoriais nativos associados às referências existentes; não foi acrescentado um interpretador SVG genérico. Outros endereços resultam em “Sem capa”.

A estrutura das URLs e o tamanho `cover_big` seguem a [documentação oficial da IGDB](https://api-docs.igdb.com/#images). O uso dos painéis segue a [documentação do WrapPanel](https://docs.avaloniaui.net/controls/layout/panels/wrappanel) e do [ItemsControl](https://docs.avaloniaui.net/controls/data-display/collections/itemscontrol). As garantias de funcionamento descritas nesta entrega foram verificadas pelos testes do projeto; a documentação externa não substitui essas execuções.

## Dados, compatibilidade e backup

O caminho de persistência **não mudou**: `backlog.json`, relativo ao diretório de trabalho do processo. Na sessão de implementação, o arquivo pessoal é `${PROJETO}/backlog.json`.

O estado encontrado nesta fase tem **9 registros e 1535 bytes**. Não foi restaurado o backup histórico de 11 registros da Fase 0 nem tentado reconciliar bibliotecas diferentes.

Antes das mudanças na persistência foi criada e conferida uma cópia externa em:

`${HOME}/Backups/BacklogManager/fase3-20261028-103358/backlog.json`

A pasta tem permissão 700; a cópia tem permissão 600. `verificacao.json`, ao lado, registra quantidade, tamanho e SHA-256. Original e backup têm o mesmo hash:

`d33b826a57148dc8c7017ae22e15f6898163ba7d5d5eea57a7ba64b6ddaac810`

Foram conferidos novamente os hashes da biblioteca, `Contrato.md`, `qodana.yaml`, dos relatórios anteriores e dos seis arquivos de backup/manifesto preexistentes. Todos foram preservados. A evidência está em [verificacoes-fase3.json](verificacoes-fase3.json), que registra também os hashes dos TRX desta fase.

Os novos metadados ficam no bloco `Multimidia` de versão 1:

- `DataAdicao`: opcional; ausente quando desconhecida.
- `Plataformas`: opcional; omitida quando vazia; exclusiva de jogos.

A Fase 3 lê os registros legados e os registros multimídia da Fase 2. Não há migração automática ao abrir, nem atualização em massa de datas, gêneros ou capas. A pesquisa nova traz metadados para novos cadastros; registros existentes não são enriquecidos automaticamente. `SteamAppId` permanece um valor legado opaco; não é convertido em `IgdbId` nem usado para deduplicar. A compatibilidade com `IGDB_ACESS_TOKEN` e `IGDB_ACCESS_TOKEN` foi mantida.

**Retorno a versões antigas:** o executável da Fase 2 rejeita campos desconhecidos dentro do bloco multimídia. Depois de salvar registros com `DataAdicao` ou `Plataformas`, a Fase 2 pode bloquear a abertura/gravação desse arquivo por segurança. Para retornar à versão antiga, use uma cópia compatível ou o backup verificado; não remova campos da biblioteca pessoal automaticamente. A versão 1 foi conservada porque os campos são opcionais e os campos anteriores mantêm a mesma interpretação; não se promete leitura desses novos campos por executáveis anteriores.

Na execução normal, a preferência fica em `biblioteca.preferencias.json`, ao lado do JSON usado pela biblioteca. O arquivo está no `.gitignore`. A composição impede colisão caso uma biblioteca explicitamente configurada tenha esse mesmo nome. Ler ou salvar preferências não lê nem regrava a biblioteca. Somente o modo grade/lista é persistido; filtros e ordenação são escolhas da sessão.

## Arquivos da Fase 3

Criados:

- `Controls/CapaObra.cs`
- `Data/ArmazenamentoPreferenciasBiblioteca.cs`
- `MainWindow.Biblioteca.cs`
- `Models/ConsultaBiblioteca.cs`
- `Services/CarregadorCapas.cs`
- `Tests/BacklogManager.Tests/Fase3/CapasTests.cs`
- `Tests/BacklogManager.Tests/Fase3/ConsultasTests.cs`
- `Tests/BacklogManager.Tests/Fase3/InteracaoBibliotecaTests.cs`
- `Tests/BacklogManager.Tests/Fase3/MetadadosBibliotecaTests.cs`
- `Tests/BacklogManager.Tests/Fase3/MetadadosIgdbTests.cs`
- `Tests/BacklogManager.Tests/Fase3/PreferenciasTests.cs`
- `Tests/BacklogManager.Tests/Fase3/VisualizacoesTests.cs`
- `scripts/gerar_relatorio_fase3.py`
- `docs/FASE3.md`
- `docs/FASE3_TESTES.md`
- `docs/resultados-fase3.json`
- `docs/verificacoes-fase3.json`

Modificados:

- `.gitignore`
- `Configuracao/ComposicaoAplicativo.cs`
- `Data/ConversorBibliotecaJson.cs`
- `MainWindow.Multimidia.cs`, `MainWindow.axaml`, `MainWindow.axaml.cs`
- `Models/DadosMultimidiaJson.cs`, `Models/IgdbGame.cs`, `Models/ItemBiblioteca.cs`, `Models/ObraCatalogo.cs`
- `Services/FormularioObra.cs`, `Services/IgdbService.cs`, `Services/PesquisaJogosIgdb.cs`, `Services/ServicoBiblioteca.cs`
- `Tests/BacklogManager.Tests/TelaTeste.cs`
- `Tests/BacklogManager.Tests/IgdbTests.cs`
- `Tests/BacklogManager.Tests/BuscaJanelaTests.cs`
- `Tests/BacklogManager.Tests/Fase2/InterfaceMultimidiaTests.cs`

São **17 arquivos criados e 18 modificados** na branch da Fase 3. Os dois testes visuais anteriores mudaram somente o destino de capturas para a pasta de artefatos desta fase; seus resultados históricos não foram sobrescritos. `IgdbTests` mantém as verificações anteriores com a consulta expandida. `TelaTeste` permite injetar preferências/capas isoladas. `APROVACAO_FASE2.md` foi criado na etapa anterior e integrado à `main` em `64fb4fb`, portanto não faz parte dessa contagem.

## Testes executados

Compilação final: **sucesso, zero erros e zero avisos**. Suíte final: **715 aprovados, zero reprovados e zero não executados**, incluindo os **535 cenários anteriores** e **180 novos**. Cada área nova/modificada tem pelo menos dez cenários relevantes; datas e plataformas têm dez cenários de compatibilidade cada dentro de DB01–DB20, além dos filtros e fluxos de interface.

| Área | IDs novos | Cenários | Aprovados |
|---|---|---:|---:|
| Consulta e filtros combinados | QB01–QB34 | 34 | 34 |
| Ordenação estável | OB01–OB12 | 12 | 12 |
| Datas e compatibilidade | DB01–DB10 | 10 | 10 |
| Plataformas e compatibilidade | DB11–DB20 | 10 | 10 |
| Preferência de visualização | PV01–PV16 | 16 | 16 |
| Metadados opcionais IGDB | CI01–CI15 | 15 | 15 |
| Carregamento, cache, limites e falhas das capas | CP01–CP35 | 35 | 35 |
| Operações em grade | VG01–VG13 | 13 | 13 |
| Operações em lista | VL01–VL13 | 13 | 13 |
| Interação de filtros, preferências e capas | IB01–IB22 | 22 | 22 |
| Regressão das fases anteriores | Todos os IDs anteriores | 535 | 535 |
| **Total da suíte** | | **715** | **715** |

A [tabela individual completa](FASE3_TESTES.md) contém cenário, resultado esperado, observado, tipo e status dos 715 testes. Os dados estruturados estão em [resultados-fase3.json](resultados-fase3.json). Foram gerados do TRX final real por `scripts/gerar_relatorio_fase3.py`; os relatórios das fases 0, 1 e 2 não foram regenerados. H01 e Q04–Q10 foram adaptados aos campos novos da consulta IGDB, conservando escape, limite, endpoint e autenticação.

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| X301 | Persistência na cópia externa temporária dos dados pessoais | Preservar os nove registros; permitir metadados novos e mídia simulada | Nove registros mantidos semanticamente; jogo com capa/plataforma/data e livro com página 30 adicionados; edição e recarga corretas; preferência separada; `.bak` inicial idêntico aos bytes da cópia inicial | Automatizado adicional em cópia | Aprovado |
| X302 | Inicialização desktop sem credenciais, em pasta temporária | Abrir sem gravação automática ou falha | Processo permaneceu aberto por oito segundos e foi encerrado pelo verificador; log vazio; cópia intacta; preferência não criada automaticamente | Automatizado adicional de inicialização | Aprovado |

**Totais planejados: 727** — 715 na suíte, duas verificações adicionais e dez validações manuais. **Executados: 717; aprovados: 717; reprovados atuais: zero; não executados: dez manuais.** O teste de inicialização não equivale a interação manual no Rider. Todas as gravações de teste ocorreram em pastas temporárias, com dados simulados ou cópias.

Artefatos desta execução: `/tmp/backlogmanager-fase3-resultados/`. A pasta `visualizacoes` contém 13 capturas novas: seis categorias em grade, seis em lista e filtros expandidos no tamanho mínimo. Amostras de grade/lista de livros e de filtros expandidos foram inspecionadas visualmente. As capturas de regressão também foram geradas em subpastas desta fase. As imagens não demonstram desempenho de bibliotecas grandes nem chamada autenticada real.

### Execuções e correções

| Execução real | Resultado | Interpretação / correção |
|---|---|---|
| Base, antes das mudanças | 535/535 | Referência de regressão preservada. |
| Primeira regressão | 526/535 | Um problema de thread nas capas simuladas; oito expectativas da consulta IGDB precisavam refletir os campos novos. |
| Novos testes parciais | 122/124 | Decodificador Skia lançou `NullReferenceException` para bytes inválidos; DB09 esperava a exceção interna em vez do erro controlado de `FileManager`. |
| Novos testes com grade/lista | 150/150 | Correções anteriores confirmadas. |
| Primeira suíte ampliada | 700/703 | Três verificações de interface consultavam o estado antes de os eventos de texto/data serem processados. |
| Repetição das interações | 18/18 | Testes passaram a aguardar os eventos do dispatcher, conforme o padrão dos testes anteriores; não foi necessário alterar o comportamento desses filtros. |
| Suíte ampliada completa | 712/712 | Inclui corrida de capas, cache limitado, cópia/seleção e preferência. |
| Suíte final, com prazo integral de imagem | 715/715 | Inclui corpo sem término, espera de cabeçalhos e prazo inválido; regressão completa repetida. |

Bugs de implementação corrigidos: criação de desenhos Avalonia na thread errada; falta de tratamento da exceção do decodificador para bytes inválidos. A revisão também identificou e preveniu espera indefinida do corpo de imagem e colisão excepcional entre caminho da preferência e nome da biblioteca. As evidências específicas estão em CP11, CP17, CP33–CP35, PV16 e nos testes da interface. O cartão compacto foi ajustado após inspeção visual para conservar a leitura dos dados no tamanho mínimo.

## Validação manual no Rider — pendente

Prepare uma pasta nova para esta validação, no terminal aberto na raiz do projeto:

```bash
pasta_fase3=$(mktemp -d /tmp/backlogmanager-fase3-rider.XXXXXX)
cp -- backlog.json "$pasta_fase3/backlog.json"
printf '%s\n' "$pasta_fase3"
```

No Rider, confirme `feat/biblioteca`, compile a solução e abra **Run → Edit Configurations**. Na configuração do aplicativo, defina **Working directory** como o caminho impresso. Assim, `FileManager` usará a cópia. Confira esse caminho antes de gravar qualquer teste. O executável não recebe um caminho de biblioteca por argumento de linha de comando.

Para IGDB real, mantenha as variáveis de ambiente da configuração local: `IGDB_CLIENT_ID` e `IGDB_ACESS_TOKEN` ou `IGDB_ACCESS_TOKEN`. A preferência criada durante os testes deve aparecer na mesma pasta temporária. Para testar o padrão novamente, use outra pasta nova ou remova apenas a preferência da pasta de testes, com o aplicativo fechado.

| ID | Cenário manual planejado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| M301 | Abrir cópia sem preferência e visitar as seis categorias | Grade padrão; dados antigos e estados vazios corretos | Aguardando validação do proprietário | Manual / Rider | Não executado |
| M302 | Alternar grade/lista com seleção e filtros, fechar e reabrir | Seleção conservada durante alternância; modo recuperado ao reiniciar | Aguardando validação do proprietário | Manual / Rider | Não executado |
| M303 | Pesquisar jogo real na IGDB, selecionar, cadastrar e reiniciar | Capa nas sugestões/biblioteca; gêneros e plataformas quando retornados; ID e metadados persistidos | Aguardando validação do proprietário | Manual / Rider | Não executado |
| M304 | Cadastrar exemplos de todas as mídias nas duas visualizações | Cadastro e progresso apropriado recarregáveis; simulação claramente indicada | Aguardando validação do proprietário | Manual / Rider | Não executado |
| M305 | Editar nota, review e progresso em grade/lista, incluindo legado | Alterações persistem; Steam original preservado; tipo, capa e data original mantidos | Aguardando validação do proprietário | Manual / Rider | Não executado |
| M306 | Excluir uma obra selecionada em cada modo, incluindo cópias iguais | Somente a instância selecionada é removida; demais registros persistem | Aguardando validação do proprietário | Manual / Rider | Não executado |
| M307 | Combinar busca, status, gênero, plataforma e nota; limpar | Interseção correta; plataforma desabilitada sem dados; limpeza restaura resultados | Aguardando validação do proprietário | Manual / Rider | Não executado |
| M308 | Filtrar data e alternar as sete ordenações | Limites inclusivos; aviso para intervalo invertido; legado sem data mantido fora de filtros de data | Aguardando validação do proprietário | Manual / Rider | Não executado |
| M309 | Usar cópia sem rede ou com capa ausente e simular preferência sem acesso | Biblioteca local continua; imagem padrão; falha de preferência não bloqueia edição | Aguardando validação do proprietário | Manual / Rider | Não executado |
| M310 | Reduzir para 900×660 e navegar/editar em ambos os modos | Controles e dados legíveis; filtros têm rolagem; recolher “Mais filtros” amplia os cartões | Aguardando validação do proprietário | Manual / Rider | Não executado |

Não foram atribuídos resultados manuais à Fase 3. A aprovação geral da Fase 2 permanece registrada separadamente. As ressalvas históricas da Fase 0 — M05 não executado e restauração efetiva do `.bak` não testada manualmente em M10 — continuam em [FASE0.md](FASE0.md); não foram convertidas em novas aprovações.

## Limitações e riscos restantes

- **IGDB autenticada e CDN reais:** a nova consulta e o carregamento foram validados com HTTP controlado. A configuração real deve ser conferida no Rider; não se afirma nova execução autenticada nesta fase.
- **Metadados ausentes:** registros antigos não ganham capas/gêneros/plataformas/data por adivinhação ou consulta automática. Data desconhecida fica fora somente quando há filtro de data ativo.
- **Compatibilidade de retorno:** executáveis anteriores podem recusar os dois campos novos. Use cópia compatível/backup para voltar de versão.
- **Bibliotecas grandes:** a lista usa painel virtualizado; a grade com `WrapPanel` não é virtualizada. Não foi feito benchmark com milhares de obras. O cache tem limite de entradas, mas imagens ainda exibidas nos cartões também consomem memória.
- **Cache da sessão:** entradas de falha também podem ser reutilizadas até serem substituídas ou a janela ser reaberta; não há atualização periódica ou cache em disco. Um pico com 128 carregamentos ainda pendentes pode deixar novos cartões temporariamente sem capa. A cobertura inclui limite, concorrência e descarte; não inclui teste de longa duração.
- **Origens das capas:** somente CDN IGDB e referências simuladas conhecidas são carregadas nesta fase. Outros provedores, arquivos pessoais e SVG genérico usam a imagem padrão.
- **Datas locais:** a data armazenada é um instante com offset; mudar de fuso pode alterar o dia usado no filtro. Isso não muda o instante persistido.
- **Tela mínima:** filtros expandidos reduzem a área dos cartões, que permanece rolável; recolher o painel aumenta a área disponível. O texto completo continua no tooltip/formulário. Temas, fontes personalizadas e outros sistemas operacionais não foram validados nesta fase.

Nenhuma falha relevante permanece na suíte final. A validação manual do proprietário e a conferência real da IGDB continuam pendentes.

## Como reproduzir e estado Git

```bash
dotnet build BacklogManager.slnx --no-restore -m:1
dotnet test BacklogManager.slnx --no-restore -m:1 --logger 'trx;LogFileName=fase3-final.trx' --results-directory /tmp/backlogmanager-fase3-resultados
python3 -B scripts/gerar_relatorio_fase3.py /tmp/backlogmanager-fase3-resultados/fase3-final.trx
```

Os comandos usam os pacotes já restaurados. O gerador escreve somente os relatórios da Fase 3 e exige que todos os cenários planejados da suíte estejam no TRX, sem IDs repetidos e com pelo menos dez cenários por grupo. A aplicação comum deve ser executada no diretório de cópia isolada descrito acima para testes manuais.

Commits:

- `64fb4fb` — registro da aprovação geral da Fase 2, integrado à `main` por fast-forward.
- `79fa9d6` — implementação da biblioteca, filtros, capas e preferência.
- `2ecef87` — 180 cenários novos, regressão e gerador de relatórios.
- Commit de documentação desta entrega: contém este relatório, a tabela individual e as evidências estruturadas; sua identificação é informada na entrega e em `git log`.

A `main` permanece em `64fb4fb`; `feat/biblioteca` contém os commits da Fase 3. O único arquivo pessoal deixado não rastreado é `qodana.yaml`, preservado e fora dos commits. `backlog.json`, a preferência local e arquivos locais de ferramentas permanecem ignorados conforme as regras do repositório. Nenhum push foi executado. **Não houve merge da Fase 3 nem início da Fase 4. Aguardando revisão e aprovação explícita do proprietário.**

# Fase 4 — Coleções

Implementação pronta para revisão na branch `feat/colecoes`, conforme as seções 10, 16, 18, 19 e 19.1 do `Contrato.md` versão 1.1. A aprovação definitiva da Fase 4 permanece pendente do proprietário. Não houve push, merge da Fase 4 ou início da Fase 5.

## Integração autorizada da Fase 3

A aprovação geral relatada pelo proprietário após verificações no Rider está em [APROVACAO_FASE3.md](APROVACAO_FASE3.md), sem atribuir resultados a cenários individuais não discriminados. O registro foi commitado em `8c9d147` na `fix/biblioteca-ajustes`.

A `main` avançou por **fast-forward** de `64fb4fb` para `8c9d147`, sem conflitos. Sua árvore ficou idêntica à de `fix/biblioteca-ajustes`. A `feat/colecoes` foi criada dessa base. A branch `feat/biblioteca` continua em `bbcb99f`; as demais branches anteriores foram preservadas. Não foi necessário descartar, guardar em stash ou incorporar arquivos pessoais.

## Funcionalidades entregues

- Página **Coleções**, independente das seis categorias oficiais e utilizável offline.
- Criar coleções com nome e descrição opcional; renomear e editar sem perder associações. Nomes de 1 a 120 caracteres, sem caracteres de controle, únicos sem distinguir maiúsculas/minúsculas; descrição de até 1000 caracteres. Espaços externos são removidos.
- Excluir com confirmação e opção de cancelar, sem excluir obras da biblioteca nem de outras coleções.
- Selecionar uma obra já cadastrada na biblioteca e adicioná-la a várias coleções. Repetir a mesma associação não duplica a obra nem o vínculo.
- Remover uma associação preservando a obra e seus outros vínculos.
- Exibir todas as mídias de uma coleção com o template de lista, capas/fallback, nota, review, seleção e foco já usados na biblioteca. A edição dos dados pessoais abre o formulário existente e reflete nas coleções.
- Recarregar nomes, descrições e associações ao reiniciar. Arquivos inválidos e conflitos externos bloqueiam a gravação afetada e apresentam aviso.
- Rolagem da página e dos itens para acesso aos controles no tamanho mínimo de 900×660. O seletor de obras mostra título, mídia, status e nota; a review aparece no tooltip, ajudando a distinguir registros parecidos.

Nenhuma consulta de catálogo é necessária para organizar obras existentes. O cadastro de novos jogos continua exigindo seleção válida da IGDB; as outras cinco mídias continuam com seus catálogos simulados identificados como tais. Não foram implementados temas, novo nome, dashboard, nuvem ou ferramentas de importação/exportação de fases posteriores.

## Decisões arquiteturais e conceitos C#

`IArmazenamentoColecoes` define apenas carregar/salvar e descrever a origem. `ServicoColecoes` concentra as regras; `ArmazenamentoColecoesJson` cuida dos arquivos. A interface chama o serviço, sem acessar diretamente o `FileManager`. A composição injeta os serviços usando a mesma instância de `ServicoBiblioteca`. Não foram adicionados pacotes.

`ColecaoPersonalizada` é um `record` com identificador, nome, descrição e uma lista de identificadores de obras. Ela guarda referências lógicas, não cópias completas das obras. A consulta resolve os identificadores contra a biblioteca atual; assim, uma edição pessoal aparece em todas as coleções.

Cada obra pode receber `Guid? IdInterno`: `Guid` é um identificador gerado localmente; `?` permite ausência nos registros antigos. Título e identificadores externos não servem para identificar a cópia pessoal, pois podem existir registros repetidos legítimos. `SteamAppId`, `IgdbId` e os identificadores dos demais catálogos continuam com seus valores e significados anteriores.

O ID é atribuído **somente na primeira associação**, usando `with` para criar o registro atualizado. Abrir o aplicativo, criar ou editar uma coleção não provoca migração em massa. O serviço publica a nova lista em memória apenas depois de salvar. As listas armazenadas pelo serviço são cópias de leitura, impedindo que uma tela altere associações sem passar pela validação.

A biblioteca continua localizando instâncias legadas por referência. Depois da atribuição do ID, também reconhece essa identidade estável e a instância anterior guardada por um formulário aberto. A edição usa o registro atual da biblioteca para não perder o ID quando o formulário foi aberto antes da associação.

## Persistência e compatibilidade

O caminho da biblioteca permanece o anterior: `backlog.json` relativo ao diretório de trabalho, resolvido na composição. Não houve transferência para outra pasta. No diretório de trabalho deste repositório, corresponde a `${PROJETO}/backlog.json`.

As coleções usam **`backlog.colecoes.json` na mesma pasta**. Se a composição receber outro nome de biblioteca, usa seu nome-base mais `.colecoes.json`, por exemplo `minha.biblioteca.json` → `minha.biblioteca.colecoes.json`. Preferências continuam separadas em seu arquivo anterior. Uma pasta inexistente não é criada automaticamente pela gravação: o erro é informado.

O arquivo de biblioteca mantém a raiz como lista e os campos multimídia da Fase 2. `IdInterno` é opcional e omitido quando nulo. O conversor preserva campos legados, metadados desconhecidos, progresso antigo, datas, categorias e dados de catálogo. Identificadores internos inválidos ou repetidos são recusados sem excluir ou deduplicar registros.

Formato das coleções (exemplo inteiramente fictício):

```json
{
  "Versao": 1,
  "Colecoes": [
    {
      "Id": "3d9fd765-87be-4548-8a77-cc9ed63b1772",
      "Nome": "Favoritos",
      "Descricao": "Obras para revisitar",
      "Obras": ["8cf104fe-6192-42a2-9f1b-c3d825b949ea"]
    }
  ]
}
```

Arquivo ausente representa nenhuma coleção e não é criado apenas ao abrir. Conteúdo inválido, versão desconhecida, campos inesperados, nomes/IDs repetidos e vínculos inválidos bloqueiam a gravação; não são normalizados descartando dados. A escrita usa arquivo temporário, flush, substituição atômica, `.bak`, bloqueio `.lock` e comparação de hash para detectar mudanças externas.

**Limite entre os dois arquivos:** na primeira associação, a biblioteca recebe o ID antes de gravar o vínculo nas coleções. Não existe transação conjunta. Se a segunda gravação falhar, o ID seguro permanece na biblioteca, a associação não é publicada e o usuário pode tentar novamente; a janela é atualizada para esse estado. A edição e a exclusão de coleções não gravam a biblioteca. Uma falha no carregamento das coleções não impede a edição pessoal na biblioteca válida.

Quando uma obra é excluída da biblioteca, seus IDs nas coleções são preservados para recuperação, ficam fora da lista de obras e geram aviso de referências ausentes. Não há reassociação por título ou ID de catálogo. Recriar a obra não herda esses vínculos. A restauração de um par consistente de arquivos recupera associações. Não há limpeza automática de referências ausentes nem controle específico para removê-las individualmente pela interface nesta entrega.

**Backup/recuperação:** feche o aplicativo e copie os dois arquivos correspondentes juntos para preservar um estado consistente. Os `.bak` são versões anteriores independentes de cada arquivo e não garantem um par coerente. Não substitua arquivos enquanto a janela estiver aberta; reabra após uma alteração externa. A interface de backup pertence à Fase 7 e não foi antecipada.

## Proteção dos arquivos pessoais

Antes das alterações de persistência, foi criado e verificado o backup externo:

`${HOME}/Backups/BacklogManager/fase4-20261028-164306/backlog.json`

A biblioteca encontrada no início desta execução tem **9 registros, 1535 bytes**, SHA-256 `d33b826a57148dc8c7017ae22e15f6898163ba7d5d5eea57a7ba64b6ddaac810`. O original e o backup continuam com esse mesmo hash. Nenhum teste usou o original ou esse backup. Não se atribui a este estado a quantidade histórica de 11 registros mencionada em fases anteriores.

A conferência por hash preservou os 33 arquivos registrados no início, incluindo contrato, biblioteca, relatórios anteriores, `qodana.yaml` e `skills/system/run-workflow/SKILL.md`, além dos 12 arquivos de backups anteriores. Os dois últimos arquivos não rastreados continuam fora dos commits. O novo sidecar pessoal e seu `.lock` foram incluídos no `.gitignore`; temporários e `.bak` já eram ignorados. Evidências: [verificacoes-fase4.json](verificacoes-fase4.json).

## Testes executados

As tabelas individuais com cenário, esperado, observado, tipo e resultado estão em [FASE4_TESTES.md](FASE4_TESTES.md); o arquivo [resultados-fase4.json](resultados-fase4.json) contém os mesmos resultados estruturados. Foram gerados do TRX real, sem substituir relatórios históricos.

| Grupo novo | Funcionalidade / comportamento | Executados e aprovados |
|---|---|---:|
| CC | Criar coleções, nomes, descrição, limites e falhas | 14 |
| CE | Renomear e editar sem perder vínculos | 12 |
| CD | Excluir preservando obras e outras coleções | 12 |
| AC | Associar, várias coleções, duplicatas e falhas | 14 |
| RA | Remover associações preservando obras | 12 |
| LC | Consultar, todas as mídias, atualização e referências ausentes | 12 |
| GI | Identidade interna, compatibilidade e edição | 20 |
| JS | JSON de coleções, integridade, concorrência e recuperação | 24 |
| UC | Interface, confirmação, bloqueios e tamanho mínimo | 24 |
| IX | Integração entre arquivos, composição e legado | 12 |
| **Novos** | **Mínimo de dez em cada grupo** | **156** |
| Regressão | Todos os cenários anteriores das Fases 0 a 3 e ajustes | 949 |
| **Suíte completa** | **1.105 planejados/executados; 0 reprovados; 0 não executados** | **1.105** |

Os testes usam dados simulados, arquivos temporários, falhas controladas e HTTP simulado. A cobertura inclui gravação sem carga prévia, arquivo truncado, versão futura, bloqueio concorrente, mudança externa, erro na criação do backup, retry, retomada, registros duplicados, seis mídias, edição aberta antes da associação, legados sem IDs externos, metadados desconhecidos e reinício. IX06 simula a preservação por um serializer anterior com `JsonExtensionData`; não executa binários antigos.

Rodadas reais, sem somar repetições como cenários novos:

| TRX | Total | Aprovados | Reprovados | Não executados |
|---|---:|---:|---:|---:|
| fase4-base.trx | 949 | 949 | 0 | 0 |
| fase4-regressao-inicial.trx | 949 | 949 | 0 | 0 |
| fase4-servicos-inicial.trx | 52 | 52 | 0 | 0 |
| fase4-modelos-persistencia.trx | 120 | 120 | 0 | 0 |
| fase4-interface.trx | 144 | 144 | 0 | 0 |
| fase4-integracao.trx | 1105 | 1105 | 0 | 0 |

Artefatos nesta máquina: `/tmp/backlogmanager-fase4-resultados`. A regressão mantém os destinos temporários de capturas definidos pelos testes antigos; os relatórios versionados anteriores não foram alterados. TRX em `/tmp` são efêmeros; os resultados individuais e hashes foram registrados nos arquivos versionados.

Checagens adicionais:

| ID | Cenário | Esperado | Observado | Tipo | Status |
|---|---|---|---|---|---|
| XS01 | Inicialização desktop com par de JSONs fictícios e sem credenciais IGDB | Processo ativo por oito segundos, sem erros no log nem alterações dos JSONs | Processo permaneceu ativo, log vazio e ambos os hashes iguais; encerrado pelo script ao fim | Smoke automatizado, sem interação | Aprovado |

`dotnet build --no-restore`: **0 erros e 0 avisos**. `git diff --check`: sem problemas. Inspeção das capturas Headless/Skia `imagens/colecoes-topo.png` e `imagens/colecoes-obras.png`: controles e lista acessíveis pela rolagem em 900×660. Não é uma aprovação manual no Rider.

Para repetir a suíte e gerar uma nova tabela após uma execução real:

```bash
dotnet build
dotnet test --logger 'trx;LogFileName=fase4-integracao.trx' --results-directory /tmp/backlogmanager-fase4-resultados
python3 -B scripts/gerar_relatorio_fase4.py /tmp/backlogmanager-fase4-resultados/fase4-integracao.trx
```

## Problemas e respectivas proteções

- O modelo anterior não tinha identidade interna persistente para distinguir duas cópias pessoais do mesmo título/ID externo. Foi acrescentado ID opcional por obra, atribuído na primeira associação; GI, AC, IX e UC20 verificam a preservação de duplicatas.
- Atribuir ID substitui o `record` carregado. Um formulário aberto poderia guardar a instância anterior e perder sua referência ou apagar o novo ID ao editar. O serviço reconhece a instância anterior e atualiza o registro corrente, validado em GI e UC24.
- A gravação de dois arquivos pode falhar após o primeiro. O serviço conserva a identidade, publica vínculos somente após persistir e a janela permite retry coerente: AC, JS, IX07 e UC19 passaram.
- Um arquivo de coleções inválido poderia afetar toda a biblioteca. O carregamento e o aviso são independentes; IX08 e UC16 confirmam a edição pessoal sem sobrescrever o arquivo inválido.
- Durante a inclusão dos últimos testes houve erro de compilação CS0102: método e classe auxiliar tinham o mesmo nome. O método foi renomeado e a suíte completa executada com sucesso. Uma tentativa de formatação referenciou `.sln`, inexistente; foi repetida na solução `.slnx` existente, sem alteração funcional. Não foram ocultadas reprovações de testes: as rodadas TRX listadas passaram.

## Limitações e riscos pendentes

- Revisão manual da **Fase 4** no Rider ainda não realizada pelo proprietário. Os 12 cenários abaixo estão **não executados**, pois dependem dessa revisão; não são incluídos nos 1.105 testes da suíte. Somando suíte, smoke e roteiro: 1.118 planejados, 1.106 executados/aprovados, 0 reprovados, 12 não executados.
- Não houve nova chamada autenticada à IGDB nem validação com as APIs reais das outras mídias. A regressão de HTTP/catálogos usa simulações; coleções não mudam seus adaptadores.
- Biblioteca e coleções são dois arquivos, com a limitação de gravação/backup conjunto descrita acima. Não há transação multiarquivo, sincronização em nuvem ou restauração automática dos dois `.bak`.
- Obras excluídas deixam referências recuperáveis avisadas, sem limpeza automática. Registros exatamente iguais continuam distintos; se título, tipo, status, nota e review forem todos iguais, o seletor ainda pode exigir atenção à ordem das entradas.
- Os caminhos continuam dependendo do diretório de trabalho usado ao abrir o aplicativo. Use a mesma pasta ao reiniciar e preserve o par de arquivos.
- Capas remotas dependem do carregador e do cache em memória existente; não foi implementado cache persistente em disco para garantir imagens offline após reiniciar. O fallback permanece disponível.
- Não houve medição com bibliotecas muito grandes ou teste manual com leitor de tela. Os controles padrão, foco e seleção foram mantidos; confirme navegação por teclado no Rider.

## Roteiro manual no Rider — pendente

Antes de executar, crie uma pasta temporária com uma **cópia isolada** da biblioteca ou dados fictícios. Configure essa pasta em **Run/Debug Configuration → Working directory**. Confirme que o caminho não é o repositório nem a pasta original. Se for copiar coleções existentes, copie também a biblioteca correspondente. Não use a pasta dos backups pessoais como ambiente de testes. Execute a configuração do projeto `BacklogManager`, na branch `feat/colecoes`.

Todos os cenários têm tipo **Manual** e resultado observado **Não executado — aguardando proprietário**:

| ID | Cenário | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| M01 | Abrir sem arquivo de coleções e navegar para Coleções | Estado vazio, biblioteca intacta e criação disponível | Aguardando validação do proprietário | Manual | Não executado |
| M02 | Criar “Favoritos” e uma descrição; tentar nome vazio e repetido com outra capitalização | Coleção válida criada; inválidos avisados sem duplicar | Aguardando validação do proprietário | Manual | Não executado |
| M03 | Renomear e editar descrição de uma coleção com obras | Novo nome/descrição; mesmos vínculos | Aguardando validação do proprietário | Manual | Não executado |
| M04 | Adicionar obra legada e repeti-la na mesma coleção | Uma obra e um vínculo; ID interno sem alterar Steam/IGDB/categoria | Aguardando validação do proprietário | Manual | Não executado |
| M05 | Associar a mesma obra a duas coleções | Ambas exibem a obra, sem duplicação na biblioteca | Aguardando validação do proprietário | Manual | Não executado |
| M06 | Usar coleção com jogo, anime, série, filme, mangá e livro existentes | As seis mídias aparecem juntas; filtros oficiais da biblioteca continuam independentes | Aguardando validação do proprietário | Manual | Não executado |
| M07 | Editar nota, review e progresso pela coleção | Formulário existente e persistência; mudança refletida nas coleções | Aguardando validação do proprietário | Manual | Não executado |
| M08 | Remover obra de uma das coleções | Obra continua na biblioteca e na outra coleção | Aguardando validação do proprietário | Manual | Não executado |
| M09 | Pedir exclusão, cancelar e depois confirmar | Cancelar preserva; confirmar exclui somente a coleção | Aguardando validação do proprietário | Manual | Não executado |
| M10 | Fechar e reabrir com o mesmo diretório de trabalho | Nomes, descrições, obras e associações recarregam | Aguardando validação do proprietário | Manual | Não executado |
| M11 | Navegar por teclado e testar janela mínima/maior, capas e fallback | Foco/seleção perceptíveis; controles alcançáveis pela rolagem; capas ou fallback sem distorção | Aguardando validação do proprietário | Manual | Não executado |
| M12 | Sem credenciais/rede, organizar obras locais e revisar grade/lista/filtros | Coleções e operações pessoais locais funcionam; novos jogos continuam dependendo da IGDB | Aguardando validação do proprietário | Manual | Não executado |

O smoke XS01 e os testes Headless não substituem esse roteiro. Falhas de arquivo, concorrência, restauração de par simulado e primeira associação interrompida já foram exercitadas automaticamente; não é necessário provocá-las na biblioteca pessoal.

## Arquivos criados e modificados nesta fase

Criados:

- `Contracts/IArmazenamentoColecoes.cs`
- `Models/ColecaoPersonalizada.cs`
- `Data/ArmazenamentoColecoesJson.cs`
- `Services/ServicoColecoes.cs`
- `MainWindow.Colecoes.cs`
- `Tests/BacklogManager.Tests/Fase4/ApoioColecoes.cs`
- `Tests/BacklogManager.Tests/Fase4/CriarEditarTests.cs`
- `Tests/BacklogManager.Tests/Fase4/ExcluirAssociarTests.cs`
- `Tests/BacklogManager.Tests/Fase4/RemoverConsultarTests.cs`
- `Tests/BacklogManager.Tests/Fase4/IdentidadeTests.cs`
- `Tests/BacklogManager.Tests/Fase4/PersistenciaColecoesTests.cs`
- `Tests/BacklogManager.Tests/Fase4/IntegracaoColecoesTests.cs`
- `Tests/BacklogManager.Tests/Fase4/InterfaceColecoesTests.cs`
- `Tests/BacklogManager.Tests/Fase4/TelaColecoes.cs`
- `scripts/gerar_relatorio_fase4.py`
- `docs/FASE4.md`, `docs/FASE4_TESTES.md`, `docs/resultados-fase4.json`, `docs/verificacoes-fase4.json`

Modificados:

- `.gitignore`: novos arquivos pessoais de coleções.
- `Configuracao/ComposicaoAplicativo.cs`: serviço e caminho do sidecar.
- `Models/ItemBiblioteca.cs`, `Models/BacklogItem.cs`: ID interno opcional.
- `Data/ConversorBibliotecaJson.cs`, `Data/FileManager.cs`: transporte e validação desse ID.
- `Services/ServicoBiblioteca.cs`: identidade estável e preservação em edições abertas.
- `MainWindow.axaml`, `MainWindow.axaml.cs`: navegação, página, carregamento independente e estilo compartilhado.
- `Tests/BacklogManager.Tests/TelaTeste.cs`: injeção opcional de coleções para testes isolados.

O arquivo `docs/APROVACAO_FASE3.md` foi criado antes, no commit de aprovação integrado à `main`. Todos os relatórios anteriores permanecem intactos. A Fase 4 fica nesta branch para revisão; integração e Fase 5 dependem de nova autorização explícita.

## Commits e estado para revisão

- `100fa44` — implementação de coleções locais com associações por identidade interna.
- `549e81f` — testes de coleções, persistência e regressão com dados isolados.
- A documentação e os resultados são registrados em commit próprio na mesma branch; seu identificador é informado na entrega.

A `main` permanece em `8c9d147` (somente Fase 3 aprovada). A branch ativa é `feat/colecoes`, descendente dessa base. Os únicos arquivos pessoais não rastreados esperados após os commits são `qodana.yaml` e `skills/system/run-workflow/SKILL.md`. Não houve push ou remoção de branches.

Aguardando revisão manual e autorização explícita do proprietário para integrar a Fase 4 ou iniciar a Fase 5.

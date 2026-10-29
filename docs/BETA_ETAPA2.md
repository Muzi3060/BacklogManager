# Beta — Etapa 2: auditoria antes da publicação

**Data:** 29/10/2026. **Branch:** `chore/beta-auditoria`, criada da `main` em `7b82b64`. **Situação:** Etapa 2 oficialmente aprovada pelo proprietário; integração por fast-forward autorizada. Registro em [APROVACAO_BETA_ETAPA2.md](APROVACAO_BETA_ETAPA2.md). O conteúdo abaixo preserva a auditoria e os resultados da entrega anterior. As correções foram autorizadas posteriormente pelo proprietário e estão discriminadas na seção 11. Não houve alteração do aplicativo, das credenciais ou do histórico Git. O repositório não foi publicado.

A varredura e a revisão **não identificaram Client Secret, token real acidentalmente versionado ou biblioteca pessoal nos objetos Git examinados**. Isso é uma conclusão limitada aos métodos e ao conteúdo local disponível, não uma garantia universal de ausência de segredos. Foram encontrados riscos preventivos de inclusão acidental de arquivos e dados pessoais em caminhos/identidades Git, detalhados abaixo.

O uso futuro intencional do **Client ID e do Access Token da IGDB** na distribuição beta foi considerado decisão aprovada do proprietário e **não é tratado como impedimento à publicação**. Esses valores não foram incorporados ao aplicativo nesta auditoria. Um Client Secret continua fora dessa decisão.

## 1. Finalização e integração da Etapa 1

O estado inicial foi conferido: `main` em `1771688` e `feat/beta-interface` em `f7f08c8`, sem alterações rastreadas pendentes. Os commits `f536428`, `11e528d` e `f7f08c8` e o relatório de 1541 testes estavam presentes. A ancestralidade permitia fast-forward.

A aprovação manual geral informada pelo proprietário foi registrada em [APROVACAO_BETA_ETAPA1.md](APROVACAO_BETA_ETAPA1.md). Não foram atribuídos resultados individuais a MB01–MB05. As tabelas e os resultados automatizados anteriores foram preservados; os documentos da Etapa 1 receberam uma atualização explícita de aprovação, distinguindo o relatório histórico do relato manual posterior.

| Commit integrado | Conteúdo |
|---|---|
| `f536428` | Simplificação da interface, temas fixos e testes |
| `11e528d` | Documentação inicial da entrega |
| `f7f08c8` | Enunciados revisados e resultados finais de regressão |
| `7b82b64` | Registro da aprovação manual geral da Etapa 1 |

Foi executado `git merge --ff-only feat/beta-interface` na `main`. O ponteiro avançou de `1771688` para `7b82b64`, sem conflito e sem criar commit de merge. O código do aplicativo e dos testes permaneceu idêntico ao estado aprovado `f7f08c8`; a diferença adicional foi somente documental. `chore/beta-auditoria` foi então criada a partir dessa `main`.

`feat/promocoes-ui` permaneceu em `0aa420c`; a Fase 8 não foi integrada. As demais branches anteriores foram preservadas. `feat/beta-interface` permanece em `7b82b64`. Não houve push, fetch, exclusão de branches ou reescrita de histórico.

Antes das operações foi criado um manifesto privado de hashes para os arquivos pessoais, dados, backups, contrato e documentação anterior. As verificações posteriores confirmaram a preservação dos dados e arquivos pessoais. As únicas mudanças nos documentos anteriores foram o registro de aprovação autorizado. Não houve migração ou alteração da persistência.

## 2. Escopo efetivamente auditado

O inventário está fixado no estado `7b82b64`, depois da integração e **antes do commit deste relatório**. O novo relatório passa por revisão de conteúdo antes de ser versionado; suas referências não devem ser confundidas com o inventário histórico inicial.

| Conjunto | Conteúdo examinado |
|---|---|
| Árvore de trabalho | 252 arquivos de conteúdo fora de `.git`, `bin` e `obj`: 240 rastreados, 2 não rastreados e 10 ignorados |
| Histórico e referências | 52 commits alcançáveis pelas referências locais, incluindo branches anteriores e a Fase 8; 48 pertencem à história da `main` |
| Referências | 13 branches locais, 2 referências de acompanhamento remoto; nenhuma tag, stash, note ou replace ref |
| Reflogs | 159 entradas, com os mesmos 52 commits distintos; também considerados na enumeração de objetos |
| Banco de objetos Git | 704 objetos: 52 commits, 215 árvores e 437 blobs; todos os 437 blobs eram texto UTF-8 e foram examinados |
| Objetos sem referência | 4 blobs e 3 árvores; blobs também examinados, sem segredo ou biblioteca pessoal identificado |
| Nomes históricos | 263 caminhos distintos, obtidos percorrendo as árvores dos commits; 433 blobs alcançáveis por referências |
| Metadados | Autor, committer e mensagens dos 52 commits; configuração Git local relevante e URL do remoto, sem reproduzir identidades ou URL completa |
| JSON/documentação | 24 arquivos JSON atualmente rastreados; documentos, relatórios, instruções versionadas e suas versões históricas |
| Dados locais | `backlog.json`, `.bak`, locks e configurações da IDE, apenas leitura; nenhum dado real foi usado como fixture de testes |
| Backups externos | 22 arquivos existentes nos diretórios de backup conhecidos do projeto; 11 arrays de biblioteca e um envelope de backup com 4 componentes Base64, também decodificados em memória e examinados |
| Desenvolvimento | `.gitignore`, exclusão local, configuração do Rider, `qodana.yaml`, arquivo pessoal em `skills/`, projeto .NET e recursos versionados |

Não foram seguidos links simbólicos para diretórios arbitrários nem pesquisado todo o computador. `bin`/`obj` ficaram fora da varredura de conteúdo desta auditoria de fontes; nenhum outro arquivo local ultrapassou o limite de 8 MiB da ferramenta. Artefatos de distribuição ainda inexistentes deverão ser auditados depois da Etapa 3. `.git` foi examinado por interfaces Git e configurações relevantes, em vez de tratar objetos comprimidos como texto bruto.

Há apenas um worktree. O remoto configurado usa HTTPS para `github.com`, sem senha embutida na URL observada. A referência **local** `origin/main` está 44 commits atrás da `main`, sem divergência na direção contrária. Não houve fetch ou consulta ao GitHub: isso não confirma o estado, a visibilidade, releases, pull requests, logs de Actions ou referências existentes somente no servidor.

## 3. Métodos, ferramentas e evidências

Foram usados Git 2.56.0, ripgrep 15.2.0, Python 3.14.8 e .NET SDK 10.0.113. Gitleaks, TruffleHog, detect-secrets e Semgrep não estavam disponíveis no PATH; nenhum foi instalado nem houve envio do código a um serviço externo de detecção.

A varredura local combinou regras para chaves privadas, tokens de provedores/JWT, atribuições de credenciais, headers literais, variáveis de ambiente XML do Rider, URLs autenticadas/webhooks e strings de alta entropia. Comparou em memória os valores IGDB das instruções locais com os conteúdos examinados. Também procurou e-mails, caminhos pessoais, estruturas de biblioteca JSON e componentes codificados do formato de backup reconhecido.

As evidências guardam categorias, contagens, posições e identificadores de objetos, sem valores de credenciais. Os resultados detalhados e o manifesto de preservação ficam em pasta temporária privada fora do repositório. Não são arquivos de distribuição.

| Verificação de auditoria | Evidência/resultado |
|---|---|
| Integridade do Git | `git fsck --full` concluiu sem erro de objeto; os objetos dangling foram inventariados, sem remoção |
| Todas as referências locais | `git for-each-ref`, `git rev-list --all` e reflogs: inventário descrito acima |
| Objetos fora das referências | `git cat-file --batch-all-objects` incluiu também os 4 blobs sem referência |
| Arquivos removidos/renomeados no passado | Árvores de todos os commits percorridas, sem limitar a busca à versão atual |
| Credenciais conhecidas das instruções | Zero ocorrência nos blobs Git e nas mensagens de commits examinadas |
| Client Secrets/chaves privadas | Nenhum valor real identificado pelas regras e revisão; nenhuma validação por autenticação externa foi feita |
| Triagem heurística Git | 64 ocorrências candidatas, 18 valores distintos: diretórios/caminhos de testes, propriedades de evidência e uma URL fictícia de teste; nenhum confirmado como segredo |
| Dados pessoais de biblioteca no Git | Nenhum array real de biblioteca identificado; nenhum dos quatro nomes dos arquivos pessoais ativos apareceu em qualquer árvore histórica examinada |
| Relatórios com caminhos pessoais | 134 ocorrências em 19 arquivos atualmente rastreados; 24 versões de blobs históricos contêm caminhos pessoais |
| E-mails em arquivos rastreados | Nenhum encontrado na árvore atual; os metadados de commits contêm 2 e-mails e 2 nomes distintos, sem endereço `noreply` |
| Instrução privada da beta | Não rastreada, zero commits por caminho e zero ocorrência de seus valores conhecidos nos blobs; ignorada somente por `.git/info/exclude` |
| IDE | Credenciais de ambiente no workspace do Rider, corretamente ignorado; não há arquivos da IDE versionados nas árvores examinadas |
| Backups externos | Conteúdo examinado sem escrita; candidatos de entropia eram componentes Base64, sem segredo detectado após decodificação |
| Configuração/pessoais não rastreados | `qodana.yaml` e o arquivo pessoal de `skills/` sem segredo identificado, mas sem regra de exclusão |
| Preservação | Comparação de hashes/bytes e referências antes/depois; nenhum dado pessoal ou backup modificado pela auditoria |

Uma URL autenticada apontada pela heurística é um fixture de `Tests/BacklogManager.Tests/Fase3/CapasTests.cs`, no teste `UserInfo`: sua finalidade é exigir a rejeição desse tipo de URL sem HTTP. Não foi tratada como credencial real. Strings de diretórios de resultados, propriedades JSON e caminhos de backup não foram confundidas com tokens.

As expressões locais têm falsos positivos e podem ter falsos negativos. Não constituem análise completa de fluxo de dados, código ofuscado, todas as codificações ou dependências. Uma segunda varredura com ferramenta dedicada é recomendada antes da publicação, sobretudo após introduzir os componentes da Etapa 3.

## 4. Achados da auditoria inicial — antes das correções autorizadas

“Alta preventiva” significa risco de inclusão acidental, **não evidência de vazamento já publicado**. Não foi confirmado achado crítico de credencial comprometida no conteúdo Git local.

| ID | Gravidade | Achado e evidência | Recomendação |
|---|---|---|---|
| AUD01 | Alta preventiva | `FASE_BETA_INSTRUCOES.md` contém Client ID/Access Token reais e tem proteção apenas local. `.git/info/exclude` não acompanha outro clone. O arquivo não está no Git. | Autorizar regra compartilhada específica para esse nome; preservar integralmente o documento privado e sua exclusão local. Conferir o índice antes de publicar. |
| AUD02 | Alta preventiva | O nome padrão de exportação `minhas-realizacoes-backup.json` e cópias em diretórios como `backups/` não são ignorados. Um backup real exportado dentro do repositório poderia ser adicionado. Nenhum foi encontrado versionado. | Definir convenção e regras específicas de exclusão; guardar backups reais fora do repositório. Não ignorar indiscriminadamente todos os JSON, pois há relatórios e dados sintéticos legítimos. |
| AUD03 | Média — privacidade | Caminhos pessoais nos relatórios e nomes/e-mails nos metadados Git. Exemplos localizáveis: `docs/FASE0.md:45`, `docs/FASE1.md:43` e `docs/verificacoes-fase6.json`; 19 arquivos atuais e 2 identidades Git. | O proprietário deve decidir o que aceita tornar público. Propor caminhos portáveis nos documentos atuais e identidade pública para commits futuros. Ocultar dados já existentes na história exige decisão separada, não basta um novo commit. |
| AUD04 | Média — inclusão acidental | `qodana.yaml` e `skills/system/run-workflow/SKILL.md` são pessoais, não rastreados e não ignorados; um `git add .` poderia incluí-los. | Autorizar exclusões adequadas (`/qodana.yaml`, `/skills/`) ou outra política explícita. Não adicionar ou mover esses arquivos automaticamente. |
| AUD05 | Baixa/local | A configuração ignorada do Rider contém `IGDB_CLIENT_ID` e `IGDB_ACESS_TOKEN`. O token da IDE é diferente daquele das instruções; validade não verificada. | Manter a IDE fora do Git e dos pacotes. Não houve evidência local de exposição pública que justifique inferir necessidade de revogação. Nenhuma credencial foi alterada. |
| AUD06 | Baixa — preparação pública | Não há README nem arquivo explícito de licença no conjunto rastreado. Há documentação de desenvolvimento e instruções históricas como `FASE5_INSTRUCOES.md`. Nenhum segredo foi identificado nessa instrução versionada. | Preparar documentação pública com caminhos genéricos, escopo da beta e limitações; o proprietário deve escolher a licença e quais documentos de desenvolvimento pretende expor. Não remover documentos históricos automaticamente. |
| AUD07 | Limitação de verificação | Não houve ferramenta especializada instalada, consulta ao servidor GitHub nem auditoria de pacotes ainda inexistentes. | Repetir a verificação com uma segunda ferramenta e auditar os artefatos reais e o estado remoto antes da publicação autorizada. Não tratar “nenhum achado” como garantia absoluta. |
| AUD08 | Decisão aprovada, separada dos achados acidentais | Client ID/Access Token poderão ser usados intencionalmente na distribuição da beta. Não há Client Secret autorizado por essa decisão. | Na Etapa 3, identificar essas inclusões de forma específica e permitir apenas os valores/campos aprovados na revisão. Não criar exclusão genérica para qualquer token. |

Os caminhos pessoais não foram reproduzidos integralmente neste relatório. Usar `/home/<usuario>/...` em exemplos públicos evitaria divulgar o nome da conta e detalhes da organização local. Os 2 e-mails e nomes dos commits também não são reproduzidos aqui.

Os arquivos pessoais atuais e os backups foram lidos somente para identificação estrutural e preservação. Não se reinterpretou `SteamAppId`, não se atribuiu nova data, não se deduplicou nem removeu registro. O relato histórico de 11 registros em fases anteriores não foi usado para sobrescrever ou “corrigir” o estado atual.

## 5. Matriz de exclusões da auditoria inicial

Esta tabela preserva a constatação inicial em `c14f588`; a cobertura após as correções está na seção 11 e no novo relatório de testes. Os 19 caminhos abaixo foram consultados com `git check-ignore --no-index`, sem criar, renomear ou apagar arquivos. “Sem regra” indica cobertura ausente, não que o arquivo já tenha sido publicado.

| Caminho consultado | Situação observada | Origem |
|---|---|---|
| `FASE_BETA_INSTRUCOES.md` | Ignorado somente neste clone | `.git/info/exclude` |
| `qodana.yaml` | Sem regra | — |
| `skills/system/run-workflow/SKILL.md` | Sem regra | — |
| `backlog.json` | Ignorado | `.gitignore` |
| `backlog.json.bak` | Ignorado | `.gitignore` |
| `backlog.colecoes.json` | Ignorado | `.gitignore` |
| `biblioteca.preferencias.json` | Ignorado | `.gitignore` |
| `aparencia.preferencias.json` | Ignorado | `.gitignore` |
| `.mr-restauracao.json` | Ignorado | `.gitignore` |
| `.mr-backup.lock` | Ignorado | `.gitignore` |
| `.idea/workspace.xml` | Ignorado | `.gitignore` |
| `.env` | Ignorado | `.gitignore` |
| `bin/Debug/app.dll` | Ignorado | `.gitignore` |
| `obj/project.assets.json` | Ignorado | `.gitignore` |
| `TestResults/resultado.trx` | Ignorado | `.gitignore` |
| `minhas-realizacoes-backup.json` | Sem regra | — |
| `backups/exportacao.json` | Sem regra | — |
| `dados/backlog.json` | Sem regra; a proteção do arquivo ativo é específica da raiz | — |
| `backlog-copia.json` | Sem regra | — |

São 12 correspondências do `.gitignore`, 1 exclusão local e 6 caminhos sem regra. As regras atuais protegem o armazenamento ativo conhecido, mas não todas as possíveis cópias/exportações pessoais. Na execução inicial registrada em `c14f588`, não foi alterado `.gitignore` nem acrescentada outra regra local. A seção 11 registra a correção posterior autorizada.

## 6. Build, testes e preservação da auditoria inicial

| Verificação executada | Esperado | Observado | Resultado |
|---|---|---|---|
| `dotnet build --no-restore` | Solução compila | 0 avisos, 0 erros | Aprovado |
| `dotnet test --no-build --no-restore` | Regressão da Etapa 1 aprovada | 1541 executados, 1541 aprovados, 0 reprovados, 0 ignorados | Aprovado |
| Validação da ferramenta de triagem | Detectar casos sintéticos relevantes e recusar placeholders | 16 verificações finais executadas e aprovadas | Aprovado para esses casos, sem garantia universal |
| Consultas de exclusão | Identificar corretamente a proteção efetiva | 19 consultas executadas, com 6 lacunas relatadas | Constatação da auditoria; correção pendente |
| Ancestralidade/fast-forward | `main` deve ser ancestral da branch aprovada | Fast-forward concluído sem conflito | Aprovado |
| Código integrado | Código da `main` coincide com o aprovado | Diferenças adicionais limitadas à documentação de aprovação | Aprovado |
| Dados, backups, contrato e pessoais | Preservar bytes e referências | Comparações confirmaram preservação | Aprovado |

A validação da ferramenta utilizou somente valores fictícios: marcador de chave privada, token de provedor, secret literal, variável de senha, header literal, URL autenticada, URL com token, token simulado, leitura de variável, hash hexadecimal, e-mail fictício, caminho fictício, JSON fictício, binário, componente de backup Base64 e variável XML do Rider. O primeiro exercício detectou dois filtros inadequados para URLs; o instrumento temporário foi ajustado e reexecutado até os 16 casos passarem. Isso não foi uma correção do aplicativo nem uma afirmação de segredo real encontrado.

O build e a regressão foram executados antes do fast-forward, sobre o mesmo código aprovado. Depois da integração foi verificada a igualdade do código; não se apresenta essa conferência como uma segunda execução de testes. TRX real: `beta2-integracao.trx`, em diretório temporário externo ao repositório. Os relatórios automatizados anteriores não foram regenerados ou substituídos.

Na auditoria inicial, nenhuma funcionalidade nova nem comportamento do aplicativo foi implementado; portanto, não há novo grupo funcional ao qual atribuir artificialmente dez cenários da seção 19.1. A regressão aplicável foi executada, e as verificações estáticas/instrumentais estão discriminadas acima. A aprovação manual da Etapa 1 é geral; não houve nova sessão manual do Codex no Rider nesta auditoria.

A proteção de múltiplos arquivos permanece a da Fase 7: diário e recuperação, **sem atomicidade absoluta conjunta**. Aparência versão 2 e a compatibilidade legada da Etapa 1 não sofreram alterações.

## 7. Plano proposto na auditoria inicial

1. **Exclusões compartilhadas, sem mexer nos arquivos pessoais.** Acrescentar regras específicas para a instrução privada da beta, `skills/`, `qodana.yaml` e a convenção aprovada de backups/exportações. Manter os arquivos onde estão. Testar caminhos protegidos e confirmar que código, relatórios e fixtures legítimos continuam versionáveis. Não aplicar uma regra global `*.json`.
2. **Documentação pública e privacidade.** Preparar README; decidir licença e documentos públicos; propor substituição dos caminhos pessoais por exemplos portáveis nos documentos atuais, preservando originais privados conforme autorização específica. Os resultados dos testes não devem ser inventados, alterados ou apagados para fazer essa limpeza.
3. **Decisão explícita sobre o histórico.** Se o proprietário aceitar nomes/e-mails/caminhos históricos, registrar a aceitação. Se não aceitar, apresentar estratégia específica que preserve o repositório privado original, backups e branches, com consequências e autorização próprias. **Não executar rebase, filter-repo, exclusões de objetos, troca retroativa de autoria ou force-push.** Limpar somente a versão atual não apaga esses dados da história.
4. **Revisão antes de publicar.** Repetir a detecção, inspecionar exatamente as referências e arquivos que serão publicados e apresentar o resultado para autorização. A revisão dos pacotes será complementada somente depois da Etapa 3 aprovada.

O proprietário autorizou posteriormente as exclusões preventivas, a generalização de caminhos atuais e a documentação pública básica, sem escolher licença. Esses itens foram implementados na seção 11. A reescrita do histórico, a alteração de autoria, a escolha de licença e a Etapa 3 continuam fora da autorização.

## 8. Decisões necessárias do proprietário

- Revisar as correções e a convenção de exclusões implementadas antes de autorizar a integração da Etapa 2.
- Decidir se aceita tornar públicos nomes/e-mails dos commits e caminhos históricos; se não, escolher uma estratégia separada, com autorização própria.
- Decidir quais documentos históricos pretende tornar públicos e escolher uma licença, se desejar. O README básico já foi preparado; nenhuma licença foi escolhida ou adicionada.
- Autorizar posteriormente a Etapa 3 e, em momento distinto, a publicação. A aprovação desta auditoria não deve ser confundida com essas autorizações.

Não se recomenda revogar ou trocar credenciais com base apenas na existência de variáveis locais ignoradas. Validade dos tokens, acessos anteriores ao remoto e possíveis cópias fora deste escopo não foram verificados.

## 9. Verificações a repetir após a Etapa 3

- Examinar as novas alterações e todos os refs/objetos relevantes, preferencialmente com uma segunda ferramenta dedicada; distinguir os dois campos IGDB intencionais de qualquer inclusão acidental.
- Abrir/inventariar os ZIP, TAR.GZ, instalador e diretórios publicados reais. Excluir dados pessoais, backups, diário de recuperação, preferências reais, `.idea`, `.git`, `skills/`, instrução privada, logs e artefatos de testes. Não presumir que as exclusões do Git controlam o conteúdo dos pacotes.
- Confirmar ausência de Client Secret, senhas administrativas e credenciais de outras integrações nos fontes, recursos, configurações e binários distribuídos.
- Revisar metadados dos executáveis/instaladores, símbolos e relatórios gerados, além de dependências nativas, licenças e recursos incluídos. Esta auditoria não certifica os pacotes nem avaliou vulnerabilidades das dependências.
- Testar os pacotes em Windows/Linux com dados isolados: inicialização, IGDB, comportamento offline, preservação de dados e backup/restauração. Atualizações e caminhos persistentes só poderão ser considerados validados após sua implementação autorizada.
- Revisar os endpoints e dados da eventual verificação de atualizações, sem enviar biblioteca, reviews, preferências ou credenciais em URLs/logs.
- Conferir o estado real do GitHub, as referências a publicar e a visibilidade desejada, com autorização específica. Não usar publicação de todas as branches ou espelhamento por inferência.

## 10. Conclusão e estado de entrega

A Etapa 1 está integrada à `main` por fast-forward e a auditoria está isolada em `chore/beta-auditoria`. Os dados e as branches anteriores foram preservados.

Não foi identificada necessidade de reescrever histórico por um segredo confirmado. As exclusões preventivas, a sanitização dos caminhos atuais e a documentação básica foram implementadas após autorização. Para preparar a publicação ainda faltam revisar estas correções, decidir sobre os metadados/caminhos históricos e a licença, e repetir a auditoria sobre as mudanças e os pacotes da Etapa 3. A exposição intencional futura dos dois campos IGDB não foi usada como impedimento.

O commit `c14f588` entregou somente o diagnóstico inicial. A continuação autorizada acrescenta as correções e os documentos da seção 11 nesta mesma branch. Não houve merge da Etapa 2, push, publicação, revogação de credencial ou início da Etapa 3. Aguardamos a revisão e a autorização explícita do proprietário antes de integrar esta etapa à `main`.


## 11. Correções preventivas autorizadas e implementadas

A implementação foi realizada exclusivamente em `chore/beta-auditoria`, a partir de `c14f588`. A `main` permaneceu em `7b82b64`, `feat/promocoes-ui` em `0aa420c` e todas as referências anteriores foram preservadas. Não houve mudança em C#, Avalonia, modelos, persistência, APIs, credenciais ou configurações de autoria.

### Exclusões compartilhadas

O `.gitignore` agora protege o nome privado da instrução beta e suas cópias nomeadas, instruções marcadas como `.local`/`.privado`, `qodana.yaml` e `skills/`. O conteúdo desses arquivos foi preservado integralmente por hashes. A exclusão local anterior em `.git/info/exclude` também permanece intacta.

Os nomes ativos conhecidos de biblioteca, coleções, preferências e coordenação passaram a ser ignorados também em subpastas. Foram reservados diretórios de dados pessoais (`dados`, `dados-pessoais`, `dados-locais`), backups e exportações. Na raiz, há regras para o nome padrão de exportação, cópias nomeadas de `backlog` e prefixos de backup/exportação. As regras não alteram caminhos usados pelo aplicativo e não removem arquivos já existentes.

São regras específicas, sem `*.json` global. Relatórios, recursos, código, instruções históricas públicas e fixtures com nomes explícitos continuam versionáveis. O teste em repositório temporário confirma que o staging comum ignora as instruções privadas e a exportação fictícia, sem depender de `.git/info/exclude` deste clone.

### Caminhos atuais e documentação

Foram generalizadas **134 ocorrências em 19 documentos**, usando `${PROJETO}` e `${HOME}`. Os 10 JSON afetados foram validados sem propriedades repetidas, com comparação integral contra a versão anterior sob a mesma substituição de raízes. Os resultados, contagens, datas, hashes e demais conteúdos históricos foram preservados; chaves que representavam nomes de arquivos também receberam somente a substituição da raiz. Uma normalização que colidisse chaves é recusada pelo verificador.

Antes da limpeza, os 62 documentos originais foram copiados para uma pasta temporária privada fora do Git, com acesso restrito. Essa cópia não é um novo backup da biblioteca e não muda os backups existentes. Os originais também continuam no histórico Git, que não foi reescrito.

O novo `README.md` apresenta o Minhas Realizações, as funcionalidades atuais e as limitações da beta; inclui compilação, execução com dados separados, uso do Rider, configuração IGDB por variáveis, persistência e backup. Informa expressamente que a primeira beta está em desenvolvimento e que não há pacotes de distribuição aprovados. O novo `docs/README.md` explica os placeholders e como interpretar relatórios históricos. Nenhuma licença foi escolhida ou adicionada.

### Verificações e resultados desta continuação

Resultados individuais e tabela completa: [BETA_ETAPA2_CORRECOES_TESTES.md](BETA_ETAPA2_CORRECOES_TESTES.md). Evidência estruturada: [resultados-beta-etapa2-correcoes.json](resultados-beta-etapa2-correcoes.json).

- Build real: **0 avisos e 0 erros**.
- Regressão real: **1541 executados/aprovados, 0 reprovados, 0 ignorados**, no TRX `beta2-correcoes.trx`. Os testes utilizaram dados simulados e ambientes isolados.
- Verificações específicas: **120 aprovadas**, incluindo 43 cenários de exclusão/staging, 12 de arquivos permitidos, 17 da substituição de caminhos/validação JSON, 19 comparações de documentos, 10 comparações semânticas JSON, 12 verificações de documentação pública e 7 de preservação.
- Guardas do verificador: **3 aprovadas**, confirmando recusa de saída no repositório, de nome reservado da biblioteca e de sobrescrita de resultado existente, somente com dados fictícios.
- Reexecução da ferramenta local de triagem: **16 verificações sintéticas aprovadas**; examinou 53 commits e 438 blobs (708 objetos) existentes antes do commit desta continuação. Não foram encontrados os valores IGDB privados nesses objetos. Os achados heurísticos Git continuam sendo artefatos/testes já contextualizados na auditoria inicial.
- Varredura da documentação atual: **zero caminhos pessoais concretos** após a generalização. A nova documentação e os arquivos a versionar foram revisados sem reproduzir valores de credenciais, nomes ou e-mails pessoais.
- Preservação: contrato, 6 arquivos pessoais/dados locais e os 22 arquivos de backups externos mantiveram seus hashes. Fontes do aplicativo, demais referências Git e metadados antigos permaneceram intactos.

Os 117 primeiros checks específicos já passaram; foram acrescentados três casos negativos para colisão de chaves, propriedades JSON repetidas e JSON truncado, com reexecução final de 120/120. O verificador também rejeita sobrescritas de resultados. Não houve bug do aplicativo identificado ou alteração de seus comportamentos. Os relatórios anteriores não foram regenerados: receberam somente a sanitização de caminhos autorizada.

Comandos desta continuação:

```bash
dotnet build --no-restore
dotnet test --no-build --no-restore --logger 'trx;LogFileName=beta2-correcoes.trx' --results-directory /tmp/minhas-realizacoes-beta2-correcoes-resultados
python3 scripts/verificar_beta_etapa2.py
```

O verificador cria um repositório temporário com arquivos fictícios, não escreve no índice real e não inicializa o aplicativo. A opção privada `--manifesto` permite conferir hashes contra o estado anterior; sem ela, as verificações de comparação externa são omitidas explicitamente pela ausência da referência privada. A opção `--saida` aceita apenas um arquivo novo fora do repositório, diferente dos nomes de dados ativos. Manifesto, cópias originais e logs privados não foram adicionados ao Git.

### Riscos que permanecem

AUD01/AUD02/AUD04 foram mitigados pelas regras compartilhadas para os nomes e diretórios previstos. Arquivos já rastreados, `git add -f`, exportações com nomes arbitrários fora dessas convenções e o conteúdo de futuros pacotes não são protegidos automaticamente por `.gitignore`. Guarde dados reais fora do repositório e revise o índice e a lista de distribuição antes de publicar.

AUD03 foi mitigado na **versão atual da documentação**, mas nomes/e-mails e caminhos de commits anteriores continuam existentes e não foram alterados. Nenhuma aceitação pública desses metadados foi inferida; essa decisão continua pendente. Os relatórios históricos também conservam hashes e informações técnicas de ambiente que o proprietário pode querer revisar.

AUD05 continua sendo uma configuração local ignorada do Rider, sem alteração ou revogação de credenciais. AUD06 foi parcialmente atendido com o README; licença e seleção de documentos históricos públicos continuam pendentes. AUD07 mantém as limitações de varredura heurística, ausência de verificação remota e falta de pacotes reais. AUD08 permanece uma decisão aprovada para a futura distribuição, sem antecipação da Etapa 3.

Os testes de pacotes Windows/Linux e novas verificações após a Etapa 3 permanecem **não executados**, pois essa implementação não foi autorizada nem iniciada. Não houve nova validação manual no Rider nesta continuação; a interface não foi alterada. A limitação de persistência entre múltiplos arquivos, sem atomicidade absoluta, permanece registrada.

### Arquivos desta continuação

Novos: `README.md`, `docs/README.md`, `scripts/verificar_beta_etapa2.py`, `docs/BETA_ETAPA2_CORRECOES_TESTES.md` e `docs/resultados-beta-etapa2-correcoes.json`.

Modificados: `.gitignore`, este relatório e os 19 documentos sanitizados abaixo. Nenhum arquivo pessoal foi apagado, movido ou modificado.

- `docs/AJUSTES_FASE3.md`
- `docs/EVIDENCIAS_FASE7.json`
- `docs/FASE0.md`
- `docs/FASE1.md`
- `docs/FASE2.md`
- `docs/FASE3.md`
- `docs/FASE4.md`
- `docs/FASE5.md`
- `docs/FASE7.md`
- `docs/REVISAO2_FASE3.md`
- `docs/verificacoes-ajustes-fase3.json`
- `docs/verificacoes-fase3.json`
- `docs/verificacoes-fase4.json`
- `docs/verificacoes-fase5.json`
- `docs/verificacoes-fase6.json`
- `docs/verificacoes-integracao-fase5.json`
- `docs/verificacoes-integracao-fase6.json`
- `docs/verificacoes-revisao2-fase3.json`
- `docs/verificacoes-revisao3-fase3.json`

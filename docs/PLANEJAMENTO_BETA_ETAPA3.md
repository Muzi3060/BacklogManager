# Planejamento da beta — Etapa 3: distribuição

**Estado: proposta para revisão, implementação não autorizada.** Branch `feat/beta-distribuicao`, criada da `main` em `d418f04`, após o fast-forward aprovado da Etapa 2. A operação e as verificações reais estão em [INTEGRACAO_BETA_ETAPA2.md](INTEGRACAO_BETA_ETAPA2.md).

Este documento propõe decisões técnicas; nomes de arquivos novos e comandos de publicação abaixo **não foram implementados ou executados**. A versão inicial proposta é **`0.1.0-beta.1`**. A aprovação deste plano deverá preceder qualquer implementação. Não criaremos releases, tags remotas, instaladores ou publicação sem as autorizações correspondentes.

## 1. Diagnóstico e limites

A base atual usa .NET 10, Avalonia 12.1.4, composição de dependências em `Configuracao/ComposicaoAplicativo.cs` e persistência em quatro JSON. `Program.cs` inicia o ciclo de vida desktop; `App.axaml.cs` abre `MainWindow`; seu construtor padrão cria a composição real. `FileManager` resolve `backlog.json` relativamente ao diretório de trabalho. Biblioteca, coleções, aparência e visualização já aceitam caminhos injetados, permitindo alterar a localização na composição sem reescrever modelos ou serviços de negócio.

`ServicoBackupLocal` já oferece exportação validada, prévia, backup preventivo verificado, restauração substitutiva, diário e recuperação. A recuperação é chamada antes dos leitores. Esse mecanismo será reaproveitado; não se prometerá uma transação atômica absoluta entre os quatro arquivos.

A IGDB usa `HttpClient`, APICalypse, cancelamento, sugestões e variáveis de ambiente. Não há credenciais da beta embutidas, serviço de atualização, perfis de publicação ou instalador. `BacklogManager.csproj` ainda não fixa a versão pública pretendida. Não há licença escolhida; nenhuma será adicionada por inferência.

As Etapas 1 e 2 estão aprovadas. O Dashboard simplificado, os quatro temas nos dois modos, a biblioteca, coleções e backup/restauração devem permanecer. `feat/promocoes-ui` será preservada sem integração. As cinco mídias além de jogos continuam com catálogos simulados identificados. Não haverá backend, nuvem, sincronização, novas APIs de catálogo, preços, logotipo ou novo ícone nesta etapa.

## 2. Decisões recomendadas para a primeira entrega

| Item | Proposta |
|---|---|
| Versão exibida | `0.1.0-beta.1`, obtida de uma única fonte de versão |
| Arquiteturas iniciais | `win-x64` e `linux-x64`; ARM64/x86 ficam fora da primeira matriz |
| Windows portátil | ZIP com o diretório completo publicado e executável `BacklogManager.exe` |
| Windows instalado | EXE gerado com Inno Setup, instalação por usuário e sem exigir administrador |
| Linux portátil | TAR.GZ com executável `BacklogManager`, bibliotecas e instruções; preservação do bit executável |
| Runtime | .NET 10 self-contained, Release, sem trimming, Native AOT ou single-file inicialmente |
| Diretório de dados | Por usuário, fora da instalação e sem número de versão |
| Migração | Cópia consentida, validada e recuperável; nunca mover/apagar a origem nem mesclar automaticamente |
| Credenciais | Par Client ID/Access Token fornecido pelo proprietário, incorporado somente na publicação beta; nenhum Client Secret |
| Atualização | Consulta manual em Configurações, aviso e abertura da página oficial; instalação feita pelo usuário |
| Repositório de releases | Endereço oficial fixado na configuração de distribuição, acessível anonimamente para consulta real |

Self-contained inclui o runtime .NET, mas não elimina dependências nativas do sistema. O diretório completo evita riscos adicionais de empacotamento/reflexão nesta primeira beta. [Publicação .NET](https://learn.microsoft.com/en-us/dotnet/core/deploying/).

“Portátil” descreve a execução sem instalador. A biblioteca ficará no diretório persistente do usuário; copiar apenas o ZIP/TAR.GZ para outro computador não transportará automaticamente os dados. Para isso, usar o backup JSON existente.

Matriz inicial recomendada: **Windows 11 x64** e **Ubuntu 24.04 LTS x64 com sessão gráfica**. O Linux atual poderá fornecer um ensaio adicional, sem representar todas as distribuições. Windows 10 e outras distribuições só deverão ser anunciados após validação específica. A matriz deve respeitar a interseção de .NET 10, Avalonia e bibliotecas nativas. A documentação atual do Avalonia lista X11 e, em sessões Wayland, XWayland como caminho padrão; a mudança para backend Wayland nativo não é necessária nesta entrega. [Plataformas Avalonia](https://docs.avaloniaui.net/docs/supported-platforms), [Linux Avalonia](https://docs.avaloniaui.net/docs/platform-specific-guides/linux).

## 3. Ordem de implementação e critérios de passagem

1. **Preparação e referência:** conferir Git e hashes, preservar a biblioteca/coleções/preferências e criar/verificar backup externo antes de alterações da persistência. Confirmar matriz de máquinas, destino oficial de releases e ferramenta do instalador. Rodar a regressão e registrar os resultados reais.
2. **Inicialização e diretórios:** criar resolução explícita dos caminhos, com opção de perfil isolado para testes. Integrar pela composição existente, preservando a injeção de caminhos nos testes. Não iniciar gravadores antes de decidir o perfil e resolver pendências.
3. **Migração consentida:** implementar prévia e cópia recuperável, validar interrupções/conflitos e provar preservação dos bytes. Só continuar para distribuição quando essa proteção estiver coberta.
4. **Versão e credenciais beta:** fixar versão; implementar a seleção do par de credenciais e a geração privada de build. Validar que a publicação sem credenciais obrigatórias falha claramente sem imprimir seus valores.
5. **Consulta de atualizações:** implementar serviço de leitura e painel em Configurações, com testes HTTP simulados e comparação de versões. Nenhuma release será criada por esse serviço.
6. **Publicação dos diretórios:** gerar saídas Release separadas por RID; verificar conteúdo, metadados, bibliotecas nativas e higienização. Produzir ZIP/TAR.GZ apenas a partir dessas saídas.
7. **Instalador Windows:** reutilizar o mesmo diretório Windows já validado, instalar por usuário, criar atalhos e testar atualização/desinstalação sem tocar nos dados.
8. **Qualificação:** executar a matriz nativa, regressão, testes de falha e auditoria dos três artefatos; gerar relatório, hashes e notas da beta. Não aprovar um pacote por apenas compilar ou passar em Headless/Wine.
9. **Revisão do proprietário:** entregar os artefatos locais e os resultados, incluindo não executados e limitações. Aguardar autorização separada para merge, release e publicação.

Commits futuros devem acompanhar esses blocos, com código e testes correspondentes. Não serão criadas branches adicionais, publicados artefatos ou disparados pipelines remotos por inferência.

## 4. Armazenamento persistente

### Localizações propostas

| Sistema | Dados ativos | Binários |
|---|---|---|
| Windows | `%LOCALAPPDATA%\MinhasRealizacoes\Dados` | ZIP extraído em local escolhido ou instalação por usuário em `%LOCALAPPDATA%\Programs\MinhasRealizacoes` |
| Linux | `$XDG_DATA_HOME/minhas-realizacoes/dados`; padrão `~/.local/share/minhas-realizacoes/dados` | Diretório onde o TAR.GZ foi extraído |

A localização Windows será resolvida pela API de pasta especial do usuário, sem montar um nome de usuário literal. No Linux, XDG ausente/vazio usa o padrão; valor relativo é inválido e será ignorado conforme a especificação. [Pastas conhecidas Windows](https://learn.microsoft.com/en-us/windows/win32/shell/knownfolderid), [XDG](https://specifications.freedesktop.org/basedir/latest/).

Manter os quatro JSON no mesmo conjunto local preserva a arquitetura de `CaminhosDadosLocais` e o diário existente. Não separar preferências em outro volume ou introduzir banco de dados nesta beta. Manter os nomes `backlog.json`, `backlog.colecoes.json`, `biblioteca.preferencias.json` e `aparencia.preferencias.json`, os formatos atuais e os locks/diário da Fase 7.

O destino padrão dos backups preventivos existentes (`~/Backups/MinhasRealizacoes`) será preservado, fora da pasta ativa, com os caminhos efetivos visíveis em Configurações. Nenhum backup existente será movido ou limpo automaticamente. Não haverá política de retenção destrutiva.

### Inicialização e testes isolados

- Propor `--data-dir <caminho-absoluto>` para selecionar explicitamente um perfil; a opção é para testes e usos avançados, sem exigir configuração dos amigos. Sem a opção, usar o diretório padrão por usuário.
- Resolver o caminho uma vez. Não usar a pasta do executável ou o Working directory como fallback silencioso se o diretório padrão estiver sem permissão.
- Preservar os construtores/serviços com caminhos explícitos usados pela suíte. O novo padrão será aplicado no ponto de entrada real, por `App`/composição; `FileManager` continuará recebendo um caminho concreto.
- Antes de abrir a biblioteca, tratar recuperação pendente e coordenar inicializações/migrações concorrentes. Instâncias já abertas continuam protegidas por locks e detecção de mudança externa; a migração não pode disputar o mesmo destino vazio.
- Falhas de criação/acesso devem apresentar diagnóstico sem gravar em outra biblioteca, sem simular sucesso e sem iniciar o aplicativo sobre uma coleção vazia como se a anterior tivesse desaparecido.
- Atualizar o README, o gerador de cenários manuais e a configuração orientada para o Rider para usar `--data-dir` no ambiente de testes. Não apontar ensaios nativos ao perfil real do usuário.
- O designer do Rider e os testes de janela devem continuar usando dependências simuladas; a prévia visual não deverá migrar dados ou carregar credenciais da beta.

Não será alterado o diretório de trabalho global do processo para obter esse comportamento. ZIP, instalador e TAR.GZ usarão o mesmo local de dados por usuário. Trocar a pasta dos binários ou reinstalar não criará, por si só, uma nova biblioteca.

## 5. Migração segura dos dados existentes

A migração será de **localização**, sem converter o conteúdo da biblioteca ou seus identificadores. Dados de outras pastas temporárias não serão procurados recursivamente nem importados por suposição.

1. Se o perfil persistente já contiver dados, carregá-lo e não sobrescrevê-lo por descoberta de arquivo legado. Importações posteriores permanecem disponíveis pelo fluxo substitutivo de backup da Fase 7.
2. Se o destino estiver vazio, oferecer importação de uma pasta escolhida pelo usuário. Candidatos limitados à pasta de trabalho/execução antiga podem ser apresentados como sugestões com caminho explícito, sem cópia automática. O usuário pode cancelar ou confirmar a criação de uma biblioteca vazia separada.
3. Exibir prévia com origem/destino, arquivos presentes/ausentes, obras, coleções, vínculos e preferências disponíveis. Uma biblioteca antiga sem coleções/preferências continua suportada. Não atribuir identidades para apenas ler ou copiar.
4. Se houver diário pendente na origem, interromper a importação e orientar recuperação pelo fluxo existente; não recuperar ou alterar a origem silenciosamente. No destino, recuperar primeiro pelo mecanismo existente. Rejeitar caminhos sobrepostos, links não suportados e destinos reservados.
5. Capturar os arquivos conhecidos com os locks/proteções existentes e hashes antes/depois. Criar **backup externo verificado** do conjunto original, incluindo cópias `.bak` existentes dos arquivos conhecidos, sem copiar instruções, credenciais ou toda a pasta arbitrariamente. Falha de backup/verificação cancela a migração antes da escrita no destino.
6. Reutilizar exportação/validação de um envelope compatível e `Prever`/`Restaurar` no destino. Proteger o estado anterior do destino também pelo backup preventivo obrigatório. Campos opcionais ausentes continuam ausentes; preferências inválidas/futuras exigem escolha explícita de não aplicá-las, mantendo seu original no backup.
7. Preparar e verificar as cópias no filesystem de destino; só confirmar a operação com o diário existente, revalidação de hashes e ausência de conflito. Não reserializar os JSON na migração de localização. Os backups `.bak` extras podem ficar como cópias privadas acompanhadas de manifesto, fora do envelope ativo se não forem componentes suportados por ele.
8. Reabrir os leitores e confirmar obras, vínculos, preferências e integridade antes de anunciar conclusão. Não remover a origem nem seus backups. Depois de confirmar, usar o perfil persistente nas próximas execuções, sem repetir a migração por encontrar outra cópia legada.
9. Em interrupção/falha, tentar rollback/recuperação com a Fase 7 e comunicar qualquer pendência antes de permitir novas gravações. Não prometer atomicidade conjunta entre arquivos ou proteção absoluta contra falha física.

Uma biblioteca vazia só será criada após a escolha apropriada; o arquivo válido existente no destino distinguirá essa escolha de uma migração incompleta. Não se propõe um segundo protocolo de transação independente do diário existente.

Preservar integralmente: IDs internos, `SteamAppId`, `IgdbId`, duplicatas legítimas, ausência de identificadores/datas, campos antigos de categoria, extensões desconhecidas, zeros legados, avaliações, reviews e todas as associações. Nenhuma obra será cadastrada novamente ou reclassificada automaticamente.

**Proteção do trabalho de desenvolvimento:** antes de implementar a alteração de caminhos, conferir/registrar um novo backup externo dos dados reais, mas executar os testes exclusivamente com cópias isoladas ou dados simulados. O perfil pessoal não será migrado como efeito colateral de um teste ou build.

## 6. Credenciais IGDB na distribuição

### Preparação e seleção

- Manter comunicação direta com `https://api.igdb.com/v4/games`, APICalypse, campos, seleção obrigatória, sugestões e cancelamento. Sem backend ou autenticação individual.
- Criar um perfil explícito de publicação beta. Um passo local de preparação lerá somente o par aprovado de variáveis privadas do processo de build e gerará o recurso/código necessário em `obj`, ignorado pelo Git. Não copiar nem embutir `FASE_BETA_INSTRUCOES.md` como recurso ou arquivo de conteúdo.
- Não passar tokens por parâmetros `-p:`/linha de comando, não imprimir ambiente e não publicar binlogs/logs sensíveis de build. Escapar corretamente os valores gerados e restringir permissões do material intermediário. A publicação beta deve falhar se faltar um dos dois valores; Debug/testes não devem exigir segredos reais.
- Manter o leitor de ambiente atual e sua compatibilidade com `IGDB_ACESS_TOKEN`/`IGDB_ACCESS_TOKEN`. Um provedor estreito de credenciais decide a origem, sem misturar Client ID de uma fonte e token de outra.
- Proposta de precedência: um par completo e válido explicitamente configurado no ambiente tem prioridade; na ausência de configuração explícita, a publicação beta usa seu par incorporado. Configuração local parcial/inválida gera aviso claro, sem combinação silenciosa com o par da beta. O leitor atual de ambiente continua com o nome corrigido tendo preferência e fallback para o legado quando vazio.
- A distribuição para os amigos não exige variáveis de ambiente. Substituir o token futuramente significa preparar outra versão com novo par aprovado, gerar os três pacotes e distribuí-la manualmente; não haverá renovação automática com Client Secret.

O binário contém um token recuperável por inspeção; não se promete torná-lo secreto por codificação ou ofuscação. Essa exposição é a decisão expressa da beta, distinta de segredos acidentais. A auditoria final permitirá somente esses campos aprovados nos locais esperados, sem suprimir genericamente qualquer achado de token.

### Falhas e uso local

Em 401/403, a mensagem da beta deve indicar falha de autenticação e que a configuração **pode** ter expirado, orientar verificar nova versão e preservar o uso local. Não afirmar expiração com certeza a partir de qualquer 401. O perfil de desenvolvimento pode manter a orientação de conferir sua configuração local. Tratar timeout, indisponibilidade e 429 sem travar ou gerar repetição agressiva. Não enviar credenciais da IGDB ao GitHub ou ao navegador. [Autenticação e limites IGDB](https://api-docs.igdb.com/).

A validade real do par será verificada, após autorização da implementação, em teste autenticado pequeno em cada forma distribuída, com biblioteca simulada e sem registrar valores. Testes automatizados de erros/expiração usam HTTP simulado, sem revogar ou invalidar o token real.

## 7. Versão e verificação de atualizações

### Versão

Fixar `Version`/versão informativa em `0.1.0-beta.1`, com versão numérica adequada para metadados Windows (proposta inicial `0.1.0.1`) e AssemblyVersion separado. A versão pública deverá vir de um único ponto, visível em Configurações e nos manifestos dos pacotes; não usar a string numérica do Windows para ordenar prereleases.

Comparar SemVer, e não strings ou `System.Version` sozinho: `beta.10` deve suceder `beta.2`, e a versão estável correspondente sucede sua beta. Propor `NuGet.Versioning`, com versão fixada e validação restrita das tags aceitas, em vez de construir um comparador complexo. O parser permissivo do NuGet não será usado para aceitar automaticamente versões incompletas/quartos segmentos arbitrários. Metadados de build não devem alterar a precedência. [SemVer](https://semver.org/), [NuGet.Versioning](https://learn.microsoft.com/en-us/nuget/concepts/package-versioning#where-nugetversion-diverges-from-semantic-versioning).

### Consulta

- Criar botão **Verificar atualizações** em Configurações, com versão atual, andamento e resultado. A primeira entrega propõe consulta manual, sem polling automático ou nova preferência obrigatória.
- Usar cliente HTTP dedicado/anônimo, sem PAT do GitHub e sem reaproveitar headers autenticados da IGDB. Endpoint de leitura `GET /repos/{owner}/{repo}/releases`, com User-Agent, Accept e versão de API documentada/fixada.
- **Não usar somente `/releases/latest`**, pois esse endpoint exclui prereleases. Percorrer a listagem/paginação, filtrar drafts, validar tags e comparar semanticamente as releases compatíveis. Para uma beta, considerar próximas betas e estáveis; para versão estável, não oferecer regressão a beta por padrão. [Releases GitHub](https://docs.github.com/en/rest/releases/releases).
- Identificar compatibilidade por sistema/arquitetura e nomes/manifesto de assets previstos; uma release sem pacote compatível não significa que o aplicativo está atualizado. Não usar a ordenação por data como substituta da comparação de versões.
- Limitar tempo, tamanho de resposta e paginação; suportar cancelamento e fechamento da janela. Se a consulta ficar incompleta, mostrar estado inconclusivo, sem anunciar “você está atualizado” falsamente.
- Tratar 404 (incluindo repositório privado ou release ainda ausente), 403/429, falhas de rede e respostas inválidas sem interromper a biblioteca. Respeitar Retry-After/rate-limit e evitar novas tentativas em loop. Um cache por sessão/ETag pode evitar requisições repetidas sem acrescentar armazenamento de dados da biblioteca. [Limites GitHub](https://docs.github.com/en/rest/using-the-rest-api/rate-limits-for-the-rest-api), [Boas práticas](https://docs.github.com/en/rest/using-the-rest-api/best-practices-for-using-the-rest-api).
- Receber URL apenas do repositório oficial configurado, validar HTTPS, host/caminho e eventuais redirecionamentos. Exibir dados remotos como texto simples limitado. Abrir a página oficial somente após clique explícito; não abrir URLs de terceiros arbitrárias vindas do JSON.
- Não baixar, extrair, substituir ou executar atualizações. O usuário escolhe o pacote na página e instala manualmente, após backup.

**Dependência de publicação:** consulta anônima real precisa de releases acessíveis publicamente. Enquanto o repositório permanecer privado, a funcionalidade deve informar a indisponibilidade; não embutir PAT para contornar isso. É possível propor um repositório público separado apenas de distribuição, mantendo as fontes privadas, mas sua criação/publicação também exigiria autorização. Confirmar a URL oficial antes da implementação; não derivá-la de um `.git` inexistente no computador dos amigos.

## 8. Pacotes e instalador

### Publicação comum

Publicar somente `BacklogManager.csproj`, não a solução inteira com testes/worker. Usar saídas limpas e separadas, por exemplo em `artifacts/beta/<versao>/<rid>/publish`. Não copiar recursivamente a árvore do repositório.

Exemplos de comandos **planejados, não executados**:

```bash
dotnet publish BacklogManager.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -p:PublishTrimmed=false -p:PublishAot=false -o artifacts/beta/0.1.0-beta.1/win-x64/publish
dotnet publish BacklogManager.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=false -p:PublishTrimmed=false -p:PublishAot=false -o artifacts/beta/0.1.0-beta.1/linux-x64/publish
```

O perfil beta acrescentará a configuração de versão e a preparação privada de credenciais, sem valores nos comandos. Fixar SDK/versões de ferramentas na implementação; registrar versão do SDK, RID, versão pública e commit de origem no manifesto de cada build. Não atualizar Avalonia ou refatorar a arquitetura por conveniência do empacotamento sem necessidade demonstrada.

Gerar inventário e SHA-256 dos arquivos, preservando JSON de runtime/dependências e bibliotecas nativas necessários. Excluir fontes gerados privados, `.pdb` com caminhos locais quando não necessários à distribuição, testes/worker, dados pessoais, preferências, backups, diários, `.git`, `.idea`, `skills/`, `qodana.yaml`, instrução privada e logs. `.gitignore` não é uma lista de conteúdo de pacote. Manter avisos de terceiros necessários, sem escolher licença para o projeto.

Nomes propostos: `MinhasRealizacoes-0.1.0-beta.1-win-x64.zip`, `MinhasRealizacoes-0.1.0-beta.1-win-x64-setup.exe`, `MinhasRealizacoes-0.1.0-beta.1-linux-x64.tar.gz`, acompanhados por checksums e instruções. Sem sufixos de “validado” até os testes nativos correspondentes passarem.

### Windows ZIP

Extrair e executar sem instalar .NET, sem credenciais de ambiente e sem exigência de administrador. A pasta extraída não conterá biblioteca inicial real. Conferir inicialização a partir de outro Working directory, pasta com espaços/acentos e diretório sem permissão de gravar binários; dados continuam na pasta do usuário. O pacote precisa conter todas as DLL/runtime/recursos publicados, não apenas o `.exe`.

### Instalador EXE

Propor Inno Setup, com versão fixada após conferir disponibilidade e termos da ferramenta. Usar `PrivilegesRequired=lowest`, AppId estável entre versões, diretório de binários por usuário, atalhos com o nome público e desinstalação apenas do aplicativo. Não escolher licença do projeto nem criar novos recursos de marca. [Privilégios Inno Setup](https://jrsoftware.org/ishelp/topic_setup_privilegesrequired.htm), [AppId](https://jrsoftware.org/ishelp/topic_setup_appid.htm).

O instalador não executará migração, não lerá bibliotecas e não conterá regras para excluir a pasta persistente ou backups na desinstalação. Atualização com aplicativo aberto exige fechamento consentido; não encerrar o processo à força nem apagar arquivos para contornar locks. Reinstalar e desinstalar/reinstalar devem conservar dados e vínculos. A validação incluirá versão anterior simulada e novo pacote de teste, com registro de antes/depois. Certificado de assinatura não está disponível por inferência; não anunciar assinatura ou exigir desativar proteções do Windows para validar.

### Linux TAR.GZ

Preservar permissão de execução e arquivos nativos. Fornecer instruções de extração, execução e substituição manual da pasta de binários com o app fechado. Não exigir escrita no diretório extraído; não criar serviço ou instalador de sistema. Validar bibliotecas nativas no sistema-alvo e X11/XWayland; self-contained não garante execução em toda distribuição nem em musl/Alpine com RID glibc.

## 9. Arquivos previstos e responsabilidades

Os nomes novos são propostas, não scaffolding criado nesta solicitação.

| Área | Arquivos atuais afetados | Novos arquivos propostos |
|---|---|---|
| Inicialização/perfil | `Program.cs`, `App.axaml.cs`, `MainWindow.axaml.cs`, `Configuracao/ComposicaoAplicativo.cs` | `Configuracao/ResolvedorDiretorios.cs`, `Configuracao/OpcoesInicializacao.cs` |
| Migração | `Backup/CaminhosDadosLocais.cs`, integração restrita com `ServicoBackupLocal`/`ValidadorBackup` | `Services/ServicoMigracaoLocal.cs`, `JanelaInicializacaoDados.axaml` e `.cs` se necessária |
| IGDB beta | `Services/ConfiguracaoIgdb.cs`, composição e mensagem de autenticação em `IgdbService.cs` | `Configuracao/ProvedorCredenciaisIgdb.cs`, geração privada em `obj` |
| Versão | `BacklogManager.csproj`, metadados Windows quando necessário | `Configuracao/VersaoAplicativo.cs`, `Configuracao/ConfiguracaoDistribuicao.cs`, `global.json`, perfis `Properties/PublishProfiles/BetaWindows.pubxml` e `BetaLinux.pubxml` |
| Atualizações | `MainWindow.axaml`, composição e ciclo de descarte | `Services/ServicoAtualizacoesGitHub.cs`, modelos restritos, `MainWindow.Atualizacoes.cs`, adaptador injetável de abertura de links |
| Pacotes | `.gitignore` e exclusões específicas para novos arquivos locais | scripts de publicação/validação em `scripts/`, `packaging/windows/MinhasRealizacoes.iss` e instruções Linux |
| Testes/documentação | `README.md`, `docs/README.md`, gerador de ambiente fictício | `Tests/BacklogManager.Tests/BetaDistribuicao/`, testes de pacote isolados, relatório de implementação/testes da Etapa 3 e manifestos sem segredos |

Não alterar `Contrato.md`, `FASE_BETA_INSTRUCOES.md`, arquivos pessoais ou namespaces internos por conveniência. Os modelos e formatos de biblioteca/coleções devem permanecer; uma necessidade diferente interrompe o avanço para revisão de escopo.

O verificador da Etapa 2 possui condições históricas que exigiam ausência de mudanças em C# e documentação anterior à distribuição. Após mudanças autorizadas, esses checks precisam de transição documentada: manter a evidência histórica, reusar os checks aplicáveis de exclusões/privacidade e criar verificação da Etapa 3. Não apresentar `PR02` (“nenhum fonte alterado desde c14f588”) como aprovado sobre fontes que foram modificados. Não apagar testes ou desativar falhas sem justificativa/mapeamento.

## 10. Estratégia de testes — planejada, não executada

Reexecutar os **1541 testes automatizados existentes** e ampliar a regressão à medida que os novos casos forem incorporados. Manter dados simulados/cópias isoladas e HTTP simulado. Testes de pacote usarão `--data-dir` temporário, ambiente sem credenciais de usuário e pastas de binários/dados separadas. Migração/restauração destrutiva somente em fixtures.

Planejamento inicial: **206 cenários/verificações novos** — 190 de funcionalidades/pacotes e 16 de higiene da distribuição — distribuídos abaixo; cada comportamento novo/modificado tem pelo menos dez casos relevantes. O número é uma base, não um teto. Os cinco cenários manuais consolidados são adicionais. Resultados futuros deverão listar esperado/observado/status, falhas corrigidas, reexecuções e não executados; não converter planejamento em aprovação.

| Grupo | Quantidade | Método principal |
|---|---:|---|
| D — diretórios/inicialização | 20 | Unitários e integração com filesystem isolado |
| M — migração/recuperação | 30 | Integração, falhas injetadas e interrupção de processo |
| I — credenciais/IGDB beta | 20 | Unitários/HTTP simulado e ensaios reais separados |
| V — versão | 16 | Comparação/validação de versões e metadados |
| U — consulta de releases | 24 | HTTP simulado, paginação, limites e links |
| A — interface de inicialização/migração | 12 | Avalonia Headless e nativo |
| T — interface de atualizações | 12 | Avalonia Headless e nativo |
| P — publicação/inventário comum | 16 | Scripts e inspeção do conteúdo gerado |
| Z — Windows ZIP | 12 | Windows real/VM, sem runtime previamente instalado |
| W — Windows EXE | 16 | Windows real/VM, instalação/atualização/desinstalação |
| L — Linux TAR.GZ | 12 | Linux alvo real/VM, sessão gráfica e dependências |
| H — higiene/regressão da distribuição | 16 | Verificações de privacidade, escopo e preservação |
| **Total novo** | **206** | Todos pendentes até implementação autorizada |

### D — diretórios/inicialização (20)

D01 Windows por usuário; D02 Linux sem XDG; D03 XDG absoluto; D04 XDG vazio; D05 XDG relativo inválido; D06 perfil explícito absoluto; D07 perfil explícito relativo recusado; D08 caminho com espaços; D09 acentos/Unicode; D10 pasta inexistente criada com proteção adequada; D11 pasta sem permissão, sem fallback; D12 Working directory diferente; D13 binários movidos; D14 binários sem escrita; D15 arquivos ativos ausentes sem inventar dados; D16 recuperação antes dos leitores; D17 duas inicializações concorrentes; D18 perfis distintos isolados; D19 injeção de caminho preservada nos testes; D20 designer sem acesso ao perfil real.

### M — migração/recuperação (30)

M01 biblioteca legada sozinha; M02 conjunto completo; M03 coleções ausentes; M04 preferências ausentes; M05 aparência v1; M06 aparência v2; M07 campos extras preservados; M08 SteamAppId e IgdbId distintos mantidos; M09 duplicatas legítimas/IDs ausentes; M10 datas ausentes/zeros legados; M11 vínculos múltiplos preservados; M12 referência órfã tratada conforme regras atuais; M13 JSON truncado; M14 esquema futuro de biblioteca; M15 preferência futura com opção explícita de não aplicar; M16 cancelar prévia; M17 destino com dados impede sobreposição automática; M18 origem alterada durante captura; M19 destino alterado após prévia; M20 origem com diário pendente; M21 destino com diário pendente; M22 caminhos iguais/sobrepostos; M23 symlink rejeitado; M24 falha ao criar backup; M25 falha na verificação do backup; M26 erro/disk-full durante preparação; M27 interrupção entre arquivos; M28 falha de rollback mantém bloqueio/pendência; M29 reabertura após confirmação sem repetir migração; M30 origem e `.bak` intactos, com cancelamento/criação vazia explícita preservados.

### I — credenciais/IGDB (20)

I01 publicação com par completo; I02 falta Client ID bloqueia publicação; I03 falta token bloqueia publicação; I04 geração escapa valores e não imprime conteúdo; I05 execução beta sem variáveis; I06 par completo de ambiente tem precedência; I07 nome legado; I08 nome corrigido; I09 ambos os nomes com precedência preservada; I10 corrigido vazio usa legado; I11 configuração local parcial sem mistura; I12 formato inválido sem vazamento em erro; I13 Debug/testes sem par beta obrigatório; I14 401 com orientação da beta; I15 403 sem encerrar; I16 429 sem loop; I17 timeout/offline preservam uso local; I18 cancelamento e seleção da última consulta; I19 edição/exclusão/filtros/coleções offline sem IGDB; I20 novo par de uma versão futura substitui o anterior no pacote, sem Client Secret.

Ensaios autenticados reais em ZIP, EXE e TAR.GZ, previstos em Z10/W05/L10, são critérios de qualificação separados dos mocks I01–I20, com valores fornecidos localmente e sem uso da biblioteca original. Não são substituídos por I01–I20 simulados nem podem ser declarados aprovados antes da execução.

### V — versão (16)

V01 `0.1.0-beta.1` válido; V02 próxima beta; V03 beta.10 maior que beta.2; V04 estável maior que beta correspondente; V05 versão menor não oferecida; V06 versão igual sem aviso novo; V07 major; V08 minor; V09 patch; V10 prefixo `v` da tag; V11 metadados de build sem mudar precedência; V12 tag malformada ignorada; V13 versão incompleta recusada; V14 quarto segmento não aprovado recusado; V15 identificador numérico com zeros inválidos recusado; V16 versão exibida/assembly/manifesto coerentes.

### U — releases (24)

U01 lista com beta mais nova; U02 lista com estável; U03 draft excluído; U04 tag solta sem release não oferecida; U05 lista vazia; U06 ordem por data diferente da semântica; U07 próxima página com candidato relevante; U08 release somente de outro RID; U09 assets ausentes; U10 JSON inválido; U11 corpo vazio/incompleto; U12 campos essenciais nulos; U13 404; U14 403; U15 429/Retry-After; U16 timeout; U17 cancelamento ao fechar; U18 offline; U19 resposta muito grande; U20 limite de paginação com resultado inconclusivo; U21 cache/304 sem falso positivo; U22 headers sem token da IGDB/PAT; U23 URL não oficial/HTTP/redirecionamento indevido bloqueado; U24 falha ao abrir navegador comunicada sem instalar nada.

### A — interface de inicialização/migração (12)

A01 primeira execução vazia; A02 origem sugerida não importa sozinha; A03 escolha de pasta; A04 prévia mostra origem/destino e contagens; A05 cancelar sem gravar; A06 confirmação explícita; A07 destino ocupado; A08 erro de backup visível; A09 recuperação pendente bloqueia gravação; A10 mensagem de conclusão só depois da releitura; A11 teclado/rolagem em 900 × 660; A12 legibilidade nas oito combinações de tema/modo.

### T — interface de atualização (12)

T01 versão atual visível; T02 botão inicia consulta sem bloquear navegação; T03 indicação de andamento; T04 desabilitar consulta duplicada; T05 resultado atualizado; T06 nova versão disponível; T07 recurso remoto indisponível; T08 nova versão sem pacote compatível; T09 abertura somente por clique; T10 teclado/foco; T11 900 × 660/rolagem; T12 oito combinações de tema/modo, sem perder edição em andamento.

### P — publicação/inventário (16)

P01 win-x64 Release; P02 linux-x64 Release; P03 runtime incluído; P04 sem Native AOT/trimming; P05 JSON de dependências preservado; P06 bibliotecas nativas presentes; P07 recursos Avalonia/capas simuladas presentes; P08 nomes/versões/manifestos; P09 inventário e SHA-256; P10 saída limpa sem sobras de outra versão; P11 testes/worker ausentes; P12 dados/diários/backups ausentes; P13 instrução privada/Qodana/skills/IDE/Git ausentes; P14 fontes gerados/binlogs/símbolos sensíveis ausentes; P15 somente credenciais intencionais nos locais esperados e nenhum Client Secret; P16 falha de preparação não deixa pacote anunciado como concluído.

### Z — Windows ZIP (12)

Z01 extração íntegra/checksum; Z02 inicialização nativa sem .NET instalado; Z03 inicialização sem credenciais no ambiente; Z04 pasta com espaços/acentos; Z05 Working directory distinto; Z06 binários sem permissão de escrita; Z07 perfil persistente após reiniciar; Z08 capas e mídias; Z09 coleções/reviews/progresso; Z10 pesquisa real IGDB e cadastro em perfil fictício; Z11 backup/restauração isolados; Z12 extração de próxima versão em outra pasta preserva a biblioteca.

### W — Windows EXE (16)

W01 instalar sem administrador; W02 instalar sem runtime; W03 atalhos/nome público; W04 executar pelo atalho; W05 consulta IGDB real sem variáveis; W06 diretório com espaços; W07 reinstalar mesma versão; W08 atualizar a partir de pacote anterior simulado; W09 app aberto pede fechamento sem força; W10 cancelar instalação; W11 falha de cópia não anuncia sucesso; W12 desinstalar preserva dados/backups; W13 reinstalar após desinstalar; W14 AppId permanece estável; W15 vários usuários não compartilham biblioteca; W16 instalar não executa migração ou grava em dados de outro usuário.

### L — Linux TAR.GZ (12)

L01 checksum/extração; L02 executável e permissões; L03 iniciar sem .NET; L04 dependência nativa ausente gera diagnóstico real; L05 iniciar em sessão X11; L06 sessão Wayland/XWayland quando disponível; L07 sem escrita na pasta dos binários; L08 pasta com espaços/Unicode; L09 XDG persistente entre reinícios; L10 pesquisa IGDB real sem variáveis; L11 backup/restauração isolados; L12 troca de pasta dos binários preserva dados e preferências.

### H — higiene/regressão (16)

H01 1541 testes antigos reexecutados; H02 novos testes incorporados à regressão; H03 exclusões compartilhadas preservadas; H04 fixtures e JSON legítimos continuam versionáveis; H05 transição documentada dos checks históricos da Etapa 2; H06 contrato intacto; H07 arquivo privado intacto/não rastreado; H08 Qodana/skills intactos/não rastreados; H09 biblioteca pessoal/backup reais com hashes preservados; H10 `.git`/branches anteriores preservados; H11 logs sem credenciais completas; H12 novos documentos sem caminhos pessoais; H13 pacotes auditados após a geração; H14 instruções públicas correspondem ao pacote real; H15 nenhum download/substituição automática de atualização; H16 nenhuma publicação, merge ou nova integração fora do escopo autorizado.

### Cinco cenários consolidados para o proprietário

| ID | Validação manual proposta | Resultado esperado |
|---|---|---|
| MB3-01 | Executar as três formas no sistema correspondente, sem runtime/variáveis IGDB, em pastas de teste; navegar pelas telas e temas. | Inicialização real, aparência/teclado legíveis, dados fora dos binários. |
| MB3-02 | Migrar uma cópia fictícia com legados/coleções/preferências; cancelar uma prévia e depois confirmar; reiniciar. | Cancelar não escreve, confirmar cria backup verificado, preserva origem/IDs/vínculos e reabre no mesmo perfil. |
| MB3-03 | Pesquisar jogo real e cadastrar em biblioteca fictícia; reiniciar e repetir uso local sem rede. | Credenciais da beta funcionam sem configuração; falhas não encerram o app e as operações locais permanecem. |
| MB3-04 | Consultar atualizações e conferir versão/estado/abertura da página oficial; considerar o estado remoto efetivamente disponível. | Sem falso “atualizado”, sem PAT ou instalação automática; descoberta real de release só aprovada se houver recurso oficial acessível. |
| MB3-05 | Atualizar ZIP/TAR.GZ e instalar/desinstalar/reinstalar o EXE com dados fictícios. | Biblioteca, coleções, temas e backups preservados; nova versão identificada corretamente. |

Esses cinco cenários são consolidados e ainda **não executados**. Não transferem ao proprietário toda a matriz técnica nem substituem os testes do Codex.

## 11. Dependências, riscos e pontos de confirmação

- **Windows real/VM:** o ambiente inspecionado é Linux x86_64; existe Wine, mas não foi identificado compilador Inno Setup no PATH. Wine e Headless não aprovam o EXE para Windows. É necessária máquina/VM Windows e sessão gráfica para qualificação; CI remoto somente com autorização própria, sem publicação automática.
- **Destino oficial de releases:** confirmar repositório/URL e sua futura acessibilidade anônima. Não consultar/alterar a visibilidade do remoto nem criar release nesta solicitação. Testes de discovery real permanecem pendentes enquanto o recurso oficial não existir/estiver privado.
- **Matriz e arquitetura:** confirmar se Windows 11/Ubuntu 24.04 x64 atendem aos primeiros usuários. Acrescentar Windows 10/ARM64 exige ampliar build, testes e suporte, sem presumir equivalência.
- **Assinatura/licença:** decidir posteriormente a licença do projeto e eventual assinatura de código. Não inserir certificado, credencial administrativa ou licença sem autorização; ausência de assinatura deve ser relatada honestamente.
- **Dados:** mudança de localização pode parecer perda de biblioteca se o perfil errado for aberto. Mitigar com caminho visível, consentimento, backup, validação e nenhuma sobreposição silenciosa. Não testar migração no perfil pessoal.
- **Atualização/reinstalação:** nunca vincular os dados ao diretório de instalação, versão do pacote ou regras de remoção do instalador. Manter backup e limites de rollback registrados.
- **IGDB:** o token compartilhado pode expirar ou sofrer limitação de uso; a substituição exige nova versão manual. A exposição intencional é aceita, mas Client Secret e outros segredos continuam proibidos.
- **Dependências nativas:** self-contained não significa independente de sistema gráfico, glibc/fontes/bibliotecas. Registrar exatamente o sistema testado; não anunciar “todo Linux”.
- **Privacidade:** a sanitização atual não apagou nomes/e-mails/caminhos históricos. A aprovação da Etapa 2 não foi interpretada como autorização para tornar esses dados públicos. Auditar os artefatos e obter autorização de publicação separada.

As decisões recomendadas neste plano podem ser aprovadas em conjunto; dependências operacionais devem estar resolvidas antes de afirmar os testes correspondentes como aprovados. Se algum ensaio não puder ser executado, registrar seu motivo e o que falta, sem substituir validação nativa por uma suposição.

## 12. Critério de entrega e situação atual

A implementação futura estará pronta para revisão quando os três artefatos tiverem inventário/checksum, todos os testes aplicáveis tiverem resultados reais, a migração e os backups estiverem recuperáveis, a IGDB tiver sido exercitada nos pacotes e o verificador de versões tiver passado nos testes simulados e nos ensaios remotos efetivamente possíveis. Pendências nativas, remotas ou de assinatura devem ficar explícitas. Preservar a limitação de ausência de atomicidade conjunta entre arquivos.

**Nesta solicitação:** apenas a Etapa 2 foi integrada, sua aprovação/integração registrada e o plano elaborado. Não houve mudança de caminhos, geração/incorporação de credenciais, instalação de ferramentas, nova dependência, publicação `dotnet publish`, empacotamento, migração, implementação de consulta de releases ou início de testes da Etapa 3. Os 206 cenários/verificações e cinco manuais deste plano estão todos pendentes. A implementação aguarda aprovação explícita do proprietário.

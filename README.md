# Minhas Realizações

Uma biblioteca pessoal para organizar jogos, animes, séries, filmes, mangás e livros, registrar experiências e acompanhar o que você está consumindo.

**Beta `0.1.0-beta.1` — versão de pré-lançamento.** Confira os pacotes e checksums na página da release. Windows 11 x64 e Ubuntu 24.04 LTS x64 são os sistemas-alvo; os 37 cenários de qualificação nativa permanecem não executados, com essa limitação aceita para a beta. O teste manual relatado ocorreu no CachyOS com dados fictícios.

## O que funciona atualmente

- Biblioteca em grade ou lista, com capas, filtros e ordenação.
- Notas, reviews e progresso pessoal adequados a cada mídia.
- Coleções personalizadas: uma obra pode pertencer a várias coleções.
- Dashboard com estatísticas locais, adições recentes e obras em andamento.
- Quatro temas — Moderno, Tecnológico, Acolhedor e Dark Mode — com modos claro e escuro.
- Backup local em JSON, prévia de importação e restauração com confirmação e recuperação.

Jogos novos exigem pesquisa e seleção de um resultado da IGDB. Os catálogos das outras cinco mídias são **simulados** e identificados como demonstração. Visualização, edição de informações pessoais, filtros, ordenação e exclusão da biblioteca local funcionam sem IGDB; capas remotas podem depender da conexão e do cache da sessão.

## Instalar a beta

Os pacotes da primeira beta estão na [pre-release 0.1.0-beta.1](https://github.com/Muzi3060/BacklogManager/releases/tag/v0.1.0-beta.1). Confira o SHA-256 do download com `SHA256SUMS.txt` antes de executar.

### Windows 11 x64

- **ZIP portátil:** extraia o pacote completo em uma pasta nova e execute `BacklogManager.exe`. Mantenha as bibliotecas ao lado do executável.
- **Instalador EXE:** execute o instalador por usuário e utilize o atalho Minhas Realizações. A desinstalação dos binários preserva a pasta de dados.
- O runtime .NET acompanha os pacotes. A beta não possui assinatura de código; confira a origem e o SHA-256 antes de executar.

### Ubuntu 24.04 LTS x64

Extraia o TAR.GZ em uma pasta nova e execute `./BacklogManager`. O arquivo mantém a permissão de execução. O runtime .NET acompanha o aplicativo; bibliotecas gráficas nativas do sistema continuam necessárias. A qualificação específica no Ubuntu está pendente; execução no CachyOS não comprova suporte validado em todas as distribuições.

### Atualizar sem perder a biblioteca

Feche o aplicativo e exporte um backup pelas Configurações. Troque apenas a pasta dos binários ou execute o novo instalador; os dados ficam em diretório persistente separado. Não apague a pasta de dados, os backups ou o diário de recuperação. A atualização é manual; o aplicativo apenas consulta versões e oferece abrir a página oficial.

## Executar a versão de desenvolvimento

O projeto usa C#, **.NET 10** e Avalonia UI. Para compilar, é necessário o SDK correspondente. A execução em Linux também depende das bibliotecas nativas de interface do sistema; ainda não há uma matriz de pacotes distribuídos validada.

No diretório do projeto:

```bash
dotnet restore
dotnet build BacklogManager.csproj
```

O nome técnico `BacklogManager.csproj` permanece preservado. O nome exibido no aplicativo é Minhas Realizações.

**Os dados agora ficam fora dos binários, em uma pasta persistente por usuário.** Para testes, escolha explicitamente uma pasta isolada:

```bash
# Bash, a partir da raiz do projeto. Não utiliza a biblioteca pessoal.
dados="$(mktemp -d)"
dotnet run --project BacklogManager.csproj -- --data-dir "$dados"
```

No Rider, abra `BacklogManager.slnx` e informe `--data-dir <pasta-absoluta-de-testes>` nos argumentos do programa. O Working directory não define mais o armazenamento da inicialização normal. Confira os caminhos em Configurações.

Para criar obras, coleções e backups fictícios, execute `python3 scripts/preparar_validacao_fase7.py`. Use a pasta de dados impressa pelo gerador como argumento `--data-dir`; a referência antiga dele ao Working directory é histórica. Pastas temporárias podem ser limpas pelo sistema; guarde seus dados reais em local persistente.

Para importar uma pasta antiga na primeira execução de um perfil novo, use `--data-dir <destino-novo-absoluto> --migrate-from <origem-absoluta>`. A tela valida, apresenta a prévia e exige confirmação. Sem origem legada identificada, uma instalação nova inicia normalmente. Um destino já ocupado não é sobrescrito; use o fluxo explícito de backup/restauração para substituição.

## Pesquisa de jogos na IGDB

A versão de desenvolvimento comum continua aceitando configuração por ambiente:

- `IGDB_CLIENT_ID`: identificador do cliente.
- `IGDB_ACCESS_TOKEN`: token de acesso, com a grafia corrigida.
- `IGDB_ACESS_TOKEN`: nome legado, mantido por compatibilidade.

Quando ambos os nomes do token têm valores, a grafia corrigida tem preferência; se estiver vazia, o nome legado é usado. Sem credenciais válidas, o uso local continua disponível, mas novos cadastros de jogos dependem da IGDB. Uma seleção válida do catálogo é obrigatória.

Não coloque credenciais reais no código, no README ou em arquivos versionados. O aplicativo não precisa do Client Secret da Twitch. Os pacotes beta preparados localmente recebem o par aprovado no build e dispensam configuração pelos amigos. Um par completo no ambiente tem precedência; configurações parciais não são misturadas com as incorporadas. O token da beta é recuperável do binário, pode expirar e será substituído em uma versão futura. Nunca inclui Client Secret.

## Dados e backup

A pasta de dados contém os arquivos ativos. Padrões: Windows `%LOCALAPPDATA%\MinhasRealizacoes\Dados`; Linux `$XDG_DATA_HOME/minhas-realizacoes/dados`, ou `~/.local/share/minhas-realizacoes/dados` quando XDG não está definido. Os formatos permanecem compatíveis:

| Arquivo | Conteúdo |
|---|---|
| `backlog.json` | Biblioteca e informações pessoais |
| `backlog.colecoes.json` | Coleções e vínculos |
| `biblioteca.preferencias.json` | Visualização da biblioteca |
| `aparencia.preferencias.json` | Tema e modo |
| `.mr-restauracao.json` | Diário de recuperação, quando há uma operação de restauração |

Locks e cópias `.bak` também podem existir. Não apague o diário nem os backups para resolver uma pendência de recuperação; use os avisos e a ação de recuperação em Configurações.

Em **Configurações → Backup local**, é possível exportar biblioteca/coleções com preferências opcionais. A importação valida o arquivo e mostra uma prévia. A restauração **substitui** os dados selecionados, exige confirmação e cria/verifica um backup preventivo; não realiza mesclagem.

O destino padrão do backup preventivo é `~/Backups/MinhasRealizacoes`; os caminhos efetivos aparecem em Configurações. O arquivo exportado contém informações pessoais e não é criptografado: guarde-o fora do repositório. A coordenação por diário e recuperação não garante atomicidade absoluta entre vários arquivos nem proteção contra toda falha física.

Os registros legados são preservados. Os identificadores antigos não devem ser convertidos ou deduplicados manualmente para acompanhar uma atualização. Faça backup antes de trocar versões. Aparência versão 2 é lida pela versão atual; versões anteriores do aplicativo podem bloquear sua gravação para preservar dados.

## Verificações do projeto

A suíte usa dados simulados, diretórios temporários e HTTP simulado; não exige a biblioteca pessoal:

```bash
dotnet build
dotnet test
python3 scripts/testar_distribuicao_beta.py
python3 scripts/testar_instalador_beta.py
```

Os scripts requerem Python 3.9 ou posterior; a compilação do instalador requer Inno Setup 6.7.4. Os testes de scripts utilizam somente fixtures temporárias.

O verificador da Etapa 2 permanece como evidência histórica: condições como ausência de alterações C# e os caminhos antigos não descrevem mais esta etapa. A transição dos checks e as novas verificações estão em [BETA_ETAPA3_TESTES.md](docs/BETA_ETAPA3_TESTES.md). Não interpretar falhas de condições históricas como uma nova execução aprovada nem removê-las para mascarar resultados.

## Documentação e situação da beta

O [guia da documentação](docs/README.md) distingue registros históricos do comportamento atual. A [Etapa 1](docs/BETA_ETAPA1.md) descreve a simplificação; a [Etapa 2](docs/BETA_ETAPA2.md) registra a auditoria, correções preventivas e limites restantes.

A [Etapa 3](docs/BETA_ETAPA3.md) descreve a distribuição implementada, os limites e os testes pendentes. Atualizações são consultadas manualmente em Configurações e instaladas pelo usuário; nenhuma substituição automática é executada. O destino de releases configurado é `Muzi3060/BacklogManager`; sem acesso público, a consulta informa indisponibilidade. Nuvem, sincronização, Google Drive e APIs reais das outras mídias permanecem futuras.

## Licença

Distribuído sob a [licença MIT](LICENSE). Copyright © 2026 Murilo Martins Ribeiro. Os avisos de terceiros permanecem em `packaging/THIRD-PARTY-NOTICES.txt`.

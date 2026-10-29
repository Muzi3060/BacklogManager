# Minhas Realizações

Uma biblioteca pessoal para organizar jogos, animes, séries, filmes, mangás e livros, registrar experiências e acompanhar o que você está consumindo.

**A primeira beta ainda está em desenvolvimento.** A interface simplificada está aprovada, mas não há pacotes de distribuição aprovados. Instaladores, adaptação da IGDB para distribuição e verificação de atualizações ainda não estão implementados.

## O que funciona atualmente

- Biblioteca em grade ou lista, com capas, filtros e ordenação.
- Notas, reviews e progresso pessoal adequados a cada mídia.
- Coleções personalizadas: uma obra pode pertencer a várias coleções.
- Dashboard com estatísticas locais, adições recentes e obras em andamento.
- Quatro temas — Moderno, Tecnológico, Acolhedor e Dark Mode — com modos claro e escuro.
- Backup local em JSON, prévia de importação e restauração com confirmação e recuperação.

Jogos novos exigem pesquisa e seleção de um resultado da IGDB. Os catálogos das outras cinco mídias são **simulados** e identificados como demonstração. Visualização, edição de informações pessoais, filtros, ordenação e exclusão da biblioteca local funcionam sem IGDB; capas remotas podem depender da conexão e do cache da sessão.

## Executar a versão de desenvolvimento

O projeto usa C#, **.NET 10** e Avalonia UI. Para compilar, é necessário o SDK correspondente. A execução em Linux também depende das bibliotecas nativas de interface do sistema; ainda não há uma matriz de pacotes distribuídos validada.

No diretório do projeto:

```bash
dotnet restore
dotnet build BacklogManager.csproj
```

O nome técnico `BacklogManager.csproj` permanece preservado. O nome exibido no aplicativo é Minhas Realizações.

**Escolha uma pasta separada para os dados antes de abrir o aplicativo.** Nesta versão, os arquivos são relativos ao diretório de trabalho do processo. O exemplo Bash abaixo inicia com uma pasta temporária nova e vazia, sem ler a biblioteca do repositório:

```bash
# Execute a partir da raiz do projeto.
projeto="$(pwd)"
dados="$(mktemp -d)"
cd "$dados"
dotnet run --project "$projeto/BacklogManager.csproj"
```

Guarde o caminho da pasta escolhida se quiser reabrir a mesma biblioteca. Para uso contínuo, configure um diretório persistente separado; pastas temporárias podem ser limpas pelo sistema. Estes passos não movem nem migram uma biblioteca existente.

No JetBrains Rider, abra `BacklogManager.slnx`, configure o projeto executável e defina o **Working directory** como a pasta de dados desejada. Isso também se aplica ao Windows: use uma pasta separada e confira os caminhos exibidos em Configurações antes de salvar ou restaurar.

Para experimentar com dados fictícios, execute na raiz do projeto:

```bash
python3 scripts/preparar_validacao_fase7.py
```

O gerador imprime uma pasta nova para o Working directory e arquivos de backup simulados. Não lê a biblioteca pessoal e não remove automaticamente a pasta criada.

## Pesquisa de jogos na IGDB

Na versão de desenvolvimento atual, configure no ambiente de execução:

- `IGDB_CLIENT_ID`: identificador do cliente.
- `IGDB_ACCESS_TOKEN`: token de acesso, com a grafia corrigida.
- `IGDB_ACESS_TOKEN`: nome legado, mantido por compatibilidade.

Quando ambos os nomes do token têm valores, a grafia corrigida tem preferência; se estiver vazia, o nome legado é usado. Sem credenciais válidas, o uso local continua disponível, mas novos cadastros de jogos dependem da IGDB. Uma seleção válida do catálogo é obrigatória.

Não coloque credenciais reais no código, no README ou em arquivos versionados. O aplicativo não precisa do Client Secret da Twitch. A configuração da futura distribuição beta será tratada em etapa própria; não foi adicionada nesta entrega.

## Dados e backup

A pasta de trabalho contém os arquivos ativos:

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
python3 scripts/verificar_beta_etapa2.py
```

O verificador adicional requer Git e Python 3.9 ou posterior. Ele avalia exclusões compartilhadas, documentação sanitizada e coerência das instruções, sem iniciar o aplicativo sobre dados reais. A comparação documental usa o commit-base `c14f588` e precisa de seu histórico local; um clone raso pode não contê-lo. As três comparações de preservação que dependem do manifesto privado são indicadas como não executadas quando ele não é fornecido.

## Documentação e situação da beta

O [guia da documentação](docs/README.md) distingue registros históricos do comportamento atual. A [Etapa 1](docs/BETA_ETAPA1.md) descreve a simplificação; a [Etapa 2](docs/BETA_ETAPA2.md) registra a auditoria, correções preventivas e limites restantes.

Distribuição Windows/Linux, integração de credenciais nos pacotes e atualização de versões ainda aguardam desenvolvimento e validação autorizados. Nuvem, sincronização, Google Drive e APIs reais das outras mídias permanecem futuras.

Uma licença ainda não foi escolhida. Nenhuma licença foi adicionada nesta etapa.

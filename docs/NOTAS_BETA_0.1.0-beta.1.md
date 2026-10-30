# Minhas Realizações — 0.1.0-beta.1

Primeira beta pública, destinada a organizar uma biblioteca pessoal de jogos, animes, séries, filmes, mangás e livros. Desenvolvida em C#, Avalonia UI e .NET 10. Licença MIT.

## Funcionalidades

- Biblioteca em grade/lista, capas, filtros e ordenação; status, notas, reviews e progresso pessoal.
- Coleções personalizadas, com uma obra em várias coleções sem duplicar a biblioteca.
- Dashboard de estatísticas e acompanhamento; quatro temas com modos claro/escuro.
- Backup local em JSON, prévia, confirmação e recuperação de restaurações.
- Dados persistentes fora da instalação e migração consentida com backup verificado.
- Pesquisa direta na IGDB, com credenciais da beta incorporadas: não é necessário configurar variáveis de ambiente para pesquisar jogos.
- Consulta manual de versões no GitHub, incluindo betas; instalação de atualizações sempre manual.

## Downloads e instalação

Confira o SHA-256 com o arquivo `SHA256SUMS.txt` anexado antes de executar.

- **Windows ZIP portátil:** extraia a pasta completa e execute `BacklogManager.exe`.
- **Windows instalador EXE:** execute o instalador por usuário e utilize o atalho Minhas Realizações.
- **Linux TAR.GZ portátil:** extraia em uma pasta nova e execute `./BacklogManager`. A permissão de execução é preservada; bibliotecas gráficas nativas do sistema continuam necessárias.

Os pacotes são self-contained e incluem o runtime .NET 10. Sistemas-alvo: **Windows 11 x64 e Ubuntu 24.04 LTS x64**, ainda sem qualificação nativa completa. Não anunciar suporte validado a outras distribuições/arquiteturas. Binários sem assinatura de código; confira origem e checksum.

## Dados, migração e atualização

Dados ativos: Windows `%LOCALAPPDATA%\MinhasRealizacoes\Dados`; Linux `$XDG_DATA_HOME/minhas-realizacoes/dados`, padrão `~/.local/share/minhas-realizacoes/dados`. Biblioteca, coleções e preferências mantêm os quatro formatos JSON existentes. A pasta dos binários não contém uma biblioteca pessoal pré-preenchida.

Para atualizar, feche o aplicativo, exporte um backup e substitua somente os binários ou utilize o novo instalador. Não exclua a pasta de dados, seus backups ou o diário de recuperação. Migração antiga exige prévia e confirmação; não há mesclagem automática. A restauração é substitutiva e cria backup verificado. Não há garantia de atomicidade absoluta entre vários arquivos ou proteção contra toda falha física.

## Testes e limitações conhecidas

- Regressão automatizada da preparação: **1707 testes C# e 41 testes Python aprovados**, build sem avisos/erros. Relatórios completos no diretório `docs/`.
- **169 dos 206 cenários planejados atendidos; 37 cenários nativos NÃO EXECUTADOS**: 27 Windows e 10 Ubuntu. O proprietário aceitou publicar a beta com essas pendências; elas não foram marcadas como aprovadas. Compilação cruzada, Wine, Headless e CachyOS não substituem Windows/Ubuntu nativos.
- Validação manual do proprietário no CachyOS com dados fictícios: inicialização/navegação, persistência de jogos e temas, pesquisa/cadastro/edição/remoção com IGDB, coleções, tratamento de releases indisponíveis e troca de binários preservando dados.
- **Não declarados aprovados manualmente:** uso offline, migração manual completa, descoberta real de uma versão mais nova pela interface e testes nativos Windows/Ubuntu.
- Jogos exigem seleção de resultado IGDB. Os catálogos das outras cinco mídias são **simulados e identificados como demonstração**; não são integrações reais com novas APIs.
- Token IGDB compartilhado pode expirar/atingir limites; a substituição será feita em versão futura. Falhas não devem impedir o uso da biblioteca local. Capas remotas podem depender de rede/cache.
- Sem nuvem/sincronização, Google Drive, novas APIs de mídia ou atualizador automático.

## SHA-256

```text
e517caddcab36a883351894b84504565801610dfe65fea5d57158d2972664805  MinhasRealizacoes-0.1.0-beta.1-win-x64.zip
73169f9ecd24da3af18c05841f435ce6bced3baf9d133b46a764127949f056c2  MinhasRealizacoes-0.1.0-beta.1-win-x64-setup.exe
55b2ed4c7683db65e5614d1fba709a9b04dd8934854c9a24a9c6abcb9a2441dc  MinhasRealizacoes-0.1.0-beta.1-linux-x64.tar.gz
```

Faça backup antes de experimentar versões novas e registre problemas sem incluir sua biblioteca pessoal ou credenciais em issues públicas.

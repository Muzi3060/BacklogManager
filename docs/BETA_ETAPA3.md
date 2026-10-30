# Beta — Etapa 3: distribuição local

> Atualização de lançamento: Etapa 3 aprovada pelo proprietário, inclusive as 37 pendências nativas aceitas como não executadas. Licença MIT adicionada. O conteúdo abaixo registra a entrega original; a situação atual está em [LANCAMENTO_BETA_RESULTADO.md](LANCAMENTO_BETA_RESULTADO.md).

**Estado:** implementação na branch `feat/beta-distribuicao`, aguardando revisão. Versão `0.1.0-beta.1`. Nenhum merge, push, release ou publicação foi realizado. Os pacotes são candidatos locais; a qualificação nativa em **Windows 11 x64 e Ubuntu 24.04 LTS x64 continua pendente**.

O [planejamento](PLANEJAMENTO_BETA_ETAPA3.md) aprovado permanece como referência histórica. A autorização posterior permitiu prosseguir com os componentes comuns sem Windows, iniciar instalações novas sem obrigar migração e utilizar NuGet.Versioning. A validação realizada no CachyOS não substitui os sistemas-alvo.

## Implementação e arquitetura

- `ResolvedorDiretorios`: localização persistente por usuário; `--data-dir` absoluto para perfis isolados. Windows: `%LOCALAPPDATA%\MinhasRealizacoes\Dados`; Linux: `$XDG_DATA_HOME/minhas-realizacoes/dados`, padrão `~/.local/share/minhas-realizacoes/dados`. XDG relativo é ignorado; falha de acesso não abre silenciosamente outro perfil.
- `InicializacaoAplicativo`: prepara a pasta antes dos leitores; instalação nova inicia normalmente e cria biblioteca vazia somente quando o conjunto está vazio. Arquivos já presentes não são substituídos. O designer utiliza um perfil temporário próprio, sem pesquisas reais.
- Migração: candidatos limitados à antiga pasta de trabalho/execução são apenas sugestões. `--migrate-from <origem-absoluta>` permite selecionar explicitamente uma origem para um destino novo. A janela também permite escolher pasta, validar, revisar contagens e confirmar. Cancelar a importação preserva a origem; continuar com biblioteca nova é uma ação explícita.
- `ServicoMigracaoLocal`: copia os quatro JSON e preserva bytes, nomes, campos extras, IDs, duplicatas legítimas, datas ausentes, zeros legados e vínculos. Não move, converte, deduplica ou mescla. A coordenação pode criar arquivos `.lock` na origem; seus JSON e `.bak` não são sobrescritos nem apagados. Aceita biblioteca antiga sem componentes opcionais. Preferências incompatíveis podem ser excluídas explicitamente da aplicação, preservando seus originais no backup.
- Antes da escrita: backup externo da origem e seus `.bak` conhecidos, conferência SHA-256, envelope validado e backup preventivo do destino. O serviço existente de restauração coordena os arquivos com diário, locks, confirmação e recuperação. Mudanças após a prévia ou concorrência cancelam a operação. Links/caminhos sobrepostos são recusados. **Não há garantia de atomicidade absoluta entre arquivos ou contra falha física.**
- Recuperação pendente: a composição existente abre a interface protegida, mantém Configurações e a tentativa de recuperação acessíveis e bloqueia leitores/gravações. Não cria biblioteca vazia sobre o conjunto pendente. Falha de acesso à própria pasta apresenta diagnóstico sem abrir perfil alternativo.
- `ProvedorCredenciaisIgdb`: ambiente completo válido tem precedência, preservando `IGDB_ACCESS_TOKEN` e o nome legado `IGDB_ACESS_TOKEN`; configuração parcial não é misturada com a beta. A publicação recebe o par aprovado em variáveis privadas `MR_BETA_IGDB_CLIENT_ID` e `MR_BETA_IGDB_ACCESS_TOKEN`, gerando recurso em `obj`, ignorado. Debug/testes comuns não exigem o par. A beta dispensa configuração pelos amigos.
- Comunicação IGDB direta e pesquisas/sugestões existentes preservadas. Nenhum Client Secret, backend ou renovação automática. Em autenticação recusada, orientar consultar nova versão sem afirmar expiração com certeza. O token pode ser extraído do assembly: decisão intencional da beta, sem promessa de sigilo por ofuscação.
- `VersaoAplicativo`: versão pública única no projeto, metadados numéricos Windows separados, comparação com **NuGet.Versioning 6.14.0** e validação estrita SemVer.
- `ServicoAtualizacoesGitHub`: botão manual em Configurações, cliente anônimo exclusivo, listagem paginada de releases com betas, comparação semântica e pacote por RID. Limites de resposta/paginação/tempo, cancelamento, cooldown e tratamento de consulta inconclusiva. Sem PAT ou headers IGDB. O navegador só abre por clique, com URL HTTPS do repositório oficial validada. Nenhum download ou instalador é executado pelo aplicativo.

O destino dos candidatos locais foi configurado para `Muzi3060/BacklogManager`, repositório atual, conforme a proposta comunicada durante a execução. Sua visibilidade não foi alterada. O ensaio anônimo retornou 404; descoberta real de atualização permanece pendente de releases públicas acessíveis e da confirmação desse destino pelo proprietário. Um destino diferente exige nova preparação dos pacotes.

## Pacotes e ferramentas

Os perfis `BetaWindows.pubxml` e `BetaLinux.pubxml` publicam apenas o projeto do aplicativo: .NET 10 self-contained, Release, diretório completo, sem single-file/trimming/Native AOT. SDK fixado em `10.0.113` com atualização de patch permitida. Runtime incluído nos candidatos: `10.0.13`.

`scripts/publicar_beta.py` prepara saídas novas em `artifacts/`, recusa sobrescrita, gera inventário/checksums, verifica ZIP/TAR e audita uma lista permitida. Símbolos de depuração gerados ficam fora do pacote; o utilitário legítimo `createdump` acompanha o runtime. JSON de runtime/dependências são necessários e não são biblioteca pessoal. Fontes, testes, instruções privadas, IDE/Git, dados, backups, diários, Qodana e skills ficam fora.

`packaging/THIRD-PARTY-NOTICES.txt` reúne avisos oficiais das dependências e da fonte Inter, com fontes/hashes. Somente espaços finais e quebras de linha foram normalizados. Não atribui licença ao Minhas Realizações; essa decisão continua com o proprietário. Revisar os avisos ao atualizar dependências.

O EXE usa **Inno Setup 6.7.4**, instalação por usuário, AppId estável, atalhos e desinstalação dos binários. Não executa migração, não encerra processos à força e não possui regras de exclusão dos dados/backups. A compilação ocorreu com o compilador oficial em prefixo Wine isolado, apenas para gerar o artefato. **Isso não valida instalação/atualização/desinstalação em Windows.** Não há assinatura de código.

Para reproduzir, preparar o par privado no ambiente do processo, sem incluí-lo em argumentos, histórico, arquivos versionados ou logs compartilhados. Python é dependência da preparação, não dos amigos que executam o aplicativo:

```bash
python3 scripts/publicar_beta.py --rid linux-x64 --saida artifacts/reproducao/linux-x64 --repositorio Muzi3060/BacklogManager
python3 scripts/publicar_beta.py --rid win-x64 --saida artifacts/reproducao/win-x64 --repositorio Muzi3060/BacklogManager
```

No Windows com Inno Setup instalado, utilizar `scripts/compilar_instalador_beta.py --publicacao <publish-Windows> --iscc <ISCC.exe>`. O script confere o inventário antes de compilar. Valores secretos não devem aparecer em linha de comando. Geração local não significa upload ou release.

Os candidatos de revisão e seus SHA-256 estão listados no [relatório de testes](BETA_ETAPA3_TESTES.md). Diretórios anteriores de ensaios foram preservados localmente; utilizar apenas **candidato4** para revisão.

## Principais conceitos em C#

A resolução de caminhos foi concentrada na inicialização/composição, evitando alterar os modelos da biblioteca. Os serviços continuam recebendo caminhos explícitos, permitindo testar com arquivos temporários.

Os `record` transportam resultados e prévias; a prévia guarda hashes para detectar mudanças antes da confirmação. `using` garante o descarte de arquivos, respostas HTTP e locks. `CancellationToken` permite cancelar consultas sem interromper o uso local. A comparação semântica distingue corretamente `beta.10`, `beta.2` e versões estáveis.

## Arquivos e preservação

Foram modificados a inicialização (`App`, composição e janela), o projeto/perfis, a mensagem de autenticação IGDB, a seção de Configurações, `.gitignore`, README e índice de documentação. Foram criados os serviços de diretórios/migração/credenciais/versões/atualizações, janela de migração, scripts/perfis/instalador, avisos de terceiros e testes em `Tests/BacklogManager.Tests/BetaDistribuicao`. A relação exata está no relatório de testes e no diff da branch.

`Contrato.md`, instrução privada, biblioteca pessoal, preferências reais, Qodana/skills, backups anteriores e demais referências Git foram conferidos contra manifesto privado anterior às mudanças. O backup inicial externo foi criado e verificado antes da alteração de persistência. Nenhum teste utilizou a biblioteca pessoal; somente fixtures/perfis temporários.

Os relatórios das Etapas 1 e 2 permanecem históricos. A condição antiga `PR02` (“nenhum C# alterado desde c14f588”) deixou de ser aplicável após autorização da distribuição; não foi apresentada como aprovada sobre fontes modificados. Suas evidências anteriores permanecem, e a transição está detalhada no relatório novo.

## Limitações e pendências

- Qualificação nativa completa de Windows 11 x64 e Ubuntu 24.04 LTS x64. CachyOS, Headless e Wine não substituem essa matriz.
- Pesquisa/cadastro IGDB pelo fluxo completo dos três executáveis ainda precisa de validação nativa. A API real respondeu 200 e os testes do serviço/provedor passaram; isso não foi apresentado como validação completa da interface distribuída.
- Verificação real de descoberta de release beta depende de recurso oficial público. O retorno 404 foi tratado como indisponibilidade, sem simular atualização.
- Interrupção abrupta da nova migração foi exercitada em dez pontos por processo separado; recuperação e origem foram verificadas. Isso não cobre falha física de disco ou perda de energia do sistema.
- Atomicidade limitada entre arquivos, fontes/coleções remotas não disponíveis offline sem cache e expiração/limites do token compartilhado continuam registrados.
- Sem assinatura; licença própria e decisão sobre metadados históricos/publicação continuam pendentes. Nenhuma publicação foi autorizada por esta entrega.

## Até cinco validações consolidadas no Rider e nos pacotes

Preparar dados com `python3 scripts/preparar_validacao_fase7.py`. Usar somente as pastas recém-geradas. No Rider, informar `--data-dir <pasta-absoluta-dos-dados-ficticios>`; não usar o perfil pessoal. Os ensaios dos pacotes devem ocorrer também nos sistemas-alvo e fora do Rider.

1. **Inicialização e interface:** executar os três formatos no sistema correspondente, sem variáveis IGDB, percorrer telas, temas/modos e teclado em 900 × 660. Conferir versão e caminhos em Configurações. Windows deve ser testado sem runtime .NET previamente instalado.
2. **Migração consentida:** perfil de destino novo e origem fictícia, usando `--migrate-from`. Cancelar uma prévia; depois validar, confirmar e reiniciar. Conferir backup externo, bytes/IDs, obras, coleções e preferências; origem intacta. Instalação sem legado deve iniciar sem exigir migração.
3. **IGDB e uso local:** pesquisar/selecionar/cadastrar um jogo real somente na biblioteca fictícia, reiniciar e verificar persistência. Depois testar operações locais sem conexão. Não registrar token em imagens/logs.
4. **Atualizações:** consultar em Configurações, verificar aviso real de indisponibilidade enquanto o GitHub retornar 404. Quando houver release oficial acessível, validar descoberta de beta/estável compatível e abertura por clique. Instalação sempre manual; indisponibilidade não aprova descoberta de releases.
5. **Troca de versão e recuperação:** trocar somente binários ZIP/TAR, instalar/desinstalar/reinstalar EXE, conferindo dados e backups fictícios. Exercitar exportação/restauração e retorno à sessão. Simulações de falha somente no perfil isolado.

Esses cinco cenários ainda não foram executados pelo proprietário nesta etapa. A implementação aguarda revisão e aprovação; integração e publicação exigem autorizações explícitas separadas.

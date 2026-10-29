# Fase 7 — Configurações e backup local

> Atualização em 29/10/2026: a Fase 7 foi oficialmente aprovada pelo proprietário após validação manual geral no Rider, em ambiente isolado. Veja [APROVACAO_FASE7.md](APROVACAO_FASE7.md). O texto abaixo preserva o registro da entrega e suas pendências naquela data; não atribui execução individual aos cinco cenários manuais propostos.

Estado: implementação na `feat/configuracoes`, aguardando revisão e validação manual do proprietário. Sem push, merge ou início da Fase 8. Contrato versão 1.1 preservado.

## Implementação e decisões

A página **Configurações** reúne visualização grade/lista, atalho para a aparência existente, caminhos efetivos de armazenamento, exportação/importação local e indicação textual de recursos futuros. Os dois seletores de visualização usam a mesma preferência. Aparência continua no serviço aprovado da Fase 5, com os três temas, modos e cores personalizadas. Nuvem, híbrido, sincronização e Google Drive continuam indisponíveis, sem OAuth, login, rede ou armazenamento remoto.

O backup é **um único JSON versionado**: `Formato = MinhasRealizacoes.BackupLocal`, `Versao = 1`, `CriadoEm` declarado e `Componentes`. Biblioteca e coleções são obrigatórios no manifesto; aparência e visualização são opcionais. Cada componente traz `Conteudo` em Base64 e `Sha256` dos bytes originais; ausência explícita é `Conteudo: null` e hash `AUSENTE`. O Base64 é uma decisão de preservação, não criptografia: conserva inclusive BOM, espaços, extensões e formatos antigos sem reserializar/migrar os dados. Os componentes de preferências disponíveis aparecem na prévia.

A exportação valida uma cópia do conjunto pelos leitores existentes, adquire locks em ordem definida, confere o estado carregado de biblioteca/coleções e os hashes antes/depois, escreve temporário com flush, conclui a substituição e reabre o resultado. Ao substituir um backup de destino, mantém o anterior em `.bak`. Não exporta credenciais, contratos, imagens, caches, logs, arquivos pessoais ou diretórios arbitrários. Referências de capas são preservadas, sem download. Os hashes detectam alterações, mas não comprovam autoria.

A importação aceita apenas esse envelope completo. Um `backlog.json` isolado ou `.bak` individual não comprova o par biblioteca/coleções. A validação recusa JSON inválido, duplicação de propriedades, formato/versão desconhecidos, hashes incorretos, dados e relacionamentos inválidos. Reutiliza as regras das fases anteriores e preserva referências órfãs permitidas, com contagem/aviso. Preferências futuras ou inválidas impedem somente sua aplicação: os dados válidos podem ser restaurados sem elas. A prévia mostra origem, data declarada, contagens atuais e novas, mídias, vínculos e referências ausentes, sem modificar arquivos ativos.

A restauração é **substitutiva, sem mesclagem**. Exige a caixa de confirmação explícita; preferências ficam desmarcadas inicialmente. Mudar a seleção ou o escopo exige nova confirmação. Arquivo de origem ou estado local alterado depois da prévia exige nova seleção. Preferência ausente nunca apaga a atual por inferência.

Antes da substituição, o serviço cria um backup externo completo do estado atual, reabre e verifica formato e hashes. Se houver falha de criação/verificação, cancela sem modificar dados ativos. Depois prepara e confere todos os novos arquivos no mesmo filesystem, grava o diário com flush e substitui cada componente individualmente. Verifica o conjunto, grava o marcador de confirmação e remove o diário. A interface só anuncia conclusão depois de recarregar biblioteca, coleções, Dashboard, visualização e aparência solicitadas/disponíveis, descartando formulários, seleções, exclusões e pesquisas antigas.

O diário `.mr-restauracao.json` guarda os estados anterior/novo e seus hashes, sem destinos de importação executáveis. Em falha controlada, tenta rollback e verifica os bytes anteriores. Se isso falhar, conserva diário/cópia e bloqueia novas gravações. Na inicialização, `ComposicaoAplicativo` tenta a recuperação **antes dos leitores e preferências**. Diário incompleto reverte ao estado anterior; diário confirmado exige que o conjunto novo esteja íntegro. Conteúdo externo desconhecido ou diário inválido/futuro mantém o bloqueio. Configurações oferece tentar recuperação novamente.

Os quatro gravadores respeitam a pendência. A preferência grade/lista passou a usar `.lock`, mantido no disco como os locks existentes; nunca se apaga um lock em uso. Seu contrato de recuperação explícita e salvamento sem carregamento prévio foi mantido. A aparência conserva seu controle de conflito existente.

## Arquitetura em C#

- `CaminhosDadosLocais` reúne os caminhos numa coleção somente de leitura, mantendo exatamente a resolução e os sufixos anteriores. Não há mudança da pasta dos dados.
- `DocumentoBackup`, `ComponenteBackup` e `PreviaRestauracao` são registros com propriedades tipadas. A prévia é uma descrição, não uma autorização para gravar; o serviço relê e valida a origem ao confirmar.
- `ServicoBackupLocal` coordena exportação, prévia, restauração, locks e recuperação. `ValidadorBackup` reaproveita os leitores existentes sobre arquivos temporários. `ArquivosBackup` concentra leitura limitada, hash e escrita por substituição.
- `using` libera os bloqueios mesmo quando há exceção; `try/catch/finally` separa sucesso, recuperação e limpeza. O diário continua existindo quando não é seguro liberar as gravações.
- `MainWindow.Configuracoes` cuida de seletores, consentimento e mensagens. A interface não implementa serialização da biblioteca. Dependências e seletores substituíveis permitem testes com arquivos e HTTP simulados.
- `BacklogManager.BackupWorker` é um executável **exclusivo de testes**, referenciado pelo projeto de testes. Encerra o próprio processo em pontos definidos da restauração, sem passar pelo rollback da exceção, para provar a recuperação por uma nova instância. Não faz parte do fluxo da aplicação.

## Arquivos criados e modificados

Lista da entrega em relação a `0983fc7`. A = criado; M = modificado.

| Estado | Arquivo |
|---|---|
| M | `.gitignore` |
| M | `Aparencia/ArmazenamentoAparenciaJson.cs` |
| A | `Backup/ArquivosBackup.cs` |
| A | `Backup/CaminhosDadosLocais.cs` |
| A | `Backup/DocumentoBackup.cs` |
| A | `Backup/ServicoBackupLocal.cs` |
| A | `Backup/ValidadorBackup.cs` |
| M | `Configuracao/ComposicaoAplicativo.cs` |
| M | `Data/ArmazenamentoColecoesJson.cs` |
| M | `Data/ArmazenamentoPreferenciasBiblioteca.cs` |
| M | `Data/FileManager.cs` |
| M | `MainWindow.Aparencia.cs` |
| M | `MainWindow.Biblioteca.cs` |
| A | `MainWindow.Configuracoes.cs` |
| M | `MainWindow.axaml` |
| M | `MainWindow.axaml.cs` |
| M | `Properties/AssemblyInfo.cs` |
| A | `Tests/BacklogManager.BackupWorker/BacklogManager.BackupWorker.csproj` |
| A | `Tests/BacklogManager.BackupWorker/Program.cs` |
| M | `Tests/BacklogManager.Tests/BacklogManager.Tests.csproj` |
| M | `Tests/BacklogManager.Tests/Fase3/PreferenciasTests.cs` |
| M | `Tests/BacklogManager.Tests/Fase5/EstadosVisuaisTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/ApoioBackup.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/ComposicaoBackupTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/ConfiguracoesTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/ContrasteBackupTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/DiarioDefensivoTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/ExportacaoTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/IntegracaoBackupTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/PreferenciasBackupTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/PreferenciasNaSessaoTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/PreviaInterfaceTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/PreviaServicoTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/ProtecoesAdicionaisTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/RecuperacaoTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/RestauracaoTests.cs` |
| A | `Tests/BacklogManager.Tests/Fase7/ValidacaoTests.cs` |
| A | `docs/EVIDENCIAS_FASE7.json` |
| A | `docs/FASE7.md` |
| A | `docs/FASE7_TESTES.md` |
| M | `docs/PLANEJAMENTO_FASE7.md` |
| A | `docs/resultados-fase7.json` |
| A | `scripts/gerar_relatorio_fase7.py` |
| A | `scripts/preparar_validacao_fase7.py` |

## Caminhos e preservação

A biblioteca continua no caminho usado na sessão, normalmente `backlog.json` no working directory do Rider. Coleções continuam em `<nome-da-biblioteca>.colecoes.json`; preferências continuam em `biblioteca.preferencias.json` e `aparencia.preferencias.json`, com os sufixos anteriores quando há colisão. Configurações exibe os caminhos completos.

As cópias obrigatórias anteriores à restauração ficam, por padrão, em `~/Backups/MinhasRealizacoes/antes-restauracao-<data>-<id>.json`, fora da pasta dos dados. Essa pasta é criada somente quando uma restauração é confirmada; nenhum teste usa esse destino de produção. Falta de acesso ou destino dentro da pasta dos dados impede a restauração. Em Unix, os novos backups/diários usam permissões `600`, e as pastas novas de segurança/validação, `700`. Não se alteram permissões de pastas pessoais existentes.

Antes de modificar a persistência nesta implementação, foi criada e verificada a cópia externa `${HOME}/Backups/BacklogManager/fase7-20261029-140929`. A biblioteca pessoal permaneceu com SHA-256 `d33b826a57148dc8c7017ae22e15f6898163ba7d5d5eea57a7ba64b6ddaac810`. Contrato, dados presentes, 16 arquivos dos backups anteriores, relatórios e arquivos pessoais foram comparados com o manifesto inicial. A evidência final consta em `EVIDENCIAS_FASE7.json`.

IDs internos/externos, `SteamAppId`, ausência de IDs e datas, categorias antigas, gêneros, duplicatas pessoais, zeros legados, reviews e associações são preservados. Exportação não atribui identidades nem normaliza registros. Não houve escrita ou restauração sobre a biblioteca pessoal, nem acesso à pasta manual `/tmp/backlogmanager-rider-Bl9X6g`.

## Verificação e resultados

O planejamento exigia ao menos 152 novos cenários e a regressão dos 1.365 existentes. A cobertura foi ampliada para **186 novos cenários**, agrupados abaixo. Resultados finais e tabela individual de cenário, esperado, observado, tipo e status estão em [FASE7_TESTES.md](FASE7_TESTES.md) e [resultados-fase7.json](resultados-fase7.json), gerados do TRX real. Evidências de execuções e hashes estão em [EVIDENCIAS_FASE7.json](EVIDENCIAS_FASE7.json).

| Grupo | Cenários novos |
|---|---:|
| Configurações, navegação, temas e seletores | 18 |
| Preferências e compatibilidade | 17 |
| Exportação consistente | 22 |
| Validação de importação | 27 |
| Prévia e consentimento | 12 |
| Restauração e cópia obrigatória | 26 |
| Recuperação e interrupção de processos | 31 |
| Integração e recarga | 19 |
| Contraste e estados dos checkboxes | 14 |
| **Total novo** | **186** |

Baseline real: 1.365 aprovados, zero reprovados/não executados. O baseline reproduziu o aviso xUnit2017 já registrado na Fase 6; foi corrigido trocando uma asserção equivalente por `Assert.Contains`, sem desativar analisadores.

**Entrega verificada:** `dotnet build --no-restore --no-incremental`, zero erros e zero avisos. `dotnet test --no-build`: **1.551 executados e aprovados**, zero reprovados, ignorados ou não executados, incluindo os 1.365 anteriores e 186 novos. TRX: `/tmp/backlogmanager-fase7-resultados/fase7-concluido.trx`; duração da execução: 2,7602 minutos. As cinco propostas manuais continuam pendentes; aprovação automatizada não equivale à aprovação da Fase 7 pelo proprietário.

Comandos de reprodução:

```bash
dotnet build --no-restore --no-incremental
dotnet test --no-build --logger 'trx;LogFileName=fase7-concluido.trx' --results-directory /tmp/backlogmanager-fase7-resultados
python3 scripts/gerar_relatorio_fase7.py /tmp/backlogmanager-fase7-resultados/fase7-concluido.trx
```

O build compila também o worker pelo projeto de testes. A suíte usa .NET 10, dados temporários, Avalonia Headless/Skia e HTTP simulado. Cenários de permissões/links foram executados em Linux; os de permissões Unix precisam de adaptação para execução em Windows. O teste do preparador manual requer Python 3. Não foram realizados testes autenticados adicionais da IGDB.

Foi realizado um smoke desktop de oito segundos, sem credenciais IGDB, com quatro JSONs fictícios: processo permaneceu ativo, log vazio e hashes preservados. Não houve interação manual com o seletor nativo. Os seis conjuntos de tema/modo, cor própria, rolagem e prévia mínima foram renderizados e inspecionados em imagens da suíte. Seletores são exercitados por substitutos nos testes de interface; a interação nativa ficará para o Rider.

## Falhas encontradas e corrigidas

1. `InvalidDataException` não estava incluída no filtro inicial de erros do backup. O diário futuro e preferências inválidas podiam escapar do tratamento previsto. O filtro foi ampliado e os testes demonstram bloqueio/recuperação e restauração somente dos dados.
2. A validação inicial tratava o JSON inválido de preferências como erro fatal do conjunto. Foi separada da validação dos dados, mantendo a possibilidade aprovada de restaurar somente biblioteca/coleções.
3. O estilo Fluent preenchia as caixas de seleção novas com o destaque sem assegurar contraste do texto. Os três controles agora usam explicitamente os tokens seguros do painel. Quatorze cenários verificam os seis conjuntos, estados, foco/hover e cor própria.
4. Navegar durante um seletor assíncrono podia reabilitar as ações. Uma indicação de seleção pendente impede seletores concorrentes e confirmação durante essa operação.
5. A etapa de aplicação passou a preparar todos os arquivos antes da primeira substituição. Falha de preparação/diário inicial não regrava o conjunto ativo. Falha após o marcador de confirmação torna a intenção de rollback durável antes de reverter.
6. Restauração somente dos dados preserva também cores válidas ainda pendentes de salvamento na sessão. Preferências só são recarregadas se o componente correspondente foi explicitamente escolhido e está presente. Dois cenários adicionais cobrem a escolha pendente e um backup só com preferência de visualização.
7. Após uma recuperação bem-sucedida, a interface limpa os avisos antigos de falha de carregamento, evitando continuar indicando bloqueio quando os serviços já estão disponíveis.
8. Foram reforçados limites de leitura durante o streaming, permissões dos temporários e rejeição de caminhos reservados/manifestações nulas ou destinos arbitrários no diário.

A primeira suíte parcial executável teve 144 aprovados e oito reprovados: parte correspondia ao tratamento de erros, parte a fixtures que tentavam construir dados inválidos pelo conversor correto ou não aguardavam o evento de pesquisa. Foram corrigidas e reexecutadas. Uma primeira regressão completa teve 1.528 aprovados e duas falhas (PV03/PV10): as asserções não previam o novo `.lock`. Agora exigem exatamente o arquivo de preferência e lock, ou somente lock após falha, continuando a recusar temporários/resíduos. Nenhum teste anterior foi removido. Tentativas que falharam na compilação não são contabilizadas como execução aprovada. Relatórios e resultados das fases anteriores não foram reescritos.

## Limitações e pendências

- **Não há atomicidade absoluta entre arquivos.** O CRUD normal de biblioteca/coleções continua em dois arquivos sem transação conjunta; aparência/visualização são separadas. A coordenação e o diário desta fase cobrem o fluxo de backup/restauração, não todos os CRUDs.
- Locks e verificações protegem instâncias compatíveis. Editores externos e versões antigas podem ignorá-los; mudanças desconhecidas bloqueiam a recuperação. Não há garantia contra falhas físicas, corrupção de filesystem ou toda interrupção elétrica; flush não equivale a uma transação de disco.
- O envelope tem limite de 64 MiB e profundidade 64; Base64 aumenta o tamanho. Diário tem limite de 192 MiB por conter ambos os estados. Falta de memória/espaço pode impedir a operação; não houve simulação física de disco cheio ou corte de energia. Há falhas de I/O injetadas, permissões reais e interrupção de processo.
- Em caso de estado atual semanticamente inválido, a prévia informa contagens não legíveis. A cópia obrigatória conserva os bytes e hashes, mas esse conteúdo danificado pode exigir análise/extração manual para reparo; não é apresentado como um snapshot semanticamente válido. Se os bytes não puderem ser lidos/copiados/verificados, a restauração não começa.
- Backups não têm criptografia, senha ou assinatura. A pasta externa padrão é fixa nesta entrega e pode necessitar permissão do sistema; escolher o destino de exportação não muda a biblioteca nem essa política.
- Cópias externas e `.bak` não têm retenção automática. Interrupção abrupta pode deixar arquivos `.tmp` preparados; eles não são dados ativos e não são consumidos como backup. Não há varredura destrutiva automática de arquivos pessoais/temporários desconhecidos.
- Operações de disco são síncronas durante a aplicação local, suspendendo interação naquele trecho; backups grandes podem causar uma pausa. Não há progresso/cancelamento depois de começar a substituição. Na próxima abertura, diário pendente é tratado antes do uso normal.
- URLs de capas podem ficar indisponíveis; exportação não inclui cache/imagens. IGDB e os cinco catálogos simulados não foram modificados.
- Validação manual da Fase 7 está pendente. Nuvem, sincronização, Drive, mesclagem, criptografia e Fase 8 não foram implementados.

## Até cinco cenários de validação manual no Rider

Preparação: execute `python3 scripts/preparar_validacao_fase7.py`. Ele cria uma pasta nova e imprime o **Working directory**, contendo somente sete obras fictícias, duas coleções e preferências. Configure esse diretório em Run/Debug Configuration → Working directory. A pasta pai contém backups fictícios completo/vazio/inválido. Não utilize sua biblioteca real nem a pasta de backups como working directory. Para repetir com estado inicial, execute o preparador novamente. A cópia externa obrigatória criada pelo aplicativo ao confirmar continua no destino exibido nas Configurações.

1. **Configurações e temas:** navegue entre Configurações, Aparência e Biblioteca; alterne grade/lista em ambos os locais, escolha temas/modos e uma cor própria; reinicie. Confira preferências, caminhos, teclado, rolagem no tamanho mínimo e o aviso de recursos futuros indisponíveis.
2. **Exportação e prévia:** exporte um JSON com preferências e outro sem; selecione o completo e confira sete obras, duas coleções, três vínculos e os componentes opcionais. Cancele. Selecione o arquivo inválido e confira mensagem clara, sem mudança da biblioteca nem reaproveitamento da prévia anterior.
3. **Substituição somente dos dados:** edite uma review fictícia e altere o tema; selecione `backup-simulado.json`, mantenha preferências desmarcadas e confirme explicitamente. A review deve voltar à versão do backup e o tema atual permanecer. Verifique a mensagem/cópia externa e reinicie; confira coleções e vínculo compartilhado sem duplicar obras.
4. **Preferências e backup vazio:** escolha restaurar também preferências do backup completo; confira lista/tema restaurados e a nova confirmação ao mudar essa opção. Selecione `backup-vazio.json`: a prévia deve mostrar redução para zero. Confirme somente na pasta fictícia, reinicie e depois restaure o backup completo para recuperar as obras.
5. **Proteção contra alteração após prévia:** selecione uma cópia fictícia válida; depois, num editor, acrescente um espaço ao final desse arquivo e salve. Ao confirmar, deve aparecer a mensagem de que o backup mudou, sem substituir os dados. Selecione-o novamente para gerar uma prévia atualizada e conclua normalmente. As falhas de permissão, rollback e interrupção já foram testadas automaticamente em ambientes isolados; não force essas falhas sobre dados/backups pessoais.

Esses cinco cenários são propostas para o proprietário; nenhum recebe aprovação manual antecipada.

## Git da entrega

- Branch `feat/configuracoes`; implementação em `64689c7`, testes/preparador em `30e4bf5`, seguidos pelo commit desta documentação e evidências.
- `main` permanece em `ca265a8`; nenhuma integração/push. Demais branches mantêm os hashes do manifesto inicial.
- Após o commit documental, sem alterações rastreadas pendentes. `qodana.yaml` e `skills/` permanecem não rastreados, intactos e fora dos commits.
- A branch tem a documentação inicial do planejamento (`0983fc7`) e os três commits da execução desta fase. O relatório/evidência registra os commits de código/testes; o hash do commit documental está no histórico Git, evitando autorreferência de hash no próprio arquivo.
- Não houve alteração de `Contrato.md`, migração de dados, exclusão de backups ou implementação da Fase 8. Aguardando revisão e aprovação explícita antes de integrar à `main`.

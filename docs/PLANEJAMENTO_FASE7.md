# Planejamento da Fase 7 — Configurações e backup

**Estado:** proposta aguardando aprovação; nenhuma implementação iniciada.
**Branch:** feat/configuracoes, criada da main em ca265a8.
**Base:** Fases 0–6 aprovadas; .NET 10 e Avalonia 12.1.4.
**Referências:** Contrato.md §§12, 13, 15, 16 e 19.1; docs/FASE6.md e docs/FASE6_TESTES.md.

A autorização atual permite integrar a Fase 6 e preparar este plano. Não autoriza implementar a Fase 7, fazer push ou iniciar a Fase 8.

## 1. Diagnóstico da arquitetura

| Área | Estado observado | Consequência |
|---|---|---|
| Biblioteca | FileManager + IArmazenamentoBiblioteca; JSON legado/multimídia; hash, .lock, temporário, flush, substituição e .bak | Reaproveitar validação e proteção; não renomear campos ou reinterpretar IDs |
| Coleções | Arquivo separado versionado; associações por IdInterno; proteção semelhante | Biblioteca e coleções precisam viajar juntas no backup |
| Aparência | Arquivo próprio, seis conjuntos, cores solicitadas/efetivas, extensões preservadas e versão futura protegida | Reutilizar tela/serviço, sem reconstruir temas ou mudar a escolha salva |
| Grade/lista | ArmazenamentoPreferenciasBiblioteca, documento versão 1; escrita por temporário | Hoje não possui o mesmo bloqueio/hash dos dados; avaliar proteção localizada para coordenar restauração |
| Caminhos | ComposicaoAplicativo resolve o diretório de trabalho e nomes/sufixos atuais | Centralizar a descrição dos caminhos usados, sem migrar ou mudar sua resolução |
| Dashboard | Projeção de leitura atualizada sobre a biblioteca carregada | Após restaurar, precisa receber serviços recarregados e sem referências antigas |
| Backup | Não existe serviço de exportação/importação na interface | Criar fluxo local com validação, prévia, confirmação e recuperação |
| Transação | Biblioteca/coleções gravam individualmente | Substituições múltiplas não são atomicamente uma só operação; restauração exige coordenação e recuperação |
| Futuro remoto | Não há Supabase, autenticação, sincronização ou Drive | Preparar contratos/indicações sem implementar integrações nem apresentar modos falsamente operacionais |

Uma primeira associação pode salvar IdInterno antes do vínculo; falha no segundo arquivo é uma limitação conhecida. Os .bak independentes não garantem um par consistente. A Fase 7 deve enfrentar esse risco no fluxo de backup/restauração, sem alegar que todo o CRUD passou a ser uma transação multiarquivo.

## 2. Escopo recomendado de Configurações

Adicionar acesso **Configurações** na navegação, preservando os atalhos existentes e o nome Minhas Realizações.

Seções:

1. **Aparência:** acesso ao fluxo aprovado, com tema, modo, cores e restaurações da Fase 5. Uma única fonte de preferências; não duplicar controles/serviços que possam divergir.
2. **Biblioteca:** preferência de grade/lista sincronizada com a biblioteca e descrição do arquivo em uso. Restaurar aparência não deve alterar essa preferência.
3. **Armazenamento:** mostrar modo Local ativo, pasta/arquivos realmente utilizados e orientação para distinguir biblioteca original de cópia de testes.
4. **Backup local:** exportar, selecionar/validar backup, apresentar prévia e restaurar após confirmação.
5. **Recursos futuros:** explicar que nuvem/híbrido e Google Drive ainda não estão disponíveis, sem login, seletores funcionais fictícios ou requisições externas.

Manter o comportamento de salvamento já aprovado para aparência e visualização. Mudança de período do Dashboard continua apenas na sessão. Não acrescentar uma coleção de configurações sem necessidade demonstrada.

A exibição dos caminhos é informativa. **Não propor migração automática, troca do diretório de trabalho ou mudança da pasta da biblioteca nesta entrega.** Escolher o destino de um backup não altera a origem da biblioteca.

Usar tokens dinâmicos da Fase 5, contraste, mensagens textuais, foco por teclado e rolagem no mínimo 900×660. Não criar identidade/ícones novos.

## 3. Conteúdo do backup JSON

Recomendação: **um único arquivo JSON versionado**, contendo o conjunto da biblioteca e coleções, com preferências opcionais. Não exportar dois arquivos independentes como se fossem um backup consistente.

Envelope proposto, a formalizar antes da implementação:

- Identificador do formato, versão 1 e data/hora de exportação.
- Biblioteca no formato atual, incluindo blocos multimídia, IdInterno, metadados desconhecidos, IDs externos, dados pessoais e ausência de datas.
- Coleções com IDs, nomes, descrições e associações exatas.
- Preferências de aparência e visualização quando incluídas, separadas dos dados de negócio.
- Manifesto dos componentes incluídos/ausentes, contagens e integridade SHA-256 por componente, com regra determinística de cálculo.
- Sem caminhos de destino executáveis contidos no backup; o aplicativo decide onde gravar a partir da configuração local vigente.

A versão do envelope não obriga a alterar o formato dos arquivos ativos. Preservar os valores e campos, sem normalizar categorias antigas, deduplicar obras ou criar identidades/datas na exportação.

**Recomendação de UX:** exportar dados e coleções juntos; oferecer “Incluir aparência e visualização”, marcado inicialmente. Na restauração, “Restaurar também preferências” fica desmarcado inicialmente e mostra exatamente quais preferências existem no arquivo.

Arquivo de coleções ausente é explicitamente representado como ausência/estado vazio conhecido; nunca confundir isso com erro de leitura. A prévia deve mostrar que restaurar aquele backup deixará zero coleções se esse for seu conteúdo. Preferência ausente não apaga uma preferência atual por inferência.

Exportar somente os componentes previstos. Não percorrer pastas nem incluir .env, credenciais IGDB, .aws, qodana.yaml, skills/, logs, contratos, relatórios, backups antigos, temporários ou .lock. Não ler variáveis de ambiente para construir o arquivo. Notas/reviews e extensões são dados do usuário e não devem ser censuradas ou alteradas silenciosamente.

Capas continuam como referências/URLs; não baixar ou incorporar imagens, cache ou arquivos arbitrários indicados por metadados. O backup preserva dados, não garante a disponibilidade futura de uma imagem remota.

Hashes detectam corrupção, mas não comprovam autoria/autenticidade. Não apresentar o arquivo como criptografado ou protegido por senha; esses recursos não estão propostos.

## 4. Exportação consistente

1. Abrir seletor de destino; cancelamento não grava nem modifica estado.
2. Impedir destinos coincidentes com biblioteca, coleções, preferências, arquivos de recuperação ou seus aliases detectáveis. Validar também colisões com caminhos personalizados e links conhecidos.
3. Suspender ações de gravação dentro da instância por um coordenador de operações locais.
4. Obter os bloqueios dos componentes em ordem determinística, sem chamar gravadores que tentem adquirir o mesmo lock novamente.
5. Ler um snapshot, validar os dados e conferir hashes/estado contra os serviços carregados. Se houver mudança externa ou arquivo inválido, abortar com orientação para reabrir/corrigir, sem exportar o estado parcial como íntegro.
6. Montar/validar o envelope e escrever temporário no destino, fazer flush e concluir por substituição segura, mantendo backup anterior quando substituir um destino existente.
7. Reabrir o resultado e conferir estrutura/integridade antes de informar sucesso.
8. Liberar bloqueios e retomar a interface, inclusive em falha/cancelamento.

Não atribuir IDs ou salvar a biblioteca só para exportá-la. Não usar o conteúdo parcialmente carregado como se fosse uma biblioteca vazia.

Bloqueios coordenam instâncias compatíveis do aplicativo; editores externos e versões antigas podem não respeitar todos os locks, especialmente de preferências. Fazer verificações antes/depois e abortar se detectar divergência; não prometer proteção absoluta contra software que ignore a coordenação.

## 5. Seleção, validação e prévia de importação

Selecionar e validar nunca altera os arquivos ativos.

Validações planejadas:

- Arquivo legível, com limites documentados de tamanho/profundidade antes de alocar grandes estruturas.
- Identificador e versão de envelope suportados; JSON íntegro e sem propriedades duplicadas ambíguas.
- Presença do par biblioteca/coleções e tipos corretos.
- Integridade por componente; componentes faltantes não viram dados vazios por acidente.
- Validações existentes de biblioteca, multimídia, coleções e preferências reaproveitadas de forma de leitura.
- IdInterno único quando presente, IDs/nomes/vínculos válidos, notas/progresso conforme compatibilidade atual.
- Preservação de duplicatas pessoais legítimas e campos desconhecidos aceitos atualmente.
- Sem reinterpretação de SteamAppId ou migração automática de identificadores.
- Referências a obras ausentes já são permitidas pela Fase 4: contabilizar, avisar e preservar, sem reassociar por título nem eliminar vínculos. Recusar estruturas inválidas, como IDs vazios/repetidos onde já são proibidos.
- Preferências futuras/inválidas não podem ser aplicadas silenciosamente como padrões. Permitir restaurar apenas dados se o par for válido, informar a incompatibilidade e preservar as preferências atuais.

Mostrar origem do backup, data declarada, quantidades de obras por mídia, coleções, vínculos, referências ausentes, preferências disponíveis e diferenças de contagem em relação ao estado atual.

**Restauração substitui o conjunto de dados; não mescla.** Explicar substituições e reduções de contagem, inclusive backup vazio. Não deduplicar nem oferecer combinações automáticas sem regras previamente aprovadas.

Recomendação inicial: aceitar somente o novo envelope completo. Um backlog.json isolado ou um .bak antigo não fornece um par comprovado e será recusado como backup completo, com mensagem clara. Dados legados continuam suportados dentro do envelope. Importação/mesclagem de arquivos isolados pode receber proposta separada, sem improvisar vínculos.

Revalidar o arquivo escolhido no momento da confirmação para detectar se mudou desde a prévia.

## 6. Restauração e recuperação de falhas

Risco principal: biblioteca restaurada, coleções antigas e memória da janela apontando para o estado anterior. O fluxo precisa lidar tanto com erro de I/O quanto com interrupção do processo.

Proposta:

1. Exigir confirmação da substituição e escolha explícita sobre preferências. Oferecer cancelar até o ponto de início da aplicação.
2. Bloquear operações concorrentes da instância e adquirir locks dos alvos em ordem fixa.
3. Criar **backup externo completo e verificável do estado atual**, preservando inclusive a ausência de componentes; se falhar, não iniciar a substituição. Arquivos atuais ilegíveis não devem ser descartados: conservar seus bytes para recuperação e bloquear alegação de snapshot válido.
4. Preparar os novos conteúdos em área temporária local, validar com os mesmos leitores e verificar espaço/permissões. Nunca usar caminhos de destino fornecidos pelo arquivo importado.
5. Gravar um diário durável da operação, contendo estado, componentes, hashes esperados e referências controladas às cópias anteriores. Arquivos e diretórios de recuperação não entram no Git ou no backup exportado.
6. Substituir componentes de forma atômica **individual**, registrando cada etapa, e validar/recarregar o conjunto completo.
7. Publicar os serviços/estado visual apenas após sucesso do conjunto. Limpar seleção/formulários que apontem para obras substituídas, cancelar buscas pendentes e atualizar Dashboard, biblioteca, coleções, visualização e aparência.
8. Em falha controlada, tentar recuperar todo o estado anterior e verificar os hashes. Não informar sucesso parcial como restauração concluída.
9. Se a recuperação também falhar ou surgir modificação externa incompatível com os hashes esperados, preservar cópias/diário e bloquear novas gravações, indicando recuperação pendente.
10. Na próxima inicialização, reconhecer o diário **antes de carregar/gravar dados e preferências**, completar a recuperação segura ou manter o bloqueio. Não iniciar silenciosamente com um par misturado.

Uma falha no meio da confirmação não pode ser tratada como simples cancelamento. As opções da tela devem explicar quando a aplicação já começou.

**Limite declarado:** vários arquivos não passam a ter transação atômica por usar File.Replace. A proposta oferece coordenação, cópias anteriores e recuperação verificável. Não garante sobrevivência a todo tipo de falha física de disco, filesystem ou falta de energia. O teste de interrupção deverá comprovar os estados suportados e o bloqueio seguro nos demais.

A recuperação é parte necessária do backup local nesta fase; não equivale a implementar banco, sincronização ou transação conjunta de todo o CRUD.

## 7. Arquitetura proposta

| Arquivo/componente candidato | Responsabilidade |
|---|---|
| Models/DocumentoBackup.cs | Envelope versionado e metadados do conjunto |
| Models/PreviaRestauracao.cs | Contagens, avisos e diferenças sem escrita |
| Services/ServicoBackupLocal.cs | Exportação, prévia, confirmação e coordenação da restauração |
| Services/ValidadorBackup.cs | Validação estrutural, integridade e compatibilidade em leitura |
| Data/ArmazenamentoBackupJson.cs | Leitura/escrita do arquivo de backup |
| Data/RecuperacaoRestauracaoLocal.cs | Área temporária, diário, cópias e recuperação após interrupção |
| Configuracao/CaminhosDadosLocais.cs | Reunir os caminhos resolvidos preservando as regras atuais |
| MainWindow.Configuracoes.cs / MainWindow.axaml | Página, seletores de arquivo/pasta e mensagens; sem FileManager direto na interface |
| ComposicaoAplicativo e inicialização | Injeção e recuperação antes do carregamento normal |

São candidatos de desenho, não arquivos implementados. Reutilizar IArmazenamentoBiblioteca/IArmazenamentoColecoes e leitores existentes; extrair validação somente onde necessário. Não introduzir uma hierarquia genérica de plugins/provedores de armazenamento.

A pequena coordenação necessária à restauração pode exigir ajuste dos pontos de gravação e do armazenamento de visualização. Manter contratos anteriores, caminhos, permissões de recuperação explícita e comportamento dos testes; documentar cada alteração. Não transformar a preparação para nuvem em implementação remota.

## 8. Compatibilidade e limites do escopo

- Manter os arquivos ativos, IDs, datas ausentes, categorias/status desconhecidos aceitos, zeros legados, avaliações, reviews e metadados.
- Não mudar o diretório de armazenamento nem adotar pasta de usuário automaticamente.
- Não acrescentar data de conclusão, desejos, filtros novos, Dashboard novo, migração de banco ou APIs reais.
- Não implementar Supabase, Google Drive/OAuth, login, sincronização, promoção real ou funcionalidades da Fase 8.
- Não executar restaurações na biblioteca pessoal para testar.
- A versão antiga do aplicativo não é obrigada a ler o envelope novo; ele não substitui os arquivos ativos. Não prometer compatibilidade retroativa com um formato desconhecido.
- Regras de backup para múltiplos arquivos, cópias anteriores e recuperação precisam constar no relatório e na validação manual.

## 9. Sequência após aprovação

1. Conferir Git; registrar hashes; criar/verificar backup externo dos dados/preferências afetados antes de alterar persistência.
2. Executar os 1.365 testes atuais em ambiente isolado e registrar o baseline.
3. Especificar envelope e invariantes com exemplos sintéticos; criar testes de validação primeiro.
4. Implementar exportação consistente e sua UI mínima.
5. Implementar seleção/prévia, sem escrita, e confirmação com escopo explícito.
6. Implementar diário/restauração/recuperação e testar falhas por etapa antes de disponibilizar a ação.
7. Integrar Configurações e preferências existentes, mantendo temas e atalhos.
8. Executar regressão completa, build, inspeção dos seis conjuntos visuais, teclado e tamanho mínimo.
9. Registrar resultados reais, limitações, arquivos/commits e até cinco cenários manuais consolidados.
10. Entregar em feat/configuracoes e aguardar revisão; sem push, merge ou início da Fase 8.

Avaliar o aviso preexistente xUnit2017 registrado na Fase 6 ao trabalhar nos testes, sem suprimi-lo ou alterar expectativas para esconder falhas.

## 10. Matriz de testes planejada — §19.1

Todos os casos abaixo estão **planejados, não executados**. Os mínimos serão ampliados diante de falhas ou riscos; não representam aprovação da Fase 7.

| Funcionalidade | Mínimo | Casos relevantes a detalhar em IDs individuais |
|---|---:|---|
| Configurações/navegação/temas | 16 | Página e atalhos; seis temas/modos; cores personalizadas; foco/teclado; tamanho mínimo; estados vazios/erro; modo Local real; recursos futuros sem ação/rede |
| Preferências e compatibilidade | 16 | Ausente/antiga/válida; extensão; inválida/futura; grade/lista sincronizadas; restaurar aparência isoladamente; falha/retomada; recarga; conflito; mesmos caminhos |
| Exportação local | 20 | Vazio; legado; seis mídias; duplicatas; vínculos; preferência opcional; ausência versus erro; destino válido/cancelado; colisões/aliases; sem credenciais; falha/permissão/espaço; anterior preservado; leitura consistente; verificação final |
| Validação da importação | 24 | JSON truncado/nulo; profundidade/tamanho; versão; hash; componentes ausentes; duplicação de propriedades; enums; IDs; referências ausentes; metadados; datas/zeros legados; preferências futuras; arquivo modificado após prévia |
| Prévia/consentimento | 12 | Nenhuma escrita ao selecionar; contagens; redução/vazio; avisos; cancelar; dados versus preferências; escolha explícita; origem; backup inválido; seleção trocada; resumo antes de confirmar |
| Restauração do conjunto | 24 | Par completo; seis mídias; duplicatas; IDs/vínculos; desconhecidos preservados; preferências opcionais; sem migração; serviços recarregados; falha em cada destino; .bak antigos; locks; referências de formulários; bloqueio concorrente |
| Recuperação/interrupção | 24 | Falha antes/depois de cada substituição; erro no diário; processo interrompido; reinício; rollback íntegro; rollback falha; arquivos ausentes; versão futura do diário; modificação externa; bloqueio seguro; cópias preservadas |
| Integração e regressão | 16 | Dashboard; atividade 7/30; andamento; CRUD; filtros; grade/lista; coleções; IGDB simulada; cinco catálogos simulados; capas/fallback; aparência; offline; paths; nenhum dado pessoal em testes |

**Mínimo inicial: 152 cenários novos e reexecução dos 1.365 existentes**, projeção de pelo menos **1.517 testes automatizados**. A lista será decomposta por comportamento; não repetir o mesmo teste dez vezes.

Misturar testes unitários, integração de arquivos temporários, falhas controladas e Avalonia Headless/Skia. A recuperação após interrupção exige processo de teste isolado, não somente simular retorno de exceção. Validação de modelo não basta para provar seletores, confirmação, tema e recarga da interface.

Relatórios terão ID, cenário, esperado, observado, tipo e status; contagens planejados/executados/aprovados/reprovados/não executados; bugs/correções e reexecuções. Falsos sucessos e testes omitidos devem impedir a entrega.

## 11. Critérios de aceite

- Configurações organizadas e preferências antigas preservadas.
- Backup único versionado exportado e reaberto com integridade verificada.
- Nenhuma captura de credenciais/configurações pessoais alheias à biblioteca.
- Importação validada antes de qualquer escrita; substituição confirmada de forma compreensível.
- Biblioteca/coleções recuperadas coerentemente; vínculos, IDs e dados legados preservados.
- Preferências só restauradas por escolha explícita e sem perda silenciosa das atuais.
- Falhas/interrupções recuperam estado anterior verificável ou deixam gravações bloqueadas com cópias preservadas.
- Dashboard e demais páginas atualizados sem referências antigas ou contagens falsas.
- Recursos futuros identificados, sem redes remotas/serviços fictícios.
- Build e regressão aprovados; temas, teclado e layout inspecionados; limitações documentadas.
- Dados pessoais, backups antigos, Contrato.md, qodana.yaml, skills/ e histórico preservados.

## 12. Decisões propostas para aprovação

Aprovar o plano implica aceitar: um envelope completo; dados+coleções sempre juntos; preferências opcionais; restauração substitutiva, sem mesclagem; somente o envelope novo como entrada inicial; caminhos ativos inalterados; e recuperação por diário/cópias, com seus limites explícitos.

Se desejar importar arquivos isolados, mesclar bibliotecas, escolher uma nova pasta permanente, criptografar backups ou outra política de preferências, isso deve ser definido antes da implementação correspondente.

## 13. Estado atual

Fase 6 integrada exclusivamente por fast-forward; aprovação manual geral registrada sem atribuição individual. feat/configuracoes foi criada dessa base e contém somente este planejamento e as evidências de preservação.

Nesta preparação não houve implementação, alteração de código/modelo/teste, execução de restauração, build ou nova suíte. Os **1.365 aprovados** são os resultados históricos consultados; não são uma execução de hoje. O aviso de analisador e as limitações de dados/cache do Dashboard permanecem registrados.

Aguardando aprovação deste plano antes de iniciar a Fase 7.

# Planejamento da Fase 6 — Dashboard

**Estado:** proposta para revisão; implementação não autorizada ainda.
**Branch:** feat/dashboard, criada da main integrada em 4a7c0bb.
**Base funcional:** Fases 0 a 5 aprovadas; Avalonia 12.1.4 e .NET 10.
**Referências:** Contrato.md, especialmente §§9, 16 e 19.1; docs/FASE5.md; docs/FASE5_TESTES.md.

Nenhum arquivo de código, modelo, teste ou persistência foi alterado nesta preparação. A aprovação da Fase 5 e a autorização do planejamento não representam aprovação deste plano.

## 1. Diagnóstico do projeto

| Componente atual | Funcionamento e impacto no Dashboard |
|---|---|
| MainWindow.axaml / PaginaInicio | Boas-vindas, contagens, percentual por obras concluídas e três cartões recentes. Aproveitar a estrutura e navegação, sem nova janela. |
| MainWindow.axaml.cs / AtualizarColecao | Atualiza início, filtros, lista e coleções após operações locais. Será o ponto para atualizar o Dashboard após gravações confirmadas. |
| ServicoBiblioteca.ObterResumo | Conta estados persistidos: Jogando, Zerado e Na Fila. Reflete toda a biblioteca, independentemente dos filtros. |
| ServicoBiblioteca.ObterRecentes | Usa os três últimos registros na ordem inversa do arquivo; não representa uma janela de datas. Preservar o método para seus consumidores/testes atuais e criar a consulta temporal específica. |
| ItemBiblioteca.DataAdicao | DateTimeOffset opcional; ausente nos legados. FormularioObra preenche UTC no cadastro novo e preserva a data na edição. |
| DadosPessoaisBiblioteca | Possui categoria, status, nota, progresso e review. Não possui data de conclusão/início nem indicador independente de desejos. |
| TextosMidia | Adapta rótulos dos mesmos estados para seis mídias. Preservar esse contrato e os status legados, sem migração. |
| ServicoColecoes / IdInterno | Obras podem aparecer em várias coleções, mas os cálculos devem percorrer registros da biblioteca uma única vez. Duplicatas pessoais legítimas permanecem registros distintos. |
| Controls/CapaObra e CarregadorCapas | Carregamento assíncrono, limites/cache em memória e fallback. Reutilizar; não fazer novas consultas de catálogo para construir o Dashboard. |
| Aparencia/ e App.axaml | Seis combinações, cores personalizadas, contraste e recursos dinâmicos. Todos os novos componentes devem consumir os tokens existentes. |
| Persistência | Biblioteca e coleções em dois arquivos sem transação conjunta; visualização e aparência separadas. O plano recomendado mantém formatos e caminhos. |

A análise foi feita pelo código e pelos relatórios, sem executar o aplicativo sobre dados pessoais.

## 2. Escopo visual proposto

Reformular somente a página **Início**, mantendo o nome público Minhas Realizações, o ícone existente e o acesso às páginas aprovadas.

Ordem proposta:

1. Cabeçalho e seletor de **últimos 7 / últimos 30 dias**, com 30 dias como padrão de sessão.
2. **Adicionadas no período:** contagem e até seis cartões com capa, título, mídia, status e data real.
3. **Continue de onde parou:** até seis obras atualmente em andamento, com seu progresso específico e acesso ao formulário pessoal.
4. **Resumo atual da biblioteca:** total, em andamento, concluídas e na fila, com indicação explícita de que são estados atuais.
5. **Sem data de adição registrada:** bloco discreto/expansível com até seis obras e contagem total, para os legados continuarem acessíveis sem parecerem adições recentes.
6. Reserva visual de **Promoções**, com indicação clara de desenvolvimento, sem preços, descontos, lojas ou ações que aparentem integração disponível.

Botões “Ver biblioteca” dão acesso à biblioteca completa. Se uma ação aplicar filtro, seu efeito será explícito e testado; o plano inicial prefere navegação simples para preservar filtros já escolhidos. Clicar em uma obra abre a edição pessoal da instância correspondente, sem cadastrar cópias e sem gerar IDs apenas pela visualização.

Usar cartões com as dimensões/cantos já aprovados. Layout com quebra conforme a largura disponível, rolagem vertical, títulos com tooltip quando abreviados e ações alcançáveis em 900×660. Estados vazios, falha de carregamento e indisponibilidade de capa devem ter mensagens distintas.

A divisão entre “período” e “estado atual” evita sugerir que uma obra foi concluída neste mês só porque hoje está concluída.

## 3. Regras temporais e compatibilidade

### Atividade recente

- Usar exclusivamente DataAdicao conhecida; nunca usar data de modificação do arquivo, ID externo, posição no JSON ou data de capa como substituto.
- Período civil no fuso local: desde 00h do dia atual menos N−1 dias até o instante atual, incluindo hoje. N vale 7 ou 30.
- Receber um relógio e um fuso controláveis nos testes; usar o relógio/fuso do sistema na execução normal.
- Converter timestamps para comparar corretamente offsets diferentes. Excluir datas futuras da atividade recente; não alterar seus registros.
- Ordenar por instante de adição decrescente, com desempate estável pela ordem da biblioteca.
- Data ausente continua ausente. Exibir “Data não registrada” no bloco correspondente.
- Mostrar data em português brasileiro e informar o período considerado. Não gravar o seletor temporal em preferências nesta fase: persistir uma nova configuração não é necessário para o Dashboard funcionar.

### Em andamento e status

- Estado persistido Jogando identifica obras em andamento nas seis mídias; TextosMidia continua apresentando Jogando, Assistindo ou Lendo conforme o tipo.
- Excluir dessa seção Na Fila, Zerado, Pausado e Abandonado. Estados desconhecidos não serão reinterpretados automaticamente.
- Ordenação previsível: datas de adição conhecidas mais recentes primeiro; sem data depois, mantendo ordem estável. Não chamar isso de “último acesso” ou “última atividade”.
- Mostrar horas, episódios, temporada/episódio, minutos, capítulo/volume ou página/capítulo sem somar unidades diferentes nem inventar totais.
- Progresso ausente tem orientação textual; zeros legados permanecem preservados, conforme as regras existentes de edição.
- Obras sem ID interno continuam acessíveis por sua instância; leitura do Dashboard não deve gerar IDs ou deduplicar.

### Estatísticas

- Uma entrada da biblioteca conta uma vez, mesmo se estiver em várias coleções. Registros duplicados legítimos não são fundidos.
- Total inclui todos os registros carregados; contagens por status são exatas para os estados reconhecidos. Estados desconhecidos permanecem no total e podem ser identificados como “outros estados”.
- “Adicionadas no período” considera apenas datas conhecidas dentro do intervalo.
- “Concluídas”, “Em andamento” e “Na fila” são contagens atuais, não eventos históricos do período.
- Se mantido o percentual, seu rótulo será “Obras concluídas na biblioteca”; não representa percentual de horas/páginas/episódios consumidos.
- Não confundir Na Fila com uma lista de desejos comercial independente.
- Falha no carregamento não pode ser apresentada como biblioteca vazia nem como contagens zero confiáveis.

## 4. Decisões que precisam integrar a aprovação do plano

**Recomendação: implementar o Dashboard como leitura dos dados já disponíveis, sem mudar o JSON nesta fase.** Isso atende às seções viáveis do painel e evita criar um histórico falso.

| Informação solicitada pelo contrato | Dado disponível hoje | Tratamento recomendado |
|---|---|---|
| Adicionadas em 7/30 dias | DataAdicao opcional | Calcular quando há data e informar quantas obras não a possuem |
| Em andamento | Status atual e progresso | Exibir estado atual nas seis mídias |
| Concluídas no período | Não existe DataConclusao/histórico | Indicar que não há datas de conclusão registradas; não exibir zero como se tivesse sido calculado |
| Concluídas na biblioteca | Status Zerado | Mostrar contagem atual, separada das métricas temporais |
| Lista de desejos independente | Não há campo/regra específica | Não inferir por nome de coleção ou por Na Fila; manter “Na fila” como informação real |
| Última obra utilizada | Não há histórico de acesso | Não prometer ordenação por último uso |

**Alternativa, apenas se o proprietário quiser aprová-la explicitamente:** ampliar o escopo da Fase 6 para registrar data de conclusão e definir uma lista de desejos independente. Nesse caso, será apresentado um complemento com modelo, regras de transição/reabertura, validação, backups e testes antes da alteração. Incluir campo no bloco multimídia exige atenção: versões anteriores usam validação estrita e podem recusar um JSON com campo novo. Não realizar essa ampliação implicitamente.

A aprovação deve esclarecer se aceita o plano recomendado com essas limitações ou se deseja o complemento de persistência. Não há implementação parcial escondida nessa escolha.

## 5. Arquitetura proposta

- **Models/ResumoDashboard.cs:** projeção de leitura com listas, contagens, período e indicação de dados ausentes. Um record transporta o resultado sem modificar as obras.
- **Services/ServicoDashboard.cs:** regras de período, classificação, ordenação e estatísticas, recebendo a lista atual e relógio/fuso controláveis. Sem FileManager, HttpClient ou acesso direto a arquivos.
- **MainWindow.Dashboard.cs:** atualizar controles e encaminhar ações aos fluxos já existentes.
- **MainWindow.axaml / PaginaInicio:** organização dos blocos e templates, consumindo os recursos dinâmicos.
- **Configuracao/ComposicaoAplicativo.cs:** ligação simples do serviço se necessária. Evitar hierarquia de interfaces sem benefício demonstrado.
- **MainWindow.axaml.cs:** extrair apenas a atualização do início de AtualizarColecao, preservando filtros, grade/lista e coleções.
- **Tests/BacklogManager.Tests/Fase6/** e relatórios próprios, preservando os históricos.

O serviço funciona como um cálculo: recebe os registros e devolve o que mostrar. A tela decide como apresentar. Essa separação permite testar datas e contagens sem abrir janela, e testar a aplicação visual com Avalonia Headless.

Não mudar os métodos anteriores só para fazer o Dashboard funcionar. Avaliar seus consumidores antes de substituir chamadas na página inicial.

## 6. Temas, contraste e capas

Usar PanelBackground/PanelForeground para resumos e CardBackground/CardForeground para obras; textos secundários devem usar o token da superfície correta. Manter SelectionText separado de SelectionBrush e os avisos semânticos existentes.

Validar seis combinações prontas, personalizações representativas e cores de baixo contraste. Não acrescentar cores fixas locais. Se surgir token indispensável, defini-lo centralmente no resolvedor e testar seu contraste.

As capas virão das referências já salvas, usando CapaObra/CarregadorCapas. Cobrir sucesso, URL ausente/inválida, rede indisponível e cache. O painel permanece utilizável offline; capa remota pode cair no fallback após reiniciar porque o cache existente é apenas em memória. Cache persistente não faz parte desta proposta.

Sem novo nome, logotipo, fontes de metadados externas ou integração de preços.

## 7. Sequência de implementação — somente após aprovação

1. Conferir novamente Git e arquivos pessoais; registrar hashes e executar a suíte-base real de 1.266 testes em dados isolados.
2. Formalizar os exemplos de datas/status, inclusive fronteiras de fuso e dados ausentes; criar os testes do serviço.
3. Implementar projeção e cálculo sem alterar persistência.
4. Montar os blocos de Início com os tokens de aparência e os controles de capa existentes.
5. Ligar atualização ao carregamento e às alterações salvas; uma gravação fracassada não pode gerar contagem otimista. Atualizar a janela temporal ao voltar ao Início, inclusive após a meia-noite.
6. Verificar navegação, edição, remoção e coleções, sem contaminar o Dashboard com filtros de mídia da biblioteca.
7. Executar testes novos, regressão completa, build e inspeções visuais. Corrigir e repetir os cenários afetados.
8. Documentar casos, resultados, arquivos, limites e roteiro manual consolidado; criar commits organizados em feat/dashboard.
9. Entregar e aguardar aprovação. Não integrar a Fase 6, fazer push ou iniciar a Fase 7.

A implementação recomendada não exige gravação dos dados para gerar o painel. Caso surja necessidade de alterar persistência, interromper essa parte e preparar backup externo verificado do conjunto afetado antes de uma mudança explicitamente aprovada. A limitação existente de biblioteca/coleções em dois arquivos sem transação conjunta continua documentada.

## 8. Matriz planejada de testes — §19.1

Todos os cenários abaixo são **planejados e não executados**. Não são resultados da Fase 6. Contagens são mínimo inicial e serão ampliadas conforme falhas e riscos encontrados.

| Grupo | Mínimo | Cenários distintos previstos |
|---|---:|---|
| Atividade e período | 20 | Vazio; um recente; um antigo; 7/30 dias; início/fim; hoje; offsets; virada do dia; horário de verão; data ausente; futura; empate; ordenação estável; limite de cartões; sem gravação; todas as mídias; troca de período; relógio controlado |
| Continue/progresso | 12 | Seis mídias com progresso adequado; sem progresso; zero legado; estados fora de andamento; desconhecido; mesma obra em coleções; registros duplicados independentes |
| Resumo e integridade | 18 | Vazio; todos os status; estado desconhecido; seis mídias; sem datas; mistura de datas; futuro; contagem atual versus período; divisão por zero; múltiplas coleções; filtros não contaminam; edição/exclusão confirmadas; falha ao salvar sem efeito otimista |
| Capas e destaques | 12 | Carrega; sem URL; URL inválida; erro HTTP; offline; cache; cancelamento; recorte; sem distorção; fallback por cartão; título longo; abertura da instância correta |
| Interface e temas | 18 | Seis combinações prontas; cores independentes; contraste ajustado; foco/hover/seleção; teclado; 900×660; janela maior; rolagem; dados ausentes; falha de carga; confirmação legível; reserva sem preços falsos ou chamada externa |
| Integração e regressão do painel | 16 | Cadastro; edição; remoção; falhas de gravação; reinício; retorno ao início; meia-noite; coleções; filtros; grade/lista; preferências; IGDB simulada; catálogos simulados; IDs legados; JSONs inalterados pela leitura; biblioteca inválida |

**Mínimo inicial: 96 cenários novos**, além da reexecução dos **1.266 existentes**; projeção mínima de 1.362 testes automatizados, sujeita à decomposição/expansão dos casos. Não contar repetições do mesmo teste como casos novos.

Os cenários terão IDs individuais e tabela com esperado, observado, tipo e status. Testes de modelo não substituem testes de interface: usar Avalonia Headless/Skia para renderização/recursos e inspeção visual real das capturas. Registrar bloqueios de ambiente sem aprová-los ficticiamente.

Usar somente dados sintéticos e diretórios temporários próprios, identificados no relatório. Nunca testar alterações no JSON pessoal ou nas pastas manuais do proprietário. Não depender de credenciais IGDB/rede reais. Build previsto: dotnet build; execução prevista: dotnet test, com TRX novo em diretório da Fase 6 e gerador de relatório próprio.

## 9. Critérios de aceite

- Períodos corretos e datas ausentes/futuras tratados sem invenção.
- Seis mídias e registros legados preservados, sem conversão/deduplicação.
- Contagens por obra pessoal, independentes das coleções e dos filtros da biblioteca.
- Estatísticas sem confusão entre data de adição, conclusão e estado atual.
- Coleções, cadastro, edição, exclusão, grade/lista e preferências anteriores funcionando.
- Seis conjuntos visuais e cores personalizadas coerentes, com teclado, foco e contraste.
- Falha de biblioteca distinguida de estado vazio; offline utilizável com fallback de capa.
- Reserva de promoções sem backend, valores ou descontos inventados.
- Build e todos os testes aplicáveis aprovados; resultados e limites documentados.
- Nenhuma escrita indevida na biblioteca, coleções ou preferências ao ler o Dashboard.

## 10. Estado desta preparação

Aprovação manual geral da Fase 5 registrada; instruções da Fase 5 versionadas; integração por fast-forward concluída. A branch feat/dashboard contém somente documentação de planejamento e evidências de preservação, sem implementação.

Nesta preparação não foram executados build, testes automatizados novos ou testes manuais do aplicativo: as alterações são documentais e a integração mantém exatamente o código aprovado. Os 1.266 aprovados citados são os resultados anteriores consultados, não uma execução de hoje.

Próximo passo depende da aprovação deste planejamento e, em especial, da decisão sobre datas de conclusão e lista de desejos. Fase 7 não iniciada.

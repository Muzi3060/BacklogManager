# Fase 6 — Dashboard

> Atualização em 29/10/2026: a Fase 6 foi oficialmente aprovada após validação manual geral do proprietário no Rider. Veja [APROVACAO_FASE6.md](APROVACAO_FASE6.md). O texto abaixo preserva o registro da entrega anterior e suas pendências naquela data; não atribui execução individual a cenários não discriminados.

Implementação na branch **feat/dashboard**, base **6bd4213**, descendente da main **4a7c0bb** com a Fase 5 aprovada. Esta entrega aguarda revisão; não houve push, merge da Fase 6 ou início da Fase 7.

## Funcionalidades

O Início agora prioriza estatísticas e acompanhamento:

1. Totais atuais: registros, em andamento, concluídos e na fila.
2. Distribuição pelas seis mídias e pelos cinco status conhecidos, mais “Outros estados”. Barras horizontais mostram a participação no total e números permitem leitura sem depender da cor. Usam ProgressBar já disponível, sem biblioteca de gráficos nova.
3. Atividade de adição com seletor de 7/30 dias. Conta todas as adições conhecidas no período e apresenta até seis entradas com data, mídia, status e miniatura de apoio.
4. Continue de onde parou: até seis obras atualmente em andamento com progresso adequado à mídia, sem calcular percentuais pessoais.
5. Obras sem data em bloco explicitamente identificado, com amostra de até três registros na ordem inversa da biblioteca. Não são apresentadas como adições recentes.
6. Reserva textual de promoções futuras, sem ofertas, preços, descontos ou chamadas de API.

As miniaturas novas medem 60×64, abaixo da hierarquia estatística. O bloco legado conserva as capas e cantos aprovados anteriormente. Obras de demonstração são identificadas. Clicar nas entradas abre a edição pessoal existente, sem duplicar registros ou atribuir IDs pela leitura.

O percentual global existente é a proporção de registros concluídos, com explicação explícita. Não é porcentagem pessoal de horas, páginas ou episódios. “Na Fila” continua sendo o status atual, sem ser convertido em lista de desejos.

## Dados e arquitetura

- Models/ResumoDashboard.cs: records de leitura com contagens, distribuições e entradas de apresentação.
- Services/ServicoDashboard.cs: cálculo independente de arquivos, APIs e coleções; recebe relógio/fuso controláveis para testes.
- MainWindow.Dashboard.cs: ligação entre resultados, controles e fluxo de edição.
- MainWindow.axaml: somente a organização visual do Início e seus templates.
- MainWindow.axaml.cs: delega o início ao serviço e mantém atualização da biblioteca/coleções.
- TelaTeste: injeção opcional do serviço, sem usar relógio real nos testes temporais.

Um record é um objeto que transporta os dados calculados. A projeção guarda referências às obras existentes; não cria cópias na biblioteca. Listas de leitura protegem o resultado. O serviço funciona como um cálculo: recebe registros e devolve o que apresentar.

Os métodos anteriores ObterResumo/ObterRecentes permanecem intactos para seus consumidores. A nova atividade datada não reutiliza a ordem do arquivo como evidência temporal.

Nenhum formato JSON, caminho, propriedade persistida ou regra de cadastro foi alterado. Não há data de conclusão, desejos independentes, migração de registros, deduplicação ou mudança de identificadores Steam/IGDB. O período é preferência apenas da sessão.

### Regras temporais

O intervalo começa na data local de hoje menos N−1 dias e termina no instante atual, incluindo hoje. A comparação por data civil no fuso escolhido evita inventar um offset para uma meia-noite inexistente durante horário de verão. Instantes futuros são excluídos e contabilizados separadamente, sem mudar o registro.

A ordenação por adição é decrescente e estável em empates. Sem data, o registro não entra na atividade; continua nos totais e pode aparecer no acompanhamento se seu status for Jogando. Os rótulos das seis mídias continuam vindo de TextosMidia.

Conclusões por período são apresentadas como indisponíveis, pois faltam datas reais. As contagens de concluídas/em andamento/na fila são explicitamente atuais. Os totais contam cada registro pessoal uma vez, sem multiplicação por coleções ou fusão de duplicatas legítimas.

### Atualização e falhas

O painel é recalculado após operações confirmadas, ao alternar o período e ao voltar ao Início. Falha de gravação mantém o estado salvo anterior. Falha de carregamento apresenta estatísticas indisponíveis e “—”, em vez de simular biblioteca vazia com zero confiável.

O relógio não gera um processo de atualização em segundo plano: se o Início permanecer aberto após meia-noite, voltar à página ou trocar o período recalcula a janela. Alterações externas de arquivos seguem exigindo reabertura, conforme a arquitetura anterior.

## Temas e preservação

Todos os componentes consomem os tokens de superfície/texto da Fase 5. Não foram adicionados hexadecimais locais nem pacotes. As seis combinações prontas foram renderizadas; a regressão de aparência e as cores personalizadas continuam cobertas.

CapaObra e CarregadorCapas permanecem os anteriores: carregamento assíncrono, cache em memória e fallback. As imagens não dominam o Dashboard. O cache não se tornou persistente em disco.

Dados pessoais, preferências presentes, Contrato.md, instruções, relatórios anteriores, branches e backups foram conferidos por hash. qodana.yaml e skills/ não foram editados nem adicionados. Nenhum teste acessou a biblioteca pessoal ou a pasta manual do proprietário. Não houve migração de persistência, portanto não foi necessária modificação/restauração dos dados nem dos backups existentes.

A limitação de biblioteca e coleções em dois arquivos **sem transação conjunta** permanece. Preferências visuais ficam separadas. Um futuro backup deve preservar um par coerente; a Fase 7 não foi antecipada.

## Testes e resultados

[Tabelas individuais](FASE6_TESTES.md), [JSON dos resultados](resultados-fase6.json) e [evidências/verificações](verificacoes-fase6.json) registram resultados reais.

| Grupo | Funcionalidade | Executados/aprovados |
|---|---|---:|
| DA | Períodos, datas reais, offsets, fronteiras e horário de verão | 21 |
| DC | Andamento, seis mídias, progresso ausente e zero legado | 12 |
| DE | Totais, distribuições, estados desconhecidos e integridade | 18 |
| DU | Interface, seis temas, período, teclado e tamanho mínimo | 20 |
| DK | Capas discretas, cache, falhas, fallback e edição | 12 |
| DX | Coleções, falhas de gravação, reinício e dados preservados | 16 |
| **Novos** | **Pelo menos dez cenários por funcionalidade** | **99** |
| Regressão | Fases 0 a 5 e ajustes | 1266 |
| **Suíte final** | **1365 planejados/executados; zero reprovados ou não executados** | **1365** |

A base foi executada antes das alterações: 1.266/1.266. A primeira rodada nova de serviço passou 50/50; a rodada ampliada passou 98/98; o caso adicional de horário de verão levou o total novo a 99. Rodadas intermediárias completas registraram as duas diferenças descritas abaixo; a suíte final passou integralmente. Repetições não contam como cenários novos.

**Build completo final:** dotnet build --no-restore --no-incremental, zero erros e **um aviso preexistente xUnit2017**, em Tests/BacklogManager.Tests/Fase5/EstadosVisuaisTests.cs:39, sobre preferir Assert.Contains a Assert.True em uma asserção de teste. O mesmo aviso já estava presente no build/teste-base desta execução. Não afeta o resultado dos testes nem o aplicativo; não foi ocultado ou desabilitado.

Comandos de validação:

~~~bash
dotnet build --no-restore --no-incremental
dotnet test --logger 'trx;LogFileName=fase6-entrega.trx' --results-directory /tmp/backlogmanager-fase6-resultados
python3 -B scripts/gerar_relatorio_fase6.py /tmp/backlogmanager-fase6-resultados/fase6-entrega.trx
~~~

### Diferenças encontradas e correções

- CR11 exigia a altura de 72 da capa antiga; a primeira composição reduzia esse bloco para 48. A dimensão aprovada foi restaurada para o bloco sem data. As novas entradas usam suas próprias miniaturas de apoio e testes.
- TX13 rejeitava a palavra “Jogos” em qualquer texto. A distribuição agora lista legitimamente os nomes das mídias oficiais. A auditoria passou a aceitar esses rótulos, mantendo a rejeição a mensagens exclusivas de jogos. Nenhum teste foi excluído.
- A inspeção visual mostrou que o fallback “Sem capa” ficava cortado em uma miniatura de 32 pixels. A largura foi ampliada para 60; a capa continua secundária e o texto fica legível.
- A data final do período passou a vir da mesma projeção/fuso do cálculo, evitando que a tela recompute o rótulo com outro fuso.
- Totais gerais/estatísticas de período foram separados e registros sem data receberam título explícito, evitando contagens temporalmente falsas.

### Evidências adicionais

Capturas Avalonia Headless/Skia de seis temas/modos e janela 900×660 foram geradas e inspecionadas. Diretório efêmero: /tmp/backlogmanager-fase6-resultados/imagens. Inclui seis-temas-dashboard.png, minimo-topo.png e minimo-fim.png. A regressão mantém seus destinos temporários históricos.

Smoke XS06: aplicação desktop com JSONs fictícios de biblioteca/coleções/aparência, sem credenciais IGDB, aberta por oito segundos; processo permaneceu ativo, log vazio e os três arquivos ficaram com os mesmos hashes. Não houve interação manual nem uso da biblioteca real.

Os resultados automatizados não equivalem a nova pesquisa autenticada IGDB ou validação em plataformas, leitores de tela e escalas físicas de monitor indisponíveis. Os artefatos brutos em /tmp podem ser removidos; contagens/resultados/hashes ficam registrados nesta documentação.

## Até cinco cenários manuais no Rider

Todos os cenários abaixo estão **não executados — aguardando proprietário**. Não representam aprovação da Fase 6.

Há um conjunto fictício opcional em /tmp/backlogmanager-fase6-resultados/rider-simulado, com seis mídias e datas distintas. Datas foram geradas no momento da preparação; se a validação ocorrer muitos dias depois, use dados simulados atualizados. Configure essa pasta, ou outra cópia isolada, em Run/Debug Configuration → Working directory. Nunca use os dados originais ou a pasta de backups como ambiente de testes.

| ID | Passos consolidados | Esperado | Observado / status |
|---|---|---|---|
| M01 | Abrir o Início e conferir totais/distribuições com mídias e status mistos | Contagens por registro, barras proporcionais e estatísticas antes das capas | Aguardando proprietário / Não executado |
| M02 | Alternar 7/30 dias usando obras de hoje, dez dias atrás, antigas, sem data e futuras | Só datas reais do intervalo entram; totais atuais permanecem; ausência/futuro são informados | Aguardando proprietário / Não executado |
| M03 | Abrir uma obra em andamento, editar status/progresso e voltar; depois excluir uma obra na cópia | Edição existente e resumo atualizado após salvar; nenhum percentual pessoal inventado | Aguardando proprietário / Não executado |
| M04 | Percorrer temas/modos e cores personalizadas; usar Tab e janela mínima | Leitura, contraste, foco, rolagem e fallback de capa utilizáveis | Aguardando proprietário / Não executado |
| M05 | Reiniciar, usar grade/lista/filtros e coleções, consultar o Início sem credenciais/rede | Dados/vínculos/preferências preservados; organização local funciona; reserva de promoções não simula ofertas | Aguardando proprietário / Não executado |

## Arquivos e entrega

Criados: Models/ResumoDashboard.cs; Services/ServicoDashboard.cs; MainWindow.Dashboard.cs; sete arquivos em Tests/BacklogManager.Tests/Fase6/ (apoio, atividade, acompanhamento, estatísticas, interface, capas e integração); scripts/gerar_relatorio_fase6.py; docs/APROVACAO_PLANO_FASE6.md; este relatório, tabela de testes, resultados e verificações.

Modificados: MainWindow.axaml; MainWindow.axaml.cs; Tests/BacklogManager.Tests/TelaTeste.cs; Tests/BacklogManager.Tests/AjustesFase3/TextosTests.cs, somente na regra de auditoria afetada.

Nenhum arquivo de dados, preferências, contrato ou persistência foi alterado. A main permanece em 4a7c0bb. Os commits de implementação/testes/documentação são informados na entrega; a Fase 6 continua exclusivamente em feat/dashboard para revisão.

Commits de implementação/testes: **f51cbeb** e **6e91037**. A documentação recebe commit separado, informado na entrega.

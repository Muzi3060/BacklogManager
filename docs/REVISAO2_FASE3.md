# Segunda revisão dos ajustes da Fase 3

## Entrega pendente de aprovação

Esta rodada continua exclusivamente em **`fix/biblioteca-ajustes`**, a partir de `7cf3afe`. As demais branches foram preservadas, incluindo `feat/biblioteca` em `bbcb99f` e `main` em `64fb4fb`. Não houve push, merge, exclusão de branches ou início da Fase 4.

O proprietário relatou outra rodada manual no Rider, com aparente funcionamento das correções anteriores, e apontou contorno externo na grade, margens nas miniaturas e validação numérica tardia. Esse relato é geral: não atribui aprovação individual aos cenários manuais anteriores e **não aprova a Fase 3**. Os relatórios anteriores foram mantidos como histórico; este documento complementa [AJUSTES_FASE3.md](AJUSTES_FASE3.md).

## Causas e correções

### Contorno, foco e altura da grade

Foram identificadas três fontes possíveis e verificadas no controle renderizado:

1. `Border.card`, compartilhado no aplicativo, acrescentava borda de um pixel e raio de dez pixels ao cartão inteiro. Os estados de hover/seleção realçavam essa borda.
2. Os estados do tema Fluent pintavam o fundo de `PART_ContentPresenter`, incluindo os dez pixels de margem além do cartão. A inspeção encontrou uma faixa azul na seleção e uma faixa cinza no hover. Definir apenas o fundo transparente dentro do template não bastava: os estilos de estado do tema tinham precedência.
3. O adorno padrão de foco poderia acrescentar outra indicação externa. Foi substituído por indicação interna, mantendo foco e seleção acessíveis.

Na grade, o cartão agora tem **borda zero e raio zero**. Os estados do `ContentPresenter` têm fundo transparente explicitamente definido por estilos para seleção, hover e foco. A seleção possui uma barra interna e o texto “✓ Selecionado”; o foco por teclado possui o texto “Foco” e realce interno. O provider padrão de acessibilidade continua informando a seleção, e os botões de edição/exclusão continuam habilitados conforme o item selecionado.

A altura fixa de 352 pixels, ou 270 no modo compacto, foi removida. O cartão usa a altura do conteúdo e alinhamento superior. A capa continua com 174 pixels, ou 104 no tamanho compacto. A faixa interna de indicação reserva 14 pixels para evitar saltos de layout ao selecionar. Cartões com progresso ou identificação de simulação ocupam apenas o espaço adicional necessário.

Os testes CG13 e CG14 comparam pixels **fora do cartão**, além de verificar suas propriedades. Isso detectou uma camada residual que não seria identificada apenas por `BorderThickness == 0`.

### Miniaturas da lista

A lista ainda usava `Uniform`, que preservava toda a imagem dentro da área de 64×96 e deixava faixas laterais em algumas proporções. Agora utiliza **`UniformToFill`**, como a grade: preenche a área proporcionalmente, sem deformação, com possível recorte nas bordas.

Foram testadas imagens horizontais, verticais e quadradas geradas em memória. As verificações de pixels nas duas laterais confirmam preenchimento real. Ausência de capa e URL inválida mantêm o fundo padrão preenchido e “Sem capa”. Também foram verificados os tamanhos 900×660 e 1280×900, seleção e alternância entre visualizações usando o cache da sessão.

### Entrada imediata de posições

Os quatro campos afetados passaram a usar `PosicaoNumerica`, um controle específico derivado de `NumericUpDown`:

- Página de livro.
- Capítulo de livro.
- Capítulo de mangá.
- Volume de mangá.

Regras visíveis antes de salvar:

| Entrada/ação | Resultado imediato |
|---|---|
| Campo vazio | Permanece `null`: progresso não informado. |
| Inteiro a partir de 1 | Aceito. |
| Zero ou inteiro negativo novo | Normalizado para 1 no valor e no texto exibido. |
| Fração ou texto não numérico | Rejeitado; restaura o último inteiro válido. Se estava vazio, permanece vazio. |
| Inteiro acima de `int.MaxValue` ainda representável pelo conversor | Limitado a `int.MaxValue`. Texto sem conversão válida é rejeitado. |
| Limpar texto ou informar apenas espaços | Volta a não informado, sem impor 1. |
| Seta para cima no vazio | Começa em 1. |
| Seta para baixo no vazio | Continua vazio. |
| Seta para baixo em 1 | Mantém 1, sem produzir zero. |
| Incrementar/decrementar posição válida | Passos de 1, respeitando os limites. |

O `NumericUpDown` padrão pode preencher um campo vazio com `Maximum` ao diminuir. Esse caso foi interceptado. A implementação consulta os pontos de extensão da [versão oficial 12.1.4 do controle Avalonia](https://github.com/AvaloniaUI/Avalonia/blob/12.1.4/src/Avalonia.Controls/NumericUpDown/NumericUpDown.cs), sem copiar ou alterar o pacote.

Capítulos continuam inteiros, como no modelo já existente, com incremento inicial 1. Não foi introduzido suporte a capítulos fracionários. Os tooltips explicam as regras de normalização e a exceção dos registros legados.

**Nota pessoal inalterada:** continua usando seu próprio `NumericUpDown`, com escala de 0 a 5 e valores fracionários. Horas, minutos, episódios assistidos e temporada/episódio também não receberam essas regras de posição.

### Zeros legados e validação ao salvar

`PreencherProgresso` usa explicitamente `CarregarValorLegado`. Um zero já existente é mostrado como zero, com orientação de que é legado, sem modificar o JSON. Nesse estado, o mínimo do controle é temporariamente zero. Ao limpar ou substituir o valor, a exceção termina e o mínimo volta a 1. Aumentar o zero legado leva a 1; diminuir não cria valores negativos.

Editar nota/review mantendo o zero legado continua permitido. Limpar ou corrigir a posição exige ação explícita. Não há migração em massa ou alteração automática de dados. As validações do formulário e dos comandos de salvar continuam como proteção adicional; os testes anteriores de recusa de posições inválidas no modelo/serviço foram executados novamente.

## Organização em C#

`PosicaoNumerica` concentra apenas o comportamento dos campos de posição. `OnCoerceValue` protege atribuições de valor, `OnTextChanged` sincroniza imediatamente texto e valor, e `OnSpin` trata a seta para baixo no vazio. `StyleKeyOverride` reaproveita a aparência do controle Avalonia existente.

A distinção entre `null` e zero é mantida: `null` representa ausência; zero só permanece quando carregado como dado legado. O método explícito de carregamento evita aplicar essa exceção a cadastros novos. Nenhuma interface, persistência ou arquitetura ampla foi refeita e nenhum pacote novo foi acrescentado.

## Preservação

O contrato versão 1.1 foi relido e não foi alterado. Não havia modificações rastreadas pendentes ao iniciar. Os arquivos não rastreados `qodana.yaml` e `skills/system/run-workflow/SKILL.md` permanecem intactos e fora dos commits.

A biblioteca pessoal permaneceu com **9 registros e 1535 bytes**, sem alteração de bytes ou caminho. Antes das mudanças no preenchimento da interface foi criado um backup externo verificado:

`${HOME}/Backups/BacklogManager/revisao2-fase3-20261028-154612/backlog.json`

Pasta com permissão 700, cópia com permissão 600 e `verificacao.json` ao lado. Original e backup têm SHA-256:

`d33b826a57148dc8c7017ae22e15f6898163ba7d5d5eea57a7ba64b6ddaac810`

Todos os relatórios e backups anteriores tiveram seus hashes conferidos. **Os testes desta rodada usaram exclusivamente dados simulados; nem a biblioteca pessoal nem uma cópia de seus registros foram usadas para exercitar o aplicativo.** A cópia externa foi criada apenas para preservação. O caminho normal de armazenamento continua relativo ao diretório de trabalho, sem mudança automática.

Não houve alteração do formato JSON, da seleção IGDB, dos catálogos simulados, do armazenamento das preferências ou das regras de compatibilidade introduzidas nas rodadas anteriores.

## Arquivos

Criados:

- `Controls/PosicaoNumerica.cs`
- `Tests/BacklogManager.Tests/Revisao2/PosicaoImediataTests.cs`
- `Tests/BacklogManager.Tests/Revisao2/GradeRefinadaTests.cs`
- `Tests/BacklogManager.Tests/Revisao2/MiniaturasTests.cs`
- `scripts/gerar_relatorio_revisao2.py`
- `docs/REVISAO2_FASE3.md`
- `docs/REVISAO2_FASE3_TESTES.md`
- `docs/resultados-revisao2-fase3.json`
- `docs/verificacoes-revisao2-fase3.json`

Modificados:

- `MainWindow.axaml`, `MainWindow.Multimidia.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/CapasGradeTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/PosicoesJanelaTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/TextosTests.cs`
- `Tests/BacklogManager.Tests/BuscaJanelaTests.cs`
- `Tests/BacklogManager.Tests/Fase2/InterfaceMultimidiaTests.cs`
- `Tests/BacklogManager.Tests/Fase3/InteracaoBibliotecaTests.cs`
- `Tests/BacklogManager.Tests/Fase3/VisualizacoesTests.cs`

São **9 arquivos criados e 9 modificados** nesta rodada, em relação a `7cf3afe`. Os destinos de capturas foram movidos para a pasta de artefatos desta revisão. Os relatórios antigos não foram sobrescritos.

## Testes e resultados

Compilação: **sucesso, zero erros e zero avisos**. Suíte final: **909 aprovados, zero reprovados, zero não executados** — 851 cenários anteriores executados novamente e **58 novos**.

| Funcionalidade | IDs novos | Cenários | Aprovados |
|---|---|---:|---:|
| Entrada imediata, setas, teclado, vazio, legado e escala da nota | NI01–NI32 | 32 | 32 |
| Contorno, altura, seleção, foco, acessibilidade e pixels externos | CG01–CG14 | 14 | 14 |
| Miniaturas da lista, proporções, padrões e tamanhos de janela | ML01–ML12 | 12 | 12 |
| Regressão anterior | Todos os cenários anteriores | 851 | 851 |
| **Total** | | **909** | **909** |

Cada funcionalidade modificada possui mais de dez cenários relevantes. A regra compartilhada dos quatro campos é testada por NI; a integração de todos eles por NI27 e pelos cenários UP adaptados. A regressão NP01–NP12, NC01–NC15 e NV01–NV12 mantém a cobertura específica de página, capítulo e volume e da proteção ao salvar.

A [tabela individual completa](REVISAO2_FASE3_TESTES.md) contém cenário, esperado, observado, tipo e status. [resultados-revisao2-fase3.json](resultados-revisao2-fase3.json) guarda os resultados/durações do TRX real. [verificacoes-revisao2-fase3.json](verificacoes-revisao2-fase3.json) registra as rodadas, hashes e preservação das branches.

| ID | Verificação adicional | Esperado | Observado | Tipo | Status |
|---|---|---|---|---|---|
| XR01 | Inicialização desktop com JSON inteiramente simulado e sem credenciais | Abrir sem erro e não regravar os dados simulados | Processo aberto por oito segundos e encerrado pelo verificador; log vazio; JSON simulado intacto | Automatizado de inicialização | Aprovado |

**Planejados: 916** — 909 da suíte, uma inicialização e seis validações manuais. **Executados: 910; aprovados: 910; reprovados atuais: zero; não executados: seis manuais.** Capturas Headless e inicialização desktop não equivalem à validação interativa no Rider.

### Falhas encontradas e novas execuções

| Execução | Resultado | Interpretação/correção |
|---|---|---|
| Base anterior | 851/851 | Estado inicial registrado. |
| Primeira regressão | 840/851 | Onze expectativas antigas ainda esperavam erro somente ao salvar ou `Uniform` na lista. Adaptadas às decisões desta rodada, sem excluir cenários. |
| Primeiros testes novos | 54/54 | Entrada imediata, aparência e acessibilidade verificadas. |
| Integração inicial | 905/905 | Toda a suíte então existente passou. |
| Reprodução da margem residual e cliques das setas | 2/3 | CG13 identificou faixa azul na margem externa apesar da borda zero; cliques das setas passaram. |
| Integração com proteção de pixels | 907/909 | CG13 e CG14 identificaram camada do tema na seleção e no hover. Valores transparentes definidos apenas dentro do template não resolveram a precedência. |
| Diagnóstico da árvore visual | 0/1 | Confirmou `ContentPresenter` com fundo do tema, apesar do fundo transparente do contêiner. |
| Repetição após estilos explícitos de estado | 58/58 | Comparações de pixels e demais cenários novos passaram. |
| Suíte final | 909/909 | Toda a regressão repetida após a correção efetiva. |

Os cenários anteriores UP01–UP06/UP11–UP12 e UM10 passaram a verificar normalização/rejeição imediata. HV02/HV09 passaram a exigir preenchimento proporcional também na lista. Os relatórios históricos conservam os resultados e regras da época em que foram gerados.

Foram inspecionadas as capturas da grade com seleção/foco e da lista com miniatura preenchida no tamanho mínimo. Os artefatos estão em `/tmp/backlogmanager-revisao2-resultados/`. A comparação de pixels da margem protege contra retorno da camada azul/cinza do tema.

## Roteiro curto no Rider — pendente

Use `fix/biblioteca-ajustes` e uma pasta de trabalho temporária com dados **simulados**, sem apontar para a biblioteca pessoal. Pode reutilizar o JSON de demonstração criado em `/tmp/backlogmanager-revisao2-resultados/desktop-simulado/backlog.json`, copiando-o para outra pasta temporária antes da validação. No Rider, ajuste **Run → Edit Configurations → Working directory** para essa pasta. O arquivo de demonstração contém apenas um livro fictício com página zero, destinado à verificação legada.

| ID | Cenário manual | Esperado | Observado | Tipo | Status |
|---|---|---|---|---|---|
| MR01 | Grade em tamanho normal e 900×660: hover, seleção e Tab/setas | Nenhum contorno ou faixa externa; seleção/foco identificados internamente; altura sem espaço fixo excessivo | Aguardando proprietário | Manual / Rider | Não executado |
| MR02 | Lista com capas de proporções diferentes e sem capa | Miniaturas preenchidas sem distorção; imagem padrão legível; comportamento consistente ao redimensionar | Aguardando proprietário | Manual / Rider | Não executado |
| MR03 | Página e volume: zero, negativos, texto, frações, vazio e setas | Normalização/rejeição imediata conforme a tabela; diminuir vazio conserva ausência | Aguardando proprietário | Manual / Rider | Não executado |
| MR04 | Capítulos de livro/mangá: digitação e setas | Inteiros a partir de 1, incremento 1 e ausência permitida | Aguardando proprietário | Manual / Rider | Não executado |
| MR05 | Livro simulado com zero legado: editar só review/nota; depois limpar/corrigir posição | Zero preservado se inalterado; limpeza explícita vira ausência; correção positiva não permite novo zero | Aguardando proprietário | Manual / Rider | Não executado |
| MR06 | Nota pessoal com 0, fração e 5; salvar/reabrir registros simulados | Escala da nota inalterada e dados recarregáveis; nenhum efeito dos controles de posição sobre a nota | Aguardando proprietário | Manual / Rider | Não executado |

Nenhuma validação manual individual desta rodada foi atribuída como aprovada.

## Limitações e estado final

- Preenchimento proporcional pode recortar bordas da imagem; não altera a proporção do arquivo nem o JSON.
- Capítulos permanecem inteiros por compatibilidade com o modelo atual.
- Normalização só altera o valor exibido no formulário; a persistência continua exigindo o comando de salvar. Zeros antigos não são migrados automaticamente.
- A grade continua sem virtualização; não houve benchmark de bibliotecas grandes.
- O cache remoto continua limitado à sessão, conforme a limitação anterior. A IGDB não foi modificada nem autenticada novamente nesta rodada.
- A validação interativa no Rider e a aprovação definitiva continuam pendentes.

Para reproduzir:

```bash
dotnet build BacklogManager.slnx --no-restore -m:1
dotnet test BacklogManager.slnx --no-restore -m:1 --logger 'trx;LogFileName=revisao2-final.trx' --results-directory /tmp/backlogmanager-revisao2-resultados
python3 -B scripts/gerar_relatorio_revisao2.py /tmp/backlogmanager-revisao2-resultados/revisao2-final.trx
```

Os commits desta rodada são informados na entrega e em `git log`. A única branch alterada foi `fix/biblioteca-ajustes`; `qodana.yaml` e a skill local permanecem não rastreados e preservados. **Sem push, merge ou Fase 4. A Fase 3 continua aguardando revisão e autorização explícita.**

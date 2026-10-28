# Terceira revisão visual da Fase 3

## Situação

Correções implementadas exclusivamente em **`fix/biblioteca-ajustes`**, a partir de `1420d39`. A branch original `feat/biblioteca` permanece em `bbcb99f`; `main`, em `64fb4fb`. Nenhuma outra branch avançou. Não houve push, merge ou início da Fase 4.

O proprietário relatou nova validação manual no Rider, com aparente funcionamento das correções anteriores, e solicitou os três ajustes desta rodada. O relato não discrimina cenários e não constitui aprovação definitiva da Fase 3. Os resultados manuais individuais não foram inventados. Este relatório complementa [REVISAO2_FASE3.md](REVISAO2_FASE3.md), mantendo os documentos anteriores como histórico.

## Correções e causas

### Capas nos adicionados recentemente

O template da página inicial tinha um `Path` fixo com o ícone de jogo. Ele não consultava `Catalogo.CapaUrl`, mesmo quando a obra já possuía uma capa na biblioteca.

O cabeçalho dos recentes passou a usar `CapaObra`, vinculado à URL da mesma obra. Esse controle já oferece carregamento assíncrono, cache compartilhado na sessão e tratamento de falhas. Não foi criado outro cliente, serviço de catálogo ou armazenamento de imagens.

- URL disponível e imagem válida: a capa aparece no início.
- Capa ausente, URL inválida, falha de conexão, 404 ou conteúdo inválido: aparece “Sem capa”.
- Catálogos simulados: usam suas referências locais já existentes, sem HTTP.
- Navegar entre início e biblioteca compartilha a mesma imagem, evitando outro download da mesma URL.
- Os recentes continuam limitados às três últimas obras na ordem anterior; clicar continua abrindo a edição do registro correto.

O cabeçalho mantém altura de 72 pixels e usa preenchimento proporcional. Não foram implementados novos componentes do dashboard de uma fase futura.

### Dimensões e alinhamento da grade

A altura automática acompanhava a quantidade de conteúdo visível. Registros com progresso ou identificação de simulação ocupavam mais espaço que registros incompletos. A largura da capa também não estava explícita no template. Isso permitia diferenças perceptíveis entre cartões de registros legados e obras com mais metadados.

O conteúdo foi organizado em linhas de alturas comuns. As linhas de progresso e identificação de simulação continuam reservadas quando seus textos estão ausentes, mantendo todos os cartões e as informações alinhados. Nenhum valor ausente foi preenchido ou inventado no modelo.

| Medida lógica do layout | Janela normal | Modo compacto, altura menor que 740 |
|---|---:|---:|
| Largura do cartão | 180 | 180 |
| Altura medida do cartão | 332 | 262 |
| Largura da capa | 160 | 160 |
| Altura da capa | 174 | 104 |
| Raio dos cantos | 8 | 8 |

São unidades lógicas do Avalonia. A quantidade de colunas continua se adaptando à largura disponível pelo `WrapPanel`. O cartão não usa um tamanho derivado da resolução ou proporção da imagem. Capas largas, estreitas e ausentes ocupam a mesma área.

Título, gêneros, status, nota, progresso e review têm linhas comuns. Textos extensos são abreviados visualmente quando necessário e continuam disponíveis no tooltip/formulário. O status passou a ter também abreviação e tooltip, evitando que valores legados extensos invadam cartões vizinhos.

A altura resulta dessas linhas padronizadas, sem recuperar a reserva anterior de 352 pixels. Há um pequeno espaço reservado para metadados opcionais: ele é necessário para a igualdade de medidas pedida nesta rodada. Seleção, foco e término do download não deslocam as informações.

### Cantos discretos e estados visuais

A remoção anterior do contorno havia definido raio zero na grade. Foi recuperado um raio discreto de oito unidades para os cartões da biblioteca e dos recentes, mantendo **espessura de borda zero na grade**.

O fundo da capa também usa raio oito. Como o raio de um `Border` não garante o recorte da imagem filha, `CapaObra` passou a aplicar uma geometria de recorte arredondada que acompanha seu tamanho. Isso vale para bitmaps, desenhos locais e o fallback.

A seleção continua indicada pela marca interna e texto “✓ Selecionado”; o foco por teclado mantém o texto “Foco” e o provider padrão de acessibilidade. Os estilos transparentes dos estados do `ContentPresenter`, que corrigiram as faixas externas do tema, foram preservados. O hover e a seleção não recuperam essas faixas ou o traço externo indesejado.

O preenchimento proporcional pode recortar bordas da imagem, mas não distorce sua proporção e não modifica a URL ou o arquivo original.

## Organização e C#

As mudanças de produção ficaram em `MainWindow.axaml` e `Controls/CapaObra.cs`. Não houve alteração de modelos, regras de cadastro, campos numéricos, persistência, IDs ou integração IGDB.

O XAML reaproveita um controle de capa existente e organiza as informações em um `Grid`. Cada obra continua sendo o mesmo `ItemBiblioteca`; a padronização ocorre somente na apresentação. No controle, `SizeChanged` atualiza `Clip` com um `RectangleGeometry` arredondado, para que o recorte acompanhe redimensionamentos. Nenhum pacote novo foi acrescentado.

## Preservação

O `Contrato.md` versão 1.1 foi relido e preservado. Não havia mudanças rastreadas pendentes ao iniciar. `qodana.yaml` e `skills/system/run-workflow/SKILL.md` continuam não rastreados, inalterados e fora dos commits.

A biblioteca pessoal permanece com **9 registros e 1535 bytes**, no mesmo caminho de armazenamento, com o SHA-256:

`d33b826a57148dc8c7017ae22e15f6898163ba7d5d5eea57a7ba64b6ddaac810`

Todos os backups e relatórios anteriores tiveram seus hashes comparados com o início desta rodada. Nenhum foi alterado ou sobrescrito. As mudanças são de interface; não houve migração, alteração de formato ou nova gravação da biblioteca pessoal. Os testes usaram exclusivamente JSONs, dados e imagens simulados em pastas temporárias, sem usar a biblioteca pessoal ou uma cópia de seus registros para exercitar o aplicativo.

A [evidência de preservação](verificacoes-revisao3-fase3.json) contém hashes dos arquivos protegidos, backups e referências das demais branches, além das execuções reais.

## Arquivos

Criados:

- `Tests/BacklogManager.Tests/Revisao3/ApoioVisual.cs`
- `Tests/BacklogManager.Tests/Revisao3/RecentesCapasTests.cs`
- `Tests/BacklogManager.Tests/Revisao3/DimensoesGradeTests.cs`
- `Tests/BacklogManager.Tests/Revisao3/ArredondamentoTests.cs`
- `scripts/gerar_relatorio_revisao3.py`
- `docs/REVISAO3_FASE3.md`
- `docs/REVISAO3_FASE3_TESTES.md`
- `docs/resultados-revisao3-fase3.json`
- `docs/verificacoes-revisao3-fase3.json`

Modificados:

- `MainWindow.axaml`, `Controls/CapaObra.cs`
- `Tests/BacklogManager.Tests/Revisao2/GradeRefinadaTests.cs`
- `Tests/BacklogManager.Tests/Revisao2/MiniaturasTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/CapasGradeTests.cs`
- `Tests/BacklogManager.Tests/AjustesFase3/TextosTests.cs`
- `Tests/BacklogManager.Tests/BuscaJanelaTests.cs`
- `Tests/BacklogManager.Tests/Fase2/InterfaceMultimidiaTests.cs`
- `Tests/BacklogManager.Tests/Fase3/InteracaoBibliotecaTests.cs`
- `Tests/BacklogManager.Tests/Fase3/VisualizacoesTests.cs`

São **9 criados e 10 modificados** em relação a `1420d39`. Dois cenários anteriores acompanharam decisões expressas desta rodada: CG01 verifica cantos discretos sem contorno; CG09 verifica altura igual com ou sem progresso. Os demais ajustes nos testes anteriores mudaram somente o destino de capturas para a pasta desta revisão. Relatórios históricos não foram regenerados.

## Testes e resultados

Compilação final: **sucesso, zero erros e zero avisos**. Suíte final: **949 aprovados, zero reprovados, zero não executados**, incluindo todos os **909 cenários anteriores** e **40 novos**.

| Funcionalidade | IDs novos | Cenários | Aprovados |
|---|---|---:|---:|
| Capas, cache e fallback nos recentes | CR01–CR14 | 14 | 14 |
| Dimensões e alinhamento da grade | DG01–DG14 | 14 | 14 |
| Cantos, recorte, estados e acessibilidade | RD01–RD12 | 12 | 12 |
| Regressão anterior | Todos os cenários anteriores | 909 | 909 |
| **Total** | | **949** | **949** |

Cada funcionalidade modificada tem ao menos dez cenários distintos. Os testes cobrem proporções diferentes de imagens, dados incompletos/legados, textos extensos, download pendente, falhas externas, ausência de capa, cache, alternância de visualização, redimensionamento, seis categorias, comparação de pixels, seleção e foco.

A [tabela individual](REVISAO3_FASE3_TESTES.md) apresenta cenário, esperado, observado, tipo e status dos 949 testes. [resultados-revisao3-fase3.json](resultados-revisao3-fase3.json) contém resultados e durações extraídos do TRX real.

| ID | Verificação adicional | Esperado | Observado | Tipo | Status |
|---|---|---|---|---|---|
| XS01 | Inicialização desktop com exemplos fictícios, com e sem capas locais | Abrir sem erro, sem credenciais IGDB e sem regravar o JSON | Processo aberto por oito segundos e encerrado pelo verificador; log vazio; JSON simulado intacto | Automatizado de inicialização | Aprovado |

**Planejados: 956** — 949 na suíte, uma inicialização e seis validações manuais. **Executados: 950; aprovados: 950; reprovados atuais: zero; não executados: seis manuais.** Inicialização e capturas não equivalem à validação interativa no Rider.

### Rodadas e ajustes

| Execução real | Resultado | Interpretação |
|---|---|---|
| Base preservada | 909/909 | Referência antes das alterações. |
| Primeira regressão | 907/909 | CG01 esperava raio zero; CG09 esperava cartões com progresso mais altos. Adaptados às decisões atuais, sem remover cenários. |
| Novos cenários | 39/40 | Uma expectativa de teste não incluía as duas unidades de espaçamento na altura de linha retornada pelo Avalonia. Corrigida a expectativa de 14 para 16, sem alterar o aplicativo. |
| Suíte final | 949/949 | Regressão e cenários novos executados novamente. |

Foram inspecionadas as capturas de recentes com capas/fallback e da grade com registros mistos em 900×660. Também foram geradas capturas da lista arredondada. As imagens e os TRX desta rodada ficam em `/tmp/backlogmanager-revisao3-resultados/`.

## Verificações manuais para aprovação

Use a branch `fix/biblioteca-ajustes` e uma pasta de trabalho temporária, sem testes sobre a biblioteca pessoal. Há um exemplo inteiramente fictício em `/tmp/backlogmanager-revisao3-resultados/desktop-simulado/backlog.json`: um jogo legado sem capa e dois livros simulados, um com capa/progresso e outro sem. Copie esse exemplo para outra pasta temporária e configure **Run → Edit Configurations → Working directory** no Rider para a pasta escolhida.

Para conferir uma capa real da IGDB, utilize um cadastro obtido pelo catálogo em uma biblioteca de testes isolada, com suas credenciais locais já configuradas. Essa chamada real não foi executada por esta rodada automatizada.

| ID | Conferência manual | Resultado esperado | Observado | Tipo | Status |
|---|---|---|---|---|---|
| MV01 | Início: obra com capa disponível e obra sem capa | Capa aparece nos recentes; somente ausência/falha usa fallback | Aguardando proprietário | Manual / Rider | Não executado |
| MV02 | Navegar entre início e biblioteca e abrir um recente | Mesma capa, título e edição corretos, sem perda da seleção funcional | Aguardando proprietário | Manual / Rider | Não executado |
| MV03 | Grade com registros novos, legados e progresso opcional | Largura, altura e linhas de informações iguais dentro do mesmo modo | Aguardando proprietário | Manual / Rider | Não executado |
| MV04 | Redimensionar entre 900×660 e janela maior | Cartões mudam juntos para o modo compacto/normal e a grade quebra linhas adequadamente | Aguardando proprietário | Manual / Rider | Não executado |
| MV05 | Hover, seleção e foco por Tab/setas | Cantos discretos, sem faixa externa ou contorno indevido; indicação interna visível | Aguardando proprietário | Manual / Rider | Não executado |
| MV06 | Lista, capas largas/estreitas e fallback | Recorte proporcional, cantos consistentes, dados legíveis e operações preservadas | Aguardando proprietário | Manual / Rider | Não executado |

Nenhuma aprovação manual individual desta rodada foi presumida.

## Limitações e estado Git

- O preenchimento proporcional pode recortar bordas; não distorce as imagens.
- Algumas linhas ficam reservadas mesmo quando faltam metadados. Isso mantém os cartões iguais e suas informações alinhadas, sem preencher dados fictícios.
- O cache de capas continua apenas na sessão. Ao abrir uma sessão nova offline, uma capa remota ainda pode cair no fallback.
- Não houve nova chamada autenticada real à IGDB; HTTP de teste foi simulado.
- A grade continua sem virtualização e sem benchmark de milhares de obras.
- Fontes personalizadas e outros sistemas operacionais não foram validados nesta rodada.
- As limitações de compatibilidade de executáveis anteriores, registradas nos relatórios anteriores, permanecem; não houve novo formato nesta revisão.

Para reproduzir:

```bash
dotnet build BacklogManager.slnx --no-restore -m:1
dotnet test BacklogManager.slnx --no-restore -m:1 --logger 'trx;LogFileName=revisao3-final.trx' --results-directory /tmp/backlogmanager-revisao3-resultados
python3 -B scripts/gerar_relatorio_revisao3.py /tmp/backlogmanager-revisao3-resultados/revisao3-final.trx
```

Os commits são informados na entrega e em `git log`. A única branch alterada foi `fix/biblioteca-ajustes`; `qodana.yaml` e a skill local continuam não rastreados e preservados. **Sem push, merge ou Fase 4. Aguardando revisão e autorização explícita antes de integrar à `main`.**

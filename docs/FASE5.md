# Fase 5 — Identidade visual e personalização

Implementação de **Minhas Realizações**, conforme FASE5_INSTRUCOES.md e Contrato.md versão 1.1, na branch **feat/identidade-visual**. Pronta para revisão do proprietário; sem push, release, merge da Fase 5 ou avanço à Fase 6.

## Base e preservação

A Fase 4 aprovada foi integrada à main exclusivamente por **fast-forward**, de **8c9d147 para 807638a**, sem conflitos e sem commit de merge. feat/identidade-visual foi criada dessa base. feat/colecoes continua em 807638a; todas as branches anteriores foram preservadas. A aprovação geral está em [APROVACAO_FASE4.md](APROVACAO_FASE4.md), sem aprovações individuais de cenários não discriminados.

FASE5_INSTRUCOES.md já estava adicionado ao índice pelo proprietário. Seu conteúdo e estado preparado foram preservados, sem incorporá-lo automaticamente aos commits desta fase. qodana.yaml e os arquivos de skills/ continuam não rastreados e fora dos commits.

Antes das alterações foi criado e verificado o backup externo:

${HOME}/Backups/BacklogManager/fase5-20261028-180111

Ele contém cópia exata do backlog.json e o manifesto verificacao.json. A biblioteca encontrada tem **9 registros, 1535 bytes**, SHA-256 **d33b826a57148dc8c7017ae22e15f6898163ba7d5d5eea57a7ba64b6ddaac810**. O original não foi usado em testes. A conferência abrange biblioteca, contrato, instruções, arquivos pessoais, recursos, relatórios e backups anteriores; veja [verificacoes-fase5.json](verificacoes-fase5.json).

## Identidade e temas

O nome público passou a ser **Minhas Realizações** no título da janela, identificação textual, nome da aplicação Avalonia e metadados de título/produto. Projeto, assembly, namespaces, classes e caminhos técnicos BacklogManager foram mantidos.

Não foi criado nem escolhido ícone, logotipo ou mascote. Os recursos existentes foram preservados. O nome não recebeu slogan/subtítulo. A frase decorativa “Cada obra, uma história” foi substituída pelo título funcional “Progresso da biblioteca”; o rodapé indica armazenamento local sem frase de marca.

| Tema | Claro | Escuro |
|---|---|---|
| Moderno | Fundos claros azulados e destaque roxo | Azul escuro, superfícies azuladas e roxo |
| Tecnológico | Cinzas claros e verde-água escuro | Preto/cinza frio e verde-água claro |
| Acolhedor | Creme, vinho e notas douradas | Superfícies em vinho e destaque dourado |

Cada conjunto define fundos, textos, bordas e estados. Para preferências ausentes, o padrão é **Moderno claro**, conforme a instrução específica mais recente. Trocar tema/modo recupera suas personalizações, preservando as demais.

## Aparência e cores independentes

A página **Aparência** oferece tema/modo e 13 cores independentes: fundo principal; fundo/texto da barra lateral; fundo/texto dos painéis; fundo/texto dos cartões; fundo/texto dos botões; destaque e texto sobre destaque; texto principal e secundário.

Cada cor tem campo hexadecimal, amostra atual e 12 amostras prontas, incluindo neutros e outras famílias de cores. São aceitos todos os códigos válidos **#RRGGBB**, inclusive preto/branco. Espaços externos são removidos e letras normalizadas para maiúsculas. Abreviações, transparência, nomes, entradas incompletas e caracteres inválidos são recusados com orientação textual. Durante a digitação inválida, a última cor válida permanece aplicada e o texto intermediário não é gravado.

Alterações válidas aplicam a prévia e salvam automaticamente, sem reiniciar. Os editores são criados ao abrir Aparência. Exibir/preencher padrões não gera gravações ou personalizações espúrias. Conteúdo dos formulários, registros e vínculos permanecem intactos.

Duas restaurações têm escopos distintos:

1. **Restaurar cores deste tema e modo:** remove somente as personalizações da combinação ativa.
2. **Restaurar toda a aparência:** exige confirmação, volta a Moderno claro e remove as personalizações conhecidas dos seis conjuntos. Cancelar preserva o estado.

Ambas persistem sem apagar biblioteca, coleções ou preferência de visualização. Falhas de escrita mantêm a prévia, avisam que ela não foi salva e oferecem retry. Mudança externa exige reabrir; versão futura fica protegida contra sobrescrita.

## Contraste e estados

O resolvedor calcula luminância sRGB e razão de contraste. Textos efetivos atendem a **4,5:1**, inclusive textos pequenos de seleção; indicadores essenciais derivados atendem a **3:1** sobre seus fundos pertinentes. O critério de texto normal também cobre textos grandes, para os quais a especificação permite 3:1.

A **cor solicitada** permanece guardada. Quando um texto não contrasta, sua **cor efetiva** passa a preto/branco e a tela explica o ajuste junto ao controle. Se o fundo voltar a permitir a cor original, ela reaparece. Texto secundário é resolvido para cada superfície, com aviso quando ajustado.

Textos e indicadores de seleção têm tokens distintos por exigirem mínimos diferentes. Hover, pressionado e foco de botão são derivados, considerando foco junto do hover. Campos usam borda de foco e adorno de dois contornos. A grade conserva barra/texto de seleção e foco sem reintroduzir o contorno externo rejeitado na Fase 3. Erro, aviso e sucesso mantêm mensagens/ícones, sem depender só de cor.

Os recursos dinâmicos alcançam início, grade/lista, coleções, cadastro/edição, campos numéricos, seletores, menus, tooltips, mensagens e confirmações. Isso não representa certificação completa de acessibilidade; leitor de tela e plataformas/monitores indisponíveis continuam sem validação manual.

## Arquitetura e conceitos C#

| Arquivo em Aparencia/ | Responsabilidade |
|---|---|
| PreferenciasAparencia.cs | Tema, modo, elementos, escolhas por combinação e contrato de armazenamento |
| CoresAparencia.cs | Paleta, seis predefinições, hexadecimal, contraste e estados derivados |
| ResolvedorAparencia.cs | Escolhas solicitadas → recursos efetivos e avisos |
| ArmazenamentoAparenciaJson.cs | Leitura compatível e escrita protegida |
| ServicoAparencia.cs | Seleção, personalização, restaurações e falha/retry |
| AplicadorAparencia.cs | Recursos dinâmicos e adaptação ao Fluent |

Controls/EditorCor.cs apresenta uma cor editável. MainWindow.Aparencia.cs coordena a tela, sem alterar regras da biblioteca. A composição injeta o armazenamento real; testes injetam armazenamento simulado.

Os enum representam opções conhecidas; record/with produzem um novo estado; cópias de leitura impedem alterações indiretas. ResourceDictionary é um mapa entre nomes visuais e brushes. DynamicResource observa esse mapa e atualiza os controles em tempo de execução.

A base permanece **Avalonia 12.1.4**, sem novos pacotes. Os nomes do adaptador foram conferidos nos templates oficiais dessa versão: [Button](https://github.com/AvaloniaUI/Avalonia/blob/12.1.4/src/Avalonia.Themes.Fluent/Controls/Button.xaml) e [TextBox](https://github.com/AvaloniaUI/Avalonia/blob/12.1.4/src/Avalonia.Themes.Fluent/Controls/TextBox.xaml).

### Preferências

Arquivo: **aparencia.preferencias.json, na mesma pasta da biblioteca**. Os caminhos existentes continuam relativos ao diretório de trabalho, resolvidos na composição. Se a biblioteca tiver exatamente esse nome, a aparência recebe sufixo .visual.json para não sobrescrevê-la.

biblioteca.preferencias.json não foi ampliado porque seu gravador anterior mantém apenas versão/grade-lista. O arquivo independente evita interferir na preferência aprovada. Exemplo fictício:

~~~json
{
  "Versao": 1,
  "Tema": "Moderno",
  "Modo": "Claro",
  "Personalizacoes": {
    "Moderno/Claro": {
      "FundoPrincipal": "#163A63",
      "TextoPrincipal": "#FFFFFF"
    }
  }
}
~~~

São persistidas as cores solicitadas; as efetivas são recalculadas deterministicamente. Campos ausentes/inválidos usam padrões seguros, preservando escolhas válidas. Extensões desconhecidas e conjuntos futuros são conservados ao editar. Versão numérica futura bloqueia gravação sem impedir abertura. JSON corrompido ou com chaves duplicadas usa padrões com aviso; alteração explícita guarda o original em .bak.

A escrita usa .lock, comparação SHA-256, temporário exclusivo, flush e substituição atômica com .bak. Pasta ausente/inacessível não é criada silenciosamente. Arquivo ausente não é criado só ao abrir. O arquivo de aparência e .lock entraram no .gitignore; temporários/.bak já eram ignorados.

### Como estender

Novo tema: acrescentar TemaVisual, definir as duas paletas, disponibilizar no seletor e ampliar testes. Nova cor: acrescentar ElementoCor, rótulo, padrão, par de contraste e recurso do resolvedor; os editores são montados a partir desses elementos. Componentes consomem DynamicResource da superfície apropriada. Estados derivam cores no resolvedor. A adaptação ao Fluent permanece no AplicadorAparencia, sem hexadecimais espalhados pelas telas.

## Matriz de testes

As tabelas individuais estão em [FASE5_TESTES.md](FASE5_TESTES.md) e [resultados-fase5.json](resultados-fase5.json), geradas do TRX real.

| Grupo | Funcionalidade | Cenários novos |
|---|---|---:|
| VT | Temas/modos, padrão e alternância | 12 |
| VI | Cores independentes, isolamento e snapshots | 19 |
| VH | Hexadecimal, normalização e entradas inválidas | 18 |
| VC | Contraste, texto solicitado e indicadores | 20 |
| VA | Persistência, recarga, compatibilidade, conflitos e falhas | 27 |
| VR | Restauração ativa/global e isolamento | 12 |
| VP | Prévia Avalonia, paletas, identidade, navegação e confirmações | 23 |
| VE | Foco, hover, pressionado, seleção, menus e mensagens | 17 |
| VF | Personalizações representativas e componentes/preferências anteriores | 13 |
| **Novos** | **Mínimo de dez por grupo, ampliado nos casos de risco** | **161** |
| Regressão | Todos os cenários anteriores das Fases 0 a 4 e ajustes | 1105 |
| **Suíte final** | **Planejados/executados/aprovados; zero falhas e zero não executados** | **1266** |

Somando suíte, smoke e roteiro manual: **1.272 cenários planejados, 1.267 executados/aprovados, zero reprovados e cinco não executados**, estes últimos aguardando o proprietário. Rodadas repetidas não aumentam essa contagem.

TX02 foi mantido e atualizado para o título neutro autorizado. Nenhum teste anterior foi removido. Arquivos são temporários/fictícios e HTTP é simulado. Não houve nova pesquisa autenticada à IGDB.

Build final: **0 erros e 0 avisos**. git diff --check: sem problemas. As contagens e os hashes de cada rodada intermediária, inclusive reprovações corrigidas, constam em [verificacoes-fase5.json](verificacoes-fase5.json); repetições não são somadas como cenários novos.

### Bugs e correções

- Eventos de texto adiados atrasavam prévia/salvamento em seis cenários. O editor passou a observar a propriedade diretamente e distinguir preenchimento programático de edição. Os cenários foram repetidos.
- Um teste de paleta procurava controles antes do layout. Passou a renderizar a página antes de localizar os botões, mantendo as asserções da aplicação.
- O adorno de foco usava ControlTemplate onde FocusAdorner exige Template. O build passava, mas a janela falhava em execução; testes reais de interface detectaram e validaram a correção.
- A largura de Aparência foi limitada ao viewport para não cortar textos. Os contornos permanentes da navegação foram removidos, mantendo foco.
- Texto pequeno de seleção recebeu token de 4,5:1 separado da barra de 3:1. O indicador de foco considera também hover/pressionado.
- TX02 ainda esperava a frase decorativa anterior; sua asserção passou a verificar o título funcional, preservando a finalidade multimídia.
- JsonObject materializa dicionários sob demanda: chaves duplicadas em uma combinação podiam interromper o carregamento. A validação recursiva antecipada garante fallback seguro, preservação e .bak em alteração explícita, cobertos por VA26/VA27.
- Uma rodada inicial de regressão ficou sem conclusão e foi cancelada para diagnóstico. Não foi registrada como aprovada nem recebeu contagem inventada. A repetição com rastreamento/timeout e a suíte final foram executadas integralmente.

### Evidências visuais e nativas

Foram geradas e inspecionadas capturas **Avalonia Headless/Skia** das seis combinações em início, biblioteca e aparência. Também foram inspecionadas personalizações azul, cinza, preta, rosada, quente e de baixo contraste, além de Aparência em 900×660 com topo/fim da rolagem.

Artefatos efêmeros: /tmp/backlogmanager-fase5-resultados/imagens, incluindo seis-temas.png e personalizacoes.png. Os hashes ficam nas verificações. Testes antigos mantêm seus destinos temporários; relatórios históricos versionados foram preservados.

| ID | Cenário | Esperado | Observado | Tipo | Status |
|---|---|---|---|---|---|
| XN01 | Desktop com biblioteca, coleção e aparência fictícias, sem credenciais | Processo ativo por oito segundos, log sem erros e JSONs intactos | Processo ativo, log vazio e três hashes iguais; encerrado pelo script ao final | Smoke automatizado sem interação | Aprovado |

O smoke não substitui interação no Rider. As capturas demonstram apenas o ambiente Headless disponível; não se afirma execução em Windows/macOS, outros monitores ou leitor de tela.

Para repetir:

~~~bash
dotnet build
dotnet test --logger 'trx;LogFileName=fase5-validacao-final.trx' --results-directory /tmp/backlogmanager-fase5-resultados
python3 -B scripts/gerar_relatorio_fase5.py /tmp/backlogmanager-fase5-resultados/fase5-validacao-final.trx
~~~

## Limitações e riscos

- Validação manual do proprietário no Rider pendente; somente os cinco cenários consolidados abaixo foram propostos, sem resultados inventados.
- Não houve chamada autenticada nova à IGDB, teste manual em outras plataformas, leitor de tela ou várias escalas físicas de monitor.
- Capas remotas conservam carregador/cache em memória anteriores; não há cache persistente novo. Aparência não altera a arte das capas nem as ilustrações simuladas existentes.
- Biblioteca e coleções continuam em **dois arquivos sem transação conjunta**. A limitação da Fase 4 permanece para backup/sincronização futuros. É necessário restaurar par coerente; .bak independentes não garantem essa coerência. Aparência usa terceiro arquivo separado e não participa dessas gravações.
- Versão futura bloqueia escrita; conflito externo exige reabertura. A prévia não salva será perdida ao fechar, conforme o aviso.
- Arquivos brutos em /tmp são efêmeros; contagens, resultados individuais e hashes ficaram versionados.
- Nenhum dashboard novo, recurso de nuvem ou ferramenta de backup de fases posteriores foi iniciado.

## Cinco cenários manuais no Rider

Configure uma **pasta temporária** em Run/Debug Configuration → Working directory, usando dados fictícios ou cópia isolada. Se houver coleções, use também a biblioteca correspondente. Não use a biblioteca original nem a pasta de backups pessoais.

Todos estão **não executados — aguardando proprietário**, separados dos 1.266 testes automatizados.

| ID | Cenário e passos curtos | Esperado | Observado | Tipo | Status |
|---|---|---|---|---|---|
| M01 | **Identidade e temas:** conferir o nome, percorrer três temas × dois modos e navegar | Nome correto sem slogan/novo ícone; seis combinações legíveis | Aguardando proprietário | Manual | Não executado |
| M02 | **Cores independentes:** mudar fundos, sidebar, painéis, cartões, botões, destaque e textos por amostras/hex | Prévia imediata e preservação dos outros grupos | Aguardando proprietário | Manual | Não executado |
| M03 | **Validação/contraste:** digitar válido/inválido, escolher texto/fundo iguais e usar Tab/hover/botões | Inválido não aplicado/salvo; mensagem e ajuste seguro; foco/estados visíveis | Aguardando proprietário | Manual | Não executado |
| M04 | **Persistência/restauração:** personalizar duas combinações, alternar, reiniciar e testar restaurações, cancelando uma vez | Escolhas recuperadas; restauração ativa isolada; global confirmada volta a Moderno claro | Aguardando proprietário | Manual | Não executado |
| M05 | **Regressão de uso:** grade/lista, filtros, cadastro/edição pessoal, coleções e associações | Dados/vínculos preservados e fluxos/confirmações normais e legíveis | Aguardando proprietário | Manual | Não executado |

## Arquivos e Git

**24 arquivos criados e dez modificados pelo agente**, separados do arquivo de instruções já preparado pelo proprietário.

| Estado | Arquivo |
|---|---|
| Modificado | .gitignore |
| Criado | Aparencia/AplicadorAparencia.cs |
| Criado | Aparencia/ArmazenamentoAparenciaJson.cs |
| Criado | Aparencia/CoresAparencia.cs |
| Criado | Aparencia/PreferenciasAparencia.cs |
| Criado | Aparencia/ResolvedorAparencia.cs |
| Criado | Aparencia/ServicoAparencia.cs |
| Modificado | App.axaml |
| Modificado | App.axaml.cs |
| Modificado | BacklogManager.csproj |
| Modificado | Configuracao/ComposicaoAplicativo.cs |
| Modificado | Controls/CapaObra.cs |
| Criado | Controls/EditorCor.cs |
| Criado | MainWindow.Aparencia.cs |
| Modificado | MainWindow.axaml |
| Modificado | MainWindow.axaml.cs |
| Modificado | Tests/BacklogManager.Tests/AjustesFase3/TextosTests.cs |
| Criado | Tests/BacklogManager.Tests/Fase5/ApoioAparencia.cs |
| Criado | Tests/BacklogManager.Tests/Fase5/CompatibilidadeVisualTests.cs |
| Criado | Tests/BacklogManager.Tests/Fase5/ContrasteTests.cs |
| Criado | Tests/BacklogManager.Tests/Fase5/EstadosVisuaisTests.cs |
| Criado | Tests/BacklogManager.Tests/Fase5/HexadecimalTests.cs |
| Criado | Tests/BacklogManager.Tests/Fase5/PersistenciaAparenciaTests.cs |
| Criado | Tests/BacklogManager.Tests/Fase5/PersonalizacaoTests.cs |
| Criado | Tests/BacklogManager.Tests/Fase5/PreviaInterfaceTests.cs |
| Criado | Tests/BacklogManager.Tests/Fase5/RestauracaoAparenciaTests.cs |
| Criado | Tests/BacklogManager.Tests/Fase5/TemasModosTests.cs |
| Modificado | Tests/BacklogManager.Tests/TelaTeste.cs |
| Criado | docs/APROVACAO_FASE4.md |
| Criado | docs/FASE5.md |
| Criado | docs/FASE5_TESTES.md |
| Criado | docs/resultados-fase5.json |
| Criado | docs/verificacoes-fase5.json |
| Criado | scripts/gerar_relatorio_fase5.py |

Contrato, APIs, regras de negócio, arquivos pessoais e relatórios anteriores não foram modificados. Os commits locais são informados na entrega. FASE5_INSTRUCOES.md permanece preparado como encontrado; qodana.yaml e skills/ permanecem não rastreados.

A main permanece em **807638a**, com a Fase 4 aprovada. A Fase 5 permanece em **feat/identidade-visual**, aguardando revisão e autorização explícita para integração. Não houve push, release ou início da Fase 6.

Commits locais de implementação e testes: **f3ab358** e **f7efbb8**. A documentação/resultados ficam em commit próprio, cujo identificador é informado na entrega.

# Fase 5 — Identidade visual e personalização

## 1. Objetivo e autoridade destas instruções

Implementar a Fase 5 do aplicativo Avalonia atualmente identificado tecnicamente como BacklogManager. O nome público passa a ser **Minhas Realizações**, sem slogan.

Leia este arquivo integralmente e leia integralmente o `Contrato.md` do repositório antes de implementar. Respeite o contrato, especialmente a seção 19.1, e as instruções locais aplicáveis. Este documento contém a especificação completa da Fase 5 e substitui o prompt anterior que ficou truncado. Não presuma requisitos contidos em trechos ausentes de conversas anteriores.

A autorização de Git descrita abaixo foi dada expressamente pelo usuário. Caso uma condição antiga exigisse aguardar a aprovação da Fase 4 ou autorização para o fast-forward, essa aprovação já foi concedida. Não solicite novamente a mesma autorização se o estado real permitir executar exatamente o procedimento autorizado.

## 2. Base aprovada e autorização de Git

Estado relatado pelo Codex, a verificar antes de agir:

| Item | Estado relatado |
| --- | --- |
| Branch de trabalho | `feat/colecoes` |
| Commit da Fase 4 | `807638a` |
| `main` | `8c9d147` |
| Integração | Três commits, com fast-forward possível |
| Branch da Fase 5 | `feat/identidade-visual` ainda não existe |
| Alterações rastreadas | Nenhuma pendente |
| Arquivos pessoais não rastreados | `qodana.yaml` e `skills/` |

O usuário aprovou a Fase 4 e **autoriza integrar `feat/colecoes` à `main` exclusivamente por fast-forward e criar `feat/identidade-visual` a partir dessa `main` atualizada**. A implementação da Fase 5 deve ocorrer nessa nova branch.

Procedimento:

1. Inspecionar o estado real, as instruções locais, o contrato e a ancestralidade dos commits; preservar o estado e os backups exigidos pelo contrato.
2. Confirmar que o conteúdo aprovado da Fase 4 está presente e que a integração é realmente fast-forward.
3. Executar a integração por fast-forward, sem commit de merge, sem rebase e sem reescrever histórico.
4. Criar e usar `feat/identidade-visual` a partir da `main` atualizada.
5. Implementar, validar e entregar a Fase 5 nessa branch, conforme o contrato.

Se a Fase 4 já estiver integrada corretamente, não repetir a operação. Se a branch de identidade visual já existir, inspecionar sua origem e seu conteúdo antes de reutilizá-la; nunca sobrescrever trabalho. Se houver divergência que torne o procedimento autorizado inseguro ou impossível, explicar a diferença concreta e solicitar apenas a decisão necessária. Não descartar alterações para obter uma árvore limpa.

**Não fazer push. Não integrar a Fase 5 à `main`. Não publicar releases.** Commits locais e backups devem seguir o contrato; não usar squash, reset destrutivo ou operações de limpeza para eliminar arquivos existentes.

## 3. Preservação obrigatória e limites de escopo

- Preservar dados reais, coleções, itens, categorias, estados, avaliações, relacionamentos e todas as funcionalidades já aprovadas.
- Preservar os arquivos pessoais não rastreados `qodana.yaml` e `skills/`, sem editar, excluir, mover ou adicionar aos commits da Fase 5. Outros arquivos pessoais encontrados recebem o mesmo cuidado.
- Preservar `Contrato.md`, histórico e backups existentes; cumprir a política de novos backups prevista no contrato.
- Não recriar banco, apagar dados, trocar caminhos de armazenamento nem introduzir migração destrutiva para aplicar aparência.
- Não alterar regras de negócio para acomodar o novo visual. Mudanças estruturais devem ser limitadas ao necessário para centralizar e aplicar os temas.
- Não decidir, criar, substituir ou selecionar logotipo, ícone ou mascote nesta fase. Se já houver um ícone técnico necessário ao funcionamento, preservá-lo sem transformá-lo em uma decisão de identidade visual.
- Não incluir slogan, subtítulo promocional ou frase de marca.

## 4. Nome público

Usar **Minhas Realizações**, com essa grafia e acentuação, nos pontos públicos em que o nome do aplicativo apareça: título da janela, identificação textual da interface, telas de apresentação/sobre se existentes e metadados de exibição aplicáveis à distribuição atual.

Manter nomes técnicos como BacklogManager em namespaces, projetos, assemblies, identificadores e caminhos persistidos quando a renomeação não for necessária ao nome público ou puder comprometer compatibilidade. Não fazer uma renomeação técnica ampla nesta fase. Atualizar a documentação pertinente para distinguir nome público e identificação técnica.

## 5. Três temas completos e dois modos

Entregar os três temas disponíveis ao usuário, cada um com **modo claro e modo escuro**, totalizando seis combinações completas:

| Tema | Direção visual |
| --- | --- |
| Moderno | Roxo e azul, composição limpa e contemporânea |
| Tecnológico | Verde-água e cinza, organização precisa e sóbria |
| Acolhedor | Vinho e dourado, aparência confortável e elegante |

Os temas são predefinições prontas e utilizáveis, não apenas uma troca de cor de botão. Cada combinação deve definir superfícies, textos, bordas, destaques e estados interativos coerentes. Evitar fundos saturados que prejudiquem leitura prolongada. As cores exatas podem ser definidas durante a implementação, respeitando a direção visual e o contraste.

O usuário deve escolher explicitamente entre claro e escuro. Integração com o modo do sistema é opcional e não substitui os dois modos exigidos. Para instalações sem preferência anterior, usar Moderno claro como padrão; preservar uma preferência anterior válida quando possível.

## 6. Personalização independente de cores

Disponibilizar uma área de Aparência, integrada às configurações ou ao fluxo já existente, com controles compreensíveis em português. O usuário deve poder alterar independentemente, no mínimo:

| Grupo | Cores configuráveis |
| --- | --- |
| Fundo principal | Fundo da área principal do aplicativo |
| Sidebar | Fundo e texto da navegação lateral |
| Painéis | Fundo e texto das áreas agrupadas e painéis |
| Cartões | Fundo e texto dos cartões de conteúdo |
| Botões | Fundo e texto dos botões principais |
| Destaque | Cor de destaque/seleção e texto sobre esse destaque |
| Textos gerais | Texto principal e texto secundário |

Quando componentes usarem outras cores expostas ao usuário, todas essas cores também devem aceitar paleta e entrada hexadecimal. Bordas e estados podem ser derivados centralmente; se forem expostos como controles de cor, obedecem à mesma regra.

Alterar um grupo não deve substituir as escolhas explícitas de outro grupo. É permitido recalcular estados e cores derivadas para manter a legibilidade, sem apagar uma personalização válida. Permitir fundos totalmente azuis, acinzentados, rosados ou de qualquer outra cor válida, sem limitar a personalização às famílias dos temas.

Manter as preferências separadas por tema e por modo. Trocar temporariamente de tema ou modo e voltar deve recuperar as personalizações daquela combinação. Evitar que uma escolha adequada ao modo claro seja aplicada inadvertidamente ao modo escuro.

## 7. Paletas e hexadecimal

Para **cada cor configurável**, oferecer amostras de paletas prontas e um campo hexadecimal editável. Exibir a cor atual e seu código, com rótulo que identifique o elemento afetado. As paletas devem incluir opções neutras e variações de cor além das três identidades iniciais.

Regras da entrada hexadecimal:

- Aceitar o formato completo `#RRGGBB`, com seis dígitos hexadecimais; aceitar letras maiúsculas ou minúsculas e normalizar a apresentação para maiúsculas.
- Remover espaços externos antes de validar. Não aceitar abreviação `#RGB`, transparência `#AARRGGBB`, nomes de cor, caracteres fora de `0–9`/`A–F` ou valores incompletos como cores aplicáveis.
- Validar a entrada com regra equivalente a `^#[0-9A-Fa-f]{6}$` após remover espaços externos.
- Durante a digitação, manter a última cor válida até que o novo valor esteja completo e válido. Não gerar exceção, piscar a interface ou persistir texto inválido.
- Mostrar orientação curta junto ao campo, por exemplo: “Use #RRGGBB, como #3366CC”. Não depender apenas de uma borda colorida para indicar erro.
- Ao selecionar uma amostra, atualizar o campo hexadecimal; ao inserir um hexadecimal válido, atualizar a amostra e a prévia.
- Preto `#000000` e branco `#FFFFFF` são valores válidos; não confundir uma validação de formato com uma validação de contraste.

## 8. Contraste, leitura e estados visuais

Validar o contraste entre textos e seus fundos efetivos, incluindo sidebar, painéis, cartões, botões e seleção. Usar como critérios mínimos 4,5:1 para texto normal e 3:1 para texto grande. Indicadores essenciais de foco e limites visuais necessários à identificação de controles devem ter contraste de pelo menos 3:1 com as cores adjacentes pertinentes.

Todas as combinações prontas devem atender aos critérios sem intervenção do usuário. Nas personalizações, aceitar qualquer hexadecimal válido como escolha de cor, mas garantir uma combinação final legível: quando o par escolhido for insuficiente, informar o problema junto aos controles e aplicar uma cor de texto segura na prévia e no resultado. Conservar a preferência solicitada separadamente da cor efetiva, para poder reaplicá-la se o fundo voltar a permitir contraste suficiente. Explicar visualmente quando um ajuste automático de texto estiver ativo. Não salvar uma combinação efetiva ilegível nem substituir silenciosamente outras cores personalizadas.

Testar contraste e estados visuais onde houver: normal, hover, pressionado, foco de teclado, selecionado, desabilitado, validação/erro e estados vazios. Cobrir navegação, formulários, listas, cartões, menus, diálogos e mensagens existentes. Estados desabilitados devem continuar identificáveis e compreensíveis; observar suas exceções de contraste sem usá-las para justificar texto ativo ilegível.

Preservar a identificação semântica de sucesso, aviso e erro. Não depender apenas de cor para comunicar informação. Garantir foco visível e uso por teclado, textos sem cortes e comportamento utilizável nos tamanhos de janela e escalas já suportados.

## 9. Prévia imediata e experiência de edição

- Aplicar alterações válidas imediatamente à interface em uso, incluindo a própria tela de Aparência, sem reiniciar o aplicativo.
- Atualizar janelas e diálogos abertos quando a arquitetura existente permitir; toda nova janela deve usar o tema vigente.
- Manter o conteúdo e o contexto de navegação durante alterações de aparência.
- Não persistir estados intermediários de digitação. As escolhas válidas devem ser salvas automaticamente; agrupar gravações se necessário para evitar escrita excessiva.
- Se uma gravação falhar, manter a interface funcional e comunicar que a preferência não pôde ser salva, sem afirmar sucesso.
- Organizar a edição para que o usuário entenda tema, modo, elemento alterado, cor escolhida e eventual ajuste automático de contraste.

## 10. Persistência e restauração

Persistir o tema selecionado, o modo e as personalizações por combinação de tema/modo. Reabrir o aplicativo deve recuperar as mesmas escolhas e produzir as mesmas cores efetivas. Usar o mecanismo de preferências existente quando adequado, sem misturar alterações visuais aos dados de negócio.

O formato de preferências deve permitir evolução compatível. Tratar configurações ausentes, antigas, incompletas, desconhecidas ou inválidas com valores padrão seguros, sem impedir a abertura do aplicativo e sem danificar dados. Validar valores carregados, além dos digitados. Preservar preferências válidas ao preencher campos ausentes.

Fornecer duas ações com escopo claro:

1. **Restaurar cores deste tema e modo**: remover apenas as personalizações da combinação ativa e reaplicar sua paleta original.
2. **Restaurar toda a aparência**: restaurar Moderno claro e remover personalizações dos seis conjuntos, sem afetar dados ou outras configurações.

Confirmar a restauração global, informando seu alcance. Persistir corretamente ambas as restaurações. Não apagar arquivos inteiros de configuração se eles contiverem outras preferências.

## 11. Arquitetura Avalonia centralizada

Inspecionar primeiro a versão e a organização Avalonia efetivamente usadas no projeto. Implementar com os recursos compatíveis com essa base, sem atualização ampla de dependências ou troca de framework visual sem necessidade demonstrada.

- Centralizar definições dos três temas, variantes claro/escuro, tokens semânticos, resolução de cores, validação e persistência.
- Usar recursos e brushes compartilhados, `ResourceDictionary`, variantes de tema e referências dinâmicas conforme suportados pela versão em uso.
- Aplicar recursos dinâmicos aos componentes para que a troca ocorra em tempo de execução. Evitar cores fixas espalhadas por XAML, code-behind e estilos locais que impeçam a personalização.
- Separar modelo de preferências, serviço/resolvedor de tema, persistência e apresentação, respeitando o padrão arquitetural existente.
- Centralizar o cálculo de contraste e estados derivados. Não duplicar fórmulas em cada tela.
- Manter detalhes técnicos fora da experiência comum do usuário; rótulos de interface devem descrever elementos visuais.
- Manter compatibilidade com os componentes, plataformas, escalas e recursos já suportados pelo projeto. Não introduzir serviço externo para escolher ou validar cores.

Documentar brevemente como acrescentar um tema ou token e como os controles consomem os recursos centrais. Não expandir o escopo para uma reescrita geral da interface.

## 12. Verificação automatizada obrigatória

**O Codex deve implementar e executar no mínimo 10 testes relevantes por funcionalidade nova ou modificada**, além das regressões exigidas pelo `Contrato.md`, especialmente §19.1. O mínimo não é de 10 testes para a Fase 5 inteira. Se o contrato exigir cobertura adicional, cumpri-la.

Identificar explicitamente as funcionalidades afetadas e apresentar uma matriz que relacione cada uma aos seus testes e resultados. Variações parametrizadas contam apenas quando verificarem cenários distintos e úteis; não inflar a contagem com repetições ou testes que apenas reproduzam a implementação. Uma alteração meramente textual deve ter sua verificação adequada documentada, sem dispensar o mínimo exigido para as funcionalidades que ela afeta.

Cobrir, no mínimo, as seguintes funcionalidades com seus respectivos conjuntos de 10 ou mais casos relevantes:

- Seleção e aplicação dos temas e modos, incluindo as seis combinações, inicialização e alternância repetida.
- Personalização independente dos grupos de cores, isolamento por tema/modo e preservação das demais escolhas.
- Paletas, entrada hexadecimal, normalização, validação e sincronização dos controles.
- Contraste, resolução de textos seguros e estados interativos derivados.
- Prévia imediata e atualização dos recursos nos componentes afetados.
- Persistência, recarga, compatibilidade das preferências e tratamento de falhas.
- Restauração da combinação ativa e restauração global, incluindo persistência e isolamento de dados.

Adicionar conjuntos para outras funcionalidades efetivamente modificadas. Usar testes unitários, de integração e de interface conforme o comportamento exigir e os recursos do projeto permitirem. Não considerar uma verificação de modelo como prova suficiente da aplicação visual em Avalonia.

Executar build e a suíte de regressão prescrita no contrato. Verificar as funcionalidades anteriores, em particular as da Fase 4: operações de coleções e seus vínculos, além dos fluxos de dados, navegação e persistência afetados pelo novo visual. Empregar dados isolados de teste; nunca executar testes destrutivos sobre dados reais.

Inspecionar visualmente as seis combinações prontas e personalizações representativas. Registrar as evidências possíveis no ambiente. Não afirmar que houve inspeção visual, execução de interface ou teste em plataforma onde isso não ocorreu. Se algum teste não puder ser executado, indicar o bloqueio, o impacto e o que permanece sem validação; não substituir silenciosamente por um teste mais fraco.

## 13. Validação manual para usuário/ChatGPT: no máximo cinco cenários

O trabalho de teste detalhado é responsabilidade do Codex. Entregar ao usuário/ChatGPT **apenas estes cinco cenários manuais consolidados**, com passos curtos e resultado esperado; não repassar a suíte automatizada como uma lista extensa de tarefas manuais.

1. **Identidade e temas:** conferir “Minhas Realizações”, ausência de slogan e as seis combinações de tema/modo, incluindo leitura e navegação.
2. **Cores independentes:** alterar fundo principal, sidebar, painéis, cartões, botões, destaque e textos por paletas e hexadecimal; conferir prévia imediata e independência.
3. **Validação e contraste:** inserir códigos válidos e inválidos, escolher um par de baixo contraste e conferir mensagem, ajuste legível, foco e estados dos controles.
4. **Persistência e restauração:** personalizar combinações distintas, alternar entre elas, reiniciar e testar as duas restaurações, verificando seus escopos.
5. **Regressão de uso:** percorrer os fluxos principais já existentes, incluindo coleções da Fase 4, confirmando dados preservados, diálogos legíveis e funcionamento normal.

## 14. Critérios de aceite e entrega do Codex

A Fase 5 estará pronta para revisão quando todos os requisitos deste documento estiverem implementados, o contrato tiver sido atendido, o build e os testes obrigatórios tiverem sido executados com sucesso e as limitações de ambiente estiverem explicitamente registradas. Pendências de validação devem permanecer identificadas como pendências.

Ao concluir, entregar um relatório objetivo contendo:

- Branch e commit/base usados; confirmação da integração da Fase 4 por fast-forward ou de que ela já estava corretamente integrada.
- O que mudou na identidade pública, nos seis conjuntos visuais e na personalização.
- Organização central dos temas e localização das preferências, sem expor dados sensíveis.
- Matriz de testes por funcionalidade com pelo menos 10 casos relevantes em cada conjunto, comandos/checks executados e resultados reais; regressões e evidências visuais.
- Confirmação de preservação dos dados, contrato, backups, histórico, `qodana.yaml` e `skills/`.
- Até cinco cenários manuais, conforme a seção anterior.
- Arquivos alterados, eventuais limitações e estado final de Git.
- Confirmação explícita de que não houve push nem integração da Fase 5 à `main`.

Concluir o trabalho autorizado sem interromper apenas para repetir aprovações já dadas. Encerrar com a Fase 5 disponível na branch `feat/identidade-visual`, pronta para revisão do usuário.

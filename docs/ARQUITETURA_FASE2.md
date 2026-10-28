# Evolução da arquitetura na Fase 2

Este documento complementa o registro histórico da [Fase 1](ARQUITETURA.md). Os modelos, contratos, serviço de biblioteca e proteção de gravação existentes foram estendidos para os seis tipos do contrato.

## Pesquisa e identificação de dados simulados

`RoteadorPesquisaCatalogo` escolhe o serviço pelo tipo da obra. Para jogos, preserva a instância de pesquisa IGDB configurada; sem credenciais, jogos continuam com pesquisa indisponível. Para anime, série, filme, mangá e livro, entrega `CatalogoSimulado`, que implementa `IPesquisaCatalogo` sem usar HTTP.

Cada catálogo tem três obras fictícias com títulos contendo “(simulado)”, IDs textuais próprios, gêneros e referência de capa SVG local. `ObraCatalogo.Simulado` é preservado no arquivo e usado em indicações visuais na pesquisa e biblioteca. Os simuladores não são fallback de jogos e não geram preços, avaliações externas ou dados apresentados como catálogo real.

A interface permite escolher o tipo no cadastro e na biblioteca. A troca de categoria cancela a busca, limpa seleção/progresso e impede que sugestões antigas sejam usadas em outra mídia. A edição mantém o tipo e a referência do item existente. A espera de 400 ms e o cancelamento da IGDB continuam preservados.

## Progresso pessoal

`DadosPessoaisBiblioteca.Progresso` é opcional. `ProgressoPessoal` mantém somente valores informados pelo usuário:

| Tipo | Campos |
|---|---|
| Jogo | Horas jogadas, decimal |
| Anime | Episódios assistidos, inteiro |
| Série | Temporada e episódio, inteiros opcionais independentes |
| Filme | Minutos assistidos, decimal opcional |
| Mangá | Capítulo e volume, inteiros opcionais independentes |
| Livro | Página e capítulo, inteiros opcionais independentes |

Ausência é diferente de zero. Valores negativos e unidades de outra mídia são recusados. Campos inteiros não aceitam frações nem truncamento silencioso. A validação ocorre no formulário, no serviço da biblioteca e na leitura/gravação do formato. Nenhum progresso é inferido de duração estimada, resultado de pesquisa ou status.

O estado interno existente (`Na Fila`, `Jogando`, `Zerado`, `Pausado`, `Abandonado`) é mantido para compatibilidade. `TextosMidia` adapta somente a apresentação: livros e mangás mostram “Quero ler/Lendo/Lido”; mídias assistidas mostram “Quero assistir/Assistindo/Assistido”. Valores legados desconhecidos são preservados.

## Extensão do JSON

A raiz continua uma lista e o caminho continua `backlog.json` no diretório de trabalho. Não há gravação na abertura, migração automática ou renomeação de propriedades legadas.

Os campos `Titulo`, `Categoria`, `Status`, `Nota`, `Review`, `SteamAppId` e `IgdbId` permanecem. Um bloco **opcional** `Multimidia`, com `Versao = 1`, comporta tipo, referência externa, capa, gêneros, flag de simulação e progresso. Jogos legados sem esses dados adicionais continuam sendo gravados sem o bloco. Um arquivo pode conter registros legados e registros multimídia juntos.

Exemplo sintético de um novo registro, com os demais campos pessoais resumidos:

```json
{
  "Titulo": "A Biblioteca das Marés (simulado)",
  "Categoria": "Fantasia",
  "Status": "Na Fila",
  "Nota": 4,
  "Review": "Exemplo",
  "SteamAppId": null,
  "Multimidia": {
    "Versao": 1,
    "Tipo": "Livro",
    "Origem": "Simulado",
    "IdentificadorExterno": "simulado:Livro:1",
    "CapaUrl": "avares://BacklogManager/Assets/CapasSimuladas/Livro.svg",
    "Generos": ["Fantasia"],
    "Simulado": true,
    "Progresso": { "Pagina": 12 }
  }
}
```

O serializador pode escrever também propriedades opcionais nulas; o exemplo omite essas propriedades para facilitar a leitura.

`ConversorBibliotecaJson` faz a conversão. `FileManager` continua validando toda a lista antes de gravar, conferindo hash/bloqueio, escrevendo temporário, mantendo `.bak` e substituindo o principal. O serviço confirma a memória apenas depois do sucesso.

Proteções do formato:

- Versão desconhecida, tipo ausente/inválido e campos desconhecidos dentro dos blocos novos bloqueiam a carga/gravação, preservando o original. Não são descartados silenciosamente.
- Extras desconhecidos da raiz do registro legado continuam sendo preservados por `JsonExtensionData`.
- `SteamAppId` continua opaco: não é convertido nem interpretado como IGDB ou como ID simulado.
- Jogos com referência confirmada mantêm `IgdbId`, que deve concordar com o ID no bloco quando presente. IDs de outras mídias ficam somente na referência externa multimídia.
- Dados simulados exigem origem e flag coerentes. Tipos futuros não são interpretados como jogos.
- Qualquer registro inválido recusa o lote inteiro antes de escrever; falhas de disco não aplicam mudanças na coleção da janela.

## Compatibilidade e recuperação

Antes da implementação foi criado um backup externo verificável. A biblioteca real permanece intacta: os testes usam arquivos temporários, e a verificação com dados pessoais opera sobre cópia, conferindo hash antes/depois.

O aplicativo desta fase lê o formato antigo. Executáveis das fases anteriores não compreendem mídia e progresso novos; não devem ser usados para trabalhar em uma coleção já ampliada. Para voltar a uma versão antiga, feche o aplicativo, preserve o arquivo multimídia e restaure conscientemente uma cópia do backup anterior. Não há downgrade nem restauração automática. O `.bak` guarda apenas a versão anterior, não um histórico completo.

## Limites da fase

As referências de capas e recursos SVG locais estão disponíveis nos modelos e na persistência. A grade de capas, a alternância entre grade/lista e a apresentação de capas nas sugestões/biblioteca continuam na Fase 3. Não foram adicionadas integrações reais para novas mídias, nuvem, preferências persistidas, exportação/importação de backup ou recursos das fases seguintes.

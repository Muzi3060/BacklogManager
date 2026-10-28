# Fase 1 — Resultados individuais

Gerado do TRX real por `scripts/gerar_relatorio_fase1.py`. Inclui os cenários de regressão da Fase 0 executados novamente sobre a Fase 1; os relatórios históricos da Fase 0 não foram modificados.

**Planejados na suíte: 323; executados: 323; aprovados: 323; reprovados: 0; não executados: 0.**

Uma aprovação confirma as asserções executadas, não uma chamada autenticada à IGDB ou validação manual no Rider. Todos os dados da suíte são simulados e os arquivos são temporários. Verificações adicionais e pendências manuais constam em `FASE1.md`.

## Persistência e preservação — 34 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| P01 | Arquivo inexistente | Lista vazia sem criar arquivo | Confirmado pelas asserções: Lista vazia sem criar arquivo | Automatizado | Aprovado |
| P02 | Lista vazia válida | Carregamento aceito | Confirmado pelas asserções: Carregamento aceito | Automatizado | Aprovado |
| P03 | JSON legado válido | Todos os campos preservados | Confirmado pelas asserções: Todos os campos preservados | Automatizado | Aprovado |
| P04 | Arquivo vazio | Erro de leitura; gravação bloqueada; bytes originais preservados | Confirmado pelas asserções: Erro de leitura; gravação bloqueada; bytes originais preservados | Automatizado | Aprovado |
| P05 | JSON com sintaxe incompleta | Erro de leitura; gravação bloqueada; bytes originais preservados | Confirmado pelas asserções: Erro de leitura; gravação bloqueada; bytes originais preservados | Automatizado | Aprovado |
| P06 | Raiz JSON null | Erro de leitura; gravação bloqueada; original preservado | Confirmado pelas asserções: Erro de leitura; gravação bloqueada; original preservado | Automatizado | Aprovado |
| P07 | Raiz JSON objeto em vez de lista | Erro de leitura; gravação bloqueada; original preservado | Confirmado pelas asserções: Erro de leitura; gravação bloqueada; original preservado | Automatizado | Aprovado |
| P08 | Registro null na lista | Lista inteira recusada sem descarte parcial | Confirmado pelas asserções: Lista inteira recusada sem descarte parcial | Automatizado | Aprovado |
| P09 | Título nulo no registro | Lista inteira recusada sem descarte parcial | Confirmado pelas asserções: Lista inteira recusada sem descarte parcial | Automatizado | Aprovado |
| P10 | Categoria nula no registro | Lista inteira recusada sem descarte parcial | Confirmado pelas asserções: Lista inteira recusada sem descarte parcial | Automatizado | Aprovado |
| P11 | Status nulo no registro | Lista inteira recusada sem descarte parcial | Confirmado pelas asserções: Lista inteira recusada sem descarte parcial | Automatizado | Aprovado |
| P12 | Nota fora de 0 a 5 | Lista inteira recusada sem descarte parcial | Confirmado pelas asserções: Lista inteira recusada sem descarte parcial | Automatizado | Aprovado |
| P13 | Review nula no registro | Lista inteira recusada sem descarte parcial | Confirmado pelas asserções: Lista inteira recusada sem descarte parcial | Automatizado | Aprovado |
| P14 | Primeira gravação | Arquivo válido e recarregável | Confirmado pelas asserções: Arquivo válido e recarregável | Automatizado | Aprovado |
| P15 | Atualização de arquivo | Backup byte a byte da versão anterior | Confirmado pelas asserções: Backup byte a byte da versão anterior | Automatizado | Aprovado |
| P16 | Gravações sucessivas | Backup acompanha última versão válida | Confirmado pelas asserções: Backup acompanha última versão válida | Automatizado | Aprovado |
| P17 | Arquivo alterado externamente | Gravação recusada e versão externa intacta | Confirmado pelas asserções: Gravação recusada e versão externa intacta | Automatizado | Aprovado |
| P18 | Arquivo removido externamente | Nenhuma recriação silenciosa | Confirmado pelas asserções: Nenhuma recriação silenciosa | Automatizado | Aprovado |
| P19 | Arquivo criado após leitura vazia | Arquivo externo preservado | Confirmado pelas asserções: Arquivo externo preservado | Automatizado | Aprovado |
| P20 | Gravação sem leitura anterior | Operação recusada sem criar arquivo | Confirmado pelas asserções: Operação recusada sem criar arquivo | Automatizado | Aprovado |
| P21 | Registro inválido ao salvar | Arquivo e backup não alterados | Confirmado pelas asserções: Arquivo e backup não alterados | Automatizado | Aprovado |
| P22 | Bloqueio por outra instância | Falha segura sem modificar coleção | Confirmado pelas asserções: Falha segura sem modificar coleção | Automatizado | Aprovado |
| P23 | Falha na substituição do backup | Original intacto e temporário removido | Confirmado pelas asserções: Original intacto e temporário removido | Automatizado | Aprovado |
| P24 | Recuperação após falha de gravação | Nova tentativa salva corretamente | Confirmado pelas asserções: Nova tentativa salva corretamente | Automatizado | Aprovado |
| P25 | Campos desconhecidos | Dados adicionais preservados no roundtrip | Confirmado pelas asserções: Dados adicionais preservados no roundtrip | Automatizado | Aprovado |
| P26 | Onze registros simulados e IDs repetidos | Quantidade e valores integralmente preservados | Confirmado pelas asserções: Quantidade e valores integralmente preservados | Automatizado | Aprovado |
| P27 | Unicode e review com quebras | Texto preservado após gravação | Confirmado pelas asserções: Texto preservado após gravação | Automatizado | Aprovado |
| P28 | Caminho padrão | Diretório de trabalho original mantido | Confirmado pelas asserções: Diretório de trabalho original mantido | Automatizado | Aprovado |
| P29 | Diretório de destino ausente | Falha sem criar pastas implicitamente | Confirmado pelas asserções: Falha sem criar pastas implicitamente | Automatizado | Aprovado |
| P30 | Caminho ocupado por diretório | Erro de leitura e gravação bloqueada | Confirmado pelas asserções: Erro de leitura e gravação bloqueada | Automatizado | Aprovado |
| P31 | Duas janelas com versões diferentes | Segunda janela não sobrescreve a primeira | Confirmado pelas asserções: Segunda janela não sobrescreve a primeira | Automatizado | Aprovado |
| P32 | Leitura falha após leitura válida | Autorização anterior para gravar é revogada | Confirmado pelas asserções: Autorização anterior para gravar é revogada | Automatizado | Aprovado |
| P33 | JSON UTF-8 com BOM | Carregamento compatível sem alterar bytes originais | Confirmado pelas asserções: Carregamento compatível sem alterar bytes originais | Automatizado | Aprovado |
| P34 | JSON UTF-16 com BOM | Codificação reconhecida como na leitura anterior | Confirmado pelas asserções: Codificação reconhecida como na leitura anterior | Automatizado | Aprovado |

## Identificadores e compatibilidade — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| I01 | ID legado positivo | Valor mantido; IgdbId não inferido nem serializado | Confirmado pelas asserções: Valor mantido; IgdbId não inferido nem serializado | Automatizado | Aprovado |
| I02 | ID legado null | Valor null mantido; IgdbId não inferido | Confirmado pelas asserções: Valor null mantido; IgdbId não inferido | Automatizado | Aprovado |
| I03 | ID legado zero | Valor legado mantido sem reinterpretação | Confirmado pelas asserções: Valor legado mantido sem reinterpretação | Automatizado | Aprovado |
| I04 | ID legado negativo | Valor legado mantido sem reinterpretação | Confirmado pelas asserções: Valor legado mantido sem reinterpretação | Automatizado | Aprovado |
| I05 | ID legado int.MaxValue | Valor legado mantido sem heurística de origem | Confirmado pelas asserções: Valor legado mantido sem heurística de origem | Automatizado | Aprovado |
| I06 | Novo identificador IGDB | Roundtrip mantém ID separado do legado | Confirmado pelas asserções: Roundtrip mantém ID separado do legado | Automatizado | Aprovado |
| I07 | Dois identificadores no JSON | Nenhum sobrescreve o outro | Confirmado pelas asserções: Nenhum sobrescreve o outro | Automatizado | Aprovado |
| I08 | IDs legados repetidos | Registros continuam distintos | Confirmado pelas asserções: Registros continuam distintos | Automatizado | Aprovado |
| I09 | Identificador IGDB zero | Gravação rejeitada sem descartar original | Confirmado pelas asserções: Gravação rejeitada sem descartar original | Automatizado | Aprovado |
| I10 | Identificador IGDB negativo | Gravação rejeitada sem descartar original | Confirmado pelas asserções: Gravação rejeitada sem descartar original | Automatizado | Aprovado |
| I11 | Legado sem propriedade SteamAppId | Carregado com ID nulo sem conversão | Confirmado pelas asserções: Carregado com ID nulo sem conversão | Automatizado | Aprovado |
| I12 | Identificador IGDB máximo | Inteiro positivo preservado sem heurística | Confirmado pelas asserções: Inteiro positivo preservado sem heurística | Automatizado | Aprovado |

## Configuração e credenciais — 14 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| C01 | Somente token com nome legado | Configuração válida; token legado utilizado | Confirmado pelas asserções: Configuração válida; token legado utilizado | Automatizado | Aprovado |
| C02 | Somente token com nome corrigido | Configuração válida; token corrigido utilizado | Confirmado pelas asserções: Configuração válida; token corrigido utilizado | Automatizado | Aprovado |
| C03 | Ambos os nomes de token configurados | Configuração válida; nome corrigido tem precedência | Confirmado pelas asserções: Configuração válida; nome corrigido tem precedência | Automatizado | Aprovado |
| C04 | Nome corrigido vazio e legado válido | Configuração válida usando o legado | Confirmado pelas asserções: Configuração válida usando o legado | Automatizado | Aprovado |
| C05 | Nome corrigido com espaços e legado válido | Configuração válida usando o legado | Confirmado pelas asserções: Configuração válida usando o legado | Automatizado | Aprovado |
| C06 | Credenciais com espaços nas bordas | Bordas aparadas; configuração válida | Confirmado pelas asserções: Bordas aparadas; configuração válida | Automatizado | Aprovado |
| C07 | Client ID ausente | Configuração indisponível sem exceção | Confirmado pelas asserções: Configuração indisponível sem exceção | Automatizado | Aprovado |
| C08 | Client ID vazio | Configuração indisponível sem exceção | Confirmado pelas asserções: Configuração indisponível sem exceção | Automatizado | Aprovado |
| C09 | Ambos os tokens ausentes | Configuração indisponível sem exceção | Confirmado pelas asserções: Configuração indisponível sem exceção | Automatizado | Aprovado |
| C10 | Token legado somente com espaços | Configuração indisponível sem exceção | Confirmado pelas asserções: Configuração indisponível sem exceção | Automatizado | Aprovado |
| C11 | Client ID com quebra de linha interna | Configuração inválida; nenhum header injetado | Confirmado pelas asserções: Configuração inválida; nenhum header injetado | Automatizado | Aprovado |
| C12 | Token com espaço interno | Configuração inválida; nenhum header enviado | Confirmado pelas asserções: Configuração inválida; nenhum header enviado | Automatizado | Aprovado |
| C13 | Token corrigido inválido e legado válido | Recusa explícita; sem troca silenciosa de credencial inválida | Confirmado pelas asserções: Recusa explícita; sem troca silenciosa de credencial inválida | Automatizado | Aprovado |
| C14 | Credenciais inválidas no serviço | Exceção segura sem expor valores | Confirmado pelas asserções: Exceção segura sem expor valores | Automatizado | Aprovado |

## HTTP e ciclo de vida dos recursos — 28 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| H01 | Pesquisa bem-sucedida | POST no endpoint correto com headers e IDs | Confirmado pelas asserções: POST no endpoint correto com headers e IDs | Automatizado | Aprovado |
| H02 | Busca sem resultados | Lista vazia sem falha | Confirmado pelas asserções: Lista vazia sem falha | Automatizado | Aprovado |
| H03 | HTTP 401 | Mensagem de autenticação; uma chamada; corpo sensível não exposto | Confirmado pelas asserções: Mensagem de autenticação; uma chamada; corpo sensível não exposto | Automatizado | Aprovado |
| H04 | HTTP 403 | Mensagem de autenticação; uma chamada; corpo sensível não exposto | Confirmado pelas asserções: Mensagem de autenticação; uma chamada; corpo sensível não exposto | Automatizado | Aprovado |
| H05 | HTTP 429 | Mensagem de limite; uma chamada; sem repetição automática | Confirmado pelas asserções: Mensagem de limite; uma chamada; sem repetição automática | Automatizado | Aprovado |
| H06 | HTTP 500 | Mensagem de indisponibilidade; uma chamada | Confirmado pelas asserções: Mensagem de indisponibilidade; uma chamada | Automatizado | Aprovado |
| H07 | HTTP 503 | Mensagem de indisponibilidade; uma chamada | Confirmado pelas asserções: Mensagem de indisponibilidade; uma chamada | Automatizado | Aprovado |
| H08 | HTTP 400 | Mensagem com status HTTP 400 sem corpo da resposta | Confirmado pelas asserções: Mensagem com status HTTP 400 sem corpo da resposta | Automatizado | Aprovado |
| H09 | HTTP 404 | Mensagem com status HTTP 404 sem corpo da resposta | Confirmado pelas asserções: Mensagem com status HTTP 404 sem corpo da resposta | Automatizado | Aprovado |
| H10 | Falha de conexão | Mensagem de conexão sem detalhes sensíveis | Confirmado pelas asserções: Mensagem de conexão sem detalhes sensíveis | Automatizado | Aprovado |
| H11 | Timeout de transporte | Mensagem distinta de cancelamento do usuário | Confirmado pelas asserções: Mensagem distinta de cancelamento do usuário | Automatizado | Aprovado |
| H12 | Resposta JSON incompleta | Falha controlada sem retornar lista parcial | Confirmado pelas asserções: Falha controlada sem retornar lista parcial | Automatizado | Aprovado |
| H13 | Resposta JSON null | Falha controlada sem retornar lista vazia enganosa | Confirmado pelas asserções: Falha controlada sem retornar lista vazia enganosa | Automatizado | Aprovado |
| H14 | Resposta objeto em vez de lista | Falha controlada sem retornar lista parcial | Confirmado pelas asserções: Falha controlada sem retornar lista parcial | Automatizado | Aprovado |
| H15 | Elemento null na resposta | Falha controlada sem retornar lista parcial | Confirmado pelas asserções: Falha controlada sem retornar lista parcial | Automatizado | Aprovado |
| H16 | Resultado com ID zero | Falha controlada; resultado inválido não selecionável | Confirmado pelas asserções: Falha controlada; resultado inválido não selecionável | Automatizado | Aprovado |
| H17 | Resultado com ID negativo | Falha controlada; resultado inválido não selecionável | Confirmado pelas asserções: Falha controlada; resultado inválido não selecionável | Automatizado | Aprovado |
| H18 | Resultado com nome null | Falha controlada sem retornar lista parcial | Confirmado pelas asserções: Falha controlada sem retornar lista parcial | Automatizado | Aprovado |
| H19 | Resultado com nome em branco | Falha controlada sem retornar lista parcial | Confirmado pelas asserções: Falha controlada sem retornar lista parcial | Automatizado | Aprovado |
| H20 | ID textual não numérico | Falha controlada de desserialização | Confirmado pelas asserções: Falha controlada de desserialização | Automatizado | Aprovado |
| H21 | Resposta vazia | Falha controlada sem confundir com zero resultados | Confirmado pelas asserções: Falha controlada sem confundir com zero resultados | Automatizado | Aprovado |
| H22 | Cancelamento antes do envio | Nenhuma chamada HTTP | Confirmado pelas asserções: Nenhuma chamada HTTP | Automatizado | Aprovado |
| H23 | Cancelamento durante HTTP | Token interrompe transporte | Confirmado pelas asserções: Token interrompe transporte | Automatizado | Aprovado |
| H24 | Recuperação após erro HTTP | Próxima busca retorna resultado | Confirmado pelas asserções: Próxima busca retorna resultado | Automatizado | Aprovado |
| H25 | Dois serviços compartilham cliente | Headers independentes sem duplicação | Confirmado pelas asserções: Headers independentes sem duplicação | Automatizado | Aprovado |
| H26 | JSON com maiúsculas | Desserialização mantém compatibilidade | Confirmado pelas asserções: Desserialização mantém compatibilidade | Automatizado | Aprovado |
| H27 | Requisições sucessivas | Cliente reutilizado permanece disponível | Confirmado pelas asserções: Cliente reutilizado permanece disponível | Automatizado | Aprovado |
| H28 | Resposta HTTP concluída | Conteúdo descartado após leitura | Confirmado pelas asserções: Conteúdo descartado após leitura | Automatizado | Aprovado |

## Validação da consulta APICalypse — 15 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| Q01 | Termo null | Lista vazia; nenhuma chamada HTTP | Confirmado pelas asserções: Lista vazia; nenhuma chamada HTTP | Automatizado | Aprovado |
| Q02 | Termo vazio | Lista vazia; nenhuma chamada HTTP | Confirmado pelas asserções: Lista vazia; nenhuma chamada HTTP | Automatizado | Aprovado |
| Q03 | Termo com somente espaços | Lista vazia; nenhuma chamada HTTP | Confirmado pelas asserções: Lista vazia; nenhuma chamada HTTP | Automatizado | Aprovado |
| Q04 | Espaços nas bordas do termo | Consulta enviada com termo aparado | Confirmado pelas asserções: Consulta enviada com termo aparado | Automatizado | Aprovado |
| Q05 | Aspas no termo | Aspas escapadas dentro da string APICalypse | Confirmado pelas asserções: Aspas escapadas dentro da string APICalypse | Automatizado | Aprovado |
| Q06 | Barras invertidas no termo | Barras escapadas dentro da string APICalypse | Confirmado pelas asserções: Barras escapadas dentro da string APICalypse | Automatizado | Aprovado |
| Q07 | Unicode e emoji no termo | Texto preservado na consulta | Confirmado pelas asserções: Texto preservado na consulta | Automatizado | Aprovado |
| Q08 | Ponto e vírgula dentro do termo | Conteúdo mantido dentro da string de pesquisa | Confirmado pelas asserções: Conteúdo mantido dentro da string de pesquisa | Automatizado | Aprovado |
| Q09 | Tentativa de fechar string e inserir instrução | Aspas escapadas; termo não vira instrução APICalypse | Confirmado pelas asserções: Aspas escapadas; termo não vira instrução APICalypse | Automatizado | Aprovado |
| Q10 | Apóstrofo no título | Texto preservado na consulta | Confirmado pelas asserções: Texto preservado na consulta | Automatizado | Aprovado |
| Q11 | Quebra de linha no termo | Entrada recusada antes do envio HTTP | Confirmado pelas asserções: Entrada recusada antes do envio HTTP | Automatizado | Aprovado |
| Q12 | Retorno de carro no termo | Entrada recusada antes do envio HTTP | Confirmado pelas asserções: Entrada recusada antes do envio HTTP | Automatizado | Aprovado |
| Q13 | Caractere null no termo | Entrada recusada antes do envio HTTP | Confirmado pelas asserções: Entrada recusada antes do envio HTTP | Automatizado | Aprovado |
| Q14 | Limite de 200 caracteres | Busca aceita | Confirmado pelas asserções: Busca aceita | Automatizado | Aprovado |
| Q15 | Mais de 200 caracteres | Busca recusada antes do HTTP | Confirmado pelas asserções: Busca recusada antes do HTTP | Automatizado | Aprovado |

## Cadastro e validação de formulário — 13 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| A01 | Adicionar seleção IGDB | Novo ID correto e SteamAppId nulo | Confirmado pelas asserções: Novo ID correto e SteamAppId nulo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A02 | Adicionar sem seleção | Nenhum arquivo criado | Confirmado pelas asserções: Nenhum arquivo criado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A03 | Seleção desfeita | Cadastro recusado | Confirmado pelas asserções: Cadastro recusado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A04 | ID zero na sugestão | Cadastro recusado | Confirmado pelas asserções: Cadastro recusado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A05 | Nome vazio na sugestão | Cadastro recusado | Confirmado pelas asserções: Cadastro recusado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A06 | Nota vazia no cadastro | Aviso e nenhuma gravação | Confirmado pelas asserções: Aviso e nenhuma gravação | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A07 | Categoria não selecionada | Cadastro recusado | Confirmado pelas asserções: Cadastro recusado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A08 | Status não selecionado | Cadastro recusado | Confirmado pelas asserções: Cadastro recusado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A09 | Conflito ao adicionar | Memória e formulário preservados sem falso sucesso | Confirmado pelas asserções: Memória e formulário preservados sem falso sucesso | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A10 | Falha de backup ao adicionar | Lista e arquivo originais preservados | Confirmado pelas asserções: Lista e arquivo originais preservados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A11 | Mesmo ID adicionado duas vezes | Dois registros sem deduplicação | Confirmado pelas asserções: Dois registros sem deduplicação | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A12 | Coleção corrompida no início | Gravação bloqueada e original intacto | Confirmado pelas asserções: Gravação bloqueada e original intacto | Automatizado (Avalonia Headless/Skia) | Aprovado |
| A13 | Review vazia e nota zero | Cadastro válido com texto padrão | Confirmado pelas asserções: Cadastro válido com texto padrão | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Edição e compatibilidade — 14 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| E01 | Editar registro legado | SteamAppId preservado e título somente leitura | Confirmado pelas asserções: SteamAppId preservado e título somente leitura | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E02 | Editar registro IGDB | IgdbId preservado | Confirmado pelas asserções: IgdbId preservado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E03 | Registro com dois IDs | Ambos preservados sem reinterpretar | Confirmado pelas asserções: Ambos preservados sem reinterpretar | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E04 | Registro sem IDs | IDs permanecem nulos | Confirmado pelas asserções: IDs permanecem nulos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E05 | Editar nota mínima | Zero persistido | Confirmado pelas asserções: Zero persistido | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E06 | Editar nota máxima | Cinco persistido | Confirmado pelas asserções: Cinco persistido | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E07 | Categoria e status legados | Valores fora das opções preservados | Confirmado pelas asserções: Valores fora das opções preservados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E08 | Campo JSON desconhecido na edição | Conteúdo adicional preservado | Confirmado pelas asserções: Conteúdo adicional preservado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E09 | Falha ao salvar edição | Nota anterior e formulário mantidos | Confirmado pelas asserções: Nota anterior e formulário mantidos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E10 | Atualizar sem item em edição | Coleção não alterada | Confirmado pelas asserções: Coleção não alterada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E11 | Nota vazia na edição | Arquivo anterior preservado | Confirmado pelas asserções: Arquivo anterior preservado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E12 | Review legada acima de 1000 caracteres | Edição de nota não trunca texto | Confirmado pelas asserções: Edição de nota não trunca texto | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E13 | Review legada com espaços | Edição sem alterar review preserva texto exato | Confirmado pelas asserções: Edição sem alterar review preserva texto exato | Automatizado (Avalonia Headless/Skia) | Aprovado |
| E14 | Review legada vazia | Edição mantém valor vazio original | Confirmado pelas asserções: Edição mantém valor vazio original | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Remoção e falhas de gravação — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| R01 | Remover item selecionado | Registro excluído da memória e do arquivo | Confirmado pelas asserções: Registro excluído da memória e do arquivo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| R02 | Remover sem seleção | Arquivo intacto | Confirmado pelas asserções: Arquivo intacto | Automatizado (Avalonia Headless/Skia) | Aprovado |
| R03 | Remover registro com ID repetido | Somente o registro selecionado é removido | Confirmado pelas asserções: Somente o registro selecionado é removido | Automatizado (Avalonia Headless/Skia) | Aprovado |
| R04 | Remoção atualiza estatísticas | Contadores refletem a coleção salva | Confirmado pelas asserções: Contadores refletem a coleção salva | Automatizado (Avalonia Headless/Skia) | Aprovado |
| R05 | Remover item em edição | Formulário volta ao cadastro | Confirmado pelas asserções: Formulário volta ao cadastro | Automatizado (Avalonia Headless/Skia) | Aprovado |
| R06 | Remover outro item durante edição | Edição do registro restante preservada | Confirmado pelas asserções: Edição do registro restante preservada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| R07 | Conflito externo na remoção | Memória e arquivo externo preservados | Confirmado pelas asserções: Memória e arquivo externo preservados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| R08 | Bloqueio na remoção | Nenhum registro desaparece da memória | Confirmado pelas asserções: Nenhum registro desaparece da memória | Automatizado (Avalonia Headless/Skia) | Aprovado |
| R09 | Falha no backup da remoção | Original preservado sem falso sucesso | Confirmado pelas asserções: Original preservado sem falso sucesso | Automatizado (Avalonia Headless/Skia) | Aprovado |
| R10 | Reiniciar após remoção | Apenas registros restantes são carregados | Confirmado pelas asserções: Apenas registros restantes são carregados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| R11 | Remover vizinho de registro IGDB | Identificadores restantes intactos | Confirmado pelas asserções: Identificadores restantes intactos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| R12 | Remover último registro | JSON vazio válido e backup recuperável | Confirmado pelas asserções: JSON vazio válido e backup recuperável | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Busca, cancelamento, janela e renderização — 23 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| B01 | Digitação rápida | Somente o termo final chega ao HTTP | Confirmado pelas asserções: Somente o termo final chega ao HTTP | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B02 | Limpar antes da espera | Nenhuma requisição enviada | Confirmado pelas asserções: Nenhuma requisição enviada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B03 | Trocar texto com HTTP ativo | Cancelamento chega ao transporte | Confirmado pelas asserções: Cancelamento chega ao transporte | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B04 | Resposta antiga chega depois | Resultado antigo não reaparece | Confirmado pelas asserções: Resultado antigo não reaparece | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B05 | Erro antigo após nova pesquisa | Aviso atual não é substituído | Confirmado pelas asserções: Aviso atual não é substituído | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B06 | Entrar em edição com busca ativa | Busca cancelada sem sugestões na edição | Confirmado pelas asserções: Busca cancelada sem sugestões na edição | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B07 | Navegar para início durante busca | Pesquisa cancelada | Confirmado pelas asserções: Pesquisa cancelada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B08 | Novo jogo durante pesquisa | Busca anterior cancelada e formulário limpo | Confirmado pelas asserções: Busca anterior cancelada e formulário limpo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B09 | Fechar com HTTP ativo | Requisição cancelada sem atualizar janela fechada | Confirmado pelas asserções: Requisição cancelada sem atualizar janela fechada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B10 | Timeout na janela | Aviso específico e coleção continua acessível | Confirmado pelas asserções: Aviso específico e coleção continua acessível | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B11 | Falha de rede na janela | Aviso sem encerramento | Confirmado pelas asserções: Aviso sem encerramento | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B12 | Resposta inválida na janela | Aviso sem lista de sugestões enganosa | Confirmado pelas asserções: Aviso sem lista de sugestões enganosa | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B13 | Buscar novamente após erro | Sugestões válidas apresentadas | Confirmado pelas asserções: Sugestões válidas apresentadas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B14 | Pesquisa válida sem resultados | Mensagem distinta de erro | Confirmado pelas asserções: Mensagem distinta de erro | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B15 | Sem credenciais | Coleção abre e edição local funciona | Confirmado pelas asserções: Coleção abre e edição local funciona | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B16 | Alterar texto após selecionar | Seleção antiga não pode cadastrar outro título | Confirmado pelas asserções: Seleção antiga não pode cadastrar outro título | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B17 | Títulos iguais com IDs diferentes | ID da sugestão escolhida é utilizado | Confirmado pelas asserções: ID da sugestão escolhida é utilizado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B18 | Título de um caractere | Pesquisa existente continua aceita | Confirmado pelas asserções: Pesquisa existente continua aceita | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B19 | Abrir edição | Nenhuma pesquisa HTTP automática | Confirmado pelas asserções: Nenhuma pesquisa HTTP automática | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B20 | Exceção inesperada do transporte | Barreira do evento evita encerramento e dados intactos | Confirmado pelas asserções: Barreira do evento evita encerramento e dados intactos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B21 | Busca volta a funcionar | Aviso antigo de falha removido | Confirmado pelas asserções: Aviso antigo de falha removido | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B22 | Texto alterado e cadastro imediato | Seleção anterior recusada antes do evento enfileirado | Confirmado pelas asserções: Seleção anterior recusada antes do evento enfileirado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| B23 | Renderizar telas com dados simulados | Quadros de início, lista e erro capturados | Confirmado pelas asserções: Quadros de início, lista e erro capturados | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Regressão dos filtros — 10 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| F01 | Filtrar título | Somente título correspondente exibido | Confirmado pelas asserções: Somente título correspondente exibido | Automatizado (Avalonia Headless/Skia) | Aprovado |
| F02 | Título com caixa diferente | Busca ignora maiúsculas e minúsculas | Confirmado pelas asserções: Busca ignora maiúsculas e minúsculas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| F03 | Filtrar categoria | Somente categoria correspondente exibida | Confirmado pelas asserções: Somente categoria correspondente exibida | Automatizado (Avalonia Headless/Skia) | Aprovado |
| F04 | Busca por trecho | Correspondência parcial preservada | Confirmado pelas asserções: Correspondência parcial preservada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| F05 | Filtro sem resultado | Estado vazio exibido sem excluir dados | Confirmado pelas asserções: Estado vazio exibido sem excluir dados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| F06 | Filtrar status | Apenas jogos no status escolhido | Confirmado pelas asserções: Apenas jogos no status escolhido | Automatizado (Avalonia Headless/Skia) | Aprovado |
| F07 | Combinar texto e status | Ambos os critérios aplicados | Confirmado pelas asserções: Ambos os critérios aplicados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| F08 | Filtro ativo | Total da coleção não é alterado | Confirmado pelas asserções: Total da coleção não é alterado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| F09 | Limpar filtros | Todos os registros reaparecem | Confirmado pelas asserções: Todos os registros reaparecem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| F10 | Seleção sai do filtro | Botões de editar e remover desabilitados | Confirmado pelas asserções: Botões de editar e remover desabilitados | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Modelos separados e conversão de compatibilidade — 20 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| MP01 | Separar registro legado | Título no catálogo e informações pessoais no bloco pessoal | Confirmado pelas asserções: Título no catálogo e informações pessoais no bloco pessoal | Automatizado | Aprovado |
| MP02 | ID legado sem IGDB | Origem externa continua desconhecida | Confirmado pelas asserções: Origem externa continua desconhecida | Automatizado | Aprovado |
| MP03 | Registro com IgdbId | Referência IGDB identificada explicitamente | Confirmado pelas asserções: Referência IGDB identificada explicitamente | Automatizado | Aprovado |
| MP04 | Dois identificadores no registro | Ambos preservados no roundtrip | Confirmado pelas asserções: Ambos preservados no roundtrip | Automatizado | Aprovado |
| MP05 | IDs ausentes | Valores continuam nulos no roundtrip | Confirmado pelas asserções: Valores continuam nulos no roundtrip | Automatizado | Aprovado |
| MP06 | ID legado zero | Valor preservado sem correção presumida | Confirmado pelas asserções: Valor preservado sem correção presumida | Automatizado | Aprovado |
| MP07 | ID legado negativo | Valor preservado sem reinterpretar origem | Confirmado pelas asserções: Valor preservado sem reinterpretar origem | Automatizado | Aprovado |
| MP08 | ID IGDB máximo | Inteiro máximo preservado | Confirmado pelas asserções: Inteiro máximo preservado | Automatizado | Aprovado |
| MP09 | Review com espaços e quebras | Texto exato preservado no roundtrip | Confirmado pelas asserções: Texto exato preservado no roundtrip | Automatizado | Aprovado |
| MP10 | Título Unicode | Texto preservado sem normalização | Confirmado pelas asserções: Texto preservado sem normalização | Automatizado | Aprovado |
| MP11 | Categoria pessoal | Categoria não convertida em gênero de catálogo | Confirmado pelas asserções: Categoria não convertida em gênero de catálogo | Automatizado | Aprovado |
| MP12 | Extras de JSON ao carregar | Dicionário original isolado do modelo | Confirmado pelas asserções: Dicionário original isolado do modelo | Automatizado | Aprovado |
| MP13 | Extras de JSON ao salvar | Alterar registro convertido não altera o modelo | Confirmado pelas asserções: Alterar registro convertido não altera o modelo | Automatizado | Aprovado |
| MP14 | Catálogo compartilhado por registros pessoais | Editar um registro não altera o outro | Confirmado pelas asserções: Editar um registro não altera o outro | Automatizado | Aprovado |
| MP15 | Preparar outra edição do catálogo | Informações pessoais permanecem independentes | Confirmado pelas asserções: Informações pessoais permanecem independentes | Automatizado | Aprovado |
| MP16 | Review legada longa e status desconhecido | Nenhum truncamento na conversão | Confirmado pelas asserções: Nenhum truncamento na conversão | Automatizado | Aprovado |
| MP17 | Modelos das seis mídias | Cada tipo pode representar uma obra sem ativar telas ou APIs | Confirmado pelas asserções: Cada tipo pode representar uma obra sem ativar telas ou APIs | Automatizado | Aprovado |
| MP18 | Registro completo após conversão | JSON semanticamente igual sem novo envelope | Confirmado pelas asserções: JSON semanticamente igual sem novo envelope | Automatizado | Aprovado |
| MP19 | Registro legado sem IGDB no salvamento | Campo IgdbId continua omitido | Confirmado pelas asserções: Campo IgdbId continua omitido | Automatizado | Aprovado |
| MP20 | Review legada vazia | Valor vazio preservado na conversão | Confirmado pelas asserções: Valor vazio preservado na conversão | Automatizado | Aprovado |

## Armazenamento pelo contrato e segurança do formato — 20 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| AR01 | Adaptador sem arquivo | Lista vazia sem criar arquivo | Confirmado pelas asserções: Lista vazia sem criar arquivo | Automatizado | Aprovado |
| AR02 | Adaptador carrega JSON legado | Dados separados sem regravar o arquivo | Confirmado pelas asserções: Dados separados sem regravar o arquivo | Automatizado | Aprovado |
| AR03 | Gravar pelo contrato | JSON continua no formato legado e recarregável | Confirmado pelas asserções: JSON continua no formato legado e recarregável | Automatizado | Aprovado |
| AR04 | JSON corrompido no adaptador | Gravação bloqueada e original preservado | Confirmado pelas asserções: Gravação bloqueada e original preservado | Automatizado | Aprovado |
| AR05 | Conflito externo pelo adaptador | Versão externa não sobrescrita | Confirmado pelas asserções: Versão externa não sobrescrita | Automatizado | Aprovado |
| AR06 | Backup da gravação pelo adaptador | Bytes anteriores preservados no bak | Confirmado pelas asserções: Bytes anteriores preservados no bak | Automatizado | Aprovado |
| AR07 | Origem do adaptador | Caminho explícito mantido sem migração | Confirmado pelas asserções: Caminho explícito mantido sem migração | Automatizado | Aprovado |
| AR08 | Propriedades JSON desconhecidas | Roundtrip pelo contrato mantém os extras | Confirmado pelas asserções: Roundtrip pelo contrato mantém os extras | Automatizado | Aprovado |
| AR09 | Tentar salvar anime no formato atual | Recusa explícita antes de qualquer gravação | Confirmado pelas asserções: Recusa explícita antes de qualquer gravação | Automatizado | Aprovado |
| AR10 | Tentar salvar série no formato atual | Recusa explícita sem perder dados | Confirmado pelas asserções: Recusa explícita sem perder dados | Automatizado | Aprovado |
| AR11 | Tentar salvar filme no formato atual | Recusa explícita sem perder dados | Confirmado pelas asserções: Recusa explícita sem perder dados | Automatizado | Aprovado |
| AR12 | Tentar salvar mangá no formato atual | Recusa explícita sem perder dados | Confirmado pelas asserções: Recusa explícita sem perder dados | Automatizado | Aprovado |
| AR13 | Tentar salvar livro no formato atual | Recusa explícita sem perder dados | Confirmado pelas asserções: Recusa explícita sem perder dados | Automatizado | Aprovado |
| AR14 | Catálogo com capa no formato atual | Recusa em vez de descartar a capa | Confirmado pelas asserções: Recusa em vez de descartar a capa | Automatizado | Aprovado |
| AR15 | Catálogo com gêneros no formato atual | Recusa em vez de descartar gêneros | Confirmado pelas asserções: Recusa em vez de descartar gêneros | Automatizado | Aprovado |
| AR16 | Referência externa de outro catálogo | Nenhuma conversão para ID IGDB | Confirmado pelas asserções: Nenhuma conversão para ID IGDB | Automatizado | Aprovado |
| AR17 | ID externo não numérico | Recusa sem conversão presumida | Confirmado pelas asserções: Recusa sem conversão presumida | Automatizado | Aprovado |
| AR18 | Referência incompleta | Origem sem ID não é descartada silenciosamente | Confirmado pelas asserções: Origem sem ID não é descartada silenciosamente | Automatizado | Aprovado |
| AR19 | Coleção mista com item não suportado | Lote inteiro recusado antes de escrever | Confirmado pelas asserções: Lote inteiro recusado antes de escrever | Automatizado | Aprovado |
| AR20 | Onze registros simulados com IDs repetidos | Quantidade e conteúdo preservados pelo adaptador | Confirmado pelas asserções: Quantidade e conteúdo preservados pelo adaptador | Automatizado | Aprovado |

## Carregamento da biblioteca pelo serviço — 10 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| CA01 | Carregar armazenamento vazio | Coleção carregada sem gravar | Confirmado pelas asserções: Coleção carregada sem gravar | Automatizado | Aprovado |
| CA02 | Carregar registros na ordem original | Todos os registros mantidos | Confirmado pelas asserções: Todos os registros mantidos | Automatizado | Aprovado |
| CA03 | Erro de leitura | Estado permanece não carregado | Confirmado pelas asserções: Estado permanece não carregado | Automatizado | Aprovado |
| CA04 | Falha ao recarregar | Gravações bloqueadas sem apagar a memória anterior | Confirmado pelas asserções: Gravações bloqueadas sem apagar a memória anterior | Automatizado | Aprovado |
| CA05 | Registro inválido na leitura | Coleção inteira recusada sem carga parcial | Confirmado pelas asserções: Coleção inteira recusada sem carga parcial | Automatizado | Aprovado |
| CA06 | Recuperar leitura após erro | Nova carga válida libera operações | Confirmado pelas asserções: Nova carga válida libera operações | Automatizado | Aprovado |
| CA07 | Categorias e status antigos | Textos legados aceitos sem normalização | Confirmado pelas asserções: Textos legados aceitos sem normalização | Automatizado | Aprovado |
| CA08 | Nota não finita na leitura | Coleção recusada | Confirmado pelas asserções: Coleção recusada | Automatizado | Aprovado |
| CA09 | Tipo de mídia desconhecido | Coleção recusada sem interpretação inventada | Confirmado pelas asserções: Coleção recusada sem interpretação inventada | Automatizado | Aprovado |
| CA10 | Lista do armazenamento muda após carregar | Quantidade em memória isolada da lista externa | Confirmado pelas asserções: Quantidade em memória isolada da lista externa | Automatizado | Aprovado |

## Cadastro pelo serviço — 10 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| AD01 | Adicionar item válido | Persistência e memória recebem o item | Confirmado pelas asserções: Persistência e memória recebem o item | Automatizado | Aprovado |
| AD02 | Adicionar mesmo catálogo duas vezes | Dois registros sem deduplicação | Confirmado pelas asserções: Dois registros sem deduplicação | Automatizado | Aprovado |
| AD03 | Adicionar antes de carregar | Operação recusada sem gravar | Confirmado pelas asserções: Operação recusada sem gravar | Automatizado | Aprovado |
| AD04 | Adicionar título vazio | Item recusado antes da persistência | Confirmado pelas asserções: Item recusado antes da persistência | Automatizado | Aprovado |
| AD05 | Adicionar nota fora do intervalo | Memória e armazenamento preservados | Confirmado pelas asserções: Memória e armazenamento preservados | Automatizado | Aprovado |
| AD06 | Erro de IO ao adicionar | Coleção anterior mantida | Confirmado pelas asserções: Coleção anterior mantida | Automatizado | Aprovado |
| AD07 | Sem permissão ao adicionar | Nenhum item aparece como salvo | Confirmado pelas asserções: Nenhum item aparece como salvo | Automatizado | Aprovado |
| AD08 | Recuperação após falha ao adicionar | Nova tentativa adiciona uma única vez | Confirmado pelas asserções: Nova tentativa adiciona uma única vez | Automatizado | Aprovado |
| AD09 | Adições sucessivas | Ordem de inserção preservada | Confirmado pelas asserções: Ordem de inserção preservada | Automatizado | Aprovado |
| AD10 | Coleção exposta à interface | Lista somente leitura impede alteração sem salvar | Confirmado pelas asserções: Lista somente leitura impede alteração sem salvar | Automatizado | Aprovado |

## Edição pessoal pelo serviço — 10 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| ED01 | Editar dados pessoais | Nova nota persistida sem alterar catálogo | Confirmado pelas asserções: Nova nota persistida sem alterar catálogo | Automatizado | Aprovado |
| ED02 | Editar registro com metadados legados | ID legado preservado | Confirmado pelas asserções: ID legado preservado | Automatizado | Aprovado |
| ED03 | Editar registro externo | Operação recusada sem gravar | Confirmado pelas asserções: Operação recusada sem gravar | Automatizado | Aprovado |
| ED04 | Dois registros com valores iguais | Somente instância selecionada atualizada | Confirmado pelas asserções: Somente instância selecionada atualizada | Automatizado | Aprovado |
| ED05 | Nota negativa na edição | Persistência não chamada | Confirmado pelas asserções: Persistência não chamada | Automatizado | Aprovado |
| ED06 | IO ao editar | Registro original permanece na memória e no armazenamento | Confirmado pelas asserções: Registro original permanece na memória e no armazenamento | Automatizado | Aprovado |
| ED07 | Sem permissão ao editar | Nota original não alterada | Confirmado pelas asserções: Nota original não alterada | Automatizado | Aprovado |
| ED08 | Repetir edição após falha | Alteração aplicada uma única vez | Confirmado pelas asserções: Alteração aplicada uma única vez | Automatizado | Aprovado |
| ED09 | Editar item removido | Nenhum outro registro é substituído | Confirmado pelas asserções: Nenhum outro registro é substituído | Automatizado | Aprovado |
| ED10 | Editar review legada vazia | Valor vazio preservado sem padrão imposto pelo serviço | Confirmado pelas asserções: Valor vazio preservado sem padrão imposto pelo serviço | Automatizado | Aprovado |

## Remoção pelo serviço — 10 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| RM01 | Remover registro válido | Memória e armazenamento ficam sem o item | Confirmado pelas asserções: Memória e armazenamento ficam sem o item | Automatizado | Aprovado |
| RM02 | Remover registro externo | Coleção preservada sem gravar | Confirmado pelas asserções: Coleção preservada sem gravar | Automatizado | Aprovado |
| RM03 | Remover segunda instância de registros iguais | Primeira instância preservada | Confirmado pelas asserções: Primeira instância preservada | Automatizado | Aprovado |
| RM04 | Remover com mesmo ID e títulos diferentes | Apenas registro escolhido removido | Confirmado pelas asserções: Apenas registro escolhido removido | Automatizado | Aprovado |
| RM05 | IO ao remover | Registro não desaparece da memória | Confirmado pelas asserções: Registro não desaparece da memória | Automatizado | Aprovado |
| RM06 | Sem permissão ao remover | Armazenamento não alterado | Confirmado pelas asserções: Armazenamento não alterado | Automatizado | Aprovado |
| RM07 | Repetir remoção após falha | Remoção confirmada uma única vez | Confirmado pelas asserções: Remoção confirmada uma única vez | Automatizado | Aprovado |
| RM08 | Remover o mesmo registro novamente | Segunda operação recusada sem novo salvamento | Confirmado pelas asserções: Segunda operação recusada sem novo salvamento | Automatizado | Aprovado |
| RM09 | Remover registro central | Ordem dos sobreviventes preservada | Confirmado pelas asserções: Ordem dos sobreviventes preservada | Automatizado | Aprovado |
| RM10 | Remover após recarga falha | Gravação bloqueada e estado anterior mantido | Confirmado pelas asserções: Gravação bloqueada e estado anterior mantido | Automatizado | Aprovado |

## Filtros, resumo e recentes pelo serviço — 13 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| FI01 | Consulta sem filtros | Todos os itens retornados na ordem original | Confirmado pelas asserções: Todos os itens retornados na ordem original | Automatizado | Aprovado |
| FI02 | Filtrar título com caixa diferente | Comparação ignora maiúsculas | Confirmado pelas asserções: Comparação ignora maiúsculas | Automatizado | Aprovado |
| FI03 | Filtrar categoria pessoal | Categoria pesquisável sem misturar com gêneros de catálogo | Confirmado pelas asserções: Categoria pesquisável sem misturar com gêneros de catálogo | Automatizado | Aprovado |
| FI04 | Texto e status combinados | Ambos os critérios respeitados | Confirmado pelas asserções: Ambos os critérios respeitados | Automatizado | Aprovado |
| FI05 | Filtro sem resultado | Dados originais continuam na biblioteca | Confirmado pelas asserções: Dados originais continuam na biblioteca | Automatizado | Aprovado |
| FI06 | Espaços no termo | Bordas aparadas antes da pesquisa | Confirmado pelas asserções: Bordas aparadas antes da pesquisa | Automatizado | Aprovado |
| FI07 | Resumo vazio | Contagens e progresso zero sem divisão inválida | Confirmado pelas asserções: Contagens e progresso zero sem divisão inválida | Automatizado | Aprovado |
| FI08 | Resumo com status mistos | Contagens reais e percentual preservados | Confirmado pelas asserções: Contagens reais e percentual preservados | Automatizado | Aprovado |
| FI09 | Recentes com mais de três itens | Últimos três por inserção em ordem inversa | Confirmado pelas asserções: Últimos três por inserção em ordem inversa | Automatizado | Aprovado |
| FI10 | Recentes vazios | Lista vazia válida | Confirmado pelas asserções: Lista vazia válida | Automatizado | Aprovado |
| FI11 | Filtros ativos | Resumo continua refletindo a coleção inteira | Confirmado pelas asserções: Resumo continua refletindo a coleção inteira | Automatizado | Aprovado |
| FI12 | Alterar array retornado pela consulta | Coleção interna não pode ser removida pelo resultado | Confirmado pelas asserções: Coleção interna não pode ser removida pelo resultado | Automatizado | Aprovado |
| FI13 | Editar status e remover | Resumo acompanha somente operações confirmadas | Confirmado pelas asserções: Resumo acompanha somente operações confirmadas | Automatizado | Aprovado |

## Preparação e validação do formulário — 21 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| FO01 | Preparar cadastro válido | Catálogo selecionado e dados pessoais separados | Confirmado pelas asserções: Catálogo selecionado e dados pessoais separados | Automatizado | Aprovado |
| FO02 | Cadastro sem seleção | Mensagem específica de seleção | Confirmado pelas asserções: Mensagem específica de seleção | Automatizado | Aprovado |
| FO03 | Texto alterado após seleção | Cadastro bloqueado antes de gravar | Confirmado pelas asserções: Cadastro bloqueado antes de gravar | Automatizado | Aprovado |
| FO04 | Nota vazia | Mensagem de nota inválida | Confirmado pelas asserções: Mensagem de nota inválida | Automatizado | Aprovado |
| FO05 | Nota abaixo de zero | Entrada recusada | Confirmado pelas asserções: Entrada recusada | Automatizado | Aprovado |
| FO06 | Nota acima de cinco | Entrada recusada | Confirmado pelas asserções: Entrada recusada | Automatizado | Aprovado |
| FO07 | Nota zero | Limite inferior aceito | Confirmado pelas asserções: Limite inferior aceito | Automatizado | Aprovado |
| FO08 | Nota cinco | Limite superior aceito | Confirmado pelas asserções: Limite superior aceito | Automatizado | Aprovado |
| FO09 | Categoria em branco | Mensagem de dados incompletos | Confirmado pelas asserções: Mensagem de dados incompletos | Automatizado | Aprovado |
| FO10 | Status ausente | Entrada recusada sem valor padrão inventado | Confirmado pelas asserções: Entrada recusada sem valor padrão inventado | Automatizado | Aprovado |
| FO11 | Nova review vazia | Texto padrão preserva comportamento anterior | Confirmado pelas asserções: Texto padrão preserva comportamento anterior | Automatizado | Aprovado |
| FO12 | Nova review com espaços nas bordas | Texto aparado sem remover quebras internas | Confirmado pelas asserções: Texto aparado sem remover quebras internas | Automatizado | Aprovado |
| FO13 | Nova review com mil caracteres | Limite aceito integralmente | Confirmado pelas asserções: Limite aceito integralmente | Automatizado | Aprovado |
| FO14 | Nova review acima do limite | Entrada recusada sem truncar | Confirmado pelas asserções: Entrada recusada sem truncar | Automatizado | Aprovado |
| FO15 | Editar review antiga longa sem alterá-la | Texto antigo mantido integralmente | Confirmado pelas asserções: Texto antigo mantido integralmente | Automatizado | Aprovado |
| FO16 | Editar dados pessoais de legado | Catálogo, ID legado e review vazia preservados | Confirmado pelas asserções: Catálogo, ID legado e review vazia preservados | Automatizado | Aprovado |
| FO17 | Seleção de outra mídia | Formulário atual de jogos recusa o cadastro | Confirmado pelas asserções: Formulário atual de jogos recusa o cadastro | Automatizado | Aprovado |
| FO18 | Seleção de outro provedor | Cadastro recusado sem fingir origem IGDB | Confirmado pelas asserções: Cadastro recusado sem fingir origem IGDB | Automatizado | Aprovado |
| FO19 | Seleção com ID inválido | Cadastro recusado | Confirmado pelas asserções: Cadastro recusado | Automatizado | Aprovado |
| FO20 | Editar sem selecionar novo catálogo | Título e identificador existentes mantidos | Confirmado pelas asserções: Título e identificador existentes mantidos | Automatizado | Aprovado |
| FO21 | ID externo com zeros à esquerda | Seleção recusada antes de chegar ao armazenamento legado | Confirmado pelas asserções: Seleção recusada antes de chegar ao armazenamento legado | Automatizado | Aprovado |

## Pesquisa pelo contrato de catálogo — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| PE01 | Pesquisa IGDB pelo contrato | Retorna modelo de catálogo de jogo com referência IGDB | Confirmado pelas asserções: Retorna modelo de catálogo de jogo com referência IGDB | Automatizado | Aprovado |
| PE02 | Pesquisa sem jogos | Lista vazia mantida pelo adaptador | Confirmado pelas asserções: Lista vazia mantida pelo adaptador | Automatizado | Aprovado |
| PE03 | Vários resultados | Ordem dos resultados e IDs preservados | Confirmado pelas asserções: Ordem dos resultados e IDs preservados | Automatizado | Aprovado |
| PE04 | Pesquisa com texto Unicode | Título retornado preservado | Confirmado pelas asserções: Título retornado preservado | Automatizado | Aprovado |
| PE05 | HTTP 401 através do adaptador | Falha de autenticação preservada sem lista enganosa | Confirmado pelas asserções: Falha de autenticação preservada sem lista enganosa | Automatizado | Aprovado |
| PE06 | HTTP 429 através do adaptador | Falha de limite preservada | Confirmado pelas asserções: Falha de limite preservada | Automatizado | Aprovado |
| PE07 | HTTP 503 através do adaptador | Falha de indisponibilidade preservada | Confirmado pelas asserções: Falha de indisponibilidade preservada | Automatizado | Aprovado |
| PE08 | JSON inválido através do adaptador | Exceção controlada preservada | Confirmado pelas asserções: Exceção controlada preservada | Automatizado | Aprovado |
| PE09 | Cancelamento antes da pesquisa | Nenhum HTTP enviado pelo adaptador | Confirmado pelas asserções: Nenhum HTTP enviado pelo adaptador | Automatizado | Aprovado |
| PE10 | Cancelamento durante HTTP | Sinal propagado pelo contrato até o transporte | Confirmado pelas asserções: Sinal propagado pelo contrato até o transporte | Automatizado | Aprovado |
| PE11 | Resultado de pesquisa | Nenhum dado pessoal inventado nem download de capa | Confirmado pelas asserções: Nenhum dado pessoal inventado nem download de capa | Automatizado | Aprovado |
| PE12 | Recuperar pesquisa após conexão falha | Próxima chamada retorna catálogo válido | Confirmado pelas asserções: Próxima chamada retorna catálogo válido | Automatizado | Aprovado |

## Composição de dependências e configuração — 10 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| CO01 | Composição sem credenciais | Biblioteca local disponível sem pesquisa ou recurso HTTP | Confirmado pelas asserções: Biblioteca local disponível sem pesquisa ou recurso HTTP | Automatizado | Aprovado |
| CO02 | Composição com token legado | Adaptador IGDB e cliente próprio criados | Confirmado pelas asserções: Adaptador IGDB e cliente próprio criados | Automatizado | Aprovado |
| CO03 | Composição com token corrigido | Adaptador IGDB disponível sem nome legado | Confirmado pelas asserções: Adaptador IGDB disponível sem nome legado | Automatizado | Aprovado |
| CO04 | Configuração ausente | Fábrica HTTP nem sequer é chamada | Confirmado pelas asserções: Fábrica HTTP nem sequer é chamada | Automatizado | Aprovado |
| CO05 | Ambos os tokens na composição | Cabeçalho usa token corrigido pela cadeia real de adaptadores | Confirmado pelas asserções: Cabeçalho usa token corrigido pela cadeia real de adaptadores | Automatizado | Aprovado |
| CO06 | Credencial com quebra interna | Pesquisa desativada sem criar HTTP | Confirmado pelas asserções: Pesquisa desativada sem criar HTTP | Automatizado | Aprovado |
| CO07 | Caminho informado na composição | Origem mantida sem mudança automática de diretório | Confirmado pelas asserções: Origem mantida sem mudança automática de diretório | Automatizado | Aprovado |
| CO08 | Cliente criado pela composição | Timeout de quinze segundos configurado | Confirmado pelas asserções: Timeout de quinze segundos configurado | Automatizado | Aprovado |
| CO09 | Compor aplicação com arquivo existente | Construção não carrega nem grava os dados | Confirmado pelas asserções: Construção não carrega nem grava os dados | Automatizado | Aprovado |
| CO10 | Duas composições independentes | Clientes e serviços não compartilham estado de sessão | Confirmado pelas asserções: Clientes e serviços não compartilham estado de sessão | Automatizado | Aprovado |

## Integração da janela com contratos substituídos — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| UI01 | Janela com armazenamento por contrato | Coleção abre sem FileManager ou arquivo JSON | Confirmado pelas asserções: Coleção abre sem FileManager ou arquivo JSON | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UI02 | Cadastro pela janela e armazenamento por contrato | Item persistido com catálogo e dados pessoais separados | Confirmado pelas asserções: Item persistido com catálogo e dados pessoais separados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UI03 | Edição local por contrato | Nota atualizada e referência de catálogo preservada | Confirmado pelas asserções: Nota atualizada e referência de catálogo preservada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UI04 | Exclusão por contrato | Registro selecionado removido do armazenamento | Confirmado pelas asserções: Registro selecionado removido do armazenamento | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UI05 | Erro na leitura do armazenamento substituído | Janela abre com aviso e gravação desativada | Confirmado pelas asserções: Janela abre com aviso e gravação desativada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UI06 | Erro na gravação do armazenamento substituído | Formulário e coleção anteriores mantidos | Confirmado pelas asserções: Formulário e coleção anteriores mantidos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UI07 | Pesquisa substituída por contrato | Sugestões usam modelo comum sem depender da IGDB | Confirmado pelas asserções: Sugestões usam modelo comum sem depender da IGDB | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UI08 | Erro inesperado em pesquisa substituída | Aviso seguro sem encerrar a janela | Confirmado pelas asserções: Aviso seguro sem encerrar a janela | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UI09 | Recurso próprio entregue à janela | Fechamento descarta o recurso uma vez | Confirmado pelas asserções: Fechamento descarta o recurso uma vez | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UI10 | Pesquisa sem transferência de propriedade | Janela não descarta recurso do chamador | Confirmado pelas asserções: Janela não descarta recurso do chamador | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UI11 | Editar segunda cópia com valores idênticos | Apenas o segundo registro recebe a edição | Confirmado pelas asserções: Apenas o segundo registro recebe a edição | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UI12 | Filtrar mantendo duas cópias idênticas | Seleção da segunda instância preservada | Confirmado pelas asserções: Seleção da segunda instância preservada | Automatizado (Avalonia Headless/Skia) | Aprovado |

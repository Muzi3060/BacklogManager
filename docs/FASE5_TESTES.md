# Fase 5 — Identidade visual: resultados individuais

Gerado do TRX real por `scripts/gerar_relatorio_fase5.py`. Inclui os 1.105 cenários anteriores executados novamente e 161 novos cenários. TX02 acompanha o título neutro autorizado. Nenhum cenário anterior foi excluído; os relatórios históricos foram preservados.

**Planejados na suíte: 1266; executados: 1266; aprovados: 1266; reprovados: 0; não executados: 0.**

Aprovação corresponde às asserções executadas. A suíte usa dados simulados, arquivos temporários e HTTP simulado; não usa a biblioteca original. Não representa nova validação autenticada da IGDB ou interação manual no Rider. Verificações adicionais e pendências constam em `FASE5.md`.

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
| A07 | Jogo IGDB sem categoria manual | Cadastro aceito sem categoria inventada | Confirmado pelas asserções: Cadastro aceito sem categoria inventada | Automatizado (Avalonia Headless/Skia) | Aprovado |
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
| AR09 | Salvar anime no formato estendido | Tipo preservado no bloco versionado | Confirmado pelas asserções: Tipo preservado no bloco versionado | Automatizado | Aprovado |
| AR10 | Salvar série no formato estendido | Tipo preservado no bloco versionado | Confirmado pelas asserções: Tipo preservado no bloco versionado | Automatizado | Aprovado |
| AR11 | Salvar filme no formato estendido | Tipo preservado no bloco versionado | Confirmado pelas asserções: Tipo preservado no bloco versionado | Automatizado | Aprovado |
| AR12 | Salvar mangá no formato estendido | Tipo preservado no bloco versionado | Confirmado pelas asserções: Tipo preservado no bloco versionado | Automatizado | Aprovado |
| AR13 | Salvar livro no formato estendido | Tipo preservado no bloco versionado | Confirmado pelas asserções: Tipo preservado no bloco versionado | Automatizado | Aprovado |
| AR14 | Catálogo com capa | Referência preservada sem download | Confirmado pelas asserções: Referência preservada sem download | Automatizado | Aprovado |
| AR15 | Catálogo com gêneros | Gêneros preservados no bloco versionado | Confirmado pelas asserções: Gêneros preservados no bloco versionado | Automatizado | Aprovado |
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
| FO09 | Jogo com seleção IGDB e categoria em branco | Cadastro usa somente os gêneros externos | Confirmado pelas asserções: Cadastro usa somente os gêneros externos | Automatizado | Aprovado |
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

## Cinco catálogos simulados — dez cenários por mídia — 50 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| SM01 | Anime: Pesquisar exemplos | Três resultados fictícios identificados | Confirmado pelas asserções: Três resultados fictícios identificados | Automatizado | Aprovado |
| SM02 | Anime: Pesquisar termo vazio | Lista vazia sem inventar resultado | Confirmado pelas asserções: Lista vazia sem inventar resultado | Automatizado | Aprovado |
| SM03 | Anime: Pesquisar termo nulo | Lista vazia sem exceção | Confirmado pelas asserções: Lista vazia sem exceção | Automatizado | Aprovado |
| SM04 | Anime: Pesquisar com caixa diferente | Pesquisa ignora maiúsculas | Confirmado pelas asserções: Pesquisa ignora maiúsculas | Automatizado | Aprovado |
| SM05 | Anime: Pesquisar título parcial | Somente a obra correspondente | Confirmado pelas asserções: Somente a obra correspondente | Automatizado | Aprovado |
| SM06 | Anime: Pesquisar termo ausente | Lista vazia válida | Confirmado pelas asserções: Lista vazia válida | Automatizado | Aprovado |
| SM07 | Anime: Cancelar antes da pesquisa | Cancelamento respeitado | Confirmado pelas asserções: Cancelamento respeitado | Automatizado | Aprovado |
| SM08 | Anime: Repetir pesquisa | IDs estáveis e distintos no catálogo | Confirmado pelas asserções: IDs estáveis e distintos no catálogo | Automatizado | Aprovado |
| SM09 | Anime: Consultar metadados | Tipo, gêneros, capa local e origem simulada disponíveis | Confirmado pelas asserções: Tipo, gêneros, capa local e origem simulada disponíveis | Automatizado | Aprovado |
| SM10 | Anime: Salvar resultado do catálogo | Origem simulada e referências preservadas após recarga | Confirmado pelas asserções: Origem simulada e referências preservadas após recarga | Automatizado | Aprovado |
| SM11 | Serie: Pesquisar exemplos | Três resultados fictícios identificados | Confirmado pelas asserções: Três resultados fictícios identificados | Automatizado | Aprovado |
| SM12 | Serie: Pesquisar termo vazio | Lista vazia sem inventar resultado | Confirmado pelas asserções: Lista vazia sem inventar resultado | Automatizado | Aprovado |
| SM13 | Serie: Pesquisar termo nulo | Lista vazia sem exceção | Confirmado pelas asserções: Lista vazia sem exceção | Automatizado | Aprovado |
| SM14 | Serie: Pesquisar com caixa diferente | Pesquisa ignora maiúsculas | Confirmado pelas asserções: Pesquisa ignora maiúsculas | Automatizado | Aprovado |
| SM15 | Serie: Pesquisar título parcial | Somente a obra correspondente | Confirmado pelas asserções: Somente a obra correspondente | Automatizado | Aprovado |
| SM16 | Serie: Pesquisar termo ausente | Lista vazia válida | Confirmado pelas asserções: Lista vazia válida | Automatizado | Aprovado |
| SM17 | Serie: Cancelar antes da pesquisa | Cancelamento respeitado | Confirmado pelas asserções: Cancelamento respeitado | Automatizado | Aprovado |
| SM18 | Serie: Repetir pesquisa | IDs estáveis e distintos no catálogo | Confirmado pelas asserções: IDs estáveis e distintos no catálogo | Automatizado | Aprovado |
| SM19 | Serie: Consultar metadados | Tipo, gêneros, capa local e origem simulada disponíveis | Confirmado pelas asserções: Tipo, gêneros, capa local e origem simulada disponíveis | Automatizado | Aprovado |
| SM20 | Serie: Salvar resultado do catálogo | Origem simulada e referências preservadas após recarga | Confirmado pelas asserções: Origem simulada e referências preservadas após recarga | Automatizado | Aprovado |
| SM21 | Filme: Pesquisar exemplos | Três resultados fictícios identificados | Confirmado pelas asserções: Três resultados fictícios identificados | Automatizado | Aprovado |
| SM22 | Filme: Pesquisar termo vazio | Lista vazia sem inventar resultado | Confirmado pelas asserções: Lista vazia sem inventar resultado | Automatizado | Aprovado |
| SM23 | Filme: Pesquisar termo nulo | Lista vazia sem exceção | Confirmado pelas asserções: Lista vazia sem exceção | Automatizado | Aprovado |
| SM24 | Filme: Pesquisar com caixa diferente | Pesquisa ignora maiúsculas | Confirmado pelas asserções: Pesquisa ignora maiúsculas | Automatizado | Aprovado |
| SM25 | Filme: Pesquisar título parcial | Somente a obra correspondente | Confirmado pelas asserções: Somente a obra correspondente | Automatizado | Aprovado |
| SM26 | Filme: Pesquisar termo ausente | Lista vazia válida | Confirmado pelas asserções: Lista vazia válida | Automatizado | Aprovado |
| SM27 | Filme: Cancelar antes da pesquisa | Cancelamento respeitado | Confirmado pelas asserções: Cancelamento respeitado | Automatizado | Aprovado |
| SM28 | Filme: Repetir pesquisa | IDs estáveis e distintos no catálogo | Confirmado pelas asserções: IDs estáveis e distintos no catálogo | Automatizado | Aprovado |
| SM29 | Filme: Consultar metadados | Tipo, gêneros, capa local e origem simulada disponíveis | Confirmado pelas asserções: Tipo, gêneros, capa local e origem simulada disponíveis | Automatizado | Aprovado |
| SM30 | Filme: Salvar resultado do catálogo | Origem simulada e referências preservadas após recarga | Confirmado pelas asserções: Origem simulada e referências preservadas após recarga | Automatizado | Aprovado |
| SM31 | Manga: Pesquisar exemplos | Três resultados fictícios identificados | Confirmado pelas asserções: Três resultados fictícios identificados | Automatizado | Aprovado |
| SM32 | Manga: Pesquisar termo vazio | Lista vazia sem inventar resultado | Confirmado pelas asserções: Lista vazia sem inventar resultado | Automatizado | Aprovado |
| SM33 | Manga: Pesquisar termo nulo | Lista vazia sem exceção | Confirmado pelas asserções: Lista vazia sem exceção | Automatizado | Aprovado |
| SM34 | Manga: Pesquisar com caixa diferente | Pesquisa ignora maiúsculas | Confirmado pelas asserções: Pesquisa ignora maiúsculas | Automatizado | Aprovado |
| SM35 | Manga: Pesquisar título parcial | Somente a obra correspondente | Confirmado pelas asserções: Somente a obra correspondente | Automatizado | Aprovado |
| SM36 | Manga: Pesquisar termo ausente | Lista vazia válida | Confirmado pelas asserções: Lista vazia válida | Automatizado | Aprovado |
| SM37 | Manga: Cancelar antes da pesquisa | Cancelamento respeitado | Confirmado pelas asserções: Cancelamento respeitado | Automatizado | Aprovado |
| SM38 | Manga: Repetir pesquisa | IDs estáveis e distintos no catálogo | Confirmado pelas asserções: IDs estáveis e distintos no catálogo | Automatizado | Aprovado |
| SM39 | Manga: Consultar metadados | Tipo, gêneros, capa local e origem simulada disponíveis | Confirmado pelas asserções: Tipo, gêneros, capa local e origem simulada disponíveis | Automatizado | Aprovado |
| SM40 | Manga: Salvar resultado do catálogo | Origem simulada e referências preservadas após recarga | Confirmado pelas asserções: Origem simulada e referências preservadas após recarga | Automatizado | Aprovado |
| SM41 | Livro: Pesquisar exemplos | Três resultados fictícios identificados | Confirmado pelas asserções: Três resultados fictícios identificados | Automatizado | Aprovado |
| SM42 | Livro: Pesquisar termo vazio | Lista vazia sem inventar resultado | Confirmado pelas asserções: Lista vazia sem inventar resultado | Automatizado | Aprovado |
| SM43 | Livro: Pesquisar termo nulo | Lista vazia sem exceção | Confirmado pelas asserções: Lista vazia sem exceção | Automatizado | Aprovado |
| SM44 | Livro: Pesquisar com caixa diferente | Pesquisa ignora maiúsculas | Confirmado pelas asserções: Pesquisa ignora maiúsculas | Automatizado | Aprovado |
| SM45 | Livro: Pesquisar título parcial | Somente a obra correspondente | Confirmado pelas asserções: Somente a obra correspondente | Automatizado | Aprovado |
| SM46 | Livro: Pesquisar termo ausente | Lista vazia válida | Confirmado pelas asserções: Lista vazia válida | Automatizado | Aprovado |
| SM47 | Livro: Cancelar antes da pesquisa | Cancelamento respeitado | Confirmado pelas asserções: Cancelamento respeitado | Automatizado | Aprovado |
| SM48 | Livro: Repetir pesquisa | IDs estáveis e distintos no catálogo | Confirmado pelas asserções: IDs estáveis e distintos no catálogo | Automatizado | Aprovado |
| SM49 | Livro: Consultar metadados | Tipo, gêneros, capa local e origem simulada disponíveis | Confirmado pelas asserções: Tipo, gêneros, capa local e origem simulada disponíveis | Automatizado | Aprovado |
| SM50 | Livro: Salvar resultado do catálogo | Origem simulada e referências preservadas após recarga | Confirmado pelas asserções: Origem simulada e referências preservadas após recarga | Automatizado | Aprovado |

## Progresso pessoal — dez cenários por mídia — 60 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| PG01 | Jogo: Progresso ausente | Campo opcional aceito | Confirmado pelas asserções: Campo opcional aceito | Automatizado | Aprovado |
| PG02 | Jogo: Progresso vazio | Nenhuma unidade inventada | Confirmado pelas asserções: Nenhuma unidade inventada | Automatizado | Aprovado |
| PG03 | Jogo: Valor zero | Zero distinguido de ausência | Confirmado pelas asserções: Zero distinguido de ausência | Automatizado | Aprovado |
| PG04 | Jogo: Valor positivo | Progresso pessoal aceito | Confirmado pelas asserções: Progresso pessoal aceito | Automatizado | Aprovado |
| PG05 | Jogo: Valor negativo | Entrada recusada | Confirmado pelas asserções: Entrada recusada | Automatizado | Aprovado |
| PG06 | Jogo: Campo de outra mídia | Unidade incompatível recusada | Confirmado pelas asserções: Unidade incompatível recusada | Automatizado | Aprovado |
| PG07 | Jogo: Valor máximo representável | Limite do tipo aceito sem converter unidade | Confirmado pelas asserções: Limite do tipo aceito sem converter unidade | Automatizado | Aprovado |
| PG08 | Jogo: Formulário sem progresso | Seleção de catálogo não gera duração pessoal | Confirmado pelas asserções: Seleção de catálogo não gera duração pessoal | Automatizado | Aprovado |
| PG09 | Jogo: Exibir unidade correta | Texto corresponde ao tipo de mídia | Confirmado pelas asserções: Texto corresponde ao tipo de mídia | Automatizado | Aprovado |
| PG10 | Jogo: Persistir e recarregar progresso | Valor pessoal preservado no JSON | Confirmado pelas asserções: Valor pessoal preservado no JSON | Automatizado | Aprovado |
| PG11 | Anime: Progresso ausente | Campo opcional aceito | Confirmado pelas asserções: Campo opcional aceito | Automatizado | Aprovado |
| PG12 | Anime: Progresso vazio | Nenhuma unidade inventada | Confirmado pelas asserções: Nenhuma unidade inventada | Automatizado | Aprovado |
| PG13 | Anime: Valor zero | Zero distinguido de ausência | Confirmado pelas asserções: Zero distinguido de ausência | Automatizado | Aprovado |
| PG14 | Anime: Valor positivo | Progresso pessoal aceito | Confirmado pelas asserções: Progresso pessoal aceito | Automatizado | Aprovado |
| PG15 | Anime: Valor negativo | Entrada recusada | Confirmado pelas asserções: Entrada recusada | Automatizado | Aprovado |
| PG16 | Anime: Campo de outra mídia | Unidade incompatível recusada | Confirmado pelas asserções: Unidade incompatível recusada | Automatizado | Aprovado |
| PG17 | Anime: Valor máximo representável | Limite do tipo aceito sem converter unidade | Confirmado pelas asserções: Limite do tipo aceito sem converter unidade | Automatizado | Aprovado |
| PG18 | Anime: Formulário sem progresso | Seleção de catálogo não gera duração pessoal | Confirmado pelas asserções: Seleção de catálogo não gera duração pessoal | Automatizado | Aprovado |
| PG19 | Anime: Exibir unidade correta | Texto corresponde ao tipo de mídia | Confirmado pelas asserções: Texto corresponde ao tipo de mídia | Automatizado | Aprovado |
| PG20 | Anime: Persistir e recarregar progresso | Valor pessoal preservado no JSON | Confirmado pelas asserções: Valor pessoal preservado no JSON | Automatizado | Aprovado |
| PG21 | Serie: Progresso ausente | Campo opcional aceito | Confirmado pelas asserções: Campo opcional aceito | Automatizado | Aprovado |
| PG22 | Serie: Progresso vazio | Nenhuma unidade inventada | Confirmado pelas asserções: Nenhuma unidade inventada | Automatizado | Aprovado |
| PG23 | Serie: Valor zero | Zero distinguido de ausência | Confirmado pelas asserções: Zero distinguido de ausência | Automatizado | Aprovado |
| PG24 | Serie: Valor positivo | Progresso pessoal aceito | Confirmado pelas asserções: Progresso pessoal aceito | Automatizado | Aprovado |
| PG25 | Serie: Valor negativo | Entrada recusada | Confirmado pelas asserções: Entrada recusada | Automatizado | Aprovado |
| PG26 | Serie: Campo de outra mídia | Unidade incompatível recusada | Confirmado pelas asserções: Unidade incompatível recusada | Automatizado | Aprovado |
| PG27 | Serie: Segundo campo opcional | Campo complementar aceito | Confirmado pelas asserções: Campo complementar aceito | Automatizado | Aprovado |
| PG28 | Serie: Segundo campo negativo | Complemento negativo recusado | Confirmado pelas asserções: Complemento negativo recusado | Automatizado | Aprovado |
| PG29 | Serie: Exibir unidade correta | Texto corresponde ao tipo de mídia | Confirmado pelas asserções: Texto corresponde ao tipo de mídia | Automatizado | Aprovado |
| PG30 | Serie: Persistir e recarregar progresso | Valor pessoal preservado no JSON | Confirmado pelas asserções: Valor pessoal preservado no JSON | Automatizado | Aprovado |
| PG31 | Filme: Progresso ausente | Campo opcional aceito | Confirmado pelas asserções: Campo opcional aceito | Automatizado | Aprovado |
| PG32 | Filme: Progresso vazio | Nenhuma unidade inventada | Confirmado pelas asserções: Nenhuma unidade inventada | Automatizado | Aprovado |
| PG33 | Filme: Valor zero | Zero distinguido de ausência | Confirmado pelas asserções: Zero distinguido de ausência | Automatizado | Aprovado |
| PG34 | Filme: Valor positivo | Progresso pessoal aceito | Confirmado pelas asserções: Progresso pessoal aceito | Automatizado | Aprovado |
| PG35 | Filme: Valor negativo | Entrada recusada | Confirmado pelas asserções: Entrada recusada | Automatizado | Aprovado |
| PG36 | Filme: Campo de outra mídia | Unidade incompatível recusada | Confirmado pelas asserções: Unidade incompatível recusada | Automatizado | Aprovado |
| PG37 | Filme: Valor máximo representável | Limite do tipo aceito sem converter unidade | Confirmado pelas asserções: Limite do tipo aceito sem converter unidade | Automatizado | Aprovado |
| PG38 | Filme: Formulário sem progresso | Seleção de catálogo não gera duração pessoal | Confirmado pelas asserções: Seleção de catálogo não gera duração pessoal | Automatizado | Aprovado |
| PG39 | Filme: Exibir unidade correta | Texto corresponde ao tipo de mídia | Confirmado pelas asserções: Texto corresponde ao tipo de mídia | Automatizado | Aprovado |
| PG40 | Filme: Persistir e recarregar progresso | Valor pessoal preservado no JSON | Confirmado pelas asserções: Valor pessoal preservado no JSON | Automatizado | Aprovado |
| PG41 | Manga: Progresso ausente | Campo opcional aceito | Confirmado pelas asserções: Campo opcional aceito | Automatizado | Aprovado |
| PG42 | Manga: Progresso vazio | Nenhuma unidade inventada | Confirmado pelas asserções: Nenhuma unidade inventada | Automatizado | Aprovado |
| PG43 | Manga: Valor zero | Zero distinguido de ausência | Confirmado pelas asserções: Zero distinguido de ausência | Automatizado | Aprovado |
| PG44 | Manga: Valor positivo | Progresso pessoal aceito | Confirmado pelas asserções: Progresso pessoal aceito | Automatizado | Aprovado |
| PG45 | Manga: Valor negativo | Entrada recusada | Confirmado pelas asserções: Entrada recusada | Automatizado | Aprovado |
| PG46 | Manga: Campo de outra mídia | Unidade incompatível recusada | Confirmado pelas asserções: Unidade incompatível recusada | Automatizado | Aprovado |
| PG47 | Manga: Segundo campo opcional | Campo complementar aceito | Confirmado pelas asserções: Campo complementar aceito | Automatizado | Aprovado |
| PG48 | Manga: Segundo campo negativo | Complemento negativo recusado | Confirmado pelas asserções: Complemento negativo recusado | Automatizado | Aprovado |
| PG49 | Manga: Exibir unidade correta | Texto corresponde ao tipo de mídia | Confirmado pelas asserções: Texto corresponde ao tipo de mídia | Automatizado | Aprovado |
| PG50 | Manga: Persistir e recarregar progresso | Valor pessoal preservado no JSON | Confirmado pelas asserções: Valor pessoal preservado no JSON | Automatizado | Aprovado |
| PG51 | Livro: Progresso ausente | Campo opcional aceito | Confirmado pelas asserções: Campo opcional aceito | Automatizado | Aprovado |
| PG52 | Livro: Progresso vazio | Nenhuma unidade inventada | Confirmado pelas asserções: Nenhuma unidade inventada | Automatizado | Aprovado |
| PG53 | Livro: Valor zero | Zero distinguido de ausência | Confirmado pelas asserções: Zero distinguido de ausência | Automatizado | Aprovado |
| PG54 | Livro: Valor positivo | Progresso pessoal aceito | Confirmado pelas asserções: Progresso pessoal aceito | Automatizado | Aprovado |
| PG55 | Livro: Valor negativo | Entrada recusada | Confirmado pelas asserções: Entrada recusada | Automatizado | Aprovado |
| PG56 | Livro: Campo de outra mídia | Unidade incompatível recusada | Confirmado pelas asserções: Unidade incompatível recusada | Automatizado | Aprovado |
| PG57 | Livro: Segundo campo opcional | Campo complementar aceito | Confirmado pelas asserções: Campo complementar aceito | Automatizado | Aprovado |
| PG58 | Livro: Segundo campo negativo | Complemento negativo recusado | Confirmado pelas asserções: Complemento negativo recusado | Automatizado | Aprovado |
| PG59 | Livro: Exibir unidade correta | Texto corresponde ao tipo de mídia | Confirmado pelas asserções: Texto corresponde ao tipo de mídia | Automatizado | Aprovado |
| PG60 | Livro: Persistir e recarregar progresso | Valor pessoal preservado no JSON | Confirmado pelas asserções: Valor pessoal preservado no JSON | Automatizado | Aprovado |

## Formato multimídia e compatibilidade JSON — 30 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| JM01 | Arquivo legado sem bloco novo | Leitura não grava nem converte SteamAppId | Confirmado pelas asserções: Leitura não grava nem converte SteamAppId | Automatizado | Aprovado |
| JM02 | Roundtrip legado sem progresso | JSON semanticamente igual sem bloco opcional | Confirmado pelas asserções: JSON semanticamente igual sem bloco opcional | Automatizado | Aprovado |
| JM03 | Coleção mista com seis tipos | Todos os registros e tipos sobrevivem à recarga | Confirmado pelas asserções: Todos os registros e tipos sobrevivem à recarga | Automatizado | Aprovado |
| JM04 | Versão do bloco multimídia | Versão um explícita sem mudar raiz de lista | Confirmado pelas asserções: Versão um explícita sem mudar raiz de lista | Automatizado | Aprovado |
| JM05 | Versão futura do bloco | Leitura e gravação bloqueadas sem apagar arquivo | Confirmado pelas asserções: Leitura e gravação bloqueadas sem apagar arquivo | Automatizado | Aprovado |
| JM06 | Bloco sem versão | Formato incompleto recusado sem assumir versão | Confirmado pelas asserções: Formato incompleto recusado sem assumir versão | Automatizado | Aprovado |
| JM07 | Bloco sem tipo | Tipo ausente não vira jogo silenciosamente | Confirmado pelas asserções: Tipo ausente não vira jogo silenciosamente | Automatizado | Aprovado |
| JM08 | Tipo desconhecido | Arquivo preservado sem reinterpretar mídia | Confirmado pelas asserções: Arquivo preservado sem reinterpretar mídia | Automatizado | Aprovado |
| JM09 | Tipo numérico indefinido | Arquivo preservado sem aceitar enum inválido | Confirmado pelas asserções: Arquivo preservado sem aceitar enum inválido | Automatizado | Aprovado |
| JM10 | Campo novo desconhecido no bloco | Carga bloqueada em vez de descartar informação futura | Confirmado pelas asserções: Carga bloqueada em vez de descartar informação futura | Automatizado | Aprovado |
| JM11 | Campo de progresso desconhecido | Nenhum progresso futuro descartado silenciosamente | Confirmado pelas asserções: Nenhum progresso futuro descartado silenciosamente | Automatizado | Aprovado |
| JM12 | Progresso negativo no JSON | Carga recusada sem normalizar dados | Confirmado pelas asserções: Carga recusada sem normalizar dados | Automatizado | Aprovado |
| JM13 | Progresso com unidade incompatível | Carga recusada sem perda de dados | Confirmado pelas asserções: Carga recusada sem perda de dados | Automatizado | Aprovado |
| JM14 | Gêneros nulos | Arquivo recusado com dados originais preservados | Confirmado pelas asserções: Arquivo recusado com dados originais preservados | Automatizado | Aprovado |
| JM15 | Simulado sem identificação explícita | Carga recusada sem apresentar dados fictícios como reais | Confirmado pelas asserções: Carga recusada sem apresentar dados fictícios como reais | Automatizado | Aprovado |
| JM16 | ID simulado em campo IGDB | Conflito recusado sem conversão de identificadores | Confirmado pelas asserções: Conflito recusado sem conversão de identificadores | Automatizado | Aprovado |
| JM17 | Referência externa incompleta | Arquivo preservado e gravação bloqueada | Confirmado pelas asserções: Arquivo preservado e gravação bloqueada | Automatizado | Aprovado |
| JM18 | Game com progresso e ID IGDB | ID continua no campo correto e bloco concorda com ele | Confirmado pelas asserções: ID continua no campo correto e bloco concorda com ele | Automatizado | Aprovado |
| JM19 | Jogo com IDs conflitantes | Arquivo recusado sem escolher um identificador | Confirmado pelas asserções: Arquivo recusado sem escolher um identificador | Automatizado | Aprovado |
| JM20 | Falha de backup com coleção multimídia | Arquivo anterior preservado e memória não confirmada | Confirmado pelas asserções: Arquivo anterior preservado e memória não confirmada | Automatizado | Aprovado |
| JM21 | Extras legados desconhecidos e bloco novo | Extras raiz preservados junto do progresso | Confirmado pelas asserções: Extras raiz preservados junto do progresso | Automatizado | Aprovado |
| JM22 | Alteração externa após carga multimídia | Versão externa não sobrescrita | Confirmado pelas asserções: Versão externa não sobrescrita | Automatizado | Aprovado |
| JM23 | Dois registros simulados iguais | Nenhuma deduplicação no arquivo | Confirmado pelas asserções: Nenhuma deduplicação no arquivo | Automatizado | Aprovado |
| JM24 | Editar dados pessoais de simulado | Origem, ID e referência de capa mantidos | Confirmado pelas asserções: Origem, ID e referência de capa mantidos | Automatizado | Aprovado |
| JM25 | Campo opcional de progresso vazio | Não cria bloco adicional em jogo legado | Confirmado pelas asserções: Não cria bloco adicional em jogo legado | Automatizado | Aprovado |
| JM26 | Hora fracionária | Precisão decimal preservada após recarga | Confirmado pelas asserções: Precisão decimal preservada após recarga | Automatizado | Aprovado |
| JM27 | Campo inteiro fracionário no arquivo | JSON recusado sem arredondar página | Confirmado pelas asserções: JSON recusado sem arredondar página | Automatizado | Aprovado |
| JM28 | Lote com progresso inválido | Nenhum registro é gravado parcialmente | Confirmado pelas asserções: Nenhum registro é gravado parcialmente | Automatizado | Aprovado |
| JM29 | Recarga de simulado | Gêneros e capa persistem junto da origem fictícia | Confirmado pelas asserções: Gêneros e capa persistem junto da origem fictícia | Automatizado | Aprovado |
| JM30 | Jogo legado com progresso novo | SteamAppId original mantido sem preencher IgdbId | Confirmado pelas asserções: SteamAppId original mantido sem preencher IgdbId | Automatizado | Aprovado |

## Cadastro e edição multimídia pelo formulário — 20 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| FM01 | Cadastro de anime | Catálogo fictício explicitamente identificado | Confirmado pelas asserções: Catálogo fictício explicitamente identificado | Automatizado | Aprovado |
| FM02 | Cadastro de série | Tipo da seleção mantido | Confirmado pelas asserções: Tipo da seleção mantido | Automatizado | Aprovado |
| FM03 | Cadastro de filme | Referência simulada não vira ID de jogo | Confirmado pelas asserções: Referência simulada não vira ID de jogo | Automatizado | Aprovado |
| FM04 | Cadastro de mangá | Gêneros mantidos no catálogo | Confirmado pelas asserções: Gêneros mantidos no catálogo | Automatizado | Aprovado |
| FM05 | Cadastro de livro | Progresso de página opcional aceito | Confirmado pelas asserções: Progresso de página opcional aceito | Automatizado | Aprovado |
| FM06 | Seleção de outra categoria | Cadastro recusado sem trocar tipo implicitamente | Confirmado pelas asserções: Cadastro recusado sem trocar tipo implicitamente | Automatizado | Aprovado |
| FM07 | Novo tipo com origem simulada sem flag | Dados fictícios não podem ser apresentados como reais | Confirmado pelas asserções: Dados fictícios não podem ser apresentados como reais | Automatizado | Aprovado |
| FM08 | Simulado com origem incoerente | Seleção recusada | Confirmado pelas asserções: Seleção recusada | Automatizado | Aprovado |
| FM09 | Tipo desconhecido no formulário | Validação explícita antes de salvar | Confirmado pelas asserções: Validação explícita antes de salvar | Automatizado | Aprovado |
| FM10 | Texto alterado após escolher simulado | Seleção antiga recusada | Confirmado pelas asserções: Seleção antiga recusada | Automatizado | Aprovado |
| FM11 | Editar livro sem nova pesquisa | Catálogo e progresso escolhido preservados | Confirmado pelas asserções: Catálogo e progresso escolhido preservados | Automatizado | Aprovado |
| FM12 | Mudar tipo de item em edição | Operação recusada sem reclassificar registro | Confirmado pelas asserções: Operação recusada sem reclassificar registro | Automatizado | Aprovado |
| FM13 | Progresso negativo no cadastro | Aviso específico de progresso inválido | Confirmado pelas asserções: Aviso específico de progresso inválido | Automatizado | Aprovado |
| FM14 | Horas em filme | Unidade incompatível recusada | Confirmado pelas asserções: Unidade incompatível recusada | Automatizado | Aprovado |
| FM15 | Campo vazio de progresso | Null mantido sem preencher duração estimada | Confirmado pelas asserções: Null mantido sem preencher duração estimada | Automatizado | Aprovado |
| FM16 | Progresso zero informado | Zero mantido como informação pessoal | Confirmado pelas asserções: Zero mantido como informação pessoal | Automatizado | Aprovado |
| FM17 | Episódio sem temporada | Campo complementar continua opcional | Confirmado pelas asserções: Campo complementar continua opcional | Automatizado | Aprovado |
| FM18 | Volume sem capítulo | Campo complementar continua opcional | Confirmado pelas asserções: Campo complementar continua opcional | Automatizado | Aprovado |
| FM19 | Filme com minutos fracionários | Precisão preservada | Confirmado pelas asserções: Precisão preservada | Automatizado | Aprovado |
| FM20 | Jogo IGDB e horas pessoais | Referência IGDB permanece independente do progresso | Confirmado pelas asserções: Referência IGDB permanece independente do progresso | Automatizado | Aprovado |

## Roteamento IGDB e catálogos locais — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| RT01 | Jogos sem credenciais | Roteador mantém pesquisa de jogos indisponível | Confirmado pelas asserções: Roteador mantém pesquisa de jogos indisponível | Automatizado | Aprovado |
| RT02 | Jogos com serviço fornecido | Mesma instância IGDB é preservada | Confirmado pelas asserções: Mesma instância IGDB é preservada | Automatizado | Aprovado |
| RT03 | Rota de anime sem credenciais | Catálogo local disponível | Confirmado pelas asserções: Catálogo local disponível | Automatizado | Aprovado |
| RT04 | Rota de série sem credenciais | Catálogo local disponível | Confirmado pelas asserções: Catálogo local disponível | Automatizado | Aprovado |
| RT05 | Rota de filme sem credenciais | Catálogo local disponível | Confirmado pelas asserções: Catálogo local disponível | Automatizado | Aprovado |
| RT06 | Rota de mangá sem credenciais | Catálogo local disponível | Confirmado pelas asserções: Catálogo local disponível | Automatizado | Aprovado |
| RT07 | Rota de livro sem credenciais | Catálogo local disponível | Confirmado pelas asserções: Catálogo local disponível | Automatizado | Aprovado |
| RT08 | Tipo desconhecido no roteador | Tipo recusado em vez de usar categoria errada | Confirmado pelas asserções: Tipo recusado em vez de usar categoria errada | Automatizado | Aprovado |
| RT09 | Consultar cinco mídias simuladas | Nenhuma chamada feita ao serviço de jogos | Confirmado pelas asserções: Nenhuma chamada feita ao serviço de jogos | Automatizado | Aprovado |
| RT10 | Instanciar catálogo simulado de jogos | Recusa evita substituir a IGDB por dados falsos | Confirmado pelas asserções: Recusa evita substituir a IGDB por dados falsos | Automatizado | Aprovado |
| RT11 | Alternar rotas repetidamente | Serviço de jogos e catálogos locais mantêm instâncias | Confirmado pelas asserções: Serviço de jogos e catálogos locais mantêm instâncias | Automatizado | Aprovado |
| RT12 | Referências de capas simuladas | Recursos locais existem sem acesso à rede | Confirmado pelas asserções: Recursos locais existem sem acesso à rede | Automatizado | Aprovado |

## Fluxos das seis categorias na interface — 28 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| UM01 | Anime pela interface sem credenciais | Cadastro e episódios persistem com aviso de simulação | Confirmado pelas asserções: Cadastro e episódios persistem com aviso de simulação | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM02 | Série pela interface sem credenciais | Cadastro e temporada persistem | Confirmado pelas asserções: Cadastro e temporada persistem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM03 | Filme pela interface sem credenciais | Cadastro e minutos persistem | Confirmado pelas asserções: Cadastro e minutos persistem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM04 | Mangá pela interface sem credenciais | Cadastro e capítulo persistem | Confirmado pelas asserções: Cadastro e capítulo persistem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM05 | Livro pela interface sem credenciais | Cadastro e página persistem | Confirmado pelas asserções: Cadastro e página persistem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM06 | Biblioteca mista | Cada categoria exibe somente sua mídia | Confirmado pelas asserções: Cada categoria exibe somente sua mídia | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM07 | Trocar tipo durante espera da busca | Sugestões da categoria antiga desaparecem | Confirmado pelas asserções: Sugestões da categoria antiga desaparecem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM08 | Trocar tipo após seleção | Seleção antiga não pode cadastrar em outra categoria | Confirmado pelas asserções: Seleção antiga não pode cadastrar em outra categoria | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM09 | Cadastro simulado sem progresso | Nenhum zero automático é gravado | Confirmado pelas asserções: Nenhum zero automático é gravado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM10 | Campo de posição recebe fração | Último inteiro válido é conservado antes do salvamento | Confirmado pelas asserções: Último inteiro válido é conservado antes do salvamento | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM11 | Editar livro e reiniciar | Página e origem simulada permanecem | Confirmado pelas asserções: Página e origem simulada permanecem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM12 | Remover anime em coleção mista | Livro e jogo permanecem intactos | Confirmado pelas asserções: Livro e jogo permanecem intactos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM13 | Falha de gravação em cadastro de livro | Coleção e formulário preservados sem falso sucesso | Confirmado pelas asserções: Coleção e formulário preservados sem falso sucesso | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM14 | Progresso de jogo legado | SteamAppId e ausência de IgdbId preservados ao editar horas | Confirmado pelas asserções: SteamAppId e ausência de IgdbId preservados ao editar horas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM15 | Alternar tipos do formulário | Campos de progresso anteriores limpos e painéis adequados | Confirmado pelas asserções: Campos de progresso anteriores limpos e painéis adequados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM16 | Livro usa texto de status apropriado | Rótulo Lendo mantém estado interno Jogando | Confirmado pelas asserções: Rótulo Lendo mantém estado interno Jogando | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM17 | Filtrar status em categoria livro | Rótulos adaptados filtram pelo estado interno correto | Confirmado pelas asserções: Rótulos adaptados filtram pelo estado interno correto | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM18 | Abrir categoria sem obras | Estado vazio claro sem excluir outras mídias | Confirmado pelas asserções: Estado vazio claro sem excluir outras mídias | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM19 | Novo cadastro após editar outra mídia | Tipo volta a ser selecionável | Confirmado pelas asserções: Tipo volta a ser selecionável | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM20 | Capturar telas de seis mídias | Categorias, avisos e progresso renderizam no tamanho mínimo | Confirmado pelas asserções: Categorias, avisos e progresso renderizam no tamanho mínimo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM21 | Série com temporada e episódio | Ambos os campos opcionais persistem | Confirmado pelas asserções: Ambos os campos opcionais persistem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM22 | Mangá com capítulo e volume positivos | Campos de posição preservados a partir de um | Confirmado pelas asserções: Campos de posição preservados a partir de um | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM23 | Livro com capítulo sem página | Campo opcional vazio permanece nulo | Confirmado pelas asserções: Campo opcional vazio permanece nulo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM24 | Aviso de catálogo simulado | Cinco mídias têm indicação clara e jogos mantêm IGDB | Confirmado pelas asserções: Cinco mídias têm indicação clara e jogos mantêm IGDB | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM25 | Limpar busca simulada antes de concluir | Sugestões não reaparecem após limpar | Confirmado pelas asserções: Sugestões não reaparecem após limpar | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM26 | Falha e nova tentativa de cadastro multimídia | Retomada salva uma vez sem duplicar o registro | Confirmado pelas asserções: Retomada salva uma vez sem duplicar o registro | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM27 | Selecionar mídia simulada sem credenciais IGDB | Aviso de jogos indisponíveis não contradiz o catálogo local | Confirmado pelas asserções: Aviso de jogos indisponíveis não contradiz o catálogo local | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UM28 | Trocar para livro mantendo primeiro status | Campo fechado mostra Quero ler sem conservar texto antigo | Confirmado pelas asserções: Campo fechado mostra Quero ler sem conservar texto antigo | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Categorias e status adaptados na biblioteca — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| MT01 | Filtrar jogos em coleção mista | Somente jogos retornados | Confirmado pelas asserções: Somente jogos retornados | Automatizado | Aprovado |
| MT02 | Filtrar animes em coleção mista | Somente animes retornados | Confirmado pelas asserções: Somente animes retornados | Automatizado | Aprovado |
| MT03 | Filtrar séries em coleção mista | Somente séries retornadas | Confirmado pelas asserções: Somente séries retornadas | Automatizado | Aprovado |
| MT04 | Filtrar filmes em coleção mista | Somente filmes retornados | Confirmado pelas asserções: Somente filmes retornados | Automatizado | Aprovado |
| MT05 | Filtrar mangás em coleção mista | Somente mangás retornados | Confirmado pelas asserções: Somente mangás retornados | Automatizado | Aprovado |
| MT06 | Filtrar livros em coleção mista | Somente livros retornados | Confirmado pelas asserções: Somente livros retornados | Automatizado | Aprovado |
| MT07 | Tipo combinado com texto e status | Todos os critérios aplicados sem alterar coleção | Confirmado pelas asserções: Todos os critérios aplicados sem alterar coleção | Automatizado | Aprovado |
| MT08 | Consulta sem categoria | Compatibilidade permite consultar todos os registros | Confirmado pelas asserções: Compatibilidade permite consultar todos os registros | Automatizado | Aprovado |
| MT09 | Status de jogo legado | Textos originais continuam inalterados | Confirmado pelas asserções: Textos originais continuam inalterados | Automatizado | Aprovado |
| MT10 | Status de mídias assistidas | Rótulos adaptados sem converter valores internos | Confirmado pelas asserções: Rótulos adaptados sem converter valores internos | Automatizado | Aprovado |
| MT11 | Status de mídias lidas | Rótulos adequados e status legados desconhecidos preservados | Confirmado pelas asserções: Rótulos adequados e status legados desconhecidos preservados | Automatizado | Aprovado |
| MT12 | Resumo de coleção mista | Estatísticas usam registros pessoais sem inventar atividade | Confirmado pelas asserções: Estatísticas usam registros pessoais sem inventar atividade | Automatizado | Aprovado |

## Consultas e filtros combinados da biblioteca — 34 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| QB01 | Consulta vazia | Todos os itens permanecem na ordem original | Confirmado pelas asserções: Todos os itens permanecem na ordem original | Automatizado | Aprovado |
| QB02 | Gênero do catálogo | Somente obras com o gênero escolhido | Confirmado pelas asserções: Somente obras com o gênero escolhido | Automatizado | Aprovado |
| QB03 | Gênero sem diferença de caixa | rpg encontra RPG | Confirmado pelas asserções: rpg encontra RPG | Automatizado | Aprovado |
| QB04 | Gênero não encontrado | Resultado vazio sem alterar coleção | Confirmado pelas asserções: Resultado vazio sem alterar coleção | Automatizado | Aprovado |
| QB05 | Legado sem gêneros | Categoria pessoal serve de alternativa | Confirmado pelas asserções: Categoria pessoal serve de alternativa | Automatizado | Aprovado |
| QB06 | Catálogo com gêneros | Categoria pessoal não substitui gênero disponível | Confirmado pelas asserções: Categoria pessoal não substitui gênero disponível | Automatizado | Aprovado |
| QB07 | Obra com múltiplos gêneros | Segundo gênero também pode ser filtrado | Confirmado pelas asserções: Segundo gênero também pode ser filtrado | Automatizado | Aprovado |
| QB08 | Plataforma de jogo | PC encontra apenas os jogos correspondentes | Confirmado pelas asserções: PC encontra apenas os jogos correspondentes | Automatizado | Aprovado |
| QB09 | Plataforma sem diferença de caixa | pc encontra PC | Confirmado pelas asserções: pc encontra PC | Automatizado | Aprovado |
| QB10 | Plataforma desconhecida no legado | Legado não ganha plataforma inventada | Confirmado pelas asserções: Legado não ganha plataforma inventada | Automatizado | Aprovado |
| QB11 | Plataformas múltiplas | Segunda plataforma é pesquisável | Confirmado pelas asserções: Segunda plataforma é pesquisável | Automatizado | Aprovado |
| QB12 | Plataforma e gênero combinados | Interseção correta | Confirmado pelas asserções: Interseção correta | Automatizado | Aprovado |
| QB13 | Nota mínima inclusiva | Nota exatamente no limite permanece | Confirmado pelas asserções: Nota exatamente no limite permanece | Automatizado | Aprovado |
| QB14 | Nota mínima zero | Nenhum item é excluído | Confirmado pelas asserções: Nenhum item é excluído | Automatizado | Aprovado |
| QB15 | Nota mínima cinco | Somente a nota máxima | Confirmado pelas asserções: Somente a nota máxima | Automatizado | Aprovado |
| QB16 | Nota fracionária | Comparação preserva casas decimais | Confirmado pelas asserções: Comparação preserva casas decimais | Automatizado | Aprovado |
| QB17 | Nota negativa | Filtro rejeitado sem alterar dados | Confirmado pelas asserções: Filtro rejeitado sem alterar dados | Automatizado | Aprovado |
| QB18 | Nota acima do máximo | Filtro rejeitado | Confirmado pelas asserções: Filtro rejeitado | Automatizado | Aprovado |
| QB19 | Nota não numérica | NaN rejeitado | Confirmado pelas asserções: NaN rejeitado | Automatizado | Aprovado |
| QB20 | Data inicial inclusiva | Apenas registros com data conhecida no intervalo | Confirmado pelas asserções: Apenas registros com data conhecida no intervalo | Automatizado | Aprovado |
| QB21 | Data final inclusiva | Registro do dia final permanece | Confirmado pelas asserções: Registro do dia final permanece | Automatizado | Aprovado |
| QB22 | Intervalo de um dia | Horário não exclui item do mesmo dia | Confirmado pelas asserções: Horário não exclui item do mesmo dia | Automatizado | Aprovado |
| QB23 | Intervalo invertido | Filtro rejeitado sem perda de itens | Confirmado pelas asserções: Filtro rejeitado sem perda de itens | Automatizado | Aprovado |
| QB24 | Sem filtro de data | Registros sem data continuam visíveis | Confirmado pelas asserções: Registros sem data continuam visíveis | Automatizado | Aprovado |
| QB25 | Intervalo sem correspondência | Lista vazia sem exceção | Confirmado pelas asserções: Lista vazia sem exceção | Automatizado | Aprovado |
| QB26 | Busca junto de novos filtros | Termo, gênero e nota combinam | Confirmado pelas asserções: Termo, gênero e nota combinam | Automatizado | Aprovado |
| QB27 | Status junto de novos filtros | Status incompatível retorna vazio | Confirmado pelas asserções: Status incompatível retorna vazio | Automatizado | Aprovado |
| QB28 | Consulta com mídia diferente | Nenhum jogo aparece como livro | Confirmado pelas asserções: Nenhum jogo aparece como livro | Automatizado | Aprovado |
| QB29 | Filtros não gravam | Consulta deixa persistência e instâncias intactas | Confirmado pelas asserções: Consulta deixa persistência e instâncias intactas | Automatizado | Aprovado |
| QB30 | Consulta nula | Argumento rejeitado claramente | Confirmado pelas asserções: Argumento rejeitado claramente | Automatizado | Aprovado |
| QB31 | Coleção vazia | Filtros válidos retornam vazio | Confirmado pelas asserções: Filtros válidos retornam vazio | Automatizado | Aprovado |
| QB32 | Limpar filtros | Todos os registros voltam à consulta | Confirmado pelas asserções: Todos os registros voltam à consulta | Automatizado | Aprovado |
| QB33 | Data em fuso diferente | Filtro usa o dia local da adição | Confirmado pelas asserções: Filtro usa o dia local da adição | Automatizado | Aprovado |
| QB34 | Tipo enum desconhecido | Consulta inválida rejeitada | Confirmado pelas asserções: Consulta inválida rejeitada | Automatizado | Aprovado |

## Ordenação estável da biblioteca — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| OB01 | Ordenação A a Z | Títulos em ordem alfabética | Confirmado pelas asserções: Títulos em ordem alfabética | Automatizado | Aprovado |
| OB02 | Ordenação Z a A | Títulos em ordem inversa | Confirmado pelas asserções: Títulos em ordem inversa | Automatizado | Aprovado |
| OB03 | Maior nota primeiro | Ordem 5, 4, 2 | Confirmado pelas asserções: Ordem 5, 4, 2 | Automatizado | Aprovado |
| OB04 | Menor nota primeiro | Ordem 2, 4, 5 | Confirmado pelas asserções: Ordem 2, 4, 5 | Automatizado | Aprovado |
| OB05 | Adição recente primeiro | Datas conhecidas decrescentes e desconhecidas ao final | Confirmado pelas asserções: Datas conhecidas decrescentes e desconhecidas ao final | Automatizado | Aprovado |
| OB06 | Adição antiga primeiro | Datas conhecidas crescentes e desconhecidas ao final | Confirmado pelas asserções: Datas conhecidas crescentes e desconhecidas ao final | Automatizado | Aprovado |
| OB07 | Voltar à ordem de cadastro | Ordenar não altera coleção original | Confirmado pelas asserções: Ordenar não altera coleção original | Automatizado | Aprovado |
| OB08 | Empate de nota | Ordem relativa das instâncias permanece | Confirmado pelas asserções: Ordem relativa das instâncias permanece | Automatizado | Aprovado |
| OB09 | Duplicatas iguais | Ordenação não deduplica registros | Confirmado pelas asserções: Ordenação não deduplica registros | Automatizado | Aprovado |
| OB10 | Ordenação inválida | Enum desconhecido é rejeitado | Confirmado pelas asserções: Enum desconhecido é rejeitado | Automatizado | Aprovado |
| OB11 | Ordenar resultado filtrado | Filtro de gênero é respeitado | Confirmado pelas asserções: Filtro de gênero é respeitado | Automatizado | Aprovado |
| OB12 | Todos sem data | Ordenação por data mantém ordem relativa | Confirmado pelas asserções: Ordenação por data mantém ordem relativa | Automatizado | Aprovado |

## Datas, plataformas e compatibilidade JSON — 20 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| DB01 | Cadastro novo | Data de adição real em UTC é registrada | Confirmado pelas asserções: Data de adição real em UTC é registrada | Automatizado | Aprovado |
| DB02 | Data salva e recarregada | Instante e offset preservados | Confirmado pelas asserções: Instante e offset preservados | Automatizado | Aprovado |
| DB03 | JSON legado | Data permanece desconhecida | Confirmado pelas asserções: Data permanece desconhecida | Automatizado | Aprovado |
| DB04 | Formato da Fase 2 sem data | Nenhuma data inventada no ciclo JSON | Confirmado pelas asserções: Nenhuma data inventada no ciclo JSON | Automatizado | Aprovado |
| DB05 | Edição de legado | Editar nota não cria data de adição | Confirmado pelas asserções: Editar nota não cria data de adição | Automatizado | Aprovado |
| DB06 | Edição de registro datado | Data original permanece | Confirmado pelas asserções: Data original permanece | Automatizado | Aprovado |
| DB07 | Atualização pelo serviço | Data e catálogo preservados | Confirmado pelas asserções: Data e catálogo preservados | Automatizado | Aprovado |
| DB08 | Cadastro das cinco mídias simuladas | Todas recebem data sem mudar origem | Confirmado pelas asserções: Todas recebem data sem mudar origem | Automatizado | Aprovado |
| DB09 | Data inválida no JSON | Carregamento bloqueia gravação e preserva bytes | Confirmado pelas asserções: Carregamento bloqueia gravação e preserva bytes | Automatizado | Aprovado |
| DB10 | Steam e extras com nova data | Identificador opaco e extensão permanecem | Confirmado pelas asserções: Identificador opaco e extensão permanecem | Automatizado | Aprovado |
| DB11 | Plataformas em JSON | Todas as plataformas do catálogo persistem | Confirmado pelas asserções: Todas as plataformas do catálogo persistem | Automatizado | Aprovado |
| DB12 | Plataforma ausente | Lista vazia sem propriedade nova no legado | Confirmado pelas asserções: Lista vazia sem propriedade nova no legado | Automatizado | Aprovado |
| DB13 | Plataforma vazia | Gravação recusa metadado inválido | Confirmado pelas asserções: Gravação recusa metadado inválido | Automatizado | Aprovado |
| DB14 | Plataforma nula no modelo | Gravação recusa contrato inválido | Confirmado pelas asserções: Gravação recusa contrato inválido | Automatizado | Aprovado |
| DB15 | Plataforma em livro | Não aceita plataforma de jogo em outra mídia | Confirmado pelas asserções: Não aceita plataforma de jogo em outra mídia | Automatizado | Aprovado |
| DB16 | Edição de jogo com plataforma | Plataforma e capa não são perdidas | Confirmado pelas asserções: Plataforma e capa não são perdidas | Automatizado | Aprovado |
| DB17 | Mistura de formatos | Legado, multimídia e novos metadados coexistem | Confirmado pelas asserções: Legado, multimídia e novos metadados coexistem | Automatizado | Aprovado |
| DB18 | Falha de persistência com data | Memória anterior permanece intacta | Confirmado pelas asserções: Memória anterior permanece intacta | Automatizado | Aprovado |
| DB19 | Array de plataforma inválido no disco | Leitura recusa e original permanece | Confirmado pelas asserções: Leitura recusa e original permanece | Automatizado | Aprovado |
| DB20 | Metadados só de plataforma | Bloco opcional é criado sem alterar IgdbId | Confirmado pelas asserções: Bloco opcional é criado sem alterar IgdbId | Automatizado | Aprovado |

## Preferência independente de visualização — 16 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| PV01 | Preferência inexistente | Grade é o padrão e nenhum arquivo é criado | Confirmado pelas asserções: Grade é o padrão e nenhum arquivo é criado | Automatizado | Aprovado |
| PV02 | Salvar lista e reiniciar | Lista é recuperada do arquivo separado | Confirmado pelas asserções: Lista é recuperada do arquivo separado | Automatizado | Aprovado |
| PV03 | Salvar grade após lista | Preferência atualizada sem temporários restantes | Confirmado pelas asserções: Preferência atualizada sem temporários restantes | Automatizado | Aprovado |
| PV04 | JSON truncado | Erro controlado e conteúdo original preservado | Confirmado pelas asserções: Erro controlado e conteúdo original preservado | Automatizado | Aprovado |
| PV05 | Versão desconhecida | Preferência recusada sem sobrescrita | Confirmado pelas asserções: Preferência recusada sem sobrescrita | Automatizado | Aprovado |
| PV06 | Enum inválido no disco | Preferência recusada | Confirmado pelas asserções: Preferência recusada | Automatizado | Aprovado |
| PV07 | Campos obrigatórios ausentes | Leitura recusa preferência incompleta | Confirmado pelas asserções: Leitura recusa preferência incompleta | Automatizado | Aprovado |
| PV08 | Arquivo contendo null | Preferência recusada | Confirmado pelas asserções: Preferência recusada | Automatizado | Aprovado |
| PV09 | Enum inválido ao salvar | Nenhum arquivo é criado | Confirmado pelas asserções: Nenhum arquivo é criado | Automatizado | Aprovado |
| PV10 | Falha real no destino | Preferência anterior permanece e temporário é removido | Confirmado pelas asserções: Preferência anterior permanece e temporário é removido | Automatizado | Aprovado |
| PV11 | Recuperar após falha | Nova gravação funciona quando o destino é corrigido | Confirmado pelas asserções: Nova gravação funciona quando o destino é corrigido | Automatizado | Aprovado |
| PV12 | Mudar preferência com biblioteca presente | Bytes da biblioteca não são tocados | Confirmado pelas asserções: Bytes da biblioteca não são tocados | Automatizado | Aprovado |
| PV13 | Armazenamento em memória | Janelas de teste não escrevem no repositório | Confirmado pelas asserções: Janelas de teste não escrevem no repositório | Automatizado | Aprovado |
| PV14 | Caminho relativo explícito | Caminho é documentável como absoluto | Confirmado pelas asserções: Caminho é documentável como absoluto | Automatizado | Aprovado |
| PV15 | Preferência inválida substituída explicitamente | Escolha válida pode recuperar o arquivo | Confirmado pelas asserções: Escolha válida pode recuperar o arquivo | Automatizado | Aprovado |
| PV16 | Composição com biblioteca de mesmo nome da preferência | Caminhos diferentes impedem sobrescrita de dados | Confirmado pelas asserções: Caminhos diferentes impedem sobrescrita de dados | Automatizado | Aprovado |

## Metadados opcionais da IGDB — 15 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| CI01 | IGDB com image_id | Referência HTTPS de capa grande é construída | Confirmado pelas asserções: Referência HTTPS de capa grande é construída | Automatizado | Aprovado |
| CI02 | IGDB sem capa | Jogo continua disponível com capa ausente | Confirmado pelas asserções: Jogo continua disponível com capa ausente | Automatizado | Aprovado |
| CI03 | Capa explicitamente null | Compatibilidade com catálogo sem imagem | Confirmado pelas asserções: Compatibilidade com catálogo sem imagem | Automatizado | Aprovado |
| CI04 | Capa com URL relativa ao protocolo | URL é normalizada para HTTPS | Confirmado pelas asserções: URL é normalizada para HTTPS | Automatizado | Aprovado |
| CI05 | image_id com tentativa de caminho | ID inseguro não vira URL | Confirmado pelas asserções: ID inseguro não vira URL | Automatizado | Aprovado |
| CI06 | URL de capa fora do CDN | Referência não autorizada é ignorada | Confirmado pelas asserções: Referência não autorizada é ignorada | Automatizado | Aprovado |
| CI07 | Gêneros múltiplos | Nomes são separados do gênero pessoal | Confirmado pelas asserções: Nomes são separados do gênero pessoal | Automatizado | Aprovado |
| CI08 | Plataformas múltiplas | Nomes das plataformas são mantidos | Confirmado pelas asserções: Nomes das plataformas são mantidos | Automatizado | Aprovado |
| CI09 | Nomes incompletos ou repetidos | Nulos e vazios ignorados e repetidos removidos | Confirmado pelas asserções: Nulos e vazios ignorados e repetidos removidos | Automatizado | Aprovado |
| CI10 | Resposta antiga id e name | Novos campos opcionais não são exigidos | Confirmado pelas asserções: Novos campos opcionais não são exigidos | Automatizado | Aprovado |
| CI11 | Plataformas e gêneros null | Coleções vazias em vez de nulas | Confirmado pelas asserções: Coleções vazias em vez de nulas | Automatizado | Aprovado |
| CI12 | image_id e URL simultâneos | image_id válido tem prioridade | Confirmado pelas asserções: image_id válido tem prioridade | Automatizado | Aprovado |
| CI13 | Consulta expandida | POST, endpoint, headers e limite continuam corretos | Confirmado pelas asserções: POST, endpoint, headers e limite continuam corretos | Automatizado | Aprovado |
| CI14 | Cancelamento antes da busca | Nenhum pedido HTTP é enviado | Confirmado pelas asserções: Nenhum pedido HTTP é enviado | Automatizado | Aprovado |
| CI15 | Metadados completos persistidos | Capa, gêneros e plataforma sobrevivem ao reinício | Confirmado pelas asserções: Capa, gêneros e plataforma sobrevivem ao reinício | Automatizado | Aprovado |

## Capas: carregamento, cache, limites e falhas — 35 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| CP01 | Capa nula | Imagem padrão sem HTTP | Confirmado pelas asserções: Imagem padrão sem HTTP | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP02 | Capa vazia | Imagem padrão sem HTTP | Confirmado pelas asserções: Imagem padrão sem HTTP | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP03 | URL malformada | Imagem padrão sem exceção | Confirmado pelas asserções: Imagem padrão sem exceção | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP04 | HTTP sem TLS | URL recusada | Confirmado pelas asserções: URL recusada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP05 | Caminho pessoal local | Arquivo não é aberto como capa | Confirmado pelas asserções: Arquivo não é aberto como capa | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP06 | Host parecido com CDN | Domínio falso recusado | Confirmado pelas asserções: Domínio falso recusado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP07 | Porta alternativa | URL recusada sem rede | Confirmado pelas asserções: URL recusada sem rede | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP08 | Credenciais em URL | Userinfo recusado | Confirmado pelas asserções: Userinfo recusado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP09 | Caminho remoto fora das imagens | Recurso recusado | Confirmado pelas asserções: Recurso recusado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP10 | Recurso local desconhecido | Sem tentativa de arquivo ou HTTP | Confirmado pelas asserções: Sem tentativa de arquivo ou HTTP | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP11 | Cinco capas simuladas | Desenhos locais válidos e nenhuma rede | Confirmado pelas asserções: Desenhos locais válidos e nenhuma rede | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP12 | Imagem remota válida | Bitmap decodificado com largura limitada | Confirmado pelas asserções: Bitmap decodificado com largura limitada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP13 | Repetir a mesma capa | Instância reutilizada e um único download | Confirmado pelas asserções: Instância reutilizada e um único download | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP14 | Pedidos simultâneos da mesma capa | Download compartilhado pelos consumidores | Confirmado pelas asserções: Download compartilhado pelos consumidores | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP15 | Capa inexistente 404 | Imagem padrão sem encerramento | Confirmado pelas asserções: Imagem padrão sem encerramento | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP16 | Servidor de imagens 500 | Falha isolada do catálogo | Confirmado pelas asserções: Falha isolada do catálogo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP17 | Bytes não representam imagem | Conteúdo inválido vira imagem padrão | Confirmado pelas asserções: Conteúdo inválido vira imagem padrão | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP18 | Content-Length acima do limite | Imagem grande é recusada | Confirmado pelas asserções: Imagem grande é recusada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP19 | Fluxo sem tamanho supera limite | Leitura é interrompida antes da decodificação | Confirmado pelas asserções: Leitura é interrompida antes da decodificação | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP20 | Falha de conexão da capa | Biblioteca pode continuar sem imagem | Confirmado pelas asserções: Biblioteca pode continuar sem imagem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP21 | Timeout da imagem | Resultado ausente sem exceção não tratada | Confirmado pelas asserções: Resultado ausente sem exceção não tratada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP22 | Cancelamento antes de carregar | Nenhum pedido é enviado | Confirmado pelas asserções: Nenhum pedido é enviado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP23 | Um cartão cancela pedido compartilhado | Outro consumidor ainda recebe a capa | Confirmado pelas asserções: Outro consumidor ainda recebe a capa | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP24 | Encerrar com pedido em andamento | Pedido é cancelado e carregador não aceita novos downloads | Confirmado pelas asserções: Pedido é cancelado e carregador não aceita novos downloads | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP25 | URL sem protocolo | Pedido normalizado para HTTPS | Confirmado pelas asserções: Pedido normalizado para HTTPS | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP26 | Redirecionamento do CDN | Resposta não é tratada como imagem | Confirmado pelas asserções: Resposta não é tratada como imagem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP27 | Download de imagem | Nenhuma credencial IGDB é enviada | Confirmado pelas asserções: Nenhuma credencial IGDB é enviada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP28 | Muitas imagens ao mesmo tempo | No máximo quatro downloads simultâneos | Confirmado pelas asserções: No máximo quatro downloads simultâneos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP29 | Mesma URL com e sem protocolo | Cache normalizado evita download duplicado | Confirmado pelas asserções: Cache normalizado evita download duplicado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP30 | Falha de uma capa seguida de outra válida | Erro não bloqueia novos endereços | Confirmado pelas asserções: Erro não bloqueia novos endereços | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP31 | Endereços diferentes | Cache não confunde capas distintas | Confirmado pelas asserções: Cache não confunde capas distintas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP32 | Cache excede 128 endereços | Entradas concluídas antigas são substituídas com segurança | Confirmado pelas asserções: Entradas concluídas antigas são substituídas com segurança | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP33 | Headers recebidos mas corpo não termina | Prazo completo cancela a leitura da imagem | Confirmado pelas asserções: Prazo completo cancela a leitura da imagem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP34 | Servidor não envia headers | Prazo completo cancela o pedido HTTP | Confirmado pelas asserções: Prazo completo cancela o pedido HTTP | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CP35 | Prazo de carregamento inválido | Configuração rejeitada antes de criar pedidos | Confirmado pelas asserções: Configuração rejeitada antes de criar pedidos | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Operações e apresentação em grade — 13 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| VG01 | Painel de visualização em grade | Painel correto e seleção de modo aplicada | Confirmado pelas asserções: Painel correto e seleção de modo aplicada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG02 | Alternar com duplicata selecionada em grade | Mesma instância e índice permanecem selecionados | Confirmado pelas asserções: Mesma instância e índice permanecem selecionados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG03 | Adicionar jogo em grade | Cadastro persistido com data real e modo preservado | Confirmado pelas asserções: Cadastro persistido com data real e modo preservado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG04 | Adicionar livro simulado em grade | Progresso persiste sem HTTP real | Confirmado pelas asserções: Progresso persiste sem HTTP real | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG05 | Editar nota e review legadas em grade | Steam original preservado e IgdbId ausente | Confirmado pelas asserções: Steam original preservado e IgdbId ausente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG06 | Editar progresso de anime em grade | Episódios atualizados e persistidos | Confirmado pelas asserções: Episódios atualizados e persistidos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG07 | Remover segunda cópia igual em grade | Primeira instância permanece sem deduplicação | Confirmado pelas asserções: Primeira instância permanece sem deduplicação | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG08 | Filtrar e limpar em grade | Gênero e nota combinam e limpeza restaura itens | Confirmado pelas asserções: Gênero e nota combinam e limpeza restaura itens | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG09 | Ordenar títulos em grade | Ordem visual muda sem regravar biblioteca | Confirmado pelas asserções: Ordem visual muda sem regravar biblioteca | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG10 | Arquivo alterado externamente em grade | Gravação bloqueada e memória anterior preservada | Confirmado pelas asserções: Gravação bloqueada e memória anterior preservada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG11 | Biblioteca vazia em grade | Estado vazio e ações desabilitadas | Confirmado pelas asserções: Estado vazio e ações desabilitadas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG12 | Seis mídias no tamanho mínimo em grade | Telas renderizadas com lista utilizável e capas | Confirmado pelas asserções: Telas renderizadas com lista utilizável e capas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VG13 | Dados pessoais no cartão em grade | Status, nota, review e progresso renderizam | Confirmado pelas asserções: Status, nota, review e progresso renderizam | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Operações e apresentação em lista — 13 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| VL01 | Painel de visualização em lista | Painel correto e seleção de modo aplicada | Confirmado pelas asserções: Painel correto e seleção de modo aplicada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL02 | Alternar com duplicata selecionada em lista | Mesma instância e índice permanecem selecionados | Confirmado pelas asserções: Mesma instância e índice permanecem selecionados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL03 | Adicionar jogo em lista | Cadastro persistido com data real e modo preservado | Confirmado pelas asserções: Cadastro persistido com data real e modo preservado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL04 | Adicionar livro simulado em lista | Progresso persiste sem HTTP real | Confirmado pelas asserções: Progresso persiste sem HTTP real | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL05 | Editar nota e review legadas em lista | Steam original preservado e IgdbId ausente | Confirmado pelas asserções: Steam original preservado e IgdbId ausente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL06 | Editar progresso de anime em lista | Episódios atualizados e persistidos | Confirmado pelas asserções: Episódios atualizados e persistidos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL07 | Remover segunda cópia igual em lista | Primeira instância permanece sem deduplicação | Confirmado pelas asserções: Primeira instância permanece sem deduplicação | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL08 | Filtrar e limpar em lista | Gênero e nota combinam e limpeza restaura itens | Confirmado pelas asserções: Gênero e nota combinam e limpeza restaura itens | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL09 | Ordenar títulos em lista | Ordem visual muda sem regravar biblioteca | Confirmado pelas asserções: Ordem visual muda sem regravar biblioteca | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL10 | Arquivo alterado externamente em lista | Gravação bloqueada e memória anterior preservada | Confirmado pelas asserções: Gravação bloqueada e memória anterior preservada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL11 | Biblioteca vazia em lista | Estado vazio e ações desabilitadas | Confirmado pelas asserções: Estado vazio e ações desabilitadas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL12 | Seis mídias no tamanho mínimo em lista | Telas renderizadas com lista utilizável e capas | Confirmado pelas asserções: Telas renderizadas com lista utilizável e capas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VL13 | Dados pessoais no cartão em lista | Status, nota, review e progresso renderizam | Confirmado pelas asserções: Status, nota, review e progresso renderizam | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Interação entre filtros, preferências e capas — 22 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| IB01 | Preferência padrão na janela | Grade inicial sem criar preferência ou regravar biblioteca | Confirmado pelas asserções: Grade inicial sem criar preferência ou regravar biblioteca | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB02 | Preferência de lista ao abrir | Lista recuperada pela janela | Confirmado pelas asserções: Lista recuperada pela janela | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB03 | Alternar e reabrir janela | Preferência persiste sem tocar a biblioteca | Confirmado pelas asserções: Preferência persiste sem tocar a biblioteca | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB04 | Preferência corrompida na janela | Aviso, grade e biblioteca utilizável | Confirmado pelas asserções: Aviso, grade e biblioteca utilizável | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB05 | Falha ao guardar visualização | Modo muda na sessão e CRUD continua permitido | Confirmado pelas asserções: Modo muda na sessão e CRUD continua permitido | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB06 | Filtro de plataforma | Apenas jogo da plataforma selecionada aparece | Confirmado pelas asserções: Apenas jogo da plataforma selecionada aparece | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB07 | Legado sem plataforma | Controle desabilitado e registro permanece disponível | Confirmado pelas asserções: Controle desabilitado e registro permanece disponível | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB08 | Trocar para mídia sem plataforma | Opções antigas são limpas e plataforma desabilitada | Confirmado pelas asserções: Opções antigas são limpas e plataforma desabilitada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB09 | Filtro de data conhecido | Legado sem data é excluído somente com filtro ativo | Confirmado pelas asserções: Legado sem data é excluído somente com filtro ativo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB10 | Datas invertidas na janela | Aviso claro sem perder resultado e recuperação ao corrigir | Confirmado pelas asserções: Aviso claro sem perder resultado e recuperação ao corrigir | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB11 | Alternar com filtros ativos | Consulta e ordenação permanecem nas duas visualizações | Confirmado pelas asserções: Consulta e ordenação permanecem nas duas visualizações | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB12 | Ordenar com item selecionado | Mesma instância continua selecionada no novo índice | Confirmado pelas asserções: Mesma instância continua selecionada no novo índice | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB13 | Filtro oculta seleção | Ações são desabilitadas e registros não são removidos | Confirmado pelas asserções: Ações são desabilitadas e registros não são removidos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB14 | Filtros expandidos no tamanho mínimo | Controles e rodapé permanecem acessíveis | Confirmado pelas asserções: Controles e rodapé permanecem acessíveis | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB15 | Legado sem capa na grade | Imagem padrão renderizada com título preservado | Confirmado pelas asserções: Imagem padrão renderizada com título preservado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB16 | Capa de sugestão simulada | Desenho local carregado e aviso de simulação visível | Confirmado pelas asserções: Desenho local carregado e aviso de simulação visível | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB17 | Limpar preferência inválida pela janela | Troca deliberada salva nova escolha | Confirmado pelas asserções: Troca deliberada salva nova escolha | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB18 | Grade com várias obras | Cartões se distribuem em linhas sem rolagem horizontal | Confirmado pelas asserções: Cartões se distribuem em linhas sem rolagem horizontal | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB19 | Capa remota simulada no cartão | Bitmap aparece após carregamento assíncrono | Confirmado pelas asserções: Bitmap aparece após carregamento assíncrono | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB20 | Falha de capa no cartão | Imagem padrão e ações locais disponíveis | Confirmado pelas asserções: Imagem padrão e ações locais disponíveis | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB21 | Referência muda durante download | Resposta antiga não substitui a nova capa | Confirmado pelas asserções: Resposta antiga não substitui a nova capa | Automatizado (Avalonia Headless/Skia) | Aprovado |
| IB22 | Cartão removido durante download | Carga visual é cancelada sem falha na exclusão | Confirmado pelas asserções: Carga visual é cancelada sem falha na exclusão | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Cadastro de jogos, seleção IGDB e gêneros externos — 20 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| GC01 | Digitação sem seleção | Formulário recusa título arbitrário | Confirmado pelas asserções: Formulário recusa título arbitrário | Automatizado | Aprovado |
| GC02 | Seleção sem origem IGDB | Cadastro de jogo recusado | Confirmado pelas asserções: Cadastro de jogo recusado | Automatizado | Aprovado |
| GC03 | Seleção de catálogo simulado como jogo | Cadastro recusado | Confirmado pelas asserções: Cadastro recusado | Automatizado | Aprovado |
| GC04 | ID inválido na seleção | Cadastro recusado sem converter identificador | Confirmado pelas asserções: Cadastro recusado sem converter identificador | Automatizado | Aprovado |
| GC05 | Seleção IGDB sem categoria pessoal | Título, gêneros, capa, plataforma e ID vêm do catálogo | Confirmado pelas asserções: Título, gêneros, capa, plataforma e ID vêm do catálogo | Automatizado | Aprovado |
| GC06 | Texto alterado após selecionar | Formulário bloqueia seleção desatualizada | Confirmado pelas asserções: Formulário bloqueia seleção desatualizada | Automatizado | Aprovado |
| GC07 | API sem gêneros | Cadastro não cria gênero ou categoria fictícia | Confirmado pelas asserções: Cadastro não cria gênero ou categoria fictícia | Automatizado | Aprovado |
| GC08 | Categoria manual arbitrária em novo jogo | Valor não substitui gêneros externos | Confirmado pelas asserções: Valor não substitui gêneros externos | Automatizado | Aprovado |
| GC09 | Jogo novo salvo e recarregado | Categoria vazia e metadados reais continuam válidos | Confirmado pelas asserções: Categoria vazia e metadados reais continuam válidos | Automatizado | Aprovado |
| GC10 | Jogo sem gêneros salvo e recarregado | Continua sem gênero inventado | Confirmado pelas asserções: Continua sem gênero inventado | Automatizado | Aprovado |
| GC11 | Abrir cadastro de jogo | Botão desabilitado antes de seleção válida | Confirmado pelas asserções: Botão desabilitado antes de seleção válida | Automatizado | Aprovado |
| GC12 | Digitar título arbitrário na janela | Botão continua desabilitado e nenhuma gravação ocorre | Confirmado pelas asserções: Botão continua desabilitado e nenhuma gravação ocorre | Automatizado | Aprovado |
| GC13 | Selecionar sugestão por consulta parcial | Campo exibe título oficial e seleção continua válida após eventos | Confirmado pelas asserções: Campo exibe título oficial e seleção continua válida após eventos | Automatizado | Aprovado |
| GC14 | Modificar campo após seleção | Seleção e gêneros antigos são invalidados | Confirmado pelas asserções: Seleção e gêneros antigos são invalidados | Automatizado | Aprovado |
| GC15 | Limpar consulta após selecionar | Botão fica desabilitado sem sugestões remanescentes | Confirmado pelas asserções: Botão fica desabilitado sem sugestões remanescentes | Automatizado | Aprovado |
| GC16 | Pesquisa IGDB bem-sucedida e seleção | Catálogo preenche título e todos os gêneros sem categoria manual | Confirmado pelas asserções: Catálogo preenche título e todos os gêneros sem categoria manual | Automatizado | Aprovado |
| GC17 | IGDB indisponível | Mensagem clara e nenhum cadastro sem seleção | Confirmado pelas asserções: Mensagem clara e nenhum cadastro sem seleção | Automatizado | Aprovado |
| GC18 | Resultado sem gêneros na janela | Mensagem de ausência sem RPG padrão | Confirmado pelas asserções: Mensagem de ausência sem RPG padrão | Automatizado | Aprovado |
| GC19 | Deselecionar sugestão | Botão volta a ficar desabilitado | Confirmado pelas asserções: Botão volta a ficar desabilitado | Automatizado | Aprovado |
| GC20 | Cadastro concluído | Formulário limpo exige nova seleção para próximo jogo | Confirmado pelas asserções: Formulário limpo exige nova seleção para próximo jogo | Automatizado | Aprovado |

## Gêneros e compatibilidade com categorias legadas — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| GL01 | Legado sem identificador externo | Categoria antiga permanece no modelo e JSON | Confirmado pelas asserções: Categoria antiga permanece no modelo e JSON | Automatizado | Aprovado |
| GL02 | Steam legado opaco | Edição pessoal não converte Steam em IGDB | Confirmado pelas asserções: Edição pessoal não converte Steam em IGDB | Automatizado | Aprovado |
| GL03 | Gêneros externos e categoria antiga coexistem | Exibição usa catálogo e persistência preserva categoria | Confirmado pelas asserções: Exibição usa catálogo e persistência preserva categoria | Automatizado | Aprovado |
| GL04 | Legado sem gêneros externos | Categoria legada aparece com identificação explícita | Confirmado pelas asserções: Categoria legada aparece com identificação explícita | Automatizado | Aprovado |
| GL05 | Categoria antiga fora das opções | Edição preserva valor sem classificação obrigatória | Confirmado pelas asserções: Edição preserva valor sem classificação obrigatória | Automatizado | Aprovado |
| GL06 | Legado filtrado por categoria | Filtro continua encontrando dados antigos | Confirmado pelas asserções: Filtro continua encontrando dados antigos | Automatizado | Aprovado |
| GL07 | Jogo sem gêneros externos nem categoria | Filtro não oferece categoria fictícia | Confirmado pelas asserções: Filtro não oferece categoria fictícia | Automatizado | Aprovado |
| GL08 | Categoria vazia sem origem IGDB | Persistência continua recusando registro inválido | Confirmado pelas asserções: Persistência continua recusando registro inválido | Automatizado | Aprovado |
| GL09 | Categoria null mesmo com IGDB | Nulo inválido não é silenciosamente convertido | Confirmado pelas asserções: Nulo inválido não é silenciosamente convertido | Automatizado | Aprovado |
| GL10 | Outras cinco mídias | Seleção simulada e categoria pessoal continuam disponíveis | Confirmado pelas asserções: Seleção simulada e categoria pessoal continuam disponíveis | Automatizado | Aprovado |
| GL11 | Extensões JSON antigas | Edição não remove campos adicionais | Confirmado pelas asserções: Edição não remove campos adicionais | Automatizado | Aprovado |
| GL12 | Coleção mista antiga e nova | Recarga preserva categorias antigas e vazio intencional no novo jogo | Confirmado pelas asserções: Recarga preserva categorias antigas e vazio intencional no novo jogo | Automatizado | Aprovado |

## Posições de páginas positivas e legado — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| NP01 | Pagina: Posição inicial um | Primeira posição aceita | Confirmado pelas asserções: Primeira posição aceita | Automatizado | Aprovado |
| NP02 | Pagina: Maior inteiro permitido | Valor mantido sem overflow | Confirmado pelas asserções: Valor mantido sem overflow | Automatizado | Aprovado |
| NP03 | Pagina: Posição zero nova | Cadastro recusado com mensagem de progresso inválido | Confirmado pelas asserções: Cadastro recusado com mensagem de progresso inválido | Automatizado | Aprovado |
| NP04 | Pagina: Posição negativa nova | Cadastro recusado sem conversão para positivo | Confirmado pelas asserções: Cadastro recusado sem conversão para positivo | Automatizado | Aprovado |
| NP05 | Pagina: Progresso não informado | Ausência preservada como null | Confirmado pelas asserções: Ausência preservada como null | Automatizado | Aprovado |
| NP06 | Pagina: Posição zero antiga | Leitura e edição de dados pessoais mantêm zero já existente | Confirmado pelas asserções: Leitura e edição de dados pessoais mantêm zero já existente | Automatizado | Aprovado |
| NP07 | Pagina: Corrigir zero antigo para positivo | Nova posição persiste | Confirmado pelas asserções: Nova posição persiste | Automatizado | Aprovado |
| NP08 | Pagina: Limpar posição antiga | Progresso volta a não informado | Confirmado pelas asserções: Progresso volta a não informado | Automatizado | Aprovado |
| NP09 | Pagina: Alterar posição positiva para zero | Modificação inválida recusada | Confirmado pelas asserções: Modificação inválida recusada | Automatizado | Aprovado |
| NP10 | Pagina: Salvar posição positiva | Posição recarrega corretamente | Confirmado pelas asserções: Posição recarrega corretamente | Automatizado | Aprovado |
| NP11 | Pagina: Carregar dado antigo sem alteração | Zero não é migrado automaticamente | Confirmado pelas asserções: Zero não é migrado automaticamente | Automatizado | Aprovado |
| NP12 | Pagina: Adicionar posição zero a registro sem progresso | Edição recusa novo zero | Confirmado pelas asserções: Edição recusa novo zero | Automatizado | Aprovado |

## Posições de capítulos positivas em livros e mangás — 15 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| NC01 | Capitulo: Posição inicial um | Primeira posição aceita | Confirmado pelas asserções: Primeira posição aceita | Automatizado | Aprovado |
| NC02 | Capitulo: Maior inteiro permitido | Valor mantido sem overflow | Confirmado pelas asserções: Valor mantido sem overflow | Automatizado | Aprovado |
| NC03 | Capitulo: Posição zero nova | Cadastro recusado com mensagem de progresso inválido | Confirmado pelas asserções: Cadastro recusado com mensagem de progresso inválido | Automatizado | Aprovado |
| NC04 | Capitulo: Posição negativa nova | Cadastro recusado sem conversão para positivo | Confirmado pelas asserções: Cadastro recusado sem conversão para positivo | Automatizado | Aprovado |
| NC05 | Capitulo: Progresso não informado | Ausência preservada como null | Confirmado pelas asserções: Ausência preservada como null | Automatizado | Aprovado |
| NC06 | Capitulo: Posição zero antiga | Leitura e edição de dados pessoais mantêm zero já existente | Confirmado pelas asserções: Leitura e edição de dados pessoais mantêm zero já existente | Automatizado | Aprovado |
| NC07 | Capitulo: Corrigir zero antigo para positivo | Nova posição persiste | Confirmado pelas asserções: Nova posição persiste | Automatizado | Aprovado |
| NC08 | Capitulo: Limpar posição antiga | Progresso volta a não informado | Confirmado pelas asserções: Progresso volta a não informado | Automatizado | Aprovado |
| NC09 | Capitulo: Alterar posição positiva para zero | Modificação inválida recusada | Confirmado pelas asserções: Modificação inválida recusada | Automatizado | Aprovado |
| NC10 | Capitulo: Salvar posição positiva | Posição recarrega corretamente | Confirmado pelas asserções: Posição recarrega corretamente | Automatizado | Aprovado |
| NC11 | Capitulo: Carregar dado antigo sem alteração | Zero não é migrado automaticamente | Confirmado pelas asserções: Zero não é migrado automaticamente | Automatizado | Aprovado |
| NC12 | Capitulo: Adicionar posição zero a registro sem progresso | Edição recusa novo zero | Confirmado pelas asserções: Edição recusa novo zero | Automatizado | Aprovado |
| NC13 | Capítulo de mangá zero | Mesma regra positiva aplicada ao mangá | Confirmado pelas asserções: Mesma regra positiva aplicada ao mangá | Automatizado | Aprovado |
| NC14 | Capítulo de mangá positivo | Cadastro e persistência aceitos | Confirmado pelas asserções: Cadastro e persistência aceitos | Automatizado | Aprovado |
| NC15 | Capítulo e volume de mangá não informados | Nenhum zero é preenchido automaticamente | Confirmado pelas asserções: Nenhum zero é preenchido automaticamente | Automatizado | Aprovado |

## Posições de volumes positivas e legado — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| NV01 | Volume: Posição inicial um | Primeira posição aceita | Confirmado pelas asserções: Primeira posição aceita | Automatizado | Aprovado |
| NV02 | Volume: Maior inteiro permitido | Valor mantido sem overflow | Confirmado pelas asserções: Valor mantido sem overflow | Automatizado | Aprovado |
| NV03 | Volume: Posição zero nova | Cadastro recusado com mensagem de progresso inválido | Confirmado pelas asserções: Cadastro recusado com mensagem de progresso inválido | Automatizado | Aprovado |
| NV04 | Volume: Posição negativa nova | Cadastro recusado sem conversão para positivo | Confirmado pelas asserções: Cadastro recusado sem conversão para positivo | Automatizado | Aprovado |
| NV05 | Volume: Progresso não informado | Ausência preservada como null | Confirmado pelas asserções: Ausência preservada como null | Automatizado | Aprovado |
| NV06 | Volume: Posição zero antiga | Leitura e edição de dados pessoais mantêm zero já existente | Confirmado pelas asserções: Leitura e edição de dados pessoais mantêm zero já existente | Automatizado | Aprovado |
| NV07 | Volume: Corrigir zero antigo para positivo | Nova posição persiste | Confirmado pelas asserções: Nova posição persiste | Automatizado | Aprovado |
| NV08 | Volume: Limpar posição antiga | Progresso volta a não informado | Confirmado pelas asserções: Progresso volta a não informado | Automatizado | Aprovado |
| NV09 | Volume: Alterar posição positiva para zero | Modificação inválida recusada | Confirmado pelas asserções: Modificação inválida recusada | Automatizado | Aprovado |
| NV10 | Volume: Salvar posição positiva | Posição recarrega corretamente | Confirmado pelas asserções: Posição recarrega corretamente | Automatizado | Aprovado |
| NV11 | Volume: Carregar dado antigo sem alteração | Zero não é migrado automaticamente | Confirmado pelas asserções: Zero não é migrado automaticamente | Automatizado | Aprovado |
| NV12 | Volume: Adicionar posição zero a registro sem progresso | Edição recusa novo zero | Confirmado pelas asserções: Edição recusa novo zero | Automatizado | Aprovado |

## Regras dos comandos de cadastro e atualização — 10 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| CS01 | Adição direta de jogo sem IGDB | Serviço bloqueia sem alterar memória ou arquivo | Confirmado pelas asserções: Serviço bloqueia sem alterar memória ou arquivo | Automatizado | Aprovado |
| CS02 | Adição direta de página zero | Serviço aplica a regra positiva | Confirmado pelas asserções: Serviço aplica a regra positiva | Automatizado | Aprovado |
| CS03 | Adição direta de capítulo zero | Serviço aplica a regra positiva | Confirmado pelas asserções: Serviço aplica a regra positiva | Automatizado | Aprovado |
| CS04 | Adição direta de volume zero | Serviço aplica a regra positiva | Confirmado pelas asserções: Serviço aplica a regra positiva | Automatizado | Aprovado |
| CS05 | Atualização para página zero | Serviço rejeita e conserva instância anterior | Confirmado pelas asserções: Serviço rejeita e conserva instância anterior | Automatizado | Aprovado |
| CS06 | Atualização para capítulo zero | Serviço rejeita sem gravação | Confirmado pelas asserções: Serviço rejeita sem gravação | Automatizado | Aprovado |
| CS07 | Atualização para volume negativo | Serviço rejeita sem gravação | Confirmado pelas asserções: Serviço rejeita sem gravação | Automatizado | Aprovado |
| CS08 | Nota em legado com página zero | Atualização local preserva posição antiga | Confirmado pelas asserções: Atualização local preserva posição antiga | Automatizado | Aprovado |
| CS09 | Nova mídia sem progresso | Cadastro local simulado continua aceito | Confirmado pelas asserções: Cadastro local simulado continua aceito | Automatizado | Aprovado |
| CS10 | Quantidade zero de episódios assistidos | Contagem continua aceitando zero | Confirmado pelas asserções: Contagem continua aceitando zero | Automatizado | Aprovado |

## Operações locais sem IGDB — 10 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| OL01 | Abrir biblioteca sem credenciais | Itens locais visíveis sem HTTP IGDB | Confirmado pelas asserções: Itens locais visíveis sem HTTP IGDB | Automatizado (Avalonia Headless/Skia) | Aprovado |
| OL02 | Editar nota de legado offline | Salva sem pedir seleção IGDB ou converter ID | Confirmado pelas asserções: Salva sem pedir seleção IGDB ou converter ID | Automatizado (Avalonia Headless/Skia) | Aprovado |
| OL03 | Editar review offline | Texto pessoal persiste sem requisição IGDB | Confirmado pelas asserções: Texto pessoal persiste sem requisição IGDB | Automatizado (Avalonia Headless/Skia) | Aprovado |
| OL04 | Filtrar local por categoria e nota | Operação continua offline | Confirmado pelas asserções: Operação continua offline | Automatizado (Avalonia Headless/Skia) | Aprovado |
| OL05 | Ordenar local offline | Ordem alfabética correta sem regravar JSON | Confirmado pelas asserções: Ordem alfabética correta sem regravar JSON | Automatizado (Avalonia Headless/Skia) | Aprovado |
| OL06 | Excluir legado sem ID IGDB offline | Demais registros preservados | Confirmado pelas asserções: Demais registros preservados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| OL07 | Editar jogo novo com gêneros offline | Metadados de catálogo permanecem e nota muda | Confirmado pelas asserções: Metadados de catálogo permanecem e nota muda | Automatizado (Avalonia Headless/Skia) | Aprovado |
| OL08 | Editar página de livro offline | Progresso positivo é salvo sem IGDB | Confirmado pelas asserções: Progresso positivo é salvo sem IGDB | Automatizado (Avalonia Headless/Skia) | Aprovado |
| OL09 | Registro sem qualquer identificador | Edição offline não exige recadastro | Confirmado pelas asserções: Edição offline não exige recadastro | Automatizado (Avalonia Headless/Skia) | Aprovado |
| OL10 | Alternar visualização e navegar offline | Biblioteca local continua acessível | Confirmado pelas asserções: Biblioteca local continua acessível | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Preenchimento e hover das capas na grade — 10 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| HV01 | Capa na grade | Preenchimento proporcional cobre a área do cartão | Confirmado pelas asserções: Preenchimento proporcional cobre a área do cartão | Automatizado (Avalonia Headless/Skia) | Aprovado |
| HV02 | Capa na lista | Modo reduzido preenche proporcionalmente a miniatura | Confirmado pelas asserções: Modo reduzido preenche proporcionalmente a miniatura | Automatizado (Avalonia Headless/Skia) | Aprovado |
| HV03 | Alterar preenchimento da capa | Imagem existente muda sem novo carregamento | Confirmado pelas asserções: Imagem existente muda sem novo carregamento | Automatizado (Avalonia Headless/Skia) | Aprovado |
| HV04 | Hover no cartão | Capa não é substituída, ocultada ou escurecida por opacidade | Confirmado pelas asserções: Capa não é substituída, ocultada ou escurecida por opacidade | Automatizado (Avalonia Headless/Skia) | Aprovado |
| HV05 | Hover em item não selecionado | Realce não seleciona ou exclui obra | Confirmado pelas asserções: Realce não seleciona ou exclui obra | Automatizado (Avalonia Headless/Skia) | Aprovado |
| HV06 | Hover em item selecionado | Seleção e ações de edição permanecem | Confirmado pelas asserções: Seleção e ações de edição permanecem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| HV07 | Sair do hover | Cartão conserva capa e dados pessoais | Confirmado pelas asserções: Cartão conserva capa e dados pessoais | Automatizado (Avalonia Headless/Skia) | Aprovado |
| HV08 | Grade compacta | Preenchimento mantido ao reduzir para 900 por 660 | Confirmado pelas asserções: Preenchimento mantido ao reduzir para 900 por 660 | Automatizado (Avalonia Headless/Skia) | Aprovado |
| HV09 | Alternar grade e lista | Ambos os templates preenchem a área proporcionalmente | Confirmado pelas asserções: Ambos os templates preenchem a área proporcionalmente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| HV10 | Comparar pixels antes e durante hover | Centro da capa mantém os mesmos pixels sem sobreposição | Confirmado pelas asserções: Centro da capa mantém os mesmos pixels sem sobreposição | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Textos multimídia e reviews — 13 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| TX01 | Página inicial vazia | Mensagens falam de obras e biblioteca | Confirmado pelas asserções: Mensagens falam de obras e biblioteca | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX02 | Chamada para cadastro na página inicial | Convite é genérico para todas as mídias | Confirmado pelas asserções: Convite é genérico para todas as mídias | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX03 | Biblioteca com livro concluído | Resumo usa obras e mantém contagem correta | Confirmado pelas asserções: Resumo usa obras e mantém contagem correta | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX04 | Resumo de seis mídias | Contadores locais incluem todas as categorias | Confirmado pelas asserções: Contadores locais incluem todas as categorias | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX05 | Recente de livro na página inicial | Tooltip diz obra e edição abre o tipo correto | Confirmado pelas asserções: Tooltip diz obra e edição abre o tipo correto | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX06 | Placeholder de review no cadastro | Texto genérico sem referência exclusiva a jogo | Confirmado pelas asserções: Texto genérico sem referência exclusiva a jogo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX07 | Trocar mídia no cadastro | Placeholder de review continua adequado às seis mídias | Confirmado pelas asserções: Placeholder de review continua adequado às seis mídias | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX08 | Editar review legada | Novo placeholder não modifica o conteúdo pessoal | Confirmado pelas asserções: Novo placeholder não modifica o conteúdo pessoal | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX09 | Excluir última obra | Mensagem vazia volta a ser genérica | Confirmado pelas asserções: Mensagem vazia volta a ser genérica | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX10 | Página inicial no tamanho mínimo | Textos multimídia renderizam e navegação permanece acessível | Confirmado pelas asserções: Textos multimídia renderizam e navegação permanece acessível | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX11 | Anime em andamento e filme concluído | Rótulos e contagens da página inicial abrangem mídias diferentes | Confirmado pelas asserções: Rótulos e contagens da página inicial abrangem mídias diferentes | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX12 | Recentes de leitura e reprodução | Status dos cartões continuam adaptados à mídia com tooltip genérico | Confirmado pelas asserções: Status dos cartões continuam adaptados à mídia com tooltip genérico | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TX13 | Auditoria dos textos estáticos da página inicial | Nenhuma mensagem exige que a biblioteca seja de jogos | Confirmado pelas asserções: Nenhuma mensagem exige que a biblioteca seja de jogos | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Título oficial depois da seleção nas seis mídias — 10 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| TC01 | Título de Jogo selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo | Confirmado pelas asserções: Nome oficial é exibido e persistido sem nova restrição de catálogo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TC02 | Título de Anime selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo | Confirmado pelas asserções: Nome oficial é exibido e persistido sem nova restrição de catálogo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TC03 | Título de Serie selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo | Confirmado pelas asserções: Nome oficial é exibido e persistido sem nova restrição de catálogo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TC04 | Título de Filme selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo | Confirmado pelas asserções: Nome oficial é exibido e persistido sem nova restrição de catálogo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TC05 | Título de Manga selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo | Confirmado pelas asserções: Nome oficial é exibido e persistido sem nova restrição de catálogo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TC06 | Título de Livro selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo | Confirmado pelas asserções: Nome oficial é exibido e persistido sem nova restrição de catálogo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TC07 | Título Unicode | Título exato do catálogo não é truncado nem substituído pela pesquisa | Confirmado pelas asserções: Título exato do catálogo não é truncado nem substituído pela pesquisa | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TC08 | Título com aspas | Título exato do catálogo não é truncado nem substituído pela pesquisa | Confirmado pelas asserções: Título exato do catálogo não é truncado nem substituído pela pesquisa | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TC09 | Nome maior que a consulta | Título exato do catálogo não é truncado nem substituído pela pesquisa | Confirmado pelas asserções: Título exato do catálogo não é truncado nem substituído pela pesquisa | Automatizado (Avalonia Headless/Skia) | Aprovado |
| TC10 | Nome menor que a consulta | Título exato do catálogo não é truncado nem substituído pela pesquisa | Confirmado pelas asserções: Título exato do catálogo não é truncado nem substituído pela pesquisa | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Posições no formulário e recuperação de erros — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| UP01 | Página zero no cadastro | Valor normalizado imediatamente para um e cadastro válido | Confirmado pelas asserções: Valor normalizado imediatamente para um e cadastro válido | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UP02 | Capítulo zero em livro | Valor normalizado imediatamente para um e cadastro válido | Confirmado pelas asserções: Valor normalizado imediatamente para um e cadastro válido | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UP03 | Capítulo zero em mangá | Valor normalizado imediatamente para um e cadastro válido | Confirmado pelas asserções: Valor normalizado imediatamente para um e cadastro válido | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UP04 | Volume zero no cadastro | Valor normalizado imediatamente para um e cadastro válido | Confirmado pelas asserções: Valor normalizado imediatamente para um e cadastro válido | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UP05 | Página negativa no controle | Valor normalizado para um antes de salvar | Confirmado pelas asserções: Valor normalizado para um antes de salvar | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UP06 | Página fracionária | Último inteiro válido é mantido imediatamente | Confirmado pelas asserções: Último inteiro válido é mantido imediatamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UP07 | Livro sem posição informada | Campos vazios persistem como ausência | Confirmado pelas asserções: Campos vazios persistem como ausência | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UP08 | Primeiro capítulo e volume | Posições um aceitas e recarregadas | Confirmado pelas asserções: Posições um aceitas e recarregadas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UP09 | Review de livro com página zero legada | Salva review preservando zero antigo | Confirmado pelas asserções: Salva review preservando zero antigo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UP10 | Limpar zero legado no formulário | Ausência substitui zero somente por escolha explícita | Confirmado pelas asserções: Ausência substitui zero somente por escolha explícita | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UP11 | Alterar posição positiva para zero | Posição vira um imediatamente e salva somente por confirmação | Confirmado pelas asserções: Posição vira um imediatamente e salva somente por confirmação | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UP12 | Ajustar posição normalizada | Cadastro funciona sem perder seleção | Confirmado pelas asserções: Cadastro funciona sem perder seleção | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Entrada imediata de posições: digitação, setas, ausência e legado — 32 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| NI01 | Configuração das posições | Mínimo um, incremento um, limite inteiro e ausência inicial | Confirmado pelas asserções: Mínimo um, incremento um, limite inteiro e ausência inicial | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI02 | Campo vazio ganha foco e perde foco | Ausência não se transforma em um | Confirmado pelas asserções: Ausência não se transforma em um | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI03 | Atribuição de zero | Valor vira um imediatamente | Confirmado pelas asserções: Valor vira um imediatamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI04 | Atribuição de negativo | Valor vira um imediatamente | Confirmado pelas asserções: Valor vira um imediatamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI05 | Valor acima do limite | Limite inteiro é conservado sem overflow | Confirmado pelas asserções: Limite inteiro é conservado sem overflow | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI06 | Atribuição fracionária | Inteiro anterior é mantido sem truncar | Confirmado pelas asserções: Inteiro anterior é mantido sem truncar | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI07 | Texto inteiro válido | Texto e valor sincronizam imediatamente | Confirmado pelas asserções: Texto e valor sincronizam imediatamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI08 | Texto zero | Editor e valor exibem um sem esperar salvar | Confirmado pelas asserções: Editor e valor exibem um sem esperar salvar | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI09 | Texto negativo | Editor normaliza para um imediatamente | Confirmado pelas asserções: Editor normaliza para um imediatamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI10 | Letras digitadas | Inteiro anterior é restaurado no editor | Confirmado pelas asserções: Inteiro anterior é restaurado no editor | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI11 | Texto fracionário | Valor e editor restauram inteiro anterior | Confirmado pelas asserções: Valor e editor restauram inteiro anterior | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI12 | Limpar texto após preencher | Valor fica null sem produzir um | Confirmado pelas asserções: Valor fica null sem produzir um | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI13 | Texto só de espaços | Normaliza para ausência | Confirmado pelas asserções: Normaliza para ausência | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI14 | Limpar zero legado | Valor vira ausência e mínimo volta a um | Confirmado pelas asserções: Valor vira ausência e mínimo volta a um | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI15 | Seta para cima no vazio | Primeira posição é um | Confirmado pelas asserções: Primeira posição é um | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI16 | Seta para baixo no vazio | Campo permanece não informado em vez de receber máximo | Confirmado pelas asserções: Campo permanece não informado em vez de receber máximo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI17 | Seta para baixo em um | Nunca produz zero | Confirmado pelas asserções: Nunca produz zero | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI18 | Seta para cima em dois | Incremento inicial continua um | Confirmado pelas asserções: Incremento inicial continua um | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI19 | Carregar zero legado | Zero exibido sem normalização e orientação explícita | Confirmado pelas asserções: Zero exibido sem normalização e orientação explícita | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI20 | Aumentar zero legado | Passa para um e não permite retornar a zero | Confirmado pelas asserções: Passa para um e não permite retornar a zero | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI21 | Introduzir zero depois de limpar legado | Novo zero normaliza para um | Confirmado pelas asserções: Novo zero normaliza para um | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI22 | Reutilizar formulário após legado | Novo campo vazio não herda permissão de zero | Confirmado pelas asserções: Novo campo vazio não herda permissão de zero | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI23 | Digitação real de zero | Texto visível é um imediatamente | Confirmado pelas asserções: Texto visível é um imediatamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI24 | Apagar seleção com teclado | Mantém ausência em vez de impor valor mínimo | Confirmado pelas asserções: Mantém ausência em vez de impor valor mínimo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI25 | Texto inválido em campo vazio | Permanece vazio sem inventar posição | Confirmado pelas asserções: Permanece vazio sem inventar posição | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI26 | Separador decimal brasileiro | Fração não vira página truncada | Confirmado pelas asserções: Fração não vira página truncada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI27 | Posições das duas mídias | Quatro campos usam o controle específico com incremento um | Confirmado pelas asserções: Quatro campos usam o controle específico com incremento um | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI28 | Nota pessoal | Escala zero a cinco e frações permanecem inalteradas | Confirmado pelas asserções: Escala zero a cinco e frações permanecem inalteradas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI29 | Episódios assistidos | Contagem zero continua permitida | Confirmado pelas asserções: Contagem zero continua permitida | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI30 | Carregar formulário legado sem salvar | Nenhum JSON é regravado automaticamente | Confirmado pelas asserções: Nenhum JSON é regravado automaticamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI31 | Botão de diminuir em campo vazio | Clique não preenche com o máximo nem com zero | Confirmado pelas asserções: Clique não preenche com o máximo nem com zero | Automatizado (Avalonia Headless/Skia) | Aprovado |
| NI32 | Botão de aumentar e limite inferior | Clique cria um e diminuir não produz zero | Confirmado pelas asserções: Clique cria um e diminuir não produz zero | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Grade sem contorno externo, altura e acessibilidade — 14 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| CG01 | Grade sem hover | Cartão mantém cantos discretos sem traço externo | Confirmado pelas asserções: Cartão mantém cantos discretos sem traço externo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG02 | Hover real na grade | Contorno continua ausente | Confirmado pelas asserções: Contorno continua ausente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG03 | Selecionar cartão | Marca interna e texto indicam seleção sem contorno | Confirmado pelas asserções: Marca interna e texto indicam seleção sem contorno | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG04 | Desfazer seleção | Marca de seleção desaparece sem apagar dados | Confirmado pelas asserções: Marca de seleção desaparece sem apagar dados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG05 | Foco por teclado | Texto de foco visível sem adorno externo | Confirmado pelas asserções: Texto de foco visível sem adorno externo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG06 | Acessibilidade da seleção | Provider informa seleção verdadeira após selecionar | Confirmado pelas asserções: Provider informa seleção verdadeira após selecionar | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG07 | Altura do cartão comum | Altura automática evita a reserva fixa de 352 pixels | Confirmado pelas asserções: Altura automática evita a reserva fixa de 352 pixels | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG08 | Janela mínima | Cartão compacto também usa altura de conteúdo | Confirmado pelas asserções: Cartão compacto também usa altura de conteúdo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG09 | Cartão com progresso | Altura permanece igual aos cartões sem progresso | Confirmado pelas asserções: Altura permanece igual aos cartões sem progresso | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG10 | Seleção e foco juntos | Ações de edição continuam habilitadas e ID não muda | Confirmado pelas asserções: Ações de edição continuam habilitadas e ID não muda | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG11 | Trocar visualização com seleção | Seleção permanece ao retornar à grade sem contorno | Confirmado pelas asserções: Seleção permanece ao retornar à grade sem contorno | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG12 | Captura de grade selecionada no mínimo | Marca interna e conteúdo renderizam sem contorno externo | Confirmado pelas asserções: Marca interna e conteúdo renderizam sem contorno externo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG13 | Margem externa com seleção e foco | Pixels fora do cartão não recebem camada do tema | Confirmado pelas asserções: Pixels fora do cartão não recebem camada do tema | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CG14 | Margem externa durante hover | Fundo além do cartão permanece igual ao fundo da página | Confirmado pelas asserções: Fundo além do cartão permanece igual ao fundo da página | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Miniaturas preenchidas na lista e imagem padrão — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| ML01 | Miniatura na lista | Preenchimento proporcional e área de 64 por 96 | Confirmado pelas asserções: Preenchimento proporcional e área de 64 por 96 | Automatizado (Avalonia Headless/Skia) | Aprovado |
| ML02 | Imagem horizontal na lista | Pixels das duas bordas são imagem sem margens laterais | Confirmado pelas asserções: Pixels das duas bordas são imagem sem margens laterais | Automatizado (Avalonia Headless/Skia) | Aprovado |
| ML03 | Imagem vertical estreita | Miniatura preenche lateralmente sem distorção de modo Fill | Confirmado pelas asserções: Miniatura preenche lateralmente sem distorção de modo Fill | Automatizado (Avalonia Headless/Skia) | Aprovado |
| ML04 | Imagem quadrada | Preenchimento também ocupa as duas laterais | Confirmado pelas asserções: Preenchimento também ocupa as duas laterais | Automatizado (Avalonia Headless/Skia) | Aprovado |
| ML05 | Miniatura no tamanho mínimo | Área e preenchimento conservados em 900 por 660 | Confirmado pelas asserções: Área e preenchimento conservados em 900 por 660 | Automatizado (Avalonia Headless/Skia) | Aprovado |
| ML06 | Miniatura em janela maior | Área reservada permanece preenchida | Confirmado pelas asserções: Área reservada permanece preenchida | Automatizado (Avalonia Headless/Skia) | Aprovado |
| ML07 | Ausência de capa | Fundo padrão ocupa toda a área e mensagem é legível | Confirmado pelas asserções: Fundo padrão ocupa toda a área e mensagem é legível | Automatizado (Avalonia Headless/Skia) | Aprovado |
| ML08 | URL inválida | Imagem padrão sem download nem mudança do tamanho | Confirmado pelas asserções: Imagem padrão sem download nem mudança do tamanho | Automatizado (Avalonia Headless/Skia) | Aprovado |
| ML09 | Seleção na lista | Capa permanece preenchida sem sobreposição na imagem | Confirmado pelas asserções: Capa permanece preenchida sem sobreposição na imagem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| ML10 | Troca de visualização | Lista volta preenchida usando cache da mesma capa | Confirmado pelas asserções: Lista volta preenchida usando cache da mesma capa | Automatizado (Avalonia Headless/Skia) | Aprovado |
| ML11 | Orientação original da imagem | Source conserva sua proporção enquanto o recorte é proporcional | Confirmado pelas asserções: Source conserva sua proporção enquanto o recorte é proporcional | Automatizado (Avalonia Headless/Skia) | Aprovado |
| ML12 | Captura da lista preenchida | Miniatura sem margens renderiza no tamanho mínimo | Confirmado pelas asserções: Miniatura sem margens renderiza no tamanho mínimo | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Capas e fallback nos adicionados recentemente — 14 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| CR01 | Recente com URL de capa | Imagem do catálogo aparece no lugar do ícone fixo | Confirmado pelas asserções: Imagem do catálogo aparece no lugar do ícone fixo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR02 | Recente sem capa | Fallback Sem capa permanece visível | Confirmado pelas asserções: Fallback Sem capa permanece visível | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR03 | Recente com URL inválida | Fallback sem download externo | Confirmado pelas asserções: Fallback sem download externo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR04 | CDN retorna 404 no recente | Imagem indisponível não interrompe página inicial | Confirmado pelas asserções: Imagem indisponível não interrompe página inicial | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR05 | Falha de conexão da capa recente | Uso local e fallback preservados | Confirmado pelas asserções: Uso local e fallback preservados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR06 | Recente de catálogo simulado | Desenho local carrega sem HTTP | Confirmado pelas asserções: Desenho local carrega sem HTTP | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR07 | Alternar início e biblioteca | Mesma capa é compartilhada pelo cache da sessão | Confirmado pelas asserções: Mesma capa é compartilhada pelo cache da sessão | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR08 | Abrir obra pelo recente com capa | Botão continua abrindo edição do registro correto | Confirmado pelas asserções: Botão continua abrindo edição do registro correto | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR09 | Carregar e navegar com capa recente | Arquivo não é regravado ou enriquecido automaticamente | Confirmado pelas asserções: Arquivo não é regravado ou enriquecido automaticamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR10 | Quatro obras na coleção | Recentes continuam limitados às três últimas na ordem anterior | Confirmado pelas asserções: Recentes continuam limitados às três últimas na ordem anterior | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR11 | Recentes na janela mínima | Cabeçalho com capa mantém altura e texto legível | Confirmado pelas asserções: Cabeçalho com capa mantém altura e texto legível | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR12 | Download recente pendente | Fallback inicial é substituído pela capa ao terminar | Confirmado pelas asserções: Fallback inicial é substituído pela capa ao terminar | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR13 | Bytes inválidos no recente | Fallback controlado sem encerramento | Confirmado pelas asserções: Fallback controlado sem encerramento | Automatizado (Avalonia Headless/Skia) | Aprovado |
| CR14 | Biblioteca vazia | Nenhum recente fictício é criado para exibir capa | Confirmado pelas asserções: Nenhum recente fictício é criado para exibir capa | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Dimensões e alinhamento uniformes na grade — 14 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| DG01 | Grade com capa larga, estreita e legado sem capa | Todos os cartões e áreas de capa têm medidas idênticas | Confirmado pelas asserções: Todos os cartões e áreas de capa têm medidas idênticas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG02 | Mesmos registros em 900 por 660 | Dimensões compactas padronizadas sem depender dos metadados | Confirmado pelas asserções: Dimensões compactas padronizadas sem depender dos metadados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG03 | Grade em 1280 por 900 | Cartões conservam dimensões normais e quebram linha pelo painel | Confirmado pelas asserções: Cartões conservam dimensões normais e quebram linha pelo painel | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG04 | Título, gêneros e review extensos | Textos não alargam ou aumentam cartões | Confirmado pelas asserções: Textos não alargam ou aumentam cartões | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG05 | Títulos e notas em registros mistos | Linhas visíveis ficam na mesma posição relativa ao cartão | Confirmado pelas asserções: Linhas visíveis ficam na mesma posição relativa ao cartão | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG06 | Progresso ausente ou presente | Espaço da linha não desaparece nos registros legados | Confirmado pelas asserções: Espaço da linha não desaparece nos registros legados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG07 | Fallback e bitmap | Área padrão ocupa o mesmo espaço da imagem carregada | Confirmado pelas asserções: Área padrão ocupa o mesmo espaço da imagem carregada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG08 | Obra simulada e registro sem flag de simulação | Linha reservada mantém a mesma altura nas duas situações | Confirmado pelas asserções: Linha reservada mantém a mesma altura nas duas situações | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG09 | Capa termina de carregar | Dimensões anteriores ao download não mudam | Confirmado pelas asserções: Dimensões anteriores ao download não mudam | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG10 | Lista e retorno à grade | Padronização não depende do template usado antes | Confirmado pelas asserções: Padronização não depende do template usado antes | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG11 | Selecionar cartão | Marca interna não muda altura ou alinhamento | Confirmado pelas asserções: Marca interna não muda altura ou alinhamento | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG12 | Reduzir e ampliar a janela | Medidas mudam juntas e nenhum JSON é regravado | Confirmado pelas asserções: Medidas mudam juntas e nenhum JSON é regravado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG13 | Seis categorias com e sem progresso | Padronização aplicada a todas as mídias | Confirmado pelas asserções: Padronização aplicada a todas as mídias | Automatizado (Avalonia Headless/Skia) | Aprovado |
| DG14 | Progresso numericamente extenso | Texto é abreviado sem deslocar as linhas seguintes | Confirmado pelas asserções: Texto é abreviado sem deslocar as linhas seguintes | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Arredondamento, recorte, estados e acessibilidade — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| RD01 | Grade arredondada | Raio discreto oito com espessura de borda zero | Confirmado pelas asserções: Raio discreto oito com espessura de borda zero | Automatizado (Avalonia Headless/Skia) | Aprovado |
| RD02 | Lista arredondada | Mesmo raio discreto dos cartões da grade | Confirmado pelas asserções: Mesmo raio discreto dos cartões da grade | Automatizado (Avalonia Headless/Skia) | Aprovado |
| RD03 | Recentes e capas | Contêiner e recorte compartilham raio oito | Confirmado pelas asserções: Contêiner e recorte compartilham raio oito | Automatizado (Avalonia Headless/Skia) | Aprovado |
| RD04 | Bitmap preenchido nos cantos | Imagem é realmente recortada e não invade os cantos arredondados | Confirmado pelas asserções: Imagem é realmente recortada e não invade os cantos arredondados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| RD05 | Imagem padrão arredondada | Fallback também usa recorte consistente sem virar imagem quadrada | Confirmado pelas asserções: Fallback também usa recorte consistente sem virar imagem quadrada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| RD06 | Hover arredondado | Cantos não reativam traço nem faixa externa do tema | Confirmado pelas asserções: Cantos não reativam traço nem faixa externa do tema | Automatizado (Avalonia Headless/Skia) | Aprovado |
| RD07 | Seleção arredondada | Marca interna permanece visível sem borda externa | Confirmado pelas asserções: Marca interna permanece visível sem borda externa | Automatizado (Avalonia Headless/Skia) | Aprovado |
| RD08 | Foco por teclado com cantos recuperados | Texto de foco continua visível sem adorno externo | Confirmado pelas asserções: Texto de foco continua visível sem adorno externo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| RD09 | Acessibilidade após arredondar | Provider mantém seleção do item | Confirmado pelas asserções: Provider mantém seleção do item | Automatizado (Avalonia Headless/Skia) | Aprovado |
| RD10 | Alternância das visualizações | Novos controles de capa conservam recorte arredondado | Confirmado pelas asserções: Novos controles de capa conservam recorte arredondado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| RD11 | Redimensionar recentes | Geometria de recorte acompanha a nova largura sem novo download | Confirmado pelas asserções: Geometria de recorte acompanha a nova largura sem novo download | Automatizado (Avalonia Headless/Skia) | Aprovado |
| RD12 | Capturas no tamanho mínimo | Grade uniforme e cantos renderizam nas três áreas | Confirmado pelas asserções: Grade uniforme e cantos renderizam nas três áreas | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Criar coleções: nomes, descrição, limites e falhas — 14 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| CC01 | Criar coleção normal | ID exclusivo, nome e descrição disponíveis | Confirmado pelas asserções: ID exclusivo, nome e descrição disponíveis | Automatizado | Aprovado |
| CC02 | Nome e descrição com espaços externos | Valores são aparados antes de salvar | Confirmado pelas asserções: Valores são aparados antes de salvar | Automatizado | Aprovado |
| CC03 | Nome vazio | Recusa sem criar coleção | Confirmado pelas asserções: Recusa sem criar coleção | Automatizado | Aprovado |
| CC04 | Nome só com espaços | Recusa sem gravação | Confirmado pelas asserções: Recusa sem gravação | Automatizado | Aprovado |
| CC05 | Nome de 120 caracteres | Limite aceito sem truncamento | Confirmado pelas asserções: Limite aceito sem truncamento | Automatizado | Aprovado |
| CC06 | Nome de 121 caracteres | Recusa sem truncar ou salvar | Confirmado pelas asserções: Recusa sem truncar ou salvar | Automatizado | Aprovado |
| CC07 | Nome duplicado com caixa e espaços diferentes | Segunda coleção recusada | Confirmado pelas asserções: Segunda coleção recusada | Automatizado | Aprovado |
| CC08 | Descrição opcional | Ausência vira string vazia sem valor inventado | Confirmado pelas asserções: Ausência vira string vazia sem valor inventado | Automatizado | Aprovado |
| CC09 | Descrição no limite e acima dele | Mil aceitos e mil e um recusados | Confirmado pelas asserções: Mil aceitos e mil e um recusados | Automatizado | Aprovado |
| CC10 | Nome Unicode | Acentos e símbolos preservados | Confirmado pelas asserções: Acentos e símbolos preservados | Automatizado | Aprovado |
| CC11 | Falha externa ao criar | Coleção não aparece na memória sem persistência | Confirmado pelas asserções: Coleção não aparece na memória sem persistência | Automatizado | Aprovado |
| CC12 | Criar sem carregamento válido | Operação bloqueada antes de gravar | Confirmado pelas asserções: Operação bloqueada antes de gravar | Automatizado | Aprovado |
| CC13 | Nome com quebra de linha | Recusa mensagem ilegível na seleção | Confirmado pelas asserções: Recusa mensagem ilegível na seleção | Automatizado | Aprovado |
| CC14 | Criar não grava biblioteca | Nenhuma obra recebe ID apenas por criar coleção | Confirmado pelas asserções: Nenhuma obra recebe ID apenas por criar coleção | Automatizado | Aprovado |

## Renomear e editar coleções sem perder vínculos — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| CE01 | Renomear coleção | ID permanece o mesmo | Confirmado pelas asserções: ID permanece o mesmo | Automatizado | Aprovado |
| CE02 | Editar descrição | Texto é persistido no snapshot novo | Confirmado pelas asserções: Texto é persistido no snapshot novo | Automatizado | Aprovado |
| CE03 | Renomear coleção com obras | Associações permanecem | Confirmado pelas asserções: Associações permanecem | Automatizado | Aprovado |
| CE04 | Editar para mesmo nome | Atualização válida sem criar coleção extra | Confirmado pelas asserções: Atualização válida sem criar coleção extra | Automatizado | Aprovado |
| CE05 | Renomear para nome de outra coleção | Recusa e mantém nomes anteriores | Confirmado pelas asserções: Recusa e mantém nomes anteriores | Automatizado | Aprovado |
| CE06 | Renomear para vazio | Nome original preservado | Confirmado pelas asserções: Nome original preservado | Automatizado | Aprovado |
| CE07 | Renomear acima do limite | Nome original não é truncado | Confirmado pelas asserções: Nome original não é truncado | Automatizado | Aprovado |
| CE08 | Editar descrição acima do limite | Descrição anterior preservada | Confirmado pelas asserções: Descrição anterior preservada | Automatizado | Aprovado |
| CE09 | Editar coleção inexistente | Nenhuma coleção ou arquivo novo | Confirmado pelas asserções: Nenhuma coleção ou arquivo novo | Automatizado | Aprovado |
| CE10 | Falha ao editar | Snapshot anterior permanece inteiro | Confirmado pelas asserções: Snapshot anterior permanece inteiro | Automatizado | Aprovado |
| CE11 | Aparar e limpar descrição | Valores normalizados sem mudar identidade | Confirmado pelas asserções: Valores normalizados sem mudar identidade | Automatizado | Aprovado |
| CE12 | Editar e reiniciar | Nome, descrição, ID e associações recarregam | Confirmado pelas asserções: Nome, descrição, ID e associações recarregam | Automatizado | Aprovado |

## Excluir coleções preservando obras e outras coleções — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| CD01 | Excluir coleção vazia | Somente a coleção desaparece | Confirmado pelas asserções: Somente a coleção desaparece | Automatizado | Aprovado |
| CD02 | Excluir coleção com obra | Obra permanece na biblioteca | Confirmado pelas asserções: Obra permanece na biblioteca | Automatizado | Aprovado |
| CD03 | Obra em duas coleções | Excluir uma preserva outra e sua associação | Confirmado pelas asserções: Excluir uma preserva outra e sua associação | Automatizado | Aprovado |
| CD04 | Excluir coleção inexistente | Nenhum estado é alterado | Confirmado pelas asserções: Nenhum estado é alterado | Automatizado | Aprovado |
| CD05 | Falha ao excluir | Coleção e vínculos anteriores permanecem | Confirmado pelas asserções: Coleção e vínculos anteriores permanecem | Automatizado | Aprovado |
| CD06 | Excluir sem biblioteca carregada | Operação bloqueada | Confirmado pelas asserções: Operação bloqueada | Automatizado | Aprovado |
| CD07 | Excluir última coleção | Biblioteca e identificadores permanecem | Confirmado pelas asserções: Biblioteca e identificadores permanecem | Automatizado | Aprovado |
| CD08 | Excluir não grava biblioteca | Arquivo de obras não recebe nova gravação | Confirmado pelas asserções: Arquivo de obras não recebe nova gravação | Automatizado | Aprovado |
| CD09 | Excluir e reiniciar | Exclusão persistida e obras recarregadas | Confirmado pelas asserções: Exclusão persistida e obras recarregadas | Automatizado | Aprovado |
| CD10 | Coleção com seis mídias | Excluir coleção não remove nenhuma categoria | Confirmado pelas asserções: Excluir coleção não remove nenhuma categoria | Automatizado | Aprovado |
| CD11 | Reutilizar nome de coleção excluída | Nova coleção tem identidade diferente | Confirmado pelas asserções: Nova coleção tem identidade diferente | Automatizado | Aprovado |
| CD12 | Falha seguida de recuperação | Excluir funciona após restaurar o armazenamento | Confirmado pelas asserções: Excluir funciona após restaurar o armazenamento | Automatizado | Aprovado |

## Associar obras: várias coleções, duplicatas e falhas — 14 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| AC01 | Associar obra existente | Vínculo criado sem duplicar biblioteca | Confirmado pelas asserções: Vínculo criado sem duplicar biblioteca | Automatizado | Aprovado |
| AC02 | Uma obra em duas coleções | Mesma identidade e uma única obra pessoal | Confirmado pelas asserções: Mesma identidade e uma única obra pessoal | Automatizado | Aprovado |
| AC03 | Associar novamente mesma obra | Operação idempotente sem nova gravação | Confirmado pelas asserções: Operação idempotente sem nova gravação | Automatizado | Aprovado |
| AC04 | Registros iguais na biblioteca | Instâncias recebem identidades e vínculos independentes | Confirmado pelas asserções: Instâncias recebem identidades e vínculos independentes | Automatizado | Aprovado |
| AC05 | Associar à coleção inexistente | Nenhuma identidade ou gravação na biblioteca | Confirmado pelas asserções: Nenhuma identidade ou gravação na biblioteca | Automatizado | Aprovado |
| AC06 | Obra que não pertence à biblioteca | Recusa sem adicionar registro manual | Confirmado pelas asserções: Recusa sem adicionar registro manual | Automatizado | Aprovado |
| AC07 | Falha ao salvar identidade da obra | Nenhum vínculo é salvo | Confirmado pelas asserções: Nenhum vínculo é salvo | Automatizado | Aprovado |
| AC08 | Falha no segundo arquivo | Identidade salva permanece e associação não é aplicada | Confirmado pelas asserções: Identidade salva permanece e associação não é aplicada | Automatizado | Aprovado |
| AC09 | Tentar novamente após falha no segundo arquivo | Identidade é reaproveitada e vínculo é criado | Confirmado pelas asserções: Identidade é reaproveitada e vínculo é criado | Automatizado | Aprovado |
| AC10 | Seis categorias na mesma coleção | Vínculos independem do tipo oficial de mídia | Confirmado pelas asserções: Vínculos independem do tipo oficial de mídia | Automatizado | Aprovado |
| AC11 | Registro legado sem ID de catálogo | Associação não exige IGDB ou recadastro | Confirmado pelas asserções: Associação não exige IGDB ou recadastro | Automatizado | Aprovado |
| AC12 | Associar e reiniciar | Associação aponta para mesma obra com dados pessoais preservados | Confirmado pelas asserções: Associação aponta para mesma obra com dados pessoais preservados | Automatizado | Aprovado |
| AC13 | Associação com ID já persistido | Biblioteca não é regravada desnecessariamente | Confirmado pelas asserções: Biblioteca não é regravada desnecessariamente | Automatizado | Aprovado |
| AC14 | Remover obra da biblioteca e cadastrar mesmo catálogo | Nova instância não herda vínculo antigo | Confirmado pelas asserções: Nova instância não herda vínculo antigo | Automatizado | Aprovado |

## Remover associações sem excluir obras — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| RA01 | Remover vínculo | Obra permanece na biblioteca | Confirmado pelas asserções: Obra permanece na biblioteca | Automatizado | Aprovado |
| RA02 | Remover em uma das duas coleções | Outra associação permanece | Confirmado pelas asserções: Outra associação permanece | Automatizado | Aprovado |
| RA03 | Remover vínculo inexistente | Retorna falso sem gravação | Confirmado pelas asserções: Retorna falso sem gravação | Automatizado | Aprovado |
| RA04 | Remover de coleção inexistente | Nenhuma associação muda | Confirmado pelas asserções: Nenhuma associação muda | Automatizado | Aprovado |
| RA05 | Falha ao remover vínculo | Associação anterior permanece | Confirmado pelas asserções: Associação anterior permanece | Automatizado | Aprovado |
| RA06 | Remover não grava biblioteca | Conteúdo e identidade da obra não mudam | Confirmado pelas asserções: Conteúdo e identidade da obra não mudam | Automatizado | Aprovado |
| RA07 | Duplicatas pessoais associadas | Remover segundo vínculo preserva primeira instância | Confirmado pelas asserções: Remover segundo vínculo preserva primeira instância | Automatizado | Aprovado |
| RA08 | Remover e reiniciar | Vínculo continua ausente e obra continua disponível | Confirmado pelas asserções: Vínculo continua ausente e obra continua disponível | Automatizado | Aprovado |
| RA09 | Reassociar após remoção | Usa mesma identidade sem duplicar obra | Confirmado pelas asserções: Usa mesma identidade sem duplicar obra | Automatizado | Aprovado |
| RA10 | Remover último vínculo | Coleção passa a vazia sem ser excluída | Confirmado pelas asserções: Coleção passa a vazia sem ser excluída | Automatizado | Aprovado |
| RA11 | Referência de obra excluída da biblioteca | Pode ser removida explicitamente da coleção | Confirmado pelas asserções: Pode ser removida explicitamente da coleção | Automatizado | Aprovado |
| RA12 | Recuperar falha de remoção | Repetição funciona quando armazenamento volta | Confirmado pelas asserções: Repetição funciona quando armazenamento volta | Automatizado | Aprovado |

## Consultar obras: todas as mídias, atualização e referências ausentes — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| LC01 | Coleção vazia | Projeção retorna lista vazia | Confirmado pelas asserções: Projeção retorna lista vazia | Automatizado | Aprovado |
| LC02 | Projeção de associação | Retorna instância da biblioteca em vez de duplicá-la | Confirmado pelas asserções: Retorna instância da biblioteca em vez de duplicá-la | Automatizado | Aprovado |
| LC03 | Editar dados pessoais após associar | Todas as coleções exibem a atualização | Confirmado pelas asserções: Todas as coleções exibem a atualização | Automatizado | Aprovado |
| LC04 | Coleção mista | Todas as seis mídias podem ser exibidas juntas | Confirmado pelas asserções: Todas as seis mídias podem ser exibidas juntas | Automatizado | Aprovado |
| LC05 | Ordem de associação | Projeção mantém ordem de inclusão na coleção | Confirmado pelas asserções: Projeção mantém ordem de inclusão na coleção | Automatizado | Aprovado |
| LC06 | Coleção desconhecida | Projeção recusa sem retornar outra coleção | Confirmado pelas asserções: Projeção recusa sem retornar outra coleção | Automatizado | Aprovado |
| LC07 | Obra removida da biblioteca | Projeção não mostra registro fantasma e conserva referência | Confirmado pelas asserções: Projeção não mostra registro fantasma e conserva referência | Automatizado | Aprovado |
| LC08 | Restaurar obra com ID anterior | Referência preservada volta a resolver sem heurística por título | Confirmado pelas asserções: Referência preservada volta a resolver sem heurística por título | Automatizado | Aprovado |
| LC09 | Novo registro com mesmo título | Não ocupa automaticamente vínculo de obra excluída | Confirmado pelas asserções: Não ocupa automaticamente vínculo de obra excluída | Automatizado | Aprovado |
| LC10 | Consultar não grava | Arquivos e memória não são alterados pela visualização | Confirmado pelas asserções: Arquivos e memória não são alterados pela visualização | Automatizado | Aprovado |
| LC11 | Snapshot de associações | Lista exposta não permite mutação externa | Confirmado pelas asserções: Lista exposta não permite mutação externa | Automatizado | Aprovado |
| LC12 | Coleções e biblioteca recarregadas | Projeção conserva metadados, progresso e IDs | Confirmado pelas asserções: Projeção conserva metadados, progresso e IDs | Automatizado | Aprovado |

## Identidade interna opcional, compatibilidade e edição — 20 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| GI01 | Carregar legado | Nenhum ID interno ou gravação automática | Confirmado pelas asserções: Nenhum ID interno ou gravação automática | Automatizado | Aprovado |
| GI02 | Identificar explicitamente | GUID não vazio salvo e disponível | Confirmado pelas asserções: GUID não vazio salvo e disponível | Automatizado | Aprovado |
| GI03 | Identificar repetidamente | Mesmo ID sem outra gravação | Confirmado pelas asserções: Mesmo ID sem outra gravação | Automatizado | Aprovado |
| GI04 | Identidade já persistida | GUID existente nunca é substituído | Confirmado pelas asserções: GUID existente nunca é substituído | Automatizado | Aprovado |
| GI05 | Duplicatas sem ID | Cada instância recebe GUID diferente | Confirmado pelas asserções: Cada instância recebe GUID diferente | Automatizado | Aprovado |
| GI06 | Falha na gravação de ID | Instância anterior continua sem ID e sem alias confirmado | Confirmado pelas asserções: Instância anterior continua sem ID e sem alias confirmado | Automatizado | Aprovado |
| GI07 | Recuperar após falha de ID | Identificação funciona sem duplicar registro | Confirmado pelas asserções: Identificação funciona sem duplicar registro | Automatizado | Aprovado |
| GI08 | Instância anterior à identificação | Edição encontra obra atual e preserva GUID | Confirmado pelas asserções: Edição encontra obra atual e preserva GUID | Automatizado | Aprovado |
| GI09 | Instância anterior à identificação | Remoção local encontra somente a obra correspondente | Confirmado pelas asserções: Remoção local encontra somente a obra correspondente | Automatizado | Aprovado |
| GI10 | Identificador de obra externa | Não é atribuído a objeto sem relação com a biblioteca | Confirmado pelas asserções: Não é atribuído a objeto sem relação com a biblioteca | Automatizado | Aprovado |
| GI11 | Roundtrip do identificador | GUID persiste com todos os campos pessoais | Confirmado pelas asserções: GUID persiste com todos os campos pessoais | Automatizado | Aprovado |
| GI12 | Legado sem ID no JSON | Propriedade nova é omitida sem migração | Confirmado pelas asserções: Propriedade nova é omitida sem migração | Automatizado | Aprovado |
| GI13 | GUID vazio no arquivo | Leitura bloqueada e bytes preservados | Confirmado pelas asserções: Leitura bloqueada e bytes preservados | Automatizado | Aprovado |
| GI14 | Dois registros com mesmo GUID | Leitura recusa sem deduplicar ou excluir | Confirmado pelas asserções: Leitura recusa sem deduplicar ou excluir | Automatizado | Aprovado |
| GI15 | GUID inválido no texto JSON | Leitura controlada sem gravar | Confirmado pelas asserções: Leitura controlada sem gravar | Automatizado | Aprovado |
| GI16 | Adição com GUID duplicado | Serviço não grava candidato inválido | Confirmado pelas asserções: Serviço não grava candidato inválido | Automatizado | Aprovado |
| GI17 | GUID reservado em extensão JSON | Persistência recusa duplicação de propriedade | Confirmado pelas asserções: Persistência recusa duplicação de propriedade | Automatizado | Aprovado |
| GI18 | ID interno não interpreta IDs externos | Steam e IGDB permanecem distintos | Confirmado pelas asserções: Steam e IGDB permanecem distintos | Automatizado | Aprovado |
| GI19 | Recarga da biblioteca | Identidades persistidas continuam estáveis | Confirmado pelas asserções: Identidades persistidas continuam estáveis | Automatizado | Aprovado |
| GI20 | Arquivo alterado externamente antes de identificar | Nenhuma associação sobrescreve a versão externa | Confirmado pelas asserções: Nenhuma associação sobrescreve a versão externa | Automatizado | Aprovado |

## Persistência de coleções: integridade, concorrência e recuperação — 24 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| JS01 | Arquivo de coleções inexistente | Leitura vazia sem criação automática | Confirmado pelas asserções: Leitura vazia sem criação automática | Automatizado | Aprovado |
| JS02 | Salvar primeira coleção | Documento versionado e vínculos recarregáveis | Confirmado pelas asserções: Documento versionado e vínculos recarregáveis | Automatizado | Aprovado |
| JS03 | Atualizar arquivo | Backup contém bytes da versão anterior | Confirmado pelas asserções: Backup contém bytes da versão anterior | Automatizado | Aprovado |
| JS04 | Salvar sem ler antes | Operação bloqueada sem arquivo novo | Confirmado pelas asserções: Operação bloqueada sem arquivo novo | Automatizado | Aprovado |
| JS05 | JSON truncado | Leitura controlada e gravação bloqueada | Confirmado pelas asserções: Leitura controlada e gravação bloqueada | Automatizado | Aprovado |
| JS06 | Documento null | Não substitui arquivo por coleção vazia | Confirmado pelas asserções: Não substitui arquivo por coleção vazia | Automatizado | Aprovado |
| JS07 | Versão desconhecida | Nenhum campo futuro é descartado | Confirmado pelas asserções: Nenhum campo futuro é descartado | Automatizado | Aprovado |
| JS08 | Campo futuro desconhecido | Documento preservado sem sobrescrita | Confirmado pelas asserções: Documento preservado sem sobrescrita | Automatizado | Aprovado |
| JS09 | Campo obrigatório ausente | Recusa sem assumir valor padrão | Confirmado pelas asserções: Recusa sem assumir valor padrão | Automatizado | Aprovado |
| JS10 | Coleção nula na lista | Arquivo inválido não é filtrado ou truncado | Confirmado pelas asserções: Arquivo inválido não é filtrado ou truncado | Automatizado | Aprovado |
| JS11 | GUID malformado | Erro controlado preserva original | Confirmado pelas asserções: Erro controlado preserva original | Automatizado | Aprovado |
| JS12 | IDs de coleção duplicados | Recusa sem deduplicar | Confirmado pelas asserções: Recusa sem deduplicar | Automatizado | Aprovado |
| JS13 | Nomes duplicados no disco | Recusa sem escolher uma coleção arbitrária | Confirmado pelas asserções: Recusa sem escolher uma coleção arbitrária | Automatizado | Aprovado |
| JS14 | Associação repetida no documento | Recusa sem remover vínculo automaticamente | Confirmado pelas asserções: Recusa sem remover vínculo automaticamente | Automatizado | Aprovado |
| JS15 | Associação de GUID vazio | Recusa metadado inválido | Confirmado pelas asserções: Recusa metadado inválido | Automatizado | Aprovado |
| JS16 | Duas instâncias alteram coleções | Segunda gravação detecta conflito e preserva primeira | Confirmado pelas asserções: Segunda gravação detecta conflito e preserva primeira | Automatizado | Aprovado |
| JS17 | Arquivo removido externamente | Gravação não recria e sobrescreve silenciosamente | Confirmado pelas asserções: Gravação não recria e sobrescreve silenciosamente | Automatizado | Aprovado |
| JS18 | Arquivo de lock em uso | Gravação falha sem mudar documento | Confirmado pelas asserções: Gravação falha sem mudar documento | Automatizado | Aprovado |
| JS19 | Destino do backup inválido | Documento anterior preservado e temporário removido | Confirmado pelas asserções: Documento anterior preservado e temporário removido | Automatizado | Aprovado |
| JS20 | Corrigir falha no backup e tentar novamente | Gravação volta a funcionar sem descartar dados | Confirmado pelas asserções: Gravação volta a funcionar sem descartar dados | Automatizado | Aprovado |
| JS21 | BOM UTF-16 e nomes Unicode | Leitura preserva dados e backup preserva bytes originais | Confirmado pelas asserções: Leitura preserva dados e backup preserva bytes originais | Automatizado | Aprovado |
| JS22 | Coleções salvas ao lado da biblioteca | Bytes da biblioteca nunca são regravados por metadados de coleção | Confirmado pelas asserções: Bytes da biblioteca nunca são regravados por metadados de coleção | Automatizado | Aprovado |
| JS23 | Caminho relativo resolvido | Origem é absoluta e estável | Confirmado pelas asserções: Origem é absoluta e estável | Automatizado | Aprovado |
| JS24 | Recuperação após JSON inválido | Reabrir conteúdo corrigido permite novas alterações | Confirmado pelas asserções: Reabrir conteúdo corrigido permite novas alterações | Automatizado | Aprovado |

## Interface, confirmação, indisponibilidade e tamanho mínimo — 24 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| UC01 | Abrir página de coleções vazia | Estado vazio e ações dependentes desabilitadas | Confirmado pelas asserções: Estado vazio e ações dependentes desabilitadas | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC02 | Criar coleção pela janela | Seleção, nome normalizado e descrição disponíveis | Confirmado pelas asserções: Seleção, nome normalizado e descrição disponíveis | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC03 | Nome vazio na janela | Aviso sem criar arquivo de coleções | Confirmado pelas asserções: Aviso sem criar arquivo de coleções | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC04 | Nome duplicado na janela | Segunda criação não altera coleção existente | Confirmado pelas asserções: Segunda criação não altera coleção existente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC05 | Salvar nome e descrição pela janela | Alteração preserva associação | Confirmado pelas asserções: Alteração preserva associação | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC06 | Associar obra pelo seletor | Biblioteca não duplica e lista da coleção mostra obra | Confirmado pelas asserções: Biblioteca não duplica e lista da coleção mostra obra | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC07 | Repetir associação pela janela | Mensagem específica e apenas um vínculo | Confirmado pelas asserções: Mensagem específica e apenas um vínculo | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC08 | Associar mesma obra em duas coleções | Uma obra pessoal e dois vínculos independentes | Confirmado pelas asserções: Uma obra pessoal e dois vínculos independentes | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC09 | Remover associação selecionada | Obra permanece na biblioteca e outra coleção | Confirmado pelas asserções: Obra permanece na biblioteca e outra coleção | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC10 | Solicitar exclusão de coleção | Confirmação aparece sem excluir antecipadamente | Confirmado pelas asserções: Confirmação aparece sem excluir antecipadamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC11 | Cancelar exclusão | Coleção e associações permanecem | Confirmado pelas asserções: Coleção e associações permanecem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC12 | Confirmar exclusão | Coleção removida sem apagar registros pessoais | Confirmado pelas asserções: Coleção removida sem apagar registros pessoais | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC13 | Trocar coleção enquanto confirma exclusão | Confirmação antiga é cancelada sem excluir outra coleção | Confirmado pelas asserções: Confirmação antiga é cancelada sem excluir outra coleção | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC14 | Coleção com todas as mídias | Lista mostra seis tipos sem mudar categorias oficiais | Confirmado pelas asserções: Lista mostra seis tipos sem mudar categorias oficiais | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC15 | Editar dados pessoais pela coleção | Formulário preserva tipo e atualiza obra em todas as visualizações | Confirmado pelas asserções: Formulário preserva tipo e atualiza obra em todas as visualizações | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC16 | Arquivo de coleções inválido | Coleções bloqueadas e edição pessoal local continua | Confirmado pelas asserções: Coleções bloqueadas e edição pessoal local continua | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC17 | Biblioteca inválida | Coleções não podem receber alterações ou obras inexistentes | Confirmado pelas asserções: Coleções não podem receber alterações ou obras inexistentes | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC18 | Falha ao salvar coleção | Aviso e estado anterior preservados na janela | Confirmado pelas asserções: Aviso e estado anterior preservados na janela | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC19 | Falha na associação após salvar identidade | Interface permanece coerente e repetição funciona | Confirmado pelas asserções: Interface permanece coerente e repetição funciona | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC20 | Duplicatas na janela | Selecionar segunda obra não associa primeira implicitamente | Confirmado pelas asserções: Selecionar segunda obra não associa primeira implicitamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC21 | Reabrir janela com os dois arquivos | Nome, descrição e associação recarregam | Confirmado pelas asserções: Nome, descrição e associação recarregam | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC22 | Coleção no tamanho mínimo | Controles e obras ficam acessíveis pela rolagem | Confirmado pelas asserções: Controles e obras ficam acessíveis pela rolagem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC23 | Obra excluída fora da coleção | Contagem e aviso não exibem registro fantasma | Confirmado pelas asserções: Contagem e aviso não exibem registro fantasma | Automatizado (Avalonia Headless/Skia) | Aprovado |
| UC24 | Formulário de edição aberto antes de atribuir ID | Edição posterior salva sem perder associação | Confirmado pelas asserções: Edição posterior salva sem perder associação | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Integração entre arquivos, composição e registros legados — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| IX01 | Composição local padrão | Coleções usam arquivo irmão e abertura não grava JSON | Confirmado pelas asserções: Coleções usam arquivo irmão e abertura não grava JSON | Automatizado | Aprovado |
| IX02 | Caminho de biblioteca personalizado | Sidecar específico não colide com biblioteca nem preferência | Confirmado pelas asserções: Sidecar específico não colide com biblioteca nem preferência | Automatizado | Aprovado |
| IX03 | Duplicatas com mesmo ID externo após reiniciar | Duas identidades pessoais e dois vínculos persistem | Confirmado pelas asserções: Duas identidades pessoais e dois vínculos persistem | Automatizado | Aprovado |
| IX04 | Identificar legado com extensão desconhecida | Somente novo ID é acrescentado semanticamente | Confirmado pelas asserções: Somente novo ID é acrescentado semanticamente | Automatizado | Aprovado |
| IX05 | Metadados de coleção sem associações | Criar, editar e excluir não mudam bytes da biblioteca | Confirmado pelas asserções: Criar, editar e excluir não mudam bytes da biblioteca | Automatizado | Aprovado |
| IX06 | Serializer do formato anterior com extensão JSON | ID desconhecido é preservado para recarga das associações | Confirmado pelas asserções: ID desconhecido é preservado para recarga das associações | Automatizado | Aprovado |
| IX07 | Conflito no arquivo de coleções na primeira associação | Identidade segura e arquivo externo não sobrescrito | Confirmado pelas asserções: Identidade segura e arquivo externo não sobrescrito | Automatizado | Aprovado |
| IX08 | Arquivo de coleções corrompido | Dados pessoais continuam editáveis pelo serviço de biblioteca | Confirmado pelas asserções: Dados pessoais continuam editáveis pelo serviço de biblioteca | Automatizado | Aprovado |
| IX09 | Biblioteca não carregada com coleções válidas | Operações de vínculo ficam bloqueadas | Confirmado pelas asserções: Operações de vínculo ficam bloqueadas | Automatizado | Aprovado |
| IX10 | Restaurar par consistente de arquivos simulados | Biblioteca e associações recuperam o mesmo estado | Confirmado pelas asserções: Biblioteca e associações recuperam o mesmo estado | Automatizado | Aprovado |
| IX11 | Criar coleção com biblioteca vazia | Coleção persiste sem inventar obras | Confirmado pelas asserções: Coleção persiste sem inventar obras | Automatizado | Aprovado |
| IX12 | Diretório de trabalho muda após composição | Caminhos já resolvidos permanecem na pasta inicial | Confirmado pelas asserções: Caminhos já resolvidos permanecem na pasta inicial | Automatizado | Aprovado |

## Temas e modos: seleção, inicialização e alternância — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| VT01 | Aplicar Moderno Claro | Seleção recuperada com paleta completa e recursos válidos | Confirmado pelas asserções: Seleção recuperada com paleta completa e recursos válidos | Automatizado | Aprovado |
| VT02 | Aplicar Moderno Escuro | Seleção recuperada com paleta completa e recursos válidos | Confirmado pelas asserções: Seleção recuperada com paleta completa e recursos válidos | Automatizado | Aprovado |
| VT03 | Aplicar Tecnologico Claro | Seleção recuperada com paleta completa e recursos válidos | Confirmado pelas asserções: Seleção recuperada com paleta completa e recursos válidos | Automatizado | Aprovado |
| VT04 | Aplicar Tecnologico Escuro | Seleção recuperada com paleta completa e recursos válidos | Confirmado pelas asserções: Seleção recuperada com paleta completa e recursos válidos | Automatizado | Aprovado |
| VT05 | Aplicar Acolhedor Claro | Seleção recuperada com paleta completa e recursos válidos | Confirmado pelas asserções: Seleção recuperada com paleta completa e recursos válidos | Automatizado | Aprovado |
| VT06 | Aplicar Acolhedor Escuro | Seleção recuperada com paleta completa e recursos válidos | Confirmado pelas asserções: Seleção recuperada com paleta completa e recursos válidos | Automatizado | Aprovado |
| VT07 | Primeira instalação | Moderno claro sem gravação automática | Confirmado pelas asserções: Moderno claro sem gravação automática | Automatizado | Aprovado |
| VT08 | Preferência anterior válida | Tema e modo carregados sem reinicializar escolhas | Confirmado pelas asserções: Tema e modo carregados sem reinicializar escolhas | Automatizado | Aprovado |
| VT09 | Alternância repetida | Cada seleção válida permanece coerente | Confirmado pelas asserções: Cada seleção válida permanece coerente | Automatizado | Aprovado |
| VT10 | Tema inválido | Não aplica nem grava enum desconhecido | Confirmado pelas asserções: Não aplica nem grava enum desconhecido | Automatizado | Aprovado |
| VT11 | Modo inválido | Não altera tema nem grava modo desconhecido | Confirmado pelas asserções: Não altera tema nem grava modo desconhecido | Automatizado | Aprovado |
| VT12 | Falha de gravação durante seleção | Prévia mantém tema e informa persistência pendente | Confirmado pelas asserções: Prévia mantém tema e informa persistência pendente | Automatizado | Aprovado |

## Cores independentes e isolamento por tema e modo — 19 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| VI01 | Personalizar FundoPrincipal | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI02 | Personalizar FundoSidebar | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI03 | Personalizar TextoSidebar | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI04 | Personalizar FundoPainel | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI05 | Personalizar TextoPainel | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI06 | Personalizar FundoCartao | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI07 | Personalizar TextoCartao | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI08 | Personalizar FundoBotao | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI09 | Personalizar TextoBotao | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI10 | Personalizar Destaque | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI11 | Personalizar TextoDestaque | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI12 | Personalizar TextoPrincipal | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI13 | Personalizar TextoSecundario | Somente a escolha solicitada muda e demais escolhas permanecem | Confirmado pelas asserções: Somente a escolha solicitada muda e demais escolhas permanecem | Automatizado | Aprovado |
| VI14 | Isolar modo claro do escuro | Personalização clara reaparece ao voltar | Confirmado pelas asserções: Personalização clara reaparece ao voltar | Automatizado | Aprovado |
| VI15 | Isolar os seis conjuntos | Cada conjunto recupera sua própria escolha | Confirmado pelas asserções: Cada conjunto recupera sua própria escolha | Automatizado | Aprovado |
| VI16 | Reeditar um grupo após outro | Última escolha substitui apenas o próprio grupo | Confirmado pelas asserções: Última escolha substitui apenas o próprio grupo | Automatizado | Aprovado |
| VI17 | Cor arbitrária fora da paleta | Hexadecimal válido é aceito sem limitação temática | Confirmado pelas asserções: Hexadecimal válido é aceito sem limitação temática | Automatizado | Aprovado |
| VI18 | Grupo inexistente | Entrada inválida não grava | Confirmado pelas asserções: Entrada inválida não grava | Automatizado | Aprovado |
| VI19 | Snapshot anterior de personalizações | Alteração posterior não modifica snapshot anterior | Confirmado pelas asserções: Alteração posterior não modifica snapshot anterior | Automatizado | Aprovado |

## Hexadecimal: formato, normalização e entradas inválidas — 18 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| VH01 | Preto | Hexadecimal completo normalizado | Confirmado pelas asserções: Hexadecimal completo normalizado | Automatizado | Aprovado |
| VH02 | Branco | Hexadecimal completo normalizado | Confirmado pelas asserções: Hexadecimal completo normalizado | Automatizado | Aprovado |
| VH03 | Misto | Hexadecimal completo normalizado | Confirmado pelas asserções: Hexadecimal completo normalizado | Automatizado | Aprovado |
| VH04 | Espaços externos | Hexadecimal completo normalizado | Confirmado pelas asserções: Hexadecimal completo normalizado | Automatizado | Aprovado |
| VH05 | Fora da paleta | Hexadecimal completo normalizado | Confirmado pelas asserções: Hexadecimal completo normalizado | Automatizado | Aprovado |
| VH06 | Abreviado | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH07 | Transparência | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH08 | Nome de cor | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH09 | Vazio | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH10 | Sem cerquilha | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH11 | Dígito fora da faixa | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH12 | Incompleto | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH13 | Sete dígitos | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH14 | Espaço interno | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH15 | Pontuação | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH16 | Cerquilha não ASCII | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH17 | Dígitos não ASCII | Valor recusado e preferência anterior preservada | Confirmado pelas asserções: Valor recusado e preferência anterior preservada | Automatizado | Aprovado |
| VH18 | Nulo | Não é cor válida | Confirmado pelas asserções: Não é cor válida | Automatizado | Aprovado |

## Contraste, textos efetivos e indicadores derivados — 20 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| VC01 | Contraste pronto Moderno Claro | Todos os pares de texto e indicadores atendem aos mínimos | Confirmado pelas asserções: Todos os pares de texto e indicadores atendem aos mínimos | Automatizado | Aprovado |
| VC02 | Contraste pronto Moderno Escuro | Todos os pares de texto e indicadores atendem aos mínimos | Confirmado pelas asserções: Todos os pares de texto e indicadores atendem aos mínimos | Automatizado | Aprovado |
| VC03 | Contraste pronto Tecnologico Claro | Todos os pares de texto e indicadores atendem aos mínimos | Confirmado pelas asserções: Todos os pares de texto e indicadores atendem aos mínimos | Automatizado | Aprovado |
| VC04 | Contraste pronto Tecnologico Escuro | Todos os pares de texto e indicadores atendem aos mínimos | Confirmado pelas asserções: Todos os pares de texto e indicadores atendem aos mínimos | Automatizado | Aprovado |
| VC05 | Contraste pronto Acolhedor Claro | Todos os pares de texto e indicadores atendem aos mínimos | Confirmado pelas asserções: Todos os pares de texto e indicadores atendem aos mínimos | Automatizado | Aprovado |
| VC06 | Contraste pronto Acolhedor Escuro | Todos os pares de texto e indicadores atendem aos mínimos | Confirmado pelas asserções: Todos os pares de texto e indicadores atendem aos mínimos | Automatizado | Aprovado |
| VC07 | Branco sobre branco | Texto efetivo legível e preferência solicitada intacta | Confirmado pelas asserções: Texto efetivo legível e preferência solicitada intacta | Automatizado | Aprovado |
| VC08 | Preto sobre preto | Texto efetivo legível e preferência solicitada intacta | Confirmado pelas asserções: Texto efetivo legível e preferência solicitada intacta | Automatizado | Aprovado |
| VC09 | Azul sobre azul | Texto efetivo legível e preferência solicitada intacta | Confirmado pelas asserções: Texto efetivo legível e preferência solicitada intacta | Automatizado | Aprovado |
| VC10 | Dourado sobre rosa | Texto efetivo legível e preferência solicitada intacta | Confirmado pelas asserções: Texto efetivo legível e preferência solicitada intacta | Automatizado | Aprovado |
| VC11 | Cinzas iguais | Texto efetivo legível e preferência solicitada intacta | Confirmado pelas asserções: Texto efetivo legível e preferência solicitada intacta | Automatizado | Aprovado |
| VC12 | Par próximo do limite válido | Texto efetivo legível e preferência solicitada intacta | Confirmado pelas asserções: Texto efetivo legível e preferência solicitada intacta | Automatizado | Aprovado |
| VC13 | Fundo volta a permitir o texto escolhido | Cor solicitada reaplicada sem perdê-la | Confirmado pelas asserções: Cor solicitada reaplicada sem perdê-la | Automatizado | Aprovado |
| VC14 | Contraste matemático conhecido | Preto e branco têm razão 21 e mesma cor tem razão 1 | Confirmado pelas asserções: Preto e branco têm razão 21 e mesma cor tem razão 1 | Automatizado | Aprovado |
| VC15 | Hover e pressionado de botão customizado | Cada estado tem texto legível | Confirmado pelas asserções: Cada estado tem texto legível | Automatizado | Aprovado |
| VC16 | Destaque igual ao cartão | Indicador derivado contrasta sem apagar a escolha | Confirmado pelas asserções: Indicador derivado contrasta sem apagar a escolha | Automatizado | Aprovado |
| VC17 | Sucesso aviso e erro nos dois modos | Mensagens semânticas têm texto legível | Confirmado pelas asserções: Mensagens semânticas têm texto legível | Automatizado | Aprovado |
| VC18 | Texto secundário de baixo contraste | Aviso explícito e texto seguro | Confirmado pelas asserções: Aviso explícito e texto seguro | Automatizado | Aprovado |
| VC19 | Todos os fundos customizados de uma só cor | Textos gerais e de grupos continuam legíveis | Confirmado pelas asserções: Textos gerais e de grupos continuam legíveis | Automatizado | Aprovado |
| VC20 | Foco com destaque limítrofe e botão branco | Indicador contrasta nos estados normal hover e pressionado | Confirmado pelas asserções: Indicador contrasta nos estados normal hover e pressionado | Automatizado | Aprovado |

## Persistência, compatibilidade, concorrência e falhas — 27 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| VA01 | Arquivo ausente | Padrão seguro e nenhum arquivo criado | Confirmado pelas asserções: Padrão seguro e nenhum arquivo criado | Automatizado | Aprovado |
| VA02 | Tema modo e cor após reiniciar | Preferências solicitadas e cores efetivas recuperadas | Confirmado pelas asserções: Preferências solicitadas e cores efetivas recuperadas | Automatizado | Aprovado |
| VA03 | Preferência incompleta antiga | Campos válidos preservados e ausentes preenchidos | Confirmado pelas asserções: Campos válidos preservados e ausentes preenchidos | Automatizado | Aprovado |
| VA04 | Preferência com enum inválido | Inicialização usa padrão seguro sem sobrescrever arquivo | Confirmado pelas asserções: Inicialização usa padrão seguro sem sobrescrever arquivo | Automatizado | Aprovado |
| VA05 | Cores salvas parcialmente inválidas | Cores válidas permanecem e inválidas não são aplicadas | Confirmado pelas asserções: Cores válidas permanecem e inválidas não são aplicadas | Automatizado | Aprovado |
| VA06 | Arquivo JSON truncado | Aviso e padrão sem alterar bytes inválidos | Confirmado pelas asserções: Aviso e padrão sem alterar bytes inválidos | Automatizado | Aprovado |
| VA07 | Substituir preferência inválida explicitamente | Original fica no backup e nova preferência é válida | Confirmado pelas asserções: Original fica no backup e nova preferência é válida | Automatizado | Aprovado |
| VA08 | Versão futura | Abertura funciona e gravação fica bloqueada | Confirmado pelas asserções: Abertura funciona e gravação fica bloqueada | Automatizado | Aprovado |
| VA09 | Campos desconhecidos | Metadados e conjuntos futuros são preservados na gravação | Confirmado pelas asserções: Metadados e conjuntos futuros são preservados na gravação | Automatizado | Aprovado |
| VA10 | Arquivo mudou externamente | Não sobrescreve alteração de outra janela | Confirmado pelas asserções: Não sobrescreve alteração de outra janela | Automatizado | Aprovado |
| VA11 | Arquivo removido externamente | Detecta remoção e não recria silenciosamente | Confirmado pelas asserções: Detecta remoção e não recria silenciosamente | Automatizado | Aprovado |
| VA12 | Lock em uso | Falha controlada sem escrita concorrente | Confirmado pelas asserções: Falha controlada sem escrita concorrente | Automatizado | Aprovado |
| VA13 | Falha no arquivo de backup | Anterior permanece e temporário é limpo | Confirmado pelas asserções: Anterior permanece e temporário é limpo | Automatizado | Aprovado |
| VA14 | Recuperar após falha de backup | Nova gravação funciona sem recarregar nem perder modelo | Confirmado pelas asserções: Nova gravação funciona sem recarregar nem perder modelo | Automatizado | Aprovado |
| VA15 | Salvar sem carregar | Recusa escrita sem base conhecida | Confirmado pelas asserções: Recusa escrita sem base conhecida | Automatizado | Aprovado |
| VA16 | Falha de leitura por serviço simulado | Aplicativo usa Moderno claro e explica falha | Confirmado pelas asserções: Aplicativo usa Moderno claro e explica falha | Automatizado | Aprovado |
| VA17 | Arquivo nulo | Preferência inválida não impede abertura | Confirmado pelas asserções: Preferência inválida não impede abertura | Automatizado | Aprovado |
| VA18 | Objeto com conjuntos de tipo inesperado | Ignora somente configuração inválida e mantém tema válido | Confirmado pelas asserções: Ignora somente configuração inválida e mantém tema válido | Automatizado | Aprovado |
| VA19 | Biblioteca coleções e visualização vizinhas | Nenhum dos três arquivos de negócio é alterado | Confirmado pelas asserções: Nenhum dos três arquivos de negócio é alterado | Automatizado | Aprovado |
| VA20 | Persistir seis combinações | Todas as personalizações sobrevivem a nova instância | Confirmado pelas asserções: Todas as personalizações sobrevivem a nova instância | Automatizado | Aprovado |
| VA21 | Texto solicitado de baixo contraste após reiniciar | Solicitado e efetivo diferentes são reproduzidos sem perda | Confirmado pelas asserções: Solicitado e efetivo diferentes são reproduzidos sem perda | Automatizado | Aprovado |
| VA22 | UTF8 com BOM | Preferência válida é reconhecida | Confirmado pelas asserções: Preferência válida é reconhecida | Automatizado | Aprovado |
| VA23 | UTF16 com BOM | Preferência válida é reconhecida e regravada com backup | Confirmado pelas asserções: Preferência válida é reconhecida e regravada com backup | Automatizado | Aprovado |
| VA24 | Pasta ausente | Padrão carrega e falha ao salvar é informada | Confirmado pelas asserções: Padrão carrega e falha ao salvar é informada | Automatizado | Aprovado |
| VA25 | Conflito de preferências visto pelo serviço | Aviso orienta reabrir e preserva alteração externa | Confirmado pelas asserções: Aviso orienta reabrir e preserva alteração externa | Automatizado | Aprovado |
| VA26 | Chaves duplicadas no conjunto de cores | Abertura usa padrão e original fica preservado | Confirmado pelas asserções: Abertura usa padrão e original fica preservado | Automatizado | Aprovado |
| VA27 | Tema duplicado na raiz do JSON | Arquivo ambíguo não produz seleção arbitrária | Confirmado pelas asserções: Arquivo ambíguo não produz seleção arbitrária | Automatizado | Aprovado |

## Restauração ativa e global, persistência e isolamento — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| VR01 | Restaurar conjunto ativo | Cores originais reaparecem sem trocar tema nem modo | Confirmado pelas asserções: Cores originais reaparecem sem trocar tema nem modo | Automatizado | Aprovado |
| VR02 | Restaurar ativo com outro tema personalizado | Outro tema mantém sua cor | Confirmado pelas asserções: Outro tema mantém sua cor | Automatizado | Aprovado |
| VR03 | Restaurar ativo com outro modo personalizado | Outro modo mantém sua cor | Confirmado pelas asserções: Outro modo mantém sua cor | Automatizado | Aprovado |
| VR04 | Restaurar todas as cores do ativo | Treze cores voltam à paleta original | Confirmado pelas asserções: Treze cores voltam à paleta original | Automatizado | Aprovado |
| VR05 | Restaurar conjunto sem personalização | Ação idempotente preserva padrão | Confirmado pelas asserções: Ação idempotente preserva padrão | Automatizado | Aprovado |
| VR06 | Restaurar toda a aparência | Moderno claro e nenhum conjunto personalizado | Confirmado pelas asserções: Moderno claro e nenhum conjunto personalizado | Automatizado | Aprovado |
| VR07 | Repetir restauração global | Não introduz personalização nem falha | Confirmado pelas asserções: Não introduz personalização nem falha | Automatizado | Aprovado |
| VR08 | Restauração ativa persiste | Reabrir recupera paleta original do conjunto | Confirmado pelas asserções: Reabrir recupera paleta original do conjunto | Automatizado | Aprovado |
| VR09 | Restauração global persiste | Reabrir recupera Moderno claro sem cores pessoais | Confirmado pelas asserções: Reabrir recupera Moderno claro sem cores pessoais | Automatizado | Aprovado |
| VR10 | Falha ao persistir restauração | Prévia usa padrão e avisa gravação pendente | Confirmado pelas asserções: Prévia usa padrão e avisa gravação pendente | Automatizado | Aprovado |
| VR11 | Restaurar conserva metadados desconhecidos | Arquivo não é apagado integralmente | Confirmado pelas asserções: Arquivo não é apagado integralmente | Automatizado | Aprovado |
| VR12 | Restaurar remove somente ajustes de contraste do ativo | Aviso desaparece e cor original retorna | Confirmado pelas asserções: Aviso desaparece e cor original retorna | Automatizado | Aprovado |

## Prévia imediata, paletas, identidade e navegação Avalonia — 23 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| VP01 | Prévia Avalonia Moderno Claro | Janela controles e captura refletem a combinação em tempo real | Confirmado pelas asserções: Janela controles e captura refletem a combinação em tempo real | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP02 | Prévia Avalonia Moderno Escuro | Janela controles e captura refletem a combinação em tempo real | Confirmado pelas asserções: Janela controles e captura refletem a combinação em tempo real | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP03 | Prévia Avalonia Tecnologico Claro | Janela controles e captura refletem a combinação em tempo real | Confirmado pelas asserções: Janela controles e captura refletem a combinação em tempo real | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP04 | Prévia Avalonia Tecnologico Escuro | Janela controles e captura refletem a combinação em tempo real | Confirmado pelas asserções: Janela controles e captura refletem a combinação em tempo real | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP05 | Prévia Avalonia Acolhedor Claro | Janela controles e captura refletem a combinação em tempo real | Confirmado pelas asserções: Janela controles e captura refletem a combinação em tempo real | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP06 | Prévia Avalonia Acolhedor Escuro | Janela controles e captura refletem a combinação em tempo real | Confirmado pelas asserções: Janela controles e captura refletem a combinação em tempo real | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP07 | Cor do fundo principal em tempo real | Brush da janela atualiza sem recriar janela | Confirmado pelas asserções: Brush da janela atualiza sem recriar janela | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP08 | Texto incompleto durante digitação | Mantém última cor e não grava estado intermediário | Confirmado pelas asserções: Mantém última cor e não grava estado intermediário | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP09 | Paleta sincroniza campo e amostra | Escolha pronta aplica código e prévia correspondente | Confirmado pelas asserções: Escolha pronta aplica código e prévia correspondente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP10 | Hexadecimal arbitrário sincroniza amostra | Cor fora da paleta é mostrada e normalizada | Confirmado pelas asserções: Cor fora da paleta é mostrada e normalizada | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP11 | Aviso de contraste na própria tela | Ajuste explícito mantém campo solicitado e texto efetivo legível | Confirmado pelas asserções: Ajuste explícito mantém campo solicitado e texto efetivo legível | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP12 | Prévia com formulário preenchido | Título review e navegação não são perdidos | Confirmado pelas asserções: Título review e navegação não são perdidos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP13 | Nova janela recebe tema carregado | Preferência persistida é aplicada ao abrir novamente | Confirmado pelas asserções: Preferência persistida é aplicada ao abrir novamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP14 | Janela já aberta atualiza recursos compartilhados | Troca em segunda janela chega à primeira janela | Confirmado pelas asserções: Troca em segunda janela chega à primeira janela | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP15 | Falha ao salvar pela interface | Prévia funciona e mensagem não afirma sucesso | Confirmado pelas asserções: Prévia funciona e mensagem não afirma sucesso | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP16 | Retry de gravação pela interface | Grava preferências atuais e encerra estado pendente | Confirmado pelas asserções: Grava preferências atuais e encerra estado pendente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP17 | Restaurar global exige confirmação | Solicitar não altera as cores antes de confirmar | Confirmado pelas asserções: Solicitar não altera as cores antes de confirmar | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP18 | Cancelar restauração global | Mantém as escolhas e fecha confirmação | Confirmado pelas asserções: Mantém as escolhas e fecha confirmação | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP19 | Confirmar restauração global | Moderno claro é aplicado imediatamente | Confirmado pelas asserções: Moderno claro é aplicado imediatamente | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP20 | Restaurar ativo pela interface | Campos e amostra retornam aos padrões e outro modo permanece | Confirmado pelas asserções: Campos e amostra retornam aos padrões e outro modo permanece | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP21 | Nome público e recursos técnicos | Interface usa nome aprovado sem slogan ou troca de namespace | Confirmado pelas asserções: Interface usa nome aprovado sem slogan ou troca de namespace | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP22 | Aparência no tamanho mínimo | Todos os grupos ficam alcançáveis pela rolagem | Confirmado pelas asserções: Todos os grupos ficam alcançáveis pela rolagem | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VP23 | Prévia mantém biblioteca e coleções | Aparência não grava registros nem seus vínculos | Confirmado pelas asserções: Aparência não grava registros nem seus vínculos | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Estados visuais: foco, hover, pressionado, erro e seleção — 17 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| VE01 | Botão normal | Texto efetivo contrasta com fundo personalizado | Confirmado pelas asserções: Texto efetivo contrasta com fundo personalizado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE02 | Botão com hover | Estado muda e texto mantém contraste | Confirmado pelas asserções: Estado muda e texto mantém contraste | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE03 | Botão pressionado | Estado pressionado mantém contraste | Confirmado pelas asserções: Estado pressionado mantém contraste | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE04 | Foco por teclado no botão | Indicador e texto permanecem visíveis | Confirmado pelas asserções: Indicador e texto permanecem visíveis | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE05 | Botão desabilitado | Texto identificável e controle permanece desabilitado | Confirmado pelas asserções: Texto identificável e controle permanece desabilitado | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE06 | Seleção de texto no formulário | Texto selecionado contrasta com destaque | Confirmado pelas asserções: Texto selecionado contrasta com destaque | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE07 | Erro hexadecimal | Mensagem textual e preview anterior preservados | Confirmado pelas asserções: Mensagem textual e preview anterior preservados | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE08 | Estado vazio da biblioteca | Orientação continua legível nos dois modos | Confirmado pelas asserções: Orientação continua legível nos dois modos | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE09 | Seleção com destaque igual ao cartão | Barra permanece visível sobre o cartão | Confirmado pelas asserções: Barra permanece visível sobre o cartão | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE10 | Aviso de sucesso | Mensagem mantém contraste com superfície semântica | Confirmado pelas asserções: Mensagem mantém contraste com superfície semântica | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE11 | Aviso de falha | Ícone texto e mensagem de erro continuam presentes | Confirmado pelas asserções: Ícone texto e mensagem de erro continuam presentes | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE12 | Menu contextual existente | Menu aberto recebe superfície e texto do tema | Confirmado pelas asserções: Menu aberto recebe superfície e texto do tema | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE13 | Abrir editores sem editar | Exibir padrões não provoca gravações | Confirmado pelas asserções: Exibir padrões não provoca gravações | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE14 | Texto do cartão com fundo independente | Não herda indevidamente o texto do painel | Confirmado pelas asserções: Não herda indevidamente o texto do painel | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE15 | Corrigir entrada voltando à cor anterior | Mensagem inválida desaparece sem gravação redundante | Confirmado pelas asserções: Mensagem inválida desaparece sem gravação redundante | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE16 | Foco de teclado com mouse sobre botão | Hover e foco simultâneos mantêm indicador visível | Confirmado pelas asserções: Hover e foco simultâneos mantêm indicador visível | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VE17 | Texto de seleção com destaque limítrofe | Texto pequeno atende a 4.5 mesmo quando indicador aceita 3 | Confirmado pelas asserções: Texto pequeno atende a 4.5 mesmo quando indicador aceita 3 | Automatizado (Avalonia Headless/Skia) | Aprovado |

## Personalizações representativas e compatibilidade dos componentes — 13 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| VF01 | Personalização azul em todas as superfícies | Cores independentes chegam à janela barra e cartões | Confirmado pelas asserções: Cores independentes chegam à janela barra e cartões | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF02 | Personalização inteiramente acinzentada | Interface funciona sem cores obrigatórias dos presets | Confirmado pelas asserções: Interface funciona sem cores obrigatórias dos presets | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF03 | Personalização preta | Preto válido aplicado com textos seguros | Confirmado pelas asserções: Preto válido aplicado com textos seguros | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF04 | Personalização rosada fora dos temas | Escolhas arbitrárias preservadas com contraste | Confirmado pelas asserções: Escolhas arbitrárias preservadas com contraste | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF05 | Personalização quente independente | Painéis e cartões usam fundos próprios | Confirmado pelas asserções: Painéis e cartões usam fundos próprios | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF06 | Todas as escolhas com contraste insuficiente | Preferências mantidas e textos efetivos legíveis | Confirmado pelas asserções: Preferências mantidas e textos efetivos legíveis | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF07 | Lista de sugestões com painel personalizado | Não combina texto da página com fundo do controle | Confirmado pelas asserções: Não combina texto da página com fundo do controle | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF08 | Menu suspenso com painel personalizado | Itens não selecionados recebem texto e fundo coerentes | Confirmado pelas asserções: Itens não selecionados recebem texto e fundo coerentes | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF09 | Seleção branca com texto branco solicitado | Seleção efetiva mantém contraste e preserva a solicitação | Confirmado pelas asserções: Seleção efetiva mantém contraste e preserva a solicitação | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF10 | Fallback de capa após personalizar cartão | Texto e fundo do fallback acompanham cartão | Confirmado pelas asserções: Texto e fundo do fallback acompanham cartão | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF11 | Trocar presets sem editar | Preenchimento dos campos não cria personalizações | Confirmado pelas asserções: Preenchimento dos campos não cria personalizações | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF12 | Preferência anterior de lista | Aparência não altera configuração de visualização | Confirmado pelas asserções: Aparência não altera configuração de visualização | Automatizado (Avalonia Headless/Skia) | Aprovado |
| VF13 | Preferência JSON aplicada em nova janela | Tema modo e código arbitrário reaparecem após fechar | Confirmado pelas asserções: Tema modo e código arbitrário reaparecem após fechar | Automatizado (Avalonia Headless/Skia) | Aprovado |

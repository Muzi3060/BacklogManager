# Fase 0 — Resultados individuais

Gerado a partir do TRX real pelo script `scripts/gerar_relatorio_testes.py`. Cada aprovação significa que as asserções do cenário foram executadas sem falha; não representa validação autenticada da API ou inspeção manual no Rider.

Total da suíte: **175**. Aprovados: **175**. Reprovados: **0**. Não executados: **0**.

Os testes usam pastas temporárias próprias e HTTP simulado. Nenhum teste da suíte lê o JSON pessoal.

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

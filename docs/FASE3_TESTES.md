# Fase 3 — Resultados individuais

Gerado do TRX real por `scripts/gerar_relatorio_fase3.py`. Inclui todos os 535 cenários anteriores executados novamente sobre a Fase 3. H01 e Q04–Q10 foram adaptados aos novos campos opcionais solicitados à IGDB, preservando as verificações de protocolo e escape. Nenhum cenário foi removido; os relatórios históricos continuam intactos.

**Planejados na suíte: 715; executados: 715; aprovados: 715; reprovados: 0; não executados: 0.**

Aprovação corresponde às asserções executadas. A suíte usa dados simulados, arquivos temporários e HTTP simulado; não usa a biblioteca original. Não representa nova validação autenticada da IGDB ou interação manual no Rider. Verificações adicionais e pendências constam em `FASE3.md`.

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
| UM10 | Campo inteiro recebe fração | Aviso de progresso inválido sem truncamento ou encerramento | Confirmado pelas asserções: Aviso de progresso inválido sem truncamento ou encerramento | Automatizado (Avalonia Headless/Skia) | Aprovado |
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
| UM22 | Mangá com capítulo e volume | Campos próprios preservados incluindo zero | Confirmado pelas asserções: Campos próprios preservados incluindo zero | Automatizado (Avalonia Headless/Skia) | Aprovado |
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

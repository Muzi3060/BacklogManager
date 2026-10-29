# Beta — Etapa 1: testes reais

Gerado por `scripts/gerar_relatorio_beta_etapa1.py` a partir dos TRX reais. Somente arquivos sintéticos/temporários e HTTP simulado. Nenhuma chamada autenticada à IGDB nem teste na biblioteca pessoal.

Referência anterior: **1551 testes**. Suíte beta: **1541 planejados/executados**, **1541 aprovados**, **0 reprovados**, **0 não executados**. São 95 novos e 1446 de regressão mantidos/adaptados.

Os 105 cenários da versão anterior retirados por remoção expressa das funcionalidades estão individualmente identificados em [beta-etapa1-transicao-testes.json](beta-etapa1-transicao-testes.json). Eles passaram na referência anterior; não são contados como aprovados ou ignorados na beta. A contabilidade é **1551 − 105 + 95 = 1541**.

Resultados individuais da regressão, horários e hashes dos TRX: [resultados-beta-etapa1.json](resultados-beta-etapa1.json). Os relatórios anteriores permanecem históricos, sem alteração.

## Navegação simplificada — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| BN01 | Início na navegação | Página correta e seleção ativa | Confirmado pelas asserções: Página correta e seleção ativa | Automatizado | Aprovado |
| BN02 | Biblioteca na navegação | Página correta e seleção ativa | Confirmado pelas asserções: Página correta e seleção ativa | Automatizado | Aprovado |
| BN03 | Coleções na navegação | Página correta e seleção ativa | Confirmado pelas asserções: Página correta e seleção ativa | Automatizado | Aprovado |
| BN04 | Adicionar editar na navegação | Página correta e seleção ativa | Confirmado pelas asserções: Página correta e seleção ativa | Automatizado | Aprovado |
| BN05 | Configurações no rodapé | Botão ancorado abaixo dos links principais | Confirmado pelas asserções: Botão ancorado abaixo dos links principais | Automatizado | Aprovado |
| BN06 | Rodapé em janela mínima | Configurações visíveis dentro da janela | Confirmado pelas asserções: Configurações visíveis dentro da janela | Automatizado | Aprovado |
| BN07 | Teclado no rodapé | Foco por Tab chega a Configurações | Confirmado pelas asserções: Foco por Tab chega a Configurações | Automatizado | Aprovado |
| BN08 | Navegar e voltar | Somente a página escolhida permanece visível | Confirmado pelas asserções: Somente a página escolhida permanece visível | Automatizado | Aprovado |
| BN09 | Remoção do link Aparência | Nenhuma rota ou editor independente | Confirmado pelas asserções: Nenhuma rota ou editor independente | Automatizado | Aprovado |
| BN10 | Rodapé sem indicador antigo | Dados neste computador e Armazenamento local ausentes | Confirmado pelas asserções: Dados neste computador e Armazenamento local ausentes | Automatizado | Aprovado |
| BN11 | Rascunho durante navegação | Review editada preservada ao abrir configurações | Confirmado pelas asserções: Review editada preservada ao abrir configurações | Automatizado | Aprovado |
| BN12 | Navegação local sem IGDB | Biblioteca e configurações disponíveis sem credenciais | Confirmado pelas asserções: Biblioteca e configurações disponíveis sem credenciais | Automatizado | Aprovado |

## Configurações e preservação do backup — 15 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| BC01 | Temas diretos | Quatro opções aprovadas nas configurações | Confirmado pelas asserções: Quatro opções aprovadas nas configurações | Automatizado | Aprovado |
| BC02 | Modos diretos | Claro e Escuro preservados | Confirmado pelas asserções: Claro e Escuro preservados | Automatizado | Aprovado |
| BC03 | Padrão sem preferência | Moderno claro sem gravação automática | Confirmado pelas asserções: Moderno claro sem gravação automática | Automatizado | Aprovado |
| BC04 | Aplicação imediata | Mudar tema atualiza a mesma janela | Confirmado pelas asserções: Mudar tema atualiza a mesma janela | Automatizado | Aprovado |
| BC05 | Editor removido | Nenhum campo de cores ou reset acessível | Confirmado pelas asserções: Nenhum campo de cores ou reset acessível | Automatizado | Aprovado |
| BC06 | Grade e lista na biblioteca | Preferência continua disponível no local próprio | Confirmado pelas asserções: Preferência continua disponível no local próprio | Automatizado | Aprovado |
| BC07 | Configurações na janela mínima | Rolagem alcança backup e rodapé sem corte lateral | Confirmado pelas asserções: Rolagem alcança backup e rodapé sem corte lateral | Automatizado | Aprovado |
| BC08 | Acessibilidade dos seletores | Tema e modo possuem nomes e foco de teclado | Confirmado pelas asserções: Tema e modo possuem nomes e foco de teclado | Automatizado | Aprovado |
| BC09 | Exportação em Dark Mode | Backup inclui dados e preferência versão dois | Confirmado pelas asserções: Backup inclui dados e preferência versão dois | Automatizado | Aprovado |
| BC10 | Restauração só dos dados | Tema da sessão preservado e dados substituídos | Confirmado pelas asserções: Tema da sessão preservado e dados substituídos | Automatizado | Aprovado |
| BC11 | Restauração com preferências | Preferência escolhida no backup aparece nos seletores | Confirmado pelas asserções: Preferência escolhida no backup aparece nos seletores | Automatizado | Aprovado |
| BC12 | Restauração exige consentimento | Prévia não altera dados nem habilita escrita sem confirmação | Confirmado pelas asserções: Prévia não altera dados nem habilita escrita sem confirmação | Automatizado | Aprovado |
| BC13 | Recuperação pendente | Seletores bloqueados e original de aparência preservado | Confirmado pelas asserções: Seletores bloqueados e original de aparência preservado | Automatizado | Aprovado |
| BC14 | Falha e nova tentativa de salvar | Aviso honesto e escolha atual persistida ao tentar novamente | Confirmado pelas asserções: Aviso honesto e escolha atual persistida ao tentar novamente | Automatizado | Aprovado |
| BC15 | Restaurar preferência com seleção pendente | Recarga elimina aviso de gravação antiga e usa tema restaurado | Confirmado pelas asserções: Recarga elimina aviso de gravação antiga e usa tema restaurado | Automatizado | Aprovado |

## Quatro temas fixos e dois modos — 24 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| BT01 | Moderno Claro persistido | Reabrir recupera tema e modo em arquivo isolado | Confirmado pelas asserções: Reabrir recupera tema e modo em arquivo isolado | Automatizado | Aprovado |
| BT02 | Moderno Claro contraste | Textos indicadores foco estados e mensagens legíveis | Confirmado pelas asserções: Textos indicadores foco estados e mensagens legíveis | Automatizado | Aprovado |
| BT03 | Moderno Claro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo | Confirmado pelas asserções: Recursos ativos sem editor e capturas no tamanho mínimo | Automatizado | Aprovado |
| BT04 | Moderno Escuro persistido | Reabrir recupera tema e modo em arquivo isolado | Confirmado pelas asserções: Reabrir recupera tema e modo em arquivo isolado | Automatizado | Aprovado |
| BT05 | Moderno Escuro contraste | Textos indicadores foco estados e mensagens legíveis | Confirmado pelas asserções: Textos indicadores foco estados e mensagens legíveis | Automatizado | Aprovado |
| BT06 | Moderno Escuro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo | Confirmado pelas asserções: Recursos ativos sem editor e capturas no tamanho mínimo | Automatizado | Aprovado |
| BT07 | Tecnologico Claro persistido | Reabrir recupera tema e modo em arquivo isolado | Confirmado pelas asserções: Reabrir recupera tema e modo em arquivo isolado | Automatizado | Aprovado |
| BT08 | Tecnologico Claro contraste | Textos indicadores foco estados e mensagens legíveis | Confirmado pelas asserções: Textos indicadores foco estados e mensagens legíveis | Automatizado | Aprovado |
| BT09 | Tecnologico Claro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo | Confirmado pelas asserções: Recursos ativos sem editor e capturas no tamanho mínimo | Automatizado | Aprovado |
| BT10 | Tecnologico Escuro persistido | Reabrir recupera tema e modo em arquivo isolado | Confirmado pelas asserções: Reabrir recupera tema e modo em arquivo isolado | Automatizado | Aprovado |
| BT11 | Tecnologico Escuro contraste | Textos indicadores foco estados e mensagens legíveis | Confirmado pelas asserções: Textos indicadores foco estados e mensagens legíveis | Automatizado | Aprovado |
| BT12 | Tecnologico Escuro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo | Confirmado pelas asserções: Recursos ativos sem editor e capturas no tamanho mínimo | Automatizado | Aprovado |
| BT13 | Acolhedor Claro persistido | Reabrir recupera tema e modo em arquivo isolado | Confirmado pelas asserções: Reabrir recupera tema e modo em arquivo isolado | Automatizado | Aprovado |
| BT14 | Acolhedor Claro contraste | Textos indicadores foco estados e mensagens legíveis | Confirmado pelas asserções: Textos indicadores foco estados e mensagens legíveis | Automatizado | Aprovado |
| BT15 | Acolhedor Claro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo | Confirmado pelas asserções: Recursos ativos sem editor e capturas no tamanho mínimo | Automatizado | Aprovado |
| BT16 | Acolhedor Escuro persistido | Reabrir recupera tema e modo em arquivo isolado | Confirmado pelas asserções: Reabrir recupera tema e modo em arquivo isolado | Automatizado | Aprovado |
| BT17 | Acolhedor Escuro contraste | Textos indicadores foco estados e mensagens legíveis | Confirmado pelas asserções: Textos indicadores foco estados e mensagens legíveis | Automatizado | Aprovado |
| BT18 | Acolhedor Escuro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo | Confirmado pelas asserções: Recursos ativos sem editor e capturas no tamanho mínimo | Automatizado | Aprovado |
| BT19 | DarkMode Claro persistido | Reabrir recupera tema e modo em arquivo isolado | Confirmado pelas asserções: Reabrir recupera tema e modo em arquivo isolado | Automatizado | Aprovado |
| BT20 | DarkMode Claro contraste | Textos indicadores foco estados e mensagens legíveis | Confirmado pelas asserções: Textos indicadores foco estados e mensagens legíveis | Automatizado | Aprovado |
| BT21 | DarkMode Claro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo | Confirmado pelas asserções: Recursos ativos sem editor e capturas no tamanho mínimo | Automatizado | Aprovado |
| BT22 | DarkMode Escuro persistido | Reabrir recupera tema e modo em arquivo isolado | Confirmado pelas asserções: Reabrir recupera tema e modo em arquivo isolado | Automatizado | Aprovado |
| BT23 | DarkMode Escuro contraste | Textos indicadores foco estados e mensagens legíveis | Confirmado pelas asserções: Textos indicadores foco estados e mensagens legíveis | Automatizado | Aprovado |
| BT24 | DarkMode Escuro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo | Confirmado pelas asserções: Recursos ativos sem editor e capturas no tamanho mínimo | Automatizado | Aprovado |

## Compatibilidade e persistência da aparência — 16 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| BP01 | Preferências ausentes | Padrão seguro sem criar arquivo | Confirmado pelas asserções: Padrão seguro sem criar arquivo | Automatizado | Aprovado |
| BP02 | Tema e modo antigos | Versão um lida sem migrar automaticamente | Confirmado pelas asserções: Versão um lida sem migrar automaticamente | Automatizado | Aprovado |
| BP03 | Cores legadas válidas | Cor antiga não sobrepõe tema fixo e bytes são preservados | Confirmado pelas asserções: Cor antiga não sobrepõe tema fixo e bytes são preservados | Automatizado | Aprovado |
| BP04 | Cores legadas opacas | Valores e extensões desconhecidos sobrevivem à seleção explícita | Confirmado pelas asserções: Valores e extensões desconhecidos sobrevivem à seleção explícita | Automatizado | Aprovado |
| BP05 | Preferência incompleta | Tema preservado e modo ausente usa Claro | Confirmado pelas asserções: Tema preservado e modo ausente usa Claro | Automatizado | Aprovado |
| BP06 | Tema desconhecido | Fallback não regrava arquivo | Confirmado pelas asserções: Fallback não regrava arquivo | Automatizado | Aprovado |
| BP07 | Dark Mode versão dois | Reabertura reconhece seleção e paleta neutra | Confirmado pelas asserções: Reabertura reconhece seleção e paleta neutra | Automatizado | Aprovado |
| BP08 | Versão futura | Leitura conservadora e gravação bloqueada | Confirmado pelas asserções: Leitura conservadora e gravação bloqueada | Automatizado | Aprovado |
| BP09 | Alteração externa | Conflito impede sobrescrever preferência | Confirmado pelas asserções: Conflito impede sobrescrever preferência | Automatizado | Aprovado |
| BP10 | JSON inválido | Aviso sem sobrescrita na abertura | Confirmado pelas asserções: Aviso sem sobrescrita na abertura | Automatizado | Aprovado |
| BP11 | Salvar nova preferência | Versão dois e backup do documento anterior | Confirmado pelas asserções: Versão dois e backup do documento anterior | Automatizado | Aprovado |
| BP12 | Backup de preferências antigas | Exportar e restaurar preserva cores como conteúdo inativo | Confirmado pelas asserções: Exportar e restaurar preserva cores como conteúdo inativo | Automatizado | Aprovado |
| BP13 | Backup Dark Mode | Documento versão dois passa validação e restauração | Confirmado pelas asserções: Documento versão dois passa validação e restauração | Automatizado | Aprovado |
| BP14 | Preferência futura no backup | Dados restauráveis sem aceitar preferência futura | Confirmado pelas asserções: Dados restauráveis sem aceitar preferência futura | Automatizado | Aprovado |
| BP15 | Temas não alteram arquivos de dados | Biblioteca coleções e preferência de lista mantêm bytes | Confirmado pelas asserções: Biblioteca coleções e preferência de lista mantêm bytes | Automatizado | Aprovado |
| BP16 | Reabertura da janela | Tema fixo salvo reaparece sem iniciar no padrão | Confirmado pelas asserções: Tema fixo salvo reaparece sem iniciar no padrão | Automatizado | Aprovado |

## Dashboard simplificado — 16 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| BD01 | Dashboard vazio | Totais zerados com convite útil sem blocos removidos | Confirmado pelas asserções: Totais zerados com convite útil sem blocos removidos | Automatizado | Aprovado |
| BD02 | Sem data na biblioteca | Registro sem data contado e preservado sem seção extra | Confirmado pelas asserções: Registro sem data contado e preservado sem seção extra | Automatizado | Aprovado |
| BD03 | Andamento sem data | Obra continua no acompanhamento sem inventar data | Confirmado pelas asserções: Obra continua no acompanhamento sem inventar data | Automatizado | Aprovado |
| BD04 | Atividade sete dias | Adição antiga excluída sem afetar total | Confirmado pelas asserções: Adição antiga excluída sem afetar total | Automatizado | Aprovado |
| BD05 | Atividade trinta dias | Adições reais do intervalo exibidas | Confirmado pelas asserções: Adições reais do intervalo exibidas | Automatizado | Aprovado |
| BD06 | Data futura | Fora da atividade sem apagar registro | Confirmado pelas asserções: Fora da atividade sem apagar registro | Automatizado | Aprovado |
| BD07 | Distribuição multimídia | Seis mídias mantêm suas quantidades | Confirmado pelas asserções: Seis mídias mantêm suas quantidades | Automatizado | Aprovado |
| BD08 | Status locais | Resumo mantém fila andamento e conclusão | Confirmado pelas asserções: Resumo mantém fila andamento e conclusão | Automatizado | Aprovado |
| BD09 | Edição no acompanhamento | Abre formulário existente sem duplicar obra | Confirmado pelas asserções: Abre formulário existente sem duplicar obra | Automatizado | Aprovado |
| BD10 | Progresso não informado | Sem percentual pessoal fabricado | Confirmado pelas asserções: Sem percentual pessoal fabricado | Automatizado | Aprovado |
| BD11 | Textos simplificados | Sem data próxima obra e explicações extensas removidos | Confirmado pelas asserções: Sem data próxima obra e explicações extensas removidos | Automatizado | Aprovado |
| BD12 | Estatísticas independem dos filtros | Filtro de mídia não reduz Dashboard | Confirmado pelas asserções: Filtro de mídia não reduz Dashboard | Automatizado | Aprovado |
| BD13 | Janela mínima e teclado | Período focável e conteúdo final alcançável | Confirmado pelas asserções: Período focável e conteúdo final alcançável | Automatizado | Aprovado |
| BD14 | Falha de leitura preservada | Arquivo inválido não vira biblioteca vazia | Confirmado pelas asserções: Arquivo inválido não vira biblioteca vazia | Automatizado | Aprovado |
| BD15 | Consulta local sem escrita | Trocar período e navegar não grava nem chama IGDB | Confirmado pelas asserções: Trocar período e navegar não grava nem chama IGDB | Automatizado | Aprovado |
| BD16 | Legado sem identificador | Título SteamAppId e categoria antigos preservados | Confirmado pelas asserções: Título SteamAppId e categoria antigos preservados | Automatizado | Aprovado |

## Ausência de promoções nos fluxos da beta — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| BZ01 | Início com obras sem promoções | Nenhum anúncio preço oferta ou navegação comercial | Confirmado pelas asserções: Nenhum anúncio preço oferta ou navegação comercial | Automatizado | Aprovado |
| BZ02 | Biblioteca em grade sem promoções | Nenhum anúncio preço oferta ou navegação comercial | Confirmado pelas asserções: Nenhum anúncio preço oferta ou navegação comercial | Automatizado | Aprovado |
| BZ03 | Coleções sem promoções | Nenhum anúncio preço oferta ou navegação comercial | Confirmado pelas asserções: Nenhum anúncio preço oferta ou navegação comercial | Automatizado | Aprovado |
| BZ04 | Cadastro sem promoções | Nenhum anúncio preço oferta ou navegação comercial | Confirmado pelas asserções: Nenhum anúncio preço oferta ou navegação comercial | Automatizado | Aprovado |
| BZ05 | Configurações sem promoções | Nenhum anúncio preço oferta ou navegação comercial | Confirmado pelas asserções: Nenhum anúncio preço oferta ou navegação comercial | Automatizado | Aprovado |
| BZ06 | Biblioteca em lista | Visualização alternativa sem conteúdo comercial | Confirmado pelas asserções: Visualização alternativa sem conteúdo comercial | Automatizado | Aprovado |
| BZ07 | Página inicial vazia | Convites locais sem reserva de promoções | Confirmado pelas asserções: Convites locais sem reserva de promoções | Automatizado | Aprovado |
| BZ08 | Sem credenciais | Aviso IGDB preservado sem promoções | Confirmado pelas asserções: Aviso IGDB preservado sem promoções | Automatizado | Aprovado |
| BZ09 | Edição pessoal | Formulário não expõe preços nem altera a obra ao abrir | Confirmado pelas asserções: Formulário não expõe preços nem altera a obra ao abrir | Automatizado | Aprovado |
| BZ10 | Filtro de status Na Fila | Fila continua status local sem lista de desejos | Confirmado pelas asserções: Fila continua status local sem lista de desejos | Automatizado | Aprovado |
| BZ11 | Backup local | Prévia e confirmação sem serviços comerciais | Confirmado pelas asserções: Prévia e confirmação sem serviços comerciais | Automatizado | Aprovado |
| BZ12 | Biblioteca indisponível | Erro local sem ofertas simuladas ou requisições | Confirmado pelas asserções: Erro local sem ofertas simuladas ou requisições | Automatizado | Aprovado |

## Transição e regressão

A troca de `NavAparencia` por `NavConfiguracoes` nos auxiliares e casos existentes usa o fluxo real da beta, sem controles fictícios nem aliases para a página removida. Os casos mantidos de temas, mensagens, teclado, formulários, grade/lista, capas, coleções, IGDB simulada, persistência e recuperação foram reexecutados.

Adaptações: VH06–VH17 exercitam o validador hexadecimal interno usado pelas paletas fixas, sem editor público; VA05 verifica cores legadas inativas; QN07/QN13/QN14 verificam seletores diretos, Dark Mode e ausência do seletor redundante; QP09 restaura tema/modo fixos; QI04/QI09 usam o seletor real de visualização da biblioteca; QI18/QI19 preservam uma seleção de tema pendente quando a restauração não inclui aparência; QH14 usa contraste de Dark Mode. DU11/DU18/DU19 e DK11 cobrem ausência do bloco legado e de promoções, com os dados preservados. TX02 remove o convite dispensado; TX05/TX12 usam obras em andamento; RD12 mantém capturas de grade/lista sem a seção removida.

## Execuções intermediárias e correções

- Referência: 1551 aprovados, zero falhas/ignorados.
- Primeira regressão adaptada: 1444 aprovados e 2 falhas (TX02 ainda exigia o convite removido; TX05 precisava conservar os demais dados pessoais do fixture e acessar o acompanhamento). Testes adaptados ao requisito e repetidos.
- Primeiros 94 novos cenários: 93 aprovados e 1 falha de fixture (BD02 pressupunha um bloco multimídia obrigatório em jogo legado). A asserção agora aceita bloco ausente, sem inventar data.
- Reprodução dirigida: BC15 falhou e outros três casos passaram. Bug confirmado: depois de restaurar aparência, `PendenteSalvar` ainda refletia a seleção anterior. `ServicoAparencia.Carregar()` agora limpa essa pendência ao substituir a sessão.
- Build final da solução: zero avisos e zero erros. Suíte completa final: 1541 aprovados, incluindo BC15, BD02, TX02 e TX05.
- Uma tentativa inicial de iniciar MSBuild no sandbox não executou cenários por falta de permissão para pipes locais; a execução foi repetida com as permissões de processo necessárias. Isso não foi registrado como teste aprovado.

## Validação complementar e limites

Há **cinco cenários manuais planejados e ainda não executados**, descritos em [BETA_ETAPA1.md](BETA_ETAPA1.md). A aprovação manual desta etapa está pendente. Capturas automatizadas não equivalem à interação manual no Rider nem à validação de pacotes Windows/Linux.

Os testes verificam contraste matemático, recursos aplicados e controles renderizados, além da regressão visual existente. Não constituem certificação completa de acessibilidade ou teste de todos os leitores de tela. A persistência entre vários arquivos mantém as limitações já documentadas na Fase 7.

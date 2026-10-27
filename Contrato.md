# CONTRATO DE DESENVOLVIMENTO — BACKLOGMANAGER

**Versão:** 1.1
**Status:** Planejamento aprovado; implementação pendente
**Projeto atual:** BacklogManager
**Tecnologias atuais:** C#, .NET, Avalonia UI, JSON, HTTP e IGDB
**Idioma principal:** Português brasileiro

---

# 1. OBJETIVO DESTE DOCUMENTO

Este documento estabelece as regras, decisões, funcionalidades e etapas de desenvolvimento que o Codex deverá seguir na evolução do BacklogManager.

O projeto começou como um gerenciador pessoal de jogos, mas evoluirá para uma plataforma de organização e acompanhamento de entretenimento, incluindo:

- Jogos
- Animes
- Séries
- Filmes
- Mangás
- Livros

O aplicativo deverá permitir organizar obras, registrar experiências pessoais, acompanhar progresso, criar coleções, personalizar a interface e, futuramente, sincronizar dados entre dispositivos.

**O Codex não está autorizado a executar todas as etapas automaticamente.**

Cada fase deve ser implementada isoladamente, testada, documentada e submetida à aprovação do proprietário do projeto antes de qualquer avanço.

---

# 2. PRINCÍPIOS OBRIGATÓRIOS

## 2.1 Preservar o projeto existente

O código atual é a base do desenvolvimento.

Não reescrever o aplicativo do zero.

Não remover funcionalidades existentes sem autorização.

Preservar especialmente:

- Interface Avalonia existente enquanto ocorre a migração gradual.
- Cadastro, edição e exclusão de itens.
- Sistema de filtros existente.
- Persistência local.
- Integração funcional com a IGDB.
- Pesquisa assíncrona com sugestões.
- Seleção de jogos por identificador.
- Dados existentes no arquivo `backlog.json`.

Antes de modificar qualquer componente, analisar seu funcionamento e suas dependências.

## 2.2 Código compreensível

O proprietário do projeto está aprendendo C# e orientação a objetos.

O código deve priorizar clareza e manutenção.

Regras:

- Preferir nomes de variáveis, métodos e classes em português.
- Utilizar nomes descritivos e fáceis de entender.
- Evitar abreviações desnecessárias.
- Respeitar nomes obrigatórios de bibliotecas, APIs e padrões do .NET.
- Evitar abstrações complexas sem benefício concreto.
- Não adicionar dependências desnecessárias.
- Documentar decisões arquiteturais relevantes.
- Explicar conceitos novos introduzidos no projeto.

Comentários devem explicar intenções, regras de negócio e trechos difíceis, sem apenas repetir o que o código já demonstra.

## 2.3 Segurança

Nunca incluir no repositório:

- Client Secret da Twitch.
- Access tokens.
- Senhas.
- Chaves administrativas do Supabase.
- Credenciais do Google.
- Arquivos contendo segredos reais.

Usar mecanismos apropriados de configuração.

A integração IGDB atual usa credenciais fornecidas localmente por variáveis de ambiente.

Não implementar renovação de tokens com Client Secret embutido no aplicativo desktop.

Para distribuição futura, considerar um backend seguro para operações que exigem segredos.

## 2.4 Preservação de dados

Toda alteração de modelos ou persistência deve considerar compatibilidade com dados existentes.

Antes de migrar dados:

1. Identificar o formato atual.
2. Criar backup.
3. Planejar migração.
4. Testar com cópias.
5. Validar preservação dos registros.
6. Permitir recuperação em caso de falha.

A propriedade `SteamAppId` merece atenção especial: atualmente pode conter identificadores da IGDB, que não são IDs da Steam.

Não renomear ou reinterpretar esses dados cegamente.

---

# 3. IDENTIDADE DO APLICATIVO

O nome BacklogManager é provisório.

O aplicativo deixará de ser exclusivamente um gerenciador de backlog e passará a representar uma biblioteca pessoal de entretenimento.

O novo nome deverá:

- Não limitar o aplicativo a jogos.
- Não limitar sua finalidade a itens pendentes.
- Ser adequado a futuras funcionalidades.
- Funcionar como identidade do aplicativo desktop e do futuro site.

Nomes conceituais considerados:

- Nexora
- MediaVault
- MediaVerse
- MyVerse
- Ludex

**Nenhum nome foi definitivamente aprovado.**

O Codex deverá solicitar a escolha do nome antes de realizar renomeações amplas de projeto, namespaces, executáveis e identidade visual.

---

# 4. DIREÇÃO VISUAL

O aplicativo deverá apresentar uma interface moderna, elegante, organizada e personalizável.

Referências visuais desejadas:

- Tema escuro como padrão.
- Gradientes discretos em azul e roxo.
- Cartões com cantos arredondados.
- Capas de obras como elementos visuais importantes.
- Boa separação entre conteúdo e navegação.
- Tipografia legível.
- Animações sutis.
- Layout responsivo às diferentes dimensões de janela.
- Suporte a aumento de fonte.
- Boa legibilidade mesmo com fundos personalizados.

A aparência deve priorizar usabilidade e desempenho, não apenas estética.

---

# 5. NAVEGAÇÃO PRINCIPAL

A navegação planejada inclui:

## Início

Painel pessoal de atividade e acompanhamento.

## Minha Biblioteca

Substitui o conceito de “Meu Backlog”.

Deverá conter categorias:

- Jogos
- Animes
- Séries
- Filmes
- Mangás
- Livros

A seleção de uma categoria deve mostrar apenas obras daquele tipo.

## Adicionar Obra

Pesquisa e cadastro de obras.

## Coleções

Organização personalizada de obras.

## Promoções de Jogos

Inicialmente apenas interface, sem integração com serviços de preços.

## Configurações

Aparência, biblioteca, armazenamento, backup e preferências.

## Busca global

Funcionalidade futura de pesquisa entre diferentes mídias.

O Codex poderá preparar seu componente visual, mas não deverá implementar integrações reais de busca global sem autorização.

---

# 6. BIBLIOTECA

## 6.1 Visualização

A visualização padrão será uma **grade de capas**.

O usuário poderá alternar para uma visualização em lista.

Ambas deverão oferecer acesso às mesmas funcionalidades.

A preferência de visualização deverá ser persistida localmente.

## 6.2 Informações comuns

Cada obra poderá ter:

- Identificador interno.
- Tipo de mídia.
- Identificador no catálogo externo, quando existir.
- Nome.
- Capa.
- Gêneros.
- Status.
- Nota pessoal.
- Review pessoal.
- Data de adição.
- Data de início, opcional.
- Data de conclusão, opcional.
- Informações de progresso.
- Coleções às quais pertence.

Separar dados obtidos de catálogos externos de informações pessoais fornecidas pelo usuário.

## 6.3 Status

Usar estados internos consistentes, com textos adaptados ao tipo de mídia.

Estados previstos:

- Quero consumir.
- Em andamento.
- Concluído.
- Pausado.
- Abandonado.

Exemplos de apresentação:

| Estado         | Jogo        | Anime          | Livro     |
| -------------- | ----------- | -------------- | --------- |
| Quero consumir | Quero jogar | Quero assistir | Quero ler |
| Em andamento   | Jogando     | Assistindo     | Lendo     |
| Concluído      | Finalizado  | Assistido      | Lido      |

## 6.4 Progresso pessoal

O progresso será informado pelo usuário.

**Não confundir progresso pessoal com duração estimada obtida de APIs.**

Campos planejados:

- Jogos: horas jogadas.
- Animes: episódios assistidos.
- Séries: temporada e episódio.
- Filmes: tempo assistido opcional ou status de conclusão.
- Mangás: capítulo ou volume.
- Livros: página ou capítulo.

Esses campos deverão ser opcionais e adequados ao tipo de mídia.

## 6.5 Filtros e ordenação

Permitir, conforme os dados disponíveis:

- Tipo de mídia.
- Status.
- Gênero.
- Nota.
- Data de adição.
- Ordem alfabética.
- Plataforma, para jogos.

Os filtros devem funcionar tanto na grade quanto na lista.

---

# 7. CATÁLOGOS E APIS

## 7.1 IGDB

A integração IGDB já funciona e deve ser preservada.

O serviço atual utiliza:

- `HttpClient`
- POST
- APICalypse
- Headers `Client-ID` e `Authorization`
- JSON
- Desserialização para modelos C#
- Pesquisa assíncrona

O Codex poderá melhorar tratamento de erros, cancelamento e organização do serviço, preservando seu funcionamento.

## 7.2 Novas mídias

Para animes, séries, filmes, mangás e livros, o Codex deverá preparar:

- Modelos apropriados.
- Contratos ou interfaces de serviço.
- Implementações simuladas para testes.
- Pesquisa com seleção de resultados.
- Suporte a títulos, gêneros e capas.
- Possibilidade de substituir serviços simulados por serviços reais.

**Não implementar integrações HTTP reais para essas mídias nesta etapa.**

A escolha das APIs e a implementação real serão realizadas posteriormente pelo proprietário, com orientação do ChatGPT, como exercício de aprendizado.

O aplicativo não deve apresentar dados simulados como informações reais.

---

# 8. CAPAS E IMAGENS

Exibir capas:

- Na pesquisa de obras.
- Na grade da biblioteca.
- Na visualização em lista, em tamanho reduzido.
- Em destaques da página inicial.
- Em coleções, quando apropriado.

Preferir armazenar referências ou URLs das imagens, em vez de incorporar imagens grandes ao JSON.

Prever:

- Imagem padrão quando não houver capa.
- Tratamento de URLs inválidas.
- Carregamento assíncrono.
- Possibilidade futura de cache.
- Evitar downloads repetidos desnecessários.

---

# 9. PÁGINA INICIAL

A página inicial será reformulada para funcionar como um painel pessoal.

Componentes planejados:

## Atividade recente

Obras adicionadas nos últimos 7 ou 30 dias.

Exibir capas e datas.

## Continue de onde parou

Obras em andamento.

## Resumo do período

Informações como:

- Obras adicionadas.
- Obras concluídas.
- Obras em andamento.
- Itens na lista de desejos.

## Destaques visuais

Capas de obras recentes ou relevantes.

## Promoções

Reservar espaço para futuras ofertas de jogos da lista de desejos.

Não exibir descontos inventados como promoções reais.

As estatísticas devem ser calculadas a partir de dados reais do usuário.

---

# 10. COLEÇÕES PERSONALIZADAS

Permitir criar coleções independentes das categorias oficiais.

Exemplos:

- Meus favoritos.
- Jogos para zerar.
- Animes para assistir.
- Livros de fantasia.
- Obras para revisitar.

Requisitos:

- Criar coleção.
- Renomear coleção.
- Excluir coleção.
- Adicionar obras.
- Remover obras.
- Exibir obras da coleção.

Uma obra poderá participar de várias coleções sem ser duplicada na biblioteca.

A exclusão de uma coleção não deverá excluir as obras associadas.

---

# 11. PERSONALIZAÇÃO

## 11.1 Aparência

Preparar opções de:

- Tema escuro.
- Tema claro.
- Seguir tema do sistema.
- Ajuste de tamanho de fonte.
- Cor de destaque.
- Modo compacto.
- Restaurar aparência padrão.

Alterações de fonte não devem cortar controles ou prejudicar a navegação.

## 11.2 Fundo

Permitir, quando tecnicamente viável:

- Fundo padrão.
- Cor sólida.
- Gradiente.
- Imagem escolhida pelo usuário.
- Ajuste de escurecimento/opacidade.
- Desfoque opcional.

O usuário deverá conseguir remover a personalização.

Imagens pessoais não devem ser enviadas automaticamente para serviços externos.

## 11.3 Persistência

Preferências de aparência devem ser armazenadas separadamente dos dados da biblioteca.

---

# 12. ARMAZENAMENTO

A arquitetura futura deverá suportar três modos:

## Local

Dados armazenados no computador.

Funcionamento sem conta e sem internet.

## Nuvem

Dados armazenados no Supabase, utilizando PostgreSQL.

Acesso vinculado à conta do usuário.

## Híbrido

Dados locais com sincronização na nuvem.

A sincronização deverá considerar conflitos e falhas de conexão.

**Nesta reformulação, implementar apenas o armazenamento local e preparar abstrações para os demais modos.**

Não implementar Supabase ainda.

Não apresentar opções futuras como se já estivessem operacionais.

---

# 13. BACKUP

## 13.1 Backup local

Preparar exportação e importação de backups em JSON.

Requisitos:

- Exportar dados da biblioteca.
- Identificar a versão do formato.
- Validar arquivos importados.
- Evitar perda acidental de registros.
- Permitir recuperação após falhas.
- Não incluir credenciais nos backups.

## 13.2 Google Drive

Integração futura para enviar e restaurar backups.

O Google Drive será inicialmente um **destino de backup**, não um banco de dados.

A implementação de OAuth e upload para Google Drive ficará reservada para etapa posterior.

---

# 14. PROMOÇÕES DE JOGOS

Criar uma página visual destinada a promoções.

A interface poderá prever:

- Capa do jogo.
- Nome.
- Loja.
- Preço original.
- Preço promocional.
- Percentual de desconto.
- Filtros.
- Jogos desejados.

**Não implementar o backend de promoções agora.**

Não consultar APIs reais de preços.

Não inventar preços ou descontos apresentados como reais.

Exibir claramente estados vazios ou indicação de funcionalidade em desenvolvimento.

A integração real será feita posteriormente pelo proprietário com orientação do ChatGPT.

---

# 15. ARQUITETURA

Separar responsabilidades entre:

- Interface.
- Modelos de catálogo.
- Modelos da biblioteca pessoal.
- Serviços de pesquisa.
- Persistência.
- Preferências.
- Backup.
- Futuras integrações externas.

Evitar acoplamento direto da interface ao `FileManager`.

Preparar uma abstração para armazenamento local e futuro armazenamento remoto.

Não criar uma arquitetura excessivamente complexa apenas por antecipação.

A prioridade é manter o código compreensível, testável e extensível.

---

# 16. ROADMAP DE IMPLEMENTAÇÃO PELO CODEX

Cada fase terá sua própria branch.

## Fase 0 — Estabilidade

Branch: `chore/seguranca-estabilidade`

Objetivos:

- Tratar erros HTTP.
- Evitar encerramento do aplicativo por falhas de rede.
- Melhorar cancelamento de buscas.
- Revisar uso de `HttpClient`.
- Corrigir problemas de identificação IGDB/Steam.
- Preservar os dados JSON.
- Revisar tratamento de entradas inválidas.

Critério principal: o aplicativo atual continua funcionando, inclusive quando a API falha.

## Fase 1 — Arquitetura

Branch: `refactor/arquitetura`

Objetivos:

- Separar responsabilidades.
- Preparar modelos para múltiplas mídias.
- Separar catálogo de dados pessoais.
- Preparar abstrações de persistência.
- Documentar arquitetura.

Critério principal: a estrutura melhora sem alterar indevidamente o comportamento existente.

## Fase 2 — Multimídia

Branch: `feat/multimidia`

Objetivos:

- Adicionar as seis categorias.
- Preparar serviços simulados para novas mídias.
- Implementar cadastro e seleção.
- Preservar a IGDB.
- Adicionar progresso específico por mídia.

Critério principal: todas as categorias funcionam com dados simulados ou existentes, sem depender de novas APIs reais.

## Fase 3 — Biblioteca

Branch: `feat/biblioteca`

Objetivos:

- Grade de capas padrão.
- Alternância para lista.
- Filtros e ordenação.
- Status, notas, reviews e progresso.
- Capas nas sugestões e na biblioteca.

Critério principal: todas as operações existentes continuam funcionando nas duas visualizações.

## Fase 4 — Coleções

Branch: `feat/colecoes`

Objetivos:

- Criar e editar coleções.
- Associar obras a várias coleções.
- Remover associações.
- Preservar obras ao excluir coleções.

## Fase 5 — Identidade visual

Branch: `feat/identidade-visual`

Objetivos:

- Aplicar o nome aprovado.
- Atualizar identidade.
- Refinar navegação.
- Implementar temas, cores, fontes e fundos.
- Garantir legibilidade.

## Fase 6 — Dashboard

Branch: `feat/dashboard`

Objetivos:

- Atividade recente.
- Obras em andamento.
- Estatísticas do período.
- Destaques com capas.
- Espaço reservado para promoções.

## Fase 7 — Configurações e backup

Branch: `feat/configuracoes`

Objetivos:

- Organizar preferências.
- Persistir configurações locais.
- Implementar exportação e importação de backup.
- Preparar interface para futuros modos de armazenamento.
- Identificar claramente recursos ainda indisponíveis.

## Fase 8 — Promoções: interface

Branch: `feat/promocoes-ui`

Objetivos:

- Criar página visual de promoções.
- Preparar componentes e estados vazios.
- Não implementar API real.
- Não apresentar dados fictícios como reais.

---

# 17. ETAPAS RESERVADAS PARA IMPLEMENTAÇÃO GUIADA

O Codex não deverá executar as etapas abaixo sem nova autorização explícita.

## Fase 9

Integração das APIs reais para animes, séries, filmes, mangás e livros.

## Fase 10

Fundamentos de PostgreSQL, Supabase, autenticação e políticas RLS.

## Fase 11

Sincronização local/nuvem e resolução de conflitos.

## Fase 12

Integração real de promoções de jogos.

## Fase 13

Backup e restauração via Google Drive.

## Fase 14

Desenvolvimento do site.

## Fase 15

Busca global integrada entre mídias e serviços de catálogo.

Essas etapas serão desenvolvidas pelo proprietário do projeto com orientação didática do ChatGPT.

---

# 18. PROTOCOLO GIT

Para cada fase:

1. Atualizar a branch principal.
2. Confirmar que não existem alterações não preservadas.
3. Criar branch específica a partir da `main`.
4. Implementar somente o escopo aprovado.
5. Criar commits claros e organizados.
6. Executar compilação e testes.
7. Documentar alterações.
8. Apresentar resultado ao proprietário.
9. Aguardar revisão e aprovação.
10. Realizar merge somente após autorização explícita.

Não executar merge automático.

Não apagar branches ou histórico sem autorização.

Não utilizar comandos Git destrutivos para descartar alterações existentes.

Não misturar funcionalidades de fases diferentes sem autorização.

---

# 19. VALIDAÇÃO OBRIGATÓRIA

Ao concluir cada fase, o Codex deverá verificar:

- `dotnet build` executado com sucesso.
- `dotnet test` quando houver testes aplicáveis.
- Ausência de erros de compilação.
- Funcionalidades anteriores preservadas.
- Persistência funcionando.
- Ausência de credenciais expostas.
- Tratamento de erros relevantes.
- Compatibilidade com dados existentes.
- Interface funcional no ambiente disponível.

Quando testes visuais ou manuais não puderem ser executados, isso deverá ser declarado claramente.

Não afirmar que algo foi testado quando apenas foi compilado.

---

## 19.1 — POLÍTICA OBRIGATÓRIA DE TESTES E QUALIDADE

Para cada funcionalidade nova ou comportamento existente que seja modificado, o Codex deverá planejar e executar **no mínimo 10 cenários de teste distintos e relevantes**, sempre que tecnicamente aplicável.

**IMPORTANTE: 10 testes são o mínimo, não o máximo.**

Se a complexidade, os riscos ou a importância de uma funcionalidade justificarem mais testes, o Codex deverá criar e executar quantos cenários adicionais forem necessários para alcançar uma cobertura adequada.

O objetivo não é apenas atingir uma quantidade numérica, mas garantir a estabilidade, a segurança e o funcionamento correto do aplicativo.

### 19.1.1 — Cenários que devem ser considerados

Os testes deverão abranger, conforme a funcionalidade:

1. **Funcionamento normal:** verificar se a funcionalidade executa corretamente sua operação principal.
2. **Entradas válidas:** testar diferentes valores aceitos pelo sistema.
3. **Entradas inválidas:** verificar como o aplicativo reage a informações incorretas.
4. **Valores vazios ou nulos:** garantir que campos não preenchidos sejam tratados adequadamente.
5. **Casos extremos e limites:** testar valores mínimos, máximos e situações incomuns.
6. **Falhas externas:** simular problemas de conexão, APIs indisponíveis e respostas inesperadas.
7. **Cancelamentos e interrupções:** verificar o comportamento quando uma operação é interrompida.
8. **Persistência e integridade:** confirmar que informações são salvas, carregadas e preservadas corretamente.
9. **Compatibilidade e regressão:** garantir que alterações não prejudiquem funcionalidades existentes.
10. **Recuperação de falhas:** verificar se o aplicativo consegue continuar funcionando ou recuperar dados após problemas.

Essas categorias são referências para elaborar os testes, não uma lista rígida. O Codex deverá adaptar os cenários às características e aos riscos reais de cada funcionalidade.

### 19.1.2 — Regras obrigatórias

- Os testes devem ser diferentes e relevantes. Não repetir o mesmo teste dez vezes apenas para atingir a quantidade mínima.
- A quantidade de 10 testes não deve limitar a cobertura. Funcionalidades complexas ou críticas deverão receber testes adicionais.
- Sempre que possível, implementar testes automatizados que possam ser executados novamente nas próximas fases.
- Testes de integração com APIs externas deverão utilizar simulações controladas quando necessário, evitando dependência de credenciais reais ou disponibilidade dos serviços.
- Executar testes de regressão para verificar se funcionalidades anteriores continuam funcionando.
- Quando um bug for identificado, corrigi-lo e executar novamente os testes afetados.
- Não considerar uma funcionalidade aprovada enquanto existirem falhas críticas ou relevantes não resolvidas.
- Não modificar ou excluir dados pessoais reais para realizar testes.
- Utilizar arquivos temporários, dados simulados e ambientes isolados sempre que necessário.
- Caso algum teste não possa ser executado, documentar o motivo e fornecer instruções para validação manual.
- Não confundir compilação bem-sucedida com funcionamento correto.
- Não afirmar que um teste passou sem tê-lo executado.
- Não criar testes artificiais ou irrelevantes apenas para cumprir a quantidade mínima.

### 19.1.3 — Relatório obrigatório dos testes

Para cada funcionalidade implementada ou modificada, o Codex deverá apresentar uma tabela contendo:

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| T01 | Descrição do cenário | Comportamento esperado | Resultado obtido | Automatizado/Manual | Aprovado/Reprovado/Não executado |
| T02 | Descrição do cenário | Comportamento esperado | Resultado obtido | Automatizado/Manual | Aprovado/Reprovado/Não executado |

A tabela acima é apenas um exemplo de formato, não representa testes executados.

O relatório também deverá informar:

- Quantidade total de cenários planejados.
- Quantidade de cenários efetivamente executados.
- Quantidade de testes aprovados.
- Quantidade de testes reprovados.
- Quantidade de testes não executados e suas justificativas.
- Bugs encontrados e respectivas correções.
- Testes executados novamente após as correções.
- Riscos conhecidos que ainda precisam de atenção.

### 19.1.4 — Critérios para aprovação

Uma funcionalidade somente poderá ser considerada pronta para revisão quando:

1. Os cenários relevantes tiverem sido executados.
2. Os testes automatizados aplicáveis estiverem passando.
3. Não existirem falhas críticas ou relevantes sem resolução.
4. Os dados existentes estiverem preservados.
5. Os testes de regressão aplicáveis tiverem sido realizados.
6. Os resultados estiverem documentados.
7. As limitações e os testes não executados estiverem claramente identificados.

**A aprovação definitiva continua sendo responsabilidade do proprietário do projeto.**

O Codex não poderá realizar merge na `main` ou avançar para a próxima fase sem autorização explícita.

**Esta política é obrigatória para todas as fases de desenvolvimento do projeto.**

---

# 20. RELATÓRIO OBRIGATÓRIO AO FINAL DE CADA FASE

O Codex deverá apresentar:

1. Resumo do que foi implementado.
2. Arquivos criados.
3. Arquivos modificados.
4. Principais decisões arquiteturais.
5. Explicação de novos conceitos C# utilizados.
6. Testes executados e seus resultados.
7. Testes manuais recomendados.
8. Problemas conhecidos.
9. Possíveis impactos sobre dados antigos.
10. Instruções para executar e validar.
11. Confirmação de que não houve merge.
12. Solicitação de aprovação antes da próxima etapa.

---

# 21. REGRAS DE EXECUÇÃO

**REGRA 1:** Não implementar todas as fases de uma vez.

**REGRA 2:** Não avançar automaticamente para a próxima fase.

**REGRA 3:** Não fazer merge sem aprovação.

**REGRA 4:** Não substituir o projeto existente por uma reescrita completa.

**REGRA 5:** Não apagar dados existentes.

**REGRA 6:** Não introduzir credenciais reais no código.

**REGRA 7:** Não implementar integrações reservadas ao aprendizado guiado.

**REGRA 8:** Não apresentar funcionalidades simuladas como reais.

**REGRA 9:** Explicar mudanças importantes em linguagem acessível.

**REGRA 10:** Priorizar estabilidade, legibilidade e manutenção.

**REGRA 11:** Solicitar esclarecimentos quando uma decisão de produto ou arquitetura ainda não estiver definida.

**REGRA 12:** Encerrar cada fase aguardando aprovação do proprietário.

---

# 22. INSTRUÇÃO INICIAL AO CODEX

Antes de iniciar qualquer implementação:

1. Leia integralmente este contrato.
2. Inspecione o repositório atual.
3. Identifique a estrutura e os componentes existentes.
4. Verifique o estado do Git e possíveis alterações não commitadas.
5. Confirme como a persistência JSON funciona.
6. Identifique a integração IGDB existente.
7. Avalie riscos da Fase 0.
8. Apresente um plano específico para a Fase 0.
9. Aguarde autorização antes de modificar arquivos.

Não iniciar fases posteriores.

**O objetivo é evoluir um projeto funcional, preservando seu histórico e permitindo que o proprietário compreenda o código que está construindo.**

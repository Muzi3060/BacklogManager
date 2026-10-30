# Beta — Etapa 3: resultados de testes
**Execução real em 30/10/2026.** Implementação em `feat/beta-distribuicao`, sem merge/push/publicação. Pacotes candidatos; sem aprovação final de distribuição.
## Resultados efetivamente executados
| Conjunto | Aprovados | Reprovados finais | Não executados/ignorados na execução |
|---|---:|---:|---:|
| `dotnet build` final | Sucesso: 0 avisos, 0 erros | 0 | 0 |
| Regressão C# completa | **1707** | **0** | **0** |
| Desses, regressão anterior | 1541 | 0 | 0 |
| Desses, novos testes C# | 166 | 0 | 0 |
| Scripts de publicação/credenciais | 29 | 0 | 0 |
| Preparo do instalador (fixtures) | 12 | 0 | 0 |
| Verificações de preservação/artefatos | 29 | 0 | 0 |
As linhas “desses” são subconjuntos dos 1707, não execuções adicionais a somar. Os testes Python e as verificações locais têm escopos próprios. Não confundir esses totais com os 206 cenários previstos.
Os testes do instalador verificam entradas/inventário e não equivalem a instalar o EXE em Windows. Nenhum teste usou a biblioteca pessoal. Build sozinho não foi apresentado como aprovação funcional.
Baseline anterior às mudanças: 1541/1541 aprovados. Execuções intermediárias e reexecuções foram mantidas como evidência local; os resultados finais abaixo não apagam as falhas encontradas.
## Evidências e ambiente
- Host: CachyOS x64; SDK 10.0.113; runtime self-contained 10.0.13; Avalonia 12.1.4. Windows 11 e Ubuntu 24.04 não estavam disponíveis para qualificação nativa.
- TRX final privado: `${RESULTADOS}/regressao-final.trx`, SHA-256 `50a678080b0810d67371596f689402ec302d56bd375412a3bbff5f56f1ea7d8c`. Logs e manifesto privado permanecem locais, fora do Git público. O relatório inclui nomes/asserções e estado, sem caminhos pessoais dos logs.
- Código dos candidatos finais: commit `708edd0`. O manifesto registra pendências documentais existentes na árvore durante o build; nenhuma alteração C#/Avalonia não commitada entrou nos candidatos.
- TAR.GZ extraído iniciou em sessão X11/XWayland no CachyOS, sem variáveis IGDB, com dados fora dos binários/cwd. Duas inicializações mantiveram o hash da biblioteca fictícia. Após trocar a pasta de binários, o perfil continuou preservado. Com pasta de dados sem escrita, abriu diagnóstico sem fallback. Instâncias de teste foram encerradas; arquivos fictícios permanecem isolados.
- Teste nativo inicial do harness usou prazo insuficiente para localizar a janela; refeito aguardando disponibilidade real. Não foi tratado como falha funcional comprovada do aplicativo. A automação de cliques não confirmou o fluxo completo; não foi registrada como teste de cadastro aprovado.
- Ensaio IGDB real limitado: HTTP **200**, um resultado válido, sem registrar título/credenciais. Consulta realizada separadamente da UI; não aprova Z10/W05/L10.
- Ensaio GitHub real anônimo: HTTP **404**, tratado como indisponibilidade. Descoberta remota de beta real não executada; respostas simuladas cobrem ordenação/paginação/erros.
## Cobertura prevista: 206 cenários
**169 cenários atendidos pelas evidências descritas; 37 não executados integralmente.** Os cinco manuais do proprietário permanecem não executados. Os 37 pendentes são 27 de funcionamento nativo Windows e 10 de qualificação funcional Ubuntu. Não são testes aprovados.
A aprovação nesta tabela limita-se ao método indicado. Diretórios Windows e HTTP simulado são testes de lógica, sem afirmar execução do pacote no Windows. Os IDs dos testes individuais não têm correspondência exclusiva 1:1 com o planejamento: uma evidência pode apoiar várias condições, e há casos adicionais além dos 206.
| ID previsto | Cenário/resultado esperado | Resultado observado e evidência | Tipo | Status |
|---|---|---|---|---|
| D01 | Windows por usuário | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D02 | Linux sem XDG | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D03 | XDG absoluto | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D04 | XDG vazio | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D05 | XDG relativo inválido | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D06 | perfil explícito absoluto | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D07 | perfil explícito relativo recusado | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D08 | caminho com espaços | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D09 | acentos/Unicode | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D10 | pasta inexistente criada com proteção adequada | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D11 | pasta sem permissão, sem fallback | Unidades e ensaios nativos isolados no CachyOS: pasta sem escrita, cwd independente, troca de binários e binários sem escrita; não qualifica Windows/Ubuntu. | Automatizado/revisão/ensaio indicado | Aprovado |
| D12 | Working directory diferente | Unidades e ensaios nativos isolados no CachyOS: pasta sem escrita, cwd independente, troca de binários e binários sem escrita; não qualifica Windows/Ubuntu. | Automatizado/revisão/ensaio indicado | Aprovado |
| D13 | binários movidos | Unidades e ensaios nativos isolados no CachyOS: pasta sem escrita, cwd independente, troca de binários e binários sem escrita; não qualifica Windows/Ubuntu. | Automatizado/revisão/ensaio indicado | Aprovado |
| D14 | binários sem escrita | Unidades e ensaios nativos isolados no CachyOS: pasta sem escrita, cwd independente, troca de binários e binários sem escrita; não qualifica Windows/Ubuntu. | Automatizado/revisão/ensaio indicado | Aprovado |
| D15 | arquivos ativos ausentes sem inventar dados | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D16 | recuperação antes dos leitores | Dez casos R01–R10 de inicialização/recuperação; Configurações acessível, sem criar JSON sobre estado pendente. | Automatizado/revisão/ensaio indicado | Aprovado |
| D17 | duas inicializações concorrentes | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D18 | perfis distintos isolados | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D19 | injeção de caminho preservada nos testes | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| D20 | designer sem acesso ao perfil real | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| M01 | biblioteca legada sozinha | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M02 | conjunto completo | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M03 | coleções ausentes | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M04 | preferências ausentes | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M05 | aparência v1 | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M06 | aparência v2 | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M07 | campos extras preservados | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M08 | SteamAppId e IgdbId distintos mantidos | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M09 | duplicatas legítimas/IDs ausentes | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M10 | datas ausentes/zeros legados | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M11 | vínculos múltiplos preservados | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M12 | referência órfã tratada conforme regras atuais | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M13 | JSON truncado | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M14 | esquema futuro de biblioteca | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M15 | preferência futura com opção explícita de não aplicar | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M16 | cancelar prévia | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M17 | destino com dados impede sobreposição automática | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M18 | origem alterada durante captura | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M19 | destino alterado após prévia | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M20 | origem com diário pendente | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M21 | destino com diário pendente | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M22 | caminhos iguais/sobrepostos | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M23 | symlink rejeitado | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M24 | falha ao criar backup | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M25 | falha na verificação do backup | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M26 | erro/disk-full durante preparação | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M27 | interrupção entre arquivos | Dez processos encerrados abruptamente em pontos distintos da migração; origem intacta e destino revertido ou confirmado conforme diário. | C#/processo isolado | Aprovado |
| M28 | falha de rollback mantém bloqueio/pendência | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M29 | reabertura após confirmação sem repetir migração | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| M30 | origem e `.bak` intactos, com cancelamento/criação vazia explícita preservados | Integração isolada: cópias byte a byte, preferências/legados, locks, falhas injetadas, backups verificados e recuperação. | Automatizado/revisão/ensaio indicado | Aprovado |
| I01 | publicação com par completo | Preparação privada e scripts Python com credenciais fictícias; publicação real usa somente o par aprovado, sem conteúdo no relatório. | Automatizado/revisão/ensaio indicado | Aprovado |
| I02 | falta Client ID bloqueia publicação | Preparação privada e scripts Python com credenciais fictícias; publicação real usa somente o par aprovado, sem conteúdo no relatório. | Automatizado/revisão/ensaio indicado | Aprovado |
| I03 | falta token bloqueia publicação | Preparação privada e scripts Python com credenciais fictícias; publicação real usa somente o par aprovado, sem conteúdo no relatório. | Automatizado/revisão/ensaio indicado | Aprovado |
| I04 | geração escapa valores e não imprime conteúdo | Preparação privada e scripts Python com credenciais fictícias; publicação real usa somente o par aprovado, sem conteúdo no relatório. | Automatizado/revisão/ensaio indicado | Aprovado |
| I05 | execução beta sem variáveis | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I06 | par completo de ambiente tem precedência | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I07 | nome legado | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I08 | nome corrigido | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I09 | ambos os nomes com precedência preservada | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I10 | corrigido vazio usa legado | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I11 | configuração local parcial sem mistura | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I12 | formato inválido sem vazamento em erro | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I13 | Debug/testes sem par beta obrigatório | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I14 | 401 com orientação da beta | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I15 | 403 sem encerrar | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I16 | 429 sem loop | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I17 | timeout/offline preservam uso local | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| I18 | cancelamento e seleção da última consulta | Regressão BuscaJanelaTests (cancelamento, consulta mais recente e limpeza), mais cenários HTTP da beta. | Automatizado/revisão/ensaio indicado | Aprovado |
| I19 | edição/exclusão/filtros/coleções offline sem IGDB | Regressão de biblioteca/coleções/backup com dados isolados e sem dependência de requisições IGDB para operações locais. | Automatizado/revisão/ensaio indicado | Aprovado |
| I20 | novo par de uma versão futura substitui o anterior no pacote, sem Client Secret | Cenários xUnit isolados e composição existente; resultados detalhados abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V01 | `0.1.0-beta.1` válido | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V02 | próxima beta | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V03 | beta.10 maior que beta.2 | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V04 | estável maior que beta correspondente | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V05 | versão menor não oferecida | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V06 | versão igual sem aviso novo | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V07 | major | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V08 | minor | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V09 | patch | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V10 | prefixo `v` da tag | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V11 | metadados de build sem mudar precedência | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V12 | tag malformada ignorada | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V13 | versão incompleta recusada | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V14 | quarto segmento não aprovado recusado | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V15 | identificador numérico com zeros inválidos recusado | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| V16 | versão exibida/assembly/manifesto coerentes | NuGet.Versioning com validação estrita e ordem semântica; resultados xUnit abaixo. | Automatizado/revisão/ensaio indicado | Aprovado |
| U01 | lista com beta mais nova | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U02 | lista com estável | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U03 | draft excluído | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U04 | tag solta sem release não oferecida | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U05 | lista vazia | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U06 | ordem por data diferente da semântica | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U07 | próxima página com candidato relevante | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U08 | release somente de outro RID | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U09 | assets ausentes | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U10 | JSON inválido | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U11 | corpo vazio/incompleto | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U12 | campos essenciais nulos | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U13 | 404 | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U14 | 403 | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U15 | 429/Retry-After | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U16 | timeout | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U17 | cancelamento ao fechar | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U18 | offline | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U19 | resposta muito grande | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U20 | limite de paginação com resultado inconclusivo | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U21 | cache/304 sem falso positivo | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U22 | headers sem token da IGDB/PAT | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U23 | URL não oficial/HTTP/redirecionamento indevido bloqueado | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| U24 | falha ao abrir navegador comunicada sem instalar nada | HTTP simulado/controlado e falha do navegador injetada; nenhuma instalação. Leitura real do GitHub retornou 404. | Automatizado/revisão/ensaio indicado | Aprovado |
| A01 | primeira execução vazia | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| A02 | origem sugerida não importa sozinha | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| A03 | escolha de pasta | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| A04 | prévia mostra origem/destino e contagens | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| A05 | cancelar sem gravar | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| A06 | confirmação explícita | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| A07 | destino ocupado | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| A08 | erro de backup visível | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| A09 | recuperação pendente bloqueia gravação | Dez casos R01–R10 de inicialização/recuperação; Configurações acessível, sem criar JSON sobre estado pendente. | Automatizado/revisão/ensaio indicado | Aprovado |
| A10 | mensagem de conclusão só depois da releitura | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| A11 | teclado/rolagem em 900 × 660 | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| A12 | legibilidade nas oito combinações de tema/modo | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T01 | versão atual visível | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T02 | botão inicia consulta sem bloquear navegação | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T03 | indicação de andamento | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T04 | desabilitar consulta duplicada | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T05 | resultado atualizado | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T06 | nova versão disponível | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T07 | recurso remoto indisponível | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T08 | nova versão sem pacote compatível | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T09 | abertura somente por clique | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T10 | teclado/foco | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T11 | 900 × 660/rolagem | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| T12 | oito combinações de tema/modo, sem perder edição em andamento | Avalonia Headless: fluxo, mensagens, confirmação, foco/teclado, 900 × 660 e oito combinações de tema/modo; qualificação nativa permanece separada. | Automatizado/revisão/ensaio indicado | Aprovado |
| P01 | win-x64 Release | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P02 | linux-x64 Release | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P03 | runtime incluído | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P04 | sem Native AOT/trimming | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P05 | JSON de dependências preservado | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P06 | bibliotecas nativas presentes | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P07 | recursos Avalonia/capas simuladas presentes | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P08 | nomes/versões/manifestos | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P09 | inventário e SHA-256 | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P10 | saída limpa sem sobras de outra versão | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P11 | testes/worker ausentes | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P12 | dados/diários/backups ausentes | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P13 | instrução privada/Qodana/skills/IDE/Git ausentes | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P14 | fontes gerados/binlogs/símbolos sensíveis ausentes | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P15 | somente credenciais intencionais nos locais esperados e nenhum Client Secret | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| P16 | falha de preparação não deixa pacote anunciado como concluído | Perfis/saídas reais por RID, inventários/checksums, auditoria por lista permitida e testes de falha dos scripts. | Automatizado/revisão/ensaio indicado | Aprovado |
| Z01 | extração íntegra/checksum | Arquivo real gerado, conteúdo relido e comparado por SHA-256; TAR preserva executável e iniciou no CachyOS. | Automatizado/revisão/ensaio indicado | Aprovado |
| Z02 | inicialização nativa sem .NET instalado | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| Z03 | inicialização sem credenciais no ambiente | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| Z04 | pasta com espaços/acentos | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| Z05 | Working directory distinto | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| Z06 | binários sem permissão de escrita | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| Z07 | perfil persistente após reiniciar | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| Z08 | capas e mídias | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| Z09 | coleções/reviews/progresso | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| Z10 | pesquisa real IGDB e cadastro em perfil fictício | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| Z11 | backup/restauração isolados | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| Z12 | extração de próxima versão em outra pasta preserva a biblioteca | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W01 | instalar sem administrador | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W02 | instalar sem runtime | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W03 | atalhos/nome público | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W04 | executar pelo atalho | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W05 | consulta IGDB real sem variáveis | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W06 | diretório com espaços | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W07 | reinstalar mesma versão | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W08 | atualizar a partir de pacote anterior simulado | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W09 | app aberto pede fechamento sem força | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W10 | cancelar instalação | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W11 | falha de cópia não anuncia sucesso | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W12 | desinstalar preserva dados/backups | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W13 | reinstalar após desinstalar | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W14 | AppId permanece estável | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W15 | vários usuários não compartilham biblioteca | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| W16 | instalar não executa migração ou grava em dados de outro usuário | Não executado em Windows 11 x64. Compilação/inspeção e Wine não substituem validação nativa. | Nativo pendente | Não executado |
| L01 | checksum/extração | Arquivo real gerado, conteúdo relido e comparado por SHA-256; TAR preserva executável e iniciou no CachyOS. | Automatizado/revisão/ensaio indicado | Aprovado |
| L02 | executável e permissões | Arquivo real gerado, conteúdo relido e comparado por SHA-256; TAR preserva executável e iniciou no CachyOS. | Automatizado/revisão/ensaio indicado | Aprovado |
| L03 | iniciar sem .NET | Qualificação Ubuntu 24.04 LTS ainda não executada. Ensaios limitados no CachyOS registrados separadamente. | Nativo pendente | Não executado |
| L04 | dependência nativa ausente gera diagnóstico real | Qualificação Ubuntu 24.04 LTS ainda não executada. Ensaios limitados no CachyOS registrados separadamente. | Nativo pendente | Não executado |
| L05 | iniciar em sessão X11 | Qualificação Ubuntu 24.04 LTS ainda não executada. Ensaios limitados no CachyOS registrados separadamente. | Nativo pendente | Não executado |
| L06 | sessão Wayland/XWayland quando disponível | Qualificação Ubuntu 24.04 LTS ainda não executada. Ensaios limitados no CachyOS registrados separadamente. | Nativo pendente | Não executado |
| L07 | sem escrita na pasta dos binários | Qualificação Ubuntu 24.04 LTS ainda não executada. Ensaios limitados no CachyOS registrados separadamente. | Nativo pendente | Não executado |
| L08 | pasta com espaços/Unicode | Qualificação Ubuntu 24.04 LTS ainda não executada. Ensaios limitados no CachyOS registrados separadamente. | Nativo pendente | Não executado |
| L09 | XDG persistente entre reinícios | Qualificação Ubuntu 24.04 LTS ainda não executada. Ensaios limitados no CachyOS registrados separadamente. | Nativo pendente | Não executado |
| L10 | pesquisa IGDB real sem variáveis | Qualificação Ubuntu 24.04 LTS ainda não executada. Ensaios limitados no CachyOS registrados separadamente. | Nativo pendente | Não executado |
| L11 | backup/restauração isolados | Qualificação Ubuntu 24.04 LTS ainda não executada. Ensaios limitados no CachyOS registrados separadamente. | Nativo pendente | Não executado |
| L12 | troca de pasta dos binários preserva dados e preferências | Qualificação Ubuntu 24.04 LTS ainda não executada. Ensaios limitados no CachyOS registrados separadamente. | Nativo pendente | Não executado |
| H01 | 1541 testes antigos reexecutados | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H02 | novos testes incorporados à regressão | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H03 | exclusões compartilhadas preservadas | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H04 | fixtures e JSON legítimos continuam versionáveis | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H05 | transição documentada dos checks históricos da Etapa 2 | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H06 | contrato intacto | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H07 | arquivo privado intacto/não rastreado | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H08 | Qodana/skills intactos/não rastreados | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H09 | biblioteca pessoal/backup reais com hashes preservados | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H10 | `.git`/branches anteriores preservados | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H11 | logs sem credenciais completas | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H12 | novos documentos sem caminhos pessoais | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H13 | pacotes auditados após a geração | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H14 | instruções públicas correspondem ao pacote real | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H15 | nenhum download/substituição automática de atualização | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |
| H16 | nenhuma publicação, merge ou nova integração fora do escopo autorizado | Verificações locais, manifesto privado, revisão de escopo e regressão; ver JSON sanitizado. H16 refere-se à publicação remota. | Automatizado/revisão/ensaio indicado | Aprovado |

## Testes C# novos — resultados individuais reais
Esperado: asserções de cada método e seus parâmetros abaixo. Observado: resultado retornado pelo xUnit no TRX final; não se inventaram mensagens ou resultados manuais.
| Caso executado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T01") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T02") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T03") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T04") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T05") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T06") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T07") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T08") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T09") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T10") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T11") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "T12") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesInterfaceTests.ConsultaNaoInstala(id: "U24") | Atualizar somente a interface esperada, preservar navegação/edição e abrir por clique | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.BloqueiaLinksEstranhos(url: "http://github.com/exemplo/projeto/releases/tag/v1") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.BloqueiaLinksEstranhos(url: "https://github.com/exemplo/outro/releases/tag/v1") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.BloqueiaLinksEstranhos(url: "https://github.com/exemplo/projeto/releases/tag/v1"···) | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.BloqueiaLinksEstranhos(url: "https://github.com/exemplo/projeto/releases/tag/v1"···) | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.BloqueiaLinksEstranhos(url: "https://github.com/exemplo/projeto/releases/tag/v1"···) | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.BloqueiaLinksEstranhos(url: "https://github.com:8443/exemplo/projeto/releases/t"···) | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U01") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U02") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U03") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U04") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U05") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U06") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U07") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U08") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U09") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U10") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U11") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U12") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U13") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U14") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U15") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U16") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U17") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U18") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U19") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U20") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U21") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U22") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U23") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| AtualizacoesTests.ConsultaControlada(id: "U25") | Retornar o estado esperado e validar requisições/links sem instalação | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.DesenvolvimentoSemRecursoNaoExigeSegredos | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.ErrosSeguros(id: "I14", codigo: 401) | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.ErrosSeguros(id: "I15", codigo: 403) | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.ErrosSeguros(id: "I16", codigo: 429) | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.ErrosSeguros(id: "I17", codigo: 503) | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I05", cliente: null, novo: null, legado: null, daBeta: True, esperado: "beta") | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I06", cliente: "ambiente", novo: "novo", legado: null, daBeta: False, esperado: "ambiente") | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I07", cliente: "ambiente", novo: null, legado: "legado", daBeta: False, esperado: "ambiente") | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I08", cliente: "ambiente", novo: "novo", legado: null, daBeta: False, esperado: "ambiente") | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I09", cliente: "ambiente", novo: "novo", legado: "legado", daBeta: False, esperado: "ambiente") | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I10", cliente: "ambiente", novo: " ", legado: "legado", daBeta: False, esperado: "ambiente") | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I11", cliente: "ambiente", novo: null, legado: null, daBeta: False, esperado: null) | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I12", cliente: null, novo: "invalido\r\n", legado: null, daBeta: False, esperado: null) | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I13", cliente: null, novo: null, legado: null, daBeta: True, esperado: "beta") | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I20", cliente: null, novo: null, legado: null, daBeta: True, esperado: "beta") | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I21", cliente: null, novo: "novo", legado: null, daBeta: False, esperado: null) | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.PrecedenciaSemMistura(id: "I22", cliente: "cliente com espaço", novo: "novo", legado: null, daBeta: False, esperado: null) | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| CredenciaisBetaTests.RecursoInvalidoNaoMistura | Respeitar precedência, impedir mistura de fontes e tratar erro sem expor token | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.ArgumentoInvalido(id: "D07", args: ["--data-dir", "relativo"]) | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.ArgumentoInvalido(id: "D21", args: ["--data-dir"]) | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.ArgumentoInvalido(id: "D22", args: ["--data-dir", "/a", "--data-dir", "/b"]) | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.D11PastaEArquivoNaoSeConfundem | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.D17PreparacoesConcorrentesSaoIsoladas | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.D19ComposicaoPreservaCaminhoInjetado | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.D20DesignerNaoAbrePerfilPessoal | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.Padrao(id: "D01", windows: True, xdg: null, esperado: "/local/MinhasRealizacoes/Dados") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.Padrao(id: "D02", windows: False, xdg: null, esperado: "/usuario/.local/share/minhas-realizacoes/dados") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.Padrao(id: "D03", windows: False, xdg: "/xdg", esperado: "/xdg/minhas-realizacoes/dados") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.Padrao(id: "D04", windows: False, xdg: "", esperado: "/usuario/.local/share/minhas-realizacoes/dados") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.Padrao(id: "D05", windows: False, xdg: "relativo", esperado: "/usuario/.local/share/minhas-realizacoes/dados") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.PastaNovaIsolada(id: "D10") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.PastaNovaIsolada(id: "D15") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.PastaNovaIsolada(id: "D18") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.PerfilExplicito(id: "D06", pasta: "/perfil") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.PerfilExplicito(id: "D08", pasta: "/perfil com espaço") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.PerfilExplicito(id: "D09", pasta: "/ação 日本") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.PerfilExplicito(id: "D12", pasta: "/outra-pasta") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| DiretoriosTests.PerfilExplicito(id: "D13", pasta: "/binarios-movidos") | Resolver/validar os caminhos e preservar isolamento conforme parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A02") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A03") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A04") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A05") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A06") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A07") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A08") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A09") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A10") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A11") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A12") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InicializacaoInterfaceTests.FluxoConsentido(id: "A13") | Exigir validação/consentimento, preservar cancelamento e usabilidade | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InterrupcaoMigracaoTests.M27ProcessoInterrompidoPreservaOrigemERecuperaDestino(etapa: "antes-backup") | Após saída abrupta 73, preservar origem e recuperar o destino pelo diário, conforme o ponto interrompido | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InterrupcaoMigracaoTests.M27ProcessoInterrompidoPreservaOrigemERecuperaDestino(etapa: "antes-confirmacao") | Após saída abrupta 73, preservar origem e recuperar o destino pelo diário, conforme o ponto interrompido | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InterrupcaoMigracaoTests.M27ProcessoInterrompidoPreservaOrigemERecuperaDestino(etapa: "arquivos-preparados") | Após saída abrupta 73, preservar origem e recuperar o destino pelo diário, conforme o ponto interrompido | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InterrupcaoMigracaoTests.M27ProcessoInterrompidoPreservaOrigemERecuperaDestino(etapa: "backup-verificado") | Após saída abrupta 73, preservar origem e recuperar o destino pelo diário, conforme o ponto interrompido | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InterrupcaoMigracaoTests.M27ProcessoInterrompidoPreservaOrigemERecuperaDestino(etapa: "confirmada") | Após saída abrupta 73, preservar origem e recuperar o destino pelo diário, conforme o ponto interrompido | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InterrupcaoMigracaoTests.M27ProcessoInterrompidoPreservaOrigemERecuperaDestino(etapa: "depois-Aparencia") | Após saída abrupta 73, preservar origem e recuperar o destino pelo diário, conforme o ponto interrompido | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InterrupcaoMigracaoTests.M27ProcessoInterrompidoPreservaOrigemERecuperaDestino(etapa: "depois-Biblioteca") | Após saída abrupta 73, preservar origem e recuperar o destino pelo diário, conforme o ponto interrompido | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InterrupcaoMigracaoTests.M27ProcessoInterrompidoPreservaOrigemERecuperaDestino(etapa: "depois-Colecoes") | Após saída abrupta 73, preservar origem e recuperar o destino pelo diário, conforme o ponto interrompido | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InterrupcaoMigracaoTests.M27ProcessoInterrompidoPreservaOrigemERecuperaDestino(etapa: "depois-Visualizacao") | Após saída abrupta 73, preservar origem e recuperar o destino pelo diário, conforme o ponto interrompido | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| InterrupcaoMigracaoTests.M27ProcessoInterrompidoPreservaOrigemERecuperaDestino(etapa: "diario-gravado") | Após saída abrupta 73, preservar origem e recuperar o destino pelo diário, conforme o ponto interrompido | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.ConjuntoEFalhas(id: "M02") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.ConjuntoEFalhas(id: "M11") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.ConjuntoEFalhas(id: "M12") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.ConjuntoEFalhas(id: "M15") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.ConjuntoEFalhas(id: "M23") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.ConjuntoEFalhas(id: "M26") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.ConjuntoEFalhas(id: "M28") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.ConjuntoEFalhas(id: "M31") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.M32MigracoesConcorrentesNaoSobrescrevem | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.OrigemInvalida(id: "M13", json: "{") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.OrigemInvalida(id: "M14", json: "{\"Versao\":999}") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.PreferenciasOpcionais(id: "M03", aparencia: False, visualizacao: False) | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.PreferenciasOpcionais(id: "M04", aparencia: False, visualizacao: True) | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.PreferenciasOpcionais(id: "M05", aparencia: True, visualizacao: False) | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.PreferenciasOpcionais(id: "M06", aparencia: True, visualizacao: True) | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.PreservaBytes(id: "M01", json: "[]") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.PreservaBytes(id: "M07", json: "[{\"Categoria\":\"Ação\",\"Status\":\"Na Fila\",\""···) | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.PreservaBytes(id: "M08", json: "[{\"Categoria\":\"Ação\",\"Status\":\"Na Fila\",\""···) | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.PreservaBytes(id: "M09", json: "[{\"Categoria\":\"Ação\",\"Status\":\"Na Fila\",\""···) | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.PreservaBytes(id: "M10", json: "[{\"Categoria\":\"Ação\",\"Status\":\"Na Fila\",\""···) | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M16") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M17") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M18") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M19") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M20") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M21") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M22") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M24") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M25") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M27") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M29") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| MigracaoTests.Protecoes(id: "M30") | Preservar bytes e vínculos ou cancelar/recuperar diante da falha indicada | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| RecuperacaoInicializacaoTests.InicializacaoNaoEliminaRecuperacao(id: "R01") | Inicializar somente conjunto vazio; preservar componentes e acesso à recuperação pendente | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| RecuperacaoInicializacaoTests.InicializacaoNaoEliminaRecuperacao(id: "R02") | Inicializar somente conjunto vazio; preservar componentes e acesso à recuperação pendente | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| RecuperacaoInicializacaoTests.InicializacaoNaoEliminaRecuperacao(id: "R03") | Inicializar somente conjunto vazio; preservar componentes e acesso à recuperação pendente | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| RecuperacaoInicializacaoTests.InicializacaoNaoEliminaRecuperacao(id: "R04") | Inicializar somente conjunto vazio; preservar componentes e acesso à recuperação pendente | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| RecuperacaoInicializacaoTests.InicializacaoNaoEliminaRecuperacao(id: "R05") | Inicializar somente conjunto vazio; preservar componentes e acesso à recuperação pendente | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| RecuperacaoInicializacaoTests.InicializacaoNaoEliminaRecuperacao(id: "R06") | Inicializar somente conjunto vazio; preservar componentes e acesso à recuperação pendente | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| RecuperacaoInicializacaoTests.InicializacaoNaoEliminaRecuperacao(id: "R07") | Inicializar somente conjunto vazio; preservar componentes e acesso à recuperação pendente | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| RecuperacaoInicializacaoTests.InicializacaoNaoEliminaRecuperacao(id: "R08") | Inicializar somente conjunto vazio; preservar componentes e acesso à recuperação pendente | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| RecuperacaoInicializacaoTests.InicializacaoNaoEliminaRecuperacao(id: "R09") | Inicializar somente conjunto vazio; preservar componentes e acesso à recuperação pendente | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| RecuperacaoInicializacaoTests.InicializacaoNaoEliminaRecuperacao(id: "R10") | Inicializar somente conjunto vazio; preservar componentes e acesso à recuperação pendente | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.Ordem(id: "V03", a: "0.1.0-beta.10", b: "0.1.0-beta.2", ordem: 1) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.Ordem(id: "V04", a: "0.1.0", b: "0.1.0-beta.1", ordem: 1) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.Ordem(id: "V05", a: "0.0.9", b: "0.1.0-beta.1", ordem: -1) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.Ordem(id: "V06", a: "0.1.0-beta.1", b: "0.1.0-beta.1", ordem: 0) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.Ordem(id: "V07", a: "1.0.0", b: "0.1.0", ordem: 1) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.Ordem(id: "V08", a: "0.2.0", b: "0.1.0", ordem: 1) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.Ordem(id: "V09", a: "0.1.1", b: "0.1.0", ordem: 1) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.Ordem(id: "V21", a: "0.1.0+x", b: "0.1.0+y", ordem: 0) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.V16VersaoDoProduto | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V01", texto: "0.1.0-beta.1", valido: True) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V02", texto: "0.1.0-beta.2", valido: True) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V10", texto: "v0.1.0-beta.1", valido: True) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V11", texto: "0.1.0+build.2", valido: True) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V12", texto: "versao estranha", valido: False) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V13", texto: "1.0", valido: False) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V14", texto: "1.0.0.1", valido: False) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V15", texto: "1.0.0-beta.01", valido: False) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V17", texto: "01.0.0", valido: False) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V18", texto: "1.0.0-", valido: False) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V19", texto: "1.0.0+", valido: False) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |
| VersaoTests.ValidacaoEstrita(id: "V20", texto: null, valido: False) | Aceitar/rejeitar ou ordenar exatamente conforme os parâmetros | xUnit: Passed; asserções satisfeitas | C#/isolado | Aprovado |

## Testes Python e verificações locais
| Caso | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|
| test_L01_tar_integro | L01 tar integro conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_L02_bit_executavel | L02 bit executavel conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P05_json_runtime | P05 json runtime conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P06_biblioteca_nativa | P06 biblioteca nativa conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P09_hash | P09 hash conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P11_testes_recusados | P11 testes recusados conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P12_backup_recusado | P12 backup recusado conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P12_dados_recusados | P12 dados recusados conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P13_instrucao_recusada | P13 instrucao recusada conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P13_qodana_recusado | P13 qodana recusado conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P13_skills_recusadas | P13 skills recusadas conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P14_fonte_recusado | P14 fonte recusado conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P14_simbolos_recusados | P14 simbolos recusados conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P15_token_apenas_assembly | P15 token apenas assembly conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P16_publicacao_incompleta | P16 publicacao incompleta conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P17_oculto_recusado | P17 oculto recusado conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P18_link_recusado | P18 link recusado conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P19_pasta_vazia | P19 pasta vazia conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P20_sem_sobrescrita | P20 sem sobrescrita conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P21_avisos_preservados | P21 avisos preservados conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_P22_worker_recusado | P22 worker recusado conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_Z01_zip_integro | Z01 zip integro conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_I01_par_completo | I01 par completo conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_I02_sem_cliente | I02 sem cliente conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_I03_sem_token | I03 sem token conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_I04_escapes_permissoes | I04 escapes permissoes conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_I23_controles | I23 controles conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_I24_sem_segredo_na_excecao | I24 sem segredo na excecao conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_I25_link_bloqueado | I25 link bloqueado conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB01_entrada_valida | WB01 entrada valida conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB02_linux_recusado | WB02 linux recusado conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB03_rid_ausente | WB03 rid ausente conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB04_versao_invalida | WB04 versao invalida conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB05_manifesto_sem_inventario | WB05 manifesto sem inventario conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB06_hash_divergente | WB06 hash divergente conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB07_biblioteca_pessoal | WB07 biblioteca pessoal conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB08_arquivo_novo | WB08 arquivo novo conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB09_executavel_ausente | WB09 executavel ausente conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB10_runtime_ausente | WB10 runtime ausente conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB11_instrucao_pessoal | WB11 instrucao pessoal conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| test_WB12_pdb_recusado | WB12 pdb recusado conforme asserções da fixture | unittest: ok | Python/isolado | Aprovado |
| H06 | Condição de preservação/qualidade satisfeita | Contrato comparado por SHA-256 com referência anterior à implementação. | Script local | Aprovado |
| H07 | Condição de preservação/qualidade satisfeita | Instrução privada intacta e não rastreada. | Script local | Aprovado |
| H08 | Condição de preservação/qualidade satisfeita | Arquivos pessoais existentes comparados por hash e ausentes do índice. | Script local | Aprovado |
| H09 | Condição de preservação/qualidade satisfeita | Todos os arquivos protegidos e 22 backups anteriores mantêm os hashes originais. | Script local | Aprovado |
| H10 | Condição de preservação/qualidade satisfeita | main e todas as demais referências anteriores inalteradas. | Script local | Aprovado |
| H16 | Condição de preservação/qualidade satisfeita | Trabalho na branch autorizada; script não realiza operações remotas. | Script local | Aprovado |
| H01 | Condição de preservação/qualidade satisfeita | 1707 resultados automatizados lidos do TRX, incluindo regressão anterior. | Script local | Aprovado |
| H02 | Condição de preservação/qualidade satisfeita | Casos C# novos incorporados à execução completa. | Script local | Aprovado |
| H03 | Condição de preservação/qualidade satisfeita | Exclusões compartilhadas efetivamente avaliadas pelo Git. | Script local | Aprovado |
| H04 | Condição de preservação/qualidade satisfeita | Fixtures JSON continuam elegíveis ao versionamento. | Script local | Aprovado |
| H11 | Condição de preservação/qualidade satisfeita | Logs de teste/publicação inspecionados sem imprimir valores. | Script local | Aprovado |
| H12 | Condição de preservação/qualidade satisfeita | Novos relatórios sem caminhos pessoais concretos ou credenciais completas. | Script local | Aprovado |
| P09-linux-x64 | Condição de preservação/qualidade satisfeita | Inventário SHA-256 confere com a publicação local. | Script local | Aprovado |
| P03-linux-x64 | Condição de preservação/qualidade satisfeita | Runtime self-contained presente. | Script local | Aprovado |
| P14-linux-x64 | Condição de preservação/qualidade satisfeita | Nenhum símbolo de depuração na publicação. | Script local | Aprovado |
| P07-linux-x64 | Condição de preservação/qualidade satisfeita | Índice de recursos das cinco capas simuladas presente no assembly de cada RID. | Script local | Aprovado |
| P15-linux-x64 | Condição de preservação/qualidade satisfeita | Par aprovado presente no assembly; auditoria rejeita presença em outros arquivos. | Script local | Aprovado |
| CHECKSUM-MinhasRealizacoes-0.1.0-beta.1-linux-x64.tar.gz | Condição de preservação/qualidade satisfeita | Checksum do artefato confere com a lista local. | Script local | Aprovado |
| P09-win-x64 | Condição de preservação/qualidade satisfeita | Inventário SHA-256 confere com a publicação local. | Script local | Aprovado |
| P03-win-x64 | Condição de preservação/qualidade satisfeita | Runtime self-contained presente. | Script local | Aprovado |
| P14-win-x64 | Condição de preservação/qualidade satisfeita | Nenhum símbolo de depuração na publicação. | Script local | Aprovado |
| P07-win-x64 | Condição de preservação/qualidade satisfeita | Índice de recursos das cinco capas simuladas presente no assembly de cada RID. | Script local | Aprovado |
| P15-win-x64 | Condição de preservação/qualidade satisfeita | Par aprovado presente no assembly; auditoria rejeita presença em outros arquivos. | Script local | Aprovado |
| CHECKSUM-MinhasRealizacoes-0.1.0-beta.1-win-x64.zip | Condição de preservação/qualidade satisfeita | Checksum do artefato confere com a lista local. | Script local | Aprovado |
| CHECKSUM-MinhasRealizacoes-0.1.0-beta.1-win-x64-setup.exe | Condição de preservação/qualidade satisfeita | Checksum do artefato confere com a lista local. | Script local | Aprovado |
| H13 | Condição de preservação/qualidade satisfeita | Publicações finais auditadas por lista permitida e inventário, com tokens restritos ao assembly aprovado. | Script local | Aprovado |
| H05 | Condição de preservação/qualidade satisfeita | Verificador histórico intacto; transição de escopo documentada explicitamente. | Script local | Aprovado |
| H14 | Condição de preservação/qualidade satisfeita | Instruções públicas correspondem às opções e limites dos candidatos, sem anunciar qualificação nativa inexistente. | Script local | Aprovado |
| H15 | Condição de preservação/qualidade satisfeita | Revisão estática e testes HTTP: consulta de releases sem instalação/download automático; abertura vinculada ao clique. | Script local | Aprovado |

## Artefatos locais de revisão
Utilizar somente `artifacts/beta/0.1.0-beta.1/candidato4/`. Saídas anteriores foram preservadas como ensaios e não devem ser distribuídas. Os três arquivos e inventários ficam ignorados pelo Git. Sem assinatura de código; sem release publicada.
| Arquivo | Bytes | SHA-256 | Qualificação |
|---|---:|---|---|
| linux-x64/MinhasRealizacoes-0.1.0-beta.1-linux-x64.tar.gz | 45507107 | `55b2ed4c7683db65e5614d1fba709a9b04dd8934854c9a24a9c6abcb9a2441dc` | Nativa no sistema-alvo pendente |
| win-x64/MinhasRealizacoes-0.1.0-beta.1-win-x64.zip | 49576629 | `e517caddcab36a883351894b84504565801610dfe65fea5d57158d2972664805` | Nativa no sistema-alvo pendente |
| win-x64/MinhasRealizacoes-0.1.0-beta.1-win-x64-setup.exe | 36113467 | `73169f9ecd24da3af18c05841f435ce6bced3baf9d133b46a764127949f056c2` | Nativa no sistema-alvo pendente |

## Falhas encontradas e correções
- Fixtures iniciais usavam um campo de título incompatível e omitiam campos obrigatórios; quatro testes falharam corretamente na validação. Corrigidas as fixtures para o formato legado real, sem afrouxar validadores.
- Evento duplicado substituía a referência da tarefa de consulta na interface. Proteção movida para antes da atribuição; cenários de concorrência da UI reexecutados.
- Auditoria inicial recusou `createdump`, utilitário legítimo do runtime. A lista foi ajustada especificamente; dados arbitrários continuam recusados.
- PDBs nativos eram copiados por dependências apesar de DebugSymbols=false. Excluídos somente da saída de publicação recém-gerada; auditoria continua recusando símbolos remanescentes.
- Recurso de fundo da janela de migração apontava para chave inexistente. Corrigido para os recursos centrais PageBackground/PageForeground e retestado nos oito temas/modos.
- Revisão encontrou perda de acesso à recuperação de Configurações na inicialização bloqueada. Corrigida a composição e acrescidos dez cenários R01–R10; sem criar biblioteca vazia sobre estado pendente.
- Harness de teclado passou a usar foco/layout explícitos e a tecla Espaço para ativação de botão. Preservada a verificação efetiva de clique por teclado. Avisos dos analisadores de teste corrigidos; build final sem avisos.
- Após publish Release, o grafo restaurado podia não conter as ferramentas condicionais Debug. A execução de build/test restaurou o grafo correto; não foi mascarado erro de compilação. Bloqueio IPC inicial do sandbox foi resolvido pela execução autorizada.
- Auditoria acrescentou avisos oficiais das dependências aos candidatos finais. Nenhuma licença foi atribuída ao projeto.
## Transição dos verificadores da Etapa 2
O script e os relatórios anteriores foram preservados. `PR02` exigia nenhuma mudança C#/Avalonia desde `c14f588`: deixou de ser aplicável ao código autorizado da Etapa 3 e não foi declarado aprovado. Condições documentais sobre caminhos relativos/ausência de distribuição também são históricas. Preservação do contrato corresponde a H06; arquivos privados a H07/H08; dados/backups a H09; referências a H10; exclusões/fixtures a H03/H04; documentação/artefatos a H12/H13/H14. H05 verifica que a transição foi documentada sem apagar o verificador antigo. Os 139 checks históricos da Etapa 2 não foram apresentados como nova execução.
## Arquivos criados/modificados
Lista relativa à base de planejamento `10a0196`, incluindo os novos relatórios desta entrega:
- `.gitignore`
- `App.axaml.cs`
- `BacklogManager.csproj`
- `Backup/ServicoBackupLocal.cs`
- `Configuracao/ComposicaoAplicativo.cs`
- `Configuracao/ConfiguracaoDistribuicao.cs`
- `Configuracao/InicializacaoAplicativo.cs`
- `Configuracao/ProvedorCredenciaisIgdb.cs`
- `Configuracao/ResolvedorDiretorios.cs`
- `Configuracao/VersaoAplicativo.cs`
- `JanelaInicializacaoDados.axaml`
- `JanelaInicializacaoDados.axaml.cs`
- `MainWindow.Atualizacoes.cs`
- `MainWindow.axaml`
- `MainWindow.axaml.cs`
- `Properties/PublishProfiles/BetaLinux.pubxml`
- `Properties/PublishProfiles/BetaWindows.pubxml`
- `README.md`
- `Services/IgdbService.cs`
- `Services/ServicoAtualizacoesGitHub.cs`
- `Services/ServicoMigracaoLocal.cs`
- `Tests/BacklogManager.BackupWorker/Program.cs`
- `Tests/BacklogManager.Tests/BetaDistribuicao/AtualizacoesInterfaceTests.cs`
- `Tests/BacklogManager.Tests/BetaDistribuicao/AtualizacoesTests.cs`
- `Tests/BacklogManager.Tests/BetaDistribuicao/CredenciaisBetaTests.cs`
- `Tests/BacklogManager.Tests/BetaDistribuicao/DiretoriosTests.cs`
- `Tests/BacklogManager.Tests/BetaDistribuicao/InicializacaoInterfaceTests.cs`
- `Tests/BacklogManager.Tests/BetaDistribuicao/InterrupcaoMigracaoTests.cs`
- `Tests/BacklogManager.Tests/BetaDistribuicao/MigracaoTests.cs`
- `Tests/BacklogManager.Tests/BetaDistribuicao/RecuperacaoInicializacaoTests.cs`
- `Tests/BacklogManager.Tests/BetaDistribuicao/VersaoTests.cs`
- `docs/BETA_ETAPA3.md`
- `docs/BETA_ETAPA3_TESTES.md`
- `docs/BETA_ETAPA3_VERIFICACOES.json`
- `docs/README.md`
- `global.json`
- `packaging/THIRD-PARTY-NOTICES.txt`
- `packaging/windows/MinhasRealizacoes.iss`
- `scripts/compilar_instalador_beta.py`
- `scripts/preparar_credenciais_beta.py`
- `scripts/publicar_beta.py`
- `scripts/testar_distribuicao_beta.py`
- `scripts/testar_instalador_beta.py`
- `scripts/verificar_beta_etapa3.py`

## Validação do proprietário e integração
Os cinco cenários consolidados estão em [BETA_ETAPA3.md](BETA_ETAPA3.md). Todos pendentes de execução pelo proprietário; não foi atribuída aprovação manual a nenhum deles. A matriz nativa permanece pendente. Os dez casos de interrupção de processo da nova migração foram executados. A `main` permanece em `d418f04`, e as branches anteriores foram preservadas. Aguardar revisão e autorização explícita antes de merge ou publicação.

# Preparação do lançamento 0.1.0-beta.1

> Atualização: o proprietário classificou e autorizou expressamente somente as duas grafias de e-mail dos quatro commits antigos acessíveis. O bloqueio anterior está resolvido. O relato abaixo preserva a situação histórica; resultado atual em [PUBLICACAO_BETA.md](PUBLICACAO_BETA.md).


**Estado atual: identidade resolvida e main saneada enviada ao destino privado; publicidade suspensa por retenção de commit antigo no GitHub.** O relato inicial abaixo é histórico. Resultado da retomada em [LANCAMENTO_BETA_RESULTADO.md](LANCAMENTO_BETA_RESULTADO.md). Nenhuma tag ou release foi criada.

## Trabalho concluído

- Documentação de autorização e referências lidas; os dois documentos privados estão ignorados localmente e intactos. A regra compartilhada adicional foi preparada sem versionar a autorização.
- Backup integral privado e externo criado antes das operações: 4102 arquivos relidos por SHA-256, incluindo banco Git, arquivos locais, artefatos e configurações. Bundle de todas as referências criado e verificado. Nenhum backup anterior apagado.
- `git fsck --full` sem erros de integridade. Auditoria local de 66 commits, 890 objetos e 540 blobs, incluindo referências, metadados e objetos sem referência.
- Dois nomes e dois e-mails históricos identificados, sem reproduzi-los. Há 24 objetos com raiz pessoal concreta. Nenhuma ocorrência das credenciais locais conhecidas, nenhum candidato de chave/token pelas regras aplicadas e nenhum caminho histórico de biblioteca pessoal/backup/log/instrução privada identificado. Isso não garante ausência universal de segredos; não havia detector especializado instalado.
- Acesso administrativo ao destino correto `Muzi3060/BacklogManager` confirmado por leitura. O remoto tem somente `main`, no commit `7ce3009`, conhecido e ancestral da base local. Não foi encontrado avanço remoto desconhecido na consulta; repetir antes de qualquer envio.
- Aprovação geral registrada em `APROVACAO_BETA_ETAPA3.md`; integração local por fast-forward concluída de `d418f04` para `063484e`, sem conflito. Retorno à branch `feat/beta-distribuicao`.
- Os três artefatos `candidato4` foram novamente conferidos por checksum; conteúdos ZIP/TAR comparados integralmente com o inventário. Instalador PE e hash conferidos. Nenhum pacote reconstruído ou publicado.

## Critério de interrupção encontrado

A identidade configurada não corresponde ao nome do perfil público, não usa e-mail noreply e não foi possível confirmá-la por um e-mail público do perfil GitHub. Não há identidade pública apropriada confirmada para substituir os metadados. A identidade privada não será reproduzida nem utilizada para novos commits de lançamento.

A licença MIT foi escolhida pelo proprietário, mas seu titular de copyright ainda não foi confirmado com segurança. O arquivo LICENSE não foi criado com titular inventado. Solicitar nome Git, e-mail Git públicos exatos e titular do copyright, ou indicação de configuração pública existente apropriada.

As alterações de documentação/exclusão ficam locais e não commitadas até essa confirmação. A autorização original de lançamento continua válida; não é necessária nova aprovação de todas as etapas normais.

## Estratégia de saneamento após resolver o bloqueio

1. Trabalhar em cópia isolada sem hardlinks, protegida pelo backup integral e bundle verificados; manter o repositório original e seus dados recuperáveis.
2. Mapear autores/committers pessoais para a identidade pública confirmada, sem atribuir identidades fictícias. Revisar propriedade das identidades históricas antes da transformação.
3. Substituir apenas raízes pessoais identificadas em conteúdo/mensagens históricos por referências genéricas; preservar JSON válido, conteúdo funcional, resultados documentados e o blob do contrato. Não publicar a cópia de segurança ou referências originais.
4. Verificar correspondência de commits/árvores transformadas e auditar toda a ancestralidade destinada ao envio, não apenas HEAD. Manter o mapeamento antigo/novo em evidência privada.
5. Conferir novamente o remoto. A reescrita altera os hashes; não presumir que o envio será fast-forward. Não sobrescrever commits remotos divergentes. Avaliar também a retenção de objetos antigos no servidor antes de tornar a visibilidade pública.
6. Finalizar MIT/README/notas e conferir os pacotes sem alterar o artefato auditado. Somente publicar main/tag saneadas; nunca branches antigas, mirror, referências de backup ou tags não auditadas.
7. Criar pre-release e anexos, verificar acesso anônimo e listagem da API, respeitando todas as condições da autorização.

## Evidências e resultados

Os detalhes sensíveis do inventário, backup e metadados ficam em pasta externa privada. Este documento não contém nomes/e-mails pessoais ou credenciais. As 37 pendências nativas permanecem aceitas como não executadas; a validação manual informada não inclui offline, migração manual completa nem descoberta real de novas versões.

Nova execução de lançamento: build com **0 avisos e 0 erros**; **1707 testes C# aprovados, 0 reprovados e 0 ignorados**; **41 testes Python aprovados** (29 publicação/credenciais + 12 instalador); **29 verificações de preservação/artefatos aprovadas**. O manifesto dessa verificação registra explicitamente o avanço autorizado da main; não foi apresentado como preservação do ponteiro antigo. As 37 pendências nativas continuam não executadas.

Os resultados anteriores permanecem históricos. A nova evidência TRX e os logs ficam em diretório privado externo. SHA-256 do TRX desta execução: `3a9abe99ac89819b82fc11c6ced303c21ec0591baafd18745d22df54923030f5`.

README de instalação e apresentação atualizado como preparação local; a licença MIT aguarda somente titular confirmado. Nenhum commit novo foi criado com a identidade privada atual.

### Checksums conferidos dos artefatos candidato4

| Arquivo | SHA-256 |
|---|---|
| Windows ZIP | `e517caddcab36a883351894b84504565801610dfe65fea5d57158d2972664805` |
| Windows instalador EXE | `73169f9ecd24da3af18c05841f435ce6bced3baf9d133b46a764127949f056c2` |
| Linux TAR.GZ | `55b2ed4c7683db65e5614d1fba709a9b04dd8934854c9a24a9c6abcb9a2441dc` |

Acesso anônimo, downloads públicos e descoberta da pre-release pela API ainda não foram testados porque não houve publicação.

## Retomada com identidade confirmada

O proprietário confirmou o nome público Murilo Martins Ribeiro, o e-mail `276617221+Muzi3060@users.noreply.github.com` e a titularidade MIT. A identidade foi configurada somente neste projeto, e LICENSE foi preenchido com o ano 2026. O bloqueio anterior de identidade está resolvido. As seções anteriores permanecem como registro histórico.

Antes da publicidade será verificada também a acessibilidade dos commits antigos no servidor, pois reescrever referências não garante eliminação de objetos em cache. O estado definitivo da publicação será registrado após essa verificação.

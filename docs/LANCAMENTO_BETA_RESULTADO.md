# Resultado da retomada do lançamento 0.1.0-beta.1

> Atualização: o proprietário classificou e autorizou expressamente somente as duas grafias de e-mail dos quatro commits antigos acessíveis. O bloqueio anterior está resolvido. O relato abaixo preserva a situação histórica; resultado atual em [PUBLICACAO_BETA.md](PUBLICACAO_BETA.md).


**Publicação pública não concluída por bloqueio de segurança.** O destino `Muzi3060/BacklogManager` permanece privado. Não há tag nem pre-release criadas nesta execução.

## Identidade, MIT e preparação

O proprietário confirmou a identidade pública **Murilo Martins Ribeiro**, e-mail `276617221+Muzi3060@users.noreply.github.com`, e o mesmo titular para a licença MIT. Configuração aplicada somente neste repositório; `LICENSE` contém copyright 2026. README, instalação, backup e limitações foram preparados. Os dois documentos privados de autorização continuam ignorados e não integram a ancestralidade saneada.

A Etapa 3 está aprovada em caráter geral. O relato manual CachyOS foi registrado sem aprovar offline, migração manual completa, descoberta real de atualizações ou execução Windows/Ubuntu. As **37 pendências nativas continuam não executadas** e sua aceitação não foi transformada em aprovação de testes.

## Backup e reescrita

O backup integral externo já existente foi relido e verificado por SHA-256: **4102 arquivos**, mais bundle de todas as referências. Um segundo bundle, incluindo a preparação com MIT, foi criado e verificado antes da reescrita. Nenhum backup foi apagado.

A transformação ocorreu em clone independente com `--no-local`, utilizando a ferramenta oficial git-filter-repo 2.47.0. A primeira cópia local não satisfez sua guarda de clone fresco; a operação foi abortada nessa cópia e refeita em clone fresco, sem forçar a guarda nem modificar o original.

- **67 commits** transformados na cópia, preservando topologia e commits vazios.
- **63 commits e 835 objetos** na ancestralidade da main enviada ao destino privado.
- **11200 comparações arquivo/commit** conferidas com a transformação esperada.
- O blob do `Contrato.md` permaneceu idêntico em todas as **65 árvores** em que estava presente.
- Caminhos pessoais/emails identificados foram sanitizados em **21 caminhos documentais históricos**; nenhum fonte C#/Avalonia funcional foi alterado pela sanitização de conteúdo.
- Autores e committers da história saneada usam exclusivamente a identidade pública confirmada.
- Auditoria da ancestralidade não encontrou raízes pessoais identificadas, emails antigos ou valores das credenciais conhecidas. Documentos privados, dados ativos, IDE, Qodana e skills não aparecem nas árvores destinadas ao envio.

O mapa de commits e as regras com valores antigos ficam apenas na evidência privada externa. Branches antigas e backups foram preservados localmente; não foram enviados ao servidor. O push padrão do projeto foi limitado à main, sem envio automático de tags.

## Envio privado e bloqueio encontrado

Antes do envio, o único head remoto foi confirmado como conhecido e ancestral da base original; não havia avanço divergente desconhecido, PRs, Actions, forks ou releases. Foi enviada **somente main**, usando lease exato para rejeitar qualquer atualização concorrente. Nenhum histórico remoto divergente foi sobrescrito.

A main saneada enviada foi `e93bb66f3067a772a5bde7006090801fba2304bc`. A main local foi alinhada à mesma história sem apagar as branches antigas; este relatório acrescenta apenas documentação local posterior.

**Depois da troca da referência, a API autenticada do GitHub ainda retornou o commit remoto antigo com e-mail de autor/committer não autorizado.** Os valores não foram reproduzidos. Tornar o repositório público poderia expor esse objeto retido, contrariando a condição do proprietário. A visibilidade não foi alterada, nenhum pacote foi anexado e nenhuma release/tag foi publicada.

A documentação oficial esclarece que reescrever/forçar as referências não elimina necessariamente commits acessíveis por hash ou cache: [remoção de dados no GitHub](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/removing-sensitive-data-from-a-repository). Portanto não se apresentou o saneamento dos heads como expurgo completo do servidor.

## Validação e artefatos

Código funcional preservado; validação novamente executada na main saneada com dados isolados:

- Build: **0 avisos, 0 erros**.
- Regressão C#: **1707 aprovados, 0 reprovados, 0 ignorados**.
- Python: **41 aprovados** (29 publicação/credenciais e 12 instalador).
- As 29 verificações da preparação anterior permanecem como evidência histórica; não foram apresentadas como nova execução após a reescrita. A auditoria desta retomada é a comparação de 11200 arquivos/commit e dos 835 objetos destinados ao envio.
- SHA-256 do novo TRX: `2aaa2ad0480457a9fad907d48de9fd853ac8961145092de8b563608213d905ed`.

Os 169 cenários atendidos e os 37 não executados da matriz continuam registrados, sem aprovação nativa presumida.

Os três pacotes **candidato4** mantêm exatamente os bytes auditados. ZIP/TAR e inventários foram conferidos na preparação anterior, e seus hashes foram novamente verificados nesta retomada. Nenhuma reconstrução ou upload foi feito. Proveniência original dos binários continua registrada nos manifestos; a reescrita documental não recompilou os executáveis.

| Artefato | SHA-256 |
|---|---|
| MinhasRealizacoes-0.1.0-beta.1-win-x64.zip | `e517caddcab36a883351894b84504565801610dfe65fea5d57158d2972664805` |
| MinhasRealizacoes-0.1.0-beta.1-win-x64-setup.exe | `73169f9ecd24da3af18c05841f435ce6bced3baf9d133b46a764127949f056c2` |
| MinhasRealizacoes-0.1.0-beta.1-linux-x64.tar.gz | `55b2ed4c7683db65e5614d1fba709a9b04dd8934854c9a24a9c6abcb9a2441dc` |

Documentos privados, contrato, dados, preferências e pessoais conhecidos mantiveram seus hashes contra o backup. Os relatórios automatizados históricos foram preservados, com apenas as substituições documentais autorizadas no histórico.

## Pendência para publicação

É necessário resolver a retenção dos objetos antigos no GitHub e reauditar sua acessibilidade **antes** de alterar a visibilidade. Uma solicitação de análise ao suporte pode ser necessária; a remoção depende das políticas do provedor e não foi prometida. Outra estratégia de repositório precisaria preservar o atual privado e seus backups, sem renomear/excluir recursos por suposição.

Nenhuma mensagem foi enviada ao suporte em nome do proprietário. Não houve tentativa de contornar o bloqueio tornando o repositório público, excluindo o destino ou publicando em outro endereço. Os links públicos de release/download e a consulta anônima de uma pre-release permanecem pendentes, pois o lançamento público foi interrompido conforme os critérios de segurança autorizados.

**Cuidado com as branches locais antigas:** foram preservadas para recuperação, mas ainda apontam ao histórico anterior. Não utilizar push `--all`/`--mirror`; o push padrão foi restrito à main saneada.

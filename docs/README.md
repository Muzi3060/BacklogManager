# Documentação do Minhas Realizações

O [README principal](../README.md) apresenta a versão de desenvolvimento atual. A primeira beta ainda está em preparação; a Etapa 2 está integrada por fast-forward. A implementação da Etapa 3 está na branch `feat/beta-distribuicao`, com candidatos locais e validações nativas pendentes; merge e publicação dependem de aprovação própria.

- [Simplificação da interface](BETA_ETAPA1.md) e [resultados automatizados](BETA_ETAPA1_TESTES.md).
- [Aprovação manual geral da Etapa 1](APROVACAO_BETA_ETAPA1.md), sem inferir execuções individuais.
- [Auditoria e correções preventivas da Etapa 2](BETA_ETAPA2.md).
- [Planejamento aprovado da distribuição](PLANEJAMENTO_BETA_ETAPA3.md).
- [Implementação, pacotes e validação manual da Etapa 3](BETA_ETAPA3.md).
- [Resultados e cobertura prevista da Etapa 3](BETA_ETAPA3_TESTES.md).

## Relatórios históricos e caminhos genéricos

Os documentos `FASE*`, `REVISAO*`, `AJUSTES*`, arquivos de verificações e resultados registram o projeto no momento de cada execução. Nomes, funcionalidades e comandos de uma fase antiga podem ter sido substituídos em etapas aprovadas posteriores. Esses registros não equivalem a uma nova execução de testes.

As correções preventivas da Etapa 2 substituíram raízes pessoais em 19 documentos atuais. Foram preservados os resultados, contagens, hashes, datas e demais campos; somente referências de caminhos foram alteradas. Cópias integrais anteriores foram guardadas privadamente fora do repositório.

| Referência documental | Significado |
|---|---|
| `${PROJETO}` | Raiz da cópia local deste projeto |
| `${HOME}` | Pasta pessoal do usuário no sistema |
| `/tmp/...` | Caminho temporário de uma execução histórica; pode não existir mais |

Essas referências em JSON e tabelas são **descrições**, não novos caminhos configurados pelo aplicativo. Para adaptar um comando histórico, substitua a referência pela sua pasta; em shell, defina a variável correspondente e use aspas quando necessário. A sanitização não move arquivos e não altera a persistência.

As chaves de mapas JSON que representavam nomes absolutos de arquivos também foram generalizadas. Seus valores de hash continuam os mesmos. Não use uma evidência histórica como se fosse uma configuração atual ou uma instrução automática de restauração.

A limpeza da versão atual **não remove caminhos, nomes ou e-mails do histórico Git**. Commits antigos e branches foram preservados. A decisão sobre eventual exposição desses metadados continua com o proprietário; nenhuma reescrita foi autorizada ou realizada.

Os relatórios de auditoria não contêm valores completos de credenciais. As instruções privadas da beta, arquivos pessoais de ferramentas, bibliotecas reais e backups devem permanecer fora do versionamento. Regras de `.gitignore` ajudam no staging comum; não substituem a revisão de arquivos, referências e futuros pacotes antes de publicar.

- [Resultado da preparação do lançamento e bloqueio de publicidade](LANCAMENTO_BETA_RESULTADO.md).

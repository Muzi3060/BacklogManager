# Publicação da beta 0.1.0-beta.1

## Escopo e autorização

O proprietário confirmou e autorizou exclusivamente as duas grafias de seu Hotmail pessoal presentes nos quatro commits antigos retidos no GitHub, incluindo uma grafia incorreta. Nenhum e-mail institucional, senha, credencial privada ou outro dado sensível recebeu autorização por extensão. Os valores antigos não foram adicionados à história saneada para registrar essa decisão.

O bloqueio anterior foi resolvido por essa confirmação expressa, após a auditoria específica de 67 hashes antigos: quatro commits acessíveis, 63 respostas “No commit found for SHA” (HTTP 422), sem consultas inconclusivas. As quatro árvores completas e 22 blobs únicos (173320 bytes) foram obtidos pela API e seus hashes Git conferidos. Não foram encontrados outros e-mails nos arquivos/mensagens, dados ativos pessoais, documentos privados, caminhos pessoais identificados ou segredos pelas regras aplicadas. A conclusão é limitada ao conjunto conhecido e às técnicas usadas, não uma garantia sobre objetos desconhecidos.

## Código, privacidade e licença

A main foi saneada e usa a identidade pública confirmada. Foram transformados 67 commits na cópia isolada e verificadas 11200 combinações arquivo/commit, com preservação dos blobs do contrato e dos fontes funcionais. Backups integrais e bundles continuam privados e recuperáveis. Branches antigas ficaram somente locais e não devem ser enviadas com `--all`/`--mirror`.

MIT, copyright 2026 Murilo Martins Ribeiro. Avisos de terceiros preservados. Os documentos privados de instruções/autorização, biblioteca, coleções pessoais, preferências, Qodana e skills não fazem parte da distribuição de fontes ou binários.

Apenas os três artefatos candidato4 auditados serão anexados; seus bytes não foram alterados nesta preparação. As credenciais IGDB da beta permanecem incorporadas por decisão expressa. Nenhum Client Secret, backend ou autenticação individual foi introduzido. Os manifestos conservam a proveniência original dos binários e a evidência privada contém o mapa para a história saneada.

## Links oficiais

- Repositório: https://github.com/Muzi3060/BacklogManager
- Pre-release: https://github.com/Muzi3060/BacklogManager/releases/tag/v0.1.0-beta.1
- Windows ZIP: https://github.com/Muzi3060/BacklogManager/releases/download/v0.1.0-beta.1/MinhasRealizacoes-0.1.0-beta.1-win-x64.zip
- Windows EXE: https://github.com/Muzi3060/BacklogManager/releases/download/v0.1.0-beta.1/MinhasRealizacoes-0.1.0-beta.1-win-x64-setup.exe
- Linux TAR.GZ: https://github.com/Muzi3060/BacklogManager/releases/download/v0.1.0-beta.1/MinhasRealizacoes-0.1.0-beta.1-linux-x64.tar.gz
- Checksums: https://github.com/Muzi3060/BacklogManager/releases/download/v0.1.0-beta.1/SHA256SUMS.txt

Os endereços são os destinos da publicação. A verificação de acessibilidade anônima será registrada após a criação e liberação efetivas; esta seção não substitui a evidência de publicação.

| Artefato | SHA-256 |
|---|---|
| Windows ZIP | `e517caddcab36a883351894b84504565801610dfe65fea5d57158d2972664805` |
| Windows EXE | `73169f9ecd24da3af18c05841f435ce6bced3baf9d133b46a764127949f056c2` |
| Linux TAR.GZ | `55b2ed4c7683db65e5614d1fba709a9b04dd8934854c9a24a9c6abcb9a2441dc` |

## Testes e limitações aceitas

A main saneada passou em build com 0 avisos/erros, 1707 testes C# e 41 testes Python; evidências anteriores permanecem preservadas. Revalidações finais têm resultado próprio, sem converter planejamento em aprovação.

Dos 206 cenários planejados, 169 atendidos e **37 nativos não executados**: 27 Windows e 10 Ubuntu. Os sistemas-alvo são Windows 11 x64 e Ubuntu 24.04 LTS x64. Wine, compilação cruzada, Headless e execução no CachyOS não equivalem à qualificação nativa desses sistemas.

O proprietário relatou validação manual CachyOS com dados fictícios: inicialização/navegação, persistência de jogos/temas, pesquisa/cadastro/edição/remoção pela IGDB, coleções, tratamento de releases indisponíveis e troca de binários com dados preservados. Não atribuir aprovação manual a offline, migração completa ou descoberta real de novas versões. Verificar a listagem pública da release não é executar esse fluxo completo pela interface.

Outras limitações: catálogos de cinco mídias ainda simulados e identificados; token IGDB compartilhado pode expirar/atingir limites; capas remotas dependem de rede/cache; sem assinatura de código; atualização e instalação manuais; backup/restauração coordenados por diário sem atomicidade absoluta entre arquivos ou proteção contra toda falha física.

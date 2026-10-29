# Beta — Etapa 2: verificações das correções preventivas

> Atualização: o proprietário aprovou oficialmente a Etapa 2 após revisar os resultados. Registro em [APROVACAO_BETA_ETAPA2.md](APROVACAO_BETA_ETAPA2.md). Tabelas e totais abaixo continuam históricos; não representam nova execução nem aprovação individual de testes manuais.

Tabelas geradas dos resultados reais do verificador, das guardas CLI, da ferramenta de triagem e do TRX desta continuação. Não substituem os relatórios históricos das fases anteriores.

**Verificações específicas: 139 planejadas, 139 executadas/aprovadas, 0 reprovadas, 0 não executadas. Regressão: 1541 executados/aprovados, 0 reprovados, 0 ignorados. Build: 0 avisos e 0 erros.** São 1680 verificações/testes nesta entrega, sem somar reexecuções como cenários novos.

Os 139 checks são 120 do verificador (com manifesto privado), 3 guardas CLI com arquivos fictícios e 16 casos sintéticos da triagem. Não são apresentados como novas funcionalidades do aplicativo. Os resultados estruturados estão em [resultados-beta-etapa2-correcoes.json](resultados-beta-etapa2-correcoes.json).

O verificador público, executado sem o manifesto privado, informa PR05–PR07 como não executadas; isso não altera a execução completa registrada aqui. A regressão usou diretórios temporários, dados simulados e HTTP simulado. Nenhum teste destrutivo foi executado sobre a biblioteca pessoal.

## Exclusões e staging em repositório temporário — 43 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| IG01 | instrução privada da beta | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG02 | cópia privada da instrução | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG03 | instrução local genérica | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG04 | instrução privada em subpasta | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG05 | configuração pessoal Qodana | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG06 | habilidade pessoal | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG07 | biblioteca na raiz | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG08 | biblioteca em subpasta | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG09 | cópia da biblioteca com hífen | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG10 | cópia da biblioteca com sublinhado | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG11 | coleções na raiz | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG12 | coleções em subpasta | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG13 | preferência de visualização | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG14 | preferência de aparência | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG15 | lock de biblioteca | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG16 | lock de coleções | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG17 | lock de aparência | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG18 | lock de visualização | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG19 | diário de recuperação | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG20 | lock do conjunto | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG21 | exportação com nome padrão | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG22 | exportação com nome padrão e data | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG23 | segurança anterior à restauração | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG24 | backup nomeado com hífen | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG25 | backup nomeado com sublinhado | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG26 | exportação nomeada com hífen | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG27 | exportação nomeada com sublinhado | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG28 | diretório de backups | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG29 | diretório de backups com maiúscula | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG30 | diretório de exports | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG31 | diretório de exportações | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG32 | diretório de dados | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG33 | diretório de dados pessoais | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG34 | diretório de dados locais | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG35 | backup individual | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG36 | configuração da IDE | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG37 | credenciais de ambiente | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG38 | variante local de ambiente | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG39 | log de execução | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG40 | saída de compilação | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG41 | cache de compilação | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG42 | resultado de testes | Ignorado pelas regras compartilhadas | Ignorado pelas regras compartilhadas | Automatizado (verificação estática/isolada) | Aprovado |
| IG43 | staging comum em clone sem exclusão local | Somente README e .gitignore entram no dry-run | Somente README e .gitignore entram no dry-run | Automatizado (verificação estática/isolada) | Aprovado |

## Arquivos legítimos continuam versionáveis — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| IP01 | código C# | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |
| IP02 | interface Avalonia | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |
| IP03 | documentação pública | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |
| IP04 | contrato protegido e versionado | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |
| IP05 | instrução histórica sem marca privada | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |
| IP06 | relatório JSON | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |
| IP07 | fixture JSON com nome explícito | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |
| IP08 | fixture de backup em testes | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |
| IP09 | configuração de exemplo | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |
| IP10 | recurso SVG | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |
| IP11 | roteiro de validação | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |
| IP12 | JSON genérico | Continua versionável | Continua versionável | Automatizado (verificação estática/isolada) | Aprovado |

## Generalização de caminhos e rejeição de JSON ambíguo — 17 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| SN01 | projeto Linux | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN02 | backup Linux | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN03 | projeto macOS | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN04 | home macOS | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN05 | projeto Windows | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN06 | home Windows | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN07 | caminho Windows escapado em JSON | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN08 | raiz home sem sufixo | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN09 | nome de projeto semelhante | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN10 | caminho genérico já sanitizado | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN11 | placeholder anterior | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN12 | caminho temporário útil | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN13 | URL de catálogo | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN14 | contagens resultados e hash | Substitui somente a raiz pessoal prevista | Substitui somente a raiz pessoal prevista | Automatizado (verificação estática/isolada) | Aprovado |
| SN15 | normalização que colidiria duas chaves pessoais | Colisão detectada sem descartar silenciosamente uma chave | Colisão detectada sem descartar silenciosamente uma chave | Automatizado (verificação estática/isolada) | Aprovado |
| SN16 | propriedade duplicada em objeto aninhado | JSON ambíguo recusado | JSON ambíguo recusado | Automatizado (verificação estática/isolada) | Aprovado |
| SN17 | JSON truncado na verificação de preservação | JSON inválido recusado | JSON inválido recusado | Automatizado (verificação estática/isolada) | Aprovado |

## Preservação integral dos documentos sob substituição de raízes — 19 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| DH01 | docs/AJUSTES_FASE3.md | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH02 | docs/EVIDENCIAS_FASE7.json | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH03 | docs/FASE0.md | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH04 | docs/FASE1.md | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH05 | docs/FASE2.md | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH06 | docs/FASE3.md | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH07 | docs/FASE4.md | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH08 | docs/FASE5.md | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH09 | docs/FASE7.md | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH10 | docs/REVISAO2_FASE3.md | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH11 | docs/verificacoes-ajustes-fase3.json | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH12 | docs/verificacoes-fase3.json | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH13 | docs/verificacoes-fase4.json | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH14 | docs/verificacoes-fase5.json | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH15 | docs/verificacoes-fase6.json | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH16 | docs/verificacoes-integracao-fase5.json | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH17 | docs/verificacoes-integracao-fase6.json | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH18 | docs/verificacoes-revisao2-fase3.json | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |
| DH19 | docs/verificacoes-revisao3-fase3.json | Documento histórico difere somente pelas raízes de caminhos | Documento histórico difere somente pelas raízes de caminhos | Automatizado (verificação estática/isolada) | Aprovado |

## Validade e preservação semântica dos JSON históricos — 10 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| JS02 | docs/EVIDENCIAS_FASE7.json | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | Automatizado (verificação estática/isolada) | Aprovado |
| JS11 | docs/verificacoes-ajustes-fase3.json | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | Automatizado (verificação estática/isolada) | Aprovado |
| JS12 | docs/verificacoes-fase3.json | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | Automatizado (verificação estática/isolada) | Aprovado |
| JS13 | docs/verificacoes-fase4.json | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | Automatizado (verificação estática/isolada) | Aprovado |
| JS14 | docs/verificacoes-fase5.json | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | Automatizado (verificação estática/isolada) | Aprovado |
| JS15 | docs/verificacoes-fase6.json | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | Automatizado (verificação estática/isolada) | Aprovado |
| JS16 | docs/verificacoes-integracao-fase5.json | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | Automatizado (verificação estática/isolada) | Aprovado |
| JS17 | docs/verificacoes-integracao-fase6.json | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | Automatizado (verificação estática/isolada) | Aprovado |
| JS18 | docs/verificacoes-revisao2-fase3.json | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | Automatizado (verificação estática/isolada) | Aprovado |
| JS19 | docs/verificacoes-revisao3-fase3.json | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | JSON válido; campos e resultados preservados sob a mesma substituição de raízes | Automatizado (verificação estática/isolada) | Aprovado |

## Documentação pública coerente com o projeto — 12 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| PB01 | nome público | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |
| PB02 | beta em desenvolvimento | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |
| PB03 | SDK coincide com o projeto | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |
| PB04 | projeto indicado existe | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |
| PB05 | execução fora do repositório | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |
| PB06 | três nomes de configuração IGDB aceitos | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |
| PB07 | armazenamento ativo explicitado | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |
| PB08 | links locais do README existentes | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |
| PB09 | gerador fictício existente e documentado | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |
| PB10 | regressão documentada sem executar aplicativo real | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |
| PB11 | placeholders e histórico esclarecidos | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |
| PB12 | licença não escolhida e pacotes não prometidos | Documentação coerente com o projeto e seus limites | Documentação coerente com o projeto e seus limites | Automatizado (verificação estática/isolada) | Aprovado |

## Preservação e privacidade — 7 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| PR01 | documentação atual sem raízes pessoais concretas | Nenhuma raiz pessoal concreta nos documentos atuais | Nenhuma raiz pessoal concreta nos documentos atuais | Automatizado (verificação estática/isolada) | Aprovado |
| PR02 | código C# e Avalonia preservado | Nenhum fonte do aplicativo alterado desde a base auditada | Nenhum fonte do aplicativo alterado desde a base auditada | Automatizado (verificação estática/isolada) | Aprovado |
| PR03 | contrato preservado | Contrato idêntico ao da base auditada | Contrato idêntico ao da base auditada | Automatizado (verificação estática/isolada) | Aprovado |
| PR04 | pessoais fora do índice | Instrução beta Qodana e skills não rastreados | Instrução beta Qodana e skills não rastreados | Automatizado (verificação estática/isolada) | Aprovado |
| PR05 | pessoais e dados locais | Hashes iguais ao manifesto privado anterior | Hashes iguais ao manifesto privado anterior | Automatizado (verificação estática/isolada) | Aprovado |
| PR06 | backups externos | Hashes iguais ao manifesto privado anterior | Hashes iguais ao manifesto privado anterior | Automatizado (verificação estática/isolada) | Aprovado |
| PR07 | outras referências Git | Nenhuma referência anterior alterada | Nenhuma referência anterior alterada | Automatizado (verificação estática/isolada) | Aprovado |

## Guardas de saída do verificador — 3 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| CL01 | Saída dentro do repositório recusada | Recusa explícita sem criar ou sobrescrever arquivo | Recusa explícita sem criar ou sobrescrever arquivo | Automatizado (CLI em dados fictícios) | Aprovado |
| CL02 | Nome de biblioteca recusado fora do repositório | Recusa explícita sem criar ou sobrescrever arquivo | Recusa explícita sem criar ou sobrescrever arquivo | Automatizado (CLI em dados fictícios) | Aprovado |
| CL03 | Saída existente preservada | Recusa explícita sem criar ou sobrescrever arquivo | Recusa explícita sem criar ou sobrescrever arquivo | Automatizado (CLI em dados fictícios) | Aprovado |

## Autoverificação da triagem de segredos — 16 cenários

| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |
|---|---|---|---|---|---|
| DS01 | chave privada | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS02 | token GitHub | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS03 | secret literal | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS04 | password env | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS05 | header literal | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS06 | url autenticada | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS07 | URL com token | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS08 | token simulado | Detecção esperada: False | Detecção observada: False | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS09 | nome de variável | Detecção esperada: False | Detecção observada: False | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS10 | SHA hexadecimal | Detecção esperada: False | Detecção observada: False | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS11 | e-mail | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS12 | caminho local | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS13 | JSON de biblioteca | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS14 | binário | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS15 | backup codificado | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |
| DS16 | env Rider | Detecção esperada: True | Detecção observada: True | Automatizado (triagem com dados sintéticos) | Aprovado |

## Reexecuções e limitações

Os primeiros 117 checks específicos passaram. Foram adicionados três casos negativos de JSON (colisão de chaves após generalização, propriedade repetida e conteúdo truncado); a execução final completa passou em 120/120. As revisões posteriores do verificador também passaram. Não foram observados bugs no aplicativo, e seus fontes permaneceram idênticos à base auditada.

Os 19 documentos foram comparados integralmente com `c14f588`, admitindo somente a substituição das raízes pessoais. Os 10 JSON afetados também passaram por parsing com rejeição de chaves repetidas e comparação de conteúdo. Datas, resultados, contagens e hashes não foram editados.

O build e a regressão completa foram executados uma vez nesta continuação. Mudanças posteriores ficaram no verificador/documentação, com as verificações específicas repetidas; não se afirma uma segunda execução de `dotnet test`. O TRX e seu hash estão registrados no JSON acima.

A nova varredura examinou os 53 commits/438 blobs existentes em `c14f588`, além da árvore de trabalho naquele momento; a documentação criada depois disso recebe revisão adicional de conteúdo antes do commit. O histórico conserva caminhos e identidades antigos por determinação do proprietário.

Nenhum pacote de distribuição Windows/Linux ou verificador de atualizações foi implementado/testado. A disponibilidade real da IGDB não foi consultada; não houve nova validação manual no Rider. Essas verificações de etapas futuras não são contadas como aprovadas nem como parte das 139 verificações executadas.

A aprovação das correções e sua integração à main permanecem pendentes. Nenhum merge, push, release ou publicação foi realizado.

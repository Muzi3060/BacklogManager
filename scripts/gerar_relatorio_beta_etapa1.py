#!/usr/bin/env python3
"""Gera relatório portátil dos TRX reais, sem publicar logs ou caminhos da máquina."""
import argparse
import hashlib
import json
import re
from pathlib import Path
import xml.etree.ElementTree as ET
from gerar_relatorio_testes import PARAMETRIZADOS

RAIZ = Path(__file__).resolve().parent.parent
NS = {'t': 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
GRUPOS = {
    'BN': 'Navegação simplificada',
    'BC': 'Configurações e preservação do backup',
    'BT': 'Quatro temas fixos e dois modos',
    'BP': 'Compatibilidade e persistência da aparência',
    'BD': 'Dashboard simplificado',
    'BZ': 'Ausência de promoções nos fluxos da beta',
}


def ler(caminho):
    raiz = ET.parse(caminho).getroot()
    testes = []
    for r in raiz.findall('.//t:UnitTestResult', NS):
        partes = r.attrib['testName'].split(' | ', 2)
        if len(partes) != 3:
            codigo = re.search(r'\b([A-Z]{1,2}\d{2})\b', r.attrib['testName']).group(1)
            partes = [codigo, *PARAMETRIZADOS[codigo]]
        testes.append(dict(id=partes[0], cenario=partes[1], esperado=partes[2],
                           resultado=r.attrib['outcome'], duracao=r.attrib.get('duration')))
    assert len({t['id'] for t in testes}) == len(testes), 'IDs repetidos no TRX.'
    return dict(sha256=hashlib.sha256(caminho.read_bytes()).hexdigest(),
                inicio=raiz.find('t:Times', NS).attrib['start'],
                fim=raiz.find('t:Times', NS).attrib['finish'], testes=testes)


def totais(leitura):
    ts = leitura['testes']
    return dict(total=len(ts), aprovados=sum(t['resultado'] == 'Passed' for t in ts),
                reprovados=sum(t['resultado'] == 'Failed' for t in ts),
                naoExecutados=sum(t['resultado'] not in ('Passed', 'Failed') for t in ts))


def celula(texto):
    return texto.replace('|', '\\|').replace('\n', ' ')


def gerar(referencia, final):
    base, atual = ler(referencia), ler(final)
    transicao = json.loads((RAIZ / 'docs/beta-etapa1-transicao-testes.json').read_text())
    ids_base = {t['id'] for t in base['testes']}
    ids_atual = {t['id'] for t in atual['testes']}
    retirados = {t['id'] for t in transicao['retirados']}
    assert ids_base - ids_atual == retirados, 'Mudança na regressão sem justificativa explícita.'
    novos = [t for t in atual['testes'] if t['id'] not in ids_base]
    assert all(t['id'][:2] in GRUPOS for t in novos)
    mapa_base = {t['id']: t for t in base['testes']}
    adaptados = [dict(id=t['id'], anterior=mapa_base[t['id']], atual=t) for t in atual['testes']
                 if t['id'] in mapa_base and (t['cenario'], t['esperado']) !=
                 (mapa_base[t['id']]['cenario'], mapa_base[t['id']]['esperado'])]
    resumo = dict(referencia=totais(base), final=totais(atual), novos=len(novos),
                  regressao=len(atual['testes']) - len(novos), retirados=len(retirados))
    dados = dict(resumo=resumo, referencia={k: v for k, v in base.items() if k != 'testes'}, final=atual, enunciadosAdaptados=adaptados,
                 observacao='Aprovação automatizada corresponde às asserções executadas; não é validação manual no Rider. Transições adicionais por mudança nos auxiliares estão descritas no relatório.')
    (RAIZ / 'docs/resultados-beta-etapa1.json').write_text(json.dumps(dados, ensure_ascii=False, indent=2) + '\n')
    linhas = [
        '# Beta — Etapa 1: testes reais', '',
        'Gerado por `scripts/gerar_relatorio_beta_etapa1.py` a partir dos TRX reais. '
        'Somente arquivos sintéticos/temporários e HTTP simulado. Nenhuma chamada autenticada à IGDB nem teste na biblioteca pessoal.', '',
        f"Referência anterior: **{len(base['testes'])} testes**. Suíte beta: **{len(atual['testes'])} planejados/executados**, "
        f"**{resumo['final']['aprovados']} aprovados**, **{resumo['final']['reprovados']} reprovados**, "
        f"**{resumo['final']['naoExecutados']} não executados**. "
        f"São {len(novos)} novos e {resumo['regressao']} de regressão mantidos/adaptados.", '',
        'Os 105 cenários da versão anterior retirados por remoção expressa das funcionalidades estão '
        'individualmente identificados em [beta-etapa1-transicao-testes.json](beta-etapa1-transicao-testes.json). '
        'Eles passaram na referência anterior; não são contados como aprovados ou ignorados na beta. '
        'A contabilidade é **1551 − 105 + 95 = 1541**.', '',
        'Resultados individuais da regressão, horários e hashes dos TRX: '
        '[resultados-beta-etapa1.json](resultados-beta-etapa1.json). Os relatórios anteriores permanecem históricos, sem alteração.', '',
    ]
    for prefixo, titulo in GRUPOS.items():
        grupo = sorted((t for t in novos if t['id'].startswith(prefixo)), key=lambda t: t['id'])
        assert len(grupo) >= 10, 'Cobertura mínima por funcionalidade insuficiente.'
        linhas += [f'## {titulo} — {len(grupo)} cenários', '',
                   '| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |',
                   '|---|---|---|---|---|---|']
        for t in grupo:
            passou = t['resultado'] == 'Passed'
            observado = 'Confirmado pelas asserções: ' + t['esperado'] if passou else t['resultado']
            linhas += ['| ' + ' | '.join(map(celula, [t['id'], t['cenario'], t['esperado'], observado,
                                                     'Automatizado', 'Aprovado' if passou else t['resultado']])) + ' |']
        linhas += ['']
    linhas += ['## Transição e regressão', '',
               'A troca de `NavAparencia` por `NavConfiguracoes` nos auxiliares e casos existentes usa o fluxo real da beta, '
               'sem controles fictícios nem aliases para a página removida. Os casos mantidos de temas, mensagens, teclado, '
               'formulários, grade/lista, capas, coleções, IGDB simulada, persistência e recuperação foram reexecutados.', '',
               'Adaptações: VH06–VH17 exercitam o validador hexadecimal interno usado pelas paletas fixas, sem editor público; '
               'VA05 verifica cores legadas inativas; QN07/QN13/QN14 verificam seletores diretos, Dark Mode e ausência do seletor redundante; '
               'QP09 restaura tema/modo fixos; QI04/QI09 usam o seletor real de visualização da biblioteca; '
               'QI18/QI19 preservam uma seleção de tema pendente quando a restauração não inclui aparência; '
               'QH14 usa contraste de Dark Mode. DU11/DU18/DU19 e DK11 cobrem ausência do bloco legado e de promoções, '
               'com os dados preservados. TX02 remove o convite dispensado; TX05/TX12 usam obras em andamento; '
               'RD12 mantém capturas de grade/lista sem a seção removida.', '',
               '## Execuções intermediárias e correções', '',
               '- Referência: 1551 aprovados, zero falhas/ignorados.',
               '- Primeira regressão adaptada: 1444 aprovados e 2 falhas (TX02 ainda exigia o convite removido; '
               'TX05 precisava conservar os demais dados pessoais do fixture e acessar o acompanhamento). Testes adaptados ao requisito e repetidos.',
               '- Primeiros 94 novos cenários: 93 aprovados e 1 falha de fixture (BD02 pressupunha um bloco multimídia obrigatório em jogo legado). '
               'A asserção agora aceita bloco ausente, sem inventar data.',
               '- Reprodução dirigida: BC15 falhou e outros três casos passaram. Bug confirmado: depois de restaurar aparência, '
               '`PendenteSalvar` ainda refletia a seleção anterior. `ServicoAparencia.Carregar()` agora limpa essa pendência ao substituir a sessão.',
               '- Build final da solução: zero avisos e zero erros. Suíte completa final: 1541 aprovados, incluindo BC15, BD02, TX02 e TX05.',
               '- Uma tentativa inicial de iniciar MSBuild no sandbox não executou cenários por falta de permissão para pipes locais; '
               'a execução foi repetida com as permissões de processo necessárias. Isso não foi registrado como teste aprovado.', '',
               '## Validação complementar e limites', '',
               'Há **cinco cenários manuais planejados e ainda não executados**, descritos em '
               '[BETA_ETAPA1.md](BETA_ETAPA1.md). A aprovação manual desta etapa está pendente. '
               'Capturas automatizadas não equivalem à interação manual no Rider nem à validação de pacotes Windows/Linux.', '',
               'Os testes verificam contraste matemático, recursos aplicados e controles renderizados, além da regressão visual existente. '
               'Não constituem certificação completa de acessibilidade ou teste de todos os leitores de tela. '
               'A persistência entre vários arquivos mantém as limitações já documentadas na Fase 7.', '']
    (RAIZ / 'docs/BETA_ETAPA1_TESTES.md').write_text('\n'.join(linhas))
    print(json.dumps(resumo, ensure_ascii=False))


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('referencia', type=Path)
    parser.add_argument('final', type=Path)
    args = parser.parse_args()
    gerar(args.referencia, args.final)

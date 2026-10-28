#!/usr/bin/env python3
"""Gera resultados da Fase 3 a partir de TRX sem modificar relatórios anteriores."""
import argparse
import json
import re
from pathlib import Path
import xml.etree.ElementTree as ET
from gerar_relatorio_testes import PARAMETRIZADOS
from gerar_relatorio_fase2 import GRUPOS as GRUPOS_ANTERIORES

RAIZ = Path(__file__).resolve().parents[1]
GRUPOS = {
    **GRUPOS_ANTERIORES,
    'QB': 'Consultas e filtros combinados da biblioteca',
    'OB': 'Ordenação estável da biblioteca',
    'DB': 'Datas, plataformas e compatibilidade JSON',
    'PV': 'Preferência independente de visualização',
    'CI': 'Metadados opcionais da IGDB',
    'CP': 'Capas: carregamento, cache, limites e falhas',
    'VG': 'Operações e apresentação em grade',
    'VL': 'Operações e apresentação em lista',
    'IB': 'Interação entre filtros, preferências e capas',
}


def gerar(trx):
    cenarios = dict(PARAMETRIZADOS)
    for fonte in (RAIZ / 'Tests/BacklogManager.Tests').rglob('*.cs'):
        if 'obj' in fonte.parts or 'bin' in fonte.parts:
            continue
        for codigo, texto in re.findall(r'DisplayName\s*=\s*"([A-Z]{1,2}\d{2}) \| ([^"\n]+)"', fonte.read_text()):
            assert codigo not in cenarios, f'ID duplicado: {codigo}'
            cenarios[codigo] = texto.split(' | ', 1)
    ns = {'t': 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
    arvore = ET.parse(trx)
    resultados = []
    for teste in arvore.findall('.//t:UnitTestResult', ns):
        codigo = re.search(r'\b([A-Z]{1,2}\d{2})\b', teste.attrib['testName']).group(1)
        cenario, esperado = cenarios[codigo]
        status = {'Passed': 'Aprovado', 'Failed': 'Reprovado', 'NotExecuted': 'Não executado'}.get(teste.attrib['outcome'], teste.attrib['outcome'])
        detalhe = teste.find('t:Output/t:ErrorInfo/t:Message', ns)
        observado = 'Confirmado pelas asserções: ' + esperado if status == 'Aprovado' else detalhe.text if detalhe is not None else teste.attrib['outcome']
        prefixo = re.match(r'[A-Z]+', codigo).group()
        tipo = 'Automatizado (Avalonia Headless/Skia)' if prefixo in ['A', 'E', 'R', 'B', 'F', 'UI', 'UM', 'CP', 'VG', 'VL', 'IB'] else 'Automatizado'
        resultados.append(dict(id=codigo, grupo=prefixo, cenario=cenario, esperado=esperado,
                               observado=observado, tipo=tipo, status=status, duracao=teste.attrib.get('duration', '')))
    assert len({r['id'] for r in resultados}) == len(resultados), 'Resultados duplicados'
    for prefixo in GRUPOS:
        assert sum(r['grupo'] == prefixo for r in resultados) >= 10, f'Mínimo de 10 não atendido: {prefixo}'
    assert set(cenarios) == {r['id'] for r in resultados}, 'Há cenários planejados ausentes no TRX'
    resultados.sort(key=lambda r: (list(GRUPOS).index(r['grupo']), r['id']))
    totais = {s: sum(r['status'] == s for r in resultados) for s in ['Aprovado', 'Reprovado', 'Não executado']}
    dados = dict(origem=Path(trx).name, base='64fb4fb', total=len(resultados), totais=totais, resultados=resultados)
    (RAIZ / 'docs/resultados-fase3.json').write_text(json.dumps(dados, indent=2, ensure_ascii=False) + '\n')
    linhas = ['# Fase 3 — Resultados individuais', '',
              'Gerado do TRX real por `scripts/gerar_relatorio_fase3.py`. Inclui todos os 535 cenários anteriores '
              'executados novamente sobre a Fase 3. H01 e Q04–Q10 foram adaptados aos novos campos opcionais '
              'solicitados à IGDB, preservando as verificações de protocolo e escape. Nenhum cenário foi removido; '
              'os relatórios históricos continuam intactos.', '',
              f"**Planejados na suíte: {len(cenarios)}; executados: {sum(r['status'] != 'Não executado' for r in resultados)}; "
              f"aprovados: {totais['Aprovado']}; reprovados: {totais['Reprovado']}; não executados: {totais['Não executado']}.**", '',
              'Aprovação corresponde às asserções executadas. A suíte usa dados simulados, arquivos temporários e HTTP '
              'simulado; não usa a biblioteca original. Não representa nova validação autenticada da IGDB ou interação '
              'manual no Rider. Verificações adicionais e pendências constam em `FASE3.md`.', '']
    for prefixo, nome in GRUPOS.items():
        registros = [r for r in resultados if r['grupo'] == prefixo]
        linhas += [f'## {nome} — {len(registros)} cenários', '',
                   '| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |',
                   '|---|---|---|---|---|---|']
        for r in registros:
            linhas.append('| ' + ' | '.join(str(r[k]).replace('|', '\\|').replace('\n', '<br>') for k in ['id', 'cenario', 'esperado', 'observado', 'tipo', 'status']) + ' |')
        linhas.append('')
    (RAIZ / 'docs/FASE3_TESTES.md').write_text('\n'.join(linhas))
    print(json.dumps(dict(total=len(resultados), **totais), ensure_ascii=False))


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('trx', type=Path)
    gerar(parser.parse_args().trx)

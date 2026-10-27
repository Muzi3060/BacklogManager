#!/usr/bin/env python3
"""Gera evidências por cenário a partir do TRX, sem ler a coleção pessoal."""
import argparse
import json
import re
from pathlib import Path
import xml.etree.ElementTree as ET

RAIZ = Path(__file__).resolve().parents[1]
PARAMETRIZADOS = {
    'P04': ('Arquivo vazio', 'Erro de leitura; gravação bloqueada; bytes originais preservados'),
    'P05': ('JSON com sintaxe incompleta', 'Erro de leitura; gravação bloqueada; bytes originais preservados'),
    'P06': ('Raiz JSON null', 'Erro de leitura; gravação bloqueada; original preservado'),
    'P07': ('Raiz JSON objeto em vez de lista', 'Erro de leitura; gravação bloqueada; original preservado'),
    'P08': ('Registro null na lista', 'Lista inteira recusada sem descarte parcial'),
    'P09': ('Título nulo no registro', 'Lista inteira recusada sem descarte parcial'),
    'P10': ('Categoria nula no registro', 'Lista inteira recusada sem descarte parcial'),
    'P11': ('Status nulo no registro', 'Lista inteira recusada sem descarte parcial'),
    'P12': ('Nota fora de 0 a 5', 'Lista inteira recusada sem descarte parcial'),
    'P13': ('Review nula no registro', 'Lista inteira recusada sem descarte parcial'),
    'H03': ('HTTP 401', 'Mensagem de autenticação; uma chamada; corpo sensível não exposto'),
    'H04': ('HTTP 403', 'Mensagem de autenticação; uma chamada; corpo sensível não exposto'),
    'H05': ('HTTP 429', 'Mensagem de limite; uma chamada; sem repetição automática'),
    'H06': ('HTTP 500', 'Mensagem de indisponibilidade; uma chamada'),
    'H07': ('HTTP 503', 'Mensagem de indisponibilidade; uma chamada'),
    'H08': ('HTTP 400', 'Mensagem com status HTTP 400 sem corpo da resposta'),
    'H09': ('HTTP 404', 'Mensagem com status HTTP 404 sem corpo da resposta'),
    'H12': ('Resposta JSON incompleta', 'Falha controlada sem retornar lista parcial'),
    'H13': ('Resposta JSON null', 'Falha controlada sem retornar lista vazia enganosa'),
    'H14': ('Resposta objeto em vez de lista', 'Falha controlada sem retornar lista parcial'),
    'H15': ('Elemento null na resposta', 'Falha controlada sem retornar lista parcial'),
    'H16': ('Resultado com ID zero', 'Falha controlada; resultado inválido não selecionável'),
    'H17': ('Resultado com ID negativo', 'Falha controlada; resultado inválido não selecionável'),
    'H18': ('Resultado com nome null', 'Falha controlada sem retornar lista parcial'),
    'H19': ('Resultado com nome em branco', 'Falha controlada sem retornar lista parcial'),
    'H20': ('ID textual não numérico', 'Falha controlada de desserialização'),
    'H21': ('Resposta vazia', 'Falha controlada sem confundir com zero resultados'),
    'Q01': ('Termo null', 'Lista vazia; nenhuma chamada HTTP'),
    'Q02': ('Termo vazio', 'Lista vazia; nenhuma chamada HTTP'),
    'Q03': ('Termo com somente espaços', 'Lista vazia; nenhuma chamada HTTP'),
    'Q04': ('Espaços nas bordas do termo', 'Consulta enviada com termo aparado'),
    'Q05': ('Aspas no termo', 'Aspas escapadas dentro da string APICalypse'),
    'Q06': ('Barras invertidas no termo', 'Barras escapadas dentro da string APICalypse'),
    'Q07': ('Unicode e emoji no termo', 'Texto preservado na consulta'),
    'Q08': ('Ponto e vírgula dentro do termo', 'Conteúdo mantido dentro da string de pesquisa'),
    'Q09': ('Tentativa de fechar string e inserir instrução', 'Aspas escapadas; termo não vira instrução APICalypse'),
    'Q10': ('Apóstrofo no título', 'Texto preservado na consulta'),
    'Q11': ('Quebra de linha no termo', 'Entrada recusada antes do envio HTTP'),
    'Q12': ('Retorno de carro no termo', 'Entrada recusada antes do envio HTTP'),
    'Q13': ('Caractere null no termo', 'Entrada recusada antes do envio HTTP'),
    'C01': ('Somente token com nome legado', 'Configuração válida; token legado utilizado'),
    'C02': ('Somente token com nome corrigido', 'Configuração válida; token corrigido utilizado'),
    'C03': ('Ambos os nomes de token configurados', 'Configuração válida; nome corrigido tem precedência'),
    'C04': ('Nome corrigido vazio e legado válido', 'Configuração válida usando o legado'),
    'C05': ('Nome corrigido com espaços e legado válido', 'Configuração válida usando o legado'),
    'C06': ('Credenciais com espaços nas bordas', 'Bordas aparadas; configuração válida'),
    'C07': ('Client ID ausente', 'Configuração indisponível sem exceção'),
    'C08': ('Client ID vazio', 'Configuração indisponível sem exceção'),
    'C09': ('Ambos os tokens ausentes', 'Configuração indisponível sem exceção'),
    'C10': ('Token legado somente com espaços', 'Configuração indisponível sem exceção'),
    'C11': ('Client ID com quebra de linha interna', 'Configuração inválida; nenhum header injetado'),
    'C12': ('Token com espaço interno', 'Configuração inválida; nenhum header enviado'),
    'C13': ('Token corrigido inválido e legado válido', 'Recusa explícita; sem troca silenciosa de credencial inválida'),
    'I01': ('ID legado positivo', 'Valor mantido; IgdbId não inferido nem serializado'),
    'I02': ('ID legado null', 'Valor null mantido; IgdbId não inferido'),
    'I03': ('ID legado zero', 'Valor legado mantido sem reinterpretação'),
    'I04': ('ID legado negativo', 'Valor legado mantido sem reinterpretação'),
    'I05': ('ID legado int.MaxValue', 'Valor legado mantido sem heurística de origem'),
}
GRUPOS = {
    'P': 'Persistência e preservação', 'I': 'Identificadores e compatibilidade',
    'C': 'Configuração e credenciais', 'H': 'HTTP e ciclo de vida dos recursos',
    'Q': 'Validação da consulta APICalypse', 'A': 'Cadastro e validação de formulário',
    'E': 'Edição e compatibilidade', 'R': 'Remoção e falhas de gravação',
    'B': 'Busca, cancelamento, janela e renderização', 'F': 'Regressão dos filtros',
}


def gerar(arquivo):
    cenarios = dict(PARAMETRIZADOS)
    for fonte in (RAIZ / 'Tests/BacklogManager.Tests').glob('*.cs'):
        for codigo, descricao in re.findall(r'DisplayName\s*=\s*"([A-Z]\d{2}) \| ([^"\n]+)"', fonte.read_text()):
            cenario, esperado = descricao.split(' | ', 1)
            cenarios[codigo] = (cenario, esperado)
    ns = {'t': 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
    arvore = ET.parse(arquivo)
    resultados = []
    for teste in arvore.findall('.//t:UnitTestResult', ns):
        codigo = re.search(r'\b([A-Z]\d{2})\b', teste.attrib['testName']).group(1)
        cenario, esperado = cenarios[codigo]
        status = {'Passed': 'Aprovado', 'Failed': 'Reprovado', 'NotExecuted': 'Não executado'}.get(teste.attrib['outcome'], teste.attrib['outcome'])
        detalhe = teste.find('t:Output/t:ErrorInfo/t:Message', ns)
        observado = ('Confirmado pelas asserções: ' + esperado if status == 'Aprovado'
                     else detalhe.text if detalhe is not None else teste.attrib['outcome'])
        resultados.append(dict(id=codigo, cenario=cenario, esperado=esperado, observado=observado,
                               tipo='Automatizado (Avalonia Headless/Skia)' if codigo[0] in 'AERBF' else 'Automatizado',
                               status=status, duracao=teste.attrib.get('duration', '')))
    resultados.sort(key=lambda r: (list(GRUPOS).index(r['id'][0]), r['id']))
    assert len({r['id'] for r in resultados}) == len(resultados), 'IDs duplicados'
    for prefixo in GRUPOS:
        assert sum(r['id'].startswith(prefixo) for r in resultados) >= 10, f'Menos de 10 cenários: {prefixo}'
    totais = {s: sum(r['status'] == s for r in resultados) for s in ['Aprovado', 'Reprovado', 'Não executado']}
    dados = {'origem': Path(arquivo).name, 'total': len(resultados), 'totais': totais, 'resultados': resultados}
    (RAIZ / 'docs/resultados-fase0.json').write_text(json.dumps(dados, indent=2, ensure_ascii=False) + '\n')
    linhas = ['# Fase 0 — Resultados individuais', '',
              'Gerado a partir do TRX real pelo script `scripts/gerar_relatorio_testes.py`. '
              'Cada aprovação significa que as asserções do cenário foram executadas sem falha; '
              'não representa validação autenticada da API ou inspeção manual no Rider.', '',
              f"Total da suíte: **{len(resultados)}**. Aprovados: **{totais['Aprovado']}**. "
              f"Reprovados: **{totais['Reprovado']}**. Não executados: **{totais['Não executado']}**.", '',
              'Os testes usam pastas temporárias próprias e HTTP simulado. Nenhum teste da suíte lê o JSON pessoal.', '']
    for prefixo, grupo in GRUPOS.items():
        grupo_resultados = [r for r in resultados if r['id'].startswith(prefixo)]
        linhas += [f'## {grupo} — {len(grupo_resultados)} cenários', '',
                   '| ID | Cenário testado | Resultado esperado | Resultado observado | Tipo | Status |',
                   '|---|---|---|---|---|---|']
        for r in grupo_resultados:
            linhas.append('| ' + ' | '.join(str(r[k]).replace('|', '\\|').replace('\n', '<br>') for k in ['id', 'cenario', 'esperado', 'observado', 'tipo', 'status']) + ' |')
        linhas.append('')
    (RAIZ / 'docs/FASE0_TESTES.md').write_text('\n'.join(linhas))
    print(json.dumps({'total': len(resultados), **totais}, ensure_ascii=False))


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('trx', type=Path)
    gerar(parser.parse_args().trx)

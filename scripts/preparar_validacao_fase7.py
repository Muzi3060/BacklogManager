#!/usr/bin/env python3
"""Cria somente dados fictícios em uma pasta temporária nova, sem ler a biblioteca pessoal."""
import base64
import hashlib
import json
import tempfile
import uuid
from datetime import datetime, timezone
from pathlib import Path


def preparar():
    raiz = Path(tempfile.mkdtemp(prefix='minhas-realizacoes-fase7-rider-'))
    dados = raiz / 'dados'
    dados.mkdir()
    agora = datetime.now(timezone.utc).isoformat()
    tipos = ['Jogo', 'Anime', 'Serie', 'Filme', 'Manga', 'Livro']
    obras = []
    for tipo in tipos:
        registro = dict(IdInterno=str(uuid.uuid4()), Titulo=f'{tipo} de teste', Categoria='Fantasia',
                        Status='Jogando' if tipo == 'Jogo' else 'Na Fila', Nota=4, Review='Review fictícia', SteamAppId=None,
                        Multimidia=dict(Versao=1, Tipo=tipo, Origem='IGDB' if tipo == 'Jogo' else 'Simulado',
                                       IdentificadorExterno='42' if tipo == 'Jogo' else f'simulado:{tipo}:1',
                                       Generos=['Fantasia'], Simulado=tipo != 'Jogo', DataAdicao=agora))
        if tipo == 'Jogo':
            registro['IgdbId'] = 42
        else:
            registro['Multimidia']['CapaUrl'] = f'avares://BacklogManager/Assets/CapasSimuladas/{tipo}.svg'
        obras.append(registro)
    obras.append(dict(Titulo='Jogo legado fictício', Categoria='Categoria antiga', Status='Na Fila', Nota=3,
                      Review='Sem identidade nem data de adição', SteamAppId=12345))
    colecoes = dict(Versao=1, Colecoes=[
        dict(Id=str(uuid.uuid4()), Nome='Favoritas de teste', Descricao='Uma obra em duas coleções',
             Obras=[obras[0]['IdInterno'], obras[5]['IdInterno']]),
        dict(Id=str(uuid.uuid4()), Nome='Para revisitar', Descricao='Vínculo compartilhado', Obras=[obras[0]['IdInterno']])])
    arquivos = {'Biblioteca': ('backlog.json', obras), 'Colecoes': ('backlog.colecoes.json', colecoes),
                'Visualizacao': ('biblioteca.preferencias.json', dict(Versao=1, Visualizacao=1)),
                'Aparencia': ('aparencia.preferencias.json', dict(Versao=1, Tema='Moderno', Modo='Claro', Personalizacoes={}))}
    componentes = {}
    for nome, (arquivo, conteudo) in arquivos.items():
        raw = json.dumps(conteudo, ensure_ascii=False, indent=2).encode()
        (dados / arquivo).write_bytes(raw)
        componentes[nome] = dict(Conteudo=base64.b64encode(raw).decode(), Sha256=hashlib.sha256(raw).hexdigest().upper())
    def salvar(nome, cs):
        (raiz / nome).write_text(json.dumps(dict(Formato='MinhasRealizacoes.BackupLocal', Versao=1, CriadoEm=agora,
                                                 Componentes=cs), indent=2), encoding='utf-8')
    salvar('backup-simulado.json', componentes)
    vazios = {}
    for nome, objeto in [('Biblioteca', []), ('Colecoes', dict(Versao=1, Colecoes=[]))]:
        raw = json.dumps(objeto).encode()
        vazios[nome] = dict(Conteudo=base64.b64encode(raw).decode(), Sha256=hashlib.sha256(raw).hexdigest().upper())
    salvar('backup-vazio.json', vazios)
    (raiz / 'backup-invalido.json').write_text('{"Formato":"incompleto"}', encoding='utf-8')
    print(f'Working directory do Rider: {dados}')
    print(f'Arquivos de backup fictícios: {raiz}')
    print('Nenhum dado pessoal foi lido. A pasta criada não é removida automaticamente.')
    return raiz


if __name__ == '__main__':
    preparar()

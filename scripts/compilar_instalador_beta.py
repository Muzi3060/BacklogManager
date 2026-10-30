#!/usr/bin/env python3
"""Compila o instalador local. Wine pode gerar o artefato, mas não qualificá-lo para Windows."""
import argparse,hashlib,json,os,pathlib,re,subprocess,sys
from publicar_beta import ROOT,auditar,hash_arquivo
INNO_SHA256='a549929319f9120716677c685535ca4671e4b937b5fdc87e2585f32d8eb30e8c'

def validar_publicacao(pasta):
    manifesto=json.loads((pasta/'manifesto-beta.json').read_text(encoding='utf-8'))
    if manifesto.get('rid')!='win-x64':raise ValueError('O instalador exige publicação Windows x64.')
    versao=manifesto.get('versao','')
    if not re.fullmatch(r'[0-9]+\.[0-9]+\.[0-9]+-beta\.[1-9][0-9]*',versao):raise ValueError('Versão beta inválida.')
    inventario=auditar(pasta)
    registrados=manifesto.get('arquivos')
    if not isinstance(registrados,dict) or {p:h for p,h in inventario.items() if p!='manifesto-beta.json'}!=registrados:
        raise ValueError('Publicação mudou após o inventário. Gere e audite novamente.')
    for nome in ('BacklogManager.exe','coreclr.dll'):
        if nome not in inventario:raise ValueError('Publicação Windows self-contained incompleta.')
    return versao

def compilar(pasta,iscc,prefixo=None):
    pasta=pasta.resolve()
    if not pasta.is_relative_to(ROOT/'artifacts'):raise ValueError('Use somente a publicação local em artifacts/.')
    versao=validar_publicacao(pasta);saida=pasta.parent
    exe=saida/f'MinhasRealizacoes-{versao}-win-x64-setup.exe'
    if exe.exists():raise ValueError('O instalador já existe; não será sobrescrito.')
    env=dict(os.environ)
    if prefixo:
        env['WINEPREFIX']=str(prefixo.resolve());env['WINEDEBUG']='-all'
        def caminho(p):return 'Z:'+str(p.resolve()).replace('/','\\')
        comando=['wine',str(iscc),'/Qp','/DPublishDir='+caminho(pasta),'/DOutputDir='+caminho(saida),'/DBetaVersion='+versao,caminho(ROOT/'packaging/windows/MinhasRealizacoes.iss')]
    else:
        comando=[str(iscc),'/Qp','/DPublishDir='+str(pasta),'/DOutputDir='+str(saida),'/DBetaVersion='+versao,str(ROOT/'packaging/windows/MinhasRealizacoes.iss')]
    with (saida/'compilacao-instalador.log').open('w') as log:
        os.chmod(saida/'compilacao-instalador.log',0o600)
        r=subprocess.run(comando,env=env,stdout=log,stderr=subprocess.STDOUT)
    if r.returncode or not exe.exists() or exe.open('rb').read(2)!=b'MZ':raise ValueError('Compilação do instalador não concluída.')
    with (saida/'SHA256SUMS.txt').open('a') as f:f.write(hash_arquivo(exe)+'  '+exe.name+'\n')
    print('Instalador local gerado. Testes nativos no Windows 11 continuam pendentes.')
    return exe

if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__);p.add_argument('--publicacao',type=pathlib.Path,required=True);p.add_argument('--iscc',type=pathlib.Path,required=True);p.add_argument('--wine-prefix',type=pathlib.Path)
    a=p.parse_args()
    try:compilar(a.publicacao,a.iscc,a.wine_prefix)
    except (OSError,ValueError,subprocess.SubprocessError):print('Instalador não concluído. Consulte o log local.',file=sys.stderr);sys.exit(1)

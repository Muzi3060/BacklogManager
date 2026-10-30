#!/usr/bin/env python3
"""Publicação local: nenhum upload, release ou operação Git de escrita."""
import argparse, hashlib, json, os, pathlib, re, shutil, subprocess, sys, tarfile, tempfile, zipfile
ROOT = pathlib.Path(__file__).resolve().parents[1]

def hash_arquivo(p):
    h = hashlib.sha256()
    with p.open('rb') as f:
        for b in iter(lambda: f.read(1024*1024), b''):
            h.update(b)
    return h.hexdigest()

def auditar(pasta, credenciais=()):
    arquivos = sorted(p for p in pasta.rglob('*') if p.is_file())
    if not arquivos:
        raise ValueError('Publicação vazia.')
    inventario = {}
    for p in arquivos:
        relativo = p.relative_to(pasta).as_posix()
        nome = p.name
        if p.is_symlink() or any(x.startswith('.') for x in p.relative_to(pasta).parts):
            raise ValueError('Arquivo oculto/link inesperado no pacote.')
        if len(p.relative_to(pasta).parts) != 1:
            raise ValueError('Subdiretório inesperado: revise o inventário antes de ampliar a permissão.')
        permitido = (nome in {'BacklogManager','BacklogManager.exe','createdump','createdump.exe','BacklogManager.deps.json','BacklogManager.runtimeconfig.json','manifesto-beta.json','LEIA-ME.txt','THIRD-PARTY-NOTICES.txt','LICENSE.txt'} or
                      nome.endswith(('.dll','.so','.dylib')) or re.fullmatch(r'lib[A-Za-z0-9_.+-]+\.so\.[0-9.]+',nome))
        if not permitido or nome.lower().startswith(('backlog.json','test','qodana','fase_beta','backup','aparencia.','biblioteca.')):
            raise ValueError('Conteúdo não autorizado no pacote: '+relativo)
        if any(segredo.encode() in p.read_bytes() for segredo in credenciais if segredo) and nome != 'BacklogManager.dll':
            raise ValueError('Credencial fora do assembly aprovado.')
        inventario[relativo] = hash_arquivo(p)
    for obrigatorio in ('BacklogManager.dll','BacklogManager.deps.json','BacklogManager.runtimeconfig.json'):
        if obrigatorio not in inventario:
            raise ValueError('Publicação incompleta: '+obrigatorio)
    return inventario

def compactar(pasta, destino, rid):
    # Nunca sobrescrever artefatos existentes por acidente.
    if destino.exists():
        raise ValueError('O pacote de destino já existe; escolha uma nova pasta de saída.')
    arquivos=sorted(p for p in pasta.rglob('*') if p.is_file())
    if rid == 'win-x64':
        with zipfile.ZipFile(destino,'x',compression=zipfile.ZIP_DEFLATED,compresslevel=6) as z:
            for p in arquivos:z.write(p,p.relative_to(pasta).as_posix())
        with zipfile.ZipFile(destino) as z:
            if z.testzip() is not None:raise ValueError('ZIP não verificado.')
            for p in arquivos:
                if hashlib.sha256(z.read(p.relative_to(pasta).as_posix())).hexdigest()!=hash_arquivo(p):raise ValueError('ZIP diverge da publicação.')
    else:
        with tarfile.open(destino,'x:gz') as t:
            for p in arquivos:t.add(p,arcname=p.relative_to(pasta).as_posix(),recursive=False)
        with tarfile.open(destino,'r:gz') as t:
            for p in arquivos:
                if hashlib.sha256(t.extractfile(p.relative_to(pasta).as_posix()).read()).hexdigest()!=hash_arquivo(p):raise ValueError('TAR diverge da publicação.')
    return hash_arquivo(destino)

def publicar(rid, saida, repositorio=None):
    import xml.etree.ElementTree as ET
    versao=ET.parse(ROOT/'BacklogManager.csproj').getroot().find('./PropertyGroup/Version').text
    if saida.exists():raise ValueError('Saída já existe. Não será removida ou sobrescrita.')
    if repositorio and not re.fullmatch(r'[A-Za-z0-9][A-Za-z0-9-]{0,38}/[A-Za-z0-9_.-]{1,100}',repositorio):raise ValueError('Repositório inválido.')
    from preparar_credenciais_beta import preparar
    # Validar antecipadamente o par sem imprimir valores; o MSBuild também gera seu recurso privado.
    credenciais=[os.environ.get(n,'').strip() for n in ('MR_BETA_IGDB_CLIENT_ID','MR_BETA_IGDB_ACCESS_TOKEN')]
    saida.mkdir(parents=True,mode=0o700)
    preparar(saida/'igdb-preparacao.tmp',os.environ)
    (saida/'igdb-preparacao.tmp').unlink()
    pasta=saida/'publish'
    comando=['dotnet','publish',str(ROOT/'BacklogManager.csproj'),'-m:1','-p:PublishProfile='+('BetaWindows' if rid=='win-x64' else 'BetaLinux'),'-o',str(pasta)]
    if repositorio:comando.append('-p:RepositorioReleases='+repositorio)
    log=saida/'publicacao.log'
    with log.open('w') as f:
        os.chmod(log,0o600)
        resultado=subprocess.run(comando,cwd=ROOT,stdout=f,stderr=subprocess.STDOUT)
    if resultado.returncode:raise ValueError('dotnet publish falhou. Examine o log local sem compartilhar credenciais.')
    texto=log.read_bytes()
    if any(c.encode() in texto for c in credenciais):raise ValueError('O log contém credenciais; preserve-o localmente e não publique.')
    # Símbolos nativos de pacotes podem ser copiados mesmo com DebugSymbols=false.
    # Remover somente estes arquivos gerados dentro da saída recém-criada.
    for simbolo in pasta.glob('*.pdb'):simbolo.unlink()
    runtime='coreclr.dll' if rid=='win-x64' else 'libcoreclr.so'
    if not (pasta/runtime).exists():raise ValueError('Runtime self-contained ausente.')
    if rid=='linux-x64':(pasta/'BacklogManager').chmod(0o755)
    (pasta/'LEIA-ME.txt').write_text('Minhas Realizações '+versao+'\nBeta em validação. Não contém dados pessoais.\nDados: Windows %LOCALAPPDATA%/MinhasRealizacoes/Dados; Linux $XDG_DATA_HOME/minhas-realizacoes/dados (padrão ~/.local/share/minhas-realizacoes/dados).\nExecute BacklogManager.exe (Windows) ou ./BacklogManager (Linux). Linux exige bibliotecas gráficas nativas.\nMigração: use --migrate-from /pasta/absoluta com destino novo; revise e confirme a prévia.\nTestes: --data-dir /pasta/absoluta/isolada.\nAtualização manual: feche o aplicativo, faça backup e substitua somente os binários. O instalador preserva os dados na desinstalação.\nAs credenciais IGDB da beta estão incorporadas; token expirado exige nova versão.\nSistemas-alvo: Windows 11 x64 e Ubuntu 24.04 LTS x64. Consulte o relatório de qualificação antes de distribuir.\n',encoding='utf-8')
    shutil.copy2(ROOT/'packaging/THIRD-PARTY-NOTICES.txt',pasta/'THIRD-PARTY-NOTICES.txt')
    inventario=auditar(pasta,credenciais)
    manifesto={'versao':versao,'rid':rid,'sdk':subprocess.check_output(['dotnet','--version'],cwd=ROOT,text=True).strip(),
        'commit':subprocess.check_output(['git','rev-parse','HEAD'],cwd=ROOT,text=True).strip(),
        'fontesComAlteracoes':bool(subprocess.check_output(['git','status','--porcelain','--untracked-files=no'],cwd=ROOT)),
        'qualificacao':'Pendente de validação nativa no sistema-alvo','arquivos':inventario}
    (pasta/'manifesto-beta.json').write_text(json.dumps(manifesto,indent=2,ensure_ascii=False),encoding='utf-8')
    auditar(pasta,credenciais)
    nome='MinhasRealizacoes-'+versao+'-'+rid+('.zip' if rid=='win-x64' else '.tar.gz')
    pacote=saida/nome;digest=compactar(pasta,pacote,rid)
    (saida/'SHA256SUMS.txt').write_text(digest+'  '+nome+'\n')
    print('Pacote local gerado e inventário verificado:',nome)
    return pacote

if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--rid',required=True,choices=['win-x64','linux-x64'])
    parser.add_argument('--saida',required=True,type=pathlib.Path)
    parser.add_argument('--repositorio',help='owner/repo oficial aprovado; não inclui credenciais')
    args=parser.parse_args()
    try:
        destino=args.saida.resolve()
        # Saídas de publicação ficam exclusivamente no diretório ignorado de artefatos.
        if not destino.is_relative_to(ROOT/'artifacts'):raise ValueError('Escolha uma saída dentro de artifacts/.')
        publicar(args.rid,destino,args.repositorio)
    except (ValueError,OSError,subprocess.SubprocessError):
        print('Publicação não concluída. Confira as condições, as credenciais locais e o log privado.',file=sys.stderr);sys.exit(1)

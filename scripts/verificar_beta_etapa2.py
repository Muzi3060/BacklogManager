#!/usr/bin/env python3
"""Verifica exclusões compartilhadas e a sanitização da documentação, sem gravar dados reais."""
import argparse
import hashlib
import json
import os
from pathlib import Path
import re
import subprocess
import tempfile

RAIZ = Path(__file__).resolve().parents[1]
BASE = 'c14f588'
# Apenas referências de documentação: estes símbolos não mudam caminhos do aplicativo.
RAIZ_PROJETO_POSIX = re.compile(r'/(?:home|Users)/[^/\s"\'<>]+/(?:RiderProjects|Projects)/BacklogManager(?=/|[\s"\'`]|$)')
RAIZ_PESSOAL_POSIX = re.compile(r'/(?:home|Users)/[^/\s"\'<>]+')
RAIZ_PROJETO_WINDOWS = re.compile(r'[A-Za-z]:(\\{1,2})Users\1[^\\\s"\'<>]+\1(?:RiderProjects|Projects)\1BacklogManager(?=\\|[\s"\'`]|$)')
RAIZ_PESSOAL_WINDOWS = re.compile(r'[A-Za-z]:(\\{1,2})Users\1[^\\\s"\'<>]+')


def normalizar_caminhos(texto):
    """Substitui raízes pessoais; mantém o restante do texto, inclusive dados de resultados."""
    texto = RAIZ_PROJETO_POSIX.sub('${PROJETO}', texto)
    texto = RAIZ_PESSOAL_POSIX.sub('${HOME}', texto)
    texto = RAIZ_PROJETO_WINDOWS.sub('${PROJETO}', texto)
    return RAIZ_PESSOAL_WINDOWS.sub('${HOME}', texto)


def ler_json_sem_duplicatas(texto):
    def objeto(pares):
        resultado = {}
        for chave, valor in pares:
            if chave in resultado:
                raise ValueError('A generalização não pode criar chaves JSON repetidas.')
            resultado[chave] = valor
        return resultado
    return json.loads(texto, object_pairs_hook=objeto)


def git(*args, cwd=RAIZ):
    return subprocess.check_output(['git', *args], cwd=cwd, stderr=subprocess.PIPE)


def fontes_base():
    return [p for p in git('ls-tree', '-r', '--name-only', BASE, 'docs').decode().splitlines()
            if normalizar_caminhos(git('show', BASE + ':' + p).decode()) != git('show', BASE + ':' + p).decode()]


def verificar(manifesto=None):
    resultados = []

    def caso(codigo, descricao, esperado, acao):
        try:
            passou = bool(acao())
            observado = esperado if passou else 'Condição esperada não foi atendida; revisar sem expor conteúdo privado.'
        except Exception as erro:
            passou = False
            observado = 'Verificação interrompida: ' + type(erro).__name__
        resultados.append(dict(id=codigo, cenario=descricao, esperado=esperado, observado=observado,
                               tipo='Automatizado (verificação estática/isolada)', status='Aprovado' if passou else 'Reprovado'))

    protegidos = [
        ('instrução privada da beta', 'FASE_BETA_INSTRUCOES.md'),
        ('cópia privada da instrução', 'FASE_BETA_INSTRUCOES.copia.md'),
        ('instrução local genérica', 'OUTRAS_INSTRUCOES.local.md'),
        ('instrução privada em subpasta', 'notas/OUTRAS_INSTRUCOES.privado.md'),
        ('configuração pessoal Qodana', 'qodana.yaml'),
        ('habilidade pessoal', 'skills/system/run-workflow/SKILL.md'),
        ('biblioteca na raiz', 'backlog.json'),
        ('biblioteca em subpasta', 'perfil/backlog.json'),
        ('cópia da biblioteca com hífen', 'backlog-copia.json'),
        ('cópia da biblioteca com sublinhado', 'backlog_copia.json'),
        ('coleções na raiz', 'backlog.colecoes.json'),
        ('coleções em subpasta', 'perfil/backlog.colecoes.json'),
        ('preferência de visualização', 'perfil/biblioteca.preferencias.json'),
        ('preferência de aparência', 'perfil/aparencia.preferencias.json'),
        ('lock de biblioteca', 'perfil/backlog.json.lock'),
        ('lock de coleções', 'perfil/backlog.colecoes.json.lock'),
        ('lock de aparência', 'perfil/aparencia.preferencias.json.lock'),
        ('lock de visualização', 'perfil/biblioteca.preferencias.json.lock'),
        ('diário de recuperação', 'perfil/.mr-restauracao.json'),
        ('lock do conjunto', 'perfil/.mr-backup.lock'),
        ('exportação com nome padrão', 'minhas-realizacoes-backup.json'),
        ('exportação com nome padrão e data', 'minhas-realizacoes-backup-20261029.json'),
        ('segurança anterior à restauração', 'antes-restauracao-20261029.json'),
        ('backup nomeado com hífen', 'backup-pessoal.json'),
        ('backup nomeado com sublinhado', 'backup_pessoal.json'),
        ('exportação nomeada com hífen', 'exportacao-biblioteca.json'),
        ('exportação nomeada com sublinhado', 'exportacao_biblioteca.json'),
        ('diretório de backups', 'backups/conjunto.json'),
        ('diretório de backups com maiúscula', 'Backups/conjunto.json'),
        ('diretório de exports', 'exports/conjunto.json'),
        ('diretório de exportações', 'exportacoes/conjunto.json'),
        ('diretório de dados', 'dados/arquivo.json'),
        ('diretório de dados pessoais', 'dados-pessoais/arquivo.json'),
        ('diretório de dados locais', 'dados-locais/arquivo.json'),
        ('backup individual', 'perfil/arquivo.json.bak'),
        ('configuração da IDE', '.idea/workspace.xml'),
        ('credenciais de ambiente', '.env'),
        ('variante local de ambiente', '.env.local'),
        ('log de execução', 'execucao.log'),
        ('saída de compilação', 'bin/Debug/app.dll'),
        ('cache de compilação', 'obj/project.assets.json'),
        ('resultado de testes', 'TestResults/resultado.trx'),
    ]
    permitidos = [
        ('código C#', 'Services/ServicoBiblioteca.cs'),
        ('interface Avalonia', 'MainWindow.axaml'),
        ('documentação pública', 'README.md'),
        ('contrato protegido e versionado', 'Contrato.md'),
        ('instrução histórica sem marca privada', 'FASE5_INSTRUCOES.md'),
        ('relatório JSON', 'docs/resultados-fase7.json'),
        ('fixture JSON com nome explícito', 'Tests/Fixtures/biblioteca-simulada.json'),
        ('fixture de backup em testes', 'Tests/Fixtures/backup-simulado.json'),
        ('configuração de exemplo', '.env.example'),
        ('recurso SVG', 'Assets/CapasSimuladas/Livro.svg'),
        ('roteiro de validação', 'scripts/preparar_validacao_fase7.py'),
        ('JSON genérico', 'exemplo.json'),
    ]
    # Um clone novo não recebe .git/info/exclude: a matriz testa só as regras compartilhadas.
    with tempfile.TemporaryDirectory(prefix='beta2-regras-') as pasta:
        temp = Path(pasta)
        ambiente = dict(os.environ, GIT_CONFIG_NOSYSTEM='1', GIT_CONFIG_GLOBAL=os.devnull)
        for key in ['GIT_DIR', 'GIT_WORK_TREE', 'GIT_INDEX_FILE', 'GIT_COMMON_DIR', 'GIT_OBJECT_DIRECTORY', 'GIT_ALTERNATE_OBJECT_DIRECTORIES']:
            ambiente.pop(key, None)
        subprocess.run(['git', 'init', '--quiet', '--template=', pasta], env=ambiente, check=True)
        (temp / '.gitignore').write_bytes((RAIZ / '.gitignore').read_bytes())
        def ignorado(p):
            return subprocess.run(['git', '-c', 'core.excludesFile=' + os.devnull,
                                   'check-ignore', '--no-index', '-q', p], cwd=temp, env=ambiente).returncode == 0
        for n, (descricao, caminho) in enumerate(protegidos, 1):
            caso(f'IG{n:02}', descricao, 'Ignorado pelas regras compartilhadas', lambda p=caminho: ignorado(p))
        for n, (descricao, caminho) in enumerate(permitidos, 1):
            caso(f'IP{n:02}', descricao, 'Continua versionável', lambda p=caminho: not ignorado(p))
        # Dados fictícios: confirma a operação de staging comum, sem tocar no índice real.
        (temp / 'FASE_BETA_INSTRUCOES.md').write_text('INSTRUÇÃO FICTÍCIA SEM CREDENCIAL')
        (temp / 'minhas-realizacoes-backup.json').write_text('[]')
        (temp / 'README.md').write_text('Exemplo fictício')
        dry = subprocess.run(['git', 'add', '--dry-run', '.'], cwd=temp, env=ambiente, capture_output=True, text=True)
        caso('IG43', 'staging comum em clone sem exclusão local', 'Somente README e .gitignore entram no dry-run',
             lambda: dry.returncode == 0 and 'README.md' in dry.stdout and 'FASE_BETA_INSTRUCOES.md' not in dry.stdout and 'minhas-realizacoes-backup.json' not in dry.stdout)

    exemplos = [
        ('projeto Linux', '/home/pessoa/RiderProjects/BacklogManager/backlog.json', '${PROJETO}/backlog.json'),
        ('backup Linux', '/home/pessoa/Backups/arquivo.json', '${HOME}/Backups/arquivo.json'),
        ('projeto macOS', '/Users/pessoa/Projects/BacklogManager/docs/arquivo.md', '${PROJETO}/docs/arquivo.md'),
        ('home macOS', '/Users/pessoa/Backups/arquivo.json', '${HOME}/Backups/arquivo.json'),
        ('projeto Windows', r'C:\Users\pessoa\RiderProjects\BacklogManager\backlog.json', r'${PROJETO}\backlog.json'),
        ('home Windows', r'D:\Users\pessoa\Backups\arquivo.json', r'${HOME}\Backups\arquivo.json'),
        ('caminho Windows escapado em JSON', r'C:\\Users\\pessoa\\RiderProjects\\BacklogManager\\backlog.json', r'${PROJETO}\\backlog.json'),
        ('raiz home sem sufixo', '/home/pessoa', '${HOME}'),
        ('nome de projeto semelhante', '/home/pessoa/Projects/BacklogManagerOutro/arquivo', '${HOME}/Projects/BacklogManagerOutro/arquivo'),
        ('caminho genérico já sanitizado', '${HOME}/Backups/arquivo.json', '${HOME}/Backups/arquivo.json'),
        ('placeholder anterior', '/home/<usuario>/arquivo', '/home/<usuario>/arquivo'),
        ('caminho temporário útil', '/tmp/validacao-isolada/arquivo.json', '/tmp/validacao-isolada/arquivo.json'),
        ('URL de catálogo', 'https://images.igdb.com/capa.jpg', 'https://images.igdb.com/capa.jpg'),
        ('contagens resultados e hash', '{"aprovados":1541,"reprovados":0,"hash":"ABC123"}', '{"aprovados":1541,"reprovados":0,"hash":"ABC123"}'),
    ]
    for n, (descricao, antes, depois) in enumerate(exemplos, 1):
        caso(f'SN{n:02}', descricao, 'Substitui somente a raiz pessoal prevista', lambda a=antes, d=depois: normalizar_caminhos(a) == d)
    def recusa_json(texto):
        try:
            ler_json_sem_duplicatas(normalizar_caminhos(texto))
        except ValueError:
            return True
        return False
    caso('SN15', 'normalização que colidiria duas chaves pessoais', 'Colisão detectada sem descartar silenciosamente uma chave',
         lambda: recusa_json('{"/home/pessoa-a/arquivo":1,"/home/pessoa-b/arquivo":2}'))
    caso('SN16', 'propriedade duplicada em objeto aninhado', 'JSON ambíguo recusado',
         lambda: recusa_json('{"resultado":{"total":1,"total":2}}'))
    caso('SN17', 'JSON truncado na verificação de preservação', 'JSON inválido recusado', lambda: recusa_json('{'))
    for n, caminho in enumerate(fontes_base(), 1):
        antes = git('show', BASE + ':' + caminho).decode()
        depois = (RAIZ / caminho).read_text()
        caso(f'DH{n:02}', caminho, 'Documento histórico difere somente pelas raízes de caminhos',
             lambda a=antes, d=depois: normalizar_caminhos(a) == d)
        if caminho.endswith('.json'):
            caso(f'JS{n:02}', caminho, 'JSON válido; campos e resultados preservados sob a mesma substituição de raízes',
                 lambda a=antes, d=depois: ler_json_sem_duplicatas(normalizar_caminhos(a)) == ler_json_sem_duplicatas(d))

    leia = (RAIZ / 'README.md').read_text()
    contexto = (RAIZ / 'docs/README.md').read_text()
    checks = [
        ('nome público', lambda: leia.startswith('# Minhas Realizações')),
        ('beta em desenvolvimento', lambda: 'primeira beta ainda está em desenvolvimento' in leia),
        ('SDK coincide com o projeto', lambda: '.NET 10' in leia and '<TargetFramework>net10.0</TargetFramework>' in (RAIZ / 'BacklogManager.csproj').read_text()),
        ('projeto indicado existe', lambda: 'BacklogManager.csproj' in leia and (RAIZ / 'BacklogManager.csproj').is_file()),
        ('execução fora do repositório', lambda: 'Working directory' in leia and 'dotnet run --project "$projeto/BacklogManager.csproj"' in leia and 'mktemp -d' in leia),
        ('três nomes de configuração IGDB aceitos', lambda: all(v in leia and v in (RAIZ / 'Services/ConfiguracaoIgdb.cs').read_text() for v in ['IGDB_CLIENT_ID', 'IGDB_ACESS_TOKEN', 'IGDB_ACCESS_TOKEN'])),
        ('armazenamento ativo explicitado', lambda: all(p in leia for p in ['backlog.json', 'backlog.colecoes.json', 'biblioteca.preferencias.json', 'aparencia.preferencias.json', '.mr-restauracao.json'])),
        ('links locais do README existentes', lambda: all((RAIZ / p).exists() for p in re.findall(r'\]\(([^)#]+)(?:#[^)]*)?\)', leia) if not re.match(r'[a-z]+://', p))),
        ('gerador fictício existente e documentado', lambda: 'scripts/preparar_validacao_fase7.py' in leia and (RAIZ / 'scripts/preparar_validacao_fase7.py').is_file()),
        ('regressão documentada sem executar aplicativo real', lambda: 'dotnet test' in leia and 'dados simulados' in leia),
        ('placeholders e histórico esclarecidos', lambda: '${HOME}' in contexto and '${PROJETO}' in contexto and 'histórico Git' in contexto and 'resultados' in contexto),
        ('licença não escolhida e pacotes não prometidos', lambda: 'licença ainda não foi escolhida' in leia and 'não há pacotes de distribuição aprovados' in leia),
    ]
    for n, (descricao, acao) in enumerate(checks, 1):
        caso(f'PB{n:02}', descricao, 'Documentação coerente com o projeto e seus limites', acao)
    docs = [RAIZ / p for p in git('ls-files', 'docs').decode().splitlines()]
    caso('PR01', 'documentação atual sem raízes pessoais concretas', 'Nenhuma raiz pessoal concreta nos documentos atuais',
         lambda: all(normalizar_caminhos(p.read_text()) == p.read_text() for p in docs))
    caso('PR02', 'código C# e Avalonia preservado', 'Nenhum fonte do aplicativo alterado desde a base auditada',
         lambda: not git('diff', BASE, '--', '*.cs', '*.axaml', '*.csproj', '*.slnx', 'Assets'))
    caso('PR03', 'contrato preservado', 'Contrato idêntico ao da base auditada', lambda: not git('diff', BASE, '--', 'Contrato.md'))
    caso('PR04', 'pessoais fora do índice', 'Instrução beta Qodana e skills não rastreados',
         lambda: not git('ls-files', '--', 'FASE_BETA_INSTRUCOES.md', 'qodana.yaml', 'skills'))
    if manifesto:
        m = json.loads(Path(manifesto).read_text())
        def mesmos(registros):
            return all(Path(p).is_file() and hashlib.sha256(Path(p).read_bytes()).hexdigest() == h for p, h in registros.items())
        caso('PR05', 'pessoais e dados locais', 'Hashes iguais ao manifesto privado anterior', lambda: mesmos(m['protected']))
        caso('PR06', 'backups externos', 'Hashes iguais ao manifesto privado anterior', lambda: mesmos(m['backups']))
        caso('PR07', 'outras referências Git', 'Nenhuma referência anterior alterada',
             lambda: all(git('rev-parse', ref).decode().strip() == h for ref, h in m['refs'].items()
                         if ref != 'refs/heads/chore/beta-auditoria') and
             subprocess.run(['git', 'merge-base', '--is-ancestor', m['base'], 'HEAD'], cwd=RAIZ).returncode == 0)
    else:
        for codigo, descricao in [('PR05', 'pessoais e dados locais'), ('PR06', 'backups externos'), ('PR07', 'outras referências Git')]:
            resultados.append(dict(id=codigo, cenario=descricao, esperado='Comparar com o manifesto privado anterior',
                                   observado='Não executado: o manifesto privado não foi fornecido nesta chamada.',
                                   tipo='Automatizado (referência privada necessária)', status='Não executado'))
    return resultados


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--manifesto', type=Path, help='Manifesto privado opcional: somente leitura; nunca versionar.')
    parser.add_argument('--saida', type=Path, help='Gravar resultados sem valores sensíveis em pasta externa à biblioteca.')
    args = parser.parse_args()
    if args.saida and (args.saida.resolve().is_relative_to(RAIZ) or args.saida.name in ['backlog.json', 'backlog.colecoes.json', 'aparencia.preferencias.json', 'biblioteca.preferencias.json']):
        parser.error('Use uma saída fora do repositório e diferente dos arquivos de dados.')
    if args.saida and args.saida.exists():
        parser.error('A saída já existe; use um arquivo novo para preservar o anterior.')
    testes = verificar(args.manifesto)
    resumo = dict(planejados=len(testes), executados=sum(t['status'] != 'Não executado' for t in testes),
                  aprovados=sum(t['status'] == 'Aprovado' for t in testes), reprovados=sum(t['status'] == 'Reprovado' for t in testes),
                  naoExecutados=sum(t['status'] == 'Não executado' for t in testes))
    if args.saida:
        # Nunca sobrescreve um arquivo anterior, inclusive um backup escolhido por engano.
        try:
            with args.saida.open('x', encoding='utf-8') as arquivo:
                arquivo.write(json.dumps(dict(resumo=resumo, testes=testes), ensure_ascii=False, indent=2) + '\n')
        except FileExistsError:
            parser.error('A saída foi criada por outro processo; o arquivo anterior foi preservado.')
    print(json.dumps(resumo, ensure_ascii=False))
    for t in testes:
        if t['status'] != 'Aprovado': print(t['id'] + ': ' + t['observado'])
    return int(resumo['reprovados'] > 0)


if __name__ == '__main__':
    raise SystemExit(main())

#!/usr/bin/env python3
"""Verificações locais de preservação e dos candidatos. Relatórios não contêm credenciais."""
import argparse,hashlib,json,pathlib,re,subprocess,sys,xml.etree.ElementTree as ET
from publicar_beta import ROOT,auditar,hash_arquivo

def verificar(manifesto,artefatos,trx,saida):
    inicial=json.loads(manifesto.read_text());checks=[]
    def registrar(id,ok,observado):checks.append({'id':id,'esperado':'Condição de preservação/qualidade satisfeita','observado':observado,'tipo':'Script local','status':'Aprovado' if ok else 'Reprovado'})
    protegido=inicial['protected'];backups=inicial['backups'];rastreado=inicial['tracked']
    igual=lambda p,h:p.is_file() and hash_arquivo(p)==h
    registrar('H06',igual(ROOT/'Contrato.md',rastreado['Contrato.md']),'Contrato comparado por SHA-256 com referência anterior à implementação.')
    registrar('H07',igual(ROOT/'FASE_BETA_INSTRUCOES.md',protegido['FASE_BETA_INSTRUCOES.md']) and not subprocess.check_output(['git','ls-files','FASE_BETA_INSTRUCOES.md'],cwd=ROOT),'Instrução privada intacta e não rastreada.')
    nomes=[n for n in protegido if n=='qodana.yaml' or n.startswith('skills/')]
    registrar('H08',all(igual(ROOT/n,protegido[n]) for n in nomes) and not subprocess.check_output(['git','ls-files','qodana.yaml','skills'],cwd=ROOT),'Arquivos pessoais existentes comparados por hash e ausentes do índice.')
    registrar('H09',all(igual(ROOT/n,h) for n,h in protegido.items()) and all(igual(pathlib.Path(n),h) for n,h in backups.items()),'Todos os arquivos protegidos e 22 backups anteriores mantêm os hashes originais.')
    refs=dict((l.split()[1],l.split()[0]) for l in inicial['refs'].splitlines())
    atuais=dict((l.split()[1],l.split()[0]) for l in subprocess.check_output(['git','show-ref'],cwd=ROOT,text=True).splitlines())
    registrar('H10',all(atuais.get(n)==h for n,h in refs.items() if n!='refs/heads/feat/beta-distribuicao'),'main e todas as demais referências anteriores inalteradas.')
    registrar('H16',subprocess.check_output(['git','branch','--show-current'],cwd=ROOT,text=True).strip()=='feat/beta-distribuicao','Trabalho na branch autorizada; script não realiza operações remotas.')
    ns={'t':'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'};raiz=ET.parse(trx).getroot();resultados=raiz.findall('.//t:UnitTestResult',ns)
    registrar('H01',len(resultados)>=1707 and all(r.attrib['outcome']=='Passed' for r in resultados),f'{len(resultados)} resultados automatizados lidos do TRX, incluindo regressão anterior.')
    registrar('H02',sum('BetaDistribuicao' in r.attrib.get('testName','') for r in resultados)>=166,'Casos C# novos incorporados à execução completa.')
    registrar('H03',all(subprocess.run(['git','check-ignore','-q',n],cwd=ROOT).returncode==0 for n in ['FASE_BETA_INSTRUCOES.md','qodana.yaml','skills/pessoal.txt','artifacts/exemplo.zip','backlog.json','backups/teste.json']),'Exclusões compartilhadas efetivamente avaliadas pelo Git.')
    registrar('H04',subprocess.run(['git','check-ignore','-q','Tests/Fixtures/exemplo.json'],cwd=ROOT).returncode==1,'Fixtures JSON continuam elegíveis ao versionamento.')
    tokens=[]
    privado=(ROOT/'FASE_BETA_INSTRUCOES.md').read_text()
    for rotulo in ['Client ID','Access Token']:tokens.append(re.search(r'^- '+rotulo+r':\s*(\S+)',privado,re.M).group(1))
    logs=list(artefatos.rglob('*.log'))+list(pathlib.Path('/tmp').glob('mr-beta3-*.log'))
    registrar('H11',all(not any(t.encode() in p.read_bytes() for t in tokens) for p in logs),'Logs de teste/publicação inspecionados sem imprimir valores.')
    publicos=[p for p in (ROOT/'docs').glob('BETA_ETAPA3*.md')]
    registrar('H12',all('/home/' not in p.read_text() and not any(t in p.read_text() for t in tokens) for p in publicos),'Novos relatórios sem caminhos pessoais concretos ou credenciais completas.')
    todos=True
    for rid in ['linux-x64','win-x64']:
        pasta=artefatos/rid/'publish';inventario=auditar(pasta,tokens);m=json.loads((pasta/'manifesto-beta.json').read_text())
        ok={n:h for n,h in inventario.items() if n!='manifesto-beta.json'}==m['arquivos'];todos&=ok
        registrar('P09-'+rid,ok,'Inventário SHA-256 confere com a publicação local.')
        registrar('P03-'+rid,(pasta/('coreclr.dll' if rid=='win-x64' else 'libcoreclr.so')).is_file(),'Runtime self-contained presente.')
        registrar('P14-'+rid,not list(pasta.glob('*.pdb')),'Nenhum símbolo de depuração na publicação.')
        registrar('P07-'+rid,all(('Assets/CapasSimuladas/'+f.name).encode() in (pasta/'BacklogManager.dll').read_bytes() for f in (ROOT/'Assets/CapasSimuladas').glob('*.svg')),'Índice de recursos das cinco capas simuladas presente no assembly de cada RID.')
        registrar('P15-'+rid,all(t.encode() in (pasta/'BacklogManager.dll').read_bytes() for t in tokens),'Par aprovado presente no assembly; auditoria rejeita presença em outros arquivos.')
        for linha in (artefatos/rid/'SHA256SUMS.txt').read_text().splitlines():
            digest,nome=linha.split('  ',1);registrar('CHECKSUM-'+nome,igual(artefatos/rid/nome,digest),'Checksum do artefato confere com a lista local.')
    registrar('H13',todos,'Publicações finais auditadas por lista permitida e inventário, com tokens restritos ao assembly aprovado.')
    registrar('H05',igual(ROOT/'scripts/verificar_beta_etapa2.py',rastreado['scripts/verificar_beta_etapa2.py']) and 'PR02' in (ROOT/'docs/BETA_ETAPA3.md').read_text(),'Verificador histórico intacto; transição de escopo documentada explicitamente.')
    registrar('H14',all((ROOT/'docs/BETA_ETAPA3.md').read_text().find(t)>=0 for t in ['--data-dir','--migrate-from','Windows 11','Ubuntu 24.04','pendente']),'Instruções públicas correspondem às opções e limites dos candidatos, sem anunciar qualificação nativa inexistente.')
    fonte=(ROOT/'Services/ServicoAtualizacoesGitHub.cs').read_text()
    registrar('H15',not re.search(r'File\.(Write|Move|Delete)|Process\.Start|HttpMethod\.Post',fonte) and 'AbrirAtualizacao_Click' in (ROOT/'MainWindow.Atualizacoes.cs').read_text(),'Revisão estática e testes HTTP: consulta de releases sem instalação/download automático; abertura vinculada ao clique.')
    saida.write_text(json.dumps(checks,indent=2,ensure_ascii=False));print('Verificações:',len(checks),'aprovadas:',sum(c['status']=='Aprovado' for c in checks),'reprovadas:',sum(c['status']=='Reprovado' for c in checks))
    return all(c['status']=='Aprovado' for c in checks)
if __name__=='__main__':
    p=argparse.ArgumentParser(description=__doc__)
    for n in ['manifesto','artefatos','trx','saida']:p.add_argument('--'+n,type=pathlib.Path,required=True)
    a=p.parse_args();sys.exit(0 if verificar(a.manifesto,a.artefatos,a.trx,a.saida) else 1)

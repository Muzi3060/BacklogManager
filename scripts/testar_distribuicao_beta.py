#!/usr/bin/env python3
"""Testes isolados dos scripts. Todos os valores de credenciais são fictícios."""
import importlib.util,json,os,pathlib,tarfile,tempfile,unittest,zipfile
from publicar_beta import auditar,compactar
from preparar_credenciais_beta import preparar

class Preparacao(unittest.TestCase):
    def setUp(self):
        self.tmp=tempfile.TemporaryDirectory(prefix='MR.Beta.Script.');self.p=pathlib.Path(self.tmp.name)
    def tearDown(self):self.tmp.cleanup()
    def test_I01_par_completo(self):
        preparar(self.p/'recurso.json',{'MR_BETA_IGDB_CLIENT_ID':'simulado','MR_BETA_IGDB_ACCESS_TOKEN':'token-ficticio'})
        self.assertEqual('simulado',json.loads((self.p/'recurso.json').read_text())['ClientId'])
    def test_I02_sem_cliente(self):
        with self.assertRaises(ValueError):preparar(self.p/'recurso.json',{'MR_BETA_IGDB_ACCESS_TOKEN':'simulado'})
        self.assertFalse((self.p/'recurso.json').exists())
    def test_I03_sem_token(self):
        with self.assertRaises(ValueError):preparar(self.p/'recurso.json',{'MR_BETA_IGDB_CLIENT_ID':'simulado'})
    def test_I04_escapes_permissoes(self):
        preparar(self.p/'recurso.json',{'MR_BETA_IGDB_CLIENT_ID':'cliente"ficticio','MR_BETA_IGDB_ACCESS_TOKEN':'token\\ficticio'})
        self.assertEqual('cliente"ficticio',json.loads((self.p/'recurso.json').read_text())['ClientId'])
        if os.name!='nt':self.assertEqual(0o600,(self.p/'recurso.json').stat().st_mode&0o777)
    def test_I23_controles(self):
        with self.assertRaises(ValueError):preparar(self.p/'recurso.json',{'MR_BETA_IGDB_CLIENT_ID':'simulado','MR_BETA_IGDB_ACCESS_TOKEN':'com\nespaco'})
    def test_I24_sem_segredo_na_excecao(self):
        segredo='simulado secreto'
        try:preparar(self.p/'recurso.json',{'MR_BETA_IGDB_CLIENT_ID':'simulado','MR_BETA_IGDB_ACCESS_TOKEN':segredo})
        except ValueError as e:self.assertNotIn(segredo,str(e))
    def test_I25_link_bloqueado(self):
        (self.p/'alvo').write_text('intacto');(self.p/'recurso.json').symlink_to(self.p/'alvo')
        with self.assertRaises(ValueError):preparar(self.p/'recurso.json',{'MR_BETA_IGDB_CLIENT_ID':'simulado','MR_BETA_IGDB_ACCESS_TOKEN':'simulado'})
        self.assertEqual('intacto',(self.p/'alvo').read_text())

class Pacotes(unittest.TestCase):
    def setUp(self):
        self.tmp=tempfile.TemporaryDirectory(prefix='MR.Beta.Pacote.');self.raiz=pathlib.Path(self.tmp.name);self.p=self.raiz/'publish';self.p.mkdir()
        for n in ['BacklogManager','BacklogManager.dll','BacklogManager.deps.json','BacklogManager.runtimeconfig.json','libcoreclr.so']:(self.p/n).write_bytes(b'conteudo-simulado')
        (self.p/'BacklogManager').chmod(0o755)
    def tearDown(self):self.tmp.cleanup()
    def test_P05_json_runtime(self):self.assertIn('BacklogManager.deps.json',auditar(self.p))
    def test_P06_biblioteca_nativa(self):self.assertIn('libcoreclr.so',auditar(self.p))
    def test_P09_hash(self):
        import hashlib
        self.assertEqual(hashlib.sha256(b'conteudo-simulado').hexdigest(),auditar(self.p)['BacklogManager.dll'])
    def test_P11_testes_recusados(self):self.recusa('Tests.dll')
    def test_P12_dados_recusados(self):self.recusa('backlog.json')
    def test_P12_backup_recusado(self):self.recusa('antes-restauracao.json')
    def test_P13_instrucao_recusada(self):self.recusa('FASE_BETA_INSTRUCOES.md')
    def test_P13_qodana_recusado(self):self.recusa('qodana.yaml')
    def test_P13_skills_recusadas(self):
        (self.p/'skills').mkdir();(self.p/'skills'/'pessoal.dll').write_bytes(b'pessoal')
        with self.assertRaises(ValueError):auditar(self.p)
    def test_P14_fonte_recusado(self):self.recusa('Credenciais.g.cs')
    def test_P14_simbolos_recusados(self):self.recusa('BacklogManager.pdb')
    def test_P15_token_apenas_assembly(self):
        (self.p/'BacklogManager.dll').write_bytes(b'token-ficticio');auditar(self.p,['token-ficticio'])
        (self.p/'LEIA-ME.txt').write_bytes(b'token-ficticio')
        with self.assertRaises(ValueError):auditar(self.p,['token-ficticio'])
    def test_P16_publicacao_incompleta(self):
        (self.p/'BacklogManager.dll').unlink()
        with self.assertRaises(ValueError):auditar(self.p)
    def test_P17_oculto_recusado(self):self.recusa('.mr-restauracao.json')
    def test_P18_link_recusado(self):
        (self.p/'link.dll').symlink_to(self.p/'BacklogManager.dll')
        with self.assertRaises(ValueError):auditar(self.p)
    def test_P19_pasta_vazia(self):
        vazio=self.raiz/'vazia';vazio.mkdir()
        with self.assertRaises(ValueError):auditar(vazio)
    def test_Z01_zip_integro(self):
        destino=self.raiz/'pacote.zip';self.assertEqual(64,len(compactar(self.p,destino,'win-x64')))
        with zipfile.ZipFile(destino) as z:self.assertEqual((self.p/'BacklogManager.dll').read_bytes(),z.read('BacklogManager.dll'));self.assertIsNone(z.testzip())
    def test_L01_tar_integro(self):
        destino=self.raiz/'pacote.tar.gz';compactar(self.p,destino,'linux-x64')
        with tarfile.open(destino) as t:self.assertEqual((self.p/'BacklogManager.dll').read_bytes(),t.extractfile('BacklogManager.dll').read())
    def test_L02_bit_executavel(self):
        destino=self.raiz/'pacote.tar.gz';compactar(self.p,destino,'linux-x64')
        with tarfile.open(destino) as t:self.assertTrue(t.getmember('BacklogManager').mode&0o111)
    def test_P20_sem_sobrescrita(self):
        destino=self.raiz/'pacote.zip';destino.write_bytes(b'anterior')
        with self.assertRaises(ValueError):compactar(self.p,destino,'win-x64')
        self.assertEqual(b'anterior',destino.read_bytes())
    def recusa(self,nome):
        (self.p/nome).write_bytes(b'dado-pessoal-simulado')
        with self.assertRaises(ValueError):auditar(self.p)

if __name__=='__main__':unittest.main(verbosity=2)

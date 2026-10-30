import json,pathlib,tempfile,unittest
from compilar_instalador_beta import validar_publicacao
from publicar_beta import auditar
class Instalador(unittest.TestCase):
    def setUp(self):
        self.tmp=tempfile.TemporaryDirectory();self.p=pathlib.Path(self.tmp.name)
        for n in ['BacklogManager.exe','BacklogManager.dll','BacklogManager.deps.json','BacklogManager.runtimeconfig.json','coreclr.dll']:(self.p/n).write_bytes(b'ficticio')
        self.m={'versao':'0.1.0-beta.1','rid':'win-x64','arquivos':auditar(self.p)};self.salvar()
    def tearDown(self):self.tmp.cleanup()
    def salvar(self):(self.p/'manifesto-beta.json').write_text(json.dumps(self.m),encoding='utf-8')
    def test_WB01_entrada_valida(self):self.assertEqual('0.1.0-beta.1',validar_publicacao(self.p))
    def test_WB02_linux_recusado(self):self.m['rid']='linux-x64';self.invalido()
    def test_WB03_rid_ausente(self):self.m.pop('rid');self.invalido()
    def test_WB04_versao_invalida(self):self.m['versao']='1.0.0;comando';self.invalido()
    def test_WB05_manifesto_sem_inventario(self):self.m.pop('arquivos');self.invalido()
    def test_WB06_hash_divergente(self):(self.p/'BacklogManager.dll').write_bytes(b'mudou');self.invalido()
    def test_WB07_biblioteca_pessoal(self):(self.p/'backlog.json').write_text('[]');self.invalido()
    def test_WB08_arquivo_novo(self):(self.p/'Nova.dll').write_bytes(b'novo');self.invalido()
    def test_WB09_executavel_ausente(self):(self.p/'BacklogManager.exe').unlink();self.m['arquivos']=auditar(self.p);self.invalido()
    def test_WB10_runtime_ausente(self):(self.p/'coreclr.dll').unlink();self.m['arquivos']=auditar(self.p);self.invalido()
    def test_WB11_instrucao_pessoal(self):(self.p/'FASE_BETA_INSTRUCOES.md').write_text('ficticio');self.invalido()
    def test_WB12_pdb_recusado(self):(self.p/'nativo.pdb').write_bytes(b'ficticio');self.invalido()
    def invalido(self):
        self.salvar()
        with self.assertRaises(ValueError):validar_publicacao(self.p)
if __name__=='__main__':unittest.main(verbosity=2)

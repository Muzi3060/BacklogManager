; Compilador fixado: Inno Setup 6.7.4. Geração local; qualificação Windows 11 ainda obrigatória.
#ifndef PublishDir
  #error Defina PublishDir com a publicação Windows auditada.
#endif
#ifndef OutputDir
  #error Defina OutputDir fora da publicação.
#endif
#ifndef BetaVersion
  #define BetaVersion "0.1.0-beta.1"
#endif
[Setup]
AppId={{F1C23136-BDA7-456C-93D9-13F85013731A}
AppName=Minhas Realizações
AppVersion={#BetaVersion}
AppVerName=Minhas Realizações {#BetaVersion}
DefaultDirName={localappdata}\Programs\MinhasRealizacoes
DefaultGroupName=Minhas Realizações
PrivilegesRequired=lowest
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
OutputDir={#OutputDir}
OutputBaseFilename=MinhasRealizacoes-{#BetaVersion}-win-x64-setup
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
UninstallDisplayName=Minhas Realizações
UninstallDisplayIcon={app}\BacklogManager.exe
CloseApplications=yes
CloseApplicationsFilter=BacklogManager.exe
RestartApplications=no
SetupMutex=MinhasRealizacoesSetup
[Languages]
Name: "portuguesbr"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
[Files]
Source: "{#PublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
[Icons]
Name: "{autoprograms}\Minhas Realizações"; Filename: "{app}\BacklogManager.exe"; WorkingDir: "{app}"
Name: "{autodesktop}\Minhas Realizações"; Filename: "{app}\BacklogManager.exe"; WorkingDir: "{app}"; Tasks: desktopicon
[Tasks]
Name: "desktopicon"; Description: "Criar atalho na área de trabalho"; Flags: unchecked
; Sem [UninstallDelete], execução pós-instalação ou migração pelo instalador.
; Os JSON e backups ficam fora de {app}. Não selecionar licença do projeto por inferência.

#define AppName "Ajedrez 2026"
#define AppVersion "1.1.0"
#define AppPublisher "Manu Castillo"
#define AppExeName "Ajedrez.exe"

[Setup]
AppId={{3E5C1633-3BB3-4AC5-A6C5-643EF8CADE49}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
DefaultDirName={autopf}\Ajedrez
DisableProgramGroupPage=yes
OutputDir=..\dist
OutputBaseFilename=Ajedrez-Setup-{#AppVersion}
SetupIconFile=..\Ajedrez\Logo_final.ico
UninstallDisplayIcon={app}\{#AppExeName}
UninstallDisplayName={#AppName}
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64compatible x86
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin
MinVersion=10.0

[Languages]
Name: "spanish"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "..\Ajedrez\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Excludes: "*.pdb,*.xml"

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\{#AppExeName}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchProgram,{#AppName}}"; Flags: nowait postinstall skipifsilent

[Code]
function IsDotNet472OrNewer(): Boolean;
var
  release: Cardinal;
begin
  Result := RegQueryDWordValue(HKLM,
    'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full',
    'Release', release) and (release >= 461808);
end;

function InitializeSetup(): Boolean;
var
  resultCode: Integer;
begin
  Result := True;
  if not IsDotNet472OrNewer() then
  begin
    if MsgBox('Este ordenador no tiene Microsoft .NET Framework 4.7.2 o superior,'
      + #13#10 + 'que es necesario para ejecutar {#AppName}.'
      + #13#10 + #13#10 + 'Si utilizas Windows 10 o Windows 11 ya viene instalado.'
      + #13#10 + #13#10 + '¿Abrir la página de descarga de Microsoft ahora?',
      mbConfirmation, MB_YESNO) = IDYES then
    begin
      ShellExec('open', 'https://dotnet.microsoft.com/es-es/download/dotnet-framework/net472',
        '', '', SW_SHOW, ewNoWait, resultCode);
    end;
    Result := False;
  end;
end;

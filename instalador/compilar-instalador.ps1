# Compila la app en Release y genera el instalador de Windows en dist\.
$ErrorActionPreference = 'Stop'
$raiz = Split-Path -Parent $PSScriptRoot

$vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
$msbuild = if (Test-Path $vswhere) {
    & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
} else {
    Get-ChildItem 'C:\Program Files\Microsoft Visual Studio' -Recurse -Filter MSBuild.exe -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -match '\\Bin\\MSBuild.exe$' } | Select-Object -First 1 -ExpandProperty FullName
}
if (-not $msbuild) { throw 'No se encontro MSBuild. Instala Visual Studio o Build Tools.' }

$iscc = @(
    "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles}\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe"
) | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $iscc) { throw 'No se encontro Inno Setup 6. Instalalo desde https://jrsoftware.org/isinfo.php' }

Write-Host "Compilando Release con $msbuild"
& $msbuild (Join-Path $raiz 'Ajedrez\Ajedrez.csproj') /t:Build /p:Configuration=Release /p:SolutionDir="$raiz\" /v:m /nologo
if ($LASTEXITCODE -ne 0) { throw 'La compilacion Release fallo.' }

Write-Host 'Generando el instalador'
& $iscc (Join-Path $PSScriptRoot 'Ajedrez.iss')
if ($LASTEXITCODE -ne 0) { throw 'La generacion del instalador fallo.' }

Get-ChildItem (Join-Path $raiz 'dist') -Filter 'Ajedrez-Setup-*.exe' |
    ForEach-Object { Write-Host "Instalador listo: $($_.FullName) ($([math]::Round($_.Length/1MB,1)) MB)" }

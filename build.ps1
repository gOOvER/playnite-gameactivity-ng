param(
    [string]$Configuration = 'Release',
    [switch]$NoInstall
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$projectPath = Join-Path $root 'source\GameActivity.csproj'
$outDir = Join-Path $root "source\bin\$Configuration"

Write-Host "==> Building GameActivityNG ($Configuration)..." -ForegroundColor Cyan
dotnet build $projectPath -c $Configuration -m:4
if ($LASTEXITCODE -ne 0) { throw "Build failed." }

$pext = Get-ChildItem -Path (Join-Path $root 'source\bin') -Filter "goover_GameActivityNG_Plugin*.pext" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
if ($pext) {
    Write-Host "==> Package ready: $($pext.FullName)" -ForegroundColor Green
    # Copy pext to root for easy access if desired
    Copy-Item $pext.FullName -Destination (Join-Path $root $pext.Name) -Force
}

if (-not $NoInstall) {
    $extTarget = "$env:APPDATA\Playnite\Extensions\goover_GameActivityNG_Plugin"
    $legacyTarget = "$env:APPDATA\Playnite\Extensions\playnite-gameactivity-plugin"
    
    # Close Playnite if running so files are not locked
    Stop-Process -Name "Playnite.DesktopApp", "Playnite.FullscreenApp", "Playnite" -Force -ErrorAction SilentlyContinue
    Start-Sleep -Milliseconds 500

    # Clean up legacy Lacro59 GameActivity installation if present
    if (Test-Path $legacyTarget) {
        Remove-Item -Path $legacyTarget -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "==> Cleaned up legacy extension directory: $legacyTarget" -ForegroundColor Yellow
    }

    Write-Host "==> Installing to Playnite extensions directory: $extTarget..." -ForegroundColor Cyan
    if (!(Test-Path $extTarget)) {
        New-Item -ItemType Directory -Path $extTarget -Force | Out-Null
    }
    Copy-Item -Path "$outDir\*" -Destination $extTarget -Recurse -Force
    Write-Host "==> Extension successfully installed to $extTarget" -ForegroundColor Green
}

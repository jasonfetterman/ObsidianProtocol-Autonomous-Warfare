$ErrorActionPreference = "Stop"

$ProjectRoot = "C:\ObsidianProtocol-Autonomous-Warfare\OPAW"
$UnityExe = "C:\Program Files\Unity 6000.0.80f1\Editor\Unity.exe"
$Builder = "$ProjectRoot\Assets\Editor\BuildCommanderProfileScene.cs"

$LogDir = "$ProjectRoot\BuildLogs"
$CompileLog = "$LogDir\CommanderProfile_Compile.log"
$BuildLog = "$LogDir\CommanderProfile_Build.log"

New-Item -ItemType Directory -Force -Path $LogDir | Out-Null

function Invoke-Unity {
    param([string[]]$Arguments)

    $p = Start-Process `
        -FilePath $UnityExe `
        -ArgumentList $Arguments `
        -Wait `
        -PassThru `
        -NoNewWindow

    return $p.ExitCode
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host " OBSIDIAN PROTOCOL" -ForegroundColor Cyan
Write-Host " COMMANDER PROFILE BUILD" -ForegroundColor Cyan
Write-Host " UNITY 6000.0.80f1" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

if (!(Test-Path $UnityExe)) {
    throw "Unity 6000.0.80f1 not found: $UnityExe"
}

if (!(Test-Path $Builder)) {
    throw "Builder not found: $Builder"
}

Write-Host "[1/4] Unity verified." -ForegroundColor Green
Write-Host $UnityExe

Write-Host ""
Write-Host "[2/4] Builder verified." -ForegroundColor Green
Write-Host $Builder

Write-Host ""
Write-Host "[3/4] Unity compile/import pass..." -ForegroundColor Yellow

$exitCode = Invoke-Unity @(
    "-batchmode",
    "-quit",
    "-projectPath",
    $ProjectRoot,
    "-logFile",
    $CompileLog
)

if ($exitCode -ne 0) {
    Write-Host ""
    Write-Host "UNITY COMPILE/IMPORT FAILED." -ForegroundColor Red
    Write-Host "EXIT CODE: $exitCode" -ForegroundColor Red
    Write-Host "LOG: $CompileLog" -ForegroundColor Yellow
    exit $exitCode
}

Write-Host "Compile/import PASS." -ForegroundColor Green

Write-Host ""
Write-Host "[4/4] Building Commander Profile..." -ForegroundColor Yellow

$exitCode = Invoke-Unity @(
    "-batchmode",
    "-quit",
    "-projectPath",
    $ProjectRoot,
    "-executeMethod",
    "BuildCommanderProfileScene.Build",
    "-logFile",
    $BuildLog
)

if ($exitCode -ne 0) {
    Write-Host ""
    Write-Host "COMMANDER PROFILE BUILD FAILED." -ForegroundColor Red
    Write-Host "EXIT CODE: $exitCode" -ForegroundColor Red
    Write-Host "LOG: $BuildLog" -ForegroundColor Yellow
    exit $exitCode
}

$Scene = Get-ChildItem `
    -Path "$ProjectRoot\Assets" `
    -Filter "Player_Commander_Profile.unity" `
    -Recurse `
    -File |
    Select-Object -First 1 -ExpandProperty FullName

if (!$Scene) {
    Write-Host ""
    Write-Host "BUILD RAN BUT PLAYER COMMANDER PROFILE SCENE WAS NOT FOUND." -ForegroundColor Red
    Write-Host "BUILD LOG: $BuildLog" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host " COMMANDER PROFILE BUILD COMPLETE" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""
Write-Host "Unity: 6000.0.80f1"
Write-Host ""
Write-Host "SCENE FOUND:" -ForegroundColor Green
Write-Host $Scene
Write-Host ""
Write-Host "Compile Log: $CompileLog"
Write-Host "Build Log:   $BuildLog"
Write-Host ""

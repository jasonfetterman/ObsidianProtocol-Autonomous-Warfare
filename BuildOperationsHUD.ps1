# ============================================================
# OBSIDIAN PROTOCOL
# OPERATIONS HUD BUILDER
# BuildOperationsHUD.ps1
#
# Unity 6 / 6000.5.5f1
#
# Automatically:
#   1. Fixes invalid package dependencies
#   2. Writes manifest.json as UTF-8 WITHOUT BOM
#   3. Runs Unity compile/import
#   4. Runs OperationsVisualBuilder.Build
#   5. Verifies Operations.unity
# ============================================================

$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "============================================================" -ForegroundColor Cyan
Write-Host " OBSIDIAN PROTOCOL - OPERATIONS HUD BUILDER" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor Cyan
Write-Host ""

# ============================================================
# PROJECT
# ============================================================

$ProjectRoot = "C:\ObsidianProtocol-Autonomous-Warfare\OPAW"

if (-not (Test-Path $ProjectRoot)) {
    Write-Host "ERROR: Project not found:" -ForegroundColor Red
    Write-Host $ProjectRoot -ForegroundColor Red
    exit 1
}

Set-Location $ProjectRoot

Write-Host "PROJECT:" -ForegroundColor Yellow
Write-Host $ProjectRoot
Write-Host ""

# ============================================================
# UNITY
# ============================================================

$UnityExe = "C:\Program Files\Unity\Hub\Editor\6000.5.5f1\Editor\Unity.exe"

if (-not (Test-Path $UnityExe)) {
    Write-Host "ERROR: Unity executable not found:" -ForegroundColor Red
    Write-Host $UnityExe -ForegroundColor Red
    exit 1
}

Write-Host "UNITY:" -ForegroundColor Yellow
Write-Host $UnityExe
Write-Host ""

# ============================================================
# PATHS
# ============================================================

$ManifestPath = Join-Path $ProjectRoot "Packages\manifest.json"

$OperationsScene = Join-Path `
    $ProjectRoot `
    "Assets\Scenes\SCN-05  OPERATIONS\[HUD] OPERATIONS HUD\Operations.unity"

$LogDirectory = Join-Path $ProjectRoot "BuildLogs"

$CompileLog = Join-Path `
    $LogDirectory `
    "OperationsHUD_Compile.log"

$BuildLog = Join-Path `
    $LogDirectory `
    "OperationsHUD_Build.log"

if (-not (Test-Path $LogDirectory)) {
    New-Item `
        -ItemType Directory `
        -Path $LogDirectory `
        -Force | Out-Null
}

# ============================================================
# FIX MANIFEST
# ============================================================

Write-Host "============================================================" -ForegroundColor Cyan
Write-Host " FIXING UNITY PACKAGE MANIFEST" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor Cyan
Write-Host ""

if (-not (Test-Path $ManifestPath)) {
    Write-Host "ERROR: manifest.json not found:" -ForegroundColor Red
    Write-Host $ManifestPath -ForegroundColor Red
    exit 1
}

Write-Host "Manifest:" -ForegroundColor Yellow
Write-Host $ManifestPath
Write-Host ""

# ------------------------------------------------------------
# Read manifest using .NET directly.
# This avoids PowerShell encoding surprises.
# ------------------------------------------------------------

$ManifestText = [System.IO.File]::ReadAllText(
    $ManifestPath,
    [System.Text.UTF8Encoding]::new($false)
)

# ------------------------------------------------------------
# Remove invalid Unity module dependencies.
# ------------------------------------------------------------

$InvalidPackages = @(
    "com.unity.modules.adaptiveperformance",
    "com.unity.modules.physicscore2d",
    "com.unity.modules.vectorgraphics"
)

foreach ($PackageName in $InvalidPackages) {

    $Pattern = '(?m)^\s*"' +
        [regex]::Escape($PackageName) +
        '"\s*:\s*"[^"]+"\s*,?\s*\r?\n'

    if ($ManifestText -match $Pattern) {

        Write-Host "Removing invalid dependency:" -ForegroundColor Yellow
        Write-Host "  $PackageName" -ForegroundColor Yellow

        $ManifestText = [regex]::Replace(
            $ManifestText,
            $Pattern,
            ""
        )
    }
    else {

        Write-Host "Not present:" -ForegroundColor DarkGray
        Write-Host "  $PackageName" -ForegroundColor DarkGray
    }
}

# ============================================================
# REMOVE UTF-8 BOM IF ONE EXISTS
# ============================================================

$ManifestText = $ManifestText.TrimStart([char]0xFEFF)

# ============================================================
# VALIDATE JSON
# ============================================================

try {

    $ManifestObject = $ManifestText | ConvertFrom-Json

}
catch {

    Write-Host ""
    Write-Host "ERROR: manifest.json is invalid after modification." -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}

# ============================================================
# WRITE UTF-8 WITHOUT BOM
# ============================================================

$Utf8NoBom = New-Object System.Text.UTF8Encoding($false)

[System.IO.File]::WriteAllText(
    $ManifestPath,
    $ManifestText,
    $Utf8NoBom
)

Write-Host ""
Write-Host "Manifest saved as UTF-8 WITHOUT BOM." -ForegroundColor Green
Write-Host ""

# ============================================================
# VERIFY RAW FIRST BYTES
# ============================================================

$RawManifestBytes = [System.IO.File]::ReadAllBytes($ManifestPath)

if ($RawManifestBytes.Length -ge 3) {

    $HasUtf8Bom = (
        $RawManifestBytes[0] -eq 0xEF -and
        $RawManifestBytes[1] -eq 0xBB -and
        $RawManifestBytes[2] -eq 0xBF
    )

    if ($HasUtf8Bom) {

        Write-Host "ERROR: UTF-8 BOM still exists." -ForegroundColor Red
        exit 1
    }
}

Write-Host "MANIFEST BOM CHECK: PASS" -ForegroundColor Green

# ============================================================
# VERIFY INVALID PACKAGES
# ============================================================

$FinalManifestText = [System.IO.File]::ReadAllText(
    $ManifestPath,
    [System.Text.UTF8Encoding]::new($false)
)

$StillInvalid = @()

foreach ($PackageName in $InvalidPackages) {

    if ($FinalManifestText -match [regex]::Escape($PackageName)) {
        $StillInvalid += $PackageName
    }
}

if ($StillInvalid.Count -gt 0) {

    Write-Host ""
    Write-Host "ERROR: Invalid package dependencies remain:" -ForegroundColor Red

    foreach ($PackageName in $StillInvalid) {
        Write-Host "  $PackageName" -ForegroundColor Red
    }

    exit 1
}

Write-Host "PACKAGE MANIFEST CHECK: PASS" -ForegroundColor Green
Write-Host ""

# ============================================================
# SHOW RELEVANT PACKAGES
# ============================================================

Write-Host "Current relevant package entries:" -ForegroundColor Yellow

$FinalManifestText `
    -split "`r?`n" |
    Where-Object {
        $_ -match "com.unity.(inputsystem|ugui|render-pipelines.universal|ai.navigation)"
    } |
    ForEach-Object {
        Write-Host "  $_"
    }

Write-Host ""

# ============================================================
# DELETE OLD LOGS
# ============================================================

if (Test-Path $CompileLog) {
    Remove-Item $CompileLog -Force
}

if (Test-Path $BuildLog) {
    Remove-Item $BuildLog -Force
}

# ============================================================
# UNITY RUNNER
# ============================================================

function Invoke-Unity {

    param (
        [string]$Arguments,
        [string]$LogPath
    )

    Write-Host ""
    Write-Host "------------------------------------------------------------" -ForegroundColor DarkGray
    Write-Host "UNITY COMMAND" -ForegroundColor Yellow
    Write-Host "------------------------------------------------------------" -ForegroundColor DarkGray
    Write-Host ""

    Write-Host "$UnityExe $Arguments" -ForegroundColor DarkGray
    Write-Host ""

    $StartInfo = New-Object System.Diagnostics.ProcessStartInfo

    $StartInfo.FileName = $UnityExe
    $StartInfo.Arguments = $Arguments
    $StartInfo.WorkingDirectory = $ProjectRoot

    $StartInfo.UseShellExecute = $false
    $StartInfo.RedirectStandardOutput = $true
    $StartInfo.RedirectStandardError = $true
    $StartInfo.CreateNoWindow = $true

    $Process = New-Object System.Diagnostics.Process

    $Process.StartInfo = $StartInfo

    $OutputBuilder = New-Object System.Text.StringBuilder

    $OutputHandler = {

        param (
            $Sender,
            $EventArgs
        )

        if ($null -ne $EventArgs.Data) {

            [void]$OutputBuilder.AppendLine(
                $EventArgs.Data
            )

            Write-Host $EventArgs.Data
        }
    }

    $ErrorHandler = {

        param (
            $Sender,
            $EventArgs
        )

        if ($null -ne $EventArgs.Data) {

            [void]$OutputBuilder.AppendLine(
                $EventArgs.Data
            )

            Write-Host $EventArgs.Data
        }
    }

    $Process.add_OutputDataReceived($OutputHandler)
    $Process.add_ErrorDataReceived($ErrorHandler)

    try {

        [void]$Process.Start()

        $Process.BeginOutputReadLine()
        $Process.BeginErrorReadLine()

        $Process.WaitForExit()

        Start-Sleep -Milliseconds 500

    }
    finally {

        $Process.remove_OutputDataReceived($OutputHandler)
        $Process.remove_ErrorDataReceived($ErrorHandler)
    }

    $OutputBuilder.ToString() |
        Set-Content `
            -Path $LogPath `
            -Encoding UTF8

    return $Process.ExitCode
}

# ============================================================
# PASS 1
# ============================================================

Write-Host ""
Write-Host "============================================================" -ForegroundColor Cyan
Write-Host " PASS 1 - UNITY IMPORT / COMPILE" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor Cyan
Write-Host ""

$Pass1Arguments = @(
    "-batchmode",
    "-quit",
    "-projectPath `"$ProjectRoot`"",
    "-logFile `"$CompileLog`""
)

$Pass1ArgumentString = $Pass1Arguments -join " "

$Pass1ExitCode = Invoke-Unity `
    -Arguments $Pass1ArgumentString `
    -LogPath $CompileLog

if ($Pass1ExitCode -ne 0) {

    Write-Host ""
    Write-Host "============================================================" -ForegroundColor Red
    Write-Host " PASS 1 FAILED" -ForegroundColor Red
    Write-Host "============================================================" -ForegroundColor Red
    Write-Host ""

    Write-Host "Unity exit code: $Pass1ExitCode" -ForegroundColor Red
    Write-Host ""

    if (Test-Path $CompileLog) {

        Write-Host "Last compiler output:" -ForegroundColor Yellow
        Write-Host ""

        Get-Content `
            $CompileLog `
            -Tail 150
    }

    exit $Pass1ExitCode
}

Write-Host ""
Write-Host "PASS 1 COMPLETE." -ForegroundColor Green
Write-Host ""

# ============================================================
# PASS 2
# ============================================================

Write-Host "============================================================" -ForegroundColor Cyan
Write-Host " PASS 2 - BUILD OPERATIONS HUD" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor Cyan
Write-Host ""

$Pass2Arguments = @(
    "-batchmode",
    "-quit",
    "-projectPath `"$ProjectRoot`"",
    "-executeMethod OperationsVisualBuilder.Build",
    "-logFile `"$BuildLog`""
)

$Pass2ArgumentString = $Pass2Arguments -join " "

$Pass2ExitCode = Invoke-Unity `
    -Arguments $Pass2ArgumentString `
    -LogPath $BuildLog

if ($Pass2ExitCode -ne 0) {

    Write-Host ""
    Write-Host "============================================================" -ForegroundColor Red
    Write-Host " PASS 2 FAILED" -ForegroundColor Red
    Write-Host "============================================================" -ForegroundColor Red
    Write-Host ""

    Write-Host "Unity exit code: $Pass2ExitCode" -ForegroundColor Red
    Write-Host ""

    if (Test-Path $BuildLog) {

        Write-Host "Last build output:" -ForegroundColor Yellow
        Write-Host ""

        Get-Content `
            $BuildLog `
            -Tail 200
    }

    exit $Pass2ExitCode
}

# ============================================================
# VERIFY OPERATIONS SCENE
# ============================================================

Write-Host ""
Write-Host "============================================================" -ForegroundColor Cyan
Write-Host " VERIFYING OPERATIONS SCENE" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor Cyan
Write-Host ""

if (Test-Path $OperationsScene) {

    $SceneInfo = Get-Item $OperationsScene

    Write-Host "OPERATIONS HUD BUILD COMPLETE." -ForegroundColor Green
    Write-Host ""

    Write-Host "Scene:" -ForegroundColor Yellow
    Write-Host $OperationsScene
    Write-Host ""

    Write-Host "Scene size:" -ForegroundColor Yellow
    Write-Host (
        "{0:N0} bytes" -f $SceneInfo.Length
    )
    Write-Host ""

}
else {

    Write-Host "ERROR: Operations.unity was not created." -ForegroundColor Red
    Write-Host ""

    Write-Host "Expected path:" -ForegroundColor Yellow
    Write-Host $OperationsScene
    Write-Host ""

    if (Test-Path $BuildLog) {

        Write-Host "Last build output:" -ForegroundColor Yellow
        Write-Host ""

        Get-Content `
            $BuildLog `
            -Tail 200
    }

    exit 1
}

# ============================================================
# FINAL SUCCESS
# ============================================================

Write-Host ""
Write-Host "============================================================" -ForegroundColor Green
Write-Host " SUCCESS" -ForegroundColor Green
Write-Host "============================================================" -ForegroundColor Green
Write-Host ""

Write-Host "PASS 1 COMPLETE." -ForegroundColor Green
Write-Host "OPERATIONS HUD BUILD COMPLETE." -ForegroundColor Green
Write-Host ""

Write-Host "Operations scene:" -ForegroundColor Yellow
Write-Host $OperationsScene
Write-Host ""

Write-Host "Compile log:" -ForegroundColor Yellow
Write-Host $CompileLog
Write-Host ""

Write-Host "Build log:" -ForegroundColor Yellow
Write-Host $BuildLog
Write-Host ""

Write-Host "============================================================" -ForegroundColor Green
Write-Host " OPERATIONS HUD BUILD FINISHED" -ForegroundColor Green
Write-Host "============================================================" -ForegroundColor Green
Write-Host ""
$ErrorActionPreference = "Stop"

$scene = "Assets\Scenes\SCN06_RESEARCH\RESEARCH HUD\Research_Lab.unity"
if (-not (Test-Path $scene)) { throw "Scene not found: $scene" }

$original = Get-Content $scene -Raw

# Each target is a direct canvas child. Values are X, Y, width, height.
$layout = [ordered]@{
    "TOP BAR" = @(0, 475, 1843, 90)
    "BOTTOM BAR" = @(0, -505, 1843, 54)
    "HUD BACKGROUND" = @(0, 0, 1920, 1080)
    "TECHNOLOGY TREE PANEL" = @(-640, -5, 520, 760)
    "CENTER TECHNOLOGY PANEL" = @(0, -5, 600, 760)
    "RIGHT RESEARCH PANEL" = @(640, -5, 520, 760)
    "TEXT TECHNOLOGY TREE" = @(-640, 330, 460, 46)
    "TEXT TECHNOLOGY TREE // MOBILITY" = @(0, 330, 540, 46)
    "TEXT RESEARCH PROJECTS" = @(640, 330, 440, 40)
    "TEXT RESEARCH RESOURCES" = @(640, 55, 440, 38)
    "TEXT ACTIVE RESEARCH" = @(640, 245, 440, 42)
    "TEXT SELECT A TECHNOLOGY NODE TO VIEW REQUIREMENTS" = @(640, 135, 440, 100)
    "TEXT ENERGY ALLOCATION  91%" = @(640, -245, 440, 32)
    "PROGRESS BACKGROUND" = @(640, -160, 400, 24)
    "TAB SENSORS" = @(-640, 245, 440, 44)
    "TAB WEAPONS" = @(-640, 190, 440, 44)
    "TAB PROPULSION" = @(-640, 135, 440, 44)
    "TAB MOBILITY" = @(-640, 80, 440, 44)
    "TAB DEFENSE" = @(-640, 25, 440, 44)
    "TAB AI" = @(-640, -30, 440, 44)
    "TAB COMMUNICATIONS" = @(-640, -85, 440, 44)
    "TAB LOGISTICS" = @(-640, -140, 440, 44)
    "TAB EXPERIMENTAL" = @(-640, -195, 440, 44)
    "TECH AUTONOMOUS MOVEMENT" = @(0, 175, 190, 82)
    "TECH MOBILITY I" = @(-175, 35, 170, 82)
    "TECH MOBILITY II" = @(0, 35, 170, 82)
    "TECH MOBILITY III" = @(175, 35, 170, 82)
    "TECH ADVANCED MOBILITY" = @(0, -105, 190, 82)
    "PROJECT TAB ACTIVE" = @(640, 185, 400, 34)
    "PROJECT TAB QUEUED" = @(640, 143, 400, 34)
    "PROJECT TAB COMPLETED" = @(640, 101, 400, 34)
    "PROJECT TAB AVAILABLE" = @(640, 59, 400, 34)
    "TEXT PROGRESS" = @(640, -125, 400, 30)
    "TEXT MATERIALS        72%" = @(640, -205, 400, 30)
    "TEXT SCIENTISTS       1 / 1" = @(640, -285, 400, 30)
    "TEXT AVAILABLE RESEARCH          18" = @(-640, -300, 440, 32)
    "TEXT UNLOCKED TECHNOLOGIES     42" = @(-640, -340, 440, 32)
    "TEXT BRANCH STATUS" = @(-640, -260, 440, 32)
    "TEXT 12. RESEARCH & TECHNOLOGY" = @(-500, 475, 520, 48)
    "TEXT RESEARCH LAB // LEVEL 12" = @(420, 475, 440, 42)
    "TEXT ENERGY  91%" = @(700, 475, 160, 38)
    "TEXT ONLINE" = @(850, 475, 100, 38)
    "'TEXT RESEARCH NETWORK: ONLINE'" = @(160, 475, 380, 32)
    "OPEN TECHNOLOGY TREE" = @(0, -445, 360, 44)
    "CONFIRM RESEARCH" = @(520, -385, 180, 42)
    "CANCEL RESEARCH" = @(730, -385, 180, 42)
    "VIEW DETAILS" = @(520, -330, 180, 38)
}

# Parse scene objects and preserve the exact serialized names.
$goNames = @{}
foreach ($m in [regex]::Matches($original, '(?ms)^--- !u!1 &(?<id>\d+)\s*\n(?<body>.*?)(?=^--- !u!|\z)')) {
    $n = [regex]::Match($m.Groups['body'].Value, '(?m)^\s*m_Name:\s*(?<name>.*)$')
    if ($n.Success) { $goNames[$m.Groups['id'].Value] = $n.Groups['name'].Value.Trim() }
}

$rects = @{}
foreach ($m in [regex]::Matches($original, '(?ms)^--- !u!224 &(?<id>\d+)\s*\n(?<body>.*?)(?=^--- !u!|\z)')) {
    $g = [regex]::Match($m.Groups['body'].Value, '(?m)^\s*m_GameObject:\s*\{fileID:\s*(?<go>-?\d+)')
    if ($g.Success) { $rects[$g.Groups['go'].Value] = $m.Groups['id'].Value }
}

$resolved = @{}
foreach ($name in $layout.Keys) {
    $matches = @($goNames.GetEnumerator() | Where-Object { $_.Value -ceq $name })
    if ($matches.Count -ne 1) { throw "Validation failed: exact GameObject name [$name] found $($matches.Count) times." }
    $goId = $matches[0].Key
    if (-not $rects.ContainsKey($goId)) { throw "Validation failed: RectTransform missing for [$name]." }
    $resolved[$name] = $rects[$goId]
}

$updated = $original
foreach ($name in $layout.Keys) {
    $tid = $resolved[$name]
    $v = $layout[$name]
    $pattern = "(?ms)(^--- !u!224 &$tid\s*\n)(?<body>.*?)(?=^--- !u!|\z)"
    $m = [regex]::Match($updated, $pattern)
    if (-not $m.Success) { throw "Validation failed: RectTransform block missing for [$name]." }

    $body = $m.Groups['body'].Value
    foreach ($field in @("m_AnchorMin", "m_AnchorMax", "m_AnchoredPosition", "m_SizeDelta")) {
        if ($body -notmatch "(?m)^\s*${field}:") { throw "Validation failed: $field missing for [$name]." }
    }

    $body = [regex]::Replace($body, '(?m)^(\s*m_AnchorMin:\s*).*$',
        '${1}{x: 0.5, y: 0.5}', 1)
    $body = [regex]::Replace($body, '(?m)^(\s*m_AnchorMax:\s*).*$',
        '${1}{x: 0.5, y: 0.5}', 1)
    $body = [regex]::Replace($body, '(?m)^(\s*m_AnchoredPosition:\s*).*$',
        ('${1}{x: ' + $v[0] + ', y: ' + $v[1] + '}'), 1)
    $body = [regex]::Replace($body, '(?m)^(\s*m_SizeDelta:\s*).*$',
        ('${1}{x: ' + $v[2] + ', y: ' + $v[3] + '}'), 1)

    $updated = $updated.Substring(0, $m.Index) +
        $m.Groups[1].Value + $body +
        $updated.Substring($m.Index + $m.Length)
}

if ($updated -eq $original) { throw "No changes generated; scene not written." }

# Final integrity checks happen before the only write.
foreach ($name in $layout.Keys) {
    $tid = $resolved[$name]
    if ($updated -notmatch "(?m)^--- !u!224 &$tid\s*$") {
        throw "Pre-write verification failed for [$name]. Scene not written."
    }
}

$stamp = Get-Date -Format "yyyyMMdd_HHmmss"
$backup = "$scene.pre_full_hud_layout_$stamp.bak"
Copy-Item -LiteralPath $scene -Destination $backup
if ((Get-FileHash $backup -Algorithm SHA256).Hash -ne (Get-FileHash $scene -Algorithm SHA256).Hash) {
    throw "Backup verification failed; scene not modified."
}

[System.IO.File]::WriteAllText(
    (Join-Path (Get-Location) $scene),
    $updated,
    [System.Text.UTF8Encoding]::new($false)
)

if ((Get-Content $scene -Raw) -ne $updated) {
    Copy-Item -LiteralPath $backup -Destination $scene -Force
    throw "Post-write verification failed. Original scene restored."
}

Write-Host "HUD layout written and verified." -ForegroundColor Green
Write-Host "Backup: $backup"
Write-Host "Updated targets: $($layout.Count)"

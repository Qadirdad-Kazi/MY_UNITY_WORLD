# Extract Downloads\ASSETS into Assets/Game_Models with a clean category layout.
# Safe for Unity: keeps .meta on existing assets; only organizes NEW downloads + loose root GLBs.

$ErrorActionPreference = "Continue"
$SevenZip = "C:\Program Files\7-Zip\7z.exe"
$Src = Join-Path $env:USERPROFILE "Downloads\ASSETS"
$DestRoot = "D:\Github Clones\MY_UNITY_WORLD\Assets\Game_Models"
$Log = Join-Path $DestRoot "_import_log.txt"

function Write-Log($msg) {
    $line = "[{0}] {1}" -f (Get-Date -Format "HH:mm:ss"), $msg
    Write-Host $line
    Add-Content -Path $Log -Value $line
}

function Ensure-Dir($path) {
    if (-not (Test-Path $path)) { New-Item -ItemType Directory -Force -Path $path | Out-Null }
}

function Sanitize-Name([string]$name) {
    $n = $name.ToLowerInvariant()
    $n = $n -replace '\.(zip|rar|7z|glb|fbx)$', ''
    $n = $n -replace '[^a-z0-9]+', '-'
    $n = $n.Trim('-')
    if ([string]::IsNullOrWhiteSpace($n)) { $n = "asset" }
    return $n
}

function Get-Category([string]$name) {
    $n = $name.ToLowerInvariant()
    # Use word-ish boundaries so "bat" doesn't match "bathroom", "crow" != "scarecrow"
    if ($n -match '(^|[^a-z])speedometer([^a-z]|$)') { return "UI" }
    if ($n -match 'dirt-road|road-through') { return "Environment" }
    if ($n -match 'fighter-jet|sukhoi|musclecar|mustang|truck|predator|volt-panther') { return "Vehicles" }
    if ($n -match '(^|[^a-z])(vulture|bat|humming|pigeon|egret|titmouse|crow|eagle|raven|bird)([^a-z]|$)') { return "Birds" }
    if ($n -match 'animal|cat|crocodile|boar|horse|tiger|rhino|turtle|toad|panther|alien|collection-with|scarecrow') { return "Animals" }
    if ($n -match 'plant|cactus|succulent|aloe|hyacinth|mushroom|fungus|echeveria') { return "Plants" }
    if ($n -match 'boat|paddle|gazebo|tent|fire-pit|playground|lantern|speaker|pickaxe|skull|robot|castle|bridge|house|tree|throne|defective|tylosaurus') { return "Props" }
    return "Props"
}

function Extract-Archive([string]$archive, [string]$outDir) {
    Ensure-Dir $outDir
    # Prefer 7-Zip (handles zip/rar/7z)
    & $SevenZip x -y "-o$outDir" $archive | Out-Null
    return ($LASTEXITCODE -eq 0)
}

function Flatten-SingleRoot([string]$dir) {
    # If extract created one nested folder with same content, unwrap one level
    $children = Get-ChildItem $dir -Force | Where-Object { $_.Name -ne ".DS_Store" }
    if ($children.Count -eq 1 -and $children[0].PSIsContainer) {
        $inner = $children[0]
        Get-ChildItem $inner.FullName -Force | ForEach-Object {
            $target = Join-Path $dir $_.Name
            if (Test-Path $target) {
                # keep nested
            } else {
                Move-Item -LiteralPath $_.FullName -Destination $dir -Force
            }
        }
        # remove empty inner if empty
        if (-not (Get-ChildItem $inner.FullName -Force -ErrorAction SilentlyContinue)) {
            Remove-Item -LiteralPath $inner.FullName -Force -Recurse -ErrorAction SilentlyContinue
        }
    }
}

# --- start ---
"=== Game_Models import $(Get-Date -Format o) ===" | Set-Content $Log
Write-Log "Source: $Src"
Write-Log "Dest:   $DestRoot"

if (-not (Test-Path $Src)) { throw "Missing source folder: $Src" }
if (-not (Test-Path $SevenZip)) { throw "7-Zip not found at $SevenZip" }
Ensure-Dir $DestRoot

$categories = @("Vehicles","Animals","Birds","Plants","Props","Environment","UI")
foreach ($c in $categories) { Ensure-Dir (Join-Path $DestRoot $c) }

$ok = 0; $fail = 0; $skip = 0

# 1) Extract archives from Downloads\ASSETS
$archives = Get-ChildItem $Src -File | Where-Object { $_.Extension -match '\.(zip|rar|7z)$' }
Write-Log ("Found {0} archives" -f $archives.Count)

foreach ($arch in $archives) {
    $slug = Sanitize-Name $arch.Name
    $cat = Get-Category $arch.Name
    $outDir = Join-Path (Join-Path $DestRoot $cat) $slug

    if (Test-Path $outDir) {
        $hasContent = Get-ChildItem $outDir -Recurse -File -ErrorAction SilentlyContinue |
            Where-Object { $_.Extension -match '\.(fbx|glb|gltf|obj|png|jpg|jpeg|tga|psd|mat|prefab)$' } |
            Select-Object -First 1
        if ($hasContent) {
            Write-Log "SKIP (exists): $cat/$slug"
            $skip++
            continue
        }
    }

    Write-Log "EXTRACT → $cat/$slug  ($([math]::Round($arch.Length/1MB,1)) MB)"
    Ensure-Dir $outDir
    if (Extract-Archive $arch.FullName $outDir) {
        Flatten-SingleRoot $outDir
        # Drop nested archives leftover inside extract (optional keep for now)
        $ok++
    } else {
        Write-Log "FAIL: $($arch.Name)"
        $fail++
    }
}

# 2) Copy loose GLB/FBX from Downloads\ASSETS
$looseSrc = Get-ChildItem $Src -File | Where-Object { $_.Extension -match '\.(glb|fbx|gltf)$' }
foreach ($f in $looseSrc) {
    $slug = Sanitize-Name $f.Name
    $cat = Get-Category $f.Name
    $outDir = Join-Path (Join-Path $DestRoot $cat) $slug
    Ensure-Dir $outDir
    $destFile = Join-Path $outDir $f.Name
    if (-not (Test-Path $destFile)) {
        Copy-Item -LiteralPath $f.FullName -Destination $destFile -Force
        Write-Log "COPY → $cat/$slug/$($f.Name)"
        $ok++
    } else {
        Write-Log "SKIP loose: $cat/$slug/$($f.Name)"
        $skip++
    }
}

# 3) Organize loose GLBs already sitting on Game_Models root
$rootLoose = Get-ChildItem $DestRoot -File | Where-Object { $_.Extension -match '\.(glb|fbx|gltf)$' }
foreach ($f in $rootLoose) {
    $slug = Sanitize-Name $f.Name
    $cat = Get-Category $f.Name
    $outDir = Join-Path (Join-Path $DestRoot $cat) $slug
    Ensure-Dir $outDir
    $destFile = Join-Path $outDir $f.Name
    $meta = "$($f.FullName).meta"
    if (-not (Test-Path $destFile)) {
        Move-Item -LiteralPath $f.FullName -Destination $destFile -Force
        if (Test-Path $meta) {
            Move-Item -LiteralPath $meta -Destination "$destFile.meta" -Force
        }
        Write-Log "MOVE root → $cat/$slug/$($f.Name)"
        $ok++
    } else {
        Write-Log "SKIP root (dest exists): $($f.Name)"
        $skip++
    }
}

# 4) Nested archive pass (one level) — extract zip/rar left inside new folders
$nested = Get-ChildItem (Join-Path $DestRoot "*") -Recurse -File -ErrorAction SilentlyContinue |
    Where-Object {
        $_.Extension -match '\.(zip|rar|7z)$' -and
        $_.FullName -match '\\(Vehicles|Animals|Birds|Plants|Props|Environment|UI)\\'
    }
foreach ($n in $nested) {
    # extract beside archive into "<name>_extracted" once
    $target = Join-Path $n.DirectoryName ($n.BaseName + "_extracted")
    if (Test-Path $target) { continue }
    Write-Log "NESTED EXTRACT → $($n.FullName.Substring($DestRoot.Length+1))"
    if (Extract-Archive $n.FullName $target) {
        Flatten-SingleRoot $target
        $ok++
    } else {
        $fail++
    }
}

Write-Log "DONE  ok=$ok  skip=$skip  fail=$fail"
Write-Log "Categories under Game_Models:"
foreach ($c in $categories) {
    $p = Join-Path $DestRoot $c
    $count = (Get-ChildItem $p -Directory -ErrorAction SilentlyContinue | Measure-Object).Count
    Write-Log ("  {0,-12} {1} packs" -f $c, $count)
}

Write-Host ""
Write-Host "Log: $Log"

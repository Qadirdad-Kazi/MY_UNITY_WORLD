# Import Downloads\ASSETS (folders + zips) into Assets/Game_Models
# - Extract archives
# - Rename packs to clean UPPERCASE names
# - Categorize
# - Delete source zips after successful extract
# - Keep Unity-safe: never touch existing .meta on already-imported packs

$ErrorActionPreference = "Continue"
$SevenZip = "C:\Program Files\7-Zip\7z.exe"
$Src = Join-Path $env:USERPROFILE "Downloads\ASSETS"
$DestRoot = "D:\Github Clones\MY_UNITY_WORLD\Assets\Game_Models"
$Log = Join-Path $DestRoot "_import_batch2_log.txt"
$DeleteZips = $true

function Write-Log($msg) {
    $line = "[{0}] {1}" -f (Get-Date -Format "HH:mm:ss"), $msg
    Write-Host $line
    Add-Content -Path $Log -Value $line
}

function Ensure-Dir($path) {
    if (-not (Test-Path -LiteralPath $path)) {
        New-Item -ItemType Directory -Force -Path $path | Out-Null
    }
}

function Sanitize-Upper([string]$name) {
    $n = $name
    $n = $n -replace '\.(zip|rar|7z|glb|fbx|gltf)$', ''
    $n = $n -replace '\s*\(\d+\)\s*$', ''   # drop " (1)" duplicates
    $n = $n -replace '[^A-Za-z0-9]+', '_'
    $n = $n.Trim('_').ToUpperInvariant()
    if ([string]::IsNullOrWhiteSpace($n)) { $n = "ASSET" }
    return $n
}

# Explicit rename map (source basename -> pack name). Keys are lowercase without extension.
$RenameMap = @{
    # BEACH
    'beach-chair-blue-stripes' = 'BEACH_CHAIR_BLUE'
    'beach-chair'              = 'BEACH_CHAIR'
    'beach-kit'                = 'BEACH_KIT'
    'beach-restaurant'         = 'BEACH_RESTAURANT'
    'beach-table'              = 'BEACH_TABLE'
    'beach_ball'               = 'BEACH_BALL'
    'blue-beach-umbrella'      = 'BEACH_UMBRELLA_BLUE'
    'house'                    = 'BEACH_HOUSE'          # only used when under BEACH folder
    'metallic_garden_table'    = 'GARDEN_TABLE_METAL'

    # BUILDINGS
    'apartment-houses-level-four-pack'           = 'APARTMENT_L4'
    'apartment-houses-level-three-pack'          = 'APARTMENT_L3'
    'brutalist-building'                         = 'BRUTALIST_BUILDING'
    'city_nyc_buildings'                         = 'NYC_BUILDINGS'
    'free-london-skyscraper'                     = 'LONDON_SKYSCRAPER'
    'future-city'                                = 'FUTURE_CITY'
    'low-poly-night-city-building-skyline'       = 'NIGHT_CITY_SKYLINE'
    'old_house'                                  = 'OLD_HOUSE'
    'wooden-house-house-in-the-woods-project'    = 'WOODEN_HOUSE_WOODS'

    # FACTORIES
    'abandoned_factory_hall_scan' = 'FACTORY_ABANDONED_HALL'
    'factory--electrical--box-12-mb' = 'FACTORY_ELECTRICAL_BOX'
    'tanr_makina_factory_building' = 'FACTORY_TANR_MAKINA'
    'vaccume_tube_factory'         = 'FACTORY_VACUUM_TUBE'
    'victorian-factory'            = 'FACTORY_VICTORIAN'

    # PARK
    'amusement-park-game-attractions-pack' = 'AMUSEMENT_PARK'
    'indigo-park-park-assets'              = 'INDIGO_PARK'
    'park-bench'                           = 'PARK_BENCH'
    'park-bin'                             = 'PARK_BIN'
    'recreational_place_play_children_seesaw' = 'PARK_SEESAW'
    'the-slide-park-play-child'            = 'PARK_SLIDE'

    # SHOPS
    'agraba_market_shop'            = 'MARKET_AGRABA'
    'arabian-market'                = 'MARKET_ARABIAN'
    'japanese-tea-shop'             = 'SHOP_JAPANESE_TEA'
    'medieval-market-asset-pack'    = 'MARKET_MEDIEVAL'
    'medieval-market-tent-prop-02'  = 'MARKET_TENT_MEDIEVAL'
    'old-japanese-store-lowpoly'    = 'SHOP_JAPANESE_OLD'
    'ramen-shop'                    = 'SHOP_RAMEN'
    'shop'                          = 'SHOP_GENERIC'
    'shop (1)'                      = 'SHOP_GENERIC_2'
    'small_coffee_shop'             = 'SHOP_COFFEE'
    'street_market'                 = 'MARKET_STREET'
    'village_market_stall'          = 'MARKET_STALL_VILLAGE'

    # Root environment / props
    'fishing-hut'                                  = 'FISHING_HUT'
    'low-poly-forest-tree-pack'                    = 'FOREST_TREE_PACK'
    'rock-mountain-with-cave-realistic-version'    = 'ROCK_MOUNTAIN_CAVE'
    'the-landscape-is-a-forest-in-the-mountains'   = 'FOREST_MOUNTAIN_LANDSCAPE'
    'the-landscape-is-a-forest-in-the-mountains (1)' = 'FOREST_MOUNTAIN_LANDSCAPE'
    'update-dirt-road-through-forest'              = 'DIRT_ROAD_FOREST'
}

function Get-PackName([string]$fileName, [string]$hintFolder) {
    $base = [System.IO.Path]::GetFileNameWithoutExtension($fileName)
    $key = $base.ToLowerInvariant()

    # Ambiguous "house" - beach folder vs buildings folder
    if ($key -eq 'house') {
        if ($hintFolder -match 'BEACH') { return 'BEACH_HOUSE' }
        if ($hintFolder -match 'BUILDING') { return 'HOUSE_1' }
        return 'HOUSE_1'
    }

    if ($RenameMap.ContainsKey($key)) { return $RenameMap[$key] }
    return (Sanitize-Upper $base)
}

function Get-Category([string]$packName, [string]$hintFolder, [string]$rawName) {
    $n = ($packName + ' ' + $rawName + ' ' + $hintFolder).ToLowerInvariant()

    if ($hintFolder -match 'BUILDING|FACTOR') { return 'Buildings' }
    if ($n -match 'factory|apartment|brutalist|skyscraper|nyc|future.city|night.city|wooden.house|old.house|house_1|building') {
        return 'Buildings'
    }
    if ($n -match 'forest.tree|tree.pack') { return 'Plants' }
    if ($n -match 'dirt.road|mountain|landscape|forest.mountain|rock.mountain|cave') { return 'Environment' }
    if ($n -match 'beach|park|market|shop|stall|fishing|amusement|indigo|seesaw|slide|bench|bin|tent|garden.table') {
        return 'Props'
    }
    return 'Props'
}

function Extract-Archive([string]$archive, [string]$outDir) {
    Ensure-Dir $outDir
    & $SevenZip x -y "-o$outDir" -- $archive 2>&1 | Out-Null
    return ($LASTEXITCODE -eq 0)
}

function Flatten-SingleRoot([string]$dir) {
    $children = @(Get-ChildItem -LiteralPath $dir -Force -ErrorAction SilentlyContinue |
        Where-Object { $_.Name -notin @('.DS_Store', 'Thumbs.db') })
    if ($children.Count -eq 1 -and $children[0].PSIsContainer) {
        $inner = $children[0]
        Get-ChildItem -LiteralPath $inner.FullName -Force | ForEach-Object {
            $target = Join-Path $dir $_.Name
            if (-not (Test-Path -LiteralPath $target)) {
                Move-Item -LiteralPath $_.FullName -Destination $dir -Force
            }
        }
        if (-not (Get-ChildItem -LiteralPath $inner.FullName -Force -ErrorAction SilentlyContinue)) {
            Remove-Item -LiteralPath $inner.FullName -Force -Recurse -ErrorAction SilentlyContinue
        }
    }
}

function Organize-ExtractLayout([string]$packDir) {
    # Prefer source/ + textures/ layout when loose models/textures sit at pack root
    $models = @(Get-ChildItem -LiteralPath $packDir -File -ErrorAction SilentlyContinue |
        Where-Object { $_.Extension -match '\.(fbx|glb|gltf|obj)$' })
    $texs = @(Get-ChildItem -LiteralPath $packDir -File -ErrorAction SilentlyContinue |
        Where-Object { $_.Extension -match '\.(png|jpg|jpeg|tga|tif|tiff|webp|dds|exr)$' })

    if ($models.Count -gt 0) {
        $srcDir = Join-Path $packDir 'source'
        Ensure-Dir $srcDir
        foreach ($m in $models) {
            $dest = Join-Path $srcDir $m.Name
            if (-not (Test-Path -LiteralPath $dest)) {
                Move-Item -LiteralPath $m.FullName -Destination $dest -Force
            }
        }
    }
    if ($texs.Count -gt 0) {
        $texDir = Join-Path $packDir 'textures'
        Ensure-Dir $texDir
        foreach ($t in $texs) {
            $dest = Join-Path $texDir $t.Name
            if (-not (Test-Path -LiteralPath $dest)) {
                Move-Item -LiteralPath $t.FullName -Destination $dest -Force
            }
        }
    }
}

function Pack-HasContent([string]$dir) {
    if (-not (Test-Path -LiteralPath $dir)) { return $false }
    $hit = Get-ChildItem -LiteralPath $dir -Recurse -File -ErrorAction SilentlyContinue |
        Where-Object { $_.Extension -match '\.(fbx|glb|gltf|obj|png|jpg|jpeg|tga|psd|mat|prefab)$' } |
        Select-Object -First 1
    return [bool]$hit
}

function Import-Archive([System.IO.FileInfo]$arch, [string]$hintFolder) {
    $pack = Get-PackName $arch.Name $hintFolder
    $cat = Get-Category $pack $hintFolder $arch.Name
    $outDir = Join-Path (Join-Path $DestRoot $cat) $pack

    if (Pack-HasContent $outDir) {
        Write-Log "SKIP (exists): $cat/$pack"
        return 'skip'
    }

    Write-Log ("EXTRACT -> {0}/{1}  ({2} MB)  from {3}" -f $cat, $pack, [math]::Round($arch.Length/1MB,1), $arch.Name)
    Ensure-Dir $outDir
    if (Extract-Archive $arch.FullName $outDir) {
        Flatten-SingleRoot $outDir
        Organize-ExtractLayout $outDir
        return 'ok'
    }
    Write-Log "FAIL extract: $($arch.FullName)"
    return 'fail'
}

function Import-LooseModel([System.IO.FileInfo]$file, [string]$hintFolder) {
    $pack = Get-PackName $file.Name $hintFolder
    # Disambiguate shop.glb vs shop (1).glb
    if ($file.BaseName -match '^\s*shop\s*$') { $pack = 'SHOP_GENERIC' }
    if ($file.BaseName -match 'shop\s*\(\s*1\s*\)') { $pack = 'SHOP_GENERIC_2' }

    $cat = Get-Category $pack $hintFolder $file.Name
    $outDir = Join-Path (Join-Path $DestRoot $cat) $pack
    Ensure-Dir $outDir

    $srcDir = Join-Path $outDir 'source'
    Ensure-Dir $srcDir
    $destFile = Join-Path $srcDir $file.Name

    if (Test-Path -LiteralPath $destFile) {
        Write-Log "SKIP loose: $cat/$pack/$($file.Name)"
        return 'skip'
    }

    Copy-Item -LiteralPath $file.FullName -Destination $destFile -Force
    Write-Log "COPY -> $cat/$pack/source/$($file.Name)"
    return 'ok'
}

# --- start ---
"=== Game_Models batch2 import $(Get-Date -Format o) ===" | Set-Content -Path $Log
Write-Log "Source: $Src"
Write-Log "Dest:   $DestRoot"

if (-not (Test-Path -LiteralPath $Src)) { throw "Missing source folder: $Src" }
if (-not (Test-Path -LiteralPath $SevenZip)) { throw "7-Zip not found at $SevenZip" }

$categories = @('Vehicles','Animals','Birds','Plants','Props','Environment','UI','Buildings')
foreach ($c in $categories) { Ensure-Dir (Join-Path $DestRoot $c) }

# Rename already-imported dirt road pack for consistency
$oldDirt = Join-Path $DestRoot 'Environment\update-dirt-road-through-forest'
$newDirt = Join-Path $DestRoot 'Environment\DIRT_ROAD_FOREST'
if ((Test-Path -LiteralPath $oldDirt) -and -not (Test-Path -LiteralPath $newDirt)) {
    Rename-Item -LiteralPath $oldDirt -NewName 'DIRT_ROAD_FOREST'
    Write-Log "RENAME Environment/update-dirt-road-through-forest -> DIRT_ROAD_FOREST"
}

$ok = 0; $fail = 0; $skip = 0
$zipsToDelete = New-Object System.Collections.Generic.List[string]

# 1) Root-level archives in Downloads\ASSETS
$rootArchives = @(Get-ChildItem -LiteralPath $Src -File |
    Where-Object { $_.Extension -match '\.(zip|rar|7z)$' })

# Prefer non-duplicate for landscape (skip " (1)" if base exists)
$seenPacks = @{}
foreach ($arch in ($rootArchives | Sort-Object Name)) {
    $pack = Get-PackName $arch.Name 'ROOT'
    if ($seenPacks.ContainsKey($pack)) {
        Write-Log "SKIP duplicate archive: $($arch.Name) -> $pack"
        $skip++
        if ($DeleteZips) { $zipsToDelete.Add($arch.FullName) }
        continue
    }
    $seenPacks[$pack] = $true
    $result = Import-Archive $arch 'ROOT'
    switch ($result) {
        'ok'   { $ok++; if ($DeleteZips) { $zipsToDelete.Add($arch.FullName) } }
        'skip' { $skip++; if ($DeleteZips) { $zipsToDelete.Add($arch.FullName) } }
        'fail' { $fail++ }
    }
}

# 2) Category folders (BEACH, BUILDINGS, FACTORIES, PARK, SHOP AND STALLS)
$hintFolders = @(Get-ChildItem -LiteralPath $Src -Directory)
foreach ($folder in $hintFolders) {
    Write-Log "--- Folder: $($folder.Name) ---"
    $hint = $folder.Name

    # Archives inside folder
    $archives = @(Get-ChildItem -LiteralPath $folder.FullName -File |
        Where-Object { $_.Extension -match '\.(zip|rar|7z)$' })
    $localSeen = @{}
    foreach ($arch in ($archives | Sort-Object Name)) {
        $pack = Get-PackName $arch.Name $hint
        if ($localSeen.ContainsKey($pack)) {
            Write-Log "SKIP duplicate: $($arch.Name) -> $pack"
            $skip++
            if ($DeleteZips) { $zipsToDelete.Add($arch.FullName) }
            continue
        }
        $localSeen[$pack] = $true
        $result = Import-Archive $arch $hint
        switch ($result) {
            'ok'   { $ok++; if ($DeleteZips) { $zipsToDelete.Add($arch.FullName) } }
            'skip' { $skip++; if ($DeleteZips) { $zipsToDelete.Add($arch.FullName) } }
            'fail' { $fail++ }
        }
    }

    # Loose models inside folder
    $loose = @(Get-ChildItem -LiteralPath $folder.FullName -File |
        Where-Object { $_.Extension -match '\.(glb|fbx|gltf)$' })
    foreach ($f in $loose) {
        $result = Import-LooseModel $f $hint
        switch ($result) {
            'ok'   { $ok++ }
            'skip' { $skip++ }
            'fail' { $fail++ }
        }
    }
}

# 3) Nested archives left inside newly imported packs (one pass)
$nested = @(Get-ChildItem -LiteralPath $DestRoot -Recurse -File -ErrorAction SilentlyContinue |
    Where-Object {
        $_.Extension -match '\.(zip|rar|7z)$' -and
        $_.FullName -match '\\(Vehicles|Animals|Birds|Plants|Props|Environment|UI|Buildings)\\'
    })
foreach ($n in $nested) {
    $target = Join-Path $n.DirectoryName ($n.BaseName + '_extracted')
    if (Test-Path -LiteralPath $target) {
        if ($DeleteZips) { $zipsToDelete.Add($n.FullName) }
        continue
    }
    Write-Log "NESTED EXTRACT -> $($n.FullName.Substring($DestRoot.Length+1))"
    if (Extract-Archive $n.FullName $target) {
        Flatten-SingleRoot $target
        Organize-ExtractLayout $target
        $ok++
        if ($DeleteZips) { $zipsToDelete.Add($n.FullName) }
    } else {
        $fail++
    }
}

# 4) Delete processed zips
if ($DeleteZips) {
    $uniqueZips = $zipsToDelete | Select-Object -Unique
    foreach ($z in $uniqueZips) {
        if (Test-Path -LiteralPath $z) {
            try {
                Remove-Item -LiteralPath $z -Force
                Write-Log "DELETED zip: $z"
            } catch {
                Write-Log "WARN could not delete: $z - $($_.Exception.Message)"
            }
        }
    }
}

# 5) Summary
Write-Log "DONE  ok=$ok  skip=$skip  fail=$fail"
Write-Log "Categories under Game_Models:"
foreach ($c in $categories) {
    $p = Join-Path $DestRoot $c
    $count = @(Get-ChildItem -LiteralPath $p -Directory -ErrorAction SilentlyContinue).Count
    Write-Log ("  {0,-12} {1} packs" -f $c, $count)
}

Write-Host ""
Write-Host "Log: $Log"

# My Fantasy World — Starter Pack (Delete Old Project Safely)

This folder is your **portable kit**.  
After you copy what you need into a **new Unity URP project**, you can delete `D:\Unity\My_World_Game`.

**Location:** `Downloads\MyFantasyWorld_StarterPack\`

---

## Folder map

```
MyFantasyWorld_StarterPack/
  README.md                          ← you are here
  Docs/
    FROM_SCRATCH_WORLD_BUILDING_GUIDE.md
    FROM_SCRATCH_WORLD_BUILDING_GUIDE.pdf
    HOW_TO_USE_SCRIPTS.md
    ASSETS_INCLUDED_AND_DOWNLOAD.md
  Scripts/                           ← professional C# (import these)
    Core/
    Player/
    Interaction/
    Buildings/
    Vehicles/
    World/
  AssetsToImport/                    ← art/tools copied from old project
  HowToImport/
    IMPORT_ORDER.txt
```

---

## 1) Create a new Unity project first

1. Install **Unity Hub** + **Unity 6**
2. **New project → Universal 3D (URP)**  
   Name example: `MyFantasyWorld`
3. Open the empty project and wait until it finishes importing

**Start with this (section-wise: build + paint + assets + scripts):**  
`Docs/SECTION_BY_SECTION_BUILD_BIBLE.md` (and `.pdf`)

**Also read:**
- `Docs/HOW_TO_USE_SCRIPTS.md` (and `.pdf`) — deeper player/house/car/bike/plane/boat setup
- `Docs/ASSETS_INCLUDED_AND_DOWNLOAD.md` (and `.pdf`) — what is copied + what to download
- `Docs/FROM_SCRATCH_WORLD_BUILDING_GUIDE.md` — full beginner theory from Unity install
- `DELETE_OLD_PROJECT_CHECKLIST.txt` — only delete old project after this passes

---

## 2) Import scripts

1. In your new project, create folder: `Assets/MyWorld/Scripts`
2. Copy everything from this pack’s `Scripts/` into that folder  
   (keep subfolders: Core, Player, Interaction, Buildings, Vehicles, World)
3. Wait for Unity compile (bottom-right spinner)
4. If errors mention `linearVelocity`, you are on an older Unity — use Unity 6

Detailed script setup: **`Docs/HOW_TO_USE_SCRIPTS.md`**

---

## 3) Import art assets

1. Copy folders from `AssetsToImport/` into your new project `Assets/`  
   (you can copy one pack at a time to avoid long freezes)
2. Follow `HowToImport/IMPORT_ORDER.txt`
3. Fix pink materials: select material → Shader → **Universal Render Pipeline/Lit**
4. For Forst trees, open and import `Conifers_URP.unitypackage` inside that pack

What is included + what else to download: **`Docs/ASSETS_INCLUDED_AND_DOWNLOAD.md`**

---

## 4) Recommended first hour in the new project

1. Create Terrain  
2. Add player capsule + `PlayerMotor` + `PlayerInteraction` + `SimpleFollowCamera`  
3. Tag player **Player**  
4. Place one house + `HouseDoor`  
5. Place one car prefab + WheelColliders + `CarController` + `VehicleEnterExit`  
6. Press Play and test walk / enter door / drive  

---

## 5) Scripts included (quick list)

| Script | Purpose |
|---|---|
| `PlayerMotor` | Walk / run / jump |
| `PlayerInteraction` | Press E to use doors/vehicles |
| `PlayerSpawnPoint` + `PlayerBootstrap` | Spawn player |
| `HouseDoor` | Enter/exit house or swing door |
| `BuildingInteriorVolume` | Detect inside house |
| `CarController` | Drive cars (WheelColliders) |
| `BikeController` | Drive bikes |
| `PlaneController` | Fly planes |
| `BoatController` | Drive boats |
| `VehicleEnterExit` + `VehicleSeat` | Enter/exit any vehicle |
| `ZoneTrigger` | Zone enter detection |
| `DayNightCycle` | Sun day/night |
| `AmbientZoneAudio` | Zone sound beds |
| `SimpleFollowCamera` | Temporary camera |

---

## 6) After everything is copied

You may delete the old project folder.

Keep this Downloads pack as backup until your new project is saved and working.

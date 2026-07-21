<div align="center">

# MY UNITY WORLD

### Fantasy Open World — Unity 6 · URP

**One project. One master guide. Build → Walk → Drive → Expand the world.**

[![Unity](https://img.shields.io/badge/Unity-6000.5-black?style=for-the-badge&logo=unity)](https://unity.com/)
[![URP](https://img.shields.io/badge/Pipeline-URP-2266AA?style=for-the-badge)](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest)

<br/>

### → [Open the Master Guide](Assets/MyWorld/Docs/MASTER_GUIDE.md) ←

*Player · First car · Tuning · Sky · Weather · Fog · World sections*

</div>

---

## Contents — what each part is for

Click a link to jump. Read only what you need.

| | Section | What it does (simple English) |
|---|---------|-------------------------------|
| **★** | **[MASTER GUIDE (full)](Assets/MyWorld/Docs/MASTER_GUIDE.md)** | **Everything in one file** — start here |
| **★** | **[Localhost Docs Site](DocsSite/README.md)** | **Beautiful web UI** — run `DocsSite/start.bat` → http://127.0.0.1:5050 |
| | [1 — Before you start](Assets/MyWorld/Docs/MASTER_GUIDE.md#1-before-you-start) | Scene path and tidy Hierarchy |
| | [2 — Player (walk)](Assets/MyWorld/Docs/MASTER_GUIDE.md#2-part-a--player-walk-the-world) | Character + camera so you can walk |
| | [3 — First car](Assets/MyWorld/Docs/MASTER_GUIDE.md#3-part-b--first-car-full-setup-once) | Build **one** working car (do once) |
| | [4 — Second car (fast)](Assets/MyWorld/Docs/MASTER_GUIDE.md#4-second-car--and-every-car-after--fastest-way) | Duplicate prefab / swap mesh only |
| | [5 — All vehicles + bike setup](Assets/MyWorld/Docs/MASTER_GUIDE.md#5-part-c--all-vehicle-types-cars-bikes-boats) | Cars, **first bike steps**, boats |
| | [6 — Tune mass / slip / bounce](Assets/MyWorld/Docs/MASTER_GUIDE.md#6-tune-vehicles--mass-grip-bounce-flips) | Mountain, Ferrari, sports/trail bikes |
| | [7 — Terrain](Assets/MyWorld/Docs/MASTER_GUIDE.md#7-world-1--terrain--ground-paint) | Sculpt and paint ground |
| | [8 — Beach & water](Assets/MyWorld/Docs/MASTER_GUIDE.md#8-world-2--beach--water) | Shore, water, pier |
| | [9 — Forest](Assets/MyWorld/Docs/MASTER_GUIDE.md#9-world-3--forest--trees) | Trees and rocks |
| | [10 — Village](Assets/MyWorld/Docs/MASTER_GUIDE.md#10-world-4--village--farm) | Houses and farm |
| | [11 — Roads & bridges](Assets/MyWorld/Docs/MASTER_GUIDE.md#11-world-5--roads--bridges) | EasyRoads3D click-terrain roads, bridges |
| | [12 — Sky, weather, fog](Assets/MyWorld/Docs/MASTER_GUIDE.md#12-world-6--sky-daynight-weather-fog--polish) | AllSkyFree, day/night, rain, forest fog |
| | [13 — Asset folders](Assets/MyWorld/Docs/MASTER_GUIDE.md#13-asset-locations-cheat-sheet) | Houses, trees, cars, boats — where each folder is |
| | [15 — Troubleshooting](Assets/MyWorld/Docs/MASTER_GUIDE.md#15-troubleshooting) | Common fixes |

### Other docs (archive — optional)

| Guide | Use it for |
|--------|------------|
| [Start Here](Assets/MyWorld/Docs/START_HERE.md) | Short production rules (archive) |
| [Section Build Bible](Assets/MyWorld/Docs/SECTION_BY_SECTION_BUILD_BIBLE.md) | Older long bible (archive) |
| [From Scratch Guide](Assets/MyWorld/Docs/FROM_SCRATCH_WORLD_BUILDING_GUIDE.md) | Unity install theory (archive) |
| [How to Use Scripts](Assets/MyWorld/Docs/HOW_TO_USE_SCRIPTS.md) | Script-only reference (archive) |
| [Vehicles & Player](Assets/MyWorld/Docs/VEHICLES_AND_PLAYER.md) | Merged into Master (archive) |
| [Assets & Downloads](Assets/MyWorld/Docs/ASSETS_INCLUDED_AND_DOWNLOAD.md) | Download list (archive) |
| [Starter Pack Notes](Assets/MyWorld/Docs/STARTER_PACK_README.md) | Legacy notes (archive) |

---

## Quick start

1. Open this repo in **Unity Hub** (Unity 6 / URP)
2. Open scene `Assets/Scenes/MY WORLD`
3. Open **[`MASTER_GUIDE.md`](Assets/MyWorld/Docs/MASTER_GUIDE.md)**  
   **or** run the docs website: `DocsSite/start.bat` → http://127.0.0.1:5050
4. Do **Player** → **First car** → save as prefab
5. Next cars: **Duplicate → swap mesh** (Master Guide §4)
6. If the car feels bad: **Tune** (Master Guide §6 — mass, slip, bounce)
7. Then build world sections 7–12

### Localhost documentation (recommended)

```bat
cd DocsSite
start.bat
```

Opens a section-based web UI with Master Guide + all archive docs + search.

### Cars in short

| Step | Do this |
|------|---------|
| Car #1 | Full setup once ([§3](Assets/MyWorld/Docs/MASTER_GUIDE.md#3-part-b--first-car-full-setup-once)) or menu **MyWorld → Vehicles → Create Car Rig From Selection** |
| Car #2+ | Duplicate / swap mesh only ([§4](Assets/MyWorld/Docs/MASTER_GUIDE.md#4-second-car--and-every-car-after--fastest-way)) |
| Feels wrong | Change Mass / Spring / Damper / Friction ([§6](Assets/MyWorld/Docs/MASTER_GUIDE.md#6-tune-vehicles--mass-grip-bounce-flips)) |

---

## Project layout

```text
Assets/
├── MyWorld/
│   ├── Docs/          ← MASTER_GUIDE.md (main)
│   ├── Scripts/       ← Player, Vehicles, Buildings, World
│   │   └── Editor/    ← Car rig menu (MyWorld → Vehicles)
│   └── Prefabs/       ← save your finished cars/houses here
├── Scenes/MY WORLD
├── StarterAssets/     ← player character
├── Game_Models/       ← cars & bikes
├── 3DForge_.../       ← village
├── Forst/ ALP_.../    ← trees
└── ... more art packs
```

---

## Repo notes

- `Library/` is not in git (Unity rebuilds it)
- One oversized file may be gitignored (`Rock 9.fbx`)
- Check Asset Store licenses before shipping commercially

---

<div align="center">

**One guide. First car once. Every next car = duplicate. Tune in §6.**

[Master Guide](Assets/MyWorld/Docs/MASTER_GUIDE.md)

<sub>MY UNITY WORLD · Unity 6 · URP</sub>

</div>

<div align="center">

# MY UNITY WORLD

### A Fantasy Open-World Project — Unity 6 · URP

**Build land. Build villages. Drive. Explore.**  
Professional world-building, beginner-friendly guides.

[![Unity](https://img.shields.io/badge/Unity-6000.5-black?style=for-the-badge&logo=unity)](https://unity.com/)
[![URP](https://img.shields.io/badge/Pipeline-URP-2266AA?style=for-the-badge)](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest)
[![License](https://img.shields.io/badge/Project-Personal-2ea44f?style=for-the-badge)](#)

<br/>

[Open in Unity](#-quick-start) · [Contents](#-contents) · [Guides](#-documentation) · [Scripts](#-systems--scripts) · [Assets](#-what-you-already-have)

</div>

---

## Contents

| # | Guide | What it’s for |
|---|--------|----------------|
| 01 | [Start Here](Assets/MyWorld/Docs/START_HERE.md) | Clean foundation & production rules |
| 02 | [Section-by-Section Build Bible](Assets/MyWorld/Docs/SECTION_BY_SECTION_BUILD_BIBLE.md) | **Main handbook** — build + paint + assets + scripts per zone |
| 03 | [From Scratch World Building](Assets/MyWorld/Docs/FROM_SCRATCH_WORLD_BUILDING_GUIDE.md) | Unity install → terrain → living world (full theory) |
| 04 | [How to Use Scripts](Assets/MyWorld/Docs/HOW_TO_USE_SCRIPTS.md) | Player, house, car, bike, plane, boat — click steps |
| 05 | [Vehicles & Player](Assets/MyWorld/Docs/VEHICLES_AND_PLAYER.md) | StarterAssets camera setup + vehicle inventory + suspension |
| 06 | [Assets Included & Downloads](Assets/MyWorld/Docs/ASSETS_INCLUDED_AND_DOWNLOAD.md) | What’s in the project vs what to download later |
| 07 | [Starter Pack Notes](Assets/MyWorld/Docs/STARTER_PACK_README.md) | Portable pack overview (legacy notes) |

**PDFs** (same Docs folder): `SECTION_BY_SECTION_BUILD_BIBLE.pdf`, `HOW_TO_USE_SCRIPTS.pdf`, `FROM_SCRATCH_WORLD_BUILDING_GUIDE.pdf`, and others.

---

## Documentation

### Start with these three

```text
1. START_HERE.md
2. SECTION_BY_SECTION_BUILD_BIBLE.md   ← daily building
3. VEHICLES_AND_PLAYER.md              ← player camera + cars
```

<details>
<summary><b>01 — Start Here</b></summary>

<br/>

**File:** [`Assets/MyWorld/Docs/START_HERE.md`](Assets/MyWorld/Docs/START_HERE.md)

Clean production foundation. What was removed from prototypes, folder layout, and rules so you never wipe a scene by accident.

</details>

<details>
<summary><b>02 — Section-by-Section Build Bible</b> ★ recommended</summary>

<br/>

**File:** [`Assets/MyWorld/Docs/SECTION_BY_SECTION_BUILD_BIBLE.md`](Assets/MyWorld/Docs/SECTION_BY_SECTION_BUILD_BIBLE.md)

Every zone in order: terrain, paint textures, forest trees, beach, village, roads, bridges, player, cars, bikes, boats, day/night, and more.

Each section includes:

- Click steps  
- Ground painting  
- Assets to use  
- Scripts (if needed)  
- Done checklist  

</details>

<details>
<summary><b>03 — From Scratch World Building Guide</b></summary>

<br/>

**File:** [`Assets/MyWorld/Docs/FROM_SCRATCH_WORLD_BUILDING_GUIDE.md`](Assets/MyWorld/Docs/FROM_SCRATCH_WORLD_BUILDING_GUIDE.md)

Long-form beginner → professional guide: install Unity, create a **Universal 3D (URP)** project, sculpt mountains, paint forests, water, villages, weather, wildlife, audio, and a living-world checklist.

</details>

<details>
<summary><b>04 — How to Use Scripts</b></summary>

<br/>

**File:** [`Assets/MyWorld/Docs/HOW_TO_USE_SCRIPTS.md`](Assets/MyWorld/Docs/HOW_TO_USE_SCRIPTS.md)

Step-by-step setup for:

| System | Scripts |
|--------|---------|
| Player | `PlayerMotor`, `PlayerInteraction`, StarterAssets + `CameraFollowTarget` |
| House | `HouseDoor`, `BuildingInteriorVolume` |
| Car | `CarController`, `VehicleSeat`, `VehicleEnterExit` |
| Bike | `BikeController` |
| Plane | `PlaneController` |
| Boat | `BoatController` |
| World | `ZoneTrigger`, `DayNightCycle`, `AmbientZoneAudio` |

</details>

<details>
<summary><b>05 — Vehicles & Player</b></summary>

<br/>

**File:** [`Assets/MyWorld/Docs/VEHICLES_AND_PLAYER.md`](Assets/MyWorld/Docs/VEHICLES_AND_PLAYER.md)

The setup you’re using now:

- `PlayerArmature` + `PlayerCameraRoot` + `CameraFollowTarget`  
- Third-person offset `(0, 0.5, -4)`  
- Full vehicle list (Silverado, Ferrari, Mercedes, UAZ, Jeep, Lada, bikes, boats)  
- WheelCollider suspension tuning  

</details>

<details>
<summary><b>06 — Assets Included & Downloads</b></summary>

<br/>

**File:** [`Assets/MyWorld/Docs/ASSETS_INCLUDED_AND_DOWNLOAD.md`](Assets/MyWorld/Docs/ASSETS_INCLUDED_AND_DOWNLOAD.md)

Maps every biome need → folders already in `Assets/` → optional Asset Store downloads (palms, castle, rain VFX, audio, plane mesh, etc.).

</details>

<details>
<summary><b>07 — Starter Pack README</b></summary>

<br/>

**File:** [`Assets/MyWorld/Docs/STARTER_PACK_README.md`](Assets/MyWorld/Docs/STARTER_PACK_README.md)

Notes from the portable starter pack era. Keep for reference; prefer the Build Bible for day-to-day work.

</details>

---

## Quick start

| Step | Action |
|------|--------|
| 1 | Open folder in **Unity Hub** → Unity **6000.5** (or your installed Unity 6) |
| 2 | Wait for first import (`Library` rebuilds if cloned fresh) |
| 3 | Open scene `Assets/Scenes/MY WORLD` |
| 4 | Read [`SECTION_BY_SECTION_BUILD_BIBLE.md`](Assets/MyWorld/Docs/SECTION_BY_SECTION_BUILD_BIBLE.md) |
| 5 | Play — walk with StarterAssets player |

```text
Project path (example)
D:\Github Clones\MY_UNITY_WORLD
```

> **Important:** Always use a **Universal 3D (URP)** project. HDRP packs will look pink.

---

## Systems & scripts

```text
Assets/MyWorld/Scripts/
├── Core/           GameLayers, GameTags
├── Player/         Motor, Interaction, Spawn
├── Interaction/    IInteractable
├── Buildings/      HouseDoor, InteriorVolume
├── Vehicles/       Car, Bike, Plane, Boat, Enter/Exit
└── World/          Zones, DayNight, Audio, CameraFollowTarget
```

| Feature | Status |
|---------|--------|
| Third-person player | Ready (StarterAssets + follow camera) |
| Terrain paint / trees | Ready (your scene) |
| Cars / bikes (WheelCollider + suspension) | Scripts ready — wire per model |
| Boats | Scripts + Alstra prefabs |
| House doors | Scripts ready |
| Day / night | Script ready |
| Plane | Script ready — need airplane mesh |

---

## What you already have

| Category | Location |
|----------|----------|
| Fantasy village | `Assets/3DForge_FantasyExteriors/` |
| Trees (URP pines, oak, maple) | `Forst/`, `ALP_Assets/`, `Game_RocksAndVegetation/` |
| Ground textures / layers | `Game_Textures/`, `Game_Terrains/`, root `*.terrainlayer` |
| Bridges | `3DWorldInSeconds_Bridges/Prefabs/Models/` |
| Roads | `EasyRoads3D/`, `KajamansRoads/` |
| Water / boats | `AQUAS-Lite/`, `Alstra_Boats/`, `Game_Materials/` |
| Skies | `AllSkyFree/` |
| Player | `StarterAssets/` |
| Vehicles (meshes) | `Game_Models/` |
| Your docs & code | `Assets/MyWorld/` |

---

## Build order (short)

```text
Terrain → Paint ground → Water/beach → Player walk
    → Path → Village → Forest trees → Bridge
        → Farm → Vehicles → Day/night & audio
```

---

## Repo notes

- **`Library/` is not in git** — Unity recreates it on open  
- One large file may be gitignored (`Rock 9.fbx` > 100 MB) — keep a local copy if needed  
- Check Asset Store licenses before commercial shipping  

---

## Contributing to your own world

1. Build one zone at a time  
2. Save reusable props under `Assets/MyWorld/Prefabs/`  
3. Prefer **Prefabs** folders from packs over raw Meshes  
4. Fix pink materials → Shader **URP / Lit**  

---

<div align="center">

**Made for building a fantasy world from scratch — carefully, zone by zone.**

[Start Here](Assets/MyWorld/Docs/START_HERE.md) ·
[Build Bible](Assets/MyWorld/Docs/SECTION_BY_SECTION_BUILD_BIBLE.md) ·
[Scripts](Assets/MyWorld/Docs/HOW_TO_USE_SCRIPTS.md) ·
[Vehicles](Assets/MyWorld/Docs/VEHICLES_AND_PLAYER.md)

<br/>

<sub>MY UNITY WORLD · Unity 6 · URP</sub>

</div>

# Start here — clean production foundation

Prototype systems were removed so you can build the world properly from scratch.

**Starting completely from scratch (Unity install → first world):**  
[`FROM_SCRATCH_WORLD_BUILDING_GUIDE.md`](FROM_SCRATCH_WORLD_BUILDING_GUIDE.md)  
PDF copy: `Downloads\FROM_SCRATCH_WORLD_BUILDING_GUIDE.pdf`

**Older project-specific notes:** [`WORLD_BUILDING_GUIDE.md`](WORLD_BUILDING_GUIDE.md)

## What was removed (do not rebuild the same way)

- **Build My World** scene-wipe baker  
- Hacky driveable car / bike Setup menus  
- Fake sit / door / OnGUI prompts  
- Toy fish / float / breakable / sky switcher / minimap systems  
- Static `GlobalWaterConfig`  
- Old prototype roadmap docs  

## What you still have (use these)

| Keep | Use for |
|---|---|
| `Assets/StarterAssets/` | Temporary player + camera |
| Asset Store art under `Assets/` (3DForge, trees, bridges, roads, etc.) | Drag into **your** scenes as props |
| `Assets/Game/Models/`, `Textures/`, `Terrains/`, `Materials/` | Your content library |
| URP project settings | Rendering |

## Recommended folder layout

```
Assets/Game/
  Scenes/          ← MainWorld.unity (new empty scene)
  Prefabs/         ← finished buildings / props
  Art/
  Audio/
  UI/
  Zones/           ← optional zone notes / roots
  Scripts/
    Core/
    Player/
    Vehicles/
    World/
    Editor/
  Docs/            ← START_HERE + WORLD_BUILDING_GUIDE
```

## How to start building (clean)

1. **Window → My World → Create Empty MainWorld Scene**
2. Open **`WORLD_BUILDING_GUIDE.md`** → follow Section 1, then Week 1 in Section 18
3. Add Terrain → paint → place props by hand
4. Drop Starter Assets player + camera
5. Build **one zone at a time** (Beach → Village → Forest…)

Optional: **Window → My World → Create Clean World Folders**

## Production rules

1. Never use a tool that deletes your whole Hierarchy without backup  
2. Vehicles later — real system, not the deleted wheel hacks  
3. UI with UGUI / UI Toolkit only  
4. Check Asset Store licenses before shipping  
5. One art look per zone — don’t mix every kit in one shot  

## Old scenes

`01_World.unity` / `01_Beach.unity` may show **Missing Script**. Prefer a **new** `MainWorld` scene.

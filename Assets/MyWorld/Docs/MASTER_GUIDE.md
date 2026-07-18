# MY UNITY WORLD — Master Guide (ONE file)

**Unity 6 · URP · Project:** open `Assets/Scenes/MY WORLD`

This is the **only guide you need**.  
Other MDs in `Assets/MyWorld/Docs/` are extras / archive. Use **this file**.

---

## Contents (what each section is for)

Use this table to jump straight to what you need. Click a link, do only that section.

| # | Section | What it does (simple English) |
|---|---------|-------------------------------|
| 1 | [Before you start](#1-before-you-start) | Project path, scene name, how to keep Hierarchy tidy |
| 2 | [Player (walk)](#2-part-a--player-walk-the-world) | Set up character + camera so you can walk the map |
| 3 | [First car (full setup)](#3-part-b--first-car-full-setup-once) | Build **one** working car from scratch (do this once) |
| 4 | [Second car (fastest way)](#4-second-car--and-every-car-after--fastest-way) | After car #1 works: duplicate / menu / swap mesh only |
| 5 | [All vehicle types](#5-part-c--all-vehicle-types-cars-bikes-boats) | List of cars, bikes, boats and which script each uses |
| 6 | [Tune vehicles (mass, slip, bounce)](#6-tune-vehicles--mass-grip-bounce-flips) | Change mass; fix slippery, bouncing, flipping cars |
| 7 | [Terrain & ground paint](#7-world-1--terrain--ground-paint) | Sculpt land and paint sand / grass / dirt / rock |
| 8 | [Beach & water](#8-world-2--beach--water) | Shoreline, water plane, pier, boats at the coast |
| 9 | [Forest & trees](#9-world-3--forest--trees) | Paint trees, fix pink materials, place rocks |
| 10 | [Village & farm](#10-world-4--village--farm) | Houses, fences, farm props, optional doors |
| 11 | [Roads & bridges](#11-world-5--roads--bridges) | Paths between zones and place bridges |
| 12 | [Sky & polish](#12-world-6--sky-daynight-polish) | Skybox, lighting, day/night, final clean-up |
| 13 | [Asset paths](#13-asset-locations-cheat-sheet) | “Where is the X folder?” quick lookup |
| 14 | [Scripts cheat sheet](#14-scripts-cheat-sheet) | Which component does what |
| 15 | [Troubleshooting](#15-troubleshooting) | Common problems and quick fixes |

**Recommended order:** §2 Player → §3 First car → Play → §4 more cars as needed → World §7–§12 → use §6 whenever a vehicle feels wrong.

---

# 1) Before you start

| Item | Value |
|------|--------|
| Pipeline | **URP** (Universal 3D) |
| Scene | `Assets/Scenes/MY WORLD` |
| Scripts | `Assets/MyWorld/Scripts/` |
| Your prefabs | `Assets/MyWorld/Prefabs/Vehicles/` |
| Editor menus | Top bar → **MyWorld → Vehicles → …** |

**Hierarchy habit (keep forever):**

```text
MY WORLD
├── MAIN_SECTIONS / Terrain / Zones...
├── PLAYER
│   ├── PlayerArmature
│   └── MainCamera
└── VEHICLES
    ├── Car_UAZ          ← first finished car (prefab this)
    ├── Car_Mercedes     ← later cars = copies
    └── ...
```

---

# 2) PART A — Player (walk the world)

**Goal:** walk around before anything else.

### Assets

| What | Path |
|------|------|
| Character | `Assets/StarterAssets/ThirdPersonController/Prefabs/PlayerArmature` |
| Camera | `Assets/StarterAssets/ThirdPersonController/Prefabs/MainCamera` |
| Follow script | `Assets/MyWorld/Scripts/World/CameraFollowTarget.cs` |
| Interact script | `Assets/MyWorld/Scripts/Player/PlayerInteraction.cs` |

### Step-by-step

1. Open scene `MY WORLD`
2. Create empty `PLAYER` (if missing)
3. Drag **PlayerArmature** under `PLAYER`
4. Drag **MainCamera** under `PLAYER`
5. Delete **PlayerFollowCamera** if it exists (do not use Cinemachine here)
6. Select MainCamera → remove any yellow **Missing Script**
7. **Add Component** → `Camera Follow Target`
8. Expand PlayerArmature → find child **`PlayerCameraRoot`**
9. Drag **PlayerCameraRoot** into Camera Follow Target → **Target**
10. Set **Offset** = `X:0  Y:0.5  Z:-4`  
    (`Z=-4` = third person behind. `Z=0` looks first-person)
11. Select PlayerArmature → Tag = **Player**
12. **Add Component** → `Player Interaction` (needed for **E** on cars/doors)
13. Place player on flat ground slightly above terrain
14. **Ctrl+S** → **Play**

### Controls

| Key | Action |
|-----|--------|
| WASD | Move |
| Mouse | Look |
| Space | Jump |
| Left Shift | Sprint |
| E | Interact (cars/doors) |

### Done when
You can walk beach ↔ trees without falling through ground.

---

# 3) PART B — First car (full setup once)

**Goal:** make **one** car that you can enter, drive, and exit.  
Do this carefully **once**. After it works, never rebuild from zero — use [§4](#4-second-car--and-every-car-after--fastest-way).

**Good starter mesh:** UAZ FBX → `Assets/Game_Models/uaz/source/uaz.fbx`  
(Mercedes FBX also fine. Some `.glb` cars are broken — prefer FBX.)

### Option A — automatic rig (recommended)

1. Drag the car FBX into the scene under empty parent `VEHICLES`
2. Select the mesh
3. Menu: **MyWorld → Vehicles → Create Car Rig From Selection**
4. Fix orientation (very important):
   - Select the **car root** → blue **Z** arrow must point out the **hood / front**
   - If the mesh faces the wrong way: select **only the mesh child** → Rotation **Y = 180**
5. Nudge `WC_FL` / `WC_FR` / `WC_RL` / `WC_RR` onto the visual wheels
6. Move `Seat` into the driver seat; move `ExitPoint` beside the door
7. Select root → **Rigidbody → Mass** (see [§6](#6-tune-vehicles--mass-grip-bounce-flips) for values)
8. Play → walk up → **E** enter → **WASD** → **Space** handbrake → **E** exit
9. Menu: **MyWorld → Vehicles → Save Selected Car As Prefab**

### Option B — manual setup (same result)

1. Create empty `VEHICLES` → drop mesh → rename e.g. `Car_UAZ`
2. Place on **flat** ground
3. Add **Box Collider** (cover chassis)
4. Add **Rigidbody** — Mass `1500–1600`, Interpolation = Interpolate, Collision = Continuous
5. Child `COM` → low in chassis center
6. Children `WC_FL`, `WC_FR`, `WC_RL`, `WC_RR` + **Wheel Collider** each (radius ≈ visual wheel)
7. On root: **Car Controller** + **Vehicle Seat** + **Vehicle Enter Exit**
8. Assign 4 wheels + COM on Car Controller; ExitPoint on Vehicle Seat
9. Blue Z = hood direction (mesh Y=180 if needed)
10. Play test → save prefab

### If W/S or A/D feel backwards

On **Car Controller**:

- **Invert Throttle** — if W goes reverse  
- **Invert Steer** — if A/D are swapped  

Better long-term fix: mesh + blue Z both face the hood, then leave invert **off**.

### Done when
Enter → drive on flat ground → exit → walk again. Prefab saved under `MyWorld/Prefabs/Vehicles/`.

---

# 4) Second car (and every car after) — fastest way

**You already have one working car.** Do **not** repeat §3 from scratch.

### Fast path (2–5 minutes per car)

1. Select your finished car in the Hierarchy  
   **or** drag its prefab from `Assets/MyWorld/Prefabs/Vehicles/`
2. Menu: **MyWorld → Vehicles → Duplicate Selected Car (fast clone)**  
   (or Ctrl+D)
3. Rename → e.g. `Car_Mercedes`
4. Delete / replace **only the mesh child** with the new FBX
5. Move new mesh to local `0,0,0` under the root
6. Rotate mesh **Y = 180** only if hood ≠ blue Z arrow
7. Slide `WC_*` onto the new wheels; move `Seat` + `ExitPoint`
8. Set **Rigidbody Mass** for this vehicle type ([table in §6](#mass-cheat-sheet-all-vehicles))
9. Play-test once
10. **MyWorld → Vehicles → Save Selected Car As Prefab** (new name)

### What you reuse (do not rebuild)

| Keep on the root | Only change |
|------------------|------------|
| Rigidbody, Box Collider | Mass / collider size if needed |
| Car Controller | Wheel refs if you remade WC objects |
| Vehicle Seat + Enter Exit | Seat / Exit positions |
| 4 WheelColliders | Local positions + radius |

**Rule:** one good prefab = template. Every new car = **duplicate → swap mesh → nudge wheels/seat → save**.

---

# 5) PART C — All vehicle types (cars, bikes, boats)

Save each as a prefab under `MyWorld/Prefabs/Vehicles/`.

### Cars / trucks

| Scene name | Model path | Mass hint |
|------------|------------|-----------|
| `Car_Ferrari` | `Game_Models/ferrari-sf90-xx-wwwvecarzcom/source/*.glb` | 1200 |
| `Car_Mercedes` | `Game_Models/mercedes-benz-g-class-amg-g-63/source/.../*.fbx` | 1800–2000 |
| `Car_UAZ` | `Game_Models/uaz/source/uaz.fbx` | 1500 |
| `Car_Jeep` | `Game_Models/jeep-gladiator/source/` (or `extracted/`) | 1800–2000 |
| `Car_Lada` | `Game_Models/lada-2107/source/` (or `extracted/`) | 1400 |
| `Car_Silverado` | prefer FBX if GLB fails | 1600–1800 |

Scripts: **Car Controller** + Seat + Enter Exit + 4 WheelColliders.

### Bikes

| Scene name | Model path | Mass |
|------------|------------|------|
| `Bike_Jawa` | `Game_Models/motorcycle-custom-bike-jawa-low-poly/source/yawa.fbx` | 220–280 |
| `Bike_Street` | `Game_Models/three-cylinder-naked-street-bike/source/*.fbx` | 220–280 |

Scripts: **Bike Controller** + 2 WheelColliders (front/rear) + Seat + Enter Exit.

### Boats

| Prefab | Path |
|--------|------|
| Wood / Fisher / Scout / Kayak / Jetski | `Assets/Alstra_Boats/Boats LowPoly/Prefabs/` |

1. Drag prefab into water  
2. Add Rigidbody + **Boat Controller**  
3. Set **Water Level Y** = your water plane Y  
4. Seat + Enter Exit  

### Plane

Not in project. Script ready: `PlaneController.cs`. Download an airplane mesh later.

### Place them

```text
VEHICLES
  Car_UAZ
  Car_Mercedes
  Car_Ferrari
  Bike_Jawa
  Boat_Wood       (in water)
```

---

# 6) Tune vehicles — mass, grip, bounce, flips

When a car feels wrong, **do not rebuild it**. Change numbers on the root.

## Where to click

| What you want | Where in Inspector |
|---------------|--------------------|
| How heavy the vehicle is | Root → **Rigidbody → Mass** |
| How “springy” the suspension is | **Car Controller → Spring / Damper / Suspension Distance** |
| Wheel size | **Car Controller → Wheel Radius** (and each WheelCollider Radius) |
| Grip / slippery | Each **WheelCollider → Forward/Sideways Friction → Stiffness**  
  *or* Car Controller → Forward Stiffness / Sideways Stiffness |
| Power / top speed | Car Controller → Motor Torque, Max Speed Kmh |
| Tippy / flips | Move **COM** child lower; lower Motor Torque |
| Handbrake | Space (if Use Space As Handbrake is on) |

Same idea for **Bike Controller** / **Boat Controller** (mass on Rigidbody; other numbers on that script).

## Mass cheat sheet (all vehicles)

| Vehicle | Rigidbody Mass | Notes |
|---------|----------------|-------|
| Small sports car | 1000–1300 | Light, easier to spin |
| Sedan / Lada | 1200–1500 | |
| UAZ / light SUV | 1500–1700 | Good default |
| Pickup / Silverado | 1600–1900 | |
| Heavy SUV / G-Class / Jeep | 1800–2200 | Feels planted |
| Motorcycle | 220–280 | Much lighter than cars |
| Boat | 400–1200 | Depends on size; tune with buoyancy |

**How to change:** select vehicle root → Rigidbody → **Mass** → type value → Play and test.

Heavier = harder to flip, slower to start moving.  
Lighter = snappier, easier to bounce/flip.

## Problem → what to change

| Problem | What to change | Which way |
|---------|----------------|-----------|
| **Car is slippery** (slides like ice) | Wheel friction **Stiffness** (forward + sideways) on Car Controller or each WheelCollider | **Raise** (try 1.5 → 2.0) |
| **Too sticky / won’t drift at all** | Stiffness | **Lower** a little (0.8–1.0) |
| **Bouncing / hopping** on flat ground | **Damper** up; **Spring** down a bit; Suspension Distance shorter | Damper 4500→6000; Spring 35000→28000 |
| **Too soft / bottoms out** | Spring up; Suspension Distance a bit higher | |
| **Flips over easily** | Lower **COM** child (more negative Y); lower Motor Torque; raise Mass slightly | |
| **Wheelspin, no speed** | Motor Torque down **or** friction stiffness up; check wheels touch ground | |
| **Wheels sink into ground** | WheelCollider **Radius** too small, or WC position too low | Match visual wheel |
| **Wheels float above ground** | Radius too big, or WC too high | |
| **Drives toward the trunk** | Mesh Y=180 so hood faces blue Z; or Invert Throttle | |
| **Steer left/right swapped** | Invert Steer, or swap FL/FR wheel assignments | |
| **Boat sinks** | Boat Controller **Water Level Y** = water surface Y; raise Buoyancy | |
| **Bike tips over** | Lower COM; lower Lean Strength; mass ~250 | |

## Suggested starter values (cars)

| Setting | Starter | If bouncing | If slippery |
|---------|---------|-------------|-------------|
| Mass | 1500–1800 | keep / raise | keep |
| Spring | 35000 | 28000–32000 | — |
| Damper | 4500 | 5500–7000 | — |
| Suspension Distance | 0.25 | 0.18–0.22 | — |
| Wheel Radius | 0.35 | match mesh | — |
| Forward Stiffness | 1.2 | — | 1.6–2.0 |
| Sideways Stiffness | 1.1 | — | 1.5–2.0 |
| Motor Torque | 1600 | lower if wheelspin | — |
| COM | low center | lower more | — |

**Tip:** change **one** thing, Play-test, then change the next. Easier to learn what each number does.

---

# 7) WORLD §1 — Terrain & ground paint

### Goal
Shape land + paint sand/grass/dirt/rock.

### Assets

| Type | Path |
|------|------|
| Ready layers | `Assets/SAND_1.terrainlayer`, `GRASS_1`, `DIRT_1`, `FOREST_FLOOR_1`, `MOSSY_ROCKY_1` |
| More layers | `Assets/Game_Terrains/` |
| Source textures | `Assets/Game_Textures/Grass*`, `Ground*`, `Rock*`, `forest_floor*`, `cliff_side*` |
| Free island tex | `Assets/Free_Island_Collection/Environment/Terrain/Textures/` |

### Steps — sculpt

1. Select your Terrain (`MAIN_GROUND` or similar)
2. **Paint Terrain → Raise or Lower** (opacity low: 0.05–0.2)
3. Flatten beach low, village a bit higher, mountains taller, river trench lowered
4. **Smooth Height** on steep spikes

### Steps — paint (brush needs 2+ layers)

1. **Paint Texture**
2. First layer = whole terrain base (normal)
3. **Edit Terrain Layers → Create Layer** for Grass / Dirt / Rock
4. Select the **second** layer → paint with brush
5. Layer Size ≈ `10–20` (not `2`)
6. Rename layers: Sand, Grass, Dirt, Rock (open `.terrainlayer` → rename)

### Done when
Beach reads sand, inland grass, mountains rock, paths dirt.

---

# 8) WORLD §2 — Beach & water

### Assets

| Type | Path |
|------|------|
| Water plane | `Assets/AQUAS-Lite/Prefabs/WaterPlane` |
| Water mats | `Assets/Game_Materials/Sea_Water.mat`, `Beach_Water.mat` |
| Pier mesh | `Assets/Game_Models/wooden-pier/` |
| Boats | `Assets/Alstra_Boats/Boats LowPoly/Prefabs/` |
| Rocks | `Assets/Free_Rocks/_prefabs/` |

### Steps

1. Flatten south/coast strip
2. Paint **Sand** at waterline
3. Place `WaterPlane` (or Plane + Sea_Water) at sea level Y
4. Place pier + 1–2 boats + few rocks
5. Soft blend sand → grass inland
6. Optional: Zone empty `Zone_Beach` + later audio

### Done when
Shoreline touches water; pier looks usable; path leads inland.

---

# 9) WORLD §3 — Forest & trees

### Assets (Paint Trees prefabs)

| Tree | Path |
|------|------|
| URP pines (best) | `Assets/Forst/Conifers [BOTD]/Render Pipeline Support/URP/Prefabs/` → `PF Conifer * URP` |
| Oak / Poplar | `Assets/ALP_Assets/Big Oak Tree FREE/Prefabs/` |
| Maple / fern / bush | `Assets/Game_RocksAndVegetation/Prefabs/` |
| Extra rocks | `Rock 1`–`Rock 10` same folder; `Free_Rocks/_prefabs/` |

### Steps

1. Terrain toolbar → **Paint Trees** (not Paint Texture)
2. **Edit Trees → Add Tree** → drag a tree prefab → Add
3. Add 2–3 types (pine + oak + maple)
4. Medium density; leave path openings and clearings
5. Paint forest floor texture under trees
6. Paint Details (grass/ferns) lightly
7. Hand-place 2–3 hero trees for landmarks
8. Scatter rock prefabs in clusters

### Pink trees?
Materials → Shader **Universal Render Pipeline/Lit** (Forst: use **URP** prefabs only).

### Done when
Forest reads dense from distance; roads stay clear.

---

# 10) WORLD §4 — Village & farm

### Assets

| Type | Path |
|------|------|
| Main kit | `Assets/3DForge_FantasyExteriors/FantasyExteriors/Village & Towns/Prefabs/` |
| Quick house | `.../Base/Templates/BasicHouse.prefab` |
| Forest village | `.../Buildings/ForestVillage/ForestVillage02.prefab`, `Smithy.prefab` |
| Farm | `.../Buildings/Farm/Stables/`, `Troughs/` |
| Fences | `.../Fences/Fence01/`, `Fence02/` |
| Extra houses | `Assets/3DWorldInSeconds_Bridges/Prefabs/Houses/` |
| Well / sheds | `Assets/Game_Models/old-village-well/`, `old-shack/` |
| Door scripts | `Assets/MyWorld/Scripts/Buildings/HouseDoor.cs` |

### Steps

1. Flatten village plateau; paint dirt + grass + small plaza
2. Paint path into village first
3. Place 3–6 houses facing the road (`BasicHouse` / ForestVillage)
4. Add fences, stables, troughs, well
5. Few trees at edges only
6. Optional enterable house:
   - Empty at door + collider + **House Door**
   - `InteriorSpawn` + `ExteriorSpawn`
7. Save good houses to `MyWorld/Prefabs/Buildings/`

### Done when
Village feels lived-in; road connects beach → village → forest.

---

# 11) WORLD §5 — Roads & bridges

### Assets

| Type | Path |
|------|------|
| Dirt path (easiest) | Paint `DIRT_1` TerrainLayer as a winding strip |
| Road tool | `Assets/EasyRoads3D/` |
| Long road meshes | `Assets/KajamansRoads/Free/Prefabs/` |
| Bridges | `Assets/3DWorldInSeconds_Bridges/Prefabs/Models/` → `Model_Bridge_01`…`04`, `Model_Wooden_Bridge_01` |
| Path stones | `Assets/Game_Textures/PavingStones*` |

**Use non-Proto bridges** (`Model_Bridge_*`, not `*_Proto`).

### Steps

1. Paint dirt path: Beach → Village → Forest → River → Mountain
2. Lower river trench; place river water
3. Drag wooden/stone bridge across; align with path height
4. Rocks under supports; continue path both sides
5. Optional: EasyRoads network later for nicer mesh roads

### Done when
You can walk or drive a clear loop to the bridge and back.

---

# 12) WORLD §6 — Sky, day/night, polish

### Assets

| Type | Path |
|------|------|
| Skies | `Assets/AllSkyFree/` (assign skybox material) |
| Day/night script | `Assets/MyWorld/Scripts/World/DayNightCycle.cs` |
| Zone trigger | `ZoneTrigger.cs` |
| Zone audio | `AmbientZoneAudio.cs` |
| Volume / look | Scene Global Volume (URP) |

### Steps

1. **Window → Rendering → Lighting → Environment** → assign AllSkyFree skybox
2. Angle Directional Light for nice sun
3. Global Volume: subtle Fog / Bloom
4. Empty `DayNight` → add **Day Night Cycle** → assign sun light
5. Optional: trigger boxes per zone + looping audio
6. Hide unused clutter; fix floating props; Ctrl+S

### Done when
World looks coherent day and night; player can walk/drive the full route.

---

# 13) Asset locations cheat sheet

| Need | Go here |
|------|---------|
| Player | `Assets/StarterAssets/ThirdPersonController/Prefabs/` |
| My scripts | `Assets/MyWorld/Scripts/` |
| My docs | `Assets/MyWorld/Docs/` |
| Car setup menu | **MyWorld → Vehicles** (Unity top menu) |
| Terrain paint layers | `Assets/SAND_1`, `GRASS_1`, … or `Game_Terrains/` |
| Ground textures | `Assets/Game_Textures/` |
| Paint trees | `Forst/.../URP/Prefabs/`, `ALP_Assets/.../Prefabs/`, `Game_RocksAndVegetation/Prefabs/` |
| Village | `3DForge_FantasyExteriors/.../Prefabs/` |
| Bridges | `3DWorldInSeconds_Bridges/Prefabs/Models/` |
| Roads | `EasyRoads3D/`, `KajamansRoads/Free/Prefabs/` |
| Water | `AQUAS-Lite/Prefabs/`, `Game_Materials/` |
| Boats | `Alstra_Boats/Boats LowPoly/Prefabs/` |
| Cars/bikes | `Game_Models/` |
| Skies | `AllSkyFree/` |
| Rocks | `Free_Rocks/_prefabs/`, `Game_RocksAndVegetation/Prefabs/` |

---

# 14) Scripts cheat sheet

| Goal | Components |
|------|------------|
| Walk (StarterAssets) | PlayerArmature + CameraFollowTarget on MainCamera |
| Press E | PlayerInteraction on player |
| Drive car | CarController + VehicleSeat + VehicleEnterExit + 4 WheelColliders |
| Drive bike | BikeController + 2 WheelColliders + Seat + EnterExit |
| Boat | BoatController + Seat + EnterExit |
| House enter | HouseDoor |
| Day/night | DayNightCycle |
| Zone name/audio | ZoneTrigger, AmbientZoneAudio |
| Fast car rig | Menu **MyWorld → Vehicles → Create Car Rig From Selection** |

All under `Assets/MyWorld/Scripts/`.

---

# 15) Troubleshooting

| Problem | Fix |
|---------|-----|
| First-person look | Camera offset `Z = -4` (behind), not `0` |
| Missing script on camera | Remove it; use CameraFollowTarget only |
| Can’t enter car (E) | Add PlayerInteraction; Tag Player; car needs collider |
| Input System errors (`Input.GetKey`) | Project uses new Input System; use updated MyWorld scripts |
| Sitting faces car rear / drives wrong way | Mesh child Y=180 so hood = blue Z; see §3 |
| Car flips | Lower COM; lower torque; raise mass a bit — §6 |
| Slippery | Raise friction stiffness — §6 |
| Bouncing | Raise damper, lower spring — §6 |
| Wheels wrong | Adjust WheelCollider radius/position |
| Texture paints whole terrain | Need 2+ layers; brush paints layer 2+ |
| Pink materials | URP/Lit shader |
| Pink pines | Use Forst **URP** prefabs folder |
| Boat sinks | Set BoatController Water Level Y |
| Falls through ground | Spawn higher; Terrain Collider on |

---

## Suggested session plan

| Session | Do |
|---------|----|
| 1 | §2 Player + §3 First car + save prefab |
| 2 | §4 Duplicate 1–2 more cars; use §6 if they feel bad |
| 3 | World §7 + §8 (terrain, beach, water) |
| 4 | World §9 Forest |
| 5 | World §10 Village |
| 6 | World §11 Roads + bridge |
| 7 | World §12 Sky/polish + more vehicles |

---

## Other MD files (optional archive)

You do **not** need these for daily work. Kept for reference only:

- `START_HERE.md`
- `SECTION_BY_SECTION_BUILD_BIBLE.md`
- `FROM_SCRATCH_WORLD_BUILDING_GUIDE.md`
- `HOW_TO_USE_SCRIPTS.md`
- `VEHICLES_AND_PLAYER.md`
- `ASSETS_INCLUDED_AND_DOWNLOAD.md`
- `STARTER_PACK_README.md`

**Primary guide = this file:** `MASTER_GUIDE.md`

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
| 8 | [Beach, boats, fish, sea](#8-world-2--beach-water-boats-fish--surrounding-sea) | Cove water, drivable boats, fish, ocean around island |
| 9 | [Forest & trees](#9-world-3--forest--trees) | Paint trees, fix pink materials, place rocks |
| 10 | [Village & farm](#10-world-4--village--farm) | Houses, fences, farm props, optional doors |
| 11 | [Roads & bridges](#11-world-5--roads--bridges) | Paths between zones and place bridges |
| 12 | [Sky, weather, fog](#12-world-6--sky-daynight-weather-fog--polish) | AllSkyFree skies, day/night, random rain, forest fog |
| 13 | [Asset paths — what you want → where to look](#13-asset-locations-cheat-sheet) | Houses, trees, cars, boats, roads — full folder map |
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

### Driving camera (same MainCamera — do not add a second camera)

Your **MainCamera** already has **Camera Follow Target**:

| Mode | What happens |
|------|----------------|
| On foot | Follows `PlayerCameraRoot` with Offset `(0, 0.5, -4)` |
| In car | Auto-switches to chase the car (behind / above) |
| Reverse (S / backing up) | Camera **smoothly moves to the front** of the car so you see where you are going |

**Adjust chase view** — select **MainCamera** → Camera Follow Target:

| Field | Meaning |
|-------|---------|
| Drive Offset | Behind the car while going forward (try `0, 2.8, -7.5`) |
| Reverse Offset | In front while reversing (try `0, 2.8, 6.5`) |
| Drive Position Sharpness | How snappy the chase is |
| Reverse Blend Speed | How fast it swings when you reverse |
| Look Height | Point on the car the camera looks at |

No extra camera object needed. Enter/exit switches this automatically.

### Can’t stay in the car / E pops you out

Usually the **same E press** enters and exits in one frame (worse when many scripts run on slopes). Scripts now **lock exit for ~0.45s** after enter. Update project scripts if you still see the old behaviour.

### Done when
Enter → drive on flat **and** slopes → exit → walk again. Prefab saved under `MyWorld/Prefabs/Vehicles/`.

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

## Fix “too slippery” right now (your current car)

1. Select the car root in the Hierarchy (e.g. `Car_UAZ`)
2. Find **Car Controller**
3. Under **Grip**:
   - **Forward Stiffness** → set **`2.2`**
   - **Sideways Stiffness** → set **`2.2`**
4. Optional: **Motor Torque** → lower a bit (e.g. `1600` → `1200`) so wheels don’t spin and slide
5. Optional: **Rigidbody → Mass** → raise a little (e.g. `1500` → `1700`) so it feels planted
6. Press **Play** and turn on flat ground, then on the mountain

Still slides? Push both stiffness values to **`2.5`–`3.0`**.  
Too sticky / can’t drift at all? Lower toward **`1.5`**.

You can change these **while Playing** — grip re-applies every physics step.

## Where to click

| What you want | Where in Inspector |
|---------------|--------------------|
| How heavy the vehicle is | Root → **Rigidbody → Mass** |
| How “springy” the suspension is | **Car Controller → Spring / Damper / Suspension Distance** |
| Wheel size | **Car Controller → Wheel Radius** (and each WheelCollider Radius) |
| Grip / slippery | **Car Controller → Forward Stiffness / Sideways Stiffness** |
| Power / top speed | Car Controller → Motor Torque, Max Speed Kmh |
| How sharp turns feel | Sideways Stiffness + Max Steer Angle |
| Tippy / flips | Move **COM** child lower; lower Motor Torque |
| Handbrake | Space (if Use Space As Handbrake is on) |

Same idea for **Bike Controller** / **Boat Controller** (mass on Rigidbody; other numbers on that script).

## Feel presets (copy these numbers)

Use these as starting points. Paste onto **Car Controller** (+ Mass on Rigidbody).

### A) Daily / SUV (UAZ, Jeep) — planted, not icy

| Setting | Value |
|---------|-------|
| Mass | 1600–1800 |
| Forward Stiffness | 2.0–2.2 |
| Sideways Stiffness | 1.9–2.2 |
| Motor Torque | 1400–1600 |
| Max Steer Angle | 28–32 |
| Downforce | 40–60 |
| Spring / Damper | 35000 / 4500 |

### B) Sports car — sticky, sharp turns, sudden response

| Setting | Value | Why |
|---------|-------|-----|
| Mass | 1100–1300 | Light = snappy |
| Forward Stiffness | **2.4–2.8** | Strong accelerate grip |
| Sideways Stiffness | **2.5–3.0** | **Sharp turns, little slide** |
| Motor Torque | 1800–2200 | Quick punch |
| Max Steer Angle | 32–36 | Responsive steering |
| Max Speed Kmh | 160–200 | |
| Downforce | **80–120** | Stays glued at speed |
| Spring / Damper | 40000 / 5000 | Firmer ride |
| COM | low, slightly forward | Stable under braking |

**Sports tip:** high **Sideways Stiffness** = non-slippery cornering. If it still understeers (plows straight), raise sideways more or lower speed before the turn. If it spins out, lower Motor Torque a bit and keep sideways high.

### C) Drift / playful (more slide on purpose)

| Setting | Value |
|---------|-------|
| Mass | 1200–1400 |
| Forward Stiffness | 1.4–1.6 |
| Sideways Stiffness | **0.9–1.2** |
| Motor Torque | 2000+ |
| Handbrake | use Space to break rear grip |

### D) Off-road mountain (less bounce + more grip)

| Setting | Value |
|---------|-------|
| Mass | 1700–2000 |
| Forward / Sideways Stiffness | 2.2–2.5 |
| Spring | 28000–32000 |
| Damper | 5500–7000 |
| Suspension Distance | 0.2–0.28 |
| Motor Torque | 1200–1500 |

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
| **Car is slippery** (slides like ice) | **Forward + Sideways Stiffness** | **Raise to 2.2–2.5** (sports: 2.5–3.0) |
| **Too sticky / won’t drift at all** | Stiffness | **Lower** a little (1.2–1.5) |
| **Want sharp sports turns** | Sideways Stiffness + Downforce + lighter Mass | Sideways **2.5+**, Downforce **80+**, Mass ~1200 |
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

| Setting | Normal | If slippery | Sports sticky |
|---------|--------|-------------|-----------------|
| Mass | 1500–1800 | raise a bit | 1100–1300 |
| Spring | 35000 | — | 40000 |
| Damper | 4500 | — | 5000 |
| Suspension Distance | 0.25 | — | 0.2 |
| Wheel Radius | 0.35 | match mesh | match mesh |
| Forward Stiffness | **1.8** | **2.2–2.5** | **2.4–2.8** |
| Sideways Stiffness | **1.7** | **2.2–2.5** | **2.5–3.0** |
| Motor Torque | 1600 | lower if wheelspin | 1800–2200 |
| Downforce | 50 | 60 | 80–120 |
| COM | low center | lower more | low + slightly forward |

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

# 8) WORLD §2 — Beach water, boats, fish & surrounding sea

Two separate water bodies:

| Water | Purpose | Size |
|-------|---------|------|
| **Beach cove** | Play area at the beach — boats, fish, pier | Small (one cove) |
| **World sea** | Ocean around the whole island on all 4 sides | Huge (covers map edges) |

**Important:** Your project is **URP**. Do **not** use `AQUAS-Lite` prefabs (Built-in shaders = pink). Use **URP water** below.

### Assets

| Type | Path | URP? |
|------|------|------|
| **Best beach/cove water** | `Assets/3DWorldInSeconds_Bridges/Materials/Nature/LakeWater.mat` | ✓ URP shader |
| River water (alt) | `Assets/3DWorldInSeconds_Bridges/Materials/Nature/RiverWater.mat` | ✓ URP |
| Simple sea color | `Assets/Game_Materials/Sea_Water.mat` | ✓ URP Lit |
| Shallow beach tint | `Assets/Game_Materials/Beach_Water.mat` | ✓ URP Lit |
| ~~AQUAS-Lite~~ | `Assets/AQUAS-Lite/` | ✗ Built-in only — avoid |
| Boats | `Assets/Alstra_Boats/Boats LowPoly/Prefabs/` | ✓ |
| Fish / shark | `.../Prefabs/FishV1`–`FishV4`, `SharkV1` | ✓ static meshes |
| Fish swim script | `Assets/MyWorld/Scripts/World/FishSwim.cs` | |
| Boat drive script | `Assets/MyWorld/Scripts/Vehicles/BoatController.cs` | |
| Pier | `Assets/Game_Models/wooden-pier/` | |
| Sand paint | `Assets/SAND_1.terrainlayer`, `Assets/Game_Materials/Layer_Sand/` | |
| Coastal rocks | `Assets/Free_Rocks/_prefabs/` | |

### Hierarchy (keep tidy)

```text
MY WORLD
├── MAIN_GROUND (Terrain)
├── WATER
│   ├── Beach_Cove_Water      ← small cove at beach
│   ├── Ocean_Sea             ← huge sea around island
│   └── Underwater_Life
│       ├── Fish_School_01
│       └── Fish_School_02
├── BEACH
│   ├── Pier
│   └── Rocks
└── VEHICLES
    └── Boat_Wood (drivable)
```

---

### PART A — Beach cove water (realistic, local)

**Goal:** A calm water area at your beach for boats and fish.

1. **Pick one water height** for the whole project (example: **Y = 10**).  
   Write it down — boats and fish use the same number.

2. **Sculpt beach:** Select Terrain → flatten a **coast strip** → paint **Sand** (`SAND_1`) at the shoreline.

3. **Create beach water plane:**
   - **Hierarchy → 3D Object → Plane** → rename **`Beach_Cove_Water`**
   - Move to your cove center, **Y = your water level** (e.g. 10)
   - **Scale** to cover only the cove (e.g. `X:15  Y:1  Z:20`) — not the whole map
   - Parent under empty **`WATER`**

4. **Realistic material:**
   - Drag material **`LakeWater.mat`** onto the plane  
     (`Assets/3DWorldInSeconds_Bridges/Materials/Nature/LakeWater.mat`)
   - If too dark/light: duplicate mat → tweak **Depth**, **Foam**, **Color** in Inspector

5. **Shore blend:** Lower terrain slightly at the water edge so sand meets water (no gap, no cliff).

6. **Colliders:** Water plane **no collider** (boats use buoyancy script, not water collider).

7. **Optional pier + rocks:**
   - Pier from `Game_Models/wooden-pier/`
   - Rocks from `Free_Rocks/_prefabs/` along shore

**Done when:** Cove looks like calm realistic water touching sand.

---

### PART B — Drivable boats on beach water

1. Drag prefab e.g. **`Wood_BoatV1`** from  
   `Assets/Alstra_Boats/Boats LowPoly/Prefabs/` into the cove
2. On boat root add:
   - **Rigidbody** — Mass `400`–`800`, Use Gravity on
   - **Boat Controller**
   - **Vehicle Seat** + **Vehicle Enter Exit**
3. On **Boat Controller** set **Water Level Y** = same as water plane Y (e.g. `10`)
4. Empty child **`Seat`** in cockpit, **`ExitPoint`** beside boat
5. **Play** → walk to boat → **E** enter → **WASD** drive → **E** exit

| Boat prefab | Good for |
|-------------|----------|
| `Wood_BoatV1` / `Wood_BoatV2` | Beach / calm cove |
| `Fisher_Boat` | Pier fishing look |
| `KayakV1` | Shallow water |
| `Speed_Boat` / `JetskiV1` | Fast fun (later) |

Save finished boat under `Assets/MyWorld/Prefabs/Vehicles/Boat_Wood.prefab`.

---

### PART C — Fish underwater

Fish are **prefabs only** — add **`FishSwim`** for simple movement.

1. Under **`WATER/Underwater_Life`**, drag **`FishV1`**–**`FishV4`** from Alstra prefabs
2. Place **below** water surface (e.g. water Y = 10 → fish at Y = **7**–**9**)
3. Rotate fish to face along the cove
4. **Add Component → Fish Swim** on each (or parent school empty)
5. **Fish Swim settings:**
   - **Depth Lock Y** = fish depth (e.g. `8`)
   - **Wander Radius** = `5`–`10`
6. Optional: **`SharkV1`** farther out, larger wander radius

**Tip:** Duplicate 5–10 fish, vary positions — looks like a school.

---

### PART D — World sea (surrounds whole island, all 4 sides)

**Goal:** Huge ocean around the terrain edges — **separate** from the beach cove plane, same water level.

This is **not** connected to the cove mesh — it sits under/at map edges so the island looks surrounded by sea.

1. Create **`Ocean_Sea`** under **`WATER`**
2. **3D Object → Plane** → rename **`Ocean_Sea`**
3. Position **center of your terrain**, **Y = same water level** as beach (e.g. 10)
4. **Scale very large** — must extend past all terrain edges:  
   - Start with **X:500  Z:500** (adjust to your map size)  
   - Rule: plane edges must be **beyond** the furthest terrain corner on all 4 sides
5. Material options:
   - **Deeper look:** `Sea_Water.mat` (`Game_Materials/`)
   - **Same realistic shader:** duplicate `LakeWater.mat` → darker/deeper color → assign here
6. **Terrain vs sea:** Terrain island should sit **above** water Y; edges slope down to coast/beach. Sea plane is flat below/at horizon.
7. **Beach cove plane** sits **on top** of the same Y — cove is the shallow play zone; ocean is the backdrop ring.

```text
Top view:

    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  Ocean_Sea (huge)
    ~~~                     ~~~~~
    ~~~   [  ISLAND / MAP ] ~~~~~
    ~~~      [cove]        ~~~~~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
```

8. **Later polish:** fog at horizon (§12), slightly darker sea material, beach foam rocks.

**Do not** scale one plane for both cove + ocean — use **two planes**: small cove + huge ocean.

---

### PART E — Zone & audio (optional)

1. Empty **`Zone_Beach`** with Box Collider trigger + **Zone Trigger**  
   Display name: `Beach`
2. Loop wave audio when player enters (download or add later)
3. Optional **`Fog_Beach`** very light haze (§12)

### Troubleshooting

| Problem | Fix |
|---------|-----|
| Water is **pink** | Use `LakeWater.mat` or `Sea_Water.mat` — not AQUAS-Lite |
| Boat sinks / floats wrong | **Water Level Y** on Boat Controller = plane Y exactly |
| Boat won’t enter | Player needs **Player Interaction**; boat needs **Vehicle Enter Exit** |
| Fish floating above water | Lower fish Y; set **Depth Lock Y** on Fish Swim |
| Sea doesn’t surround island | Increase **Ocean_Sea** scale; center on terrain middle |
| Gap between sand and water | Lower terrain edge at shoreline; match water Y |

### Done when
Beach cove has realistic water, 1+ drivable boat, fish swimming, and huge sea visible around all map edges.

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
9. **Forest fog** — see [§12 Forest / zone fog](#steps--forest-fog-and-other-zones)

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
| **Fence style 1** | `.../Fences/Fence01/` → e.g. `fi_vil_fence01_1m_C` (short post fence) |
| **Fence style 2** | `.../Fences/Fence02/` → e.g. `fi_vil_fence02_2,5m_B`, `fi_vil_fence02_2,5m_C` + **gate** `fi_vil_fence02_gate_frame_gate` |
| Fence parts (posts/cross) | `.../Fences/Fence parts/` → posts + cross pieces to build custom lengths |
| Extra houses | `Assets/3DWorldInSeconds_Bridges/Prefabs/Houses/` |
| Well / sheds | `Assets/Game_Models/old-village-well/`, `old-shack/` |
| Door scripts | `Assets/MyWorld/Scripts/Buildings/HouseDoor.cs` |

### Steps

1. Flatten village plateau; paint dirt + grass + small plaza
2. Paint path into village first
3. Place 3–6 houses facing the road (`BasicHouse` / ForestVillage)
4. Add **fences** — pick one style or mix both:
   - **Fence01** = shorter / simpler rail fence
   - **Fence02** = taller sections + has a **gate** prefab
   - Use **Fence parts** if you need extra posts/crossbars
5. Add stables, troughs, well
6. Few trees at edges only
7. Optional enterable house:
   - Empty at door + collider + **House Door**
   - `InteriorSpawn` + `ExteriorSpawn`
8. Save good houses to `MyWorld/Prefabs/Buildings/`

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

# 12) WORLD §6 — Sky, day/night, weather, fog & polish

Everything for atmosphere: skies, moving sun, random weather/rain, and local fog (forest, beach).

### Scripts & assets

| Type | Path |
|------|------|
| AllSkyFree skies | `Assets/AllSkyFree/` |
| Day/night (sun moves) | `Assets/MyWorld/Scripts/World/DayNightCycle.cs` |
| Random weather + rain | `Assets/MyWorld/Scripts/World/WeatherSystem.cs` |
| Zone fog (forest mist) | `Assets/MyWorld/Scripts/World/ZoneFog.cs` |
| Zone name popup | `ZoneTrigger.cs` |
| Zone audio | `AmbientZoneAudio.cs` |

---

### Steps — add a sky (AllSkyFree)

**No script needed** for a static sky. For day/night + weather, use the scripts below.

1. Project → `Assets/AllSkyFree/` → pick a folder
2. Recommended **realistic** skies:

| Use | Material path |
|-----|---------------|
| Sunny day | `Epic_BlueSunset/Epic_BlueSunset.mat` |
| Cloudy / rain | `Overcast Low/AllSky_Overcast4_Low.mat` |
| Storm look | `Deep Dusk/Deep Dusk.mat` |
| Night + stars | `Night MoonBurst/Night Moon Burst.mat` |
| Night (alt) | `Cold Night/Cold Night.mat` |

3. **Window → Rendering → Lighting → Environment**
4. **Skybox Material** → drag your `.mat` (only if you are **not** using Weather System yet)
5. **Ctrl+S**

Skip **Cartoon Base BlueSky** if you want realistic.

---

### Steps — day/night (moving sun + stars at night)

1. Create empty **`DayNight`**
2. **Add Component → Day Night Cycle**
3. **Sun field:** drag **Directional Light** from Hierarchy onto **Sun**  
   (or click ⊙ next to Sun → pick Directional Light)
4. **Day Length Minutes** = `12` (full day/night cycle time)
5. On **Day Night Cycle** (optional if no Weather System):
   - **Day Sky** = `Epic_BlueSunset.mat`
   - **Night Sky** = `Night Moon Burst.mat` or `Cold Night.mat`

**Why night sky?** Epic sunset has a **baked sun in the texture** — at night you must swap to a star sky or the sun stays visible.

---

### Steps — random weather + rain

1. Select **`DayNight`** → **Add Component → Weather System**
2. Assign skies (drag from `Assets/AllSkyFree/`):

| Inspector slot | Material | When |
|----------------|----------|------|
| **Clear Sky** | `Epic_BlueSunset/Epic_BlueSunset.mat` | Sunny |
| **Cloudy Sky** | `Overcast Low/AllSky_Overcast4_Low.mat` | Cloudy |
| **Rain Sky** | `Overcast Low/AllSky_Overcast4_Low.mat` | Rain |
| **Storm Sky** | `Deep Dusk/Deep Dusk.mat` | Storm |
| **Night Sky** | `Night MoonBurst/Night Moon Burst.mat` | Stars at night |

3. **Day Night** auto-links if on same object
4. **Min / Max Weather Minutes** = `4`–`10` (random change interval)
5. **Window → Rendering → Lighting → Other Settings → Fog** = **ON** ✓ (master switch — Zone Fog still controls *where* it appears)
6. On **Weather System** → **Enable Global Fog** = **OFF** ✓ (forest-only fog; turn ON only if you want rain fog everywhere)
7. Press **Play**

| Weather | Sky | Rain | Fog |
|---------|-----|------|-----|
| Clear | Epic sunset | No | Light |
| Cloudy | Overcast | No | Medium |
| Rain | Overcast | Yes | Heavy |
| Storm | Deep Dusk | Heavy | Very heavy |
| Night | Stars (overrides weather) | No | Slight |

Rain particles follow the camera automatically. **No extra rain asset needed.**

**Test faster:** set Min/Max Weather Minutes to `0.5` for quick changes while tuning.

---

### Steps — forest fog (and other zones)

**Do not use Volume Profile → Fog** (often missing in URP Lighting menu). Use **Zone Fog** instead.

1. In the **forest**, create empty **`Fog_Forest`**
2. **Add Component → Box Collider** → **Is Trigger** = on → size box to cover forest
3. **Add Component → Zone Fog**
4. **Fog Density** = `0.02`–`0.03`
5. **Weather System → Enable Global Fog** = **OFF** (important — otherwise fog is worldwide)
6. **Lighting → Other Settings → Fog** = **ON** (required so fog can render at all)
7. Play → walk into forest → mist; leave forest → **no fog**

**Other zones (optional):**

| Zone | Object | Fog Density |
|------|--------|-------------|
| Forest | `Fog_Forest` | `0.02`–`0.03` |
| Beach haze | `Fog_Beach` | `0.005`–`0.01` |
| Mountains | `Fog_Mountains` | `0.008` |
| Village | none | skip (keep clear) |

Each = empty + **Box Collider (trigger)** + **Zone Fog**.

---

### Optional polish

1. Global Volume: subtle Bloom
2. Zone trigger boxes + audio (waves / wind / rain)
3. Hide clutter; Ctrl+S

### Done when
Sky works, sun moves, weather changes randomly, rain in wet weather, stars at night, forest has mist.

---

# 13) Asset locations cheat sheet

**One table for everything.** Folders are named by asset pack (vendor), not always `Houses/` / `Trees/`. Use this to find what you need fast.

### What you want → where to look

| What you want | Where to look |
|---------------|----------------|
| **Houses / village** | `Assets/3DForge_FantasyExteriors/FantasyExteriors/Village & Towns/Prefabs/` |
| Quick house template | `.../Village & Towns/Prefabs/Base/Templates/BasicHouse.prefab` |
| Forest village / smithy | `.../Prefabs/Buildings/ForestVillage/` |
| Farm (stables, troughs) | `.../Prefabs/Buildings/Farm/` |
| **Fence 1 (Fence01)** | `.../Prefabs/Fences/Fence01/` — shorter fence (`fi_vil_fence01_1m_C`) |
| **Fence 2 (Fence02)** | `.../Prefabs/Fences/Fence02/` — taller fence (`fi_vil_fence02_2,5m_B` / `_C`) + **gate** |
| Fence parts | `.../Prefabs/Fences/Fence parts/` — posts + cross pieces |
| Village materials (pink fix) | `.../Village & Towns/Materials/` → set shader **URP/Lit** |
| **Extra houses** | `Assets/3DWorldInSeconds_Bridges/Prefabs/Houses/` |
| Well / old shack | `Assets/Game_Models/old-village-well/`, `old-shack/` |
| **Trees (pines)** | `Assets/Forst/Conifers [BOTD]/Render Pipeline Support/URP/Prefabs/` ← **use URP only** |
| **Trees (oak / poplar)** | `Assets/ALP_Assets/Big Oak Tree FREE/Prefabs/` |
| **Trees / bushes / ferns** | `Assets/Game_RocksAndVegetation/Prefabs/` |
| **Rocks** | `Assets/Free_Rocks/_prefabs/` and `Assets/Game_RocksAndVegetation/Prefabs/` |
| **Cars / bikes** | `Assets/Game_Models/` (each vehicle in its own subfolder: `uaz/`, `mercedes-.../`, etc.) |
| **Boats** | `Assets/Alstra_Boats/Boats LowPoly/Prefabs/` |
| **Bridges** | `Assets/3DWorldInSeconds_Bridges/Prefabs/Models/` → `Model_Bridge_*` (not `*_Proto`) |
| **Roads** | `Assets/EasyRoads3D/`, `Assets/KajamansRoads/Free/Prefabs/` |
| **Water (cove / realistic)** | `3DWorldInSeconds_Bridges/Materials/Nature/LakeWater.mat` |
| **Water (surrounding sea)** | `Game_Materials/Sea_Water.mat` on huge Plane |
| **Boats + fish** | `Alstra_Boats/Boats LowPoly/Prefabs/` (FishV1–V4, boats) |
| **Ground textures** | `Assets/Game_Textures/` |
| Terrain layers (paint) | `Assets/SAND_1.terrainlayer`, `GRASS_1`, `DIRT_1`, `FOREST_FLOOR_1`, … or `Assets/Game_Terrains/` |
| **Sky (AllSkyFree)** | `Assets/AllSkyFree/` — Epic_BlueSunset (day), Overcast (cloud/rain), Night MoonBurst (stars) |
| Sky / weather / fog scripts | `DayNightCycle.cs`, `WeatherSystem.cs`, `ZoneFog.cs` in `MyWorld/Scripts/World/` |
| **Player** | `Assets/StarterAssets/ThirdPersonController/Prefabs/` (`PlayerArmature`, `MainCamera`) |
| **Your finished prefabs** | `Assets/MyWorld/Prefabs/` (`Vehicles/`, `Buildings/` — what *you* save) |
| My scripts | `Assets/MyWorld/Scripts/` |
| This guide + docs | `Assets/MyWorld/Docs/` |
| Car setup menu | Unity top bar → **MyWorld → Vehicles** |
| Pier | `Assets/Game_Models/wooden-pier/` |
| Free island textures | `Assets/Free_Island_Collection/Environment/Terrain/Textures/` |

### Pack folders at a glance

```text
Assets/
├── 3DForge_FantasyExteriors/     ← houses, fences, farm, village
├── Forst/ + ALP_Assets/          ← trees (pines + oak)
├── Game_RocksAndVegetation/      ← trees, bushes, rocks
├── Free_Rocks/                   ← more rocks
├── Game_Models/                  ← cars, bikes, well, pier, props
├── Alstra_Boats/                 ← boats
├── 3DWorldInSeconds_Bridges/     ← bridges + some houses
├── EasyRoads3D/ + KajamansRoads/ ← roads
├── AQUAS-Lite/ + Game_Materials/ ← water
├── Game_Textures/ + Game_Terrains/ ← ground paint
├── AllSkyFree/                   ← skyboxes
├── StarterAssets/                ← player
└── MyWorld/Prefabs/              ← YOUR finished cars / houses
```

### Pink assets?

| Asset | Fix |
|-------|-----|
| Village houses (3DForge) | Materials → Shader **Universal Render Pipeline/Lit** (or Render Pipeline Converter: Built-in → URP Universal Renderer) |
| Forst pines | Use the **URP/Prefabs** folder, not the default Built-in ones |
| ALP oak | Materials → **URP/Lit** if pink |

More build steps for each zone: World §7–§12 in this same file.

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
| Day/night (sun moves) | DayNightCycle on `DayNight` + Directional Light in **Sun** slot |
| Random weather + rain | WeatherSystem (same object as DayNight) + AllSkyFree mats |
| Forest / zone fog | ZoneFog + Box Collider trigger (`Fog_Forest`, etc.) |
| Fish swim | FishSwim on FishV1–V4 prefabs |
| Drive boat | BoatController + VehicleSeat + VehicleEnterExit |
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
| E enters then instantly exits | Update VehicleEnterExit (exit lock); wait a moment before pressing E again |
| Hard to enter on slopes | Stand closer; ExitPoint beside door; scripts snap exit to ground |
| Want better car camera | MainCamera → Camera Follow Target → Drive Offset / Reverse Offset |
| Sun still visible at night | Use **Night Sky** on Weather System or Day Night Cycle (Epic sky has baked sun) |
| No fog in forest | Zone Fog + trigger box; Lighting → Fog ON; **Enable Global Fog OFF** on Weather System |
| Fog everywhere (don't want) | Weather System → **Enable Global Fog OFF**; only use Zone Fog in forest |
| Volume has no Fog override | Normal in URP — use **Zone Fog** script instead (§12) |
| No rain | Weather System on DayNight; wait or lower Min Weather Minutes |
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

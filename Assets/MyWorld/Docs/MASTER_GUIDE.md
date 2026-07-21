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
| 3 | [First car (full setup)](#3-part-b--first-car-full-setup-once) | Build one car + **visible sit** step-by-step |
| 4 | [Second car (fastest way)](#4-second-car--and-every-car-after--fastest-way) | After car #1 works: duplicate / menu / swap mesh only |
| 5 | [All vehicles + first bike](#5-part-c--all-vehicle-types-cars-bikes-boats) | Cars list, **full bike setup** (like first car), boats |
| 6 | [Tune vehicles (presets)](#6-tune-vehicles--mass-grip-bounce-flips) | Mountain / Ferrari / sports bike / trail bike — don’t get stuck |
| 6b | [Vehicle sounds & lights](#6b-vehicle-sounds--headlights-car--bike) | Horn (**F**), engine, radio (**M**), headlights (**L**) |
| 6c | [Speedometer HUD](#6c-speedometer-hud) | On-screen km/h while driving — **no asset pack needed** |
| 7 | [Terrain & ground paint](#7-world-1--terrain--ground-paint) | Sculpt land and paint sand / grass / dirt / rock |
| 8 | [Beach, boats, fish, sea](#8-world-2--beach-water-boats-fish--surrounding-sea) | Cove water, **player swim**, boats, fish, ocean |
| 9 | [Forest & trees](#9-world-3--forest--trees) | Paint trees, fix pink materials, place rocks |
| 10 | [Village & farm](#10-world-4--village--farm) | Houses, fences, farm props, optional doors |
| 11 | [Roads & bridges](#11-world-5--roads--bridges) | EasyRoads3D (click terrain), bridges, dirt paths |
| 12 | [Sky, weather, fog](#12-world-6--sky-daynight-weather-fog--polish) | AllSkyFree skies, day/night, random rain, forest fog |
| 13 | [Asset paths — what you want → where to look](#13-asset-locations-cheat-sheet) | Houses, trees, cars, boats, roads — full folder map |
| 14 | [Scripts cheat sheet](#14-scripts-cheat-sheet) | Which component does what |
| 15 | [Troubleshooting](#15-troubleshooting) | Common problems and quick fixes |
| 16 | [NPCs — auto walk](#16-npcs--automatic-walking-navmesh) | Bake NavMesh, villagers wander / patrol safely |
| 17 | [Breakable plants](#17-breakable-plants--bushes-car-drives-through) | Prefab bushes break when the car hits them (not terrain brush grass) |
| 18 | [Birds & flying animals](#18-birds--flying-animals-eagles-flies) | Birds, eagles, flies in the sky — Flying Animal / Flock |
| 19 | [Ground animals](#19-ground--surface-animals-deer-rabbits-dogs) | Surface animals that walk on terrain — Ground Animal / Herd |

**Recommended order:** §2 Player → §3 First car → §5 bike if needed → §6b sounds/lights → §6c speedometer → Play → §4 more cars → World §7–§12 → use §6 whenever a vehicle feels wrong.

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

### Sitting in car / bike (pro look — visible driver)

**Goal:** when you press **E**, the character stays **visible** and looks seated (not invisible / hidden).

Works for **cars and bikes** the same way — only **Kind** and offsets change.

---

#### Before you start (checklist)

| Need | Check |
|------|--------|
| **Vehicle Seat** | On car/bike **root** |
| **Vehicle Enter Exit** | On same **root** |
| **Car / Bike Controller** | On same **root** |
| Player **Player Interaction** | On PlayerArmature |
| Player Tag | **Player** |
| **Seat** empty | Child of vehicle → assigned to **Seat Point** |

Keep scripts on the **root**. Only the **Seat** empty goes in the cushion/saddle; wire it via **Seat Point**.

---

#### Step-by-step — make the player sit visibly

**Step 1 — Find Vehicle Seat**  
1. Hierarchy → select car or bike root (e.g. `Car_UAZ` / `Bike_Street`)  
2. Inspector → **Vehicle Seat** (Add Component if missing)

**Step 2 — Hide Player While Seated = OFF**  
1. Uncheck **Hide Player While Seated**  
2. If left ON, the player disappears when driving (old style)

**Step 3 — Kind = Car or Bike**  
1. **Kind** → **Car** (cabin seat) or **Bike** (straddle / lean forward)

**Step 4 — Apply Kind Defaults**  
1. Right‑click the **Vehicle Seat** component title (not the GameObject)  
2. **Apply Kind Defaults (Car/Bike/Boat)**  
3. Fills **Seat Local Offset** / **Seat Local Euler** and forces Hide OFF

**Step 5 — Seat empty in the real seat**  
1. Under the vehicle → create empty → name **`Seat`**  
2. Scene view → move **`Seat`** into driver cushion / bike saddle  
3. Rotate so **Seat** blue **Z** = vehicle forward  
4. Vehicle root → **Vehicle Seat** → **Seat Point** → drag **`Seat`**  
5. Optional: empty **`ExitPoint`** beside door → **Exit Point**

**Step 6 — Play test**  
1. **Ctrl+S** → **Play**  
2. Walk up → press **E**  
3. Character stays **visible**, snaps to Seat, bends into sit pose  
4. **E** again to exit  

**Step 7 — Tune (Seat Local Offset / Euler)**  
Select the root with **Vehicle Seat**:

| Field | What it does | How to use |
|-------|----------------|------------|
| **Seat Local Offset** | Moves hips relative to Seat | X left/right · Y up/down · Z forward/back |
| **Seat Local Euler** | Tilts the body | X = lean forward (bike needs more) |

**Cyan gizmo** (select root with Vehicle Seat): wire sphere = snap point. Move **Seat** until the sphere sits in the cushion/saddle, then fine-tune Offset.

| Kind | Seat Local Offset | Seat Local Euler |
|------|-------------------|------------------|
| Car | `(0, -0.4, 0.08)` | `(10, 0, 0)` |
| Bike | `(0, -0.15, 0.1)` | `(15, 0, 0)` |

**Tuning tips**
- Floating → lower **Offset Y** (more negative)  
- Sinking → raise **Offset Y**  
- Bike too upright → raise **Euler X** (e.g. `15` → `22`)  
- Facing wrong way → rotate **Seat** 180° on Y  
- Odd hands/legs → normal without Mixamo; Offset still puts you “in” the seat  

**Step 8 — Save**  
1. Stop Play  
2. Copy any Offset/Euler you liked while Playing  
3. Prefab → `MyWorld/Prefabs/Vehicles/`

---

#### Optional — full animated sit (Mixamo)

Starter Assets has **no sit animation**. Procedural bend is automatic via `DriverSitPose`.

For a polished animated sit:
1. Mixamo → download **Sitting** / **Driving** / **Motorcycle Idle** (Humanoid)  
2. Add clip to `StarterAssetsThirdPerson` Animator as a state named e.g. `Sit`  
3. On **Vehicle Seat** → **Sit State Name** = `Sit`  
4. Play → enter → Animator plays that state instead of procedural pose  

---

#### Sit troubleshooting

| Problem | Fix |
|---------|-----|
| Player still invisible | **Hide Player While Seated** must be **OFF** |
| Player stands in wrong place | Move **Seat** empty; assign **Seat Point**; tune **Seat Local Offset** |
| Player too big / huge | **Seat Point** is a scaled FBX mesh. Prefer empty **Seat** (scale 1,1,1). Script auto-compensates; if still big → **Seated Scale Multiplier** ≈ `0.01` |
| Bike leans opposite way | **Bike Controller** → enable **Invert Lean** |
| Player faces backward | Rotate Seat so blue Z = vehicle forward |
| Player **stands on** the seat | Player pivot = feet. Keep **Auto Drop Hips To Seat** ON. Right‑click → **Apply Kind Defaults**. Kind must be **Bike** |
| No sit bend / idle pose | Wait for script recompile; Kind = Bike; Auto Drop ON |
| Can’t find “Apply Kind Defaults” | Right‑click the **Vehicle Seat** header (not the GameObject) |
| E does nothing | Player needs **Player Interaction** + Tag **Player**. Stand within **Enter Radius** (3m). Look for prompt **Press E — Drive** |
| Second bike E does nothing | Same scripts as bike #1; stand closer; wait for compile. Enlarge **Box Collider** if still missed |
| Bike tips / spins on Play | **COM** empty must be under the tank (center). Rigidbody COM X ≈ 0, not `1.45` |

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

**There is no camera on the Jeep itself** — chase uses **MainCamera → Camera Follow Target**.

#### Camera in the sky / player floats after exit (scaled Jeep)

Your **JEEP** root Transform shows **Scale ≈ 90, 90, 90**. That used to multiply chase offsets (`TransformPoint`) so the camera flew hundreds of meters away. Scripts now use **world meters** (rotation only).

**Still fix the Jeep scale properly:**

1. Select **JEEP** root → set **Scale = 1, 1, 1**
2. Select the **mesh child** (FBX) → scale that instead until the Jeep looks car-sized (often `0.01` or `1` depending on import)
3. Or select the FBX in Project → **Inspector → Scale Factor** → Apply (preferred)
4. Create empties **`Seat`** + **`ExitPoint`** (scale **1,1,1**) beside the door / in the cushion → assign on **Vehicle Seat**
5. **Ctrl+S** → Play → enter/exit again

| Symptom | Cause | Fix |
|---------|--------|-----|
| Camera above the whole world | Root scale 90× × old chase math | Scripts fixed; set root scale **1** |
| Exit floating in the sky | ExitPoint under scaled parent | Empty ExitPoint scale 1; ground snap on exit |
| No camera on Jeep | Expected | Use MainCamera only |

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

**Presets (don’t get stuck):** mountain cars → Master Guide **§6 D**; Ferrari/sports → **§6 E**. Same script, different Inspector numbers — save as separate prefabs.

### Bikes — first bike setup (same idea as first car)

**Scripts:** `BikeController` + `VehicleSeat` + `VehicleEnterExit`  
**Wheels:** only **2** WheelColliders (front + rear) — not 4 like cars.

#### Models

| Scene name | Model path | Mass | Preset (§6) |
|------------|------------|------|-------------|
| `Bike_Jawa` | `Game_Models/motorcycle-custom-bike-jawa-low-poly/source/yawa.fbx` | 220–280 | Trail / mountain → **§6 G** |
| `Bike_Street` | `Game_Models/three-cylinder-naked-street-bike/source/*.fbx` | 180–230 | Sports / street → **§6 G** |

#### Step-by-step (first bike)

1. Drag FBX into scene under `VEHICLES` → rename e.g. `Bike_Street`
2. Place on **flat** ground
3. Add **Box Collider** (or capsule) covering the bike body — not too low
4. Add **Rigidbody**
   - Mass `200`–`280` (see table)
   - Interpolation = Interpolate
   - Collision Detection = Continuous
5. Create empty child **`COM`** → move **low** under the seat / engine
6. Create 2 empty children at wheel centers:
   - `WC_Front`, `WC_Rear`
7. On **each child** (select `WC_Front`, then `WC_Rear`): **Add Component → Wheel Collider**
   - Radius ≈ visual wheel
   - For trail bikes: Suspension Distance ≈ `0.25`–`0.35`
   - **Do not** put Wheel Collider only on the bike root
8. On bike root add:
   - **Bike Controller**
   - **Vehicle Seat**
   - **Vehicle Enter Exit**
9. On **Bike Controller** assign (drag from Hierarchy):
   - **Wheel Front** ← drag `WC_Front` (must already have Wheel Collider)
   - **Wheel Rear** ← drag `WC_Rear` (must already have Wheel Collider)
   - Center Of Mass → `COM`
   - Optional: **Visual Front / Rear** ← mesh wheel Transforms (any object works here)
   - If W/S or A/D feel backwards: enable **Invert Throttle** / **Invert Steer** (same as cars)

**If drag-and-drop into Wheel Front / Wheel Rear fails:**  
Those fields need a **Wheel Collider**, not an empty Transform. Empty children with no Wheel Collider will not drop in (cars work because their `WC_*` already have the component). Fix: select each wheel empty → Add **Wheel Collider** → try drag again.

10. Create **`Seat`** (sit point) + **`ExitPoint`** beside the bike → assign on Vehicle Seat
11. Blue **Z** arrow = bike forward (nose). Mesh Y=180 if needed
12. Play → walk up → **E** enter → **WASD** → **E** exit
13. Save prefab: `MyWorld/Prefabs/Vehicles/Bike_Street.prefab`

#### Second bike (fast)

Same as cars: **duplicate prefab → swap mesh → nudge 2 wheels + seat → change Mass / Motor / Lean** (§6 G).

#### Where to tune (sports vs trail)

| Want | Open in docs UI |
|------|-----------------|
| Full bike build steps | **This page (§5)** — section “Bikes” |
| **Visible sit on bike** | **§3 → Sitting in car / bike** (Hide Player OFF + Seat offsets) |
| Sports vs trail numbers | **§6 → G) Bikes** |
| Don’t tip / don’t get stuck | **§6 → G)** checklists |

One script for all bikes — only Inspector values change.

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

### B) Sports / Ferrari

See full preset **→ [E) Sports / Ferrari](#e-sports--ferrari-road-car--sharp-sticky-not-for-mountains)** below (same Car Controller, different numbers).

### C) Drift / playful (more slide on purpose)

| Setting | Value |
|---------|-------|
| Mass | 1200–1400 |
| Forward Stiffness | 1.4–1.6 |
| Sideways Stiffness | **0.9–1.2** |
| Motor Torque | 2000+ |
| Handbrake | use Space to break rear grip |

### D) Off-road / mountain car (UAZ, Jeep, G-Class) — don’t get stuck

Use this on rocky hills, dirt, steep climbs. Goal: climb without spinning or belly-scraping.

| Setting | Value | Why it stops getting stuck |
|---------|-------|----------------------------|
| Mass | 1700–2200 | Heavier = more traction weight |
| **All Wheel Drive** | **ON** ✓ | Front + rear pull — biggest fix for stuck climbs |
| Motor Torque | 1400–1800 | Enough power; too high = wheelspin on dirt |
| Forward / Sideways Stiffness | **2.3–2.8** | Grip on loose / bumpy ground |
| Suspension Distance | **0.28–0.40** | Longer travel = wheels stay on ground over bumps |
| Spring | 28000–35000 | Softer = follows uneven ground |
| Damper | 5500–7500 | Controls bounce so you don’t hop and lose grip |
| Wheel Radius | match visual (often **0.38–0.45**) | Bigger wheels clear rocks |
| COM | **low** but not so low the chassis scrapes | Child `COM` slightly up if belly sticks |
| Max Steer Angle | 32–38 | Tighter turns to escape ruts |
| Downforce | 30–50 | Less important off-road than AWD + suspension |
| Max Speed Kmh | 90–110 | Mountain cars don’t need Ferrari speed |

**Still stuck checklist**
1. **All Wheel Drive = ON**
2. Raise **Suspension Distance** so wheels reach the ground
3. Nudge **WheelColliders** so they touch terrain (not floating)
4. Raise **Forward Stiffness** if wheels spin in place
5. Lower **Motor Torque** a little if it just spins and digs in
6. Make sure **Box Collider** isn’t so low it catches every rock (raise Center Y slightly)

---

### E) Sports / Ferrari (road car) — sharp, sticky, not for mountains

Same script: **Car Controller**. Different numbers. Keep these on **roads / flat**; they get stuck on steep rock if you use soft off-road settings wrong — or if torque is too high and wheels spin.

| Setting | Value | Why |
|---------|-------|-----|
| Mass | 1100–1300 | Light = snappy |
| **All Wheel Drive** | OFF (RWD feel) or ON if you want easier launches | |
| Forward Stiffness | **2.4–2.8** | Strong accelerate grip |
| Sideways Stiffness | **2.5–3.0** | Sharp turns, little slide |
| Motor Torque | 1800–2400 | Quick punch |
| Max Steer Angle | 32–36 | Responsive |
| Max Speed Kmh | 160–220 | |
| Downforce | **80–140** | Glued at high speed |
| Spring / Damper | 40000–45000 / 5000–6000 | Firm race feel |
| Suspension Distance | **0.15–0.22** | Low sports stance |
| Wheel Radius | match visual (often smaller **0.30–0.35**) | |
| COM | low + slightly forward | Stable under braking |

**Don’t get stuck (sports car)**
- On **highway / EasyRoads / flat**: high stiffness + downforce — won’t spin out as easily
- On **mountain / rocks**: do **not** use sports suspension — either stay on road **or** temporarily use mountain preset (AWD + longer suspension) and save as a separate prefab (`Car_Ferrari_Road` vs don’t take Ferrari off-road)
- If stuck spinning on gravel: lower Motor Torque, raise Forward Stiffness, enable AWD for that climb only

**Sports tip:** high **Sideways Stiffness** = non-slippery cornering. Understeer → raise sideways / slow down. Spins out → lower Motor Torque, keep sideways high.

---

### F) Drift / playful (more slide on purpose)

| Setting | Value |
|---------|-------|
| Mass | 1200–1400 |
| Forward Stiffness | 1.4–1.6 |
| Sideways Stiffness | **0.9–1.2** |
| Motor Torque | 2000+ |
| Handbrake | use Space to break rear grip |

---

### G) Bikes — sports vs trail (don’t tip / don’t get stuck)

Script: **Bike Controller** (same for all bikes — only change Inspector numbers + Rigidbody Mass).

#### Sports / street bike (fast road)

| Setting | Value | Notes |
|---------|-------|-------|
| Mass | 180–230 | Lighter = quicker |
| Motor Torque | 1000–1400 | Strong pull |
| Max Speed Kmh | 140–180 | |
| Max Steer Angle | 26–30 | |
| Lean Strength | 16–22 | Sporty lean |
| COM | low, slightly forward | Less tippy under braking |
| WheelCollider Radius | match visual (often thinner) | Must touch ground |

**Don’t get stuck / tip (sports)**
- Keep on roads; steep rock will high-center easily
- If front lifts / wheelies into stuck: lower Motor Torque, move COM slightly forward
- If tips over in turns: lower Lean Strength, raise Mass a bit (~250)
- Script keeps a **kickstand** when parked (won’t fall on Play); while riding it auto-balances — lower **Lean Strength** if it still wobbles

#### Trail / enduro / mountain bike (rough ground)

| Setting | Value | Notes |
|---------|-------|-------|
| Mass | 220–280 | Planted |
| Motor Torque | 700–1000 | Lower = less wheelspin on dirt |
| Max Speed Kmh | 80–110 | |
| Max Steer Angle | 30–36 | Tighter escape turns |
| Lean Strength | **8–14** | Less lean = more stable on bumps |
| COM | low center | Critical for not tipping |
| WheelCollider Radius | **slightly larger** than visual if needed | Helps clear bumps |
| Suspension (on each WheelCollider) | Distance **0.25–0.35**, higher Damper | Soft travel so wheels stay planted |

**Don’t get stuck (trail bike)**
1. Wheels must **touch** terrain — raise radius or lower WC positions if floating
2. Lower Motor Torque if rear spins and digs in
3. Lower Lean Strength on mountains
4. Avoid ultra-low COM that scrapes the engine case on rocks

#### Which bike prefab → which preset

| Your model | Use preset |
|------------|------------|
| `Bike_Street` / sport naked | Sports / street |
| `Bike_Jawa` / custom / trail look | Trail / mountain |
| Future dirt bike | Trail numbers |

---

### Quick “which preset?” table

| Vehicle feel | Use section | Script |
|--------------|-------------|--------|
| UAZ / Jeep on mountains | **D) Off-road mountain** | Car Controller + **AWD ON** |
| Ferrari / sports on road | **E) Sports / Ferrari** | Car Controller |
| Daily SUV on town roads | **A) Daily / SUV** | Car Controller |
| Drift fun | **F) Drift** | Car Controller |
| Sport bike on asphalt | **G) Sports bike** | Bike Controller |
| Trail bike on dirt/hills | **G) Trail bike** | Bike Controller |

**Rule:** One script type per vehicle class (`CarController` / `BikeController`). You do **not** need different scripts for Ferrari vs UAZ vs trail bike — only different **Inspector values** (save as separate prefabs).

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

# 6b) Vehicle sounds & headlights (car + bike)

Works the same on **cars and bikes**. Scripts only run while you are **driving** (after **E**).

### Scripts

| Script | What it does |
|--------|----------------|
| **Vehicle Audio** | Engine loop (pitch rises with speed), **horn**, optional **radio** |
| **Vehicle Lights** | Toggle **headlights** on/off |

Add both on the **same root** as Car/Bike Controller.

### Controls (while driving)

| Key | Action |
|-----|--------|
| **F** | Horn |
| **L** | Headlights on/off |
| **M** | Radio / music on/off (if clip assigned) |

### Setup — sounds (5 minutes)

1. Select car or bike root → **Add Component → Vehicle Audio**
2. You need AudioClips (download free engine/horn packs, or use any `.wav` / `.mp3` in `Assets/Audio/Vehicles/`)
3. Assign:
   - **Engine Loop** — looping engine idle/drive (required for engine sound)
   - **Horn Clip** — short beep
   - **Radio Music** — optional soft loop
4. Play → enter vehicle → drive → hear pitch change; press **F** for horn; **M** for radio

> No clips assigned = silent (script still safe). Engine only plays when a loop clip is set.

### Setup — headlights

1. Select vehicle root → **Add Component → Vehicle Lights**
2. **Either:**
   - Right‑click **Vehicle Lights** title → **Create Default Headlight Spots** (adds 2 Spot Lights), **or**
   - Create your own Spot Lights on the mesh (bike: often **one** light on the headlamp)
3. Drag those Lights into the **Headlights** array
4. Move/rotate spots so they point forward from the real lamp
5. Play → enter → press **L** to toggle

**Bike tip:** delete the extra spot or leave one slot empty; aim the remaining Spot from the headlamp mesh.

**Car tip:** place L/R spots at the real headlight meshes; intensity ≈ `4–8`, range ≈ `30–40`.

### Done when
Enter → drive with engine pitch → **F** horn → **L** lights → **M** radio (if clip set).

---

# 6c) Speedometer HUD

Shows **km/h** on screen while you are driving any car/bike.  
**No download / asset pack required** — uses built-in uGUI (Text + optional fill bar).

### Script

| Script | Path |
|--------|------|
| **Vehicle Speedometer** | `Assets/MyWorld/Scripts/UI/VehicleSpeedometer.cs` |

### Setup (once for the whole scene)

1. In Hierarchy: create empty `HUD_Speedometer` (or put it under your existing Canvas)
2. **Add Component → Vehicle Speedometer**
3. Leave **Speed Text** / **Speed Fill** empty → at Play it **auto-creates** a simple bottom-center overlay
4. Or wire your own UI: assign a **Text** (and optional filled **Image**) in the Inspector
5. Play → enter a vehicle → drive → see speed; exit → HUD hides

### Optional Inspector

| Field | Meaning |
|-------|---------|
| Max Speed For Fill | Speed at which the yellow bar is full (match your car’s Max Speed Kmh roughly) |
| Label Format | Default `{0:0} km/h` |
| Create Ui If Missing | ON = auto UI; OFF = you must assign Text yourself |

### Done when
Enter → drive → number rises with speed → exit → HUD disappears.

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

6. **Colliders:** Water **visual** plane = **no Mesh Collider** (or you walk/drive on it).  
   Add swimming with a **trigger volume** (below).

7. **Optional pier + rocks:**
   - Pier from `Game_Models/wooden-pier/`
   - Rocks from `Free_Rocks/_prefabs/` along shore

**Done when:** Cove looks like calm realistic water touching sand.

---

### PART A2 — Player swimming

**Scripts**
| Script | Where |
|--------|--------|
| **Player Swim** | On **PlayerArmature** |
| **Water Volume** | On a trigger covering the water |

**Setup**
1. Select **PlayerArmature** → **Add Component → Player Swim**
2. Select your water (or create empty child **`Water_SwimVolume`** under `WATER`)
3. **Add Component → Box Collider**
   - **Is Trigger = ON**
   - Size: cover the whole swim area (wide X/Z, tall enough Y — from below surface up to ~1 m above)
   - Center: so the box sits in the water body
4. **Add Component → Water Volume**
   - **Use Transform Y = ON** if this object sits at water surface height  
   - Or set **Surface Y** = same number as water plane Y (e.g. `10`)
5. Make sure the water **mesh** has **no solid collider**
6. Play → walk into the water → you float and can swim

**Swim controls**

| Key | Action |
|-----|--------|
| **WASD** | Swim / steer |
| **Shift** | Swim faster |
| **Space** | Swim up |
| **Left Ctrl** or **C** | Dive |

Walk back onto the beach (leave the trigger) to walk again.

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
| Walk / drive on water | Remove Mesh Collider from water plane; use **Water Volume** trigger for swim |
| Can’t swim | Add **Player Swim** on player + **Water Volume** + Box Collider **Is Trigger** (§8 A2) |

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

Three ways to make roads. **EasyRoads3D** is the professional tool (click the terrain).

| Method | Path | Best for |
|--------|------|----------|
| **EasyRoads3D** (recommended) | `Assets/EasyRoads3D/` | Mesh roads that follow terrain — Shift+Click markers |
| Dirt path (fastest) | Paint `DIRT_1` TerrainLayer | Quick trail with no mesh |
| KajamansRoads | `Assets/KajamansRoads/Free/Prefabs/` | Long highway meshes you drag by hand |
| Bridges | `Assets/3DWorldInSeconds_Bridges/Prefabs/Models/` → `Model_Bridge_*` (not `*_Proto`) | Rivers / gaps |
| Path stones | `Assets/Game_Textures/PavingStones*` | Optional detail |

**Docs / Quick Start file:** `Assets/EasyRoads3D/Quick Start.txt`  
**Full manual:** https://www.easyroads3d.com/v3/manualv3.html

---

### PART A — EasyRoads3D (professional roads on terrain)

This is the system where you **click the terrain** and build a real road mesh that cuts into the ground.

#### Setup once

1. Open scene **`MY WORLD`**
2. Make sure your **Terrain** exists and is selected / visible
3. Menu: **GameObject → 3D Object → EasyRoads3D → New Road Network**
4. A **Road Network** object appears in the Hierarchy — select it

#### Create a road (click path)

1. In the Inspector, find the **EasyRoads3D toolbar**
2. Click the **Road+** tab (2nd icon from the left)
3. Press **Add New Object**
4. In the **Scene** view, **Shift + Click** on the terrain along your route  
   (e.g. Beach → Village → Forest → Bridge → Mountain)
5. Keep adding markers until the road shape looks good

#### Edit markers

| Key / action | What it does |
|--------------|--------------|
| **Shift + Click** | Add a new marker on the terrain |
| **I** | Insert a marker between existing ones (move mouse to spot first) |
| **Shift + R** | Remove the selected marker |
| **Shift + click markers** | Select multiple markers to move together |

#### Fit the road to the terrain

1. When the shape is ready, open the **mountain** tab (middle tab) on the EasyRoads toolbar
2. Click **Build Terrain's**
3. EasyRoads lowers / shapes the terrain under the road so the mesh sits flush  
4. Drive or walk the road to check height

#### Materials (URP)

- Example road mats: `Assets/EasyRoads3D/Resources/Materials/roads/` (e.g. dirt material)
- If roads are **pink**: import URP support from  
  `Assets/EasyRoads3D/SRP Support Packages/` (double-click the URP package for your version)

#### Tips

- Build one main loop first (beach ↔ village ↔ forest), then side roads
- Keep markers roughly even spacing for smoother curves
- After **Build Terrain's**, you can still adjust markers and rebuild
- Save the scene (**Ctrl+S**) when happy

**Done when (EasyRoads):** You can drive a car from beach to village on a mesh road that sits on the terrain.

---

### PART B — Dirt path only (no road mesh)

1. Select **Terrain** → Paint Texture
2. Use layer **`DIRT_1`** (or create a dirt layer)
3. Paint a winding strip: Beach → Village → Forest → River → Mountain
4. Soften edges with a larger brush / lower opacity

Good for trails; use EasyRoads when you want a real driveable road surface.

---

### PART C — KajamansRoads (drag long meshes)

1. Open `Assets/KajamansRoads/Free/Prefabs/`
2. Drag a road prefab into the scene (2-lane / 6-lane / alley, etc.)
3. Move / rotate / scale to sit on the terrain by hand  
   (does **not** auto-cut terrain — EasyRoads does)

---

### PART D — Bridges

1. Lower a river trench on the terrain; place water if needed
2. Drag `Model_Bridge_01`…`04` or `Model_Wooden_Bridge_01` from  
   `Assets/3DWorldInSeconds_Bridges/Prefabs/Models/`
3. Align bridge height with your EasyRoads / dirt path on both sides
4. Scatter rocks under supports
5. Continue the road/path on both banks

**Use `Model_Bridge_*` — not `*_Proto`.**

---

### Troubleshooting roads

| Problem | Fix |
|---------|-----|
| No EasyRoads menu | Confirm `Assets/EasyRoads3D/` imported; wait for Editor DLL compile |
| Road floating / sinking | Mountain tab → **Build Terrain's** again |
| Pink road materials | Import URP package from `EasyRoads3D/SRP Support Packages/` |
| Shift+Click does nothing | Select Road Network; Road+ tab → Add New Object first; click in Scene view (not Game) |
| Road too narrow/wide | Select road object → adjust width in EasyRoads Inspector settings |

### Done when
You can walk or **drive** a clear loop (beach → village → forest → bridge and back).

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

Rain particles follow the **camera** and cover a wide box (~90m) so the whole visible area gets rain — not only a tiny cloud over the player.

**If rain looks fake / only on the player:** select **Weather System** → raise **Rain Coverage** to `120`–`150`, **Rain Rate** to `3000+`. Scripts auto-configure stretched streak particles.

### Steps — player swim (don't drown)
1. **Player Swim** on PlayerArmature  
2. Water empty with **Box Collider Is Trigger** + **Water Volume**  
3. **Surface Y** = water plane Y (or put volume at surface with **Use Transform Y**)  
4. Water mesh = **no solid collider**  
5. Trigger box must be **tall and wide** enough that walking in hits it  

If you sink: Surface Y is too low, or trigger is missing — float uses Surface Y as the water top.

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
| **Roads (EasyRoads — click terrain)** | `Assets/EasyRoads3D/` → GameObject → EasyRoads3D → New Road Network |
| **Roads (mesh prefabs)** | `Assets/KajamansRoads/Free/Prefabs/` |
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
| Drive bike | BikeController + VehicleSeat + VehicleEnterExit + **2** WheelColliders (see Master Guide **§5 Bikes**) |
| Horn / engine / radio | **Vehicle Audio** on vehicle root — **F** horn · **M** radio (§6b) |
| Headlights on/off | **Vehicle Lights** on vehicle root — **L** toggle (§6b) |
| Speedometer (km/h) | **Vehicle Speedometer** once in scene — auto UI, no asset pack (§6c) |
| Sit visibly in vehicle | **§3 Sitting** — Hide Player OFF; Seat empty in cushion/saddle; tune Offset/Euler |
| Player invisible when driving | Vehicle Seat → **Hide Player While Seated = OFF** |
| Player sit pose wrong | Move Seat; tune **Seat Local Offset** / **Euler** (§3) |
| Boat | BoatController + Seat + EnterExit |
| House enter | HouseDoor |
| Day/night (sun moves) | DayNightCycle on `DayNight` + Directional Light in **Sun** slot |
| Random weather + rain | WeatherSystem (same object as DayNight) + AllSkyFree mats |
| Forest / zone fog | ZoneFog + Box Collider trigger (`Fog_Forest`, etc.) |
| Fish swim | FishSwim on FishV1–V4 prefabs |
| Player swim | **Player Swim** on player + **Water Volume** trigger on water (§8 A2) |
| Breakable bushes / plants | **Breakable Foliage** — trigger collider so car passes through and plant breaks |
| Birds / eagles / flies | **Flying Animal** (one) or **Flying Flock** (many) — §18 |
| Ground animals (deer, dogs…) | **Ground Animal** / **Ground Herd** — §19 |
| Drive boat | BoatController + VehicleSeat + VehicleEnterExit |
| EasyRoads3D roads | Menu **GameObject → 3D Object → EasyRoads3D → New Road Network** (Shift+Click markers) |
| Zone name/audio | ZoneTrigger, AmbientZoneAudio |
| Fast car rig | Menu **MyWorld → Vehicles → Create Car Rig From Selection** |
| NPC auto walk | **NpcWander** + **Nav Mesh Agent** + bake **Nav Mesh Surface** (§16) |
| Setup NPC fast | Menu **MyWorld → NPC → Setup Selected As Wandering NPC** |

All under `Assets/MyWorld/Scripts/`.

---

# 15) Troubleshooting

| Problem | Fix |
|---------|-----|
| First-person look | Camera offset `Z = -4` (behind), not `0` |
| Missing script on camera | Remove it; use CameraFollowTarget only |
| Can’t enter car (E) | Add PlayerInteraction; Tag Player; car needs collider |
| Can’t exit car / bike | Wait a moment after enter, then **hold E**. Prompt shows **Hold E to exit** |
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
| Car stuck on mountain | §6 **D** — AWD ON, longer suspension, check wheels touch ground |
| Ferrari spins / stuck | §6 **E** — raise stiffness / lower torque; keep on roads |
| Bike tips or stuck | §6 **G** — trail vs sports presets; lower Lean Strength on hills |
| Can’t drag bike wheels into Bike Controller | Fields need **Wheel Collider** — Add Component on `WC_Front` / `WC_Rear` first (§5 Bikes) |
| Slippery | Raise friction stiffness — §6 |
| Bouncing | Raise damper, lower spring — §6 |
| Wheels wrong | Adjust WheelCollider radius/position |
| Texture paints whole terrain | Need 2+ layers; brush paints layer 2+ |
| Pink road / EasyRoads materials | Import URP from `EasyRoads3D/SRP Support Packages/` |
| EasyRoads Shift+Click does nothing | Road Network selected; Road+ → Add New Object; click in **Scene** view |
| Road floats above terrain | EasyRoads mountain tab → **Build Terrain's** |
| Pink pines | Use Forst **URP** prefabs folder |
| Boat sinks | Set BoatController Water Level Y |
| Falls through ground | Spawn higher; Terrain Collider on |
| NPC stands still / warning not on NavMesh | Bake **NavMesh Surface** (§16); place NPC on blue walkable area |
| NPC walks through houses | Include buildings as **Not Walkable** / obstacles when baking; or Navigation Static on blockers |
| NPC slides / spins | Nav Mesh Agent radius/height match character; freeze Rigidbody if any |

---

# 16) NPCs — automatic walking (NavMesh)

**Goal:** drop characters in the village / farm / roads and have them **walk by themselves**, staying on ground you allow and avoiding rocks, houses, and cliffs.

Package already in this project: **`com.unity.ai.navigation`**.

### Scripts

| Script | Path |
|--------|------|
| **Npc Wander** | `Assets/MyWorld/Scripts/Npc/NpcWander.cs` |
| Editor menu | **MyWorld → NPC → Setup Selected As Wandering NPC** |

One script does: **random wander**, **waypoint patrol**, **idle**, optional **avoid player**, optional **Animator** walk speed.

---

### Part A — Bake walkable ground (do once for the world)

This tells Unity where NPCs are allowed to walk.

1. In Hierarchy, create empty → name **`NavMesh_World`**
2. **Add Component → Nav Mesh Surface** (from AI Navigation)
3. On **Nav Mesh Surface**:
   - **Agent Type** = Humanoid (default)
   - **Collect Objects** = **All** (or **Children** if you parent only walkable meshes under it)
   - **Include Layers** = Everything (or exclude Water if you have a Water layer)
4. Select your **Terrain** (`MAIN_GROUND`) — confirm it has a **Terrain Collider**
5. Optional but recommended for buildings/rocks:
   - Select house / big rock roots → Inspector → static dropdown → enable **Navigation Static**  
     (they become obstacles the bake avoids)
6. Select **`NavMesh_World`** → click **Bake**
7. In Scene view you should see a **blue** walkable overlay on the ground

**Re-bake** after you add big props or reshape terrain.

> **Window → AI → Navigation** also works for older workflows. With this package, **Nav Mesh Surface → Bake** on an empty is the simple path.

**Tips**
- Don’t bake water / steep cliffs as walkable if you don’t want NPCs there (use areas or exclude layers).
- Roads + village plaza + farm paths should show blue.
- If blue is missing under a character, move them onto blue or raise **Agent Radius** bake settings slightly and Bake again.

---

### Part B — Make one character walk (2 minutes)

1. Place your character prefab in the scene (village square, farm, road)
2. Select it in Hierarchy
3. Menu: **MyWorld → NPC → Setup Selected As Wandering NPC**  
   (adds **Nav Mesh Agent** + **Npc Wander**)
   - Or manually: Add **Nav Mesh Agent**, then **Npc Wander**
4. On **Npc Wander**:
   - **Mode** = **Wander**
   - **Wander Radius** = `8`–`20` (how far from spawn they roam)
   - **Walk Speed** ≈ `1.2`–`1.6`
5. If they have an **Animator** with a float **Speed** (or **MoveSpeed**):
   - Assign **Animator**
   - Set **Speed Param** to that name (default `Speed`)
6. **Ctrl+S** → **Play**
7. They should idle briefly, then walk to random points on the blue NavMesh

**Green wire sphere** (select NPC) = wander radius.

---

### Part C — Patrol path (guards / road walkers)

1. Create empties: `WP_1`, `WP_2`, `WP_3` along a path
2. On **Npc Wander**:
   - **Mode** = **Patrol**
   - **Waypoints** size = 3 → drag `WP_1`…`WP_3`
   - **Loop Patrol** = ON
3. Play — they walk the loop and wait at each point

---

### Part D — Many NPCs

1. Finish **one** working NPC → save as prefab under `MyWorld/Prefabs/NPCs/`
2. Duplicate around village / farm / beach path
3. Change **Wander Radius** / **Home** per instance (they use start position as home)
4. Optional: enable **Avoid Player** so they step aside when you get close

---

### Controls / settings cheat sheet

| Field | Meaning |
|-------|---------|
| Mode Wander | Random safe points inside radius |
| Mode Patrol | Follow waypoint list |
| Mode Idle | Stand still |
| Wander Radius | Max roam distance from spawn |
| Min/Max Wait | Pause between walks |
| Run Chance | Sometimes jog to the next point |
| Avoid Player | Step away when player is near |
| Speed Param | Animator float driven by move speed |

---

### Animator note

Starter / Mixamo characters often use **Speed** or **Forward**.  
If walking looks like a T-pose slide: open Animator → confirm a blend tree uses your **Speed Param**, or leave Animator empty (they still move, without leg animation).

---

### Done when
Blue NavMesh on terrain → NPC moves alone → stops before walking into the ocean / through houses → optional patrol looks natural.

---

# 17) Breakable plants / bushes (car drives through)

**Goal:** when a car drives into a bush or fern, the plant **breaks** and the car **keeps going** (not blocked).

**Script:** `Assets/MyWorld/Scripts/World/BreakableFoliage.cs`  
**Component name:** **Breakable Foliage**

---

### Important — what works / what doesn’t

| How you placed plants | In Hierarchy? | Can use Breakable Foliage? |
|-----------------------|---------------|----------------------------|
| **Prefab** dragged into scene (bush, fern mesh) | **Yes** | **Yes** |
| Terrain **Paint Details** brush (grass / detail ferns) | **No** | **No** — not real GameObjects |
| Terrain **Paint Trees** | No (on Terrain) | Not with this script |

Terrain brush grass stays as decoration. For breakables, **place prefab plants**.

Good prefab sources:
- `Assets/Game_RocksAndVegetation/` (Bush, Fern, etc.)
- Other vegetation prefabs under `Assets/`

---

### Why Is Trigger?

| Collider | What happens |
|----------|----------------|
| **Solid** (Is Trigger OFF) | Car **bumps / stops** — plant acts like a wall |
| **Trigger** (Is Trigger ON) | Car **passes through** → script can break the plant |

---

### Setup — new plant (recommended)

1. Drag a **bush/fern prefab** into the Scene (near a road)
2. Select it in Hierarchy  
3. Collider → check **Is Trigger**  
   - No collider? **Add Component → Box Collider** → Is Trigger ON  
4. **Add Component → Breakable Foliage**  
5. **Mode** = **Hide** (simplest)  
6. Leave **Force Trigger Colliders** ON  
7. **Ctrl+S** → **Play** → drive through → plant breaks  

Save as prefab under `MyWorld/Prefabs/` if you want to reuse it.

---

### Setup — plant already in the scene

1. Hierarchy → click the bush/fern (must be a prefab instance, not terrain paint)  
2. Inspector → collider → **Is Trigger = ON**  
3. **Add Component → Breakable Foliage**  
4. Mode = **Hide**  
5. Play → drive through  

**Many at once:** Ctrl+Click several → set Is Trigger → add Breakable Foliage on each (or one at a time).

---

### Modes

| Mode | Result |
|------|--------|
| **Hide** | Plant disappears (fast, good default) |
| **Swap Mesh** | Hide intact mesh, show a broken child mesh |
| **Physics Debris** | Spawn/fling Rigidbody pieces |

Optional: assign **Break Particles** / **Break Sound**.

---

### Troubleshooting

| Problem | Fix |
|---------|-----|
| Car still blocked | Every collider on plant must be **Is Trigger** |
| Nothing breaks | Need placed **prefab** + **Breakable Foliage**; car needs Rigidbody / Car Controller |
| Can’t find plant in Hierarchy | It was painted with Terrain brush — place a prefab instead |
| Terrain grass doesn’t break | Expected — use prefab bushes along roads for breakables |

---

### Done when
Drive through a roadside bush → car continues → bush breaks / hides.

---

# 18) Birds & flying animals (eagles, flies)

**Goal:** birds / eagles / flies moving in the sky (ambient life).

**We did not have this before.** New scripts:

| Script | Use for |
|--------|---------|
| **Flying Animal** | One bird, eagle, or fly |
| **Flying Flock** | Spawn many copies (flock / swarm) |

Paths: `Assets/MyWorld/Scripts/World/FlyingAnimal.cs`, `FlyingFlock.cs`

Example mesh: `Assets/Game_Models/golden_eagle.glb` (or any bird pack you download).

---

### Setup — one bird / eagle

1. Drag bird/eagle mesh into the Scene **above** the ground (e.g. Y = 20–40)
2. Scale if needed so it looks right in the sky
3. **Add Component → Flying Animal**
4. Pick **Style**:
   - **Wander** — random flight in a volume (good default)
   - **Orbit** — circles a home point
   - **Figure8** — lazy figure-8
5. Tune presets:

| Creature | Move Speed | Wander Radius | Height Min/Max Offset |
|----------|------------|---------------|------------------------|
| Fly / insect | `8`–`14` | `5`–`12` | `-1` / `3` |
| Small bird | `5`–`8` | `15`–`30` | `-2` / `8` |
| Eagle | `3`–`6` | `40`–`80` | `-5` / `20` |

6. **Play** — it should fly on its own  
   Cyan gizmo (selected) = roam area

No collider needed (decorative). Remove colliders if the bird bumps into things.

---

### Setup — flock / swarm

1. Create empty **`Birds_Meadow`** high above a meadow/forest
2. **Add Component → Flying Flock**
3. **Animal Prefab** → drag your bird prefab (with or without Flying Animal)
4. **Count** = `5`–`12`
5. **Spawn Box** ≈ `(30, 8, 30)`
6. Play → flock appears and flies

---

### Tips

- Face the model’s **forward (blue Z)** toward the beak / nose so it flies nose-first  
- If it flies backward: rotate mesh child **Y = 180**  
- Mix: a few eagles high + small birds lower + tiny flies near village  
- Audio: optional zone birdsong with **Ambient Zone Audio** (separate from flight)

---

### Done when
You look up and see at least one bird circling / wandering over the world.

---

# 19) Ground / surface animals (deer, rabbits, dogs)

**Goal:** animals that **walk on the terrain surface** (not flying, not swimming).

**We did not have this before** (NPCs use NavMesh for people; this is for wildlife on ground).

| Script | Use for |
|--------|---------|
| **Ground Animal** | One deer, rabbit, dog, cow, sheep, etc. |
| **Ground Herd** | Spawn several in an area |

Paths: `Assets/MyWorld/Scripts/World/GroundAnimal.cs`, `GroundHerd.cs`

They **raycast onto the terrain** — no NavMesh bake required.

---

### Setup — one animal

1. Place animal mesh/prefab on the ground (meadow, farm, forest edge)
2. **Add Component → Ground Animal**
3. Tune:

| Animal | Walk Speed | Run Speed | Wander Radius |
|--------|------------|-----------|---------------|
| Rabbit | `2`–`3` | `5`–`7` | `8`–`15` |
| Deer / dog | `1.5`–`2.5` | `4`–`6` | `15`–`30` |
| Cow / sheep | `0.8`–`1.4` | `2`–`3` | `10`–`20` |

4. **Height Offset** — raise/lower if feet sink or float (try `0` or `0.05`)
5. Optional: assign **Animator** + Speed param if the model has walk anims
6. **Play** — walks, idles, sometimes runs  
   Green gizmo = roam area

**Tip:** model forward (blue Z) should be the nose/face direction. If it walks backward → rotate mesh **Y = 180**.

---

### Setup — herd / pack

1. Create empty **`Herd_Deer`** on a meadow
2. **Add Component → Ground Herd**
3. **Animal Prefab** → your deer/rabbit prefab
4. **Count** = `4`–`8`
5. **Spawn Radius** ≈ `(10, 10)`
6. Play → animals spawn on the ground and wander

---

### Ground Animal vs NPC Wander

| | **Ground Animal** (§19) | **Npc Wander** (§16) |
|--|-------------------------|-------------------------|
| Best for | Wildlife, livestock | Villagers / people |
| Needs NavMesh? | **No** (raycast) | **Yes** (bake) |
| Stays on slopes | Yes (align to slope) | Yes (NavMesh) |

---

### Troubleshooting

| Problem | Fix |
|---------|-----|
| Falls through / floats | Adjust **Height Offset**; ensure Terrain Collider exists |
| Spins / slides | Face mesh forward = blue Z; lower Turn Speed |
| Stands still | Wait for idle timer; check Wander Radius; ground mask includes Terrain |
| Can’t find in Hierarchy | Must be a **placed prefab**, not a painted detail |

---

### Done when
Animals walk around on the grass/dirt without flying or sinking.

---

## Suggested session plan

| Session | Do |
|---------|----|
| 1 | §2 Player + §3 First car + save prefab |
| 2 | §4 more cars; §6 if they feel bad; §6b sounds/lights |
| 3 | §16 Bake NavMesh + 3–5 wandering NPCs in village |
| 4 | World §7 + §8 (terrain, beach, water) |
| 5 | World §9 Forest |
| 6 | World §10 Village |
| 7 | World §11 Roads + bridge |
| 8 | World §12 Sky/polish + more vehicles |

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

**Localhost web docs (beautiful UI):** open folder `DocsSite/` → run `start.bat` → browse http://127.0.0.1:5050  
(Same content as this MD + archives, section-based with search.)


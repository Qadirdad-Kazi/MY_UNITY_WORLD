# Player + Vehicles (current project setup)

This matches what you already did in **MyFantasyWorld**.  
Also see: `HOW_TO_USE_SCRIPTS.md`, `SECTION_BY_SECTION_BUILD_BIBLE.md`

---

# PLAYER — yes, documented (updated)

## What we use in this project

| Piece | Location |
|---|---|
| Character prefab | `Assets/StarterAssets/ThirdPersonController/Prefabs/PlayerArmature` |
| Camera | `MainCamera` + script `CameraFollowTarget` |
| Look pivot | `PlayerArmature/PlayerCameraRoot` |
| Interact (doors/cars) | Add `PlayerInteraction` on PlayerArmature |

## Setup (correct third-person)

1. Drag `PlayerArmature` into scene under `PLAYER`
2. Drag `MainCamera` (StarterAssets prefab) under `PLAYER`
3. Delete `PlayerFollowCamera` if present (Cinemachine not used)
4. Remove any **Missing Script** on MainCamera
5. Add **Camera Follow Target** to MainCamera
6. Target = **PlayerCameraRoot** (expand PlayerArmature to find it)
7. Offset ≈ `(0, 0.5, -4)` — **Z negative = behind** (third person). `(0,0,0)` = first person look
8. Tag PlayerArmature = **Player**
9. Add component **Player Interaction** (for E key on doors/vehicles)
10. Press Play — WASD / mouse / Space / Shift

**Do not** put MainCamera inside the body. Keep it as sibling of PlayerArmature.

---

# VEHICLE SYSTEM — yes, in the MD

Documented in:
- `HOW_TO_USE_SCRIPTS.md` → sections **C Cars, D Bikes, E Planes, F Boats**
- `SECTION_BY_SECTION_BUILD_BIBLE.md` → Sections **15–18**

## Scripts (already in project)

`Assets/MyWorld/Scripts/Vehicles/`

| Script | Use |
|---|---|
| `CarController` | Cars — WheelColliders + **suspension spring/damper/grip** |
| `BikeController` | Motorcycles — 2 wheels |
| `BoatController` | Boats — buoyancy |
| `PlaneController` | Planes — needs a plane model (not in project) |
| `VehicleSeat` | Seat / exit point |
| `VehicleEnterExit` | Press **E** enter/exit (works with StarterAssets player) |

---

# VEHICLES YOU HAVE (import list)

All under `Assets/Game_Models/` unless noted.

| Vehicle | Type | File to drag into scene |
|---|---|---|
| Chevrolet Silverado | Car / truck | `chevrolet-silverado-1500-rst/source/*.glb` |
| Ferrari SF90 | Sports car | `ferrari-sf90-xx-wwwvecarzcom/source/*.glb` |
| Mercedes G-Class | SUV | `mercedes-benz-g-class-amg-g-63/source/.../Mercedes_Benz_g_Class.fbx` |
| UAZ | Off-road | `uaz/source/uaz.fbx` |
| Jeep Gladiator | Truck | Extract zip if needed → `jeep-gladiator/source/` FBX |
| Lada 2107 | Car | Extract zip if needed → `lada-2107/source/` FBX |
| Jawa motorcycle | Bike | `motorcycle-custom-bike-jawa-low-poly/source/yawa.fbx` |
| Naked street bike | Bike | `three-cylinder-naked-street-bike/source/*.fbx` |
| Boats / kayak / jetski | Boat | `Assets/Alstra_Boats/Boats LowPoly/Prefabs/` |
| Plane | Plane | **Not included** — download separately |

Models are **meshes**, not playable until you add physics + our scripts (below).

---

# HOW TO MAKE A CAR PLAYABLE (realistic suspension)

Do once per car, then save as prefab in `Assets/MyWorld/Prefabs/Vehicles/`.

### 1) Place model
1. Drag FBX/GLB into scene
2. Reset scale if huge/tiny
3. Add a **Box Collider** (or mesh collider convex) on the body — **not** on wheels if possible

### 2) Rigidbody
1. Add **Rigidbody**
2. Mass: truck `1600–2000`, car `1200–1400`, sports `1100–1300`
3. Interpolation = Interpolate
4. Collision Detection = Continuous

### 3) Center of mass (important for realism)
1. Create empty child `COM`
2. Place it **low** in the chassis (below center)
3. Will assign on CarController

### 4) WheelColliders (suspension)
1. Create 4 empty children: `WC_FL`, `WC_FR`, `WC_RL`, `WC_RR`
2. Place each at a wheel center
3. Add **Wheel Collider** to each
4. Radius ≈ visual wheel radius (start `0.35`)
5. Suspension Distance ≈ `0.2–0.3`

### 5) Scripts
1. Add **Car Controller**
2. Assign 4 WheelColliders
3. Assign visual wheel transforms if you have separate wheel meshes (optional)
4. Assign **COM**
5. Tune on CarController:
   - Spring `30000–45000`
   - Damper `3500–5500`
   - Suspension Distance `0.25`
6. Add **Vehicle Seat** (place empty at driver seat)
7. Add **Vehicle Enter Exit**
8. Create empty `ExitPoint` beside door → assign on Vehicle Seat

### 6) Save prefab
Drag the finished car into `Assets/MyWorld/Prefabs/Vehicles/Car_Silverado.prefab` (etc.)

### Drive controls
- **E** enter / exit (need PlayerInteraction on player)
- **WASD** drive
- **Space** handbrake

---

# BIKES
Same idea with **Bike Controller** + 2 WheelColliders (front/rear). Mass `200–280`.

# BOATS
Use Alstra prefabs + **Boat Controller**. Set **Water Level Y** to your ocean height. + Seat + EnterExit.

# PLANES
Download a plane model → **Plane Controller** + Seat + EnterExit.

---

# Honest note on “realistic suspension”

Our system uses Unity **WheelColliders** (spring, damper, friction, downforce, handbrake).  
That is the standard indie/prototyping approach and can feel solid if COM + wheel positions are careful.

It is **not** a full BeamNG-level soft-body sim. For that you’d need a specialized vehicle asset later.

---

# Checklist — make all cars playable

- [ ] Unzip Jeep + Lada if still zipped
- [ ] Setup Silverado (first test car)
- [ ] Setup Mercedes, Ferrari, UAZ, Jeep, Lada
- [ ] Setup both bikes
- [ ] Setup 1–2 boats
- [ ] Save each under `MyWorld/Prefabs/Vehicles/`
- [ ] Player has **Player Interaction**
- [ ] Test E enter on flat ground first

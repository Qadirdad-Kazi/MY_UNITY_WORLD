# How to use the scripts (step by step)

Project must be **Unity 6 + URP**.  
Copy `Scripts/` into `Assets/MyWorld/Scripts/` first and wait for compile.

Also create layers/tags:

**Tags:** `Player`  
**Layers (recommended):** `Ground`, `Vehicle`, `Water`, `Interactable`

---

# A) PLAYER

## What this project actually uses (recommended)

| Piece | Where |
|---|---|
| Character | `Assets/StarterAssets/ThirdPersonController/Prefabs/PlayerArmature` |
| Camera | StarterAssets `MainCamera` + `CameraFollowTarget` |
| Look pivot | `PlayerArmature` → child **`PlayerCameraRoot`** |
| Interact | Add `PlayerInteraction` on PlayerArmature |

## Setup player (Starter Assets — do this)

1. Hierarchy create empty `PLAYER`
2. Drag **PlayerArmature** under it
3. Drag **MainCamera** under it (from same Prefabs folder)
4. **Do not** use PlayerFollowCamera (Cinemachine removed for Unity 6 compatibility)
5. On MainCamera: remove any **Missing Script**
6. Add **Camera Follow Target**
7. Expand PlayerArmature → drag **PlayerCameraRoot** into Target
8. Set Offset to `(0, 0.5, -4)` for third-person (`Z=-4` behind). `(0,0,0)` looks first-person
9. Tag PlayerArmature = **Player**
10. Add **Player Interaction** (needed for doors/vehicles E key)
11. Play — WASD, mouse, Space, Shift

## Alternate scripts (capsule walker)
- `PlayerMotor.cs`, `SimpleFollowCamera.cs`, `PlayerSpawnPoint`, `PlayerBootstrap`  
  Use if you prefer a capsule instead of StarterAssets.

## Controls
- **WASD** move  
- **Left Shift** run  
- **Space** jump  
- **Mouse** look  
- **E** interact (doors / vehicles)

## Full guide
See also `VEHICLES_AND_PLAYER.md` in this Docs folder.

---

# B) HOUSE / DOOR

## Scripts
- `HouseDoor.cs`
- `BuildingInteriorVolume.cs`

## Setup enterable house

1. Place a house prefab in the scene
2. Create empty child `Door_Interact`
3. Add a **Box Collider** on it (not required to be trigger)
4. Add **House Door**
5. Create empty `InteriorSpawn` inside house floor
6. Create empty `ExteriorSpawn` outside door
7. Drag both into House Door fields
8. Mode = **Teleport To Interior**
9. Optional: add **Building Interior Volume** with trigger box covering interior

## Setup swinging door only

1. Put House Door on door mesh pivot
2. Mode = **Toggle Door Rotation**
3. Assign `Door Pivot`
4. Press E to open/close

## Assets to download for houses
- Fantasy / medieval village modular pack (URP)
- Medieval props (barrels, chairs) for interiors
- Optional tavern interior pack

---

# C) CARS

## Scripts
- `CarController.cs`
- `VehicleSeat.cs`
- `VehicleEnterExit.cs`

## Setup car (important — do carefully)

1. Drop car model in scene
2. Root object: add **Rigidbody**
   - Mass: `1200`–`1600`
   - Interpolation: Interpolate
   - Collision Detection: Continuous
3. Create empty child `COM` slightly below center → assign as Center Of Mass
4. Create 4 children with **Wheel Collider**:
   - `WC_FL`, `WC_FR`, `WC_RL`, `WC_RR`
   - Place at wheel positions
5. Add **Car Controller** on root → assign 4 WheelColliders + optional visual wheels
6. Add **Vehicle Seat** on root (or seat empty at driver position)
7. Add **Vehicle Enter Exit** on same object as seat/root
8. Create empty `ExitPoint` beside car → assign on Vehicle Seat
9. Layer = Vehicle (optional)

## Play
- Walk near car, look at it, press **E** to enter  
- **WASD** drive  
- **E** exit  

## Assets
- Included possible models in `AssetsToImport` (Jeep, etc.) if copied
- Or download: `driveable car URP`, `wheel collider car`

---

# D) BIKES

## Scripts
- `BikeController.cs`
- `VehicleSeat.cs`
- `VehicleEnterExit.cs`

## Setup bike

1. Bike model + **Rigidbody** mass `200`–`280`
2. Two WheelColliders: front + rear
3. Add **Bike Controller** → assign wheels
4. Add **Vehicle Seat** + **Vehicle Enter Exit**
5. Exit point beside bike

## Controls
Same enter/exit as car. Lean is automatic while turning.

## Assets
- Motorcycle / bike model pack (URP materials)

---

# E) PLANES

## Scripts
- `PlaneController.cs`
- `VehicleSeat.cs`
- `VehicleEnterExit.cs`

## Setup plane

1. Plane model + **Rigidbody**
2. Add **Plane Controller**
3. Add seat + enter/exit
4. Place on flat runway / beach strip

## Controls
- **W** thrust  
- **A/D** yaw  
- **Q/E** roll  
- **R/F** pitch  
- **E** exit  

## Assets to download
- Simple airplane model (URP)
- Optional runway / wind sock props

---

# F) BOATS

## Scripts
- `BoatController.cs`
- + seat / enter-exit

## Setup boat

1. Boat model + Rigidbody
2. Add **Boat Controller**
3. Set `Water Level Y` to your ocean/lake height
4. Add seat + enter/exit

## Assets
- Boat models (low poly boats OK)
- Pier models

---

# G) WORLD HELPERS

## ZoneTrigger
1. Empty object with Box Collider (Is Trigger)
2. Add **Zone Trigger**
3. Set names (`Zone_Beach`, display “Beach”)
4. Scale box to cover zone

## DayNightCycle
1. Empty `DayNight`
2. Add **Day Night Cycle**
3. Assign Directional Light (sun)
4. Adjust `Day Length Minutes`

## AmbientZoneAudio
1. Empty with trigger collider + Audio Source
2. Assign looping ambient clip (waves/wind/birds)
3. Add **Ambient Zone Audio**

## Assets to download for audio/sky
- Skybox pack
- Nature ambience audio (waves, wind, forest, village)

---

# H) Interaction system note

Anything interactable should implement `IInteractable`  
Ready-made:
- `HouseDoor`
- `VehicleEnterExit`

Player must have `PlayerInteraction`.

Temporary on-screen prompt uses OnGUI (fine for setup).  
Replace later with uGUI / UI Toolkit for production UI.

---

# I) Common mistakes

| Problem | Fix |
|---|---|
| Cannot enter car | Need PlayerInteraction + VehicleEnterExit + collider on vehicle |
| Car explodes / flips | Lower center of mass; check WheelCollider positions; reduce torque |
| Bike falls over | Keep COM low; test on flat ground first |
| Plane won’t lift | Hold W for speed; pitch with R/F |
| Boat sinks / flies | Adjust Water Level Y and buoyancy |
| Fall through ground | CharacterController + TerrainCollider; Ground mask |
| Scripts missing | Reimport Scripts folder; wait compile |
| Pink model | URP/Lit materials |

---

# J) Professional order of use

1. Player walk solid  
2. One house door  
3. One car  
4. Zones + audio  
5. Day/night  
6. Bike / boat / plane  
7. Then decorate world using the big world-building guide

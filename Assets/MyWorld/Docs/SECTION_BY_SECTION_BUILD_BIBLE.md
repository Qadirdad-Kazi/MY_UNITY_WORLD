# Section-by-Section Build Bible  
## How to build each part of the world + assets + scripts

Use this with your new **Universal 3D (URP)** project and the folder:

`Downloads\MyFantasyWorld_StarterPack\`

For every section below you get:

1. **What you are building**  
2. **Click steps (go here → do this)**  
3. **How to color / paint the ground**  
4. **Assets you need** (included in pack vs download)  
5. **Scripts to add** (if any) + how to attach them  
6. **Done checklist**

---

# How to import scripts once (do this before Section 1)

1. Create folder in Unity: `Assets/MyWorld/Scripts`
2. Copy all folders from pack `Scripts/` into it (Core, Player, Interaction, Buildings, Vehicles, World)
3. Wait until Unity finishes compiling
4. Create Tag: **Player**  
   (Edit → Project Settings → Tags and Layers)

Full script details also in: `Docs/HOW_TO_USE_SCRIPTS.md`

---

# SECTION 0 — New project + empty scene

### What you are building
A clean URP project and saved world scene.

### Click steps
1. Unity Hub → **Projects → New project**
2. Select **Universal 3D** (URP)
3. Name it `MyFantasyWorld` → Create
4. File → Save As → `Assets/Scenes/MainWorld.unity`
5. Hierarchy empty → Create Empty → name `World`
6. Under World create empties: `TerrainRoot`, `Zones`, `Water`, `Roads`, `Props`, `Lighting`, `Systems`

### Ground / assets / scripts
- Assets: none yet  
- Scripts: none yet  

### Done
- [ ] URP project open  
- [ ] MainWorld scene saved  
- [ ] World folders exist  

---

# SECTION 1 — Terrain (the ground)

### What you are building
Walkable land you can sculpt and paint.

### Click steps
1. Right-click `TerrainRoot` → **3D Object → Terrain**
2. Select Terrain → Inspector → Terrain Settings (gear)
3. Set Width/Length `500`, Height `120`, Heightmap Resolution `513`
4. Press **F** in Scene view to frame it

### How to color ground
Not yet — first shape land in Section 2, then paint in Section 3.

### Assets needed
| Included in pack | Download if missing |
|---|---|
| `AssetsToImport/Game_Terrains` | Extra terrain tools (optional) |
| `AssetsToImport/Game_Textures` | More PBR ground packs |

### Scripts
None on Terrain itself.

### Done
- [ ] Terrain visible  
- [ ] Size set  
- [ ] Scene saved (Ctrl+S)  

---

# SECTION 2 — Hills & mountains (shape the land)

### What you are building
Beach flat, village plateau, forest rise, river trench, mountains.

### Click steps
1. Select Terrain → **Paint Terrain → Raise or Lower Terrain**
2. Brush Size `40–80`, Opacity `0.05–0.2` (keep low)
3. Raise soft hills inland
4. Raise taller mountains at the north
5. Switch to **Smooth Height** and smooth slopes
6. Use **Flatten / Set Height** for beach (low) and village (a bit higher)
7. Use **Lower Terrain** for a winding river trench

### How to color ground
Still later (Section 3). Shape first.

### Assets / scripts
None required.

### Done
- [ ] Beach flat exists  
- [ ] Village flat exists  
- [ ] Mountains readable  
- [ ] River trench carved  

---

# SECTION 3 — Paint / color the ground (textures)

### What you are building
Sand, grass, dirt, forest floor, rock colors on terrain.

### Click steps
1. Copy `AssetsToImport/Game_Textures` into project `Assets/`
2. Select Terrain → **Paint Terrain → Paint Texture**
3. **Edit Terrain Layers → Create Layer** (or Add Layer)
4. Assign Diffuse (and Normal if you have it)
5. Set Layer Size about `10–20`
6. Make layers: Sand, Grass, Dirt, ForestFloor, Rock
7. Paint base grass on meadows
8. Paint sand on beach
9. Paint dirt where paths will go
10. Paint rock on steep mountain sides
11. Soft-blend borders with low opacity

### Assets needed
| Included | Download |
|---|---|
| `Game_Textures`, `Game_Terrains` layers | Snow/mud/swamp textures later |
| Free Island textures (if imported) | Search: `terrain textures PBR` |

### Scripts
None.

### Done
- [ ] At least 4 terrain layers  
- [ ] Beach reads as sand  
- [ ] Mountains have rock  

---

# SECTION 4 — Water (ocean / lake / river)

### What you are building
Water surface at correct height.

### Click steps
1. Right-click `Water` → **3D Object → Plane** → name `OceanWater`
2. Scale large enough for coast
3. Move Y to beach sea level
4. Create Material `M_Water` (URP Lit), blue, high Smoothness → drag onto plane  
   **OR** use prefab from `AQUAS-Lite` / `Phantom_Water`
5. Duplicate for `RiverWater` / `LakeWater` and fit into trenches/bowls

### How to color ground near water
Paint sand/mud at shoreline; blend to grass inland.

### Assets needed
| Included | Download |
|---|---|
| `AQUAS-Lite`, `Phantom_Water` | Better `stylized water URP` (recommended) |

### Scripts
| Script | When |
|---|---|
| `BoatController` | Only if you add boats (Section 16) |
| Set `Water Level Y` on boat to this water height | |

### Done
- [ ] Water touches shoreline  
- [ ] Not floating high above land  

---

# SECTION 5 — Beach zone

### Zone empty
Create `Zones/Zone_Beach`

### Click steps
1. Flatten south strip + sand paint (already partly done)
2. Import pier from `Game_Models/wooden-pier` (if present)
3. Place pier under `Zone_Beach`
4. Place 1–3 boats from `Alstra_Boats`
5. Add a few rocks from `Free_Rocks`
6. Optional palms (download)

### Ground color
Sand near water → blend to grass inland.

### Assets needed
| Included | Download |
|---|---|
| Pier, boats, rocks, sand textures | Palm trees URP, better beach props |

### Scripts
| Script | How |
|---|---|
| `ZoneTrigger` | Empty + Box Collider (Is Trigger) sized over beach → add script → names `Zone_Beach` / `Beach` |
| `AmbientZoneAudio` | Same zone or child → Audio Source with waves loop → add script |
| `BoatController` + enter/exit | On boat (Section 16) |

### Done
- [ ] Beach looks like shore  
- [ ] Path leaving beach inland  

---

# SECTION 6 — Forest zone (paint many trees)

### Zone empty
`Zones/Zone_Forest`

### What you are building
Dense forest by **painting trees**, not placing one-by-one.

### Click steps
1. Import `ALP_Assets`, `Forst`, `Game_RocksAndVegetation`
2. For Forst: open pack → double-click **`Conifers_URP.unitypackage`** → Import
3. Select Terrain → toolbar **Paint Trees**
4. **Edit Trees → Add Tree**
5. Drag a tree prefab into Tree Prefab → Add
6. Add 2–3 tree types (oak/maple + pine)
7. Brush over forest area with medium density
8. Leave openings for paths and clearings
9. Hand-place 2–3 “hero” trees under `Zone_Forest` for special spots

### How to color ground in forest
Paint darker grass / forest floor texture. Less sand. Soft dirt path through trees.

### Assets needed
| Included | Download |
|---|---|
| ALP oak/poplar, Forst conifers, maple/fern pack | Jungle/palm packs for tropical forest |
| BushGrassBend (tool) | Extra bush packs |

### Scripts
| Script | How |
|---|---|
| `ZoneTrigger` | Forest volume trigger |
| `AmbientZoneAudio` | Wind/birds clip |
| No tree script needed | Unity Terrain handles painted trees |

### Done
- [ ] Forest readable from distance  
- [ ] Paths not blocked by trees  
- [ ] Clearings exist  

---

# SECTION 7 — Grass & flowers (meadow)

### Zone empty
`Zones/Zone_Meadow`

### Click steps
1. Terrain → **Paint Details**
2. Edit Details → **Add Grass Texture** (from ALP GrassFlowers) or Detail Mesh (fern/bush)
3. Paint meadow heavily
4. Paint forest lightly
5. Almost no grass on pure sand or steep rock

### Ground color
Bright grass TerrainLayer under meadow.

### Assets needed
| Included | Download |
|---|---|
| ALP GrassFlowers, ferns/bushes | More flower detail packs |

### Scripts
None required (optional meadow ZoneTrigger + birds audio).

### Done
- [ ] Meadow feels soft and open  

---

# SECTION 8 — Rocks & cliffs

### Click steps
1. Import `Free_Rocks` + rock prefabs from vegetation pack
2. Place under `Zone_Mountain` / river banks
3. Rotate (E) and scale (R) randomly
4. Sink slightly into terrain
5. Paint rock TerrainLayer under them

### Assets needed
| Included | Download |
|---|---|
| Free_Rocks, Game stone/desert rocks | Bigger cliff megapack |

### Scripts
None.

### Done
- [ ] No floating rocks  

---

# SECTION 9 — Paths & roads

### Click steps (easiest first)
1. Paint **Dirt** TerrainLayer as a winding path: Beach → Village → Forest → River → Mountain

### Better roads later
1. Import `EasyRoads3D`
2. Create EasyRoads road network (GameObject menu → EasyRoads3D)
3. Shift+click path points
4. Build so road sits on terrain
5. Parent under `Roads`

### Ground color
Dirt under/around road always — even if mesh road exists.

### Assets needed
| Included | Download |
|---|---|
| EasyRoads3D, KajamansRoads | Medieval/dirt road materials |

### Scripts
None required.

### Done
- [ ] You can walk a clear route through the valley  

---

# SECTION 10 — River + bridge

### Zone empty
`Zone_River`

### Click steps
1. River trench + river water already placed
2. Import `3DWorldInSeconds_Bridges`
3. Drag wooden/stone bridge prefab across river
4. Align deck with path height
5. Add rocks under supports
6. Continue dirt path both sides

### Ground color
Rock/moss near water; dirt on approach.

### Assets needed
| Included | Download |
|---|---|
| Bridges pack | Extra fantasy bridges |

### Scripts
| Script | How |
|---|---|
| `ZoneTrigger` | Optional river zone |
| `AmbientZoneAudio` | Water flow sound |

### Done
- [ ] Player can cross bridge  

---

# SECTION 11 — Fantasy village (houses)

### Zone empty
`Zone_Village`

### Click steps
1. Import `3DForge_FantasyExteriors`
2. Flatten village plateau; paint dirt + grass + plaza
3. Place path into village first
4. Drag houses/fences/farm pieces under `Zone_Village`
5. Face doors toward roads
6. Add well/sheds from `Game_Models` if present
7. When a house looks good: drag to `Assets/Prefabs/Buildings/`

### Ground color
Dirt roads, grass yards, optional paving for square.

### Assets needed
| Included | Download |
|---|---|
| 3DForge village kit, sheds/well | Market stalls, interior furniture, more props |

### Scripts — HOUSE DOOR
1. On house, create empty `Door_Interact` at door
2. Add collider
3. Add script **`HouseDoor`**
4. Create `InteriorSpawn` inside and `ExteriorSpawn` outside
5. Assign both on HouseDoor
6. Mode: **Teleport To Interior** (or Toggle Door Rotation for swing doors)
7. Optional: **`BuildingInteriorVolume`** trigger box inside house

Player must have **`PlayerInteraction`** (Section 12) to press **E**.

### Done
- [ ] 3–6 houses placed  
- [ ] At least one door interactable  

---

# SECTION 12 — Player (walk the world)

### What this project uses
- Prefab: `Assets/StarterAssets/.../PlayerArmature`
- Camera: `MainCamera` + **Camera Follow Target**
- Target: `PlayerArmature/PlayerCameraRoot`
- Offset third-person: `(0, 0.5, -4)`
- Add **Player Interaction** for E key

### Click steps
1. Drag PlayerArmature + MainCamera into scene under `PLAYER`
2. Do **not** use PlayerFollowCamera
3. MainCamera → Camera Follow Target → Target = PlayerCameraRoot
4. Offset `(0, 0.5, -4)`
5. Tag = Player; add Player Interaction
6. Play

Full detail: `Assets/MyWorld/Docs/VEHICLES_AND_PLAYER.md`

### Assets needed
| Included | Download |
|---|---|
| StarterAssets PlayerArmature | Optional fantasy hero mesh |

### Scripts used
`CameraFollowTarget`, `PlayerInteraction` (+ StarterAssets ThirdPersonController on prefab)

---

# SECTION 13 — Farm & countryside

### Zone empty
`Zone_Farm`

### Click steps
1. Flatten farm fields near village
2. Paint dirt strips + grass borders
3. Place 3DForge fences, stables, troughs
4. Keep path from village to farm clear

### Ground color
Dirt rows + grass edges.

### Assets needed
| Included | Download |
|---|---|
| 3DForge farm pieces | Crop meshes, farm animals |

### Scripts
Optional `ZoneTrigger`. Animals later (download + place; no required script yet).

### Done
- [ ] Farm readable as farmland  

---

# SECTION 14 — Town square / market

### Click steps
1. Flatten plaza in village center
2. Paint paving/dirt plaza
3. Place well / center prop
4. Place stalls/barrels/crates (download props if missing)

### Assets needed
| Included | Download |
|---|---|
| Partial village props | `medieval props`, `market stall` |

### Scripts
None required. NPCs later (Section 20).

---

# SECTION 15 — Cars

### Click steps
1. Import a car from `Game_Models` (Jeep etc.) or download car model
2. Root: **Rigidbody** mass 1200–1600
3. Add 4 children **WheelCollider** at wheels (`WC_FL/FR/RL/RR`)
4. Add **`CarController`** → assign wheel colliders (+ visual wheels optional)
5. Add empty COM below center → assign Center Of Mass
6. Add **`VehicleSeat`** + **`VehicleEnterExit`**
7. Create `ExitPoint` beside car → assign on seat
8. Play: walk up, press **E**, drive WASD, **E** exit

### Ground color
Keep roads dirt/gravel under car routes.

### Assets needed
| Included | Download |
|---|---|
| Game car models (meshes) | Cleaner wheel-ready cars if needed |

### Scripts
`CarController`, `VehicleSeat`, `VehicleEnterExit`  
Requires player scripts from Section 12.

### Done
- [ ] Enter/drive/exit works on flat ground  

---

# SECTION 16 — Bikes

### Click steps
1. Place bike model from `Game_Models` or download
2. Rigidbody mass 200–280
3. 2 WheelColliders (front/rear)
4. Add **`BikeController`** + Seat + EnterExit
5. Test on flat ground first

### Assets / scripts
Bike model + `BikeController` + enter/exit scripts.

---

# SECTION 17 — Planes

### Click steps
1. **Download an airplane model** (usually not in pack)
2. Add Rigidbody + **`PlaneController`** + Seat + EnterExit
3. Place on flat beach/runway strip
4. Controls: W thrust, A/D yaw, Q/E roll, R/F pitch, E exit

### Assets needed
| Included | Download |
|---|---|
| Scripts only | Airplane model URP (required) |

---

# SECTION 18 — Boats

### Click steps
1. Place boat from `Alstra_Boats`
2. Add Rigidbody + **`BoatController`**
3. Set **Water Level Y** = your ocean/lake Y
4. Add Seat + EnterExit
5. Test near pier

### Assets / scripts
Boats included + `BoatController`.

---

# SECTION 19 — Lakes, waterfall, cave mouth

### Lakes
Lower a bowl → lake water plane → mud/rock shoreline paint.

### Waterfall
Cliff shelf + waterfall VFX (download) + water audio.

### Cave
Rock mouth arrangement under `Zone_Cave` (full cave mesh later/download).

### Scripts
ZoneTrigger + AmbientZoneAudio (water/wind).

### Download
Waterfall VFX, cave pack.

---

# SECTION 20 — Life: audio, day/night, zones, NPCs

### Day / night
1. Empty `DayNight` under Systems
2. Add **`DayNightCycle`**
3. Assign Directional Light

### Zone audio
1. On each zone trigger object add Audio Source (loop clip)
2. Add **`AmbientZoneAudio`**
3. Use waves/wind/birds clips (**download audio packs**)

### Zone names
Add **`ZoneTrigger`** boxes for Beach/Village/Forest/Mountain.

### NPCs / animals
Download villagers/birds/animals → place under zones (no AI required at first).

### Torches / campfires
Download torch/campfire VFX → place + Point Light for night.

### Assets to download
Nature ambience audio, rain VFX, villagers, birds, torch/campfire.

### Scripts
`DayNightCycle`, `AmbientZoneAudio`, `ZoneTrigger`

---

# SECTION 21 — Extra biomes (later)

| Biome | Ground color | Assets | Scripts |
|---|---|---|---|
| Snow peaks | Snow layer on tops | Snow textures + pines | ZoneTrigger |
| Desert | Sand + sparse rock | Desert rocks (included partial) | ZoneTrigger |
| Swamp | Mud + moss + shallow water | Swamp trees (download) | Fog volume + audio |
| Jungle | Dark ground + dense paint trees | Jungle/palm download | Zone audio |

---

# SECTION 22 — Sky, sun, fog (look)

### Click steps
1. Import `AllSkyFree`
2. Window → Rendering → Lighting → Environment → Skybox Material
3. Rotate Directional Light for nice sun angle
4. Lighting empty → Volume → Global Volume → add Fog / Bloom (subtle)

### Assets
AllSkyFree included. Optional more skies.

### Scripts
Works with `DayNightCycle`.

---

# MASTER ORDER (do in this sequence)

1. Section 0 Project  
2. Section 1–3 Terrain + paint  
3. Section 4–5 Water + beach  
4. Section 12 Player (so you can walk while building)  
5. Section 9 Path  
6. Section 11 Village + HouseDoor script  
7. Section 6–8 Forest, grass, rocks  
8. Section 10 Bridge  
9. Section 13–14 Farm + market  
10. Section 15–18 Vehicles you care about  
11. Section 19–22 Life + sky + biomes  

---

# Quick script cheat sheet by section

| Section | Scripts to add |
|---|---|
| Terrain / paint / trees / grass / rocks / roads | None |
| Beach / forest / river zones | `ZoneTrigger`, `AmbientZoneAudio` |
| House | `HouseDoor`, optional `BuildingInteriorVolume` |
| Player | `PlayerMotor`, `PlayerInteraction`, spawn/bootstrap, camera |
| Car | `CarController` + `VehicleSeat` + `VehicleEnterExit` |
| Bike | `BikeController` + seat + enter/exit |
| Plane | `PlaneController` + seat + enter/exit |
| Boat | `BoatController` + seat + enter/exit |
| Day/night | `DayNightCycle` |
| Whole world feel | Zone + audio scripts on each biome |

---

# If something is missing

Open also:

- `Docs/HOW_TO_USE_SCRIPTS.md` — deeper script setup  
- `Docs/ASSETS_INCLUDED_AND_DOWNLOAD.md` — full download list  
- `Docs/FROM_SCRATCH_WORLD_BUILDING_GUIDE.md` — longer beginner theory  
- `HowToImport/IMPORT_ORDER.txt` — import order for art packs  

**Rule:** Each section = build steps + ground color + assets + scripts.  
Finish one section’s checklist before jumping ahead.

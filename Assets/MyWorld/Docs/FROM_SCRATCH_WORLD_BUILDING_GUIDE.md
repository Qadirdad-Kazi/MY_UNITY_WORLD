# Build a Professional Fantasy World in Unity  
## Complete Beginner Guide — From Zero to First World

**Who this is for:** You are new to Unity. You are deleting your old project and starting clean.  
**Goal:** Build a professional-looking fantasy open world, one clear step at a time.  
**Rule:** Do each step in order. Do not skip. If something looks wrong, use the **Stuck?** boxes.

---

# TABLE OF CONTENTS

**Foundation**
1. [Before you start](#section-1--before-you-start)
2. [Download and install Unity](#section-2--download-and-install-unity)
3. [Create the correct project](#section-3--create-the-correct-project)
4. [Learn the Unity screen](#section-4--learn-the-unity-screen-5-minutes)
5. [Save your first empty world scene](#section-5--save-your-first-empty-world-scene)

**Land & nature**
6. [Create terrain (the ground)](#section-6--create-terrain-the-ground)
7. [Shape hills and mountains](#section-7--shape-hills-and-mountains)
8. [Paint ground textures](#section-8--paint-ground-textures-sand-grass-rock)
9. [Add water](#section-9--add-water-lake--ocean--river-look)
10. [Make a beach](#section-10--make-a-beach)
11. [Paint many trees (forest)](#section-11--paint-many-trees-forest)
12. [Paint grass](#section-12--paint-grass)
13. [Add rocks and cliffs](#section-13--add-rocks-and-cliffs)
14. [Roads / paths](#section-14--build-simple-roads--paths)
15. [Bridge over a river](#section-15--add-a-bridge)
16. [Fantasy village](#section-16--build-a-fantasy-village)
17. [Sky, sun, and fog](#section-17--sky-sun-and-fog-make-it-look-real)
18. [Walkable player](#section-18--add-a-player-so-you-can-walk)
19. [Organize zones](#section-19--organize-zones-like-a-professional)

**Living world (like a real world — everything)**
20. [Lakes, ponds, waterfalls](#section-20--lakes-ponds--waterfalls)
21. [Caves and mountain passes](#section-21--caves-and-mountain-passes)
22. [Farms and countryside](#section-22--farms-and-countryside)
23. [Town square, market, shops, interiors](#section-23--town-square-market-shops--interiors)
24. [Castle, ruins, and landmarks](#section-24--castle-ruins-and-landmarks)
25. [More biomes: snow, desert, swamp, jungle](#section-25--more-biomes-snow-desert-swamp-jungle)
26. [Day and night cycle](#section-26--day-and-night-cycle)
27. [Weather: rain, fog, wind, storms](#section-27--weather-rain-fog-wind-storms)
28. [Wildlife, birds, fish](#section-28--wildlife-birds-fish)
29. [People / NPCs (simple life)](#section-29--people--npcs-simple-life)
30. [Sounds of the world (audio)](#section-30--sounds-of-the-world-audio)
31. [Campfires, torches, night lights](#section-31--campfires-torches-night-lights)
32. [Signs, waypoints, travel feel](#section-32--signs-waypoints-travel-feel)
33. [Borders, walls, watchtowers](#section-33--borders-walls-watchtowers)
34. [Shore life and underwater look](#section-34--shore-life-and-underwater-look)
35. [Make the world feel alive (wind, particles, ambience)](#section-35--make-the-world-feel-alive)
36. [Bigger world later (expand without breaking)](#section-36--bigger-world-later)

**Assets, plan, help**
37. [What assets you need (full download list)](#section-37--what-assets-you-need-full-download-list)
38. [Where to get free assets safely](#section-38--where-to-get-free-assets-safely)
39. [Week-by-week plan (living world)](#section-39--week-by-week-plan-living-world)
40. [If you get stuck (fix list)](#section-40--if-you-get-stuck-fix-list)
41. [Professional rules](#section-41--professional-rules-do-not-break-these)
42. [Full living-world checklist](#section-42--full-living-world-checklist)

---

# SECTION 1 — Before you start

## 1.1 What you will build (a living world — like a real world)

A real world is not only ground and trees. It has **places**, **travel**, **weather**, **life**, **sound**, and **reasons to explore**.  
Your fantasy world should feel the same — even if you add pieces step by step.

### A) Land & nature

- Ground you can walk on (**Terrain**)
- Beach + ocean shoreline
- Soft hills and tall **mountains**
- Mountain passes and cliffs
- Dense **forest** (many trees painted at once)
- Meadows and wild flowers
- Rocks, boulders, and cliff faces
- River with a **bridge**
- Lakes and small ponds
- Waterfalls from mountain to river
- Caves (simple entrances first)
- Optional later biomes: **snow peaks**, **desert**, **swamp**, **jungle**

### B) Human places (civilization)

- Dirt **paths / roads** connecting everywhere
- Small fantasy **village** (homes, fences, well)
- Farm fields + barns / stables
- Village **market / town square**
- Shop / tavern exterior (and simple interiors later)
- Watchtowers / wooden walls / gates
- Ruins of an old kingdom
- Castle or fortress landmark (later)
- Harbor / pier with boats
- Camp sites along travel roads

### C) Travel & exploration

- Clear route: Beach → Village → Forest → River → Mountains
- Signs / waypoints / landmarks you can see from far away
- Bridge crossings and mountain trails
- Secret side paths and clearings
- Places that reward exploration (ruins, cave mouth, waterfall pool)

### D) Sky, time & weather

- Day sky + sun + soft shadows
- Distance fog (makes world feel big)
- **Day / night cycle** (sun moves, night gets darker)
- Night lights (torches, windows, campfires)
- Weather moods: clear, cloudy, rain, heavy fog, wind
- Optional storm look later

### E) Life in the world

- A character **you can walk with**
- Birds / ambient wildlife
- Fish near shore or river (even as simple moving props first)
- Farm animals later (chickens, sheep, horses — optional)
- Simple **NPCs** standing / walking in village (life, not full AI yet)
- Later: quests, dialogue, inventory (after the land feels real)

### F) Sound & atmosphere

- Ocean waves at beach
- Wind in mountains / forest
- Birds in meadow
- River / waterfall water sound
- Village ambience (soft chatter / blacksmith later)
- Footsteps and environment volume balance

### G) Polish that makes it feel “real”

- Nothing floating (props sit in ground)
- Paths stay clear of trees
- One art style per zone
- Named, organized Hierarchy (`Zone_Beach`, `Zone_Village`…)
- Lighting that matches time of day
- Performance that still runs smoothly while you walk

### Build order (important)

Do **not** try to build all of this on day one.

| Phase | Build these first |
|---|---|
| **Phase 1 — Land** | Terrain, beach, water, hills, mountains, forest, path |
| **Phase 2 — Places** | Village, farm, bridge, pier, ruins |
| **Phase 3 — Life** | Player walk, sky/fog, sounds, animals, simple NPCs |
| **Phase 4 — Living systems** | Day/night, weather, lights, more biomes |
| **Phase 5 — Gameplay** | Quests, inventory, combat, vehicles (after world feels good) |

> Think like a real planet: geography first, then towns, then weather and life, then stories.

## 1.2 What you need on your PC

| Need | Minimum | Better |
|---|---|---|
| Windows | Windows 10/11 64-bit | Same |
| RAM | 16 GB | 32 GB |
| Disk space | 40 GB free | 80 GB+ SSD |
| GPU | Dedicated GPU if possible | NVIDIA / AMD with recent drivers |

## 1.3 Words you will see often

| Word | Simple meaning |
|---|---|
| **Scene** | One level / one world map |
| **Hierarchy** | List of objects in the scene (left side) |
| **Inspector** | Settings of the selected object (right side) |
| **Project** | All your files (bottom) |
| **Prefab** | A saved object you can drag into the world many times |
| **Terrain** | Special Unity ground you can sculpt and paint |
| **Material** | The “skin” (color / texture) of an object |
| **URP** | Universal Render Pipeline — the modern graphics setup we will use |
| **Asset** | Any file: tree, house, texture, sound |

## 1.4 Delete the old project first (if you decided to)

1. Close Unity completely.
2. In File Explorer, go to your old project folder.
3. Delete the whole folder (or move it to another drive as backup).
4. Do **not** keep half-broken prototype scenes. Fresh start is cleaner.

---

# SECTION 2 — Download and install Unity

## Step 2.1 — Download Unity Hub

1. Open your browser.
2. Go to: **https://unity.com/download**
3. Click **Download for Windows** (Unity Hub).
4. Run the installer.
5. Finish install and open **Unity Hub**.

> **Stuck?** If Windows asks permission, click **Yes**.  
> If Hub asks you to sign in, create a free Unity account and sign in.

## Step 2.2 — Install a Unity Editor (the engine)

1. In Unity Hub, click **Installs** (left side).
2. Click **Install Editor**.
3. Choose a recent **Unity 6** version (or newest LTS if Unity 6 is not shown).
4. Click **Install**.
5. When modules appear, turn **ON**:
   - **Microsoft Visual Studio Community** (or VS Code) — for scripts later
   - **Windows Build Support (IL2CPP)** — useful later
6. Click **Continue / Install** and wait.

> **Stuck?** Install can take 30–90 minutes. Do not close Hub.  
> If it fails, reopen Hub → Installs → retry the same version.

## Step 2.3 — Personal / free license

1. In Hub, open your account / preferences if asked.
2. Activate a **Personal** license (free for beginners / small projects).

---

# SECTION 3 — Create the correct project

This choice matters. Wrong template = pink materials and confusion later.

## Step 3.1 — New project

1. Open **Unity Hub**.
2. Click **Projects**.
3. Click **New project**.

## Step 3.2 — Which template should you select?

Select this:

### **Universal 3D**  
(also called **3D (URP)** on some Hub versions)

| Option you might see | Select it? | Why |
|---|---|---|
| **Universal 3D / 3D (URP)** | **YES — choose this** | Best for games + Asset Store packs today |
| 3D (Built-in) | No | Older pipeline |
| High Definition 3D (HDRP) | No | Harder, heavier, many free assets break |
| 2D | No | This is a 3D world |
| VR / Mobile templates | No | Not for this guide |

## Step 3.3 — Project settings

1. **Project name:** `MyFantasyWorld` (or any clear name)
2. **Location:** choose a short path, example:  
   `D:\Unity\MyFantasyWorld`  
   Avoid very long paths and special characters.
3. Click **Create project**.
4. Wait until Unity opens the Editor.

> **Stuck?** First open can take several minutes. Wait until the bottom progress bar finishes.  
> If Unity is black/empty, wait. If still stuck, restart Unity Hub + Unity.

---

# SECTION 4 — Learn the Unity screen (5 minutes)

You must know these 4 windows:

```
+------------------+---------------------------+------------------+
| Hierarchy        | Scene view (3D world)     | Inspector        |
| (objects list)   |                           | (settings)       |
+------------------+---------------------------+------------------+
| Project (your files)                         | Console (errors) |
+------------------------------------------------------------------+
```

## Mouse controls in Scene view

| Action | How |
|---|---|
| Look around | Right-mouse hold + move mouse |
| Move | Right-mouse hold + W A S D |
| Up / Down | Right-mouse hold + E / Q |
| Zoom | Mouse wheel |
| Select object | Left click object |
| Move object | Select object → press **W** → drag arrows |
| Rotate object | Press **E** |
| Scale object | Press **R** |

## Important top buttons

- **Play** (triangle): test the game
- Always click Play again to **stop** before editing seriously

---

# SECTION 5 — Save your first empty world scene

## Step 5.1 — Create folders (professional habit)

1. In **Project** window, click `Assets`.
2. Right-click empty space → **Create → Folder**.
3. Make these folders one by one:

```
Assets/
  Scenes/
  Art/
  Prefabs/
  Materials/
  Textures/
  Audio/
  Scripts/
  Docs/
```

## Step 5.2 — Save the scene

1. Menu: **File → Save As...**
2. Go into `Assets/Scenes/`
3. Name it: `MainWorld`
4. Click **Save**

## Step 5.3 — Clean default objects (optional but clean)

In Hierarchy you may see:

- Main Camera
- Directional Light

Keep **Directional Light** (this is your sun).  
You can keep Main Camera for now (player camera comes later).

## Step 5.4 — Create empty world organizers

1. Hierarchy empty space → Right-click → **Create Empty**
2. Name it `World`
3. Right-click `World` → Create Empty children named:

```
World
  TerrainRoot
  Zones
  Water
  Roads
  Props
  Lighting
  Systems
```

Drag **Directional Light** under `Lighting`.

> **Why?** Professionals keep Hierarchy clean. Later you can hide one zone while editing another.

---

# SECTION 6 — Create terrain (the ground)

Terrain = sculptable ground for mountains, beaches, forests.

## Step 6.1 — Add Terrain

1. Hierarchy: right-click `TerrainRoot`
2. **3D Object → Terrain**
3. You should see a big flat ground in Scene view

## Step 6.2 — Set terrain size (do this before sculpting a lot)

1. Click the Terrain object in Hierarchy.
2. In Inspector, find the Terrain component toolbar icons.
3. Click the **gear** icon (**Terrain Settings**).
4. Find **Mesh Resolution** / terrain size settings.
5. Set starter values:

| Setting | Value for learning | Meaning |
|---|---|---|
| Terrain Width | `500` | Size in meters (X) |
| Terrain Length | `500` | Size in meters (Z) |
| Terrain Height | `120` | Max mountain height |
| Heightmap Resolution | `513` | Detail of hills |

> **Do not start with 4000×4000.** Big maps are slow and confusing for beginners.

## Step 6.3 — Move view so you can see it

1. Click Terrain in Hierarchy.
2. Move mouse over Scene view.
3. Press **F** (Frame Selected).
4. Zoom out with mouse wheel until you see the whole ground.

> **Stuck?** If Terrain is invisible: check Scene view is not in 2D mode (button near Scene tab).  
> If everything is gray, wait for lighting to bake/load, or just continue — sculpting still works.

---

# SECTION 7 — Shape hills and mountains

## Step 7.1 — Open Raise/Lower tool

1. Select Terrain.
2. In Inspector Terrain toolbar, click **Paint Terrain** (brush icon).
3. In the dropdown, choose **Raise or Lower Terrain**.

## Step 7.2 — Brush settings (start soft)

| Setting | Starter value |
|---|---|
| Brush Size | `40`–`80` |
| Opacity | `0.05`–`0.2` (low!) |

**Low opacity is professional.** High opacity makes ugly spikes.

## Step 7.3 — Make a simple world shape (follow this map)

Imagine your 500×500 terrain like this:

```
NORTH (back)
+---------------------------+
|      MOUNTAINS            |
|                           |
|   FOREST     RIVER        |
|                           |
|   MEADOW     VILLAGE      |
|                           |
|         BEACH             |
|      (near water)         |
+---------------------------+
SOUTH (front / player start)
```

### Do this in order

1. **Beach flat area (south):**  
   Switch tool to **Set Height** or **Flatten Height**.  
   Choose a low height (example `8`–`12`).  
   Paint a flat strip at the south edge.

2. **Village plateau:**  
   Slightly higher than beach (example beach 10, village 14–18).  
   Flatten a square area.

3. **Meadow / forest rise:**  
   Use Raise with low opacity. Make soft hills inland.

4. **Mountains (north):**  
   Raise taller hills. Then switch to **Smooth Height** and smooth the sides so slopes look natural.

5. **River trench:**  
   Use Lower Terrain to dig a soft winding line between forest and village.  
   Do not dig a vertical canyon yet.

## Step 7.4 — Mountain tips that look professional

- Build mountains in **layers** (pass 1 soft, pass 2 taller, pass 3 peaks).
- Always **Smooth** after raising.
- Leave one side walkable (player path).
- Keep the highest peak readable from the village (silhouette).

> **Stuck?**  
> - Tool does nothing → make sure Terrain is selected and you click on the terrain surface.  
> - Mountains too spiky → Opacity lower + Smooth Height.  
> - Accidentally ruined area → use Flatten / Smooth to repair. Save scene often: **Ctrl+S**.

---

# SECTION 8 — Paint ground textures (sand, grass, rock)

Right now terrain is one plain color. We paint layers: sand, grass, dirt, rock.

## Step 8.1 — You need texture images first

If you have **no textures yet**, do this temporary method:

### Temporary built-in look (works immediately)

1. Select Terrain → Paint Terrain → **Paint Texture**.
2. Click **Edit Terrain Layers → Create Layer**.
3. Unity creates a TerrainLayer.
4. For now, you can leave default or assign any diffuse texture you import later.

### Better: import free ground textures

Download free PBR ground textures (see Section 20) and put them in:

`Assets/Textures/Ground/`

For each ground type you want:

1. Create Layer
2. Assign **Diffuse** (Albedo/Color) map
3. Assign **Normal Map** if you have one
4. Set **Size** (tiling), try `10` to `20`

## Step 8.2 — Recommended first 5 layers

| Layer | Use where |
|---|---|
| Sand | Beach |
| Grass | Meadow + village yards |
| Dirt | Paths / farm |
| Forest Floor / dark grass | Forest |
| Rock | Mountain slopes |

## Step 8.3 — How to paint like a pro

1. Paint **one base** layer over a whole zone first (example: all meadow = grass).
2. Softly blend edges to next zone (sand → grass → dirt → rock).
3. Paint rock on **steep** mountain sides.
4. Paint dirt where future roads will go.

> **Stuck?**  
> - Cannot paint texture → create at least one Terrain Layer first.  
> - Texture looks stamped/repeating → increase Layer Size.  
> - Texture blurry → decrease Layer Size.

Save: **Ctrl+S**

---

# SECTION 9 — Add water (lake / ocean / river look)

Unity does not give a perfect ocean by default in every template. Beginners should start simple.

## Option A — Fast starter water (plane + material) — do this first

### Step 9.1 — Create water plane

1. Right-click `Water` in Hierarchy → **3D Object → Plane**
2. Rename to `OceanWater`
3. Press **R** and scale it large (example Scale X=50, Z=50, or bigger until it covers beach front)
4. Press **W** and move Y height to your beach sea level (example Y = 10 if beach height is ~10)

### Step 9.2 — Make it look like water

1. Project → `Assets/Materials` → Right-click → **Create → Material**
2. Name: `M_Water`
3. Select `M_Water`
4. In Inspector:
   - Shader should be **Universal Render Pipeline / Lit** (or URP Lit)
   - Base Map color: blue
   - Smoothness: high (0.8–0.95)
   - Optionally lower alpha / use Transparent surface if available
5. Drag `M_Water` onto `OceanWater`

This is **not** final AAA water. It is a clean placeholder so you can design the shoreline now.

## Option B — Better water asset (recommended soon)

Install a **URP water** asset from Unity Asset Store (see Section 20).  
Then replace the plane with that prefab at the same height.

## River water

1. Duplicate your water object (`Ctrl+D`)
2. Rename `RiverWater`
3. Scale it into a long thin shape in the river trench
4. Adjust height to sit in the trench

> **Stuck?**  
> - Water is under ground → raise water Y a little.  
> - Water is pink → material shader is wrong. Select material → set shader to **URP/Lit**.  
> - Player will walk on water later unless you add logic — for now this is visual only.

---

# SECTION 10 — Make a beach

## Step 10.1 — Shape

1. Flatten south strip (already done in Section 7).
2. Paint **Sand** on that strip.
3. Blend sand into grass inland (soft brush, low opacity).

## Step 10.2 — Add beach props (after you download assets)

Under `Zones` create empty `Zone_Beach`, then add:

- Wooden pier
- 1–3 boats
- A few rocks
- Optional palm trees (if you downloaded tropical pack)

## Step 10.3 — Professional beach checklist

- [ ] Flat sand near water
- [ ] Soft slope inland
- [ ] Water touches sand (no huge gap)
- [ ] Not too many objects on wet sand
- [ ] Clear path from beach to village

---

# SECTION 11 — Paint many trees (forest)

This is how you place hundreds of trees without dragging one by one.

## Step 11.1 — Get at least one tree prefab

Download a **URP tree pack** (Section 20).  
Put prefabs somewhere like `Assets/Art/Trees/`.

## Step 11.2 — Add tree to Terrain painter

1. Select Terrain.
2. Terrain toolbar → **Paint Trees**
3. Click **Edit Trees → Add Tree**
4. Drag your tree prefab into **Tree Prefab**
5. Click **Add**

## Step 11.3 — Paint the forest

1. Set Brush Size medium/large.
2. Set Tree Density medium (not maximum).
3. Paint over forest zone only.
4. Leave clearings and path openings.

## Step 11.4 — Hero trees (special trees)

For village square or story spots:

1. Drag one tree prefab into Hierarchy under `Zone_Village` or `Zone_Forest`
2. Place by hand
3. Scale slightly different from painted trees

**Rule:** Paint 90% of forest. Hand-place 10% special trees.

> **Stuck?**  
> - Trees pink → tree materials not URP. Fix materials to URP/Lit or import the pack’s URP package.  
> - No trees appear → confirm Add Tree was done and you are using **Paint Trees** (not Paint Details).  
> - Trees floating → adjust height settings or move hand-placed prefabs down.

---

# SECTION 12 — Paint grass

## Step 12.1 — Open Paint Details

1. Select Terrain.
2. Toolbar → **Paint Details**
3. Edit Details → **Add Grass Texture** (billboard grass)  
   or **Add Detail Mesh** (fern/bush mesh)

## Step 12.2 — Paint

- Meadow: more grass
- Forest: less grass + some ferns
- Beach sand: almost no grass
- Steep rock: no grass

> **Stuck?** Grass disappears far away — normal. Terrain details fade with distance.

---

# SECTION 13 — Add rocks and cliffs

## Step 13.1 — Download rock prefabs (Section 20)

## Step 13.2 — Place professionally

1. Create empty `Zone_Mountain` under `Zones`
2. Drag rock/cliff prefabs under it
3. Place large rocks on ridges and river banks
4. Rotate each rock (`E`) and scale randomly (`R`) a little
5. Sink rocks slightly into terrain so they do not float

Also paint **Rock** terrain texture under cliff areas.

---

# SECTION 14 — Build simple roads / paths

You have 3 beginner-friendly options.

## Option A — Paint a dirt path on Terrain (easiest, start here)

1. Paint Texture tool
2. Use Dirt layer
3. Paint a winding line: Beach → Village → Forest → River → Mountain

This alone already looks like a real path when trees and grass respect it.

## Option B — EasyRoads3D (best real road tool)

1. Install **EasyRoads3D** from Asset Store (Free version is OK to learn)
2. Create a Road Network from the EasyRoads menu  
   (usually around **GameObject → 3D Object → EasyRoads3D**)
3. Add road points with **Shift + Click** in Scene
4. Build/adjust so road sits on terrain
5. Put network under `Roads`

## Option C — Road mesh packs

Some packs give long finished road models. Drop them in and align by hand.  
Good for highways. Less good for fantasy village paths.

### Fantasy tip

For fantasy worlds, prefer **dirt / gravel paths**, not modern asphalt highways.

---

# SECTION 15 — Add a bridge

## Step 15.1 — Prepare crossing

1. Make sure river trench exists.
2. Water is in trench.
3. Path arrives at both sides.

## Step 15.2 — Place bridge

1. Download a bridge pack (stone/wood fantasy bridge).
2. Drag bridge prefab under `Zone_River` (create this empty if needed).
3. Move/rotate so deck matches path height.
4. Check player can walk across (after player is added).

## Step 15.3 — Finish the crossing

- Rocks under supports
- Trees back away from bridge entrance
- Path continues on both sides

---

# SECTION 16 — Build a fantasy village

## Step 16.1 — Get modular fantasy houses

Best type of pack: **Fantasy Village / Medieval Village modular kit**  
(Example style: stone+wood houses, fences, farm pieces)

## Step 16.2 — Village workflow (do in this order)

1. Flatten village plateau.
2. Paint dirt + grass + small plaza.
3. Draw path into village first.
4. Place 3–6 houses facing the path.
5. Add fence, well, shed, farm troughs.
6. Add 2–4 trees at edges only.
7. Leave empty space for future NPCs / quests.

## Step 16.3 — Make reusable prefabs

When one house looks good:

1. Select house root object
2. Drag it into `Assets/Prefabs/Buildings/`
3. Reuse copies in the village

> **Professional rule:** One architecture style per zone.  
> Do not mix modern city + Korean town + tropical mansion + medieval cottages in the same village.

---

# SECTION 17 — Sky, sun, and fog (make it look real)

## Step 17.1 — Sun

1. Select Directional Light under `Lighting`
2. Rotate it (`E`) to a nice late-morning angle
3. Enable Soft Shadows if available
4. Intensity around `1` to `1.5`

## Step 17.2 — Skybox

1. Menu: **Window → Rendering → Lighting**
2. Open **Environment** tab
3. Assign a **Skybox Material**
4. Download a free sky pack if default sky is weak

## Step 17.3 — Fog / volume (URP)

1. Hierarchy → Right-click `Lighting` → **Volume → Global Volume**
2. Add overrides such as:
   - Fog
   - Bloom (very low)
   - Color Adjustments
3. Keep effects subtle

Fog helps mountains feel far away.

---

# SECTION 18 — Add a player so you can walk

## Step 18.1 — Easiest starter player

1. Open **Window → Package Manager**
2. Or use Asset Store / Starter Assets:
   - Search **Starter Assets - ThirdPerson** (Unity official)
3. Import into project
4. Drag **PlayerArmature** (or similar prefab) into scene near beach/village
5. Add the follow camera prefab from the same pack
6. Press **Play** and walk your path

## Step 18.2 — Ground check note

If player falls through world:

- Terrain must have **Terrain Collider**
- Player ground layers must include Default/Terrain

> **Stuck?**  
> - Two cameras fighting → disable extra cameras on imported models.  
> - Player floats → check collider and grounded layers.  
> - Controls inverted / missing → read the Starter Assets input notes (New Input System).

---

# SECTION 19 — Organize zones like a professional

Under `World/Zones`, create empties for a full living world:

```
Zones
  Zone_Beach
  Zone_Harbor
  Zone_Meadow
  Zone_Farm
  Zone_Village
  Zone_Forest
  Zone_River
  Zone_Lake
  Zone_Waterfall
  Zone_Mountain
  Zone_Cave
  Zone_Ruins
  Zone_Castle        (later)
  Zone_SnowPeaks     (later)
  Zone_Desert        (later)
  Zone_Swamp         (later)
  Zone_Jungle        (later)
```

Put each zone’s props as children of that zone.

### Recommended build order (full world)

1. Beach + Harbor  
2. Meadow path  
3. Farm  
4. Village  
5. Forest  
6. River + bridge  
7. Lake + waterfall  
8. Mountain + cave mouth  
9. Ruins landmark  
10. Later biomes (snow / desert / swamp / jungle)  
11. Day/night + weather + wildlife + NPCs  

Finish one zone’s blockout before decorating the next.

---

# SECTION 20 — Lakes, ponds & waterfalls

## 20.1 Lake / pond

1. In Terrain, use **Lower Terrain** to dig a soft bowl (not a perfect circle).
2. Smooth the edges.
3. Paint mud/rock near the waterline, grass farther away.
4. Duplicate your water object → rename `LakeWater`.
5. Scale and place it inside the bowl at the correct height.
6. Put under `Zone_Lake`.

## 20.2 Waterfall (beginner method)

1. Raise a cliff shelf above the river/lake.
2. Place a thin water plane or waterfall VFX prefab falling down the cliff.
3. Put splash/foam particle (from a VFX pack) at the bottom.
4. Add loud water audio only near the fall (Section 30).
5. Put under `Zone_Waterfall`.

## 20.3 Professional tips

- Lakes need **irregular** edges.
- Leave a small beach/rock access so player can walk to shore.
- One great waterfall is better than five weak ones.

> **Stuck?** Water not filling lake → water Y is wrong; raise/lower carefully until shoreline looks wet.

---

# SECTION 21 — Caves and mountain passes

## 21.1 Mountain pass (easy, do this first)

1. Between two mountain peaks, lower a path.
2. Smooth it so player can walk.
3. Paint rock on walls, dirt on path.
4. Add a few rocks beside the trail (not blocking).

## 21.2 Cave entrance (simple starter)

True cave systems can be advanced. Start with an **entrance fake**:

1. Place large rock/cliff pieces to form a dark mouth.
2. Optional: put a dark tunnel mesh or rock hallway prefab behind it.
3. Add a torch outside (Section 31).
4. Later you can build a full interior scene/cave mesh.

Put under `Zone_Cave`.

## 21.3 When to build full caves

After your outdoor valley already feels good. Caves are easy to get lost in while learning.

---

# SECTION 22 — Farms and countryside

A living world has food places, not only adventure forests.

## 22.1 Shape the farm

1. Flatten a rectangle near village (`Zone_Farm`).
2. Paint dirt strips for fields + grass borders.
3. Leave a dirt path from village to farm.

## 22.2 Place farm pieces

Download / use:

- Fences
- Stables / barn
- Troughs
- Well
- Carts / hay piles (props pack)

## 22.3 Make fields readable

- Straight fence lines look intentional.
- Crops can start as grass detail of different color, or crop mesh prefabs later.
- Do not overcrowd — leave walk space.

---

# SECTION 23 — Town square, market, shops & interiors

## 23.1 Town square

1. In village center, flatten a plaza.
2. Paint paving / dirt plaza texture.
3. Place a well or statue as center landmark.
4. Face houses toward the square.

## 23.2 Market stalls

1. Download medieval market stall / crate / barrel props.
2. Place 3–6 stalls around square edges.
3. Leave open walking lanes (player + future NPCs).

## 23.3 Shop / tavern exterior

1. Choose one larger building as tavern/inn.
2. Put a sign prop near door (even a simple board).
3. Add barrels and benches outside.

## 23.4 Interiors (do after exteriors look good)

1. Either:
   - Use an interior pack (tavern/shop), or
   - Build a simple room with walls + floor + table props
2. Put interior under the building object or in a separate interior scene later.
3. Make sure door area is clear so player can enter later.

> Beginner rule: **Outside world first.** Interiors second.

---

# SECTION 24 — Castle, ruins, and landmarks

Landmarks make players say “I want to go there.”

## 24.1 Ruins (good first landmark)

1. Create `Zone_Ruins` on a hill visible from village.
2. Place broken walls, pillars, fallen stones.
3. Overgrow with grass/moss and 1–2 trees.
4. Path should hint toward it but not be a highway.

## 24.2 Castle / keep (later)

1. Place on strongest silhouette (high rock / mountain shelf).
2. Use one modular castle kit (don’t mix styles).
3. Road should climb with switchbacks.
4. Add gate + walls + tower first; details later.

## 24.3 Landmark checklist

- [ ] Visible from far away
- [ ] Unique shape
- [ ] Path or hint toward it
- [ ] Reward when you arrive (view, loot spot later, story)

---

# SECTION 25 — More biomes: snow, desert, swamp, jungle

Add these **after** your starter valley works.

## 25.1 Snow peaks

1. Raise highest mountains.
2. Paint snow TerrainLayer on tops only.
3. Use pine/conifer trees below snow line.
4. Less grass, more rock.
5. Cooler fog / bluish color grading later.

## 25.2 Desert

1. Flatten arid area far from village (or separate map edge).
2. Paint sand; sparse rocks.
3. Almost no trees.
4. One ruin / oasis later (small water + palms).

## 25.3 Swamp

1. Lower bumpy wet land.
2. Dark mud + moss textures.
3. Dead trees / sparse trees.
4. Shallow water patches + fog.
5. Keep paths on slightly higher ground.

## 25.4 Jungle

1. Dense Paint Trees with tropical/broadleaf trees.
2. Heavy undergrowth (details).
3. Narrow paths only.
4. Darker canopy feel with more fog / lower brightness.

> **Art rule:** Each biome must change **texture + trees + props + mood** together.

---

# SECTION 26 — Day and night cycle

## 26.1 Simple beginner method (manual first)

1. Select Directional Light.
2. Rotate it down for sunset / night tests.
3. Lower intensity at night.
4. Add torches (Section 31) so night is playable.

## 26.2 Real day/night (next step)

Download a simple **day night cycle** asset for URP, or later write a small script that rotates the sun over time.

What you want eventually:

- Sun moves across sky
- Sky color changes
- Night darker
- Optional moon light (very weak)

## 26.3 Professional night tips

- Night should be dark, not pitch-black unreadable.
- Put lights on roads near village.
- Keep forest darker than town (safe vs wild feeling).

---

# SECTION 27 — Weather: rain, fog, wind, storms

## 27.1 Fog (already partly in Section 17)

Use Global Volume fog. Higher fog = mystery / mountains feel farther.

## 27.2 Rain (starter)

1. Download a rain VFX particle prefab (URP).
2. Place as child of camera or a weather manager object under `Systems`.
3. Enable only when you want rain tests.
4. Add rain audio loop (Section 30).

## 27.3 Wind

1. Many tree packs include wind settings.
2. Terrain grass can bend if detail meshes support wind.
3. Even small leaf particles help.

## 27.4 Storm mood (later)

Combine: darker skybox/volume + stronger fog + rain VFX + louder wind audio.

Do not enable all weather all the time while building — it hides your work.

---

# SECTION 28 — Wildlife, birds, fish

## 28.1 Birds (easiest life)

1. Download birds / flying birds pack or ambient bird particles.
2. Place fly paths high above meadow/forest.
3. Keep count low (5–20), not hundreds.

## 28.2 Land animals

1. Download stylized deer / rabbits / sheep (URP).
2. Place small groups near meadow/farm.
3. First version can stand or use simple patrol waypoints later.

## 28.3 Fish

1. Place fish prefabs under water near pier/lake.
2. Simple swim script later; static is OK for first screenshot.

## 28.4 Professional rule

Life should be **noticed**, not chaotic. Few animals in right places beat a zoo everywhere.

---

# SECTION 29 — People / NPCs (simple life)

## 29.1 Stage 1 — Mannequin life (do this first)

1. Download simple fantasy villagers / low poly people.
2. Place 4–8 NPCs in village square and farm.
3. Rotate them to face roads / stalls.
4. No AI needed yet.

## 29.2 Stage 2 — Idle / walk

1. Add idle animations if pack includes Animator.
2. Later: simple waypoint walking between house and market.

## 29.3 Stage 3 — Interaction (much later)

Talk prompts, quests, shops — after world art is strong.

> Do not block roads with NPCs. Leave player space.

---

# SECTION 30 — Sounds of the world (audio)

Silence makes worlds feel fake.

## 30.1 Folder setup

Create:

```
Assets/Audio/
  Ambient/
  Weather/
  Water/
  Animals/
  Village/
  Music/
```

## 30.2 Place ambient sounds

1. Hierarchy → under each zone → Create Empty `Audio_Beach` etc.
2. Add component **Audio Source**
3. Assign a looping clip (waves / wind / birds)
4. Turn on **Loop**
5. Turn on **Spatial Blend = 3D** for local sounds
6. Set Min/Max distance so it fades as you walk away

## 30.3 What to put where

| Zone | Sound |
|---|---|
| Beach | Waves |
| Forest | Soft wind + birds |
| Mountain | Stronger wind |
| River / waterfall | Water flow |
| Village | Soft ambience / distant activity |
| Night | Crickets / quieter wind |

## 30.4 Music

Use very soft exploration music later. Ambient nature should stay primary while building.

> **Stuck?** No sound in Play mode → check Audio Source has clip, not muted, and listener exists on camera/player.

---

# SECTION 31 — Campfires, torches, night lights

## 31.1 Torches / lamps

1. Download fantasy torch / lantern pack with fire VFX + point light (or add Point Light yourself).
2. Place at:
   - Village streets
   - Bridge entrances
   - Cave mouth
   - Castle gate later
3. Point Light intensity low; range small.

## 31.2 Campfire travel spots

1. Place campfire prefab beside road clearings.
2. Add 2–3 sitting rocks/logs.
3. This makes travel feel human.

## 31.3 Window glow (cheap trick)

At night, put tiny Point Lights inside/near windows for “someone lives here.”

---

# SECTION 32 — Signs, waypoints, travel feel

## 32.1 Road signs

1. Place wooden signposts at junctions: Beach / Village / Forest / Mountains.
2. Even if text is simple, direction helps players.

## 32.2 Visual waypoints (no UI needed yet)

Use natural landmarks:

- Giant hero tree
- Standing stones
- Watchtower
- Ruins on hill
- Waterfall mist

## 32.3 Path language

- Wide path = safe / main route
- Narrow trail = adventure / danger
- Broken path near ruins = mystery

---

# SECTION 33 — Borders, walls, watchtowers

## 33.1 Village edge

1. Use fences first (soft border).
2. Later wooden palisade / gate if village becomes a town.

## 33.2 Watchtower

1. Place one tower where road enters forest or mountain.
2. Makes world feel guarded and story-rich.

## 33.3 World edge (temporary)

Until map is finished, block map edges with:

- Dense trees, or
- Cliffs, or
- Invisible walls later

So players don’t fall off unfinished land.

---

# SECTION 34 — Shore life and underwater look

## 34.1 Shore details

Near beach/lake edges add:

- Small rocks
- Driftwood
- Pier posts
- Footprint-free clean sand strip near water

## 34.2 Underwater tint (simple)

1. If using a water asset, enable shallow color / depth settings if available.
2. Or later add a post-effect when camera is under water.

## 34.3 Reef / fish accents

Only in clear water areas. Keep sparse.

---

# SECTION 35 — Make the world feel alive

Checklist of “alive” polish:

- [ ] Wind in trees/grass
- [ ] Birds somewhere in sky
- [ ] Water sound near water
- [ ] Path not blocked by trees
- [ ] Village has people or props suggesting people
- [ ] Night has lights
- [ ] Distant landmark visible
- [ ] Small particle dust/pollen in meadow (optional)
- [ ] Campfire smoke (optional)

Do 2–3 of these after each zone, not all at once.

---

# SECTION 36 — Bigger world later

When your first valley is good:

## 36.1 Expand options

1. Increase current terrain size carefully, or
2. Add neighboring Terrain tiles (grid of terrains), or
3. Make a new scene biome and connect later with loading

## 36.2 Beginner-safe advice

Stay on **one 500–1000m valley** until:

- paths work
- village looks good
- forest/mountain read clearly
- you can walk 5 minutes without broken props

Then expand.

---

# SECTION 37 — What assets you need (full download list)

Prefer **URP** always.

## Must-have (Phase 1 — Land)

| # | Asset type | Search words |
|---|---|---|
| 1 | Ground textures (sand, grass, dirt, rock, snow, mud) | `terrain textures PBR`, `ground textures` |
| 2 | Trees URP (oak + pine/conifer) | `trees URP`, `conifer URP` |
| 3 | Grass / flowers details | `terrain grass flowers` |
| 4 | Rocks / cliffs | `rock pack`, `cliffs` |
| 5 | Skyboxes | `skybox free`, `AllSky` |
| 6 | Starter Assets Third Person | `Starter Assets Third Person` |
| 7 | URP water | `URP water`, `stylized water URP` |

## Strongly recommended (Phase 2 — Places)

| # | Asset type | Search words |
|---|---|---|
| 8 | Fantasy / medieval village modular | `fantasy village`, `medieval village` |
| 9 | Farm fences / stables / troughs | often inside village packs |
| 10 | Bridge pack | `wooden bridge`, `stone bridge` |
| 11 | Road tool | `EasyRoads3D` |
| 12 | Medieval props (barrels, crates, carts, stalls) | `medieval props`, `market stall` |
| 13 | Pier + boats | `wooden pier`, `boats` |
| 14 | Ruins / broken stone | `ruins pack`, `ancient ruins` |
| 15 | Torches / campfire VFX | `fantasy torch`, `campfire` |

## Living world (Phase 3–4)

| # | Asset type | Search words |
|---|---|---|
| 16 | Day/night cycle | `day night cycle URP` |
| 17 | Rain / weather VFX | `rain VFX URP`, `weather` |
| 18 | Birds / animals | `birds`, `stylized animals URP` |
| 19 | Fish | `fish animated` |
| 20 | Villager characters | `fantasy villagers`, `RPG people` |
| 21 | Nature audio packs | `nature ambience`, `waves wind birds` |
| 22 | Waterfall VFX | `waterfall` |
| 23 | Palm / jungle trees | `palm trees URP`, `jungle vegetation` |
| 24 | Castle / fortress modular | `fantasy castle modular` |
| 25 | Cave rocks / tunnel pieces | `cave pack` |
| 26 | Interior tavern/shop | `tavern interior URP` |

## Do NOT download first

| Avoid now | Why |
|---|---|
| HDRP-only packs | Pink materials / wrong pipeline |
| Huge modern cities | Fights fantasy style |
| Full AAA character RPG systems | Too early |
| 10 architecture cultures at once | World looks fake |

---

# SECTION 38 — Where to get free assets safely

## Official / safe sources

1. **Unity Asset Store**  
2. **Package Manager** → Unity Registry / My Assets  
3. Packs clearly marked **URP**

## How to import

1. Get asset on Asset Store  
2. Unity → **Window → Package Manager** → **My Assets**  
3. **Download** → **Import**  
4. Drag prefabs into correct `Zone_...`

> **Pink materials?** Shader → `Universal Render Pipeline/Lit`, or import pack’s URP `.unitypackage`.

## License reminder

Read license before commercial release.

---

# SECTION 39 — Week-by-week plan (living world)

## Week 1 — Engine + land

- [ ] Unity Hub + Unity 6
- [ ] Universal 3D (URP) project
- [ ] Terrain 500×500
- [ ] Beach, hills, mountains, river shaped
- [ ] Textures painted
- [ ] Basic water
- [ ] Player can walk

## Week 2 — Nature beauty

- [ ] Trees painted (forest + mountain pines)
- [ ] Grass/flowers
- [ ] Rocks/cliffs
- [ ] Sky + fog
- [ ] Path Beach → Village area
- [ ] Ambient audio starts (waves/wind)

## Week 3 — Civilization

- [ ] Village houses + square
- [ ] Farm + fences
- [ ] Bridge + river finish
- [ ] Pier/boats
- [ ] Market props
- [ ] Signs at junctions

## Week 4 — Landmarks + night

- [ ] Ruins landmark
- [ ] Waterfall or lake
- [ ] Cave mouth
- [ ] Torches + campfires
- [ ] First night lighting test
- [ ] Watchtower

## Week 5 — Life systems

- [ ] Birds/animals sparse
- [ ] Simple NPCs in village
- [ ] Day/night asset or sun rotation
- [ ] Rain test weather
- [ ] More audio zones

## Week 6 — Expand biomes carefully

- [ ] Snow peaks OR desert OR swamp (pick one)
- [ ] Polish the starter valley again
- [ ] Only then grow map size

---

# SECTION 40 — If you get stuck (fix list)

| Problem | Do this |
|---|---|
| Unity Hub won’t install Editor | Restart PC, run Hub as Admin, retry |
| Wrong template | New project → **Universal 3D (URP)** |
| Pink materials | Switch to URP/Lit or import URP package |
| Terrain tools missing | Select Terrain object first |
| Brush too strong | Opacity 0.05–0.1 |
| Water wrong height | Nudge Y until shoreline looks wet |
| Trees don’t paint | Paint Trees → Add Tree first |
| Player falls | Terrain Collider on; spawn above ground |
| No sound | Audio Source clip + listener on camera |
| Night too dark | Add torches; don’t crush exposure |
| World feels empty | Add path + landmark + 1 audio + 1 life element |
| World feels messy | One zone at a time; hide other zones |
| Fear of breaking | Ctrl+S; duplicate scene backup |

### Emergency scene backup

1. Select `MainWorld.unity` in Project  
2. **Ctrl+D**  
3. Rename `MainWorld_Backup_01`

---

# SECTION 41 — Professional rules (do not break these)

1. URP only for this guide.  
2. One valley first — not a continent on day one.  
3. Terrain → paths → buildings → life → systems → gameplay.  
4. One art style per zone.  
5. Low sculpt opacity.  
6. Paint forests; hand-place only heroes.  
7. Name objects clearly.  
8. Save and backup.  
9. Walk the world every day.  
10. Gameplay (quests/combat/vehicles) after land feels alive.

---

# SECTION 42 — Full living-world checklist

Use this as your “is my world real yet?” list.

### Land
- [ ] Terrain exists and is sized sanely
- [ ] Beach + ocean meet cleanly
- [ ] Hills readable
- [ ] Mountains on horizon
- [ ] Forest painted with clearings
- [ ] Meadow grass
- [ ] River carved
- [ ] Lake or pond
- [ ] Waterfall (or planned)
- [ ] Cave mouth (or planned)
- [ ] Rocks not floating

### Civilization
- [ ] Main path connects key places
- [ ] Village with square
- [ ] Farm nearby
- [ ] Bridge crossing
- [ ] Harbor/pier
- [ ] Market props
- [ ] Signs/waypoints
- [ ] Ruins or castle landmark
- [ ] Torches/campfires for night

### Life & atmosphere
- [ ] Player walks whole route
- [ ] Sky + sun + fog
- [ ] Zone audio (beach/forest/village)
- [ ] Birds or animals somewhere
- [ ] Simple NPCs in town
- [ ] Day/night test
- [ ] Weather test (optional)
- [ ] Night still playable

### Quality
- [ ] Hierarchy organized by zones
- [ ] No pink materials
- [ ] No major floating props
- [ ] One style per zone
- [ ] Scene backups saved

---

# Quick “where do I click?” cheat sheet

| I want to… | Go here |
|---|---|
| New project | Unity Hub → Projects → New project → **Universal 3D** |
| Save scene | File → Save As → `Assets/Scenes/MainWorld` |
| Add ground | Hierarchy right-click → 3D Object → Terrain |
| Make mountains | Terrain → Paint Terrain → Raise or Lower |
| Smooth mountains | Paint Terrain → Smooth Height |
| Paint sand/grass | Paint Terrain → Paint Texture |
| Paint forest | Terrain → Paint Trees |
| Paint grass | Terrain → Paint Details |
| Add water | Plane + URP water material/prefab |
| Add sun angle | Directional Light → rotate with E |
| Change sky | Window → Rendering → Lighting → Environment |
| Fog/look | Global Volume overrides |
| Import assets | Window → Package Manager → My Assets |
| Test walking | Starter Assets player → Play |
| Add sound | Audio Source on zone empty object |
| Night lights | Point Light + torch/campfire prefabs |

---

# Final words

A real-feeling world has **land + places + travel + weather + life + sound**.

You still start small:

1. Correct URP project  
2. One terrain valley  
3. Beach, path, village, forest, river, mountain  
4. Then lakes, farms, ruins, lights, audio, animals, day/night  

Do not rush Phase 5 gameplay before Phase 1–3 feel good.

If you get stuck, write:

- what you clicked  
- what you expected  
- what you saw  

Then use Section 40.

**Build a world people believe they could live in — one zone at a time.**

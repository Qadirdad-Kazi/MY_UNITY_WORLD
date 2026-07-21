# MY UNITY WORLD — Local Docs Site

Beautiful localhost documentation for the whole project.  
**Source of truth:** `Assets/MyWorld/Docs/MASTER_GUIDE.md` (read live — edit MD, refresh browser).

## Open the docs

```bat
cd "D:\Github Clones\MY_UNITY_WORLD\DocsSite"
start.bat
```

Or manually:

```bat
cd DocsSite
pip install -r requirements.txt
python app.py
```

Then open:

| Where | URL |
|-------|-----|
| This PC | **http://127.0.0.1:5050** |
| Other device (same Wi‑Fi) | `http://YOUR-PC-IP:5050` (printed in the terminal) |

## What you get

| Page | Content |
|------|---------|
| Home | Brand hero, topic chips, build-order path, **grouped** Master Guide chapters |
| `/guide/...` | Full Master Guide — one section per page + reading progress |
| `/doc/...` | Archive MDs (How to Use Scripts, assets, from-scratch, etc.) |
| Search | Top bar — `/` to focus, arrow keys + Enter; full-text across Master Guide + archives |
| Mobile | Hamburger opens the section sidebar |

### UI notes

- Theme: **island atlas** (seafoam + sun accents) — CSS in `static/css/site.css`
- Code blocks get a **Copy** button
- “On this page” highlights the heading you’re reading

## Covered in Master Guide (current)

Player · cars · **visible sit** · bikes · tune · horn / lights · swim · **NPCs** · **breakable plants** · **birds / flying** · **ground animals** · terrain · beach · forest · village · EasyRoads · sky / weather · scripts · troubleshooting.

### Ground / surface animals (quick)

Docs UI chip: **Ground animals** → Master Guide **§19**.

- **Ground Animal** on deer/rabbit/dog prefab (walks on terrain via raycast).
- **Ground Herd** to spawn many.
- No NavMesh needed (unlike villager NPCs).

### Birds & flying animals (quick)

Docs UI chip: **Birds · flying** → Master Guide **§18**.

- **Flying Animal** / **Flying Flock** in the sky.

### Breakable plants (quick)

Docs UI chip: **Breakable plants** → Master Guide **§17**.

- Prefab bushes + **Is Trigger** + **Breakable Foliage**.

## Docs are live from

- `Assets/MyWorld/Docs/*.md`
- Root `README.md`

No rebuild needed after MD edits — refresh the browser.

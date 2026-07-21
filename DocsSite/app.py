"""
MY UNITY WORLD — Documentation site (localhost)

Reads Markdown from Assets/MyWorld/Docs + root README and serves
a section-based web UI. Master Guide is the source of truth — edit MD, refresh.

Usage:
  cd DocsSite
  pip install -r requirements.txt
  python app.py
  Open http://127.0.0.1:5050
"""

from __future__ import annotations

import re
from pathlib import Path

from flask import Flask, abort, render_template, url_for
import markdown

ROOT = Path(__file__).resolve().parent.parent
DOCS_DIR = ROOT / "Assets" / "MyWorld" / "Docs"
README_PATH = ROOT / "README.md"

app = Flask(__name__)

# Primary guide + archive catalog
CATALOG = [
    {
        "id": "master",
        "title": "Master Guide",
        "file": "MASTER_GUIDE.md",
        "badge": "Primary",
        "blurb": "Player, cars, bikes, visible sit, sounds & lights, NPCs / NavMesh, breakable plants, boats, world, sky, weather, fog, assets.",
        "primary": True,
    },
    {
        "id": "readme",
        "title": "Project README",
        "file": None,
        "path": README_PATH,
        "badge": "Overview",
        "blurb": "Repo overview, quick start, and links into the Master Guide.",
        "primary": True,
    },
    {
        "id": "how-scripts",
        "title": "How to Use Scripts",
        "file": "HOW_TO_USE_SCRIPTS.md",
        "badge": "Archive",
        "blurb": "Player, doors, cars, bikes, Vehicle Audio/Lights, boats, zones — mirrors Master Guide scripts.",
        "primary": False,
    },
    {
        "id": "vehicles",
        "title": "Vehicles & Player",
        "file": "VEHICLES_AND_PLAYER.md",
        "badge": "Archive",
        "blurb": "Merged into Master Guide — kept for reference.",
        "primary": False,
    },
    {
        "id": "bible",
        "title": "Section Build Bible",
        "file": "SECTION_BY_SECTION_BUILD_BIBLE.md",
        "badge": "Archive",
        "blurb": "Older long section-by-section click steps (0–22).",
        "primary": False,
    },
    {
        "id": "from-scratch",
        "title": "From Scratch Guide",
        "file": "FROM_SCRATCH_WORLD_BUILDING_GUIDE.md",
        "badge": "Archive",
        "blurb": "Unity install theory and world-building pedagogy (42 sections).",
        "primary": False,
    },
    {
        "id": "assets",
        "title": "Assets & Downloads",
        "file": "ASSETS_INCLUDED_AND_DOWNLOAD.md",
        "badge": "Archive",
        "blurb": "What is included vs what to download per world part.",
        "primary": False,
    },
    {
        "id": "start-here",
        "title": "Start Here",
        "file": "START_HERE.md",
        "badge": "Archive",
        "blurb": "Clean production foundation notes after prototype cleanup.",
        "primary": False,
    },
    {
        "id": "starter-pack",
        "title": "Starter Pack Notes",
        "file": "STARTER_PACK_README.md",
        "badge": "Archive",
        "blurb": "Legacy pack migration / import notes.",
        "primary": False,
    },
]

# Quick topic chips on the home page (deep-link into Master Guide sections)
TOPIC_CHIPS = [
    {"label": "Player walk", "num": "2"},
    {"label": "First car + sit", "num": "3"},
    {"label": "Bikes", "num": "5"},
    {"label": "Tune / don’t stuck", "num": "6"},
    {"label": "Horn · engine · lights", "num": "6b"},
    {"label": "Speedometer", "num": "6c"},
    {"label": "Swim · beach water", "num": "8"},
    {"label": "EasyRoads", "num": "11"},
    {"label": "Weather & fog", "num": "12"},
    {"label": "Scripts sheet", "num": "14"},
    {"label": "NPCs auto walk", "num": "16"},
    {"label": "Breakable plants", "num": "17"},
    {"label": "Birds · flying", "num": "18"},
    {"label": "Ground animals", "num": "19"},
    {"label": "Troubleshooting", "num": "15"},
]

# Recommended build order (section numbers as in MASTER_GUIDE)
ORDER_NUMS = ["2", "3", "5", "6b", "6c", "4", "7", "8", "9", "10", "11", "12"]

# Home page chapter groups (section nums → label)
SECTION_GROUPS = [
    {"label": "Start & player", "nums": ["0", "1", "2"]},
    {"label": "Vehicles", "nums": ["3", "4", "5", "6", "6b", "6c"]},
    {"label": "World build", "nums": ["7", "8", "9", "10", "11", "12", "13"]},
    {"label": "Life & systems", "nums": ["16", "17", "18", "19"]},
    {"label": "Reference", "nums": ["14", "15"]},
]

MD_EXTENSIONS = [
    "tables",
    "fenced_code",
    "toc",
    "nl2br",
    "sane_lists",
    "smarty",
]


def resolve_doc(entry: dict) -> Path:
    if entry.get("path"):
        return Path(entry["path"])
    return DOCS_DIR / entry["file"]


def read_markdown(path: Path) -> str:
    if not path.exists():
        return f"# Missing\n\nFile not found:\n`{path}`"
    return path.read_text(encoding="utf-8", errors="replace")


def md_to_html(text: str) -> str:
    return markdown.markdown(
        text,
        extensions=MD_EXTENSIONS,
        extension_configs={"toc": {"permalink": False}},
    )


def slugify(title: str) -> str:
    s = title.lower().strip()
    s = re.sub(r"[^\w\s-]", "", s)
    s = re.sub(r"[-\s]+", "-", s)
    return s.strip("-")[:80] or "section"


def parse_contents_blurbs(text: str) -> dict[str, str]:
    """
    Read the Master Guide contents table:
    | 6b | [Title](#…) | Blurb |
    Returns { "6b": "Blurb", ... }
    """
    blurbs: dict[str, str] = {}
    for line in text.splitlines():
        m = re.match(
            r"^\|\s*(\d+[a-zA-Z]?)\s*\|\s*\[([^\]]+)\]\([^)]+\)\s*\|\s*(.+?)\s*\|$",
            line.strip(),
        )
        if not m:
            continue
        num, _title, blurb = m.group(1), m.group(2), m.group(3)
        # Strip markdown bold
        blurb = re.sub(r"\*\*([^*]+)\*\*", r"\1", blurb).strip()
        blurbs[num] = blurb
    return blurbs


def split_master_sections(text: str) -> list[dict]:
    """Split MASTER_GUIDE on top-level # headings (except the very first title)."""
    blurbs = parse_contents_blurbs(text)
    lines = text.splitlines()
    sections: list[dict] = []
    current: dict | None = None
    buf: list[str] = []

    def flush():
        nonlocal current, buf
        if current is None:
            return
        body = "\n".join(buf).strip()
        current["body_md"] = body
        current["html"] = md_to_html(f"# {current['title']}\n\n{body}")
        current["blurb"] = blurbs.get(current["num"], "")
        sections.append(current)
        buf = []

    for line in lines:
        if line.startswith("# ") and not line.startswith("## "):
            title = line[2:].strip()
            # Skip the document title as its own page — fold into intro
            if title.startswith("MY UNITY WORLD"):
                if current is None:
                    current = {
                        "id": "intro",
                        "num": "0",
                        "title": "Introduction & contents",
                        "short": "Start here",
                    }
                    buf = []
                continue
            flush()
            # "1) Title" or "6b) Title"
            m = re.match(r"^(\d+[a-zA-Z]?)\)\s*(.+)$", title)
            if m:
                num, rest = m.group(1), m.group(2)
                short = rest
            else:
                num, rest, short = "·", title, title
            current = {
                "id": slugify(f"{num}-{rest}"),
                "num": num,
                "title": rest if m else title,
                "short": short[:56],
            }
            buf = []
        else:
            if current is None:
                current = {
                    "id": "intro",
                    "num": "0",
                    "title": "Introduction & contents",
                    "short": "Start here",
                }
            buf.append(line)

    flush()
    return sections


def extract_toc_from_html(html: str) -> list[dict]:
    """Pull h2/h3 anchors for in-page nav."""
    toc = []
    for m in re.finditer(r"<h([23])[^>]*>(.*?)</h\1>", html, re.I | re.S):
        level = int(m.group(1))
        raw = re.sub(r"<[^>]+>", "", m.group(2)).strip()
        if not raw:
            continue
        toc.append({"level": level, "title": raw, "id": slugify(raw)})
    return toc


def inject_heading_ids(html: str) -> str:
    def repl(m):
        tag, attrs, inner = m.group(1), m.group(2), m.group(3)
        text = re.sub(r"<[^>]+>", "", inner).strip()
        hid = slugify(text)
        if "id=" in attrs:
            return m.group(0)
        return f'<{tag} id="{hid}"{attrs}>{inner}</{tag}>'

    return re.sub(r"<(h[1-4])([^>]*)>(.*?)</\1>", repl, html, flags=re.I | re.S)


def catalog_by_id(doc_id: str) -> dict | None:
    for e in CATALOG:
        if e["id"] == doc_id:
            return e
    return None


def resolve_topic_chips(sections: list[dict]) -> list[dict]:
    by_num = {s["num"]: s for s in sections}
    chips = []
    for t in TOPIC_CHIPS:
        s = by_num.get(t["num"])
        if s:
            chips.append({**t, "id": s["id"], "title": s["title"]})
    return chips


def group_sections(sections: list[dict]) -> list[dict]:
    """Bucket Master Guide sections into themed home-page chapters."""
    by_num = {s["num"]: s for s in sections}
    used: set[str] = set()
    groups: list[dict] = []
    for g in SECTION_GROUPS:
        bucket = []
        for num in g["nums"]:
            s = by_num.get(num)
            if s:
                bucket.append(s)
                used.add(s["num"])
        if bucket:
            groups.append({"label": g["label"], "sections": bucket})
    leftover = [s for s in sections if s["num"] not in used]
    if leftover:
        groups.append({"label": "More", "sections": leftover})
    return groups


def ordered_build_sections(sections: list[dict]) -> list[dict]:
    """Master Guide sections in recommended build order."""
    by_num = {s["num"]: s for s in sections}
    return [by_num[n] for n in ORDER_NUMS if n in by_num]


@app.context_processor
def inject_globals():
    return {
        "catalog": CATALOG,
        "primary_docs": [c for c in CATALOG if c.get("primary")],
        "archive_docs": [c for c in CATALOG if not c.get("primary")],
    }


@app.route("/")
def home():
    master_path = DOCS_DIR / "MASTER_GUIDE.md"
    sections = split_master_sections(read_markdown(master_path)) if master_path.exists() else []
    return render_template(
        "home.html",
        sections=sections,
        order_nums=ORDER_NUMS,
        order_sections=ordered_build_sections(sections),
        topic_chips=resolve_topic_chips(sections),
        section_groups=group_sections(sections),
        page_title="MY UNITY WORLD — Docs",
    )


@app.route("/guide/")
@app.route("/guide/<section_id>/")
def guide(section_id: str | None = None):
    text = read_markdown(DOCS_DIR / "MASTER_GUIDE.md")
    sections = split_master_sections(text)
    if not sections:
        abort(404)

    if section_id is None:
        section_id = sections[0]["id"]

    current = next((s for s in sections if s["id"] == section_id), None)
    if current is None:
        abort(404)

    idx = sections.index(current)
    prev_s = sections[idx - 1] if idx > 0 else None
    next_s = sections[idx + 1] if idx < len(sections) - 1 else None

    html = inject_heading_ids(current["html"])
    toc = extract_toc_from_html(html)

    return render_template(
        "guide.html",
        sections=sections,
        current=current,
        content_html=html,
        toc=toc,
        prev_s=prev_s,
        next_s=next_s,
        page_title=f"{current['title']} — Master Guide",
    )


@app.route("/doc/<doc_id>/")
def doc_page(doc_id: str):
    entry = catalog_by_id(doc_id)
    if entry is None:
        abort(404)
    if doc_id == "master":
        return guide()

    path = resolve_doc(entry)
    text = read_markdown(path)
    html = inject_heading_ids(md_to_html(text))
    toc = extract_toc_from_html(html)

    return render_template(
        "doc.html",
        entry=entry,
        content_html=html,
        toc=toc,
        page_title=f"{entry['title']} — Docs",
        source_path=str(path.relative_to(ROOT)) if path.exists() and ROOT in path.parents else str(path),
    )


@app.route("/search.json")
def search_index():
    """Lightweight full-text index for client search."""
    items = []
    text = read_markdown(DOCS_DIR / "MASTER_GUIDE.md")
    for s in split_master_sections(text):
        items.append(
            {
                "title": f"{s['num']}) {s['title']}" if s["num"] not in ("0", "·") else s["title"],
                "group": "Master Guide",
                "url": url_for("guide", section_id=s["id"]),
                "snippet": re.sub(r"\s+", " ", s.get("body_md", ""))[:220],
            }
        )
    for e in CATALOG:
        if e["id"] == "master":
            continue
        path = resolve_doc(e)
        body = read_markdown(path)
        items.append(
            {
                "title": e["title"],
                "group": e.get("badge", "Docs"),
                "url": url_for("doc_page", doc_id=e["id"]),
                "snippet": re.sub(r"\s+", " ", body)[:220],
            }
        )
    from flask import jsonify

    return jsonify(items)


if __name__ == "__main__":
    import socket

    def lan_ip() -> str:
        try:
            s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
            s.connect(("8.8.8.8", 80))
            ip = s.getsockname()[0]
            s.close()
            return ip
        except OSError:
            return "YOUR-PC-IP"

    host_ip = lan_ip()
    print("=" * 56)
    print("  MY UNITY WORLD — Documentation")
    print(f"  This PC:     http://127.0.0.1:5050")
    print(f"  Other laptop (same Wi-Fi): http://{host_ip}:5050")
    print(f"  Docs folder: {DOCS_DIR}")
    print("  Keep this window open while browsing.")
    print("=" * 56)
    app.run(host="0.0.0.0", port=5050, debug=True)

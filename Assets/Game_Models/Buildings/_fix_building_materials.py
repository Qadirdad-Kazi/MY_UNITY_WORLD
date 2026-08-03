# Fix Buildings pack materials: create URP Lit .mat files + remap FBX externalObjects
# so textures appear automatically (embedded FBX mats are read-only / path-broken).

import os, re, uuid, hashlib

ROOT = r"D:\Github Clones\MY_UNITY_WORLD\Assets\Game_Models\Buildings"
URP_LIT = "933532a4fcc9baf4fa0491de14d08ed7"

MAT_TEMPLATE = """%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!21 &2100000
Material:
  serializedVersion: 8
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_Name: {name}
  m_Shader: {{fileID: 4800000, guid: {shader}, type: 3}}
  m_Parent: {{fileID: 0}}
  m_ModifiedSerializedProperties: 0
  m_ValidKeywords: {keywords}
  m_InvalidKeywords: []
  m_LightmapFlags: 4
  m_EnableInstancingVariants: 0
  m_DoubleSidedGI: 0
  m_CustomRenderQueue: -1
  stringTagMap:
    RenderType: Opaque
  disabledShaderPasses:
  - MOTIONVECTORS
  m_LockedProperties: 
  m_SavedProperties:
    serializedVersion: 3
    m_TexEnvs:
    - _BaseMap:
        m_Texture: {{fileID: {base_id}, guid: {base_guid}, type: 3}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _BumpMap:
        m_Texture: {{fileID: {bump_id}, guid: {bump_guid}, type: 3}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _DetailAlbedoMap:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _DetailMask:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _DetailNormalMap:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _EmissionMap:
        m_Texture: {{fileID: {emis_id}, guid: {emis_guid}, type: 3}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _MainTex:
        m_Texture: {{fileID: {base_id}, guid: {base_guid}, type: 3}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _MetallicGlossMap:
        m_Texture: {{fileID: {metal_id}, guid: {metal_guid}, type: 3}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _OcclusionMap:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _ParallaxMap:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - _SpecGlossMap:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - unity_Lightmaps:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - unity_LightmapsInd:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    - unity_ShadowMasks:
        m_Texture: {{fileID: 0}}
        m_Scale: {{x: 1, y: 1}}
        m_Offset: {{x: 0, y: 0}}
    m_Ints: []
    m_Floats:
    - _AddPrecomputedVelocity: 0
    - _AlphaClip: 0
    - _AlphaToMask: 0
    - _Blend: 0
    - _BlendModePreserveSpecular: 1
    - _BumpScale: 1
    - _ClearCoatMask: 0
    - _ClearCoatSmoothness: 0
    - _Cull: 2
    - _Cutoff: 0.5
    - _DetailAlbedoMapScale: 1
    - _DetailNormalMapScale: 1
    - _DstBlend: 0
    - _DstBlendAlpha: 0
    - _EnvironmentReflections: 1
    - _GlossMapScale: 0
    - _Glossiness: 0.35
    - _GlossyReflections: 0
    - _Metallic: {metallic}
    - _OcclusionStrength: 1
    - _Parallax: 0.005
    - _QueueOffset: 0
    - _ReceiveShadows: 1
    - _Smoothness: {smoothness}
    - _SmoothnessTextureChannel: 0
    - _SpecularHighlights: 1
    - _SrcBlend: 1
    - _SrcBlendAlpha: 1
    - _Surface: 0
    - _WorkflowMode: 1
    - _XRMotionVectorsPass: 1
    - _ZWrite: 1
    m_Colors:
    - _BaseColor: {{r: 1, g: 1, b: 1, a: 1}}
    - _Color: {{r: 1, g: 1, b: 1, a: 1}}
    - _EmissionColor: {{r: {emis_r}, g: {emis_g}, b: {emis_b}, a: 1}}
    - _SpecColor: {{r: 0.19999996, g: 0.19999996, b: 0.19999996, a: 1}}
  m_BuildTextureStacks: []
  m_AllowLocking: 1
--- !u!114 &3214903029537801867
MonoBehaviour:
  m_ObjectHideFlags: 11
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: 0}}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {{fileID: 11500000, guid: d0353a89b1f911e48b9e16bdc9f2e058, type: 3}}
  m_Name: 
  m_EditorClassIdentifier: Unity.RenderPipelines.Universal.Editor::UnityEditor.Rendering.Universal.AssetVersion
  version: 10
"""

META_TEMPLATE = """fileFormatVersion: 2
guid: {guid}
NativeFormatImporter:
  externalObjects: {{}}
  mainObjectFileID: 2100000
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"""

FOLDER_META = """fileFormatVersion: 2
guid: {guid}
folderAsset: yes
DefaultImporter:
  externalObjects: {{}}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"""


def new_guid():
    return uuid.uuid4().hex


def read_guid(meta_path):
    with open(meta_path, "r", encoding="utf-8", errors="ignore") as f:
        m = re.search(r"^guid:\s*([a-f0-9]+)", f.read(), re.M)
    return m.group(1) if m else None


def ensure_folder_meta(folder):
    meta = folder + ".meta"
    if not os.path.exists(meta):
        with open(meta, "w", encoding="utf-8", newline="\n") as f:
            f.write(FOLDER_META.format(guid=new_guid()))


def index_textures(pack_dir):
    """basename(lower) -> guid. Also stem without extension."""
    idx = {}
    for dirpath, _, files in os.walk(pack_dir):
        for f in files:
            if f.lower().endswith((".png", ".jpg", ".jpeg", ".tga", ".dds")):
                fp = os.path.join(dirpath, f)
                meta = fp + ".meta"
                if not os.path.exists(meta):
                    continue
                g = read_guid(meta)
                if not g:
                    continue
                idx[f.lower()] = g
                idx[os.path.splitext(f)[0].lower()] = g
                # also without spaces quirks
                idx[f.lower().replace(" ", "")] = g
                idx[f.lower().replace(" ", "_")] = g
                idx[f.lower().replace("__", "_")] = g
    return idx


def find_tex(idx, *candidates):
    for c in candidates:
        if not c:
            continue
        base = os.path.basename(c).replace("\\", "/").split("/")[-1]
        key = base.lower()
        if key in idx:
            return idx[key]
        stem = os.path.splitext(key)[0]
        if stem in idx:
            return idx[stem]
        # fuzzy: stem contained
        for k, g in idx.items():
            if stem and (stem in k or k in stem):
                if os.path.splitext(k)[1] or True:
                    return g
    return "00000000000000000000000000000000"


def tex_ref(guid):
    if guid == "00000000000000000000000000000000":
        return 0, "00000000000000000000000000000000"
    return 2800000, guid


def write_mat(path, name, base_g, bump_g=None, metal_g=None, emis_g=None, metallic=0, smoothness=0.35, emission=False):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    ensure_folder_meta(os.path.dirname(path))
    keywords = []
    if bump_g and bump_g != "00000000000000000000000000000000":
        keywords.append("_NORMALMAP")
    if metal_g and metal_g != "00000000000000000000000000000000":
        keywords.append("_METALLICSPECGLOSSMAP")
    if emission and emis_g and emis_g != "00000000000000000000000000000000":
        keywords.append("_EMISSION")
    kw = "[]" if not keywords else ("\n  - " + "\n  - ".join(keywords))
    # validKeywords YAML
    if keywords:
        kw_yaml = "\n".join(["  - " + k for k in keywords])
        # replace in template differently
        kw_block = "\n".join(["  - " + k for k in keywords])
    else:
        kw_block = "[]"

    bid, bg = tex_ref(base_g or "00000000000000000000000000000000")
    nid, ng = tex_ref(bump_g or "00000000000000000000000000000000")
    mid, mg = tex_ref(metal_g or "00000000000000000000000000000000")
    eid, eg = tex_ref(emis_g or "00000000000000000000000000000000")

    if keywords:
        # fix keywords formatting in template - we pass as YAML list body
        keywords_str = "\n" + "\n".join([f"  - {k}" for k in keywords])
    else:
        keywords_str = " []"

    # Rebuild keywords line properly
    content = MAT_TEMPLATE.format(
        name=name,
        shader=URP_LIT,
        keywords="[]",  # patched below
        base_id=bid, base_guid=bg,
        bump_id=nid, bump_guid=ng,
        metal_id=mid, metal_guid=mg,
        emis_id=eid, emis_guid=eg,
        metallic=metallic,
        smoothness=smoothness,
        emis_r=1 if emission else 0,
        emis_g=1 if emission else 0,
        emis_b=1 if emission else 0,
    )
    if keywords:
        content = content.replace("m_ValidKeywords: []", "m_ValidKeywords:\n" + "\n".join([f"  - {k}" for k in keywords]))
    if emission:
        content = content.replace("m_LightmapFlags: 4", "m_LightmapFlags: 2")

    with open(path, "w", encoding="utf-8", newline="\n") as f:
        f.write(content)

    meta = path + ".meta"
    if os.path.exists(meta):
        g = read_guid(meta)
    else:
        g = new_guid()
        with open(meta, "w", encoding="utf-8", newline="\n") as f:
            f.write(META_TEMPLATE.format(guid=g))
    return g


def patch_fbx_meta(fbx_meta, remaps, search_global=True):
    """remaps: list of (mat_name, mat_guid)"""
    with open(fbx_meta, "r", encoding="utf-8", errors="ignore") as f:
        text = f.read()

    lines = []
    for name, guid in remaps:
        lines.append("  - first:")
        lines.append("      type: UnityEngine:Material")
        lines.append("      assembly: UnityEngine.CoreModule")
        lines.append(f"      name: {name}")
        lines.append(f"    second: {{fileID: 2100000, guid: {guid}, type: 2}}")
    block = "  externalObjects:\n" + "\n".join(lines) if remaps else "  externalObjects: {}"

    text2 = re.sub(r"  externalObjects:.*?(?=\n  materials:)", block + "\n", text, count=1, flags=re.S)
    if text2 == text:
        text2 = text.replace("  externalObjects: {}", block)

    text2 = re.sub(r"materialImportMode: \d+", "materialImportMode: 1", text2)
    if search_global:
        text2 = re.sub(r"searchTexturesGlobally: \d+", "searchTexturesGlobally: 1", text2)

    with open(fbx_meta, "w", encoding="utf-8", newline="\n") as f:
        f.write(text2)


def parse_mtl(mtl_path):
    mats = {}
    cur = None
    with open(mtl_path, "r", encoding="utf-8", errors="ignore") as f:
        for line in f:
            line = line.strip()
            if line.startswith("newmtl "):
                cur = line[7:].strip()
                mats[cur] = {}
            elif cur and line.startswith("map_Kd "):
                mats[cur]["base"] = line[7:].strip()
            elif cur and line.startswith("map_Bump "):
                parts = line.split()
                mats[cur]["bump"] = parts[-1]
            elif cur and (line.startswith("map_Ns ") or line.startswith("map_refl ")):
                key = "rough" if line.startswith("map_Ns") else "metal"
                mats[cur][key] = line.split(None, 1)[1].strip()
            elif cur and line.startswith("map_Ke "):
                mats[cur]["emis"] = line.split(None, 1)[1].strip()
    return mats


def fix_from_mtl(pack_dir, fbx_path, mtl_path, materials_subdir="source/Materials"):
    idx = index_textures(pack_dir)
    mtl = parse_mtl(mtl_path)
    mat_dir = os.path.join(pack_dir, materials_subdir)
    remaps = []
    for name, maps in mtl.items():
        base = find_tex(idx, maps.get("base"))
        bump = find_tex(idx, maps.get("bump")) if maps.get("bump") else None
        metal = find_tex(idx, maps.get("metal") or maps.get("rough")) if (maps.get("metal") or maps.get("rough")) else None
        emis = find_tex(idx, maps.get("emis")) if maps.get("emis") else None
        # safe filename
        safe = re.sub(r"[^A-Za-z0-9_\-]+", "_", name)
        path = os.path.join(mat_dir, f"{safe}.mat")
        g = write_mat(path, name, base, bump, metal, emis, metallic=0.2 if metal else 0,
                      emission=bool(emis and emis != "00000000000000000000000000000000"))
        remaps.append((name, g))
        print(f"  MAT {name} base={base[:8]}...")
    patch_fbx_meta(fbx_path + ".meta", remaps)
    print(f"  REMAPPED {len(remaps)} -> {os.path.basename(fbx_path)}")


def enable_search_all_fbx(pack_dir):
    for dirpath, _, files in os.walk(pack_dir):
        for f in files:
            if f.endswith(".fbx.meta"):
                fp = os.path.join(dirpath, f)
                with open(fp, "r", encoding="utf-8", errors="ignore") as fh:
                    t = fh.read()
                t2 = re.sub(r"searchTexturesGlobally: \d+", "searchTexturesGlobally: 1", t)
                t2 = re.sub(r"materialImportMode: 2", "materialImportMode: 1", t2)
                if t2 != t:
                    with open(fp, "w", encoding="utf-8", newline="\n") as fh:
                        fh.write(t2)


def main():
    # --- NIGHT CITY ---
    print("\n== NIGHT_CITY_SKYLINE ==")
    pack = os.path.join(ROOT, "NIGHT_CITY_SKYLINE")
    fbx = os.path.join(pack, r"source\BACKGROUND NIGHT BUILDINGS SKYLINE_extracted\SOURCE\Background Buildings.fbx")
    mtl = fbx[:-4] + ".mtl"
    fix_from_mtl(pack, fbx, mtl)

    # --- LONDON ---
    print("\n== LONDON_SKYSCRAPER ==")
    pack = os.path.join(ROOT, "LONDON_SKYSCRAPER")
    fbx = os.path.join(pack, r"source\LONDON SKYSCRAPER_extracted\SOURCE\One_Blackfairs_Skyscraper.fbx")
    mtl = fbx[:-4] + ".mtl"
    fix_from_mtl(pack, fbx, mtl)

    # --- APARTMENTS ---
    for ap in ("APARTMENT_L3", "APARTMENT_L4"):
        print(f"\n== {ap} ==")
        pack = os.path.join(ROOT, ap)
        idx = index_textures(pack)
        base = find_tex(idx, "CP_Props_base_cranbearryGray.png")
        emis = find_tex(idx, "CP_Props_base_emission_colors.png")
        metal = find_tex(idx, "CP_Weapons_metalic.png")
        g = write_mat(os.path.join(pack, "source", "Materials", "Placeable.mat"),
                      "Placeable", base, None, metal, emis, metallic=0.1, emission=True)
        for dirpath, _, files in os.walk(pack):
            for f in files:
                if f.lower().endswith(".fbx"):
                    patch_fbx_meta(os.path.join(dirpath, f + ".meta"), [("Placeable", g)])
                    print(f"  remap {f}")

    # --- ELECTRICAL BOX ---
    print("\n== FACTORY_ELECTRICAL_BOX ==")
    pack = os.path.join(ROOT, "FACTORY_ELECTRICAL_BOX")
    idx = index_textures(pack)
    remaps = []
    for i, name in enumerate(("object_0", "object_1")):
        base = find_tex(idx, f"{i}_BaseColor.png")
        bump = find_tex(idx, f"{i}_Normal.png")
        metal = find_tex(idx, f"{i}_Metallic.png")
        g = write_mat(os.path.join(pack, "source", "Materials", f"{name}.mat"),
                      name, base, bump, metal, metallic=1, smoothness=0.5)
        remaps.append((name, g))
    fbx = os.path.join(pack, "source", "Factory_Electrical_Box.fbx")
    patch_fbx_meta(fbx + ".meta", remaps)

    # --- VICTORIAN ---
    print("\n== FACTORY_VICTORIAN ==")
    pack = os.path.join(ROOT, "FACTORY_VICTORIAN")
    idx = index_textures(pack)
    main_g = find_tex(idx, "main.png")
    roof_g = find_tex(idx, "tetto.png")
    # map textured ones; others still get main atlas as best effort
    mapping = {
        "mainMattoni": main_g,
        "altriBricks": main_g,
        "ciminiera": main_g,
        "Boiler": main_g,
        "Acciaio": main_g,
        "ghisa": main_g,
        "RuotaEsterna": main_g,
        "ruotaInterna": main_g,
        "porta": main_g,
        "phong1": main_g,
        "phong2": main_g,
        "phong3": main_g,
        "lambert1": main_g,
        "tettoFabbrica": roof_g,
        "vetro": main_g,
    }
    remaps = []
    for name, tg in mapping.items():
        g = write_mat(os.path.join(pack, "source", "Materials", f"{name}.mat"), name, tg, smoothness=0.25)
        remaps.append((name, g))
    fbx = os.path.join(pack, "source", "Fabbrica.fbx")
    patch_fbx_meta(fbx + ".meta", remaps)

    # --- HOUSE_1 ---
    print("\n== HOUSE_1 ==")
    pack = os.path.join(ROOT, "HOUSE_1")
    idx = index_textures(pack)
    # Material #2..#5 — assign available textures round-robin / by common sense
    tex_order = [
        find_tex(idx, "gray_asbestos_house_siding_background_1800.jpg", "gray_asbestos"),
        find_tex(idx, "rt-madison.jpg"),
        find_tex(idx, "T-R-C-Max-Reflect-Gypsum-Ceiling-tile-face.jpg", "Gypsum"),
        find_tex(idx, "wood__texture560.jpg", "wood"),
    ]
    remaps = []
    for i, name in enumerate(["Material #2", "Material #3", "Material #4", "Material #5"]):
        tg = tex_order[i % len(tex_order)]
        safe = f"Material_{i+2}"
        g = write_mat(os.path.join(pack, "source", "Materials", f"{safe}.mat"), name, tg)
        remaps.append((name, g))
    fbx = os.path.join(pack, "source", "fe6a53ca3ca24fa6ac881bdbfe32a255.fbx.fbx")
    patch_fbx_meta(fbx + ".meta", remaps)

    # --- WOODEN HOUSE ---
    print("\n== WOODEN_HOUSE_WOODS ==")
    pack = os.path.join(ROOT, "WOODEN_HOUSE_WOODS")
    idx = index_textures(pack)
    sets = {
        "wood": (find_tex(idx, "WoodPlanksWorn33_COL_VAR1_3K.jpg"), find_tex(idx, "WoodPlanksWorn33_NRM_3K.jpg")),
        "planks": (find_tex(idx, "Planks005_2K_Color.jpg"), find_tex(idx, "Planks005_2K_Normal.jpg")),
        "planks2": (find_tex(idx, "Planks011_2K_Color.jpg"), find_tex(idx, "Planks011_2K_Normal.jpg")),
        "brick": (find_tex(idx, "Bricks046_2K_Color.jpg"), find_tex(idx, "Bricks046_2K_Normal.jpg")),
        "concrete": (find_tex(idx, "Concrete002_2K_Color.jpg"), find_tex(idx, "Concrete002_2K_Normal.jpg")),
        "roof": (find_tex(idx, "RooftilesWood_diffuse.jpg"), find_tex(idx, "RooftilesWood_normal.png")),
    }
    # cycle materials across sets so house isn't monochrome
    mat_names = [f"Material.{str(i).zfill(3)}" for i in (1, 2, 3, 4, 6, 7, 8, 10, 11, 12)]
    # also Material without suffix if present - Unity may use Material.001 style only
    cycle = ["wood", "brick", "planks", "roof", "concrete", "planks2", "wood", "planks", "brick", "roof"]
    remaps = []
    for name, sk in zip(mat_names, cycle):
        base, bump = sets[sk]
        safe = name.replace(".", "_")
        g = write_mat(os.path.join(pack, "source", "Materials", f"{safe}.mat"), name, base, bump)
        remaps.append((name, g))
    fbx = os.path.join(pack, "source", "hous.fbx")
    patch_fbx_meta(fbx + ".meta", remaps)

    # --- BUILDING_1 remap existing URP mats ---
    print("\n== BUILDING_1 ==")
    pack = os.path.join(ROOT, "BUILDING_1")
    # Material_Building_04_1011 -> B04_M1, 1021->M2, 1031->M3, 1001->M4 (from earlier HDRP pack numbering)
    mat_map = {
        "Material_Building_04_1011": "B04_M1.mat",
        "Material_Building_04_1021": "B04_M2.mat",
        "Material_Building_04_1031": "B04_M3.mat",
        "Material_Building_04_1001": "B04_M4.mat",
    }
    remaps = []
    for mat_name, fname in mat_map.items():
        mp = os.path.join(pack, "Buildings", "Materials", "B04", fname)
        g = read_guid(mp + ".meta")
        if g:
            remaps.append((mat_name, g))
            print(f"  {mat_name} -> {fname} ({g})")
    fbx = os.path.join(pack, "Buildings", "Mesh", "Building 04.fbx")
    if remaps:
        patch_fbx_meta(fbx + ".meta", remaps)

    # --- BRUTALIST: enable search; create a fallback Lit if glb won't use it ---
    print("\n== BRUTALIST_BUILDING (search only; GLB embeds usually) ==")
    enable_search_all_fbx(os.path.join(ROOT, "BRUTALIST_BUILDING"))

    # enable search on everything under Buildings
    print("\n== enable searchTexturesGlobally on all Building FBXs ==")
    for name in os.listdir(ROOT):
        p = os.path.join(ROOT, name)
        if os.path.isdir(p):
            enable_search_all_fbx(p)
            print(" ", name)

    print("\nDONE")


if __name__ == "__main__":
    main()

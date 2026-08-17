# Free 3D Asset Sources — Bee Keeper Simulator

Where to get free 3D models for this project, organised by what the game
actually needs. Links checked August 2026; deep links to individual community
uploads rot faster than the sites themselves, so if one 404s, search the site
rather than assuming the source is dead.

**Read `Rules of the road` before downloading anything.** Two of those rules
(licence tracking and `.meta` handling) are cheap to follow from day one and
expensive to retrofit.

---

## Picking order for this project

Not all of these are equally worth your time. Suggested order:

1. **Poly Haven and Kenney/Quaternius first.** CC0, no attribution, consistent
   quality. If it exists there, stop looking.
2. **Then Poly Pizza / OpenGameArt / itch.io** for the long tail of props.
   Check each item's licence — these are mixed.
3. **Then Sketchfab**, filtered to downloadable + a licence you accept. Biggest
   library by far and the only realistic source for Eastern-European-specific
   architecture, but per-model licences and wildly variable topology.
4. **Model it in Blender** when the asset carries the game's identity — the
   hive roster, the bees, the landmark buildings. The `blender-artist` agent
   exists for this, and Appendix G of the worksheet already commits to
   hand-modelling the signature assets and buying/downloading the filler.
5. **AI generation last**, for greybox and one-off background props only.

---

## Rules of the road

- **Prefer CC0.** `PLAN.md` records the project as private/hobby *for now*, and
  the worksheet explicitly wants the door open to a public release later. CC0
  assets survive that transition with zero work; CC-BY survives it if you kept
  records; NonCommercial (`CC-BY-NC`) does not survive it at all.
- **Log every download in [`CREDITS.md`](CREDITS.md) as you download it.** One
  line per asset: what it is, where it came from, the author, the licence, the
  date. Reconstructing attribution for 200 props a year later is miserable and
  sometimes impossible. CC0 assets get a line too — attribution isn't required,
  but provenance still is.
- **Never `CC-BY-NC` or "personal use only."** Not because the project is
  commercial today, but because it makes a later release a re-art job.
- **Watch for "free" that means "free account required to download a licence
  you didn't read."** Marketplace free sections are the usual offenders.
- **Import through the Unity editor, not the shell.** Every imported asset gets
  a sibling `.meta` holding the GUID that scenes and prefabs reference. Drop
  files into `Assets/Art/...` with the editor open and let it generate the
  `.meta`. See `CLAUDE.md` — there is a hook that rejects orphaned `.meta`
  files, and it is protecting you from silently broken scene references.
- **Check the triangle count and the scale before committing to an asset.**
  Sketchfab in particular is full of 200k-triangle props modelled in
  centimetres. The sim view will have a whole district on screen at once.
- **Model in metres.** Unity scale 1.0 = 1 m. A hive is ~0.45 × 0.45 × 0.6 m.

---

## 1. Reachable directly from inside this project (Blender MCP)

The `blender-artist` agent can search and import from these without you
leaving the workflow — this is the fastest path from "I need a bench" to "there
is a bench in the scene."

| Source | What | Licence |
|---|---|---|
| [Poly Haven](https://polyhaven.com/models) | Models, HDRIs, textures | CC0 |
| [Sketchfab](https://sketchfab.com/features/free-3d-models) | Huge general library | Per-model — filter it |
| [Hyper3D / Rodin](https://hyper3d.ai/) | AI text/image → mesh | Per their terms |
| [Hunyuan3D](https://3d.hunyuan.tencent.com/) | AI text/image → mesh | Per their terms |

---

## 2. CC0, no attribution required — the default shelf

- **[Poly Haven](https://polyhaven.com/models)** — models, HDRIs and textures,
  all CC0, all high quality. Small model library but everything in it is good.
  Reachable via MCP.
- **[Kenney](https://kenney.nl/assets/category:3D)** — tens of thousands of
  game assets, all CC0, in consistent styles that mix and match. Directly
  on-point for the decor shop and the district blockout:
  [Furniture Kit](https://kenney.nl/assets/furniture-kit) (140 models),
  [Nature Kit](https://kenney.nl/assets/nature-kit) (330 models),
  [City Kit — Commercial](https://kenney.nl/assets/city-kit-commercial) and
  [City Kit — Industrial](https://kenney.nl/assets/city-kit-industrial).
  Stylised low-poly — a step away from the semi-realistic target, but
  unbeatable for greyboxing.
- **[Quaternius](https://quaternius.com/)** — large CC0 low-poly library,
  strong nature/city/prop packs, consistent style.
- **[ambientCG](https://ambientcg.com/)** — 2000+ CC0 PBR materials, HDRIs and
  some models. The go-to for concrete, asphalt, brick and roofing, which is
  most of what a Sknyliv street is made of.
- **[The Base Mesh](https://thebasemesh.com/)** — 1250+ CC0 base meshes with
  clean topology, UVs and real-world scale. Start modelling from one of these
  rather than from a cube.
- **[cgbookcase](https://www.cgbookcase.com/textures)** — CC0 PBR textures.
- **[3DTextures.me](https://3dtextures.me/)** — CC0 PBR textures.
- **[TextureCan](https://www.texturecan.com/)** — CC0 PBR textures.
- **[ShareTextures](https://www.sharetextures.com/)** — free PBR textures and
  some models.
- **[3D Models CC0 (itch.io)](https://3dmodelscc0.itch.io/)** — small CC0 prop
  packs, including a
  [City Environment Pack](https://3dmodelscc0.itch.io/city-environment-pack)
  (bench, street light, mailbox, dumpster, payphone, AC unit, water tower).
- **[Blender demo files](https://www.blender.org/download/demo-files/)** —
  official scenes, CC0 or CC-BY, good for studying construction.

## 3. Free, but mixed or attribution licences — check every item

- **[Poly Pizza](https://poly.pizza/)** — 10,000+ low-poly models including the
  rescued Google Poly archive. **Licences are mixed CC0 and CC-BY per model** —
  the Google Poly imports are mostly CC-BY 3.0. No login needed to download.
- **[OpenGameArt](https://opengameart.org/)** — old but deep, licences per
  item (CC0, CC-BY, GPL). Directly useful:
  [CC0 3D Plants](https://opengameart.org/content/cc0-3d-plants),
  [CC0 Furniture](https://opengameart.org/content/cc0-furniture),
  [3D furniture & interior/exterior decorables under CC0](https://opengameart.org/content/3d-furniture-and-other-interiorexterior-decorables-under-cc0).
- **[itch.io free 3D game assets](https://itch.io/game-assets/free/tag-3d)** —
  the most active source of small indie packs. Licence stated per pack.
- **[Sketchfab](https://sketchfab.com/features/free-3d-models)** — millions of
  models. Filter by *Downloadable* and by licence. Quality and topology vary
  enormously; treat everything as needing a retopo/decimate pass. Reachable via
  MCP.
- **[BlendSwap](https://www.blendswap.com/)** — `.blend` files under mixed
  licences (CC0, CC-BY, GPL). Good for oddities other sites lack.
- **[BlenderKit](https://www.blenderkit.com/)** — substantial free tier inside
  Blender itself; models, materials, HDRIs, scenes. Licence per asset.
- **[Thangs](https://thangs.com/)** — search engine across many model hosts,
  including geometric similarity search. Licences vary; heavily 3D-printing.
- **[Chocofur on Sketchfab](https://sketchfab.com/tags/chocofur)** — archviz
  furniture and interiors at a good realism level for the mansion. (Their own
  store reorganises its free section periodically; the Sketchfab tag is the
  stable entry point.)
- **[3D Warehouse](https://3dwarehouse.sketchup.com/)** — enormous, free,
  SketchUp-native. Topology is usually terrible and licence provenance is
  murky. Use for reference and measurement, not for shipping meshes.
- **[Smithsonian Open Access](https://3d.si.edu/)** — CC0 museum scans;
  occasionally has real insect and botanical specimens.
- **[GameDev Market free section](https://www.gamedevmarket.net/category/3d/)** —
  filter to free; licence per item.

## 4. Marketplace "free" sections — read the licence, they differ per item

- **[CGTrader free models](https://www.cgtrader.com/free-3d-models)** — large
  free tier, including [beehives](https://www.cgtrader.com/3d-models/beehive)
  and [bees](https://www.cgtrader.com/3d-models/bee). Royalty-free licence,
  usually fine for games; check per item.
- **[Free3D](https://free3d.com/)** — mixed free and paid; the free items'
  licences are inconsistently stated. Has
  [beehive](https://free3d.com/premium-3d-models/beehive) and
  [beekeeper](https://free3d.com/premium-3d-models/beekeeper) categories
  (the URLs say "premium", the listings are mixed free and paid).
- **[TurboSquid free](https://www.turbosquid.com/Search/3D-Models/free)** —
  small free selection under TurboSquid's standard royalty-free licence.
- **[RenderHub free](https://www.renderhub.com/free-3d-models)** — free tier
  including urban prop bundles.
- **[Fab](https://www.fab.com/)** (Epic's marketplace, successor to the Unreal
  Marketplace and Quixel) — has a free filter. **Caveat:** the 2024 promotion
  that made all of Quixel Megascans free for every engine **ended 31 Dec 2024**.
  What you claimed then is yours forever; what's free now is a smaller
  selection. Check each asset's licence for non-Unreal use before relying on it.
- **[Cults3D](https://cults3d.com/)**, **[Printables](https://www.printables.com/)**,
  **[MyMiniFactory](https://www.myminifactory.com/)** — 3D *printing* files.
  Cults3D has [210+ free beekeeping models](https://cults3d.com/en/tags/beekeeping).
  These are STL solids: no UVs, no sane topology, often no materials. Usable as
  modelling reference or after heavy retopo, not as game assets.

## 5. Unity-specific

- **[Unity Asset Store — 3D](https://assetstore.unity.com/3d)** (apply the
  *Free Assets* price filter in the sidebar; the store is a single-page app, so
  a bookmarked filter URL is not reliable) — drops straight into the project as
  a `.unitypackage`, already with
  materials and prefabs. Covered by the Asset Store EULA: fine to ship inside a
  game, **not** fine to redistribute as assets. Relevant free packs found:
  [Foliage Pack Free](https://assetstore.unity.com/packages/3d/vegetation/foliage-pack-free-66155),
  [Yughues Free Bushes](https://assetstore.unity.com/packages/3d/vegetation/plants/yughues-free-bushes-13168).
  Browse [3D Vegetation](https://assetstore.unity.com/3d/vegetation),
  [Flowers](https://assetstore.unity.com/3d/vegetation/flowers),
  [Trees](https://assetstore.unity.com/3d/vegetation/trees).
- **URP caveat.** Many older free packs ship Built-in-pipeline materials and
  import bright magenta. Fix with `Window → Rendering → Render Pipeline
  Converter`, or re-assign a URP/Lit material by hand. Check the pack's listed
  pipeline support before downloading.

---

## 6. By what this game actually needs

### Hives, bees, beekeeping

The hive roster is a signature asset and Appendix A of the worksheet defines
25 hive types across 7 tiers — those get hand-modelled (Blender), starting from
the `HiveBox_Placeholder.fbx` stage in `plans/hive-placement.md`. Download only
for reference and for background clutter.

- [Sketchfab: beehive](https://sketchfab.com/tags/beehive) ·
  [Sketchfab: bee](https://sketchfab.com/tags/bee)
- [CGTrader: beehive](https://www.cgtrader.com/3d-models/beehive) ·
  [CGTrader: bee](https://www.cgtrader.com/3d-models/bee)
- [Free3D: beehive](https://free3d.com/premium-3d-models/beehive) ·
  [Free3D: beekeeper](https://free3d.com/premium-3d-models/beekeeper)
- [TurboSquid: beehive](https://www.turbosquid.com/Search/3D-Models/beehive)
- [Cults3D: beekeeping](https://cults3d.com/en/tags/beekeeping) — printing
  files, but excellent dimensional reference for real Langstroth/Dadant parts.
- **Reference beats models here.** Langstroth and Dadant hives have published
  standard dimensions; a box is ten minutes of modelling from a spec sheet and
  will fit the game's scale better than anything you download.

### Flowers, vegetation, the park

Directly serves the decor/garden shop decision and Vyhovsky Park, which
`PLAN.md` calls the single biggest feature in the reference frame.

- [Poly Haven models](https://polyhaven.com/models) — CC0, filter to nature.
- [Quaternius nature packs](https://quaternius.com/) — CC0 low-poly.
- [Kenney Nature Kit](https://kenney.nl/assets/nature-kit) — CC0.
- [OpenGameArt CC0 3D Plants](https://opengameart.org/content/cc0-3d-plants) —
  trees, grass, flowers, mushrooms, bushes, ferns.
- [Simple Vegetation Pack (CC0)](https://takyin.itch.io/simple-vegetation-pack) —
  3 trees, 2 pines, 2 flower types in 4 colours, trunks.
- [itch.io plants + Unity](https://itch.io/game-assets/tag-plants/tag-unity)
- **Generators, since you need eight specific bee plants and nobody has a
  free asset called "phacelia":**
  [Sapling Tree Gen](https://extensions.blender.org/add-ons/sapling-tree-gen/)
  (a Blender extension), [Tree It](http://www.evolved-software.com/treeit/treeit)
  (free standalone tree generator), and Blender's geometry nodes for scattering a flower bed from one or
  two petal meshes. For the roster in `PLAN.md` — willow, fruit blossom,
  acacia, linden, phacelia, clover, sunflower, goldenrod — building one
  parametric flower-bed asset and reskinning it eight times is far less work
  than sourcing eight botanically correct models.

### Furniture and interior decor

For the mansion rebuild, the apiary support building, and the cosmetic half of
the decor shop.

- [Kenney Furniture Kit](https://kenney.nl/assets/furniture-kit) — CC0, the
  single best starting point.
- [Furniture Kit GLB Pack — 140 free CC0 models](https://eclair-assets.itch.io/furniture-kit-glb-pack-140-free-cc0-3d-models) —
  the Kenney kit repackaged as GLB.
- [Free Low-Poly Furniture, 22 props (CC0)](https://quin-gs.itch.io/furniture-lowpoly-cc0) —
  all under 500 tris except one.
- [OpenGameArt CC0 Furniture](https://opengameart.org/content/cc0-furniture)
- [Chocofur](https://store.chocofur.com/) — realistic archviz furniture,
  closer to the semi-realistic target; free section moves around, and
  [their Sketchfab tag](https://sketchfab.com/tags/chocofur) is the stable
  entry point.
- [BlenderNation: 50 free CC0 furniture models](https://www.blendernation.com/2021/12/11/50-free-cc0-furniture-3d-models-for-blender/) ·
  [more CC0 archviz props](https://www.blendernation.com/2022/11/02/new-free-cc0-archviz-3d-models-benianus-3dnew-free-cc0-archviz-3d-models-benianus-3d/)

### Sknyliv buildings — Soviet-era panel housing

The hardest category to source, because generic Western suburban assets will
not read as Lviv. The residential blocks in the reference polygon are mostly
*panelki* / khrushchyovka-type panel housing.

- [Sketchfab collection: Buildings-Panelki-Free](https://sketchfab.com/evaddugina/collections/buildings-panelki-free-252e5d3977eb4567a1e12edc5112cc33)
  — the single most on-target find; a curated collection of free Soviet panel
  buildings.
- [Low Poly Soviet Apartment Building 8K](https://sketchfab.com/3d-models/low-poly-soviet-apartment-building-8k-05229ac1d1f94e6c8cacaad91110c602)
- [Khrushchyovka (Eastern Europe panel house)](https://sketchfab.com/3d-models/khrushchyovka-eastern-europe-panel-house-146ee35e1ef743a284150c05cf4b9b54)
- [CGTrader: khrushchyovka](https://www.cgtrader.com/3d-models/architectural/floor/khrushchyovka) ·
  [Soviet panel building](https://www.cgtrader.com/3d-models/architectural/architectural-street/soviet-panel-building)
- [TurboSquid: soviet architecture](https://www.turbosquid.com/3d-model/architecture?keyword=soviet)
- **This category argues for modularity over downloads.** `PLAN.md` commits to
  the whole polygon as low-detail extruded footprints first. A panel building
  is, genuinely, a repeated concrete panel — a modular kit of 3–4 panel
  variants, a balcony, a stairwell entrance and a roof edge will build the
  entire district and look more coherent than a dozen mismatched downloads.

### Street props and city furniture

- [Free CC0 City Environment Pack](https://3dmodelscc0.itch.io/city-environment-pack)
  — 11 props, CC0.
- [Free Lowpoly City Props Pack by MaHa](https://sketchfab.com/3d-models/free-lowpoly-city-props-pack-by-maha-c81469186c6f442e88dc8a29fedcd082)
  — utilities, signs, benches, bins, bus stop; one trim sheet, low poly.
- [Free Low Poly Simple Urban City Asset Pack](https://sketchfab.com/3d-models/free-low-poly-simple-urban-city-3d-asset-pack-310c806355814c3794f5e3022b38db85)
  — 90+ models, 35 street props.
- [Street Asset Pack](https://sketchfab.com/3d-models/street-asset-pack-f3eb47d02e2e4ab290e66752fa354b48)
  — signs, barriers, cones, hydrant, bins.
- [Kenney City Kit](https://kenney.nl/assets/city-kit-commercial) — CC0.

### Player character, animation, NPCs

- **[Mixamo](https://www.mixamo.com/)** — free rigged characters and a huge
  animation library, free with an Adobe account. This is the standard answer
  for the on-foot mode's walk/idle/interact set.
- **[MakeHuman](http://www.makehumancommunity.org/)** — free character
  generator; output is CC0.
- [Quaternius character packs](https://quaternius.com/) — CC0, stylised.
- Note: a beekeeper suit is a distinctive silhouette and probably worth
  modelling over the base character rather than sourcing.

### Vehicles

The worksheet names a car and a bicycle in the travel loop.

- [Kenney Car Kit](https://kenney.nl/assets/car-kit) — CC0, modular.
- [Quaternius vehicle packs](https://quaternius.com/) — CC0.
- [Sketchfab: vehicles](https://sketchfab.com/tags/car), filtered to
  downloadable + acceptable licence.

### Materials, HDRIs, skies

- [Poly Haven HDRIs](https://polyhaven.com/hdris) — CC0, the standard source.
- [Poly Haven textures](https://polyhaven.com/textures) — CC0.
- [ambientCG](https://ambientcg.com/) — CC0, best for concrete/asphalt/brick.
- [cgbookcase](https://www.cgbookcase.com/textures) ·
  [3DTextures.me](https://3dtextures.me/) ·
  [TextureCan](https://www.texturecan.com/) — all CC0.

---

## 7. AI generation — greybox and background props only

Useful for "I need a specific ugly thing and nobody made it." Topology is
generally poor, so treat output as a starting mesh or a distant prop.

- **[Hyper3D / Rodin](https://hyper3d.ai/)** and
  **[Hunyuan3D](https://3d.hunyuan.tencent.com/)** — both wired into this
  project's Blender MCP already, text- and image-to-mesh.
- **[Meshy](https://www.meshy.ai/)** — has pre-made
  [beehive](https://www.meshy.ai/tags/beehive) and
  [honeybee](https://www.meshy.ai/tags/honeybee) libraries; the site states
  its pre-made assets are CC0.
- **[Tripo3D](https://www.tripo3d.ai/)** — text/image to mesh, free tier.
- **Check the terms before shipping AI-generated meshes**, especially if the
  project ever goes public. Free tiers frequently licence output differently
  from paid tiers.

## 8. Reference data, not models

`PLAN.md` decided to hand-build the district from the map screenshot rather
than import OSM or Google 3D Tiles — no API keys, no licensing, full stylistic
control. These stay useful as *reference* under that decision:

- [OpenStreetMap](https://www.openstreetmap.org/) — street layout and building
  footprints for Sknyliv, ODbL. Reference for accuracy; importing the geometry
  would pull ODbL obligations into the project.
- [Overpass Turbo](https://overpass-turbo.eu/) — query OSM for e.g. every tree
  or building footprint in the polygon, as measurement reference.
- `Part_Of_the_city.jpg` and `map for game.png` in this repo — the primary
  visual reference.
- Appendix C of the GDD worksheet — the mapped JSON of the target territory.

---

## Licence cheat sheet

| Licence | Attribution | Commercial | Safe for a later public release? |
|---|---|---|---|
| CC0 / Public Domain | No | Yes | Yes — no obligations at all |
| CC-BY | **Yes** | Yes | Yes, *if* you kept `CREDITS.md` |
| CC-BY-SA | **Yes** | Yes | Risky — share-alike can reach your derivatives |
| CC-BY-NC | Yes | **No** | **No** — avoid entirely |
| Unity Asset Store EULA | No | Yes | Yes in a game; never redistribute the assets |
| Royalty-free (CGTrader/TurboSquid) | Usually no | Yes | Usually yes; read the per-item terms |
| "Free for personal use" | — | **No** | **No** — avoid entirely |
| GPL (some BlendSwap `.blend`s) | Yes | Yes | Art assets are generally fine; ask before code |

**When in doubt, don't download it.** An asset you can't licence-verify is a
liability that surfaces years later, at the worst moment, in a file you no
longer remember adding.

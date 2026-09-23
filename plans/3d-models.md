# Plan — 3D Models (Blender → Unity)

## What this is for

One place that answers three questions: **what 3D models the game needs**,
**what already exists or has been found**, and **in what order the models get
made and brought into Unity**. The order follows the build plans: the
hive-ritual first playable (`plans/hive-ritual.md`) gets its models first; the
big rosters (25 hives, 15 breeds, the district) wait until the fun checkpoint
(`QUESTIONS.md` #44) says the loop is worth dressing.

The split between modelling and downloading is already decided: hand-model
the assets that carry the game's identity (the hive roster, the bees, landmark
buildings), download the filler (`CLAUDE.md`, `ASSET_SOURCES.md`, GDD
worksheet Appendix G).

---

## 1. What already exists (checked 2026-09-23)

### In-house Blender work — `Assets/blender assets/Untitled.blend`

Saved with **Blender 5.2** (installed at
`~/Desktop/blender-5.2.0-linux-x64/`). Measured 2026-09-23 with Blender in
background mode; the scene uses metric units at scale 1.0.

| Model | Objects in the file | Materials | Renders | Maps to in the game |
|---|---|---|---|---|
| **Worker, drone, queen bees** (v1.0) | `Bee_Worker`, `Bee_Drone`, `Bee_Queen`, each with a separate `_Wings` object | `Bee_Amber`, `Bee_Chitin`, `Bee_Eye`, `Bee_Fuzz`, `Bee_Wing` | `imagesAndSoOn/beesv1.0-dorsal.png`, `beesv1.0-threequarter.png` | The three castes of every managed colony (Appendix B caste model). Golden colouring = Italian. |
| **Wild hive** (v2.0) | `WildHive_Branch`, `WildHive_Attachment`, `WildHive_Comb0`–`Comb6`, `Comb0_Cells`, `Comb6_Cells` | `WildHive_Bark`, `WildHive_WaxNew`, `WaxMid`, `WaxOld` (wax ageing) | `imagesAndSoOn/beehivev1.0.png`, `beehivev2.0.png` | A wild colony found in the woods (GDD "locating / taming wild colonies"), and the base for a hanging swarm. Not a placeable hive. |

Also in the file: render cameras and lights (`BeeCam`, `BeeKey`, `BeeFill`,
`PreviewCam` ×5) — scene setup, not game assets.

Nothing from this file has been exported to Unity yet.

**Measurements and what they mean for export:**

| Object | Triangles | Size (m) | Notes |
|---|---|---|---|
| `Bee_Worker` + wings | 1,922 + 112 | 0.015 long | True real-world size |
| `Bee_Drone` + wings | 1,922 + 112 | 0.018 long | True real-world size |
| `Bee_Queen` + wings | 1,994 + 112 | 0.020 long | True real-world size |
| `WildHive_Comb0`–`Comb6` | 6,072 each (42,504 total) | 0.6–1.0 wide, 1.3–1.8 tall | Unapplied Solidify modifier on each |
| `WildHive_Comb0_Cells`, `Comb6_Cells` | 3,992 + 3,790 | ~0.45 × 1.18 | Cell detail on the two outer combs only |
| `WildHive_Attachment` | 1,216 | 1.21 × 0.54 × 0.71 | |
| `WildHive_Branch` | 60 | 2.6 long | |
| **Wild hive total** | **≈ 51,600** | | |

- **Bees are export-ready in shape.** Real scale, about 2k triangles each,
  wings separate objects parented to the body (good for flapping). No rig,
  shape keys or animation yet. Close to the "hero" budget; a much lighter
  version is still needed for crowds.
- **No UV maps on the bees or the combs**, and every material is a
  procedural node setup with no image textures. Procedural Blender materials
  don't travel through FBX: each needs UVs plus baked textures, or a simple
  URP material rebuilt in Unity (enough for the bees' flat colours).
- **The wild hive is too heavy for a game prop** at ~51.6k triangles
  (budget: ≤ 10k). Needs Solidify applied, then decimation or a lighter
  rebuild of the five inner combs, which are hidden behind the outer ones.
- **The wild hive is very large** — combs up to 1.8 m tall. A real wild
  colony's combs are usually well under a metre; worth checking this was
  intended before it becomes the in-game wild colony.

### Placeholder primitives in the game now

`Assets/Scripts/Editor/HiveRitualSceneBuilder.cs` builds the yard from grey
boxes. Every one of these is a model slot waiting to be filled, and its size is
the working spec until a real model replaces it:

| Placeholder | Parts (as built) | Size now | Code that depends on its parts |
|---|---|---|---|
| Hive | Stand, Body, Entrance, Lid, `Frames` ×10 | Body 0.5 × 0.5 × 0.5 m on a 0.3 m stand; lid 0.58 × 0.08 m | `HiveInteraction.lid` lifts the lid off |
| Frame (×10) | Top Bar, Honey band, Brood band, one box collider | Top bar 0.46 m; comb 0.42 × 0.40 m; 45 mm apart | `HiveFrame` rescales the Honey/Brood bands from the colony's stores |
| Smoker (held) | Canister, Nozzle, SmokePuff particles | Canister Ø0.1 × 0.2 m | `Smoker.puffParticles` sits at the nozzle tip |
| Honey stand | Table top, 4 legs, sign post, sign, 3 jars | Table 1.2 × 0.6 m, 0.8 m high | `HoneyStand` on the root |
| Shed | Walls, Roof, Door | 3 × 2.4 × 2.5 m | `SleepSpot` on the door |
| Ground | Plane | 40 × 40 m | — |

### Downloaded third-party models

**None.** The 3D tables in `CREDITS.md` are empty, and there are no
`.fbx` / `.obj` / `.glb` / `.gltf` files anywhere in the project or in
`~/Downloads`, `~/Desktop`, `~/Documents`.

### Found but not downloaded (candidates in `ASSET_SOURCES.md`)

Named items and packs already identified. Licence must still be checked per
item before downloading, and each download gets a `CREDITS.md` line.

| Need | Candidates found | Licence |
|---|---|---|
| Garden / indoor furniture (decor shop, mansion) | Kenney Furniture Kit (140 models), Quin's Low-Poly Furniture (22 props), OpenGameArt CC0 Furniture, Chocofur (Sketchfab tag) | CC0 (Kenney, Quin, OGA); Chocofur per model |
| Vegetation, the park | Kenney Nature Kit (330), Quaternius nature packs, Poly Haven models, OpenGameArt CC0 3D Plants, Simple Vegetation Pack, Unity *Foliage Pack Free*, *Yughues Free Bushes* | CC0; Unity Asset Store EULA for the two Unity packs |
| Sknyliv panel housing | Sketchfab *Buildings-Panelki-Free* collection, *Low Poly Soviet Apartment Building 8K*, *Khrushchyovka (Eastern Europe panel house)*, CGTrader khrushchyovka / soviet panel building | Per model |
| Street props | CC0 *City Environment Pack* (11 props), *Free Lowpoly City Props Pack by MaHa*, *Free Low Poly Simple Urban City Asset Pack*, *Street Asset Pack*, Kenney City Kits | CC0 / per model |
| Player & NPCs | Mixamo (rigged characters + animations), MakeHuman (CC0), Quaternius characters | Adobe terms / CC0 |
| Vehicles | Kenney Car Kit, Quaternius vehicles | CC0 |
| Materials & skies | Poly Haven HDRIs + textures, ambientCG (concrete, asphalt, brick), cgbookcase, 3DTextures.me, TextureCan | CC0 |

Hives and bees are deliberately **not** on this list: they're identity
assets, so downloads are reference only.

### Reference material for modelling (never shipped)

- **GDD Appendix A, plates A and B** — schematic silhouettes of 12 hive forms:
  bort, koloda, decorative log hive, skep, sapetka, barrel hive; Sun Hive,
  urban sculptural tower, wall observation hive, hexagonal auto-flow hive,
  Flow super + tap, smart/IoT hive.
- **GDD Appendix A roster** — all 25 hive types with frame counts and roles.
- **GDD Appendix B "Art & model notes"** — how each breed should look
  (Italian golden, Carniolan/Carpathian grey, Caucasian dark with a long
  tongue, Russian/African/European Dark near-black, solitary bees
  deliberately off-model) and the caste size order worker < drone < queen.
- `imagesAndSoOn/different bee types/` and
  `sizes-honeybee-worker-queen-drone.webp` — breed and caste photos.
  Provenance unknown, so reference only (`CREDITS.md`).
- Published Dadant / Langstroth dimension sheets, and Cults3D beekeeping
  print files as dimensional reference (`ASSET_SOURCES.md`).

---

## 2. What the game needs

### Tier 1 — the hive-ritual slice (next)

These replace the grey boxes above, so the fun checkpoint is played with
something that reads as a real apiary.

| # | Model | Make or get | Budget (placeholder) | Must-haves for the code |
|---|---|---|---|---|
| 1 | **Honey jar** | Model (small, but it's the product) | ≤ 300 tris | Pivot at base centre. Doubles as the pipeline test. |
| 2 | **Starter hive** — stand, landing board, hollow body, lid | Model | ≤ 3k tris | Body **hollow** with an open top so frames show when the lid is off (the grey box is solid). Lid a **separate object** (it animates). Pivot at base centre, sits on y = 0. |
| 3 | **Frame + comb** — top bar with lugs, side and bottom bars, comb | Model; comb texture is the real work | ≤ 500 tris per frame (×10–12 per hive) | The close-up hero asset, seen at 45 cm. Capped honey, capped brood, open cells, eggs. Honey fill must still be adjustable (see open question 3). |
| 4 | **Smoker** (held) — canister, bellows, nozzle | Model | ≤ 1.5k tris | Seen close in first person. Bellows separate if it will animate. An empty at the nozzle tip for the smoke. |
| 5 | **Bees for the slice** — v1 worker; a crowd version | Existing v1 + a low-poly version | Hero ≤ 2k, crowd ≤ 150 tris | Bees at the entrance and on frames will be many copies: needs a very light version, wings that can flap (shape key or shader), colour set by material so breeds recolour without new meshes. |
| 6 | **Yard dressing** — grass/ground material, trees, bushes, fence, sky | Download (Poly Haven, Quaternius, Kenney Nature, ambientCG) | Per asset | Log each in `CREDITS.md`. |
| 7 | **Shed** (apiary support building, exterior) | Model (simple) | ≤ 2k tris | Door a separate object (it's the sleep spot now, and will open later). |
| 8 | **Honey stand** — table, sign | Model or download a table | ≤ 1k tris | Uses the jars from #1. |

### Tier 2 — after the fun checkpoint

Listed so the size of the job is visible; not scheduled yet.

- **Hive roster (Appendix A, 25 types).** Build a **modular framed-hive kit**
  once (bodies, supers, lids, bottoms, stands, frames in 2–3 standard sizes)
  and assemble most framed hives from it. The Tier 0–1 and Tier 6 forms on
  plates A and B are one-offs. Order by when the game unlocks them: Tier 2
  (nucleus, Dadant, lezhak) → Tier 3 (Ukrainian lezhak, 12-frame + magazine,
  two-body) → Tier 0 heritage (plate A) → Tier 1 (plate B) → Tiers 4–6.
- **Bee breeds (Appendix B, 15).** Mostly materials on the existing meshes,
  not new models. New meshes: Asian honey bee (smaller), the four solitary
  bees, and a bee hotel to house them.
- **Wild colonies and swarms** — the existing wild hive, plus a swarm cluster
  hanging on a branch (same kit).
- **Tools and workshop** — hive tool, bee brush, suit (first-person gloves),
  honey extractor, uncapping knife, feeder, queen cage, workbench.
- **Decor shop** — one parametric flower-bed asset reskinned for the eight bee
  plants in `PLAN.md` (`ASSET_SOURCES.md` explains why), water features
  (waterer, stone basin, pond, birdbath), garden and indoor furniture
  (download).
- **Buildings** — the grandfather's mansion (landmark: model), a modular
  panel-housing kit for Sknyliv, generic replacements for the named shops.
- **People and vehicles** — first-person arms, grandfather and vendor NPCs,
  bicycle, car.

---

## 3. Pipeline: Blender → Unity

Conventions to settle once, on the first model (the honey jar), and then
follow for everything:

- **Where source files live.** Recommended: `.blend` files **outside**
  `Assets/` (for example `ArtSource/`), with only exported FBX files in
  `Assets/Art/<Category>/`. A `.blend` inside `Assets/` makes Unity run Blender
  to import it, which needs Blender installed on every machine that opens the
  project and pulls in cameras and lights too. `Untitled.blend` has no `.meta`
  yet, so it can still be moved without breaking anything.
- **Names.** One `.blend` per subject (`Bees.blend`, `WildHive.blend`,
  `Hive_Starter.blend`), meshes as `SM_<Thing>` (`SM_Hive_Starter`,
  `SM_Frame`), materials as `M_<Surface>`. Parts the code moves keep plain
  names (`Lid`, `Door`, `Frame 1`…) so the scene builder can find them.
- **Scale and axes.** Metres, scale 1.0. FBX export: *Apply Scalings: FBX
  All*, *Forward: -Z*, *Up: Y*, *Apply Transform* on. Check it once with a
  1 m cube: it should import at 1 m with no rotation.
- **Pivots.** Base centre for anything that stands on the ground; the grip for
  held tools; the resting edge for parts that open (lid, door).
- **Materials.** URP Lit. Base colour + normal + a mask texture where it
  earns its keep. Textures 2K for the frame close-up, 1K for most props, 512
  for small items.
- **Colliders.** Simple box colliders added in Unity (the builder does this),
  never mesh colliders on props.
- **Import.** Always with the Unity editor open, so it writes the `.meta`.
- **Hooking up.** Each finished model gets a prefab, and
  `HiveRitualSceneBuilder` swaps the matching primitives for that prefab. That
  code change comes with each model.
- **Provenance.** Every download and every AI-generated mesh gets a
  `CREDITS.md` line when it arrives (AI ones with the prompt).
- **Who models.** The `blender-artist` agent (through the Blender MCP), or you
  by hand in Blender 5.2.

---

## 4. Order of work

| Step | What | Done when |
|---|---|---|
| 0 | **Setup.** ~~Measure `Untitled.blend`~~ (done — section 1). Reconnect the Blender MCP in Claude Code (`/mcp`), optionally update the Blender add-on. Decide where source files live and rename `Untitled.blend`. | The `blender-artist` agent can reach Blender; the file has a real name and home. |
| 1 | **Pipeline test: honey jar.** Model → FBX → Unity → prefab → it replaces the three jars on the honey stand. | The jar imports at the right size and orientation, with a working URP material, placed by the scene builder. |
| 2 | **Starter hive** (needs open question 1). | The hive in the yard is the model; the lid still lifts off and frames show inside. |
| 3 | **Frame + comb** (needs open question 3). | A pulled frame looks like real comb up close; its honey still changes with the colony's stores. |
| 4 | **Smoker.** | The held smoker is the model; smoke comes out of the nozzle. |
| 5 | **Bees.** Export the v1 worker; make the crowd version; flapping wings. | Bees fly around the entrance and sit on pulled frames without a visible frame-rate drop. |
| 6 | **Yard dressing** (downloads). | Grass, a few trees, a fence and a sky, each logged in `CREDITS.md`. |
| 7 | **Shed and honey stand.** | No grey boxes left in the yard. |

After step 7: the fun checkpoint, then Tier 2 in whatever order it says.

This replaces Stage 2 of `plans/hive-placement.md` (a `HiveBox_Placeholder.fbx`
for the top-down camera). That plan is on hold, and the starter hive in step 2
is built for the first-person close-up instead.

---

## 5. Open questions this plan depends on

1. **Which hive is the starter hive?** Dadant (12 frames, ≈ 435 × 300 mm
   brood frame, the common Ukrainian choice), Langstroth, or a Ukrainian
   lezhak? GDD Appendix G proposes a Dadant with a Carpathian nucleus, but
   `PLAN.md` hasn't recorded it. This sets the body size, the frame size and
   count, and three code constants (`kFrameCount`, `kFrameSpacingMeters`,
   `kCombHeightMeters`). The placeholder's 10 frames at 45 mm spacing aren't
   a real hive's; real frames sit about 35–38 mm apart. Settle with the
   `gdd` skill.
2. **How close to real does the close-up go?** "Semi-realistic" is decided;
   whether a pulled frame shows individual cells and eggs sets the texture
   budget for the one asset the player stares at most.
3. **How does honey fill show on a real comb?** Today the code stretches two
   box-shaped bands. On a modelled comb that becomes either a modelling rule
   (bands 1 m tall with the pivot at the centre, so the stretch still works)
   or a small shader that fills the comb texture from a 0–1 value. The shader
   looks better and lets the comb show cells properly.
4. **Hands in first person?** Gloved hands and sleeves holding the tools, or
   tools floating as they do now (`QUESTIONS.md` #27 touches this).

---

## Blockers right now

- **The Blender MCP needs reconnecting in Claude Code.** It failed at the
  start of the 2026-09-23 session because the first run of `uvx
  blender-mcp` still had to download its packages (it now starts in ~4 s).
  With Blender open and the add-on's server running on `localhost:9876`, run
  `/mcp` in Claude Code and reconnect `blender`.
- **The Blender add-on is older than the bridge.** The bridge (`blender-mcp`
  2.0.0) warns: *"Blender addon is outdated … Run `uvx mcp-for-blender
  install-addon` to update it, then restart Blender or disable/enable
  'Interface: MCP for Blender', then Start MCP Server. Fallbacks keep working
  in the meantime."* Basic tools work without the update.
- The new scripts, and `Untitled.blend` if it stays in `Assets/`, have **no
  `.meta` files** until Unity is opened. Open Unity once and commit what it
  generates.

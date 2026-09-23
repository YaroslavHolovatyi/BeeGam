# Bee Keeper Simulator — Project Status & Answer Sheet

*Snapshot: 2026-09-23. Branch `docs/farming-cut-and-asset-sources` (pushed to
GitHub, not yet merged into `main`).*

**How to use this document**

1. Sections 1–6 say where the project stands: what exists, what doesn't, what
   is broken in the docs, and what to do next.
2. Section 7 is the **answer sheet**: every open question in the project, in
   one place. Write your answer after **Answer:**. Writing `agree` accepts the
   proposal shown; `skip` or leaving it blank keeps it open — that's fine.
   ⭐ questions block the next piece of work; answer those first.
3. Then tell Claude *"record my answers"*. Each answer gets written into
   `PLAN.md` with the `gdd` skill, retired from `QUESTIONS.md`, and removed
   from this sheet. `PLAN.md` stays the source of truth for decisions.

---

## 1. The game, as decided so far

A single-player beekeeping sim in Unity 6 (URP), set in a genericized,
hand-built ~2 km slice of the Sknyliv district of Lviv. The player spends most
of the game **on foot, in first person**, working hives by hand: smoke, open,
pull frames, harvest, sell. A **top-down management view unlocks late**, when
the player buys an in-game computer. Bees are **real subspecies** with real
trade-offs, stored as data. Art is **semi-realistic**. **Vertical farming is
cut**; a buy-and-place decor and garden shop replaces it, where bee plants and
water affect the bees and everything else is cosmetic. Progression is
**milestone goals, then sandbox**. The **first playable** is one hive in a
small yard (the "hive ritual").

Your GDD draft (sections 1–7 of the worksheet) adds ideas that are **not yet
recorded in `PLAN.md`**: the grandfather teaches the tutorial; the long goal is
rebuilding his mansion as a solarpunk home; a support building near it holds a
workbench for building and repairing hives; wild and runaway bee families can
be found and caught; books teach new techniques; the game should teach real
beekeeping; it's for PC and Steam Deck. Questions P3, V5 and G1–G4 below ask
you to confirm them.

---

## 2. The documents

| Document | What it is | State |
|---|---|---|
| `PLAN.md` | Decisions, each with its reason and trade-off | **Source of truth.** Latest: first-person perspective (2026-09-23). The city approach is still marked "proposed, not yet confirmed" (W3). |
| `QUESTIONS.md` | Menu of open questions, #1–#50 | All of them are gathered in section 7 here. |
| `BeeKeeper_GDD_Worksheet.docx` | The big design worksheet (Diablo-pitch structure) | **You edit this one.** Sections 1–7 hold your draft answers. Appendices: **A** 25 hive types in 7 tiers + schematic plates; **B** 15 breeds + art notes; **C** map JSON (its heading wrongly says "Appendix B"); **D** outside design review; **E** ~60 mechanics with build tiers; **F** questions #41–#95; **G** proposed answers; **H** extra ideas. **D–H are proposals, not decisions** — only G's farming cut is in `PLAN.md`. |
| `BeeKeeper_GDD_Worksheet.md` | Generated text copy of the `.docx` | New. So Claude can read the worksheet. Never edit it; after editing the `.docx` run `python3 .claude/scripts/gdd-to-markdown.py`. |
| `plans/project-status.md` | This file | Hub + answer sheet. |
| `plans/hive-ritual.md` | Build plan for the first playable, 6 steps | **Active.** Steps 1–5 coded; step 6 is playing it. |
| `plans/3d-models.md` | Models we have / need, Blender → Unity pipeline, model order | **Active.** Step 0 half done (measurements taken). |
| `plans/hive-placement.md` | Top-down hive placement plan | **On hold** since the perspective decision. |
| `ASSET_SOURCES.md` | Where to get free models and textures, by need | Reference. |
| `CREDITS.md` | Provenance ledger for third-party assets | No 3D entries — nothing downloaded yet. |
| `CLAUDE.md` | Instructions for every Claude session | Loads this file, `PLAN.md`, `QUESTIONS.md` and the two active plans. |
| `README.md` | Two-line stub ("BeeGam / mySmallTinyProject") | Needs writing someday. |

---

## 3. What we have

### 3.1 Decided design (`PLAN.md`)

- **Perspective:** on foot, first person, ~80% of play.
- **Top-down view:** unlocked late by buying an in-game computer.
- **First playable:** the hive ritual — one hive, one breed, one small yard.
- **Bees:** real subspecies; starting roster Italian, Carniolan, Buckfast,
  Russian; breeds are data (`BeeBreedData` assets), not code.
- **Setting:** a bounded area of Sknyliv, Lviv; real business names replaced
  with generic ones.
- **City:** hand-built from the map reference, whole area at low detail first
  (approach still marked "proposed").
- **Art style:** semi-realistic (Farming Simulator / Cities: Skylines
  register).
- **Farming cut;** decor and garden shop instead. Bee plants and water are
  functional, everything else is cosmetic; outdoor pieces on owned land,
  indoor pieces inside buildings.
- **Progression:** milestone/career goals with a soft win, then sandbox.
- **Time:** accelerated (direction only, no numbers).
- **Single-player;** simulation kept separate from input.
- **Release:** private hobby project for now.

### 3.2 Code — Unity 6 (`6000.5.4f1`), URP

About 1,500 lines. Everything compiles with 0 warnings, and each build step
was checked by the `unity-reviewer` agent. **None of it has been played in
Unity yet.**

| Script | Folder | What it does |
|---|---|---|
| `BeeBreedData` | Data | Breed stats as data: yield, temperament, disease resistance, cold tolerance, foraging range. |
| `HiveController` | Simulation | One colony: population, health, queen status, stored honey. Forages 07:00–20:00 on in-game time. `Harvest()` leaves a 20% reserve. |
| `HiveTicker` | Simulation | Steps every hive every 0.25 in-game hours, so cost doesn't grow per frame with hive count. |
| `GameClock` | Simulation | In-game day and time; 20 real minutes per day (placeholder); sleep to morning. |
| `EconomyManager` | Simulation | Gold (starts 100), honey price (15 per kg), sell and spend. |
| `FirstPersonController` | OnFoot | Walk (WASD), look (mouse), sprint (Shift), Esc frees the cursor. |
| `Interactable`, `PlayerInteractor` | OnFoot | Look at something, press E (or F) to use it; prompt under the crosshair. |
| `HiveInteraction` | OnFoot | Smoke calms the hive; E lifts the lid off / puts it back; F harvests. |
| `HiveFrame` | OnFoot | Pull a frame up to your eyes; the comb shows the honey; text reads bees, brood and eggs. |
| `Smoker` | OnFoot | Left mouse puffs smoke. |
| `SleepSpot`, `HoneyStand`, `PlayerInventory` | OnFoot | Sleep until morning; sell carried honey; carried honey in kg. |
| `PlayerHud` | UI | Crosshair, prompts, messages, clock, carried honey and gold. |
| `HiveRitualSceneBuilder` | Editor | Menu **BeeKeeper → Build Hive Ritual Scene** builds the yard from grey boxes. |

Other project content:

- **Input:** the template's project-wide actions, plus a new **Secondary (F)**
  action.
- **Scenes:** `SampleScene` (empty template), `test_01.unity` (scratch: one
  hive + economy; its hive no longer makes honey without a clock),
  `HiveRitual.unity` (**doesn't exist until you run the menu item**).
- **Data:** `ItalianBeeBreed.asset` — all stats still at their defaults.
- **Packages:** Input System 1.19, URP 17.5, Test Framework 1.7, uGUI 2.5,
  AI Navigation, Timeline.

### 3.3 Art

- **In-house Blender work** (`Assets/blender assets/Untitled.blend`, Blender
  5.2): worker, drone and queen bees — real size, ~2k triangles each,
  separate wings, but no UV maps and procedural materials. A wild hive — ~51.6k
  triangles (too heavy for the game), combs up to 1.8 m tall.
- **Renders** of both in `imagesAndSoOn/`.
- **Reference:** GDD schematic plates of 12 hive forms, breed art notes, bee
  photos.
- **Downloaded models:** none. Candidates are listed in `ASSET_SOURCES.md`
  and `plans/3d-models.md`.
- **Map and image references** (never shipped; R1 answered 2026-09-23 —
  commit them): `Part_Of_the_city.jpg`,
  `map for game.png`, `Untitled.png`, `imagesAndSoOn/` — all in the public
  repo (the last three added 2026-09-23 at your request). `CREDITS.md` lists
  each; only your own renders in `imagesAndSoOn/` are free of restrictions.

### 3.4 Tools and setup

- **Unity** `6000.5.4f1` in `~/Unity/Hub`.
- **Blender 5.2** at `~/Desktop/blender-5.2.0-linux-x64/`, with the MCP add-on
  installed (older than the bridge — it still works).
- **Blender MCP** (`uvx blender-mcp` 2.0.0): works; a Claude session that
  failed to connect needs `/mcp` → reconnect `blender`.
- **Hooks:** C# compile check after every script edit; `.meta` guard on shell
  moves and deletes.
- **Agents:** `game-designer`, `planner`, `unity-developer`, `unity-reviewer`,
  `tester`, `sim-balance`, `blender-artist`. **Skills:** `gdd`,
  `unity-script`.
- **Git:** GitHub `YaroslavHolovatyi/BeeGam` (**public**). The working branch
  `docs/farming-cut-and-asset-sources` is pushed, not yet merged into `main`.
  `.claude/` (agents, skills, hooks, shared settings) is versioned; only
  `.claude/settings.local.json` stays local.

---

## 4. What we don't have yet

| Area | Missing |
|---|---|
| **Testing** | Nothing has been played in Unity; the `HiveRitual` scene hasn't been generated. No automated tests (the test framework is installed, zero tests). |
| **Unity files** | No `.meta` files for the 13 new scripts, their 3 new folders, or `Untitled.blend` — Unity writes them on next open; they then need committing. |
| **Numbers** | Day length, season length, prices, honey rate, starting money — all placeholders (N3–N5). |
| **Colony simulation** | Population, health and queen never change. No pests, disease, swarming, splitting, requeening, breeding, winter, seasons, weather, bloom, or foraging by location. |
| **Hive work on foot** | No stings or player health, no tool upgrades, no extractor, workbench or repairs, no carrying frames, no inventory beyond honey, no visible hands. |
| **Hives** | One placeholder hive. No hive-type data class (the hive equivalent of `BeeBreedData`), none of the 25 roster hives, no placement. |
| **Breeds** | 1 of the 4 `PLAN.md` breeds exists as data, with default stats. The 15-breed roster is design only. |
| **Economy** | Nothing to spend money on. No shop, contracts, other products (wax, propolis, pollen), or currency decision. |
| **World** | No terrain, map, city geometry, NPCs, vehicles or travel. Not decided which map is the real one (W1). |
| **Story** | No grandfather, tutorial, mansion or books. |
| **Top-down** | No computer, automation or top-down view (a late unlock by design). |
| **Decor & garden** | No shop, plants or water features. |
| **Art** | No real model in the game; no textures, animations, sky or lighting pass. |
| **Audio** | Nothing. |
| **UI** | Only the in-game HUD. No main menu, pause, settings or save/load screens. |
| **Save / load** | None. |

---

## 5. Known problems in the documents

1. **The GDD still describes the old perspective in places.** Section 6's draft
   says "hybrid top-down + on-foot 3rd-person" and "Unity is not yet
   installed"; `PLAN.md` now says first person, with top-down late.
2. **Appendix G's "one 60-second cycle"** starts from the top-down computer
   view. Under the new decision that only fits the late game.
3. **Appendix E says hive placement is "already substantially built".** It
   isn't — that plan is on hold and nothing from it exists.
4. **Appendix C's heading says "Appendix B. Bee Breeds…"** (a copy-paste
   duplicate).
5. **Question numbers clash.** Appendix F uses #41–#95; `QUESTIONS.md` now
   has its own #41–#50. And the GDD's links to `QUESTIONS.md` #39/#40
   ("prove it's fun", "fictional-town fallback") now point to decor questions
   — those moved to #44/#45. Section 7 uses its own IDs to avoid the mix-up.
6. **Appendix G reads like decisions but isn't recorded in `PLAN.md`.** Its
   proposals appear in section 7 for you to accept or change.
7. **Three different maps** (W1): `PLAN.md`'s Google-Maps polygon, the GDD
   section 3 version with a mountain instead of the airport, and the
   AI-generated map in Appendix C.
8. **`README.md`** is a two-line stub.

---

## 6. Next steps, in order

1. **You — play-test (≈15 min).** Open Unity → **BeeKeeper → Build Hive
   Ritual Scene** → Play. Go through the checklist in `plans/hive-ritual.md`.
   Send Claude any red Console errors, plus what felt good, boring or
   missing. Then commit the `.meta` files Unity created.
2. **You — reconnect Blender.** With Blender open and the add-on's server
   running, type `/mcp` in Claude Code → `blender` → reconnect. Optional:
   update the add-on (`uvx mcp-for-blender install-addon`, then restart
   Blender).
3. **You — answer the ⭐ questions (N1–N8)** below. The rest can wait.
4. **Claude — record your answers** (`gdd` skill → `PLAN.md`,
   `QUESTIONS.md`, this sheet).
5. **Claude — fix what the play-test finds.**
6. **Claude + Blender — 3D step 1: the honey jar** (tests the whole export
   path), then the starter hive (needs N1) and the frame (needs N7). See
   `plans/3d-models.md` section 4.
7. **Claude — balance pass** once N3–N5 are answered (`sim-balance` agent):
   what the numbers mean per hour of play.
8. **You — the fun checkpoint (S5):** play 3–5 in-game days with real
   models. Decide what comes next: deeper hive work, threats and seasons, or
   more hives and placement.
9. **Housekeeping:** merge the branch into `main`, decide whether the repo
   and its reference images stay public (R2–R3), tidy the GDD (R8).

---

## 7. Answer sheet

IDs: **N** = needed now · **G** vision · **T** time & seasons · **C** colony
simulation · **H** hive work on foot · **E** economy & progression · **W**
world & map · **P** player & story · **D** decor & forage · **V** presentation
& tech · **S** scope & schedule · **R** repo & housekeeping.
"From" shows where the question came from: `Q#` = `QUESTIONS.md`, `F#` = GDD
Appendix F, `App. G` = the GDD's proposed answers.

### ⭐ N — Needed now (blocks the next work)

**N1. Which hive is the starter hive?** Dadant (12 frames), Langstroth (10
frames), Ukrainian lezhak, or something else?
- Why: sets the first real hive model, the frame size and count, and three
  code constants. The grey box has 10 frames 45 mm apart — no real hive does.
- From: `plans/3d-models.md` · Q13 · `plans/hive-placement.md` stage 1
- Proposed (App. G): a beginner Dadant (Tier 2), the common Ukrainian hive.
- **Answer:**

**N2. Which bee is the starter breed — and does the Carpathian bee join the
roster?**
- Why: the only breed file is Italian, with default stats. `PLAN.md` lists
  Italian, Carniolan, Buckfast, Russian; GDD Appendix B calls the Carpathian
  ("Karpatka") the natural Lviv starter.
- From: `PLAN.md` · App. B · App. G
- Proposed (App. G): start with a Carpathian nucleus; keep the four
  `PLAN.md` breeds as early unlocks.
- **Answer:**

**N3. How long is an in-game day, and how many days is a season?**
- Why: every rate in the sim is per in-game hour. Placeholder: 20 real
  minutes per day; no seasons yet.
- From: F80 · `PLAN.md` "accelerated game-time" (direction only)
- Proposed (App. G): a 20–40 minute sitting ≈ 1–2 in-game days, so about
  20 minutes per day. No season length proposed.
- **Answer:**

**N4. Starting money, honey price, and honey per day?**
- Why: placeholders are 100 gold, 15 per kg, ~2.5 kg per day for the starter
  hive (≈ 37 gold per in-game day).
- From: F42, F43 · Q14, Q15
- Proposed (App. G): ₴1,000 to start, ₴180 per kg, a beginner Dadant colony
  makes 10–20 kg in a good season.
- **Answer:**

**N5. What is the currency?** Gold, hryvnia (₴), or something else?
- From: Q14 · F41
- Proposed (App. G): hryvnia (₴) on screen; the code can keep calling it
  gold.
- **Answer:**

**N6. What happens if you open a hive without smoking it? Is there a sting /
player-health system?**
- Why: today the smoker has no consequence — opening unsmoked only changes
  a message.
- From: Q10 · F83, F84
- Proposed (App. G): stings drain a separate player-health meter; at zero you
  can't work for the rest of that in-game day; never lethal. Aggressive breeds
  need more smoke.
- **Answer:**

**N7. What does a frame inspection show, and how deep does it go?**
- Why: now a frame shows its honey plus a text readout. The GDD wants
  inspection to be the player's core skill; it also sets how detailed the
  frame model must be.
- From: Q11, Q12 · F69–F71 · `plans/3d-models.md`
- Proposed (App. G): pull individual frames; spot named problems on the comb
  (varroa, hive beetle, foulbrood, wax moth, overcrowding); 15–30 real seconds
  per inspection; later smart hives replace it with a sensor readout.
- **Answer:**

**N8. Harvest on the spot (as now: F at the open hive), or carry frames to
an extractor in the shed?**
- From: F44, F72
- Proposed (App. G): carry frames to the extractor; flow hives and
  automation skip the carrying later.
- **Answer:**

### G — Vision

**G1. What is the core feeling?** Your GDD section 1 says calm and
meditative; App. G proposes *mastery* (reading a hive right before it fails),
with calm as the surface.
- **Answer:**

**G2. What is the one hook?** The real bee subspecies, the real place, or the
on-foot + top-down mix?
- Proposed (App. G): the real subspecies behaviour — cheapest to make
  excellent, and what a reviewer would repeat.
- **Answer:**

**G3. The one-line pitch.** Adopt App. G's (*"a cozy management-and-inspection
sim about reviving your grandfather's apiary in a hand-recreated slice of
Sknyliv, Lviv…"*) or write your own?
- **Answer:**

**G4. Audience and release.** `PLAN.md` says private hobby project for now;
your GDD mentions PC and Steam Deck; App. G names itch.io / Steam next to
APICO and Farming Simulator. Build-for-me-decide-later, or aim at a release?
- **Answer:**

### T — Time, seasons, weather

**T1. Does the calendar follow real Lviv seasons and bloom times, or whatever
plays best?**
- From: Q24, Q39
- **Answer:**

**T2. Does weather matter day to day, or only per season?**
- From: Q7
- Proposed (App. G): day to day — rain and cold stop foraging that day,
  heatwaves raise water needs; seasons set the bloom curve.
- **Answer:**

**T3. Winter: a gradual, visible drain you can act on, or one check at the
end?**
- From: F82
- Proposed (App. G): gradual; the player feeds and insulates.
- **Answer:**

**T4. Does time keep running while the player is away from the apiary?**
- From: F81
- **Answer:**

**T5. When can the player sleep?** Any time (as now), only at night, or are
you forced to sleep late at night?
- From: the hive-ritual build
- **Answer:**

**T6. How often does the simulation step?** Now every 0.25 in-game hours.
Technical — recommended: keep it.
- From: F48
- **Answer:**

### C — Colony simulation

**C1. How many breeds in total, and are new ones bought, found, or bred?**
- From: Q1 · F58
- Proposed: App. B's roster of 15; App. G says a normal game reaches 8–10,
  and all 15 is optional.
- **Answer:**

**C2. Individual bees or group numbers?**
- From: Q2 · F52
- Proposed (App. G): one population number split into nurse / forager /
  guard shares; visible bees are decoration only.
- **Answer:**

**C3. Health: one number or several?**
- From: F49
- Proposed (App. G): food stores, mite/disease load and queen status
  underneath; one health value shown, the details found by inspecting.
- **Answer:**

**C4. What does queen status track, and how does requeening work?**
- From: Q4 · F50, F53
- Proposed (App. G): present/absent plus a quality 1–5 that falls with age;
  buy a queen (fast, costs money) or rear one (free, slow).
- **Answer:**

**C5. At zero health, is the colony lost, or is there a rescuable critical
state?**
- From: F51
- Proposed (App. G): a critical window of a few days; only an ignored colony
  dies.
- **Answer:**

**C6. Swarming and splitting: what triggers a swarm, is a lost swarm gone or
findable, and what does a split cost?**
- From: Q5 · F55–F57
- Proposed (App. G): crowding threshold scaled by the breed's swarm stat; the
  swarm can be found and caught in the woods; a split costs an empty hive box
  and a little time.
- **Answer:**

**C7. Pests and disease: simple sliders or named threats?**
- From: Q6
- Proposed (App. G): four named threats — varroa, hive beetle, foulbrood, wax
  moth — each with its own sign on the comb and its own treatment.
- **Answer:**

**C8. Foraging: do bees visibly fly to flowers, or is it a range + flower
density formula?** ⚑ blocks the flower-bed feature.
- From: Q8 · F79
- Proposed (App. G): a formula using per-area flower density; flying bees are
  decoration only.
- **Answer:**

**C9. Which products besides honey, and do they have their own chains?** Wax,
propolis, pollen, royal jelly; candles, mead?
- From: Q3
- Proposed (App. G): honey, wax, propolis and pollen, each with its own price;
  wax → candles, honey → mead.
- **Answer:**

**C10. Cross-breeding: is the result an average of the parents, a weighted
roll, or a hand-made hybrid?**
- From: F54
- **Answer:**

### H — Hive work on foot

**H1. Which tools, and do they need buying, upgrading or upkeep?**
- From: Q9
- Proposed (App. G): smoker (uses fuel), hive tool, bee brush, suit (wears
  out); better tiers make work faster and safer.
- **Answer:**

**H2. Do better hives and tools change yield and disease, or mostly
comfort?**
- From: Q13
- Proposed (App. G): tools and suits measurably matter; hives differ by the
  Appendix A stats.
- **Answer:**

**H3. Does the workbench build hives from materials, or are hives only
bought?**
- From: F59
- **Answer:**

**H4. Do hives wear out and need repair at the workbench?**
- From: F60
- **Answer:**

**H5. Are frames a supply you build or buy, or unlimited?**
- From: F61
- **Answer:**

**H6. How does placing a hive or decor work in first person?** A see-through
preview where you look, carrying it and setting it down, or a build menu?
- From: Q49
- **Answer:**

**H7. Before the computer, how does the player know a hive needs attention?**
Walking the rounds, sounds and signs at the entrance, a notebook, or only by
opening it?
- From: Q48 · App. H ("audible queenless hive")
- **Answer:**

**H8. Hands in first person?** Gloved hands and sleeves holding the tools, or
tools floating as now?
- From: `plans/3d-models.md` · Q27
- **Answer:**

### E — Economy & progression

**E1. Do prices change?** Fixed, market drift, or quality-driven — and does
each product have its own price?
- From: Q15 · F46
- Proposed (App. G): base price × quality, mild seasonal drift, price spikes
  on festival days; separate prices per product.
- **Answer:**

**E2. Contracts, or instant selling only?**
- From: Q16
- Proposed (App. G): both — instant sell always; contracts pay a premium.
- **Answer:**

**E3. Where does selling happen?** A roadside stand (the slice's
placeholder), a market day, a shop — or all of them?
- From: the hive-ritual build · App. H (farmers' market day, Honey Spas
  festival)
- **Answer:**

**E4. What does the player spend money on?**
- From: F45
- Proposed (App. G): hives by tier, land for expansion, tools, smoker fuel,
  treatments, books, repairs, and the mansion rebuild.
- **Answer:**

**E5. Is there an upgrade tree, and how deep?**
- From: Q17
- **Answer:**

**E6. Can the player hire help?**
- From: Q18
- Proposed (App. H): one helper later, for a wage, who does one chosen chore.
- **Answer:**

**E7. Does expanding to new land cost money, need a milestone, or both?**
- From: Q19
- Proposed (App. G): both — the milestone opens it, a fee buys it.
- **Answer:**

**E8. What are the actual milestones?**
- From: Q20
- Proposed (App. G): M1 *Rooted* — 3 hives, all survive the first winter,
  20 kg sold. M2 *Established* — 8 hives, 3 breeds, 100 kg total. M3
  *Apiarist* — 15 hives in two areas, first requeen and first caught swarm,
  400 kg. M4 *District Keeper* — the whole area, 8+ breeds. M5 *The
  Homestead* — rebuild the solarpunk mansion.
- **Answer:**

**E9. Starting money: tied to the inheritance story, or just a number? Any
debt?**
- From: F47
- **Answer:**

**E10. What does automation do after the computer is bought, and what still
needs you on foot?**
- From: Q46 · F73–F75
- **Answer:**

**E11. What unlocks the computer?** A hive count, a milestone, or just its
price?
- From: Q47
- **Answer:**

**E12. Do better hive tiers need only money and milestones, or also a book or
skill?**
- From: F62
- **Answer:**

### W — World, map, city

**W1. Which map is the game map?** Three versions exist: `PLAN.md`'s
Google-Maps area (airport outside it); your GDD section 3 (the airport
replaced by a mountain, a village beyond it, forests west, fields south); and
the AI-generated map in GDD Appendix C (city zone + a forest/lake/waterfall
zone for the apiary).
- From: documents disagree
- **Answer:**

**W2. Where does the player start?** Your GDD section 3 puts the start at the
grandfather's mansion (beyond the mountain); App. G makes that forest zone an
expansion at milestones M2–M3.
- From: F76
- **Answer:**

**W3. Confirm the city approach:** hand-build from the reference, no
OpenStreetMap or Google 3D tiles? (`PLAN.md` still says "proposed".)
- **Answer:**

**W4. The city seen at eye level:** shrink the detailed area, accept low
detail, or move toward a simpler fictional town?
- From: Q50, Q45
- **Answer:**

**W5. NPCs: none, background people, or a few named characters?**
- From: Q21
- Proposed (App. G): a few pedestrians and cars, plus named people — market
  vendor, supply shopkeeper, grandfather.
- **Answer:**

**W6. Rival beekeepers?**
- From: Q22
- Proposed (App. G): one rival who bids on contracts and reacts to your
  reputation, without competing for flowers.
- **Answer:**

**W7. Rules about placing hives near the school, kindergarten and
playground?**
- From: Q23
- Proposed (App. G): yes — aggressive breeds and heritage hives that can't be
  inspected are banned near them; calm breeds and closed observation hives
  are allowed; shown as a placement warning, no fines.
- **Answer:**

**W8. Getting around: walk, bike, car, fast travel — do they differ?**
- From: Q25 · F88
- Proposed (App. G): walk early, bike and car later, fast travel to known
  sites; the car carries more.
- **Answer:**

**W9. Where does the map end, and is the mountain ridge walkable?**
- From: Q26 · F78
- Proposed (App. G): the marked area is the boundary; the ridge is scenery.
- **Answer:**

**W10. Do the hill (Скнилівська гора) and the waterfall do anything in
gameplay?**
- From: F77
- **Answer:**

**W11. Online ordering (post office): does it replace shop trips, and does it
cost extra or take time?**
- From: F89
- Proposed (App. G): a no-travel way to restock, for a shipping fee.
- **Answer:**

### P — Player, story, onboarding

**P1. Character customization?**
- From: Q27
- Proposed (App. G): name plus a few appearance and suit options.
- **Answer:**

**P2. Does the player get better with practice (skills)?**
- From: Q28
- Proposed (App. G): a few skills that grow by doing — faster inspections,
  calmer handling, better diagnosis.
- **Answer:**

**P3. Confirm the story frame from your GDD:** the grandfather teaches you,
and the long goal is rebuilding his mansion?
- From: Q29 · GDD sections 3–4
- **Answer:**

**P4. The grandfather after the tutorial — recurring, one-off, or letters?
Alive?**
- From: F63, F64
- Proposed (App. G): in person for the tutorial, then letters with advice;
  his fate left gently open.
- **Answer:**

**P5. Tutorial: one scripted run, or also a "how do I…" reference later?**
- From: F65
- Proposed (App. G): a guided first hive; replayable from the help / book
  menu.
- **Answer:**

**P6. Books: instant unlock or read over time? What does one unlock? A skill
tree on top?**
- From: F66–F68
- **Answer:**

**P7. Is the real beekeeping knowledge required to progress, or optional?**
- From: F92
- Proposed (App. H): optional almanac pages that also help with diagnosis.
- **Answer:**

**P8. Losing and saving:** what happens at rock bottom (money and colonies
gone), and one save slot or several?
- From: F84, F85, F93 · Q36
- Proposed (App. G): never a game over — you drop to one hive and a small
  stake; one continuous save with autosave.
- **Answer:**

**P9. The mansion ending:** its own building materials or the same money?
What makes it "solarpunk" — installable fixtures, a final look, or a score?
- From: F90, F91
- **Answer:**

### D — Decor, garden, forage

**D1. Confirm the 8 bee plants** (willow/crocus, fruit blossom, acacia,
linden, phacelia, clover, sunflower, goldenrod) — and real Lviv bloom times?
- From: Q39
- **Answer:**

**D2. How far does a flower bed reach compared with a bee's foraging range,
and do overlapping beds add up, cap, or weaken?**
- From: Q40
- **Answer:**

**D3. Do cosmetic items give anything besides looks?**
- From: Q41
- **Answer:**

**D4. Is the decor shop open from the start? Can decor be moved or sold
back?**
- From: Q42
- **Answer:**

**D5. Is water something colonies need, or only a bonus?**
- From: Q43
- **Answer:**

### V — Presentation & tech

**V1. Games whose look you want to get close to?** (Your GDD names Farming
Simulator, Ranch Simulator, Planet Crafter, House Flipper.)
- From: Q30
- **Answer:**

**V2. Music and sound direction?**
- From: Q31
- Proposed (App. G): calm countryside ambience; the hive's hum doubles as a
  diagnostic cue.
- **Answer:**

**V3. HUD: mostly in-world, or a classic sim HUD?**
- From: Q32
- Proposed (App. G): on foot mostly in-world; the computer *is* the
  management HUD.
- **Answer:**

**V4. Accessibility from the start?**
- From: Q33
- Proposed (App. G): colour-blind-safe markers (shape + colour), remappable
  controls, text size.
- **Answer:**

**V5. Controls and platform:** keyboard + mouse only, or gamepad / Steam
Deck too? (Your GDD section 1 says "computers and Steam Deck".)
- From: Q34
- Proposed (App. G): keyboard + mouse first, designed so a gamepad can be
  added later.
- **Answer:**

**V6. Minimum hardware to run on?**
- From: Q35
- **Answer:**

**V7. How close to real is the frame close-up** — individual cells and eggs?
- From: `plans/3d-models.md`
- **Answer:**

**V8. Honey on a real comb model: stretched meshes, or a small shader that
fills the texture?** Technical — recommended: the shader.
- From: `plans/3d-models.md`
- **Answer:**

**V9. Record the 3D sourcing rule in `PLAN.md`?** Hand-model the signature
assets (hives, bees, landmarks), download filler. `CLAUDE.md` and
`ASSET_SOURCES.md` already work this way.
- From: Q38
- **Answer:**

### S — Scope & schedule

**S1. The cut-line: which of these are in the first real version, and which
are "someday"?** Driving, post office / online ordering, shops, mansion
rebuild, books, automation, wild-swarm catching, requeening, splitting,
cross-breeding, 25 hive types, 15 breeds.
- From: F94 (the most important open question in the GDD)
- **Answer:**

**S2. After farming, what goes next if something must — the mansion ending,
or the shops / driving loop?**
- From: F95
- **Answer:**

**S3. How many hours per week, and is there a target date?**
- From: Q37
- **Answer:**

**S4. What would make you switch to a simpler fictional town?**
- From: Q45
- **Answer:**

**S5. After playing: is the hive ritual fun?** What felt good, what was
boring, what's missing? (Answer after the play-test.)
- From: Q44
- **Answer:**

**S6. What is the smallest build you'd let someone else play?**
- From: GDD section 7
- **Answer:**

### R — Repo & housekeeping

**R2. `Part_Of_the_city.jpg` (Google Maps imagery) is already public on
`main`.** Leave it, or remove it? (Removing it from history needs a force
push.)
- **Answer:**

**R3. Should the GitHub repository be public?** It is now; `PLAN.md` says
private hobby project.
- **Answer:**

**R4. Merge `docs/farming-cut-and-asset-sources` into `main`?**
Recommended: yes, after the play-test.
- **Answer:**

**R5. Keep Blender source files outside `Assets/`** (for example
`ArtSource/`), exporting only FBX into `Assets/Art/`? Recommended: yes.
- **Answer:**

**R6. The wild hive's combs are up to 1.8 m tall. Intended?**
- **Answer:**

**R7. Update the Blender MCP add-on?** Recommended: yes.
- **Answer:**

**R8. Tidy the GDD worksheet?** Rename Appendix F's questions F1–F55 (ending
the clash with `QUESTIONS.md`), fix Appendix C's heading, mark Appendix G as
proposals, and update section 6's stale lines. Recommended: yes.
- **Answer:**

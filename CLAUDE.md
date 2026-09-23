# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A Unity 6 (`6000.5.4f1`, URP) beekeeping simulator, solo hobby project. The
game recreates a genericized ~2 km slice of Lviv's Sknyliv district where the
player runs an apiary. Played mostly on foot in first person (hands-on hive
work around the apiary); a top-down management view unlocks later, when the
player buys an in-game computer. The first playable target is a one-hive,
on-foot "hive ritual" slice (smoke, open, inspect a frame, harvest, sell);
city geometry, hive placement and the top-down view come after it.

The project is early: the first playable (the on-foot "hive ritual") is coded
but not yet play-tested, and most design intent still lives in the docs.

## Source of truth for design

- **`PLAN.md`** — decisions already made, each a bolded claim with its reasoning
  and the tradeoff accepted. Read this before proposing gameplay/architecture.
- **`QUESTIONS.md`** — a numbered *menu* of open design questions, grouped by
  area. "Unanswered" is a valid state; don't treat blanks as tasks.
- **`plans/project-status.md`** — the hub: what exists, what doesn't, known
  doc problems, next steps, and an **answer sheet** (section 7) gathering every
  open question with a slot for the user's answer. When the user asks to
  record their answers, use the `gdd` skill for each answered item, then
  remove it from the answer sheet too. Keep sections 3–6 current when work
  lands.
- When the user settles a design question, use the **`gdd` skill** — it records
  the decision in `PLAN.md` and retires the question from `QUESTIONS.md` so the
  two stay consistent. Don't hand-edit these docs for design decisions; don't
  invent decisions the user only mused about.
- **`BeeKeeper_GDD_Worksheet.docx`** — the user's large design worksheet; they
  edit it in Word. Read its generated text copy,
  **`BeeKeeper_GDD_Worksheet.md`** (~33k tokens, so read it on demand, not
  every session). Never edit the `.md`; after the `.docx` changes, regenerate
  it with `python3 .claude/scripts/gdd-to-markdown.py`. Its Appendices D–H are
  *proposals*, not decisions — only what's in `PLAN.md` is decided.

## Project documents loaded every session

@plans/project-status.md
@PLAN.md
@QUESTIONS.md
@plans/hive-ritual.md
@plans/3d-models.md

Not loaded automatically: `plans/hive-placement.md` (on hold — written for the
old top-down-first design; don't execute it), `BeeKeeper_GDD_Worksheet.md`,
`ASSET_SOURCES.md`, `CREDITS.md` (described below).

## Code architecture

No assembly definitions — everything compiles into `Assembly-CSharp` (or
`Assembly-CSharp-Editor` for anything under an `Editor/` folder). No namespaces.

- `Assets/Scripts/Data/` — `ScriptableObject` definitions (designer-authored
  static data). `BeeBreedData` holds per-breed yield/temperament/disease
  resistance/cold tolerance/foraging range. Breeds are real subspecies (Italian,
  Carniolan, Buckfast, Russian) and are meant to be *data, not code* — new
  breeds should be new `.asset` files, not new classes.
- `Assets/Scripts/Simulation/` — `MonoBehaviour` runtime logic. `GameClock`
  owns in-game time; `HiveTicker` steps every registered `HiveController` on a
  fixed in-game interval; `HiveController` accumulates honey from breed ×
  population × health in daylight; `EconomyManager` is a singleton
  (`Instance`) holding gold and honey price with `SellHoney`/`TrySpend`.
- `Assets/Scripts/OnFoot/` — the first-person player and everything they use:
  `FirstPersonController`, `PlayerInteractor` + the `Interactable` base
  (E = Interact, F = Secondary), `HiveInteraction`, `HiveFrame`, `Smoker`,
  `SleepSpot`, `HoneyStand`, `PlayerInventory`.
- `Assets/Scripts/UI/` — `PlayerHud`.
- `Assets/Scripts/Editor/` — `HiveRitualSceneBuilder` (menu **BeeKeeper →
  Build Hive Ritual Scene** generates `Assets/Scenes/HiveRitual.unity` from
  placeholder primitives; regenerating overwrites hand edits to that scene).

Add a new sibling folder (e.g. `Assets/Scripts/City/`) for a genuinely
different concern rather than overloading an existing one.

**Before writing or changing anything under `Assets/Scripts`, use the
`unity-script` skill** — it carries the house style and the rules that prevent
real breakage (serialization, `.meta`/GUID safety, per-frame cost). Key ones:

- Never casually rename/retype a public serialized field — field names are the
  serialization keys in `.asset`/`.unity` files; a rename silently zeroes every
  hive in every scene. Use `[FormerlySerializedAs]` if a rename is truly needed.
- Nothing that scales with hive count belongs in `Update()`. Hive simulation
  runs through `HiveTicker` on in-game time; per-hive visuals use coroutines
  that only run while animating. Keep it that way as the sim grows.
- Scale rates by *in-game* time (accelerated time is a design decision), not raw
  wall-clock `Time.deltaTime`.

## Building and verifying

Compilation runs automatically via a `PostToolUse` hook after every Write/Edit.
To compile manually:

```bash
~/Unity/Hub/Editor/6000.5.4f1/Editor/Data/DotNetSdk/dotnet build Assembly-CSharp.csproj -nologo -v q
```

This is **compile-only**. It cannot verify behavior — scene wiring, inspector
values, and actual play require the Unity editor. State plainly which you did;
a change that compiles is not a change that's verified.

Tests use `com.unity.test-framework` (Unity Test Runner), run from the editor.

## Asset / file safety (enforced by a hook)

A `PostToolUse` hook scans Bash commands that move or delete files and rejects
orphaned `.meta` files under `Assets/`. Every imported asset has a sibling
`.meta` holding its GUID, which scenes/prefabs reference. Never hand-edit a
`.meta`, and never delete or move an asset without its `.meta` — let the Unity
editor do moves/deletes when possible. If you must delete via shell, delete the
asset and its `.meta` together.

## Sourcing 3D assets

- **`ASSET_SOURCES.md`** — where to get free models, organized by what the game
  needs (hives/bees, vegetation, furniture, Soviet-era panel housing, street
  props, characters). Read it before hunting for an asset; it also records the
  sourcing order and which sources the Blender MCP can import from directly.
- **`CREDITS.md`** — the provenance ledger. **Every third-party asset gets a
  line the moment it's downloaded, before it's imported** — including CC0 ones,
  where attribution isn't required but provenance still is. An asset whose
  licence can't be established later has to be deleted and remade.
- Prefer CC0. Never `CC-BY-NC` or "personal use only" — `PLAN.md` keeps a public
  release possible, and those licences make it a re-art job.
- Reference material is not shippable material. The map screenshots
  (`Part_Of_the_city.jpg`, `map for game.png`) are Google Maps imagery: fine to
  model from, never to ship or trace into a texture. `CREDITS.md` tracks that
  boundary.
- Import downloaded assets through the Unity editor so it generates the `.meta`
  — see the `.meta` rules above.

## Specialized agents

- **`unity-reviewer`** — review C# changes for Unity-specific hazards (broken
  serialization, per-frame cost, lifecycle bugs, GUID/asset breakage). Use after
  editing anything under `Assets/Scripts`.
- **`blender-artist`** — models, textures and sources 3D assets via the Blender
  MCP, and exports them into `Assets/Art/`. Point it at `ASSET_SOURCES.md`
  before it goes looking, and require it to log anything it downloads in
  `CREDITS.md`. Hand-model the assets that carry the game's identity (the hive
  roster, the bees, landmark buildings); download the filler.
- **`sim-balance`** — works out what tuning constants mean in play (harvest time,
  earnings per hour, curve shape). Use when adding/changing rates, prices, costs,
  or progression numbers.

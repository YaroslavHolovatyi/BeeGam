# Build Plan — Hive Placement (sim-view)

> **On hold (2026-09-23).** This plan assumes a top-down-first game.
> `PLAN.md` now makes the game first-person on foot, with the top-down view
> unlocked late via an in-game computer, and the first playable is an
> on-foot one-hive slice with no placement. Don't run these stages as
> written — placement will likely happen on foot (`QUESTIONS.md` #49), which
> changes Stage 3's orthographic camera and ghost-preview design.

## Goal & done-when

**Goal:** In the top-down city/sim view, the player picks a bee breed and places
a new hive at a valid ground location, which instantiates a live `HiveController`
that begins accumulating honey.

**Done-when (checkable):** In the `CityOverview` scene, entering place-mode and
clicking a valid spot on the ground spawns a hive prefab carrying a
`HiveController` with the chosen breed assigned and `honeyStoredKg` rising over
time; clicking an invalid spot (overlapping another hive / off the ground) is
rejected with visible feedback and spawns nothing; if placement is set to cost
gold, `EconomyManager.goldBalance` drops by the cost and placement is blocked
when funds are insufficient.

## Scope note (read once)

This feature is the point where the game goes from one hive to many. Two
consequences flagged, not solved here:

1. `HiveController.AccumulateHoney` runs in `Update()` per hive. CLAUDE.md warns
   nothing that scales with hive count belongs in `Update()`. This plan does **not**
   fix that refactor (out of scope), but the placement stage must not make it worse
   and should register spawned hives with a central list so a future ticker can
   drive them. A follow-up "central hive ticker" plan is recommended once several
   hives can exist.
2. City geometry does not exist yet. The "world" for this slice is a flat ground
   plane in `CityOverview`, not real Lviv streets. Zoning-aware placement (near
   school/playground) is deferred — see Stage 1.

No scope enlargement beyond the smallest slice that reaches done-when.

## Open design questions this feature depends on

- **"Hive type" is ambiguous.** PLAN.md locks bee breeds as data-driven real
  subspecies, but QUESTIONS.md Q13 (equipment/hive-box tiers) is unresolved. The
  request says "select a hive type." This slice must know whether the thing being
  selected is the **bee breed** or a separate **hive-box tier**. Routed to Stage 1.
- **QUESTIONS.md Q23 — placement legality / city regulations** (can't place near
  school/playground) is open and directly defines "valid location." Routed to
  Stage 1 for an MVP-level answer.
- **Placement cost** (touches Q14/Q19 economy) — does dropping a hive spend gold?
  `EconomyManager.TrySpend` exists and is trivial to call, so this needs a yes/no
  + number. Routed to Stage 1.

---

## Stage 1 — Lock the design inputs (game-designer)

**Dependency:** none. Must complete before any placement code (Stages 3–4).

**Prompt to paste:**

> You are settling three narrow design questions for the hive-placement feature of
> a Unity 6 beekeeping sim (solo hobby project). Read `PLAN.md` and `QUESTIONS.md`
> first. Use the **`gdd` skill** to record each decision in `PLAN.md` and retire/annotate
> the matching `QUESTIONS.md` entry — do not hand-edit those docs any other way.
> Keep every answer to the smallest choice that lets a first playable placement
> slice ship; note richer versions as "later."
>
> Decide and record:
> 1. **What "hive type" means for this milestone.** Recommended default: the player
>    selects a **bee breed** (`BeeBreedData` — Italian/Carniolan/Buckfast/Russian,
>    already locked as data). Hive-box/equipment tiers (QUESTIONS.md Q13) stay
>    deferred and are **not** part of placement now. Confirm or override.
> 2. **What makes a placement location valid for the MVP** (QUESTIONS.md Q23).
>    Recommended default: valid = on the ground plane AND at least a fixed minimum
>    distance from any existing hive; NO zoning rules yet (school/playground
>    proximity, permits) — record those as deferred to a later "placement legality"
>    milestone. Give the minimum-distance number in meters. Confirm or override.
> 3. **Whether placing a hive costs gold**, and if so the flat cost. Recommended
>    default: yes, a single flat cost paid via `EconomyManager.TrySpend`; pick a
>    starting number consistent with `goldBalance = 100` and `honeyPricePerKg = 15`.
>    If you set a cost, consult the **`sim-balance`** agent for a sane number.
>
> **Acceptance check:** `PLAN.md` contains three new decisions (hive-type scope,
> MVP validity rule + distance number, placement cost + number); the related
> `QUESTIONS.md` items are retired or annotated as deferred. No gameplay code changed.

---

## Stage 2 — Placeholder hive box mesh (blender-artist)

**Dependency:** none. Can run in parallel with Stage 1. Must complete before Stage 3
(the prefab consumes this mesh).

**Prompt to paste:**

> Model a single low-poly **Langstroth-style beehive** (a stack of 2–3 rectangular
> boxes on a base board, with a slightly overhanging lid) for a Unity 6 URP
> beekeeping sim in a semi-realistic Cities:Skylines / Farming-Sim register — read
> `PLAN.md` for the art-style decision. This is the placeable hive seen from a
> top-down sim camera, so silhouette and readability from above matter more than
> fine detail. No internal frames, no bees.
>
> Requirements:
> - Real-world scale: roughly 0.45 m wide × 0.45 m deep × 0.6 m tall. Model in meters
>   so it imports at Unity scale 1.0.
> - Origin at the base center (bottom of the base board sits on Y=0) so it drops
>   cleanly onto a ground plane.
> - +Z forward, +Y up, Unity-friendly export.
> - Low poly (target well under ~2k tris); one simple material/color set is enough.
> - Export to `Assets/Art/Hives/HiveBox_Placeholder.fbx` (create the folder). Let the
>   Unity editor generate the `.meta`; do not hand-write it.
>
> **Acceptance check:** `Assets/Art/Hives/HiveBox_Placeholder.fbx` exists, imports at
> scale 1.0, sits on the ground with pivot at its base, and reads clearly as a hive
> from a top-down angle.

---

## Stage 3 — Placement system + prefab + overview scene (unity-developer)

**Dependency:** Stage 1 (design decisions in `PLAN.md`) and Stage 2 (hive mesh).

**Prompt to paste:**

> Add hive placement to a Unity 6 (`6000.5.4f1`, URP) beekeeping sim. **Before
> writing anything under `Assets/Scripts`, use the `unity-script` skill** for house
> style and serialization/`.meta`/GUID safety. Read `PLAN.md` (esp. the newly
> recorded hive-type scope, placement-validity rule + distance number, and
> placement-cost decision), `CLAUDE.md`, and the existing scripts:
> `Assets/Scripts/Simulation/HiveController.cs`, `EconomyManager.cs`, and
> `Assets/Scripts/Data/BeeBreedData.cs`. Follow the resolved design decisions
> exactly; the notes below assume "hive type = selected `BeeBreedData`" and adjust
> if `PLAN.md` says otherwise.
>
> Do all of the following:
> 1. **Scene:** create `Assets/Scenes/CityOverview.unity` with a flat ground plane
>    (large enough to place several hives) on its own layer named `Ground`, and an
>    **orthographic camera looking straight down** (top-down sim view). No camera
>    controller is required for this slice.
> 2. **Hive prefab:** create `Assets/Art/Hives/Hive.prefab` from
>    `Assets/Art/Hives/HiveBox_Placeholder.fbx` with a `HiveController` component and
>    a collider. Leave `breed` unassigned on the prefab (set at spawn time).
> 3. **Placement controller** in a new `Assets/Scripts/City/` folder (new concern —
>    do not overload Simulation/): a `HivePlacementController` MonoBehaviour that:
>    - holds a serialized reference to the hive prefab, the selectable
>      `BeeBreedData` assets, the `Ground` layer mask, the min-distance number and
>      flat placement cost from `PLAN.md`, and a current selected breed;
>    - has an enter/exit **place-mode**;
>    - while in place-mode, raycasts the mouse through the ortho camera to the
>      ground and shows a translucent **ghost preview** of the hive at the hit point,
>      tinted valid/invalid;
>    - **validity** = ray hit the `Ground` layer AND no existing hive within the
>      min distance (track placed hives in a list on the controller — this list is
>      also the hook a future central ticker will use, per CLAUDE.md's warning about
>      per-hive `Update` cost; do NOT add new per-frame per-hive work here);
>    - on click at a valid spot: if placement costs gold, call
>      `EconomyManager.Instance.TrySpend(cost)` and abort (with feedback) if it
>      returns false; otherwise `Instantiate` the hive prefab at the hit point,
>      assign the selected `BeeBreedData` to the spawned `HiveController.breed`, and
>      add it to the placed-hives list;
>    - on click at an invalid spot: reject and trigger a brief invalid feedback
>      signal (an event/bool the UI stage can read) — spawn nothing.
> 4. Expose a small public API the UI stage (Stage 4) will call: `SetSelectedBreed`,
>    `EnterPlaceMode`/`ExitPlaceMode`, and a readable "last placement rejected"
>    signal. Do not build UI here.
> 5. Add an `EconomyManager` instance to `CityOverview` and wire the controller's
>    serialized fields (prefab, breed assets incl. `ItalianBeeBreed.asset`, ground
>    mask, numbers) in the scene.
>
> Compile with the project's dotnet build (see CLAUDE.md) and state that it is
> compile-verified only, not play-verified. Then use the **`unity-reviewer`** agent
> on the changed C#.
>
> **Acceptance check:** project compiles; `CityOverview.unity`, `Hive.prefab`, and
> `Assets/Scripts/City/HivePlacementController.cs` exist and are wired; no new
> serialized-field renames on existing components; no added per-hive `Update` cost.

---

## Stage 4 — Minimal placement UI (unity-developer)

**Dependency:** Stage 3 (`HivePlacementController` public API + `CityOverview` scene).

**Prompt to paste:**

> Add a minimal in-scene UI for hive placement to a Unity 6 (`6000.5.4f1`, URP)
> beekeeping sim. **Use the `unity-script` skill first.** Read `CLAUDE.md` and
> `Assets/Scripts/City/HivePlacementController.cs` (built in the prior stage) and
> the `EconomyManager` script; wire UI to that controller's public API — do not
> re-implement placement logic. Put UI scripts in a new `Assets/Scripts/UI/` folder.
>
> In the `Assets/Scenes/CityOverview.unity` scene, add a UGUI Canvas with:
> - a **breed selector** (one button per available `BeeBreedData`, labeled by
>   `breedName`) that calls `SetSelectedBreed` and highlights the current choice;
> - a **place-mode toggle** button calling `EnterPlaceMode`/`ExitPlaceMode` with a
>   clear on/off visual state;
> - a **gold readout** showing `EconomyManager.Instance.goldBalance` (and, if
>   placement costs gold, the placement cost), refreshed after each placement;
> - a brief **"can't place there / not enough gold" message** driven by the
>   controller's rejection signal, that appears then fades.
>
> Keep it functional, not styled — a traditional sim HUD stub is fine (QUESTIONS.md
> Q32 on HUD philosophy is unresolved; do not invest in polish). Compile via the
> project dotnet build and state compile-only vs play-verified. Run **`unity-reviewer`**
> on the changed C#.
>
> **Acceptance check:** project compiles; the `CityOverview` Canvas exposes breed
> buttons, a place-mode toggle, a gold readout, and a rejection message, all wired to
> `HivePlacementController` / `EconomyManager` with no duplicated placement logic.

---

## Stage 5 — Verify the placement loop (tester)

**Dependency:** Stage 4 (feature complete and wired).

**Prompt to paste:**

> Verify the hive-placement feature in a Unity 6 (`6000.5.4f1`) beekeeping sim.
> Read `plans/hive-placement.md` (this plan) for the intended behavior, then read
> `Assets/Scripts/City/HivePlacementController.cs`, `Assets/Scripts/UI/` scripts,
> and `Assets/Scripts/Simulation/HiveController.cs`.
>
> Behavior verification is play-mode work (compile-only cannot confirm it) — write a
> **play-mode test checklist** and add any feasible automated EditMode/PlayMode tests
> under the Unity Test Runner (`com.unity.test-framework`) for the pure logic
> (e.g. validity check rejects a point within min-distance of an existing hive;
> `TrySpend` is invoked and blocks placement when `goldBalance` is below cost;
> a spawned hive has its `breed` assigned).
>
> The manual checklist must cover, in `CityOverview`:
> 1. Select a breed, enter place-mode, click valid ground → one hive spawns with the
>    correct breed and `honeyStoredKg` increases over time.
> 2. Click an invalid spot (off-ground, or within min distance of an existing hive)
>    → nothing spawns and the rejection message shows.
> 3. If placement costs gold: `goldBalance` drops by the cost on a successful place;
>    with insufficient gold, placement is blocked with feedback.
> 4. Placing several hives adds no visible per-frame slowdown (sanity check against
>    the per-hive `Update` warning in CLAUDE.md).
>
> **Acceptance check:** a committed test checklist + any automated tests exist; every
> checklist item has a pass/fail result; failures are reported back with repro steps
> to route to `unity-developer`.

---

## Sequencing rationale

- **Stage 1 (designer) is first** because the feature depends on two open
  QUESTIONS.md items (Q13 hive-type meaning, Q23 placement legality) plus an
  undecided placement cost. Code cannot be written cold without those answers.
- **Stage 2 (mesh) gates Stage 3** — the prefab and placement system consume the
  hive mesh, so the mesh must exist first. Stage 2 has no dependency on Stage 1 and
  can run in parallel.
- **Stage 3 before Stage 4** — the UI wires to the placement controller's public
  API, so the controller must exist first. Split from Stage 4 because UI is a
  different concern (`Assets/Scripts/UI/` vs `Assets/Scripts/City/`) and keeps each
  agent prompt runnable cold.
- **Stage 5 (tester) is last** and follows the behavior-adding unity-developer
  stages, per the rule that a tester stage trails any stage that adds behavior;
  it exists because the project's dotnet build is compile-only and cannot confirm
  placement actually works in play.

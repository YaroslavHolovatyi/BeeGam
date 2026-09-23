# Build Plan — Hive Ritual (first playable)

## Goal & done-when

**Goal:** the first playable from `PLAN.md` — one hive in a small yard,
played on foot in first person: walk up, smoke it, open it, inspect a frame,
harvest, sell, sleep.

**Done-when (whole slice):** in `Assets/Scenes/HiveRitual.unity` the player
can play several in-game days in a row. Each day they walk to the hive,
smoke and open it, inspect a frame and see the colony's state, harvest the
honey above the reserve, sell it for gold, and sleep to the next morning,
with honey building up on in-game time between visits.

## How we work through it

One step at a time. For each step Claude writes the code (plus a menu item
that builds or updates the scene from placeholder primitives); you open
Unity, run the menu item, press Play, and check the step's done-when before
the next step starts. The command-line build only proves the code compiles;
the Play-mode check is the real test.

Placeholder art (grey boxes) throughout. The real hive model comes from
Blender once the slice proves fun, not before.

---

## Status

| Step | State |
|---|---|
| 1 — Walkable yard + interact | Code written, compiles, reviewed. **Not yet play-tested.** |
| 2 — Clock, ticker, sleep | Code written, compiles, reviewed. **Not yet play-tested.** |
| 3 — Smoke and open | Code written, compiles, reviewed. **Not yet play-tested.** |
| 4 — Inspect a frame | Code written, compiles, reviewed. **Not yet play-tested.** |
| 5 — Harvest and sell | Code written, compiles, reviewed. **Not yet play-tested.** |
| 6 — Fun checkpoint | Needs Steps 1–5 play-tested first |

## Step 1 — Walkable yard + look-and-interact

No open design questions.

- Menu `BeeKeeper → Build Hive Ritual Scene` creates
  `Assets/Scenes/HiveRitual.unity`: ground, sun, a placeholder hive (Italian
  breed), a small shed, a first-person player, and a HUD (crosshair, prompt,
  message line).
- Scripts: `OnFoot/FirstPersonController`, `OnFoot/Interactable`,
  `OnFoot/PlayerInteractor`, `OnFoot/HiveInteraction`, `UI/PlayerHud`,
  `Editor/HiveRitualSceneBuilder`.

**Done-when:** press Play → mouse turns the view, WASD walks, Shift
sprints, Esc frees the cursor and a click takes it back. Looking at the hive
from within ~2.5 m shows `[E] Check hive`; pressing E shows breed, bees,
honey kg, health and queen status.

## Step 2 — Game clock, central hive ticker, sleep

- `GameClock`: in-game day + time of day, shown on the HUD.
- A central ticker steps every hive on in-game hours; `HiveController`
  loses its per-frame `Update` and its per-real-second honey rate.
- A "Sleep" interactable (in the shed) skips to the next morning, crediting
  hives for the skipped hours.
- **Needs:** real seconds per in-game day (GDD worksheet, Appendix F #80)
  and a honey rate per in-game day — placeholders now, `sim-balance` pass
  later.
- **Placeholders used:** 1 in-game day = 1200 real seconds
  (`GameClock.realSecondsPerGameDay`); bees forage 07:00–20:00; a
  full-strength healthy colony makes ~5 kg per foraging day
  (`HiveController.kBaseHoneyKgPerForagingHour`), so the starter hive (half
  population) makes ~2.5 kg/day. The builder starts the hive with its 4 kg
  reserve already stored, so day 1's honey is all harvestable.

**Done-when:** the clock advances; honey rises by a sensible amount per
in-game day, not per real hour; Sleep jumps to morning with honey credited.

## Step 3 — Smoke and open the hive

- The smoker as a held tool; puffing the hive calms it for a while.
- The lid lifts off to open the hive and goes back on to close it.
- **Needs:** what happens if you open an unsmoked hive (`QUESTIONS.md` #10).
  Until that's decided: nothing — no sting system in this slice.
- **As built:** the smoker is always in hand (no inventory yet); left mouse
  puffs, and a puff reaching the hive calms it for 30 in-game minutes
  (`HiveInteraction.calmMinutes`, placeholder). E lifts the lid off and sets
  it beside the hive, showing 10 frame top bars; E again puts it back.
  Opening unsmoked only changes the message. Opening currently also shows
  the colony readout, standing in for step 4's frame inspection.

**Done-when:** smoke → open → close works in order, with visible feedback
for each.

## Step 4 — Inspect a frame

- Pull a frame into a close-up; the frame shows how full of honey it is and
  a readout of the colony's health.
- **Needs:** what an inspection reveals (`QUESTIONS.md` #11, #12). First
  pass: honey fill + health readout, nothing diagnosed yet.

- **As built:** 10 `HiveFrame`s per hive. With the lid off, E on a top bar
  lifts that frame out and holds it 45 cm in front of the eyes, comb facing
  the camera; the player can look around but not walk while holding it. The
  comb's capped-honey band is sized from the colony's stores (outer frames
  hold more); the readout gives bee coverage (population), brood quality
  (health) and eggs (queen status). E puts it back. The lid can't go back on
  with a frame out. Smoke aimed at an open frame counts as smoking the hive.

**Done-when:** with the hive open, the player pulls a frame, sees it
up close with the colony's state, and puts it back.

## Step 5 — Harvest and sell

- Take the honey above the reserve (`HiveController.Harvest`) as carried
  honey in kg; a sell crate in the yard calls `EconomyManager.SellHoney`;
  gold shown on the HUD.
- **Needs:** where selling happens and the price (`QUESTIONS.md` #14, #15)
  — placeholder crate and the existing 15/kg until decided.

- **As built:** a new `Secondary` input action (F) in
  `InputSystem_Actions.inputactions`. With the hive open, no frame in hand
  and honey above the reserve, F (looking at the hive or its frames) takes
  it all via `HiveController.Harvest` into `PlayerInventory.carriedHoneyKg`.
  A roadside honey stand (table, jars, blank sign) sells everything carried
  through `EconomyManager.SellHoney` at 15/kg. HUD top-right shows carried
  honey and gold. At the Step 2 placeholders that's ~2.5 kg ≈ 37.5 gold per
  in-game day; there's nothing to spend gold on yet (no sinks).

**Done-when:** harvest → carry → sell → gold goes up by kg × price, and the
hive keeps its reserve.

## Step 6 — Fun checkpoint (`QUESTIONS.md` #44)

Play 3–5 in-game days. Write down what's boring, what's missing, what felt
good. That decides what comes next — more ritual depth, threats and
seasons, or placement.

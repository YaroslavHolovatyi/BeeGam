# Bee Keeper Simulator — Planning Doc

A working document for scoping a Unity game about running an apiary in a
recreated slice of Lviv. This is a thinking-out-loud doc, not a spec —
sections will change as decisions get made. Unresolved items are called out
explicitly under **Open Questions**.

## Status

- Unity: not yet installed; user will install Unity Hub + Editor themselves.
- Project folder: `/home/yaroslavholovatyi/Desktop/bee-game/` (currently just
  this plan + the map reference screenshot).

## Decisions made so far

- **Gameplay loop: Hybrid.** A city-wide top-down/overview mode for placing
  hives, watching bee routes, and running the economy, plus the ability to
  drop into a walk-around 3rd-person view at any hive for hands-on
  inspection/harvesting. This is the most flexible option but also the most
  build-effort — expect the on-foot mode to come after the sim-mode core
  loop is working, not in parallel.
- **Bee types: real-world subspecies.** Different breeds behave differently
  in-game, e.g.:
  - *Italian* — gentle, high honey yield, weaker winter survival
  - *Carniolan* — very gentle, excellent winter survival, fast spring buildup
  - *Buckfast* — disease-resistant, calm, strong all-rounder
  - *Russian* — strong mite/pest resistance, more defensive/aggressive
  - More can be added later; these four are a reasonable starting roster.
- **Setting: a specific real place.** A bounded area of the Sknyliv district
  in Lviv (see below), not a generic/fictional town.
- **City-recreation approach (proposed, not yet confirmed):** hand-build the
  area in Unity using the map screenshot as visual reference, rather than
  importing OSM data or using Google's 3D Tiles API. No API keys, licensing,
  or network dependency; full control over a stylized look. Tradeoff: it
  will be recognizably-based-on the real place, not survey-accurate.
- **MVP area size: whole polygon, low detail.** Block out the entire
  red-bordered area from day one as simple extruded street/building
  footprints (no facade detail). Add real detail later only where gameplay
  actually happens (hive locations); everything else stays scenery-level.
- **Art style: semi-realistic.** Grounded proportions/materials, not full
  photoreal — aim for something in the Cities: Skylines / Farming
  Simulator register. Chosen because geographic recognizability of Lviv
  matters; accept the bigger art budget this implies.
- **Vertical farming / crop growing: cut for now.** The worksheet's "Vertical
  Farming & Gardening" block (plot placement, crop growth cycle, garden-to-
  apiary forage synergy) is removed from scope for this pass. It was a second
  full production chain — its own growth clock, its own harvest action, its own
  economy — bolted onto a game whose core loop (hive → honey → sell → reinvest)
  isn't playable yet. Cutting it is the answer to the worksheet's own cut-line
  question ("if something has to give, what goes first"). Deferred, not
  forbidden: if the apiary loop turns out to feel thin, crops are a known place
  to add depth.
- **Replaced by: buy-and-place furniture and garden pieces.** Instead of
  growing things, the player *buys* them from a shop and places them. This
  keeps the "make the place yours" feeling and the gold sink, at a fraction of
  the cost — a catalog of static placeable props reuses the hive-placement
  machinery (ghost preview, ground raycast, `EconomyManager.TrySpend`) rather
  than needing a new simulation. Tradeoff: no growth/tending gameplay, so decor
  is a one-time purchase decision, not an ongoing activity.
- **Two item classes, split by a rule the player can learn.** Nectar/pollen
  plants and water features are **functional** — they affect the bee sim.
  Everything built (furniture) and every non-melliferous ornamental is **purely
  cosmetic**. Shop entries carry a "bee-friendly" tag showing the bloom window,
  so the split is legible rather than arbitrary. This doubles as the project's
  real-world education hook: not every pretty flower actually feeds bees.
  - *Functional — flower beds.* Each is one placeable object with a bloom
    window on the in-game calendar plus a forage contribution inside a radius.
    Recommended starter set, all real Ukrainian bee plants: willow/crocus (very
    early spring, tiny yield, bridges the gap when colonies are weakest),
    fruit blossom (spring), acacia/black locust (late spring, high yield, short
    window), linden/lipa (early summer, high yield — and Lviv is full of them),
    phacelia (fast, long window, the gap-filler), clover (steady mid-summer
    baseline), sunflower (late summer, big yield), goldenrod (late season,
    builds winter stores). Two numbers and a date range per item.
  - *Functional — water.* A bee waterer, stone basin, or small pond. Bees
    genuinely collect water to cool the hive and dilute honey for brood food.
    Effect: reduces the summer heat penalty for hives in radius. All water
    features are functional, including decorative-looking ones like a birdbath.
  - *Functional — later, gated on weather.* Windbreak hedges (cut the wind
    penalty on foraging) and shade trees only pay off if per-day weather
    (Q7) lands. Hold them until that's decided.
  - *Cosmetic.* All indoor furniture (mansion rebuild + apiary building
    interior); outdoor garden furniture — benches, tables, hammock, fire pit,
    fences, paths, gates, garden lamps; ornamental non-bee planting — hostas,
    ferns, grasses, topiary, potted foliage; statues, signage, seasonal decor.
- **Decor placement: owned land outdoors, building interiors indoors,
  category-gated.** Garden pieces and garden furniture go on owned land; indoor
  furniture goes inside the mansion and the apiary support building. The
  categories don't cross — no sofa in the yard, no flower bed in the workshop.
  Accepts a slightly less freeform builder in exchange for never having to
  solve "does this indoor prop survive rain" or weather-proof every mesh.
- **Real business names: genericized.** SoftServe HQ, McDonald's, ATB,
  Samsung service center, Nova Poshta, etc. keep their real locations/layout
  but get fictional equivalents in-game (generic tech office, generic
  fast-food building, generic supermarket, generic electronics shop, generic
  parcel point). Avoids trademark/logo concerns regardless of whether this
  stays private or gets shared later.

## The reference area (`Part_Of_the_city.jpg`)

Screenshot is Google Maps satellite view with a red polygon marking the
target area, in Lviv's Sknyliv neighborhood, near Lviv International
Airport. Scale bar reads ~100 m; the marked polygon spans roughly
2–2.5 km across — a genuinely large chunk of city.

Notable contents inside/near the border, north to south:
- North edge along **вулиця Патона**; north-east corner reaches up toward
  **вулиця Зоряна / Терлецького**.
- Retail/commercial cluster near the top: **АТБ-Маркет, МакДональдз,
  Samsung service center, SoftServe HQ, Нова Пошта №23**.
- Main streets running through the middle: **вулиця Любінська, вулиця Івана
  Виговського, вулиця Садова, вулиця Симона Петлюри**.
- Residential blocks with a **school (№40), kindergarten (№75), and a
  children's playground**.
- A large forested park in the center-south: **Парк ім. Івана Виговського /
  Скнилівський парк** — the single biggest feature in the frame, and a
  natural fit for bee foraging gameplay (real tree/flower cover).
- South edge along **вулиця Щирецька / вулиця Скнилівська**, with a
  roundabout and small retail (Futura Hub, Гетьман) near the southeast
  corner.
- Lviv Airport is visible at the bottom-left of the screenshot but appears
  to sit just outside the red border — context, not part of the recreated
  area.

## Draft scope (subject to the open questions below)

- **MVP area:** given the polygon is ~2+ km across, modeling all of it in
  hand-built detail for a first playable version is a lot. Likely approach:
  pick a smaller "core" sub-area to build first (e.g. the park plus one or
  two surrounding residential blocks where the player's first hives go),
  and treat the rest of the polygon as a backdrop/expansion target for
  later milestones.
- **Core simulation systems** (rough, to refine):
  - Hive placement & management (capacity, health, queen status)
  - Bee foraging: pathing from hive to flower/tree sources within range,
    affected by season/weather/breed
  - Honey/wax/propolis accumulation & harvest
  - Pest/disease pressure (mites, hive beetles, etc.) countered by
    inspection and treatment actions
  - Economy: sell goods, buy equipment/upgrades, expand to new hive sites
  - Seasons affecting flower bloom and hive activity
- **On-foot mode:** walk up to a hive, open it, use a smoker, pull frames,
  visually inspect for problems, harvest by hand.

## Suggested technical architecture (starting point, not final)

- Unity version: latest LTS at install time (confirm once installed).
- Render pipeline: URP — good stylized-visual support without HDRP's
  overhead, works fine for both a top-down sim view and on-foot 3rd-person.
- Suggested top-level folder structure once the Unity project exists:
  - `Assets/Scenes/` — CityOverview, OnFoot, (later) Menu
  - `Assets/Scripts/Simulation/` — hive, bee-colony, foraging, economy logic
  - `Assets/Scripts/OnFoot/` — player controller, hive-interaction tools
  - `Assets/Scripts/CityGen/` — anything supporting building the map area
  - `Assets/Art/City/`, `Assets/Art/Bees/`, `Assets/Art/Hives/`
  - `Assets/Data/` — ScriptableObjects for bee breeds, hive types, etc.
- Bee breeds as ScriptableObject definitions (yield, temperament, disease
  resistance, cold tolerance, foraging range) so new breeds are data, not
  code changes.

## Open Questions

All resolved (defaults chosen for a solo hobby project; easy to revisit):

- **Player goal/progression: milestone/career goals.** Staged targets (e.g.
  hit a honey-production number, survive a winter with N hives, unlock all
  breeds), with a soft win-condition around fully expanding across the
  recreated area. Play continues sandbox-style after milestones are done.
- **Day/night & seasons: accelerated game-time.** A day passes in minutes,
  a season over several in-game days — standard sim-game pacing. Bees more
  active in daytime/warm season, dormant at night/winter.
- **Multiplayer: single-player only.** No networking work planned. Keep
  simulation logic reasonably decoupled from player input where it's cheap
  to do, so co-op isn't a full rewrite if ever wanted later — but this is
  not an active requirement.
- **Release intent: private/hobby project for now.** No licensing/storefront
  concerns to design around; can revisit if that changes.

## Next steps

- Unity not yet installed — install Unity Hub + a recent LTS Editor version
  (URP template) before scaffolding starts.
- Once installed, scaffold the actual Unity project structure and start
  with the simulation core (hive + single bee breed + basic economy) before
  touching city geometry or the on-foot mode.

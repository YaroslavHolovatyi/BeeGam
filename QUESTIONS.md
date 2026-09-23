# Bee Keeper Simulator — Open Questions

A big batch of questions to fill out the game design beyond what's already
locked in `PLAN.md`. No need to answer all of these at once, or ever —
treat it as a menu. Answer whatever you have opinions on; anything left
blank just gets a sensible default when it's time to build that part.

Already decided (see `PLAN.md`, not repeated here): perspective (first
person on foot most of the time; top-down view unlocked later by buying an
in-game computer), first playable (an on-foot, one-hive "hive ritual"
slice), setting (Sknyliv, Lviv), MVP area scope, art style
(semi-realistic), bee breeds are real subspecies, brand names genericized,
progression (milestone/career + sandbox after), time (accelerated),
single-player, private/hobby project for now, vertical farming cut in
favour of a buy-and-place decor/garden shop.

## 1. Bees & Hive Biology

1. Beyond the four starting breeds (Italian, Carniolan, Buckfast, Russian)
   — how many total breeds should exist eventually, and should rarer/hybrid
   breeds be craftable (cross-breeding two breeds) or only found/bought?
2. Should individual bees be simulated at all (even abstractly — e.g. a
   pool of "worker/forager/nurse" counts), or is a hive just a handful of
   aggregate stats (population, health, honey)?
3. What other hive products matter besides honey — wax, propolis, pollen,
   royal jelly? Do any of them unlock separate crafting/sale chains (e.g.
   candles from wax, mead from honey)?
4. Should hives have a queen-replacement mechanic (requeening) as an active
   player action, or is queen health just a passive stat that decays/heals?
5. Swarming — should hives be able to swarm and split into a new colony if
   overcrowded/neglected, requiring the player to catch/rehive them?
6. How deep should the pest/disease system go — a few abstract "pest
   pressure" and "disease pressure" sliders, or specific named threats
   (varroa mites, hive beetles, foulbrood, wax moths) each with distinct
   symptoms and treatments?
7. Should weather (rain, wind, cold snaps, heatwaves) directly affect
   foraging/health day to day, or only matter at the season level?
8. Do bees actually need to path/fly visually between hive and flowers in
   the overview mode, or is foraging entirely abstracted (a range + a
   flower-density number feeding a formula)? **Now blocks build:** the
   decided flower-bed forage bonus (see `PLAN.md`) needs a concrete forage
   model to feed into — a planted bed has to raise *something*.

## 2. Hive Management & On-Foot Interaction

9. What toolkit does the player use on-foot — smoker, hive tool, bee
   brush, protective suit — and do any of these need to be bought/upgraded/
   maintained (e.g. suit wears out, smoker needs fuel)?
10. Is there a sting/injury risk during on-foot inspection if you skip
    smoking the hive or handle it carelessly, with a gameplay consequence
    (temporary movement penalty, health bar)?
11. How is disease/pest diagnosis done on-foot — pure visual
    inspection (spot the problem on the frame model), a
    minigame/quiz, or an automatic report once you open the hive?
12. Frame-level detail — does the player pull individual frames and
    inspect them one by one, or is "opening the hive" a single interaction
    that reveals aggregate hive health?
13. Equipment tiers — do better hive boxes/frames/tools meaningfully change
    yield or disease resistance, or are they mostly cosmetic/quality-of-life?

## 3. Economy & Progression

14. What's the in-game currency called, and roughly how many
    resource/product types feed into it (just honey, or honey + wax +
    propolis + pollen + royal jelly + processed goods)?
15. Do honey prices fluctuate (market/demand simulation), stay fixed, or
    depend on quality/purity you've cultivated?
16. Are there recurring customers/contracts (e.g. "deliver 5kg to the local
    market by Friday") or is selling always just an instant sell-for-gold
    action?
17. Is there a tech/upgrade tree (better extractors, bigger storage, faster
    harvesting tools), and roughly how many tiers deep?
18. Can the player hire staff/helpers, or is it strictly solo?
19. Loans, land purchase, or permits — should expanding to a new part of
    the city cost money/require unlocking, or is it free once a milestone
    is hit?
20. What are the actual milestone/career goals (from PLAN.md's "milestone
    progression" decision) — got any concrete targets in mind, e.g.
    specific honey totals, hive counts, or "fully expand across the
    recreated area"?

## 4. World / City

21. Should the recreated city area include NPCs (pedestrians, traffic,
    other residents) for atmosphere, or is it a static backdrop with just
    the player and their hives?
22. Do other beekeepers/competitors exist in the world (AI-controlled
    apiaries competing for the same forage), or is the player the only one?
23. Are there city regulations to work around (e.g. can't place hives too
    close to the school/playground shown in the reference screenshot,
    matching real beekeeping ordinances) as a light educational/challenge
    layer?
24. Should real seasonal/weather patterns for Lviv specifically inform the
    in-game calendar (roughly matching when local flowers actually bloom),
    or is season timing just whatever plays best?
25. How does the player get around the city between hive sites — walking
    only, bike, car, or fast-travel between saved locations?
26. Should the map ever expand beyond the originally marked red polygon
    (e.g. toward the airport, or further into the city), or is that area
    a hard boundary for the whole project?

## 5. Player Character

27. Is there any player-character customization (appearance, gender
    presentation, name), or a fixed protagonist?
28. Any skill/leveling system for the player themselves (e.g. gets faster
    at inspections, takes less sting damage over time), separate from
    hive/breed upgrades?
29. Is there any narrative framing at all — e.g. inheriting the apiary from
    a relative, moving to Lviv to start fresh — or purely a gameplay
    sandbox with no story hook?

## 6. Presentation

30. Reference points for the visual target — any specific games whose look
    you'd want to get close to (beyond the earlier Cities: Skylines /
    Farming Simulator comparison), or reference images/moodboard you have
    in mind?
31. Music/audio direction — ambient/embient countryside feel, something
    more upbeat, or not thought about yet?
32. HUD philosophy — minimal/diegetic (info shown in-world where possible)
    or a traditional sim-game HUD with menus, panels, and overlays?
33. Any accessibility needs to design around from the start (colorblind
    modes, remappable controls, subtitle/text-size options)?

## 7. Technical & Production

34. Controls — keyboard + mouse only, or gamepad support planned too?
35. Any target hardware constraint (this dev machine's specs, or a
    specific minimum spec you want the game to run on)?
36. Save system — single save slot is probably fine for a solo project,
    but worth confirming: single slot, multiple slots, or cloud sync ever
    a concern?
37. How much time per week do you realistically expect to put into this,
    and is there a rough timeframe in mind (a playable prototype in a
    month? no deadline at all)? Mostly so I can pace milestone suggestions
    realistically rather than over-scoping.
38. For 3D art (hives, bees, city props) — hand-model in Blender (now that
    we've confirmed the MCP integration works), use free/purchased Unity
    Asset Store packs, or a mix depending on the asset?

## 8. Decoration, Garden & Forage Objects

Spawned by the decision to cut vertical farming and replace it with a
buy-and-place decor/garden shop (see `PLAN.md`). The frame is settled; these
are the parts it left open.

39. The starter roster of forage plants and their bloom windows —
    `PLAN.md` records a recommended set (willow/crocus, fruit blossom,
    acacia, linden, phacelia, clover, sunflower, goldenrod) as the default.
    Confirm, trim, or swap. Do the windows follow real Lviv bloom timing
    (ties to Q24) or whatever paces best?
40. How big is a flower bed's forage radius relative to a breed's foraging
    range, and do overlapping beds stack, cap, or diminish? A stacking rule
    that's too generous turns decor into the optimal strategy and makes hive
    siting irrelevant.
41. Do cosmetic items pay off in *any* non-visual way — a comfort/homeliness
    score, milestone credit for the mansion rebuild — or literally nothing
    but looks? "Nothing but looks" is defensible for a cozy sim, but then
    furniture never competes with a hive for the same gold.
42. Is the shop open from the start, or gated behind land ownership /
    mansion progress / a milestone? Related: can decor be sold back or
    moved after placing, or is a purchase permanent?
43. Does the bee waterer interact with anything besides summer heat — e.g.
    is water a *requirement* colonies suffer without, or purely a bonus?
    The requirement version teaches real beekeeping harder but adds a
    failure mode to explain.

## 9. Scope Check-ins (revisit periodically)

44. Once the first-playable hive-ritual slice (see `PLAN.md`) works, does
    it actually feel fun before more systems get piled on top? Worth an
    explicit "does this feel good" checkpoint before moving to hive
    placement, city geometry, or the breed/hive rosters.
45. At what point does "genericized real city" stop being worth the extra
    modeling effort compared to a simpler fictional town — i.e. is there a
    fallback if city-scale modeling turns out to eat too much time?

## 10. First-Person Play & the Computer

Spawned by the perspective decision (see `PLAN.md`): first person on foot
most of the time, top-down only after buying an in-game computer, and an
on-foot hive-ritual slice as the first playable. Numbered after section 9
so existing question numbers don't shift.

46. What does automation actually do once the computer is bought — harvest
    without the player, treat disease, feed for winter, or only surface
    alerts while the player still does the work by hand? And afterwards,
    does anything still require going on foot (breeding, catching swarms,
    repairs)? The whole top-down unlock exists for this, and it's currently
    undefined.
47. What gates buying the computer — a hive count, a milestone, or just its
    price?
48. Before the computer, there's no overview. How does the player know a
    hive needs attention — walking the rounds and checking each one,
    in-world cues (the hum, activity at the entrance), a notebook/phone
    list, or nothing until the hive is opened? Ties into HUD philosophy
    (#32).
49. How does placing a hive or a decor piece work in first person — a ghost
    preview where the crosshair meets the ground, carrying the item and
    setting it down, or a build menu? The decor decision in `PLAN.md`
    reuses the hive-placement machinery, so one answer covers both.
50. The "whole polygon, low detail" block-out was chosen with a top-down
    camera in mind; seen mostly at eye level, extruded footprints hold up
    much worse. Does the walkable area shrink to a smaller, detailed zone
    with the rest as distant backdrop, does low detail stay acceptable, or
    does this bring the fictional-town fallback (#45) closer?

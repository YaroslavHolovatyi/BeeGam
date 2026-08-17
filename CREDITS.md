# Credits & Asset Provenance

Every third-party asset in this project gets a line here. See
[`ASSET_SOURCES.md`](ASSET_SOURCES.md) for where to find assets and which
licences to accept.

**The rule: log it when you download it, not when you ship it.** Reconstructing
"where did this bench come from" a year and 200 props later ranges from tedious
to impossible, and an asset whose licence can't be established has to be
deleted and remade. Thirty seconds now, or an afternoon and a re-model later.

**CC0 assets still get a line.** Attribution isn't required, but *provenance*
still matters — if the project ever goes public, or a source later turns out to
have relicensed or mis-hosted something, this file is the only record that the
asset was CC0 when it came in.

Tables here run wider than the 78-column wrap the other docs use. That's
deliberate — a ledger is for scanning, not reading.

## How to add an entry

Copy a row into the right table and fill it in. Keep `Added` as an absolute
date (`YYYY-MM-DD`). Under `Licence`, write the specific licence — `CC0 1.0`,
`CC-BY 4.0`, `Unity Asset Store EULA` — not just "free".

```
| Asset | Files in repo | Source | Author | Licence | Added | Notes |
```

---

## 3D models

*(Nothing downloaded yet — the hive placeholder is being modelled in-house.)*

| Asset | Files in repo | Source | Author | Licence | Added | Notes |
|---|---|---|---|---|---|---|
| | | | | | | |

## Textures, materials, HDRIs

| Asset | Files in repo | Source | Author | Licence | Added | Notes |
|---|---|---|---|---|---|---|
| | | | | | | |

## Audio — music, SFX, ambience

| Asset | Files in repo | Source | Author | Licence | Added | Notes |
|---|---|---|---|---|---|---|
| | | | | | | |

## Fonts

| Asset | Files in repo | Source | Author | Licence | Added | Notes |
|---|---|---|---|---|---|---|
| | | | | | | |

## Animations & rigs

Mixamo animations belong here. They're free with an Adobe account, but the
licence lives in Adobe's terms rather than in the downloaded file, so record
the date and what was downloaded.

| Asset | Files in repo | Source | Author | Licence | Added | Notes |
|---|---|---|---|---|---|---|
| | | | | | | |

## Unity packages & template content

Content that arrived with the Unity URP template or via Package Manager —
`Readme.asset`, `TutorialInfo/`, `InputSystem_Actions.inputactions` and
similar. Covered by Unity's own licence terms; listed for completeness so a
future audit doesn't mistake template files for unattributed downloads.

| Asset | Files in repo | Source | Licence | Notes |
|---|---|---|---|---|
| URP template content | `Assets/TutorialInfo/`, `Assets/Readme.asset` | Unity URP project template (`6000.5.4f1`) | Unity licence terms | Shipped with the template; safe to delete once the project outgrows it |
| Input System actions | `Assets/InputSystem_Actions.inputactions` | Unity Input System package | Unity licence terms | Default action map from the template |

## AI-generated assets

Generated meshes from Hyper3D/Rodin, Hunyuan3D, Meshy or similar. Record the
tool, the date, and the prompt — free-tier output terms change, and the prompt
is the only way to regenerate or replace an asset if the terms turn out to be
unusable.

| Asset | Files in repo | Tool | Prompt | Terms at generation | Added |
|---|---|---|---|---|---|
| | | | | | |

---

## Reference material — **not shipped**

Used to model from and to check geography. **None of this may ship in a build**
and none of it may be redistributed. Kept here so the distinction stays
explicit rather than living in someone's memory.

| Item | Files in repo | Source | Status |
|---|---|---|---|
| Sknyliv reference screenshot | `Part_Of_the_city.jpg` | Google Maps satellite view, screenshotted | **Reference only.** Google Maps imagery is not licensed for redistribution. Never ship, never trace-and-ship as a texture. |
| District map reference | `map for game.png` | Map screenshot | **Reference only.** Same restriction. |
| Hive & bee reference images | `imagesAndSoOn/` | Mixed / provenance not recorded | **Reference only.** Provenance unknown — treat as unusable in a build until each image is traced to a source and licence. |
| Mapped territory JSON | GDD worksheet, Appendix C | Derived from map reference | Data, not imagery — but the underlying survey is still third-party. |

Real-world geography itself isn't copyrightable, so a hand-built recreation of
Sknyliv is fine. What isn't fine is shipping the *imagery* it was built from.
`PLAN.md`'s decision to hand-build rather than import OSM or Google 3D Tiles
already keeps the project clear of this — the reference stays reference.

Business names are genericized per `PLAN.md`, which is a trademark matter
rather than an asset-licensing one, but it belongs in the same mental bucket:
the real locations stay, the brands don't.

---

## Assembled attribution for the build

When a build ships, the credits screen needs the CC-BY entries above rendered
as plain text. Assemble that here so it's written once and stays consistent.

*(Empty — no attribution-requiring assets in the project yet.)*

---
name: unity-script
description: Conventions and safety rules for writing or changing C# gameplay code in this Unity project. Use whenever adding or editing anything under Assets/Scripts — MonoBehaviours, ScriptableObjects, simulation logic, or editor tooling.
---

# Writing C# for this project

Unity 6 (`6000.5.4f1`), URP, no assembly definitions — everything compiles into
`Assembly-CSharp` (or `Assembly-CSharp-Editor` for anything under an `Editor/`
folder).

## Layout

- `Assets/Scripts/Data/` — `ScriptableObject` definitions (static, designer-authored
  data: breeds, later flower types, buildings, upgrades).
- `Assets/Scripts/Simulation/` — `MonoBehaviour` runtime logic (hives, economy,
  time, weather).

Put new files in the folder matching that split. Add a new sibling folder rather
than overloading one of these when a genuinely different concern appears
(`Assets/Scripts/UI/`, `Assets/Scripts/City/`).

## House style

Match `HiveController.cs` and `BeeBreedData.cs`:

- **No namespaces.** The project doesn't use them; don't introduce them piecemeal.
- **Inspector-facing fields are public and annotated.** `[Header("...")]` to group,
  `[Tooltip("...")]` to explain units and meaning, `[Range(0f, 1f)]` on normalized
  values. Anything a designer tunes must say what its units are — `honeyCapacityKg`,
  `foragingRangeMeters`, not bare `capacity`.
- **Constants are `k`-prefixed camelCase, declared at the bottom of the class**
  (`const float kBaseHoneyPerSecond = 0.0001f;`).
- **Single-line `///` summaries** on public methods that return something
  non-obvious, stating the unit: *"returns the amount harvested in kg"*.
- Prefer `Mathf.Clamp01` / `Mathf.Min` over hand-rolled clamping.

## Rules that prevent real breakage

1. **Never rename or retype a public serialized field casually.** Field names are
   the serialization keys in `.asset` and `.unity` files. Renaming `honeyStoredKg`
   silently resets every hive in every scene to zero. If a rename is genuinely
   needed, add `[UnityEngine.Serialization.FormerlySerializedAs("oldName")]` and
   say so in your response.
2. **Never hand-edit `.meta` files, and never delete an asset without its `.meta`.**
   The GUID inside is how scenes and prefabs find the asset.
3. **Nothing that scales with hive count belongs in `Update()`.** The design targets
   a city full of hives. Hive simulation is stepped by `HiveTicker` on a fixed
   in-game-time interval (`HiveController.Tick`); add new per-hive simulation
   there, and per-hive animation as coroutines that stop when idle — never as
   per-hive `Update` methods.
4. **No `Find`, `FindObjectOfType`, `GetComponent`, or allocation in a per-frame
   path.** Cache in `Awake`/`OnEnable`.
5. **Scale rates by in-game time, not wall-clock `Time.deltaTime` alone.** The design
   calls for accelerated time; a rate constant tuned against real seconds will be
   wrong the moment a time-scale multiplier lands.

## Verifying a change

Compilation is checked automatically by a `PostToolUse` hook. To run it yourself:

```bash
~/Unity/Hub/Editor/6000.5.4f1/Editor/Data/DotNetSdk/dotnet build Assembly-CSharp.csproj -nologo -v q
```

This compiles only. It cannot tell you whether the game *behaves* correctly —
anything involving scene wiring, inspector values, or actual play still needs the
Unity editor. Say plainly which of the two you did; don't call a change verified
because it compiled.

# Portability Criteria

Six categories. For each red flag: what it is, why the blank-project test hits it, and the fix.

Examples are drawn from this codebase so the shape is recognisable. They illustrate the *pattern*, not a standing list of open issues — re-check them against the code as it is now.

---

## A. Ambient dependencies

Anything the module *reaches for* instead of *being given*. This is the single biggest cause of a module refusing to move: in a blank project the thing it reaches for isn't there, and the failure is usually at runtime, not compile time.

| Red flag | Why it breaks the blank-project test | Fix |
|---|---|---|
| `static X.Instance` singleton | Can't have two, can't construct one without a GameObject, and callers depend on a global that doesn't exist in the new project. `EnemySpawner.cs:46` | Pass the dependency in (`SerializeField` on the shell, or a ctor/`Init` parameter on the logic). Keep the singleton as an optional convenience wired at the top level, never as the way the module finds its collaborators. |
| Static utility that needs initialising | Copy the module, forget the init call, get silence or a null. `MyLogger.Init()` is called from `TacticalGameManager.cs:28` — a class the module would leave behind. | Either make it safe to call uninitialised, or take a small `ILogSink` the caller supplies. A stub is then 3 lines. |
| Static ticking service | `TimerManager.Update(Time.deltaTime)` is pumped by the game manager. A copied module that schedules a timer silently never fires. | Inject the ticker, or have the module own its own update. Make the dependency visible at the seam. |
| `FindObjectOfType`, `GameObject.Find`, `Camera.main`, `Resources.Load` | Depends on scene contents and folder layout the blank project doesn't have. Fails at runtime, often silently. | `[SerializeField]` the reference, or pass it in. |
| `Time.deltaTime`, `UnityEngine.Random`, `Input` read from inside logic | The three classic mock boundaries. Read directly, the logic can't be driven deterministically — you can't reproduce a spawn layout or step a simulation. | Take delta time as a parameter; take a seed or an `IRandom`; have the shell read input and call the logic. |
| Magic tag / layer / scene / action-map names | `InputSystem.actions.FindActionMap("TacticalGame")` throws in a project without that asset. Same for a `LayerMask` set in an inspector the new project doesn't have. | Constants at minimum; serialized fields with sane defaults where possible. Note in the manifest what must be authored. |

## B. Unity-type bleed into logic

The module should separate a **logic core** (plain C#, no `MonoBehaviour`) from a **shell** (the `MonoBehaviour` wiring it to the scene). The bleed to catch is the core reaching *up* into the shell.

**The line is scene objects, not the math library.** `Vector3`, `Rect`, `Mathf`, `Bounds`, `Quaternion` are value types — a logic class using them is fine and portable. What doesn't move is `MonoBehaviour`, `GameObject`, `Transform`, `Component`, and anything holding one.

| Red flag | Why it hurts | Fix |
|---|---|---|
| Logic class stores or accepts a `MonoBehaviour` / `GameObject` / `Transform` | The transitive chain is what gets you. `QuadTreeGrid` (pure C#, 661 lines) takes `EnemyModel`, which holds an `EnemyPresenter : MonoBehaviour` — so the most reusable file in the project can't be copied alone. | Make the logic generic (`QuadTreeGrid<T>`) or have it store a position plus an opaque handle. The Grid should not know what it tracks. |
| Logic only runs from a lifecycle callback | Behaviour reachable only via `Awake` / `Start` / `Update` can't be triggered on demand, so it can't be exercised in isolation. | Put the work in a public method the callback calls. The callback becomes one line. |
| A domain type exists only to wrap a component | `EnemyModel` holding `Presenter` makes every consumer of the model a consumer of the view. | Keep the model free of the view; let the shell own the mapping from model to presenter. |

## C. Construction and wiring

| Red flag | Why it hurts | Fix |
|---|---|---|
| Dependencies constructed internally | The module decides its own collaborators, so no stub can be substituted. | Construct at the top level, pass down. `PopulationDensityController(minArea, PlaneBounds)` is the shape to copy. |
| Required prefab with no fallback | `m_enemyPrefab` must be authored before anything runs — the blank project starts with an exception. | Note it in the manifest, and where cheap, fall back to a primitive so the module runs with nothing authored. |
| Implicit initialisation ordering | Split across `Awake`, `Start`, and an external caller, with an order nothing enforces. Copy one piece and it breaks in a way that looks like a logic bug. | One entry point that fully initialises. If ordering matters, make it explicit at the seam. |
| Config buried in code | Can't vary the test without editing source. | `[SerializeField]` with defaults, so the harness has knobs. |

## D. The seam itself

| Red flag | Why it hurts | Fix |
|---|---|---|
| Callers talk to concrete types | No stub can be substituted, and the module can't be swapped. | An interface at the boundary. `ITacticalGroundStrategy` is the right instinct — a caller holding it can be handed a fake. |
| Interface leaks implementation detail | A nested enum, or a type that exists for one implementation, drags that implementation along and forces every fake to satisfy it. | Move the type out, or split the interface. |
| Interface bundles unrelated axes | Forces a fake to implement things the test doesn't care about. | Split by the thing that varies. |
| Outcome observable only by inspecting the scene | If the only way to know it worked is to look at Transforms, the seam reports nothing and the module can't be checked. | Return the outcome, expose read-only state, or raise an event. `TryGetNewEnemyPosition(out Vector3)` returning a bool is the shape to copy — success and failure both readable at the seam. |

## E. Observability

The human tests by playing. A module that is perfectly decoupled but shows nothing is still untestable, so this category carries as much weight as the rest.

| Red flag | Why it hurts | Fix |
|---|---|---|
| No way to trigger on demand | Exercising it requires booting the whole game. `TacticalGameManager.Start()` holds a hardcoded 50-enemy loop marked `//TEST SPAWN ENEMIES`, so spawning can't be run without a camera, a player and an input map. | A public method plus `[ContextMenu]` on the shell. Move throwaway drive-loops out of production classes into a harness component. |
| No debug draw | Spatial logic with nothing to look at can't be checked by eye. | `OnDrawGizmos` on the shell delegating to a `DrawDebug()` on the logic — the pattern already used at `BG3TacticalGroundController.cs:100`. |
| Silent failures | A skipped spawn that logs nothing looks identical to a bug. | Count and report. `"spawned X, skipped Y (grid full)"` is the shape to copy. |
| No state readable afterwards | Can't tell what happened once it's over. | Expose read-only collections or counts (`ActiveEnemies` is right). |

## F. Assembly definitions

An `.asmdef` is Unity's mechanical portability check: a module in its own assembly cannot compile against code it hasn't declared a reference to, so the compiler enforces what this audit does by reading.

This project has none today, and that is fine. **Never report a missing `.asmdef` as a finding.** Suggest one only when all of these hold:

- the module is already dependency-clean, or one Blocker away from it,
- it is a leaf that others use, rather than a hub that uses others (the `Utils/` files are the natural candidates),
- the user has shown interest in locking the boundary in.

When suggesting, always state the cost: every other assembly referencing it needs an explicit reference added, and code in the default assembly can't be referenced *by* it.

```
Worth an .asmdef once EnemyModel is out of the Grid:
  Assets/Scripts/Utils/Grid.asmdef   references: (none)
Cost:    anything using QuadTreeGrid needs an explicit reference.
Benefit: the compiler, not this skill, keeps it portable.
```

The Test Framework package (`com.unity.test-framework`) is already installed, so a module behind an `.asmdef` *could* also carry EditMode tests. Mention that only if asked — it is not what this skill is for.

---

## Not a finding

Do not report these. They are correct design, or noise that buries the real findings.

- `Vector3`, `Mathf`, `Rect`, `Bounds`, `Quaternion`, `Color` in logic — value types, they move fine.
- A `MonoBehaviour` at the module's edge. That's the shell, and it's supposed to be there.
- `[SerializeField]` private fields for configuration — the intended way to expose knobs.
- Coroutines, `OnDrawGizmos`, `Instantiate`, object pooling in the shell layer.
- Naming, member order, `var` usage, comment style — `/code-review` covers those.
- Commented-out code, empty `Update()` stubs, `//TODO` markers — `/check-code-todo` covers those.
- A module depending on another module *through an interface*. That's the seam working.

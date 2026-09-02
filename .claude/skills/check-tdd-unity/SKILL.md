---
name: check-tdd-unity
description: "Audit a Unity module for portability and observability: could it be lifted into a blank Unity project, with the rest mocked, and driven by hand in a scene? Use when the user asks to check testability, check a module is testable, run a portability audit, or asks \"is this code well structured to be tested outside the project?\"."
---

# Check TDD Unity

This is **not** red-green TDD. Nothing here writes a failing test first, and nothing here writes NUnit tests. The human tests by hand, in a scene, at play time, with real objects.

What this skill checks is whether the code is *shaped* so that testing is possible at all. One governing question:

> **Could this module be copied into a blank Unity project, with everything around it replaced by stubs, and then driven and observed by hand in an empty scene?**

Call that the **blank-project test**. A module passes it when the answer is a short, concrete list of files and stubs. It fails when the honest answer is "you'd have to bring half the project".

Three ideas carry over from ordinary TDD, and they are the whole basis of the audit:

- **Seam** — the public boundary where behaviour is observed without reaching inside. Everything is judged at the seam.
- **Mock only at boundaries** — here the boundary is *the rest of this project*. Faking the project's other modules is right; faking the module's own internals means the seam is in the wrong place.
- **No implementation coupling** — if driving the module requires knowing its internals, it isn't testable from outside.

Read `CONTEXT.md` before reporting, so findings use the project's domain words (Ground, Grid, Cell, Character, Register/Unregister) rather than inventing new ones. Respect anything in `docs/adr/`.

## Process

### 1. Establish scope

The argument is one of two things, and the user says which:

- **A named module** — `/check-tdd-unity QuadTreeGrid`, `/check-tdd-unity enemy spawning`. Resolve it to a file set.
- **A feature in progress** — uncommitted work. Use `git status --short` and `git diff HEAD` to get the touched files, then widen to the module those files belong to.

If no argument is given, ask which of the two it is. Do not guess.

### 2. Confirm the file set and the seam, before auditing

State three things back to the user and wait:

- the files under audit,
- the **seam** — the public boundary a caller would drive this module through,
- anything pulled in that is arguably outside the module.

No audit runs against an unconfirmed seam. This is the step that decides whether the findings land on what the user cares about, and it is cheap to correct here and expensive to correct after a full report.

### 3. Audit

Work through [PORTABILITY-CRITERIA.md](PORTABILITY-CRITERIA.md) against every file in the set. It carries the categories, the Unity-specific red flags, and the "not a finding" list that keeps the report from filling with noise.

### 4. Report

Three sections, in this order, always. The format is fixed below.

### 5. Offer to apply

End with a numbered list of the proposed fixes and ask which to apply. Apply only what the user picks, following `.claude/CODING_STANDARDS.md`. Never refactor during the audit itself: a report the user hasn't read yet is not permission to change code.

## Output format

### Findings

Grouped by severity, worst first. Every finding cites `file:line`, says what the blank-project test would hit, and names a fix.

- **Blocker** — the module cannot compile or cannot run outside the project without changing its code.
- **Friction** — it works once copied, but driving or observing it by hand is painful.
- **Note** — worth knowing, no action needed now.

```
### Blocker — QuadTreeGrid is bound to a MonoBehaviour through EnemyModel
QuadTreeGrid.cs:101,180  AddEnemy(EnemyModel) / m_storedEnemy
  QuadTreeGrid is pure C# with no Unity component of its own, but EnemyModel
  holds an EnemyPresenter (MonoBehaviour), so copying the Grid drags in the
  enemy types and a component.
  Fix: QuadTreeGrid<T>, or store a position plus an opaque handle, so the
  Grid knows nothing about what it is tracking.
```

Rank so the cheapest high-value fix is first. Do not report every violation of every category — a report the user won't act on is a failed report. If a module is clean, say so in one line and move on.

### Copy manifest

The compile-time half of the blank-project test. Three buckets:

```
Module: <name>
Seam:   <the boundary, one line>

COPY   <files that move as-is>
STUB   <what must be faked to compile, with why and rough size>
LEAVE  <what stays behind>
```

Writing this list is the audit's forcing function: a blocker that reading code alone would miss usually shows up as an entry that refuses to fit a bucket. If `STUB` is longer than `COPY`, the seam is in the wrong place — say so.

### Blank-scene harness

The runtime half. Not code — a recipe the user could follow in an empty scene. It must end in a **falsifiable expectation**: a sentence that could turn out false when they press Play.

```
Scene:   <minimum objects>
Prefab:  <what must be authored first, if anything>
Harness: <one GameObject, one driver component>
           [SerializeField] <the knobs>
           [ContextMenu("...")] <how it is triggered on demand>
           OnDrawGizmos -> <what is drawn>

Observe: <what is visible in the Scene view / console>
Expect:  <the falsifiable claim, with numbers>
```

Describe it; do not write the harness script unless the user asks. If the module has no way to be triggered on demand, or no way to show what it did, that is itself a Friction finding — a module you can only exercise by booting the whole game is not hand-testable.

## Rules

- **Judge the seam, not the style.** Naming, formatting and member order belong to `/code-review`, not here. Only raise them if they actually obstruct the blank-project test.
- **Unity is not the enemy; the project is.** Using `Vector3`, `Mathf`, `Rect`, or putting a `MonoBehaviour` at the module's edge is correct design, not a finding. What breaks portability is coupling to *this project's* other modules and to *ambient* global state.
- **Every finding is falsifiable.** It names a file and line, and a concrete consequence in the blank project. "This is hard to test" is not a finding.
- **Don't propose a rewrite.** Prefer the smallest change that moves a Blocker to clean. If a module genuinely needs restructuring, say so once, and ask.
- **Suggest an `.asmdef` only where it would pay off** — see the assembly section of the criteria file. Never treat a missing one as a failure.

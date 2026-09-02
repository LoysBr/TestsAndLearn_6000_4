# Issue tracker: Local Markdown

Issues and specs for this repo live as markdown files in `.scratch/`.

## Conventions

- One feature per directory: `.scratch/<feature-slug>/`
- The spec is `.scratch/<feature-slug>/spec.md`
- Implementation issues are one file per ticket at `.scratch/<feature-slug>/issues/<NN>-<slug>.md`, numbered from `01`, never a single combined tickets file
- Issue state is either `Todo`, `In Progress` (an Agent or the human has started designing it or implementing it), `Needs review` (implementation pass is done, human needs to review the code), `Needs test` (now it needs to be tested and approved), `Closed`, and `Reopened` (when coming back to a `Closed` issue which needs modification in the end)
- Comments and conversation history append to the bottom of the file under a `## Comments` heading

## When a skill says "publish to the issue tracker"

Create a new file under `.scratch/<feature-slug>/` (creating the directory if needed).

## When a skill says "fetch the relevant ticket"

Read the file at the referenced path. The user will normally pass the path or the issue number directly.

## Wayfinding operations

Used by `/wayfinder`. The **map** is a file with one **child** file per ticket.

- **Map**: `.scratch/<effort>/map.md` (the Notes / Decisions-so-far / Fog body).
- **Child ticket**: `.scratch/<effort>/issues/NN-<slug>.md`, numbered from `01`, with the question in the body. A `Type:` line records the ticket type (`research`/`prototype`/`grilling`/`task`); a `Status:` line records `claimed`/`resolved`.
- **Blocking**: a `Blocked by: NN, NN` line near the top. A ticket is unblocked when every file it lists is `resolved`.
- **Frontier**: scan `.scratch/<effort>/issues/` for files that are open, unblocked, and unclaimed; first by number wins.
- **Claim**: set `Status: claimed` and save before any work.
- **Resolve**: append the answer under an `## Answer` heading, set `Status: resolved`, then append a context pointer (gist + link) to the map's Decisions-so-far in `map.md`.


## Help for Human about what is an "ADR" : 

Architecture Decision Records (ADRs) are short documents that capture important architectural decisions, their context, and consequences.  They are used to document software design choices that address significant functional or non-functional requirements, helping teams avoid revisiting settled debates and facilitating knowledge transfer for new developers. 

Key practices for using ADRs in code design include:

Structure: Each ADR should focus on a single decision and include sections for status, date, context, the decision itself, alternatives considered, and consequences (positive, negative, and neutral). 
Storage: ADRs are typically stored in a dedicated directory (e.g., docs/adr/) within the project's source code repository, allowing them to be version-controlled alongside the code.
Immutability: Once accepted, an ADR should remain immutable; if a decision changes, a new ADR should supersede the previous one rather than editing the original. 
When to Write: ADRs should be written for significant choices such as technology selection, architectural patterns (e.g., microservices vs. monolith), breaking changes, or performance-critical optimizations. 
Common formats like MADR (Markdown Any Decision Records) provide standardized templates to ensure consistency across projects.  Using ADRs improves transparency, traceability, and collaboration by making the rationale behind technical choices explicit and accessible to all stakeholders.
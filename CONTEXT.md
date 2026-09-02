Use [CONTEXT_SKILLS.md](.claude\CONTEXT_SKILLS.md) to know the concepts used by skills and the human. 

# Tactical Game

This is a small Unity Project with only one developer working on it. It is on a public github. 

A prototype of a tactical game: a Player commands Characters on the Ground, while Enemies are placed across that same Ground under a Minimum Spacing rule.

## Files In the project 

I use ./TODO.txt as a temporary list of remaining tasks but we will switch to the **Issue tracker** 

## Language

**Controller** is not a domain word here. It currently labels four unrelated concepts in the code (a Player, a Character's body, a Ground Strategy, the camera), so "the controller" identifies nothing on its own. Say which one is meant.

### The playing surface

**Ground**:
The surface where characters and objects are positioned. A game-design concept: it knows nothing about how it is subdivided or indexed underneath.
_Avoid_: Plane, Tactical Grid, Terrain

**Ground Bounds**:
The four oriented corners delimiting the Ground, in world space.

**Plane**:
The Unity `Plane` primitive used to build a Ground. An implementation detail — never a substitute for "Ground".

**Grid**:
The logical subdivision of the Ground, invisible to players. It exists to answer spatial questions about the Ground — where there is still room, what sits in a given area, who is nearby — without the Ground having to know about it. Its only use today is controlling density at spawn time.
_Avoid_: Tactical Grid (implies tiles you move on, which this is not)

**Cell**:
One subdivision of the Grid. Invisible, and purely bookkeeping — a Cell is never something a Character "stands on".

**Tile**:
A discrete square of a tile-based Ground that a Character occupies and moves between: visible, and part of the movement rules. No Ground has Tiles today. Reserved so it never blurs with Cell — a tile-based Ground would still have a Grid beneath it, whose Cells need not line up with its Tiles.

**Minimum Spacing**:
The smallest distance two Characters may be placed apart on the Ground. The rule the Grid is meant to enforce when placing Enemies.

**Density**:
How tightly Characters are packed over an area of Ground. The effect of enforcing Minimum Spacing — not a rule of its own, and never a substitute term for it.

**Distribution**:
How placements are spread over the Ground when several areas still have room. *Maximum Spread* keeps Characters as far apart as the Grid allows; *Randomly Scattered* accepts clumping.

**Ground Strategy**:
The rule set that turns a location the Player points at into a legal destination for a Character, and gives feedback about it. Movement granularity is what varies between strategies: free-form (any point on the Ground) or tile-based.

### Pointing and feedback

**Pointer Context**:
What the pointer is currently over — Ground, an Enemy, a Character, or something else. A fact about the world, and what decides which cursor the Player sees.

**Ground Indicator**:
A mark placed on the Ground to tell the Player about a location. It serves one of two roles, and they are not interchangeable:

- **Movement Preview**: follows the pointer, showing where the Selected Character *would* go.
- **Movement Confirmation**: marks the destination that *was* commanded.

### Who acts on the Ground

**Character**:
An entity with a body that moves, animates and acts in the game (health, actions). A Character is not inherently controlled by anyone — control comes from a Player.
_Avoid_: Unit, Actor, Pawn

**Player**:
The object through which one human commands Characters. One Player per human, on one machine, with their own Input Settings — these never come apart, so "Player" names all of it at once. A Player is never itself present on the Ground: it has no body. An AI that commands Characters is not a Player and gets its own name.
_Avoid_: Client, User (a networking layer may identify a Player by a client id; that is not a second concept)

**Input Settings**:
One Player's mapping from raw input to commands. Belongs to a Player, not to a Character: the same Character commanded by a different Player is driven by different Input Settings.

**Team**:
The grouping a Character belongs to. Teams decide hostility; they say nothing about who commands the Characters on them.

**Enemy**:
A Character on a Team hostile to the one whose point of view is being taken. Relative, not intrinsic. Hostility alone decides: a Character nobody commands is not thereby an Enemy, and an Enemy may well be commanded.

**Selected Character**:
The Character a Player is currently commanding.

**Position**:
Where a Character is *now*. It changes as the Character moves, and the Grid tracks it so it can answer proximity questions.

**Spawn Position**:
The Position a Character is placed at when it enters the game. Only ever the starting value of a Position — never a synonym for it.

### Two lifecycles, never confused

**Spawn / Delete**:
A Character entering or leaving the game. Deleting a Character ends its existence.

**Register / Unregister**:
The Grid starting or stopping to track a Character's Position. A Character stays Registered for as long as it exists; the Grid never Deletes anything.
_Avoid_: Add / Remove for either pair — "remove" has meant both, which is how the two get confused.

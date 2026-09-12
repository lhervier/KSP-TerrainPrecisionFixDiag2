# Terrain Precision Fix - Diagnostic Mod 2

A measuring instrument for KSP 1.12. It lets you check, on your own install, a claim about the patch
of ground your craft is parked on:

> **The ground KSP builds under you is never built at the same height twice.** Load the same save five
> times, and the surface your craft is standing on comes back a little higher or a little lower each
> time — a few centimetres apart on Kerbin, less on smaller worlds.

## Why it matters

Every time you load, it is a coin toss between two outcomes.

**The ground comes back lower than it was when you saved.** Your craft is now hovering a couple of
centimetres above it, so it drops those two centimetres. You never notice, and nothing breaks.

**The ground comes back higher than it was when you saved.** Your craft is now *inside* the ground —
and the physics engine will not leave two solid things overlapping. It pushes them apart, hard, in
the only direction available: up. Your craft gets launched.

That second case is the symptom everybody already knows. The lander that twitches, hops or flips the
moment the scene finishes loading. The base that sat perfectly flush yesterday and is buried up to
the hatches today. The big base that tears itself apart the very first time you load it, and never
again afterwards. A craft with many parts spread over a wide area gives the coin toss more chances
to land the wrong way up.

### Disclaimer: it is not the only cause

The ground moving is one cause among several, and this page does not claim it is the only one. Plenty
of other things move a craft when a scene opens. Two well-known examples, among others:

- **suspensions.** Landing legs and wheels come back fully extended, because that is the only state
  KSP can restore them to. They then compress under the weight of the craft, and the craft moves
  while they do.
- **a craft bent to fit the ground.** While you play, physics twists the joints between parts so the
  craft settles onto the shape of the ground beneath it. That twisting is not saved. On loading, the
  craft comes back in its original, unbent shape — and if the ground is not flat, part of it really
  *is* underground, with no measurement error involved.

Neither of them touches the reading below: this instrument measures the ground itself, and does not
care what the craft standing on it is made of.

## This mod's demonstration

There are two grounds in KSP, and they are not the same object.

One is **computed**. The game works out the height of its terrain, for any spot on any world, from the
formulas the world is made of. That is the number behind altimeters, maps, and where the sea ends.

The other is **built**. When the scene opens, the game assembles a mesh of triangles for the terrain
around you. That mesh is what your landing legs touch, what your wheels roll on, what stops you
falling through.

They should agree. This mod reads both, for the spot your craft is standing on, and records them side
by side — one line per loading, so that reloading the same save a few times fills a table you can read
straight down.

It is the second instrument for the same problem. The first,
[Terrain Precision Fix Diag](https://github.com/lhervier/KSP-TerrainPrecisionFixDiag), measures the
**craft**: it shows that a craft set down on the ground does not come to rest where the save left it,
one loading to the next. This one measures **the ground** itself, and does not care what the craft
does.

## The window

In flight, a window shows a table with one line per loading. The **bottom line is the reading in
progress**: it carries `--` where the others carry a record number, since it is not a record until the
*Record* button at the end of it freezes it into the table. The table survives scene changes, so the
lines pile up as you reload.

| column | meaning |
|---|---|
| **Ground under craft** | the height of the surface your craft is resting on, found by pointing a ray straight down at it |
| **Ground KSP computes** | the height the game works out for that same spot |
| **Difference** | the first minus the second |

All three are in millimetres, above sea level for the first two.

Every frozen line carries a *Delete* button, and *Clear table* throws away the lot. The table lives in
memory only, and empties itself when KSP is closed.

The bottom line is measured afresh every frame, so it follows your craft: drive a rover and you watch
both heights change as the ground under it changes. What you record has to be taken standing still —
stop, wait for the digits to stop moving, then press *Record*.

That wait is short. It is the craft settling, not the ground: the terrain is built when the scene
opens and does not move afterwards. Once the craft is still, the numbers are still.

## How to read it

**This mod does not tell you which of the two heights is the wrong one.** It cannot: there is no third
ruler to check them against. What it shows is something better, and it is why the two are recorded
side by side:

> Reload the same save several times, and **one of those two columns gives the same digits on every
> line while the other does not.** The ground did not change between the loadings. So the column that
> moves is the one that is wrong.

**Ground KSP computes** is the one that repeats. Reload as many times as you like, on the same spot,
and it comes back identical. **Ground under craft** comes back somewhere else each time, and the
**Difference** column is where you read it at a glance. The layout, with the two extremes of a real
Kerbin campaign in it:

```
Record #   Ground under craft (mm)   Ground KSP computes (mm)   Difference (mm)
   1            64,780.800                 64,784.900               -4.100
   ...
   5            64,857.900                 64,784.900              +73.000
```

Everything is in millimetres, as in the first instrument: the numbers to compare are a few hundredths
of a millimetre apart, and metres would need so many decimals that the eye would stop following them.
(The campaign quoted here was recorded to a tenth of a millimetre, hence the round endings.)

This is the same argument the first instrument makes, in the same shape. There, the columns are the
distance from the craft to the centre of its world, read when the scene opens and again once the craft
has settled: the first never varies, so what varies is where the craft ended up, so the ground it
landed on moved. Here the reasoning is applied to the ground directly, with no craft in the middle of
it.

Near the KSC on Kerbin, one save loaded five times gave a computed height identical on all five, to a
tenth of a millimetre, and a resting surface that wandered over **77 mm** — appearing and disappearing
under a craft that never moved. That is the coin toss at the top of this page, measured on the ground
itself.

## Why the difference is never zero

*Difference* does not read zero even when nothing is wrong, and it is not supposed to.

A curved surface cannot be built out of flat pieces. Worlds in KSP are round, and the collision mesh
is made of flat triangles about a hundred and fifty metres across, with their corners sitting on the
true surface. Between the corners, a flat triangle sags below the curve it stands in for — the way a
straight plank laid across the top of a barrel touches only at its two ends.

So on open, flat ground the difference lands slightly **negative**, and that is correct. On Kerbin it
comes to about **−5.5 mm**. How much exactly depends on the size of the triangles and on the size of
the world, so another world gives another figure, and there is no reason for it to be −5.5 mm
anywhere but Kerbin.

**A zero is not what a healthy reading looks like. A number that repeats is.**

One thing the sign does tell you on its own: on open ground, a mesh that sags below the curve it
approximates can only read **negative**. A reading of `+40 mm` puts the ground you are standing on
*above* the ground the game computes, which no amount of triangles explains. (In a crater bottom or a
narrow gully the terrain curves the other way and so can the sign — one more reason the protocol
below asks for flat, open ground.)

## Measuring with the fix installed

[Terrain Precision Fix](https://github.com/lhervier/KSP-TerrainPrecisionFix) is the mod that repairs
this. Installing it and watching this window is what this instrument is for, so here is how to read
the result — and what *not* to expect.

**Do not expect *Difference* to get smaller.** It will not, and it is not supposed to. With the fix
installed, it settles at the value it should have had all along: on Kerbin, the −5.5 mm of a flat
triangle sagging inside a curve. Compare one stock loading against one fixed loading and the number
may well look *worse* — if the stock loading happened to draw a −4 mm, the fixed −5.5 mm looks bigger.
It is not. You compared two numbers neither of which was the point.

**Expect the *Difference* column to stop varying.** That is the fix, and that is all of it:

| | spread of the *Difference* column, over five or six loadings |
|---|---|
| stock KSP | tens of millimetres |
| with the fix | a hundredth of a millimetre |

Measured near the KSC on the same save: 77 mm of spread in stock, 0.05 mm with the fix. Three orders
of magnitude, and it does not depend on drawing a lucky loading.

**And expect the remaining −5.5 mm to stay.** It is not a leftover error to be chased. Removing it
would mean giving the collision mesh more triangles, which costs frames, for five millimetres nobody
can feel. The fix puts the mesh where it belongs; the sag of a flat triangle inside a curve is
geometry, and geometry stays.

Figures on this page come from measurement campaigns run on Kerbin near the KSC, with an earlier,
developer-facing version of this probe — the one written while the fix was being worked out. Nothing
here asks you to take them on trust: reproducing them is what this mod is for.

## The protocol

**1. Put a craft down on bare, flat ground.** The apron just off the end of the runway does nicely.
`Alt+F12 → Cheats → Set Position`, tick *Use middle click to set position*, then middle-click a patch
of grass.

⚠️ **Not on the launchpad and not on the runway.** Those are structures, not terrain. KSP puts them in
place its own way, and a reading taken there mixes two things that have nothing to do with each
other.

⚠️ **Flat, open ground.** Two reasons. In a hollow the terrain curves the other way, which changes
what a healthy difference looks like and takes the meaning out of the sign. And a craft that slides,
even slowly, takes the ray with it: on a slope, a few centimetres of drift move the computed height
too, and the column that is supposed to repeat stops repeating for a reason that is not the one being
measured.

**2. Save once.** Wait for the digits to stop moving, then press *Record*: that is your first line.

**3. Load that same save.** Not a new save — the one from step 2, again.

**4. Wait for the digits to settle and press *Record* again.** A second line appears under the first.

**5. Repeat steps 3 and 4** until you have five or six lines. One loading proves nothing: the error is
drawn afresh every time and can come out small by luck.

⚠️ **Never save again until you are done.** Saving writes a new position for the craft, and you would
be measuring your own round trip on top of the ground.

Then install [Terrain Precision Fix](https://github.com/lhervier/KSP-TerrainPrecisionFix) and do the
same thing again.

## Get it

Either way you end up with the same `GameData/TerrainPrecisionFixDiag2Mod/` folder.

**Download it** — from the assets of the
[latest release](https://github.com/lhervier/KSP-TerrainPrecisionFixDiag2/releases/latest).

**Or compile it** — clone this repository, set `KSPDIR` to your KSP install folder and run
`build.bat`. It needs the .NET SDK, takes a few seconds, reads the KSP assemblies straight from your
install, and puts the DLL in `GameData/TerrainPrecisionFixDiag2Mod/` inside the repository. It does
not install anything. Worth doing if you would rather not run a binary you have no source for while
reporting a measurement.

## Install

Drop `GameData/TerrainPrecisionFixDiag2Mod` into the `GameData` of KSP, so that you end up with
`GameData/TerrainPrecisionFixDiag2Mod/TerrainPrecisionFixDiag2Mod.dll`. It runs on a stock install:
no Harmony, no ModuleManager, no dependency of any kind.

It reads the world and writes nothing at all: the table lives in memory and is gone when you close the
game. Your saves are never touched. Removing the folder removes the mod.

## License

MIT

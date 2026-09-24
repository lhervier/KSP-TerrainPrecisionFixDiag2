# Terrain Precision Fix - Diagnostic Mod 2

**⚠️ Work in progress.** This is an active investigation, not a finished mod. The figures, the code and the conclusions on this page can still change, and several questions are still open.

A measuring instrument for KSP 1.12. It lets you check, on your own install, a claim about the patch
of ground your craft is parked on:

> **The ground KSP builds under you is never built at the same height twice.** Load the same save five
> times, and the surface your craft is standing on comes back a little higher or a little lower each
> time — a few centimetres apart on Kerbin, less on smaller worlds.

**How this was made.** Written with Claude, Anthropic's AI assistant, and reviewed line by line by a
human — me. I am saying so up front, because contributions made with an AI deserve a closer look than
others, and because some people would rather stop reading here. That look is easy to give here: this
mod changes nothing in the game, so what there is to check is the reading itself — the two methods that
take it are quoted in full, the source is public, the protocol runs on a stock install with no
dependency of any kind, and every figure on these pages is read straight off the screenshot next to it,
on your own craft if you would rather take them again.

## Why it matters

Every time you load, it is a coin toss between two outcomes.

**The ground comes back lower than it was when you saved.** Your craft is now hovering a couple of
centimetres above it, so it drops those two centimetres. You never notice, and nothing breaks.

**The ground comes back higher than it was when you saved.** Your craft is now *inside* the ground —
and the physics engine will not leave two solid things overlapping. It pushes them apart, hard, in
the only direction available: up. Your craft gets launched.

![A craft jumping on its own the moment a save is reloaded](https://raw.githubusercontent.com/lhervier/KSP-TerrainPrecisionFix/master/imgs/Booing-scaled.gif)

*KSP 1.12 with [KSP Community Fixes](https://github.com/KSPModdingLibs/KSPCommunityFixes) as the only
mod installed. A pod on an empty fuel tank, parked in the grass at the KSC, saved, then reloaded from the
pause menu, several times if needed — nothing touched in between.*

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

There are two grounds in KSP, and they are not the same object: the one the game **computes** for any
spot on any world, from the formulas that world is made of, and the one it **builds** out of flat
triangles when the scene opens — the one your landing legs touch. They should agree. This mod reads
both for the spot your craft is standing on, from two short methods that share nothing but the vessel,
and records them side by side, one line per loading. A correct reading is not zero, and the shape of
the ground says how far from it, and on which side.

**→ Full chapter: [This mod's demonstration](docs/this-mods-demonstration.md)**

## The window

In flight, a window shows a table with one line per loading, in millimetres: **Ground under craft**,
**Ground KSP computes**, and **Difference**, the first minus the second. The bottom line is the reading
in progress, measured afresh every frame, until the *Record* button at the end of it freezes it into the
table. The table survives scene changes, lives in memory only, and is gone when you close the game.

**→ Full chapter: [The window](docs/the-window.md)**

## The protocol

Three protocols, one for each way the game can set a craft down on the ground. All three fill the
same window.

**Loading the same save.** Launch a craft, move it off the runway onto bare ground with the debug
menu, let it settle and save once — then load that same save, wait for the digits to stop moving,
press *Record*, and do it again five or six times. One loading proves nothing: the error is drawn
afresh every time, and can come out small by luck. Step by step, with screenshots.

**→ Full chapter: [The protocol: loading the same save](docs/the-protocol-loading.md)**

**Coming back to a craft you left.** This one loads nothing at all. A craft stays parked while you
drive a rover away from it, past 2500 m, where the game unloads it — then back to within 200 m, where
its physics starts again. Five records per round trip, and as many round trips as you like, without
ever changing scene.

**→ Full chapter: [The protocol: coming back to a craft you left](docs/the-protocol-approach.md)**

**Switching to a craft far away.** Two craft landed 1.97 km apart, in a save that comes with this
mod. Load the save while flying one, press *Record*, switch to the other with the game's own key, and
press *Record* again. Then load the same save again, six times in all.

**→ Full chapter: [The protocol: switching to a craft far away](docs/the-protocol-switching.md)**

## The measurements

Taken in an install with Harmony, ModuleManager and
[KSP Community Fixes](https://github.com/KSPModdingLibs/KSPCommunityFixes) — what most players run —
with this mod added.

**Loading the same save** ([the protocol in full](docs/the-protocol-loading.md)). The same save,
loaded six times, on Kerbin, on the Mun, on Minmus and on Gilly. The computed height comes back with
the same digits every time — spread over 0.000 mm on Minmus, four hundredths of a millimetre at worst
on sloping ground. The ground the craft is standing on never comes back twice: lowest to highest,
108.1 mm on Kerbin, 15.0 mm on the Mun, 4.1 mm on Minmus, 3.0 mm on Gilly. One held still and the
other wandered, with nothing changed in between.

**→ Full chapter: [The measurements: loading the same save](docs/the-measurements-loading.md)**

**Coming back to a craft you left** ([the protocol in full](docs/the-protocol-approach.md)). Six
round trips in a row on Kerbin, in a single flight, with nothing loaded at any point. The computed
height reads the same digits on every line; the ground under the craft comes back somewhere else
every time, by 2.7 to 21.8 mm, and it has already moved by the time the craft is back in range,
before physics takes it over.

**→ Full chapter: [The measurements: coming back to a craft you left](docs/the-measurements-approach.md)**

**Switching to a craft far away** ([the protocol in full](docs/the-protocol-switching.md)). Six
rounds on Kerbin, each starting by loading the save. The ground under the capsule is somewhere else at
every loading, over 120.4 mm, and switching to the capsule does not move it: the two lines of a round
agree to within five thousandths of a millimetre.

**→ Full chapter: [The measurements: switching to a craft far away](docs/the-measurements-switching.md)**

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

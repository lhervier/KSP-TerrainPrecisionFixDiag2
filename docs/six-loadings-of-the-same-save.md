# Six loadings of the same save

Part of [Terrain Precision Fix Diag 2](../README.md): the readings taken with this instrument, on four worlds, on a stock install. The steps that produced them are in [The protocol](the-protocol.md).

The experiment is one thing, repeated. Park a craft on bare ground, let it settle, save once — then
load that same save, press *Record*, load it again, record again, and keep going until you have five
or six lines. Nothing changes in between: not the craft, not the spot, not the world. Every line is
the same question, asked again. (Step by step, with screenshots, in
[The protocol](the-protocol.md).)

## The readings

On Kerbin first: one save, on the flat grass just off the end of the runway, loaded six times.

![Six loadings of the same save, on Kerbin](../imgs/80-record-again-and-again.png)

(The bottom line of the screenshot is the sixth loading, still live, not a seventh one. It carries
`--` instead of a number, since it is not a record until you freeze it.)

Then the same campaign on the Mun, on Minmus and on Gilly — the first and the last of those on ground
that is anything but flat:

![Six loadings of the same save, on the Mun](../imgs/81-mune.png)

![Six loadings of the same save, on Minmus](../imgs/82-minmus.png)

![Six loadings of the same save, on Gilly](../imgs/83-gilly.png)

## What the numbers say

**Ground KSP computes never moves.** On Kerbin it reads 64,784.952 on all six lines of the screenshot,
the same eight digits every time; on the Minmus flats, `0.000` six times over, which is as plain as
this argument gets. The most it ever wanders is on the Mun and on Gilly, by four or five hundredths of
a millimetre — and those are also the two spots where the ground is not level: a craft that settles a
hair to one side is asking for the height of a slightly different point, and on a slope that shows.
Nothing surprising in any of it. That height is worked out from the formulas the world is made of, and
reloading a save does not change the world.

**Ground under craft moves every time.** On Kerbin it lands somewhere else on each of the six lines,
over a range of ten centimetres. And since *Difference* is one column minus the other, it carries the
whole of that movement — which is what lets the four campaigns be put side by side on that one column
alone, in millimetres:

| loading | Kerbin | Mun | Minmus | Gilly |
|---|---|---|---|---|
| 1 | +48.139 | −39.998 | −13.604 | +38.170 |
| 2 | +78.164 | −36.002 | −13.431 | +38.962 |
| 3 | −2.031 | −35.570 | −16.567 | +40.115 |
| 4 | +77.765 | −38.419 | −14.918 | +36.422 |
| 5 | −28.477 | −40.427 | −15.264 | +37.579 |
| 6 | +31.155 | −44.363 | −14.474 | +37.601 |
| **lowest to highest** | **106.6** | **8.8** | **3.1** | **3.7** |
| *the same, for* **Ground KSP computes** | *0.000* | *0.035* | *0.000* | *0.046* |

The bottom two rows are the argument entire. The craft never moved and the spot never changed, so
nothing about that patch of ground was different from one loading to the next — and yet one of the two
heights held still while the other wandered by tens of millimetres. The one that moved is the one that
is wrong, and it is the one that describes the surface your landing legs actually touch: it was simply
not built in the same place twice.

The sign says it too. That grass is about as flat as Kerbin gets, so a correct reading there is
negative, and around −5.5 mm at that (why, in
[Why a correct reading is not zero](this-mods-demonstration.md#why-a-correct-reading-is-not-zero)).
Four of the six lines are positive: they are not merely scattered, they are on the wrong side of the
only value they could legitimately have had.

Smaller world, smaller spread — but it never goes away, and on none of the four does it come close to
what the computed height does: two to three orders of magnitude, world after world, between one row
and the one under it.

All four campaigns were run on the same **stock install**, with nothing added to `GameData` but this
mod. There is no mod conflict to look for, and nothing to uninstall: this is what KSP does on its own.

Most players have [KSP Community Fixes](https://github.com/KSPModdingLibs/KSPCommunityFixes)
installed, so the same four campaigns were run a second time in an install that has it, and those
screenshots are in [`imgs/kspcf`](../imgs/kspcf). The spread is of the same order on every world. The
tables above stay on the stock readings on purpose: a measurement meant to show what bare KSP does is
worth more taken where nothing else is installed.

Every figure above is read straight off the screenshots above it, and nothing here asks you to take
any of them on trust: reproducing them is what this mod is for.

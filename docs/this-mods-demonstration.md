# This mod's demonstration

Part of [Terrain Precision Fix Diag 2](../README.md): the two heights the mod reads at every loading, and why a correct reading is not zero.

There are two grounds in KSP, and they are not the same object.

One is **computed**. The game works out the height of its terrain, for any spot on any world, from the
formulas the world is made of. That is the number behind altimeters, maps, and where the sea ends.

The other is **built**. When the scene opens, the game assembles a mesh of triangles for the terrain
around you. That mesh is what your landing legs touch, what your wheels roll on, what stops you
falling through.

They should agree. This mod reads both, for the spot your craft is standing on, and records them side
by side — one line per loading, so that reloading the same save a few times fills a table you can read
straight down.

The built one is read by pointing a ray straight down at it and asking the physics engine where it was
stopped:

```csharp
private static double MeasureCollisionSurface(Vessel vessel)
{
    CelestialBody body = vessel.mainBody;
    Vector3 position = vessel.vesselTransform.position;
    Vector3 up = FlightGlobals.getUpAxis(body, position);
    RaycastHit hit;
    if (!Physics.Raycast(position + up * Constants.RAY_START_HEIGHT, -up, out hit,
            Constants.RAY_LENGTH, Constants.LOCAL_SCENERY_MASK, QueryTriggerInteraction.Ignore))
    {
        return double.NaN;
    }

    return (((Vector3d)hit.point - body.position).magnitude - body.pqsController.radius) * 1000.0;
}
```

The computed one is simply asked for, at the same latitude and longitude:

```csharp
private static double MeasureComputedTerrain(Vessel vessel)
{
    CelestialBody body = vessel.mainBody;
    Vector3 position = vessel.vesselTransform.position;

    return body.TerrainAltitude(
        body.GetLatitude(position),
        body.GetLongitude(position),
        allowNegative: true
    ) * 1000.0;
}
```

Two things about that pair are deliberate. **They share nothing but the vessel**: each one reads the
position and the body again for itself, and neither is computed from the other — which is what makes
putting them side by side worth anything. And **they describe the same spot for free**: the ray runs
along the radius through the vessel, and latitude and longitude are what a direction from the centre of
a body *is*, so they hold all the way down that radius. Wherever the ray lands, it lands at the
vessel's own latitude and longitude — whatever the slope, and however the craft leans. Both sides are
in double precision from end to end, because reading a few millimetres out of six hundred kilometres
leaves no room for anything less.

It is the second instrument for the same problem. The first,
[Terrain Precision Fix Diag](https://github.com/lhervier/KSP-TerrainPrecisionFixDiag), measures the
**craft**: it shows that a craft set down on the ground does not come to rest where the save left it,
one loading to the next. This one measures **the ground** itself, and does not care what the craft
does.

## Why a correct reading is not zero

The two never land on the same number, and they are not supposed to. Zero is the wrong thing to hope
for: **a correct reading is a small number that is not zero**, and it is the shape of the ground that
decides how large it is and which sign it takes.

The collision mesh is made of flat triangles about a hundred and fifty metres across, with their
corners sitting on the surface the game computes. Chained one to the next, those flat pieces do follow
the lie of the land — that is what makes a mesh look like ground at all. What they cannot follow is
what the ground does *between* two corners. A hummock, a dip, the shoulder of a crater: the flat piece
cuts straight across, and the ray hits it wherever it happens to pass.

![On rough ground the mesh cuts straight across whatever the ground does between two corners](../imgs/why-not-zero-rough.svg)

So the ray stops short of the computed height, or goes past it, by whatever the mesh left out — on
broken ground, centimetres. The sign is the ground's to give: under the computed height where the
ground rises between two corners, over it where it dips. None of that is the defect this page is
about, and — the part that matters — **none of it should move from one loading to the next**: the
shape of the terrain should be worked out the same way every time, so whatever a hillock adds to your
reading, it ought to add again on the next loading, to the micrometre. Which is what makes the reading
worth taking on a slope or in a crater just as much as on a lawn — the Kerbin, Mun and Gilly tables
in [The measurements: loading the same save](the-measurements-loading.md) were taken on ground that
is anything but flat. Whatever the terrain is doing, it is not what varies.

A legitimate zero exists, it is simply not something you can aim for: right on a corner, the one place
where the mesh touches the surface it stands in for, and anywhere between two corners where the ground
happens to cross the flat piece spanning it. Everywhere else there is a gap.

> **And note that even perfectly flat ground does not read zero.** Worlds in KSP are round, so a flat
> triangle laid across the curve of one sags below it — the way a straight plank across the top of a
> barrel touches only at its two ends. On Kerbin that sag is about **−5.5 mm**. It is the floor under
> every reading, and out on ground as flat as the apron by the runway it is very nearly the whole of a
> correct one: negative, and known before you take it.

**A zero is not what a healthy reading looks like. A number that repeats across loadings is.**

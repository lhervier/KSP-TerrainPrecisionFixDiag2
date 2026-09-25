# The saves and the runs

Part of [Terrain Precision Fix Diag 2](../README.md): the saves and the logs of
[the approach protocol](../docs/the-protocol-approach.md) and
[the switching protocol](../docs/the-protocol-switching.md), and of loadings on Real Solar System. What their readings say is in
[The measurements: coming back to a craft you left](../docs/the-measurements-approach.md) and
[The measurements: switching to a craft far away](../docs/the-measurements-switching.md).

- [`approach-kerbin.sfs`](approach-kerbin.sfs) — the save the approach protocol uses: a capsule
  landed on the flat grass west of the KSC, and a rover 26 m from it.
- [`switch-kerbin.sfs`](switch-kerbin.sfs) — the save the switching protocol uses: a capsule landed
  on the same grass, and a rover 1.97 km to the south of it.

Copy a save into the folder of a sandbox game and load it from that game.

- [`runs/switching-stock.log`](runs/switching-stock.log) — the `KSP.log` of the session the six
  switching rounds were taken in.

## On Real Solar System

Taken on [Real Solar System](https://github.com/KSP-RO/RealSolarSystem) 20.1.3.0 and what it requires
(Kopernicus, Modular Flight Integrator, KSPTextureLoader, the RSS textures), on an install of their own:
these saves only load there. The protocol is [the loading protocol](../docs/the-protocol-loading.md).

- [`reload-moon-rss.sfs`](reload-moon-rss.sfs) — a capsule on an empty FL-T100, landed on flat ground
  on the Moon.
- [`reload-moon-rss-resave.sfs`](reload-moon-rss-resave.sfs) — the same craft, saved again at a later
  load.
- [`reload-earth-rss-resave.sfs`](reload-earth-rss-resave.sfs) — the same kind of craft, on the grass
  about 1.4 km west of the KSC on Earth.

Copy a save into the folder of a sandbox game and load it from that game.

- [`runs/reload-moon-rss-stock.log`](runs/reload-moon-rss-stock.log) — six loads of
  `reload-moon-rss-resave.sfs`, with Terrain Precision Fix Diag 1 open as well.
- [`runs/reload-earth-rss-stock.log`](runs/reload-earth-rss-stock.log) — six loads of
  `reload-earth-rss-resave.sfs`, with Terrain Precision Fix Diag 1 open as well.

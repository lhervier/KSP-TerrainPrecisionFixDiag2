using System.Collections.Generic;
using UnityEngine;

namespace com.github.lhervier.ksp.terrainprecisionfixdiag2
{
    /// <summary>
    /// Ground recorder. Follows the ground under a craft -- the one being flown, or the target when one
    /// is set on another craft -- read the two ways the ground exists in KSP: the surface a ray straight
    /// down actually hits, and the altitude the game computes for the same spot. The player freezes the
    /// pair into a table whenever it suits them, and the table survives scene changes, so reloading the
    /// same save several times builds it up line by line.
    ///
    /// Reads the world, nothing else: it moves no vessel and touches no setting.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class TerrainPrecisionFixDiag2Mod : MonoBehaviour
    {
        private static readonly List<Reading> READINGS = new List<Reading>();

        // The line in progress, the only one that still moves.
        private readonly Reading live = new Reading();

        // The vessel the readings are taken under, and what to tell the player about that choice.
        // Refreshed by Update, displayed by OnGUI.
        private Vessel subject;
        private string subjectLabel = "";

        private void Update()
        {
            live.CollisionSurfaceMm = double.NaN;
            live.ComputedTerrainMm = double.NaN;

            Vessel vessel = SelectSubject(out subjectLabel);
            subject = vessel;
            if (vessel == null || !vessel.loaded || vessel.mainBody == null
                || vessel.mainBody.pqsController == null)
            {
                return;
            }

            // The two readings share nothing but the vessel: one asks the physics engine what is under
            // it, the other asks the game what should be. Neither is derived from the other, which is
            // what makes putting them side by side worth anything.
            //
            // They still describe the same spot, and for free. The ray runs along the radius through the
            // vessel, and latitude and longitude are what a direction from the centre of the body is:
            // they are the same all the way down that radius, so the point the ray hits sits at the
            // vessel's own latitude and longitude, whatever the slope and however the vessel leans.
            live.CollisionSurfaceMm = MeasureCollisionSurface(vessel);
            live.ComputedTerrainMm = MeasureComputedTerrain(vessel);
        }

        /// <summary>
        /// The vessel the readings are taken under: the target when one is set on a vessel, the craft
        /// being flown otherwise. Null when that vessel cannot be read. <paramref name="label"/> receives
        /// what to tell the player about the choice, including why nothing can be read.
        /// </summary>
        private static Vessel SelectSubject(out string label)
        {
            ITargetable target = (FlightGlobals.fetch == null) ? null : FlightGlobals.fetch.VesselTarget;
            if (target != null)
            {
                // A target is anything targetable: a planet, a docking port, a craft. GetVessel gives
                // the vessel behind it, and null for what is not one -- targeting a planet leaves the
                // readings on the craft being flown rather than emptying the window.
                Vessel targetVessel = target.GetVessel();
                if (targetVessel != null)
                {
                    if (!targetVessel.loaded)
                    {
                        label = "Target: " + targetVessel.vesselName + " -- too far away to read";
                        return null;
                    }
                    label = "Target: " + targetVessel.vesselName;
                    return targetVessel;
                }
            }

            Vessel active = FlightGlobals.ActiveVessel;
            label = (active == null) ? "Nothing to read" : "Craft you are flying: " + active.vesselName;
            return active;
        }

        /// <summary>
        /// Altitude of the surface the vessel is resting on, above the terrain datum of its body, in
        /// millimetres. NaN when there is no terrain to be found under it.
        /// </summary>
        private static double MeasureCollisionSurface(Vessel vessel)
        {
            CelestialBody body = vessel.mainBody;

            // The ray starts above the vessel, because a vessel can sit below the collision surface and a
            // ray cast from inside the ground finds nothing at all -- one of the states worth seeing
            // rather than one to hide. It starts as low as it can, because PhysX computes the hit point
            // in single precision from the ray's origin: the step of a float is about 0.5 mm at 5 km and
            // 2 um at 20 m, and what is being read here is a few millimetres.
            Vector3 position = vessel.vesselTransform.position;
            Vector3 up = FlightGlobals.getUpAxis(body, position);
            RaycastHit hit;
            if (!Physics.Raycast(position + up * Constants.RAY_START_HEIGHT, -up, out hit,
                    Constants.RAY_LENGTH, Constants.LOCAL_SCENERY_MASK, QueryTriggerInteraction.Ignore))
            {
                return double.NaN;
            }

            // Measured from the datum the terrain itself is measured from, rather than through
            // CelestialBody.GetAltitude, which subtracts CelestialBody.Radius. The two are the same
            // number on stock bodies, but nothing in the game ties them together -- they are two fields
            // filled in from data -- and a body where they differed would put a constant offset into the
            // difference, which is the one column that has to mean something.
            return (((Vector3d)hit.point - body.position).magnitude - body.pqsController.radius) * 1000.0;
        }

        /// <summary>
        /// Altitude the game computes for the terrain under the vessel, above the terrain datum of its
        /// body, in millimetres.
        /// </summary>
        private static double MeasureComputedTerrain(Vessel vessel)
        {
            CelestialBody body = vessel.mainBody;
            Vector3 position = vessel.vesselTransform.position;

            // Through latitude and longitude, which looks like a detour but is the only correct route:
            // PQS.GetRelativePosition returns a vector in a frame the rotation of the planet separates
            // from the one the terrain is sampled in, and reading the terrain through it lands on another
            // longitude entirely. allowNegative keeps a sea floor readable instead of clamping it to zero.
            return body.TerrainAltitude(
                body.GetLatitude(position),
                body.GetLongitude(position),
                allowNegative: true
            ) * 1000.0;
        }

        // =========================================================
        // UI
        // =========================================================

        private Rect windowRect = new Rect(
            Constants.WINDOW_X,
            Constants.WINDOW_Y,
            Constants.WINDOW_WIDTH,
            0f
        );

        private void OnGUI()
        {
            GUI.skin = HighLogic.Skin;
            windowRect = GUILayout.Window(
                Constants.WINDOW_ID,
                windowRect,
                DrawWindow,
                "Terrain Precision Fix Diag 2"
            );
        }

        private void DrawWindow(int id)
        {
            GUILayout.BeginVertical();

            // Which craft the numbers are taken under. Without it the window reads the same whether it
            // follows the craft being flown or the one being approached.
            GUILayout.Label(subjectLabel);
            GUILayout.Space(4f);

            // Header
            GUILayout.BeginHorizontal();
            DrawCells("Record #", "Ground under craft (mm)", "Ground KSP computes (mm)",
                "Difference (mm)");
            GUILayout.EndHorizontal();

            // Recorded lines
            int deleteIndex = -1;
            for (int i = 0; i < READINGS.Count; i++)
            {
                Reading reading = READINGS[i];
                GUILayout.BeginHorizontal();
                DrawCells(
                    FormatUtils.Format(i + 1),
                    FormatUtils.Format(reading.CollisionSurfaceMm),
                    FormatUtils.Format(reading.ComputedTerrainMm),
                    FormatUtils.FormatSigned(reading.DifferenceMm())
                );
                if (GUILayout.Button("Delete", GUILayout.Width(Constants.COL_BUTTON)))
                {
                    deleteIndex = i;
                }
                GUILayout.EndHorizontal();
            }
            if (deleteIndex >= 0)
            {
                READINGS.RemoveAt(deleteIndex);
            }

            // Current line
            GUILayout.BeginHorizontal();
            DrawCells(
                FormatUtils.Format(FormatUtils.NO_NUMBER),
                FormatUtils.Format(live.CollisionSurfaceMm),
                FormatUtils.Format(live.ComputedTerrainMm),
                FormatUtils.FormatSigned(live.DifferenceMm())
            );
            if (GUILayout.Button("Record", GUILayout.Width(Constants.COL_BUTTON)))
            {
                // A line with nothing to read is worth freezing too: recorded while the vessel is out
                // of reach, it marks in the table that the two lines around it are separated by a real
                // trip away, and not by two readings taken where the player stood.
                READINGS.Add(
                    new Reading
                    {
                        CollisionSurfaceMm = live.CollisionSurfaceMm,
                        ComputedTerrainMm = live.ComputedTerrainMm
                    }
                );
            }
            GUILayout.EndHorizontal();

            if (double.IsNaN(live.CollisionSurfaceMm))
            {
                GUILayout.Label(subject == null
                    ? "Nothing to read there. Come closer, or clear the target to read your own craft."
                    : "No ground under that craft. Land somewhere, or wait for the scene to finish "
                        + "loading.");
            }

            // Clear table button
            GUILayout.Space(10f);
            if (GUILayout.Button("Clear table"))
            {
                READINGS.Clear();
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        /// <summary>Draws the four columns of one line. The caller owns the surrounding horizontal group,
        /// so that it can put a button at the end of the line.</summary>
        private static void DrawCells(string record, string collider, string analytic, string difference)
        {
            GUILayout.Label(record, GUILayout.Width(Constants.COL_RECORD));
            GUILayout.Label(collider, GUILayout.Width(Constants.COL_ALTITUDE));
            GUILayout.Label(analytic, GUILayout.Width(Constants.COL_ALTITUDE));
            GUILayout.Label(difference, GUILayout.Width(Constants.COL_DIFFERENCE));
        }
    }
}

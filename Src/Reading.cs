namespace com.github.lhervier.ksp.terrainprecisionfixdiag2
{
    /// <summary>
    /// One line of the table: the ground at one spot, read the two ways the ground exists in KSP, both as
    /// altitudes above the terrain datum of the body, in millimetres. Both start unknown, and a frozen
    /// line is just one that has stopped being updated.
    /// </summary>
    internal class Reading
    {
        /// <summary>Altitude of the surface the vessel rests on.</summary>
        public double CollisionSurfaceMm = double.NaN;

        /// <summary>Altitude the game computes for the terrain at that same spot.</summary>
        public double ComputedTerrainMm = double.NaN;

        /// <summary>
        /// How far the collision surface is from the computed terrain, negative when the surface is the
        /// lower of the two. NaN as long as either altitude is unknown.
        /// </summary>
        public double DifferenceMm()
        {
            return CollisionSurfaceMm - ComputedTerrainMm;
        }
    }
}

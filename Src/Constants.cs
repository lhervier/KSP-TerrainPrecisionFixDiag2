namespace com.github.lhervier.ksp.terrainprecisionfixdiag2
{
    /// <summary>Fixed sizes, identifiers and measuring distances of the flight window.</summary>
    internal static class Constants
    {
        /// <summary>Identifier of the flight window. Any value no other window in the game uses.</summary>
        public const int WINDOW_ID = 0x47485002;

        // Where the window shows up before the player drags it, and how wide it is. The height is left to
        // the layout, which grows it as records pile up.
        public const float WINDOW_X = 60f;
        public const float WINDOW_Y = 60f;
        public const float WINDOW_WIDTH = 660f;

        // Column widths, in pixels. Fixed rather than laid out by content: the numbers only speak once
        // aligned as a column, and the skin font is not monospaced.
        public const float COL_RECORD = 70f;
        public const float COL_ALTITUDE = 175f;
        public const float COL_DIFFERENCE = 120f;
        public const float COL_BUTTON = 90f;

        /// <summary>
        /// Layer the terrain collider lives on (Local Scenery), and the only one the ray is allowed to
        /// hit. The craft is on another layer, so it never blocks the ray.
        /// </summary>
        public const int LOCAL_SCENERY_MASK = 1 << 15;

        /// <summary>
        /// How far above the craft the ray starts, in metres. Do not raise it: the reason it is this low
        /// is in the comment where the ray is cast.
        /// </summary>
        public const float RAY_START_HEIGHT = 20f;

        /// <summary>
        /// How far the ray travels, in metres. Long enough to reach the ground under a craft parked on a
        /// ledge, short enough not to reach for something far away when there is nothing underneath.
        /// </summary>
        public const float RAY_LENGTH = 220f;
    }
}

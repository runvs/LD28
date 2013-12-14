using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    class GameProperties
    {

        public static int WorldSizeInTiles { get { return 30; } }

        public static float TileSizeInPixel { get { return 50.0f; } }

        public static float SidebarWidth { get { return 200.0f; } }

        public static float PlayerMovementDeadZoneTimeInSeconds { get { return 0.15f; } }

        public static int BasePlayerHealth { get { return 25; } }

        public static int BaseDamage { get { return 5; } }

        public static Vector2f InventoryHeadItemPosition { get { return new Vector2f(750.0f, 100.0f); } }
        public static Vector2f InventoryHandItemPosition { get { return new Vector2f(750.0f, 150.0f); } }
        public static Vector2f InventoryTorsoItemPosition { get { return new Vector2f(750.0f, 20.0f); } }
        public static Vector2f InventoryFeetItemPosition { get { return new Vector2f(750.0f, 300.0f); } }

        #region Palette colors

        public static Color ColorBlack { get { return new Color(20, 12, 28); } }
        public static Color ColorDarkBrown { get { return new Color(68, 36, 52); } }
        public static Color ColorDarkBlue { get { return new Color(48, 52, 109); } }
        public static Color ColorDarkGrey { get { return new Color(78, 74, 78); } }
        public static Color ColorBrown { get { return new Color(133, 76, 48); } }
        public static Color ColorDarkGreen { get { return new Color(52, 101, 36); } }
        public static Color ColorLightRed { get { return new Color(208, 70, 72); } }
        public static Color ColorLightGrey { get { return new Color(117, 113, 97); } }
        public static Color ColorBlue { get { return new Color(89, 125, 206); } }
        public static Color ColorLightBrown { get { return new Color(210, 125, 44); } }
        public static Color ColorLightBlue { get { return new Color(133, 149, 161); } }
        public static Color ColorLightGreen { get { return new Color(109, 170, 44); } }
        public static Color ColorSkin { get { return new Color(210, 170, 153); } }
        public static Color ColorTurquoise { get { return new Color(109, 194, 202); } }
        public static Color ColorBeige { get { return new Color(218, 212, 94); } }
        public static Color ColorWhite { get { return new Color(222, 238, 214); } }

        #endregion Palette colors

    }
}

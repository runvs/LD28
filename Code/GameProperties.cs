namespace JamTemplate
{
    class GameProperties
    {

        public static int WorldSizeInTiles { get { return 30; } }

        public static float TileSizeInPixel { get { return 50.0f; } }

        public static float SidebarWidth { get { return 200.0f; } }

        public static float PlayerMovementDeadZoneTimeInSeconds { get { return 0.25f; } }

        public static int BasePlayerHealth { get { return 25; } }

        public static SFML.Window.Vector2f InventoryHeadItemPosition { get { return new SFML.Window.Vector2f(750.0f, 100.0f); } }
        public static SFML.Window.Vector2f InventoryHandItemPosition { get { return new SFML.Window.Vector2f(750.0f, 150.0f); } }
        public static SFML.Window.Vector2f InventoryTorsoItemPosition { get { return new SFML.Window.Vector2f(750.0f, 20.0f); } }
        public static SFML.Window.Vector2f InventoryFeetItemPosition { get { return new SFML.Window.Vector2f(750.0f, 300.0f); } }

    }
}

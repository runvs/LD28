namespace JamTemplate
{
    class GameProperties
    {

        public static int WorldSizeInTiles { get { return 30; } }

        public static float TileSizeInPixel { get { return 50.0f; } }

        public static float SidebarWidth { get { return 200.0f; } }
        public static float PlayerMovementDeadZoneTimeInSeconds()
        {
            return 0.25f;
        }
    }
}

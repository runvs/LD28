/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net

using SFML.Graphics;
using SFML.Window;
using System;

namespace JamTemplate
{
    class GameProperties
    {

        public static float TileSizeInPixel { get { return 50.0f; } }
        public static int WorldSizeInTiles { get; set; }

        public static float SidebarWidth { get { return 200.0f; } }

        public static float PlayerMovementDeadZoneTimeInSeconds { get { return 0.125f; } }
        public static float EnemyMovementDeadZoneTimeInSeconds { get { return 0.9f; } }

        public static int IncreaseAttributeExperienceCost { get { return 3; } }
        public static int BuyItemGoldCost { get { return 4; } }
        public static int BuyHealGoldCost { get { return 2; } }

        public static int PlayerBaseHealth { get { return 11; } }
        public static int PlayerBaseStamina { get { return 35; } }

        public static float PlayerBaseStaminaRegenFrequency { get { return 0.8f; } }
        public static float PlayerBaseHealthRegenFrequency { get { return 5.5f; } }

        public static int BlockStaminaCost { get { return 2; } }
        public static int AttackStaminaCost { get { return 3; } }
        public static int MagicStaminaCost { get { return 7; } }

        public static float PlayerBattleDeadZoneTimer { get { return 0.35f; } }
        public static float EnemyBattleDeadZoneTimer { get { return 0.55f; } }

        public static int BasePlayerHealth { get { return 25; } }


        public static int PlayerBaseDamage { get { return 5; } }
        public static int EnemyBaseDamage { get { return 3; } }

        public static int PlayerMagicBaseDamage { get { return 4; } }

        public static int AttackerEvadeBonus { get { return 2; } }
        public static int BlockEvadeBonus { get { return 3; } }

        public static float SpellMoveMentTime { get { return 0.16f; } }
        public static float SpellFrameTime { get { return 0.075f; } }


        public static Vector2f InventoryHeadItemPosition { get { return new Vector2f(672.0f, 20.0f); } }
        public static Vector2f InventoryHandItemPosition { get { return new Vector2f(616.0f, 58.0f); } }
        public static Vector2f InventoryTorsoItemPosition { get { return new Vector2f(676.0f, 88.0f); } }
        public static Vector2f InventoryFeetItemPosition { get { return new Vector2f(672.0f, 166.0f); } }

        static public Random RandomGenerator = new Random();

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

        static private Font _gameFont;

        static public Font GameFont() { if (_gameFont == null) { _gameFont = new Font("../GFX/font.ttf"); } return _gameFont; }

        #region Sequence Timers
        static public float IntroDisplayTime { get { return 25.0f; } }
        #endregion Sequence Timers

        public static int EnemyEasyGold { get { return 1 + RandomGenerator.Next(2); } }
        public static int EnemyEasyExperience { get { return 2; } }
        public static int EnemyNormalGold { get { return 3 + RandomGenerator.Next(3); } }
        public static int EnemyNormalExperience { get { return 3; } }
        public static int EnemyHardGold { get { return 5 + RandomGenerator.Next(4); } }
        public static int EnemyHardExperience { get { return 5; } }

        public static float EnemyStandStillTime { get { return 2.5f; } }
    }
}

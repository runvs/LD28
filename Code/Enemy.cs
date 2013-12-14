using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    public class Enemy : Actor
    {
        #region Fields

        public int EnemyNumber { get; set; }
        public string PlayerName { get; private set; }
        public Vector2i PlayerPosition { get; private set; }

        public Item DropItem { get; private set; }

        private Texture _enemyTexture;
        private Sprite _enemySprite;

        #endregion Fields

        #region Methods



        #endregion Methods
    }
}

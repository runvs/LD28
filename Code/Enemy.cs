using SFML.Graphics;
using SFML.Window;
using System;

namespace JamTemplate
{
    public class Enemy : Actor
    {
        #region Fields

        public int EnemyNumber { get; set; }
        public string PlayerName { get; private set; }

        public Item DropItem { get; private set; }

        #endregion Fields

        #region Methods

        public Enemy(World world, Vector2i initPosition)
        {
            _world = world;

            ActorAttributes = new Attributes();

            try
            {
                LoadGraphics();
            }
            catch (SFML.LoadingFailedException e)
            {
                Console.Out.WriteLine("Error loading player Graphics.");
                Console.Out.WriteLine(e.ToString());
            }
        }

        private void LoadGraphics()
        {
            _actorTexture = new SFML.Graphics.Texture("../gfx/enemy.png");

            _actorSprite = new Sprite(_actorTexture);
            _actorSprite.Scale = new Vector2f(2.0f, 2.0f);
        }

        #endregion Methods
    }
}

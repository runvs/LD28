using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    class Sidebar
    {

        #region Fields

        private Texture _sideBarTexture;
        private Sprite _sideBarSprite;
        private Player _player;


        #endregion Fields

        #region Methods

        public void LoadGraphics()
        {
            _sideBarTexture = new Texture("../GFX/sidebar_background.png");
            _sideBarSprite = new Sprite(_sideBarTexture);
            _sideBarSprite.Scale = new Vector2f(2, 2);

            _sideBarSprite.Position = new Vector2f(600, 0);
        }

        public Sidebar(Player p)
        {
            _player = p;
            LoadGraphics();
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(_sideBarSprite);
        }

        #endregion Methods
    }
}

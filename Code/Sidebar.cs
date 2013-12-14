using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    class Sidebar
    {

        #region Fields

        public Texture SideBarTexture { get; private set; }
        public Sprite SideBarSprite { get; private set; }

        #endregion Fields

        #region Methods

        public void LoadGraphics()
        {
            SideBarTexture = new Texture("../GFX/sidebar_background.png");
            SideBarSprite = new Sprite(SideBarTexture);
            SideBarSprite.Scale = new Vector2f(2, 2);

            SideBarSprite.Position = new Vector2f(600, 0);
        }

        public Sidebar()
        {
            LoadGraphics();
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(SideBarSprite);
        }

        #endregion Methods
    }
}

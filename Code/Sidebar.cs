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

        private Font font;

        #endregion Fields

        #region Methods

        public void LoadGraphics()
        {
            _sideBarTexture = new Texture("../GFX/sidebar_background.png");
            _sideBarSprite = new Sprite(_sideBarTexture);
            _sideBarSprite.Scale = new Vector2f(2, 2);

            _sideBarSprite.Position = new Vector2f(600, 0);
            font = new Font("../GFX/font.ttf");
        }

        public Sidebar(Player p)
        {
            _player = p;
            LoadGraphics();
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(_sideBarSprite);

            DrawAttributes(window);


            if (_player != null)
            {
                if (_player.HandItem != null)
                {
                    _player.HandItem.Draw(window, new Vector2i(0, 0));
                }
                if (_player.HeadItem != null)
                {
                    _player.HeadItem.Draw(window, new Vector2i(0, 0));
                }
                if (_player.FeetItem != null)
                {
                    _player.FeetItem.Draw(window, new Vector2i(0, 0));
                }
                if (_player.TorsoItem != null)
                {
                    _player.TorsoItem.Draw(window, new Vector2i(0, 0));
                }
            }

        }

        private void DrawAttributes(RenderWindow window)
        {
            SFML.Graphics.Text text = new Text("", font);
            text.DisplayedString = "STR " + _player.ActorAttributes.Strength.ToString();
            text.Position = new Vector2f(630.0f, 250);
            text.Color = new Color(222, 238, 214);
            window.Draw(text);

            if (_player.ActorAttributes.ModifierStrength != 0)
            {
                text = new Text("", font);
                text.DisplayedString = "+ " + _player.ActorAttributes.ModifierStrength.ToString();
                text.Position = new Vector2f(730.0f, 250);
                text.Color = new Color(222, 238, 214);
                window.Draw(text);
            }


            text = new Text("", font);
            text.DisplayedString = "AGI " + _player.ActorAttributes.Agility.ToString();
            text.Position = new Vector2f(630.0f, 300);
            text.Color = new Color(222, 238, 214);
            window.Draw(text);

            if (_player.ActorAttributes.ModifierAgility != 0)
            {
                text = new Text("", font);
                text.DisplayedString = "+ " + _player.ActorAttributes.ModifierAgility.ToString();
                text.Position = new Vector2f(730.0f, 300);
                text.Color = new Color(222, 238, 214);
                window.Draw(text);
            }


            text = new Text("", font);
            text.DisplayedString = "INT " + _player.ActorAttributes.Intelligence.ToString();
            text.Position = new Vector2f(630.0f, 350);
            text.Color = new Color(222, 238, 214);
            window.Draw(text);

            if (_player.ActorAttributes.ModifierIntelligence != 0)
            {
                text = new Text("", font);
                text.DisplayedString = "+ " + _player.ActorAttributes.ModifierIntelligence.ToString();
                text.Position = new Vector2f(730.0f, 350);
                text.Color = new Color(222, 238, 214);
                window.Draw(text);
            }

            text = new Text("", font);
            text.DisplayedString = "END " + _player.ActorAttributes.Endurance.ToString();
            text.Position = new Vector2f(630.0f, 400);
            text.Color = new Color(222, 238, 214);
            window.Draw(text);

            if (_player.ActorAttributes.ModifierEndurance != 0)
            {
                text = new Text("", font);
                text.DisplayedString = "+ " + _player.ActorAttributes.ModifierEndurance.ToString();
                text.Position = new Vector2f(730.0f, 400);
                text.Color = new Color(222, 238, 214);
                window.Draw(text);
            }

        }

        #endregion Methods
    }
}

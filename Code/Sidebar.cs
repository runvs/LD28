using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    class Sidebar
    {

        #region Fields

        private Texture _sideBarTexture;
        private Sprite _sideBarSprite;

        private Texture _overlayTexture;
        private Sprite _overlaySprite;

        private Player _player;

        private RectangleShape _healthBar;
        private RectangleShape _staminaBar;

        #endregion Fields

        #region Methods

        public void LoadGraphics()
        {
            _sideBarTexture = new Texture("../GFX/sidebar_background.png");
            _sideBarSprite = new Sprite(_sideBarTexture);
            _sideBarSprite.Scale = new Vector2f(2, 2);

            _sideBarSprite.Position = new Vector2f(600, 0);

            _overlayTexture = new Texture("../GFX/sidebar_overlay.png");
            _overlaySprite = new Sprite(_overlayTexture);
            _overlaySprite.Scale = new Vector2f(2.0f, 2.0f);

            _overlaySprite.Position = new Vector2f(600.0f, 450.0f);


            _healthBar = new RectangleShape(new Vector2f(100, 150));
            _healthBar.Origin = new Vector2f(0, 150);
            _healthBar.Position = new Vector2f(602, 598);
            _healthBar.FillColor = GameProperties.ColorLightRed;

            _staminaBar = new RectangleShape(new Vector2f(94, 150));
            _staminaBar.Origin = new Vector2f(0, 150);
            _staminaBar.Position = new Vector2f(704, 598);
            _staminaBar.FillColor = GameProperties.ColorBlue;
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
            DrawBars(window);



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

        private void DrawBars(RenderWindow window)
        {
            var percentage = _player.ActorAttributes.HealthCurrent / (float)_player.ActorAttributes.HealthMaximum;
            _healthBar.Scale = new Vector2f(1, percentage);
            window.Draw(_healthBar);

            percentage = _player.ActorAttributes.StaminaCurrent / (float)_player.ActorAttributes.StaminaMaximum;
            _staminaBar.Scale = new Vector2f(1, percentage);
            window.Draw(_staminaBar);

            window.Draw(_overlaySprite);

        }

        private void DrawText(string s, Vector2f position, Color color, RenderWindow window)
        {
            Text text = new Text(s, GameProperties.GameFont());
            text.Position = position;
            text.Color = color;
            window.Draw(text);
        }

        private void DrawAttributes(RenderWindow window)
        {
            DrawText("STR " + _player.ActorAttributes.Strength, new Vector2f(630.0f, 250), GameProperties.ColorWhite, window);

            if (_player.ActorAttributes.ModifierStrength != 0)
            {
                DrawText("+ " + _player.ActorAttributes.ModifierStrength, new Vector2f(730.0f, 250), GameProperties.ColorWhite, window);
            }

            DrawText("AGI " + _player.ActorAttributes.Agility, new Vector2f(630.0f, 300), GameProperties.ColorWhite, window);

            if (_player.ActorAttributes.ModifierAgility != 0)
            {
                DrawText("+ " + _player.ActorAttributes.ModifierAgility, new Vector2f(730.0f, 300), GameProperties.ColorWhite, window);
            }

            DrawText("INT " + _player.ActorAttributes.Intelligence, new Vector2f(630.0f, 350), GameProperties.ColorWhite, window);

            if (_player.ActorAttributes.ModifierIntelligence != 0)
            {
                DrawText("+ " + _player.ActorAttributes.ModifierIntelligence, new Vector2f(730.0f, 350), GameProperties.ColorWhite, window);
            }

            DrawText("END " + _player.ActorAttributes.Endurance, new Vector2f(630.0f, 400), GameProperties.ColorWhite, window);

            if (_player.ActorAttributes.ModifierEndurance != 0)
            {
                DrawText("+ " + _player.ActorAttributes.ModifierEndurance, new Vector2f(730.0f, 400), GameProperties.ColorWhite, window);
            }

        }

        #endregion Methods
    }
}

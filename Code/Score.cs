using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    public class Score
    {

        #region Fields

        private Font font;

        private Player _player;

        #endregion Fields

        #region Methods

        public Score(Player player)
        {
            _player = player;
        }

        public void Draw(RenderWindow rw)
        {
            Text CreditsText = new Text("Game Over", GameProperties.GameFont());
            CreditsText.Color = GameProperties.ColorWhite;
            CreditsText.Scale = new Vector2f(1.5f, 1.5f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 20);
            rw.Draw(CreditsText);

            // TODO Gold and Experience
        }

        #endregion Methods

    }
}

using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    public class Score
    {

        #region Fields

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

            CreditsText.Scale = new Vector2f(1.0f, 1.0f);
            CreditsText.DisplayedString = "Gained Experience: " + _player.ActorAttributes.TotalExperience;
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 100);
            rw.Draw(CreditsText);

            CreditsText.Scale = new Vector2f(1.0f, 1.0f);
            CreditsText.DisplayedString = "Spent Experience: " + _player.ExperienceSpent;
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 125);
            rw.Draw(CreditsText);



            CreditsText.Scale = new Vector2f(1.0f, 1.0f);
            CreditsText.DisplayedString = "Collected Gold: " + _player.TotalGold;
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 175);
            rw.Draw(CreditsText);

            CreditsText.Scale = new Vector2f(1.0f, 1.0f);
            CreditsText.DisplayedString = "Spent Gold: " + _player.GoldSpent;
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 200);
            rw.Draw(CreditsText);

            CreditsText.Scale = new Vector2f(1.0f, 1.0f);
            CreditsText.DisplayedString = "Owned Items:";
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 250);
            rw.Draw(CreditsText);

            int i = 0;

            foreach (string s in _player._ownedItems)
            {
                CreditsText.Scale = new Vector2f(1.0f, 1.0f);
                CreditsText.DisplayedString = s;
                CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 275 + 25.0f * i);
                rw.Draw(CreditsText);
                i++;
            }

        }

        #endregion Methods

    }
}

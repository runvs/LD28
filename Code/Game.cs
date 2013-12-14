using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    class Game
    {

        #region Fields

        private Font _font;
        private State _gameState;

        World _myWorld;
        Score _gameStats;
        float _timeTilNextInput = 0.0f;

        #endregion Fields

        #region Methods

        public Game()
        {
            // Predefine game state to menu
            _gameState = State.Menu;
            _font = new Font("../GFX/font.ttf");
        }

        public void GetInput()
        {
            if (_timeTilNextInput < 0.0f)
            {
                if (_gameState == State.Menu)
                {
                    GetInputMenu();
                }
                else if (_gameState == State.Game)
                {
                    _myWorld.GetInput();
                }
                else if (_gameState == State.Credits || _gameState == State.Score)
                {
                    GetInputCreditsScore();
                }
            }
        }

        private void GetInputMenu()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
            {
                StartGame();
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.C))
            {
                ChangeGameState(State.Credits);
            }

        }

        private void GetInputCreditsScore()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                ChangeGameState(State.Menu, 1.0f);
            }
            else
            {
                ChangeGameState(State.Menu, 0.5f);
            }
        }

        public void Update(float deltaT)
        {
            if (_timeTilNextInput >= 0.0f)
            {
                _timeTilNextInput -= deltaT;
            }

            if (_gameState == State.Game)
            {
                _myWorld.Update(deltaT);

                // TODO Game End Criteria
            }

        }

        public void Draw(RenderWindow rw)
        {
            rw.Clear();
            if (_gameState == State.Menu)
            {
                DrawMenu(rw);
            }
            else if (_gameState == State.Game)
            {
                _myWorld.Draw(rw);
            }
            else if (_gameState == State.Credits)
            {
                DrawCredits(rw);
            }
            else if (_gameState == State.Score)
            {
                _gameStats.Draw(rw);
            }
        }

        private void DrawMenu(RenderWindow rw)
        {

            Text text = new Text();
            text.DisplayedString = "$GameTitle$";

            text.Font = _font;
            text.Scale = new Vector2f(2, 2);
            text.Position = new Vector2f(400 - text.GetGlobalBounds().Width / 2.0f, 150 - text.GetGlobalBounds().Height / 2.0f);
            rw.Draw(text);

            text = new Text();
            text.DisplayedString = "Start [Return]";
            text.Font = _font;
            text.Position = new Vector2f(400 - text.GetGlobalBounds().Width / 2.0f, 250 - text.GetGlobalBounds().Height / 2.0f);
            rw.Draw(text);

            text = new Text();
            text.Font = _font;
            text.DisplayedString = "W A S D & LShift";
            text.Color = Color.White;
            text.Scale = new Vector2f(0.75f, 0.75f);
            text.Position = new Vector2f(530 - text.GetGlobalBounds().Width / 2.0f, 340 - text.GetGlobalBounds().Height / 2.0f);
            rw.Draw(text);

            text = new Text();
            text.Font = _font;
            text.DisplayedString = "Arrows & RCtrl";
            text.Color = Color.White;
            text.Scale = new Vector2f(0.75f, 0.75f);
            text.Position = new Vector2f(180 - text.GetGlobalBounds().Width / 2.0f, 340 - text.GetGlobalBounds().Height / 2.0f);
            rw.Draw(text);


            text = new Text();
            text.DisplayedString = "[C]redits";
            text.Font = _font;
            text.Scale = new Vector2f(0.75f, 0.75f);
            text.Position = new Vector2f(30, 550 - text.GetGlobalBounds().Height / 2.0f);
            rw.Draw(text);

        }

        private void DrawCredits(RenderWindow rw)
        {


            Text CreditsText = new Text("$GameTitle$", _font);
            CreditsText.Scale = new Vector2f(1.5f, 1.5f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 20);
            rw.Draw(CreditsText);

            CreditsText = new Text("A Game by", _font);
            CreditsText.Scale = new Vector2f(.75f, 0.75f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 100);
            rw.Draw(CreditsText);

            CreditsText = new Text("$DeveloperNames$", _font);
            CreditsText.Scale = new Vector2f(1, 1);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 135);
            rw.Draw(CreditsText);

            CreditsText = new Text("Visual Studio 2012 \t C#", _font);
            CreditsText.Scale = new Vector2f(0.75f, 0.75f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 170);
            rw.Draw(CreditsText);

            CreditsText = new Text("aseprite \t SFML.NET 2.1", _font);
            CreditsText.Scale = new Vector2f(0.75f, 0.75f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 200);
            rw.Draw(CreditsText);

            CreditsText = new Text("Thanks to", _font);
            CreditsText.Scale = new Vector2f(0.75f, 0.75f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 350);
            rw.Draw(CreditsText);

            CreditsText = new Text("Families & Friends for their great support", _font);
            CreditsText.Scale = new Vector2f(0.75f, 0.75f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 375);
            rw.Draw(CreditsText);

            CreditsText = new Text("Created $Date$", _font);
            CreditsText.Scale = new Vector2f(0.75f, 0.75f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 500);
            rw.Draw(CreditsText);


        }

        private void ChangeGameState(State newState, float inputdeadTime = 0.75f)
        {
            this._gameState = newState;
            _timeTilNextInput = inputdeadTime;
        }


        private void StartGame()
        {
            _myWorld = new World();
            ChangeGameState(State.Game, 0.1f);
        }


        #endregion Methods

        #region Subclasses/Enums

        private enum State
        {
            Menu,
            Game,
            Score,
            Credits
        }

        #endregion Subclasses/Enums

    }
}

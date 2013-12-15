using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    class Game
    {

        #region Fields

        private State _gameState;

        World _myWorld;
        Score _gameScore;
        float _timeTilNextInput = 0.0f;
        Music _mainTheme;

        #endregion Fields

        #region Methods

        public Game()
        {
            // Predefine game state to menu
            _gameState = State.Menu;
            _mainTheme = new Music("../SFX/LD28_Theme.ogg");
            _mainTheme.Loop = true;
            _mainTheme.Play();
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

                if (_myWorld.IsPlayerDead())
                {
                    _gameScore = _myWorld.EndThisRound();

                    ChangeGameState(State.Score);
                }
            }
        }

        public void Draw(RenderWindow rw)
        {
            rw.Clear(GameProperties.ColorBlack);
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
                _gameScore.Draw(rw);
            }
        }

        private void DrawMenu(RenderWindow rw)
        {

            Text text = new Text();
            text.DisplayedString = "$GameTitle$";

            text.Font = GameProperties.GameFont();
            text.Scale = new Vector2f(2, 2);
            text.Position = new Vector2f(400 - text.GetGlobalBounds().Width / 2.0f, 150 - text.GetGlobalBounds().Height / 2.0f);
            rw.Draw(text);

            text = new Text();
            text.DisplayedString = "Start [Return]";
            text.Font = GameProperties.GameFont();
            text.Position = new Vector2f(400 - text.GetGlobalBounds().Width / 2.0f, 250 - text.GetGlobalBounds().Height / 2.0f);
            rw.Draw(text);

            text = new Text();
            text.Font = GameProperties.GameFont();
            text.DisplayedString = "W A S D & LShift";
            text.Color = GameProperties.ColorWhite;
            text.Scale = new Vector2f(0.75f, 0.75f);
            text.Position = new Vector2f(530 - text.GetGlobalBounds().Width / 2.0f, 340 - text.GetGlobalBounds().Height / 2.0f);
            rw.Draw(text);

            text = new Text();
            text.Font = GameProperties.GameFont();
            text.DisplayedString = "Arrows & RCtrl";
            text.Color = GameProperties.ColorWhite;
            text.Scale = new Vector2f(0.75f, 0.75f);
            text.Position = new Vector2f(180 - text.GetGlobalBounds().Width / 2.0f, 340 - text.GetGlobalBounds().Height / 2.0f);
            rw.Draw(text);


            text = new Text();
            text.DisplayedString = "[C]redits";
            text.Font = GameProperties.GameFont();
            text.Scale = new Vector2f(0.75f, 0.75f);
            text.Position = new Vector2f(30, 550 - text.GetGlobalBounds().Height / 2.0f);
            rw.Draw(text);

        }

        private void DrawCredits(RenderWindow rw)
        {


            Text CreditsText = new Text("$GameTitle$", GameProperties.GameFont());
            CreditsText.Scale = new Vector2f(1.5f, 1.5f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 20);
            rw.Draw(CreditsText);

            CreditsText = new Text("A Game by", GameProperties.GameFont());
            CreditsText.Scale = new Vector2f(.75f, 0.75f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 100);
            rw.Draw(CreditsText);

            CreditsText = new Text("$DeveloperNames$", GameProperties.GameFont());
            CreditsText.Scale = new Vector2f(1, 1);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 135);
            rw.Draw(CreditsText);

            CreditsText = new Text("Visual Studio 2012 \t C#", GameProperties.GameFont());
            CreditsText.Scale = new Vector2f(0.75f, 0.75f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 170);
            rw.Draw(CreditsText);

            CreditsText = new Text("aseprite \t SFML.NET 2.1", GameProperties.GameFont());
            CreditsText.Scale = new Vector2f(0.75f, 0.75f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 200);
            rw.Draw(CreditsText);

            CreditsText = new Text("Thanks to", GameProperties.GameFont());
            CreditsText.Scale = new Vector2f(0.75f, 0.75f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 350);
            rw.Draw(CreditsText);

            CreditsText = new Text("Families & Friends for their great support", GameProperties.GameFont());
            CreditsText.Scale = new Vector2f(0.75f, 0.75f);
            CreditsText.Position = new Vector2f(400 - (float)(CreditsText.GetGlobalBounds().Width / 2.0), 375);
            rw.Draw(CreditsText);

            CreditsText = new Text("Created $Date$", GameProperties.GameFont());
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

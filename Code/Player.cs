using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace JamTemplate
{
    class Player
    {

        #region Fields

        public int playerNumber;
        public string PlayerName { get; private set; }

        Dictionary<Keyboard.Key, Action> _actionMap;

        private Texture playerTexture;
        private Sprite playerSprite;
        private float movementTimer = 0.0f; // time between two successive movement commands
        private World myWorld;

        #endregion Fields

        #region Methods

        public Player(World world, int number)
        {
            myWorld = world;
            playerNumber = number;

            _actionMap = new Dictionary<Keyboard.Key, Action>();
            SetupActionMap();

            try
            {
                LoadGraphics();
            }
            catch (SFML.LoadingFailedException e)
            {
                System.Console.Out.WriteLine("Error loading player Graphics.");
                System.Console.Out.WriteLine(e.ToString());
            }
        }

        private void SetPlayerNumberDependendProperties()
        {
            PlayerName = "Player" + playerNumber.ToString();
        }

        public void GetInput()
        {
            if (movementTimer <= 0.0f)
            {
                MapInputToActions();
            }
        }

        public void Update(float deltaT)
        {

        }


        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            rw.Draw(this.playerSprite);
        }

        private void MoveRightAction()
        {

        }
        private void MoveLeftAction()
        {
            
        }
        private void MoveUpAction()
        {

        }
        private void MoveDownAction()
        {

        }


        private void SetupActionMap()
        {
            _actionMap.Add(Keyboard.Key.Left, MoveLeftAction);
            _actionMap.Add(Keyboard.Key.A, MoveLeftAction);

            _actionMap.Add(Keyboard.Key.Right, MoveRightAction);
            _actionMap.Add(Keyboard.Key.D, MoveRightAction);

            _actionMap.Add(Keyboard.Key.Down, MoveDownAction);
            _actionMap.Add(Keyboard.Key.S, MoveDownAction);

            _actionMap.Add(Keyboard.Key.Up, MoveUpAction);
            _actionMap.Add(Keyboard.Key.W, MoveUpAction);
        }

        private void MapInputToActions()
        {
            foreach (var kvp in _actionMap)
            {
                if (Keyboard.IsKeyPressed(kvp.Key))
                {
                    // Execute the saved callback
                    kvp.Value();
                }
            }
        }

        private void LoadGraphics()
        {
            playerTexture = new SFML.Graphics.Texture("../gfx/player.png");

            playerSprite = new Sprite(playerTexture);
            playerSprite.Scale = new Vector2f(2.0f, 2.0f);

        }

        #endregion Methods

    }
}

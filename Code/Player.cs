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
        public SFML.Window.Vector2i PlayerPosition { get; private set; }

        Dictionary<Keyboard.Key, Action> _actionMap;

        private Texture playerTexture;
        private Sprite playerSprite;
        private float movementTimer = 0.0f; // time between two successive movement commands
        private World _world;

        bool _MovingRight;
        bool _MovingLeft;
        bool _MovingDown;
        bool _MovingUp;

        #endregion Fields

        #region Methods

        public Player(World world, int number)
        {
            _world = world;
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
            ResetMovementAction();

            if (movementTimer <= 0.0f)
            {
                MapInputToActions();
            }
        }

        private void ResetMovementAction()
        {
            _MovingRight = false;
            _MovingLeft = false;
            _MovingDown = false;
            _MovingUp = false;
        }

        public void Update(float deltaT)
        {
            if (movementTimer > 0.0f)
            {
                movementTimer -= deltaT;
            }

            DoMovement();

            playerSprite.Position = new Vector2f(
                GameProperties.TileSizeInPixel * PlayerPosition.X,
                GameProperties.TileSizeInPixel * PlayerPosition.Y
            );
        }

        private void DoMovement()
        {
            SFML.Window.Vector2i newPosition = PlayerPosition;
            if (_MovingRight && !_MovingLeft)
            {
                newPosition.X++;
            }
            if (_MovingLeft && !_MovingRight)
            {
                newPosition.X--;
            }
            if (_MovingUp && !_MovingDown)
            {
                newPosition.Y--;
            }
            if (_MovingDown && !_MovingUp)
            {
                newPosition.Y++;
            }

            if (!_world.IsTileBlockd(newPosition))
            {
                PlayerPosition = newPosition;
            }
        }

        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            rw.Draw(this.playerSprite);
        }

        private void MoveRightAction()
        {
            movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds();
            _MovingRight = true;
        }
        private void MoveLeftAction()
        {
            movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds();
            _MovingLeft = true;
        }
        private void MoveUpAction()
        {
            movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds();
            _MovingUp = true;
        }
        private void MoveDownAction()
        {
            movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds();
            _MovingDown = true;
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

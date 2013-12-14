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
        public Vector2i PlayerPosition { get; private set; }

        #region Inventory

        public Item HeadItem { get; private set; }
        public Item TorsoItem { get; private set; }
        public Item HandItem { get; private set; }
        public Item FeetItem { get; private set; }

        #endregion Inventory

        Dictionary<Keyboard.Key, Action> _actionMap;

        private Texture playerTexture;
        private Sprite playerSprite;
        private float movementTimer = 0.0f; // time between two successive movement commands
        private World _world;

        bool _MovingRight;
        bool _MovingLeft;
        bool _MovingDown;
        bool _MovingUp;

        public Attributes PlayerAttributes { get; private set; }

        #endregion Fields

        #region Methods

        public Player(World world, int number)
        {
            _world = world;
            playerNumber = number;

            _actionMap = new Dictionary<Keyboard.Key, Action>();
            SetupActionMap();
            PlayerAttributes = new Attributes();

            try
            {
                LoadGraphics();
            }
            catch (SFML.LoadingFailedException e)
            {
                System.Console.Out.WriteLine("Error loading player Graphics.");
                System.Console.Out.WriteLine(e.ToString());
            }

            //PickupItem(new Item(ItemType.HAND, "sword", +1, new Vector2i(0, 0)));

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
            // position the Sprite
            playerSprite.Position = new Vector2f(GameProperties.TileSizeInPixel * PlayerPosition.X, GameProperties.TileSizeInPixel * PlayerPosition.Y);

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


        public void PickupItem(Item item)
        {
            switch (item.ItemType)
            {
                case ItemType.FEET:
                    this.FeetItem = item;
                    break;

                case ItemType.HAND:
                    this.HandItem = item;
                    break;

                case ItemType.HEAD:
                    this.HeadItem = item;
                    break;

                default:
                case ItemType.TORSO:
                    this.TorsoItem = item;
                    break;
            }
            item.PickUp();
            ReCalculateModifiers();

        }

        private void ReCalculateModifiers()
        {
            PlayerAttributes.ResetModifiers();
            PlayerAttributes.CalculateModifiersForItem(HeadItem);
            PlayerAttributes.CalculateModifiersForItem(TorsoItem);
            PlayerAttributes.CalculateModifiersForItem(FeetItem);
            PlayerAttributes.CalculateModifiersForItem(HandItem);

        }

        public void Draw(SFML.Graphics.RenderWindow rw)
        {
            rw.Draw(this.playerSprite);
        }

        private void MoveRightAction()
        {
            movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _MovingRight = true;
        }
        private void MoveLeftAction()
        {
            movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _MovingLeft = true;
        }
        private void MoveUpAction()
        {
            movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _MovingUp = true;
        }
        private void MoveDownAction()
        {
            movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _MovingDown = true;
        }

        private void PickUpItemAction()
        {
            Item newItem = _world.GetItemOnTile(this.PlayerPosition);
            if (newItem != null)
            {
                System.Console.Out.WriteLine("Picking Up Item: " + newItem.Name);
                PickupItem(newItem);
            }
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

            _actionMap.Add(Keyboard.Key.E, PickUpItemAction);

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

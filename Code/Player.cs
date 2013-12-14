using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace JamTemplate
{
    class Player : Actor
    {

        #region Fields

        public int PlayerNumber { get; set; }
        public string PlayerName { get; private set; }
        public Vector2i PlayerPosition { get; private set; }

        #region Inventory

        public Item HeadItem { get; private set; }
        public Item TorsoItem { get; private set; }
        public Item HandItem { get; private set; }
        public Item FeetItem { get; private set; }

        #endregion Inventory

        Dictionary<Keyboard.Key, Action> _actionMap;

        private Texture _playerTexture;
        private Sprite _playerSprite;

        #endregion Fields

        #region Methods

        public Player(World world, int number)
        {
            _world = world;
            PlayerNumber = number;

            _actionMap = new Dictionary<Keyboard.Key, Action>();
            SetupActionMap();
            PlayerAttributes = new Attributes();

            try
            {
                LoadGraphics();
            }
            catch (SFML.LoadingFailedException e)
            {
                Console.Out.WriteLine("Error loading player Graphics.");
                Console.Out.WriteLine(e.ToString());
            }

            //PickupItem(new Item(ItemType.HAND, "sword", +1, new Vector2i(0, 0)));

        }

        private void SetPlayerNumberDependendProperties()
        {
            PlayerName = "Player" + PlayerNumber;
        }

        public void GetInput()
        {
            ResetMovementAction();

            if (_movementTimer <= 0.0f)
            {
                MapInputToActions();
            }
        }

        private void ResetMovementAction()
        {
            _movingRight = false;
            _movingLeft = false;
            _movingDown = false;
            _movingUp = false;
        }

        public void Update(float deltaT)
        {
            if (_movementTimer > 0.0f)
            {
                _movementTimer -= deltaT;
            }

            DoMovement();
            // position the Sprite
            _playerSprite.Position = new Vector2f(GameProperties.TileSizeInPixel * PlayerPosition.X, GameProperties.TileSizeInPixel * PlayerPosition.Y);

        }

        private void DoMovement()
        {
            SFML.Window.Vector2i newPosition = PlayerPosition;
            if (_movingRight && !_movingLeft)
            {
                newPosition.X++;
            }
            if (_movingLeft && !_movingRight)
            {
                newPosition.X--;
            }
            if (_movingUp && !_movingDown)
            {
                newPosition.Y--;
            }
            if (_movingDown && !_movingUp)
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
            rw.Draw(this._playerSprite);
        }

        private void MoveRightAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingRight = true;
        }
        private void MoveLeftAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingLeft = true;
        }
        private void MoveUpAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingUp = true;
        }
        private void MoveDownAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingDown = true;
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
            _playerTexture = new SFML.Graphics.Texture("../gfx/player.png");

            _playerSprite = new Sprite(_playerTexture);
            _playerSprite.Scale = new Vector2f(2.0f, 2.0f);

        }

        #endregion Methods

    }
}

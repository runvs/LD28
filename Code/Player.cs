﻿using SFML.Graphics;
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

        #region Inventory

        public Item HeadItem { get; private set; }
        public Item TorsoItem { get; private set; }
        public Item HandItem { get; private set; }
        public Item FeetItem { get; private set; }

        #endregion Inventory

        Dictionary<Keyboard.Key, Action> _actionMap;

        #endregion Fields

        #region Methods

        public Player(World world, int number)
        {
            _world = world;
            PlayerNumber = number;

            _actionMap = new Dictionary<Keyboard.Key, Action>();
            SetupActionMap();
            ActorAttributes = new Attributes();

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
            _movingEast = false;
            _movingWest = false;
            _movingSouth = false;
            _movingNorth = false;
        }

        public void Update(float deltaT)
        {
            if (_movementTimer > 0.0f)
            {
                _movementTimer -= deltaT;
            }

            DoMovement();
            // position the Sprite


        }

        private void DoMovement()
        {
            Vector2i newPosition = ActorPosition;

            if (_movingEast && !_movingWest && !_movingNorth && !_movingSouth)
            {
                newPosition.X++;
                Direction = Direction.EAST;
            }
            else if (_movingWest && !_movingEast && !_movingNorth && !_movingSouth)
            {
                newPosition.X--;
                Direction = Direction.WEST;
            }
            else if (_movingNorth && !_movingSouth && !_movingEast && !_movingWest)
            {
                newPosition.Y--;
                Direction = Direction.NORTH;
            }
            else if (_movingSouth && !_movingNorth && !_movingEast && !_movingWest)
            {
                newPosition.Y++;
                Direction = Direction.SOUTH;
            }

            if (!_world.IsTileBlocked(newPosition))
            {
                ActorPosition = newPosition;
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
            ActorAttributes.ResetModifiers();
            ActorAttributes.CalculateModifiersForItem(HeadItem);
            ActorAttributes.CalculateModifiersForItem(TorsoItem);
            ActorAttributes.CalculateModifiersForItem(FeetItem);
            ActorAttributes.CalculateModifiersForItem(HandItem);

        }

        public void Draw(RenderWindow rw, Vector2i CameraPosition)
        {
            _actorSprite.Position = new Vector2f(
                GameProperties.TileSizeInPixel * (ActorPosition.X - CameraPosition.X),
                GameProperties.TileSizeInPixel * (ActorPosition.Y - CameraPosition.Y)
            );

            rw.Draw(this._actorSprite);
        }

        private void MoveRightAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingEast = true;
        }
        private void MoveLeftAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingWest = true;
        }
        private void MoveUpAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingNorth = true;
        }
        private void MoveDownAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingSouth = true;
        }

        private void PickUpItemAction()
        {
            Item newItem = _world.GetItemOnTile(this.ActorPosition);
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
            _actorTexture = new SFML.Graphics.Texture("../gfx/player.png");

            _actorSprite = new Sprite(_actorTexture);
            _actorSprite.Scale = new Vector2f(2.0f, 2.0f);

        }

        #endregion Methods

    }
}

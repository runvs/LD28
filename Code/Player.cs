﻿using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace JamTemplate
{
    public class Player : Actor
    {

        #region Fields

        public int PlayerNumber { get; set; }
        public string PlayerName { get; private set; }



        #region Inventory

        public Item HeadItem { get; private set; }
        public Item TorsoItem { get; private set; }
        public Item HandItem { get; private set; }
        public Item FeetItem { get; private set; }

        public int Gold { get; set; }


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
            //ResetMovementAction();

            if (_movementTimer <= 0.0f)
            {
                MapInputToActions();
            }
        }

        public override float GetMovementTimerDeadZone()
        {

            return GameProperties.PlayerMovementDeadZoneTimeInSeconds;
        }


        public void Update(float deltaT)
        {
            if (_movementTimer > 0.0f)
            {
                _movementTimer -= deltaT;
            }
            DoMovement();

            _battleTimer -= deltaT;
            if (_battleTimer <= 0.0f)
            {
                _battleTimer += GameProperties.PlayerBattleDeadZoneTimer;
                DoBattleAction();
                ResetBattleActions();
            }
        }

        protected override void DoBattleAction()
        {
            IsBlocking = false;
            if (_battleAttack && !_battleBlock && !_battleMagic)
            {
                PlayerAttack();

            }
            else if (_battleBlock && !_battleAttack && !_battleMagic)
            {
                IsBlocking = true;

            }
            else if (_battleMagic && !_battleAttack && !_battleBlock)
            {
                throw new System.NotImplementedException();
            }
        }

        private void PlayerAttack()
        {
            Vector2i attackTile = this.ActorPosition;
            if (this.Direction == JamTemplate.Direction.EAST)
            {
                attackTile.X++;
            }
            else if (this.Direction == JamTemplate.Direction.WEST)
            {
                attackTile.X--;
            }
            else if (this.Direction == JamTemplate.Direction.NORTH)
            {
                attackTile.Y--;
            }
            else if (this.Direction == JamTemplate.Direction.SOUTH)
            {
                attackTile.Y++;
            }

            Enemy actor2 = _world.GetEnemyOnTile(attackTile);

            BattleManager.DoBattleAction(this, actor2, BattleAction.Attack);



        }


        public override int GetBaseDamage()
        {
            return GameProperties.PlayerBaseDamage;
        }

        public override void Die()
        {
            IsDead = true;
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

            DrawBlockString(rw, CameraPosition);

            rw.Draw(this._actorSprite);
        }

        private void DrawBlockString(RenderWindow rw, Vector2i CameraPosition)
        {
            if (IsBlocking)
            {
                Text text = new Text("Block!", GameProperties.GameFont());
                text.Scale = new Vector2f(0.8f, 0.8f);
                text.Position = new Vector2f(
                    GameProperties.TileSizeInPixel * (ActorPosition.X - CameraPosition.X),
                    GameProperties.TileSizeInPixel * (ActorPosition.Y - CameraPosition.Y) - 25.0f
                );
                text.Color = GameProperties.ColorWhite;
                rw.Draw(text);
            }
        }

        private void PickUpItemAction()
        {
            Item newItem = _world.GetItemOnTile(this.ActorPosition);
            if (newItem != null)
            {
                System.Console.Out.WriteLine("Picking Up Item: " + newItem.Name);
                PickupItem(newItem);
                return;
            }

            NomadsHouse house = _world.GetHouseOnTile(this.ActorPosition);
            if (house != null)
            {
                house.IsActive = true;
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

            _actionMap.Add(Keyboard.Key.Space, AttackAction);
            _actionMap.Add(Keyboard.Key.LShift, MagicAction);
            _actionMap.Add(Keyboard.Key.LControl, BlockAction);


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

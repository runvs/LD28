/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net

using SFML.Graphics;
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

        public QuestLog _log;
        private float _timerLog;

        private float _HealthRegenTimer;
        private float _StaminaRegenTimer;
        public IList<String> _ownedItems;



        #region Inventory

        public Item HeadItem { get; private set; }
        public Item TorsoItem { get; private set; }
        public Item HandItem { get; private set; }
        public Item FeetItem { get; private set; }

        public int Gold { get; set; }
        public int TotalGold { get; set; }
        public int GoldSpent { get; set; }
        public int ExperienceSpent { get; set; }


        #endregion Inventory

        Dictionary<Keyboard.Key, Action> _actionMap;

        #endregion Fields

        #region Methods

        public Player(World world, int number)
        {
            _world = world;
            PlayerNumber = number;

            ActorPosition = new Vector2i(30, 30);

            _actionMap = new Dictionary<Keyboard.Key, Action>();
            SetupActionMap();
            ActorAttributes = new Attributes();
            _log = new QuestLog();
            _timerLog = 0.0f;
            _HealthRegenTimer = 0.0f;
            _StaminaRegenTimer = 0.0f;
            _ownedItems = new List<String>();

            ActorAttributes.StaminaRegenfreuency = GameProperties.PlayerBaseStaminaRegenFrequency;
            ActorAttributes.HealthRegenfreuency = GameProperties.PlayerBaseHealthRegenFrequency;
            ActorAttributes.ResetHealth(GameProperties.PlayerBaseHealth);


            try
            {
                LoadGraphics();
            }
            catch (SFML.LoadingFailedException e)
            {
                Console.Out.WriteLine("Error loading player Graphics.");
                Console.Out.WriteLine(e.ToString());
            }
        }

        public void MovePlayer(Vector2i position)
        {
            ActorPosition = position;
        }

        private void SetPlayerNumberDependendProperties()
        {
            PlayerName = "Player" + PlayerNumber;
        }

        public void GetInput()
        {
            if (_movementTimer <= 0.0f)
            {
                MapInputToActions();
            }
        }
        protected override void ReactOnDamage()
        {
            // do nothing
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

            if (_timerLog >= 0.0f)
            {
                _timerLog -= deltaT;
            }

            _HealthRegenTimer -= deltaT;
            if (_HealthRegenTimer < 0.0f && ActorAttributes.HealthRegenfreuency > 0)
            {
                _HealthRegenTimer += ActorAttributes.HealthRegenfreuency;
                ActorAttributes.AddToCurrentHealth(1);
            }

            _StaminaRegenTimer -= deltaT;
            if (_StaminaRegenTimer < 0.0f && ActorAttributes.StaminaRegenfreuency > 0)
            {
                _StaminaRegenTimer += ActorAttributes.StaminaRegenfreuency;
                ActorAttributes.AddToCurrentStamina(1);
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
                if (ActorAttributes.StaminaCurrent >= GameProperties.BlockStaminaCost)
                {
                    IsBlocking = true;
                    ActorAttributes.StaminaCurrent -= GameProperties.BlockStaminaCost;
                }

            }
            else if (_battleMagic && !_battleAttack && !_battleBlock)
            {
                PlayerMagic();
            }
        }

        private void PlayerMagic()
        {
            if (ActorAttributes.StaminaCurrent >= GameProperties.MagicStaminaCost)
            {

                ActorAttributes.StaminaCurrent -= GameProperties.AttackStaminaCost;
                Vector2i SpellStartTile = this.ActorPosition;
                if (this.Direction == JamTemplate.Direction.EAST)
                {
                    SpellStartTile.X++;
                }
                else if (this.Direction == JamTemplate.Direction.WEST)
                {
                    SpellStartTile.X--;
                }
                else if (this.Direction == JamTemplate.Direction.NORTH)
                {
                    SpellStartTile.Y--;
                }
                else if (this.Direction == JamTemplate.Direction.SOUTH)
                {
                    SpellStartTile.Y++;
                }

                if (!_world.IsTileBlocked(SpellStartTile))
                {
                    Spell newSpell = new Spell(_world, this, SpellStartTile, this.Direction);
                    _world.AddSpell(newSpell);
                }
                PlaySoundSpell();
            }


        }

        private void PlayerAttack()
        {

            if (ActorAttributes.StaminaCurrent >= GameProperties.AttackStaminaCost)
            {


                Enemy actor2 = _world.GetEnemyOnTile(ActorPosition);

                if (actor2 == null)
                {
                    Vector2i attackTile = this.ActorPosition + Actor.GetVectorFromDirection(this.Direction);
                    actor2 = _world.GetEnemyOnTile(attackTile);
                }
                if (actor2 != null)
                {
                    ActorAttributes.StaminaCurrent -= GameProperties.AttackStaminaCost;
                    BattleManager.DoBattleAction(this, actor2, BattleAction.Attack);
                }
            }


        }


        public override int GetBaseDamage()
        {
            return GameProperties.PlayerBaseDamage;
        }

        public override int GetMagicBaseDamage()
        {
            return GameProperties.PlayerMagicBaseDamage;
        }

        public override void Die()
        {
            PlaySoundDie();
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
            _ownedItems.Add(item.Name);
            item.PickUp();
            ReCalculateModifiers();
            PlaySoundPickup();

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

            _log.Draw(rw);

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

        private void ToggleQuestlog()
        {

            if (_timerLog <= 0.0f)
            {
                _log.IsActive = !_log.IsActive;
                _timerLog += 0.5f;
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
            _actionMap.Add(Keyboard.Key.L, ToggleQuestlog);


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

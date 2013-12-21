/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net

using SFML.Graphics;
using SFML.Window;
using System;
using JamUtilities;

namespace JamTemplate
{
    public class Enemy : Actor
    {
        #region Fields

        public int EnemyNumber { get; set; }
        public string PlayerName { get; private set; }

        public Item DropItem { get; private set; }
        public int DropGold { get; private set; }
        public int DropExperience { get; private set; }

        private bool _hasSeenPlayer = false;
        private EnemyStrength _strength;
        private EnemyType _type;
        private float _standstillTimer;

        #endregion Fields

        #region Methods

        public Enemy(World world, Vector2i initPosition, EnemyStrength strength, EnemyType type)
        {
            _world = world;
            ActorPosition = initPosition;

            ActorAttributes = new Attributes();
            _strength = strength;
            _type = type;

            SetStrength();

            DropItem = ItemFactory.GetRandomItem(initPosition, _strength);

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

        private void SetStrength()
        {
            if (_strength == EnemyStrength.EASY)
            {
                ActorAttributes.BaseStrength = 1;
                ActorAttributes.BaseAgility = 2 + GameProperties.RandomGenerator.Next(-1, +2);
                ActorAttributes.BaseEndurance = 1 + GameProperties.RandomGenerator.Next(0, +2);
                ActorAttributes.BaseIntelligence = 1 + GameProperties.RandomGenerator.Next(0, +2);
                DropGold = GameProperties.EnemyEasyGold;
                DropExperience = GameProperties.EnemyEasyExperience;
            }
            else if (_strength == EnemyStrength.NORMAL)
            {
                ActorAttributes.BaseStrength = 3 + GameProperties.RandomGenerator.Next(-2, +3);
                ActorAttributes.BaseAgility = 2 + GameProperties.RandomGenerator.Next(-1, +4);
                ActorAttributes.BaseEndurance = 2 + GameProperties.RandomGenerator.Next(-1, +3);
                ActorAttributes.BaseIntelligence = 2 + GameProperties.RandomGenerator.Next(-1, +3);
                DropGold = GameProperties.EnemyNormalGold;
                DropExperience = GameProperties.EnemyNormalExperience;
            }
            else if (_strength == EnemyStrength.HARD)
            {
                ActorAttributes.BaseStrength = 6 + GameProperties.RandomGenerator.Next(-1, +6);
                ActorAttributes.BaseAgility = 6 + GameProperties.RandomGenerator.Next(-1, +6);
                ActorAttributes.BaseEndurance = 6 + GameProperties.RandomGenerator.Next(-1, +6);
                ActorAttributes.BaseIntelligence = 6 + GameProperties.RandomGenerator.Next(-1, +4);
                DropGold = GameProperties.EnemyHardGold;
                DropExperience = GameProperties.EnemyHardExperience;
            }
        }

        private void LoadGraphics()
        {
            switch (_type)
            {
                case EnemyType.GOBLIN:
                    _sprite = new SmartSprite("../gfx/enemy_goblin_2.png");
                    break;
                case EnemyType.HEADLESS_GOBLIN:
                    _sprite = new SmartSprite("../gfx/enemy_goblin_1.png");
                    break;
                case EnemyType.RAT:
                    _sprite = new SmartSprite("../gfx/enemy_rat.png");
                    break;
                case EnemyType.GOBLIN_RED:
                    _sprite = new SmartSprite("../GFX/enemy_goblin_3.png");
                    break;
                default:
                case EnemyType.ENEMY:
                    _sprite = new SmartSprite("../gfx/enemy.png");
                    break;
            }
        }

        protected override void ReactOnDamage()
        {
            _hasSeenPlayer = true;
            _standstillTimer += GameProperties.EnemyStandStillTime;
        }

        public void Draw(RenderWindow rw, Vector2i CameraPosition)
        {
            _sprite.Position = new Vector2f(
                GameProperties.TileSizeInPixel * (ActorPosition.X - CameraPosition.X),
                GameProperties.TileSizeInPixel * (ActorPosition.Y - CameraPosition.Y)
            );

            DrawHealthBar(rw, CameraPosition);

            rw.Draw(this._sprite.Sprite);
        }

        private void DrawHealthBar(RenderWindow rw, Vector2i CameraPosition)
        {
            if (_hasSeenPlayer)
            {
                var outline = new RectangleShape(new Vector2f(GameProperties.TileSizeInPixel, 10));
                outline.Position = new Vector2f(
                    GameProperties.TileSizeInPixel * (ActorPosition.X - CameraPosition.X),
                    GameProperties.TileSizeInPixel * (ActorPosition.Y - CameraPosition.Y) - 25.0f
                );
                outline.FillColor = Color.Transparent;
                outline.OutlineColor = GameProperties.ColorDarkGrey;
                outline.OutlineThickness = 1;
                rw.Draw(outline);

                var fill = new RectangleShape(new Vector2f(GameProperties.TileSizeInPixel, 10));
                float percentage = (float)ActorAttributes.HealthCurrent / (float)ActorAttributes.HealthMaximum;
                fill.Position = new Vector2f(
                    GameProperties.TileSizeInPixel * (ActorPosition.X - CameraPosition.X),
                    GameProperties.TileSizeInPixel * (ActorPosition.Y - CameraPosition.Y) - 25.0f
                );
                fill.FillColor = GameProperties.ColorLightRed;
                fill.Scale = new Vector2f(percentage, 1);
                rw.Draw(fill);
            }
        }

        public override float GetMovementTimerDeadZone()
        {
            return GameProperties.EnemyMovementDeadZoneTimeInSeconds;
        }

        public void Update(float deltaT)
        {

            _sprite.Update(deltaT);
            DoAIOperations(deltaT);

            if (_movementTimer > 0.0f)
            {
                _movementTimer -= deltaT;
            }
            DoMovement();
            //System.Console.Out.WriteLine(ActorPosition.ToString());

            _battleTimer -= deltaT;
            if (_battleTimer <= 0.0f)
            {
                _battleTimer += GameProperties.EnemyBattleDeadZoneTimer;
                EnemyAttack();
                ResetBattleActions();
            }
        }

        private void DoAIOperations(float deltaT)
        {
            if (!_hasSeenPlayer)
            {
                // do a random walk
                if (_movementTimer <= 0.0f)
                {

                    int roll = RollTheDie.Roll();
                    if (roll == 1)
                    {
                        MoveRightAction();
                    }
                    if (roll == 2)
                    {
                        MoveLeftAction();
                    }
                    if (roll == 3)
                    {
                        MoveUpAction();
                    }
                    if (roll == 4)
                    {
                        MoveDownAction();
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                //Console.WriteLine(_standstillTimer);
                _standstillTimer -= deltaT;
                if (_standstillTimer <= 0.0f)
                {
                    _hasSeenPlayer = false;
                }
            }
        }

        protected override void DoBattleAction()
        {
        }

        private void EnemyAttack()
        {
            Vector2i playerPos = _world._player.ActorPosition;
            Vector2i difference = playerPos - ActorPosition;

            if (Math.Abs(difference.X) + Math.Abs(difference.Y) <= 1)
            {
                if (_hasSeenPlayer == false)
                {
                    _hasSeenPlayer = true;
                    _standstillTimer += GameProperties.EnemyStandStillTime;
                }
                BattleManager.DoBattleAction(this, _world._player, BattleAction.Attack);
            }
        }


        public override int GetBaseDamage()
        {
            return GameProperties.EnemyBaseDamage;
        }
        public override int GetMagicBaseDamage()
        {
            return GameProperties.PlayerMagicBaseDamage - 1;
        }

        public override void Die()
        {
            if (!IsDead)
            {
                Console.WriteLine("Enemie dies");

                IsDead = true;
                PlaySoundDie();

                _world._player.Gold += this.DropGold;
                _world._player.TotalGold += this.DropGold;

                _world._player.ActorAttributes.Experience += this.DropExperience;
                _world._player.ActorAttributes.TotalExperience += this.DropExperience;

                if (DropItem != null)
                {
                    DropItem.ItemPositionInTiles = ActorPosition;
                    _world.AddItem(DropItem);
                }

                // special case for the final boss
                if (_type == EnemyType.HEADLESS_GOBLIN)
                {
                    _world.StartSequence(5);    // not quite clean code but it should work
                }

                ActorPosition = new Vector2i(-500, -500);
            }
        }

        #endregion Methods
    }

    public enum EnemyStrength
    {
        EASY, NORMAL, HARD
    }

    public enum EnemyType
    {
        ENEMY, HEADLESS_GOBLIN, GOBLIN, RAT, GOBLIN_RED
    }
}

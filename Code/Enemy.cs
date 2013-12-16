using SFML.Graphics;
using SFML.Window;
using System;

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

        #endregion Fields

        #region Methods

        public Enemy(World world, Vector2i initPosition, EnemyStrength strength, EnemyType type)
        {
            _world = world;
            ActorPosition = initPosition;

            ActorAttributes = new Attributes();
            ActorAttributes.ResetHealth(8);
            _strength = strength;
            _type = type;

            if (_strength == EnemyStrength.EASY)
            {
                DropGold = GameProperties.EnemyEasyGold;
                DropExperience = GameProperties.EnemyEasyExperience;
            }
            else if (_strength == EnemyStrength.NORMAL)
            {
                DropGold = GameProperties.EnemyNormalGold;
                DropExperience = GameProperties.EnemyNormalExperience;
            }
            else if (_strength == EnemyStrength.HARD)
            {
                DropGold = GameProperties.EnemyHardGold;
                DropExperience = GameProperties.EnemyHardExperience;
            }

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

        private void LoadGraphics()
        {
            switch (_type)
            {
                case EnemyType.GOBLIN:
                    _actorTexture = new SFML.Graphics.Texture("../gfx/enemy_goblin_2.png");
                    break;
                case EnemyType.HEADLESS_GOBLIN:
                    _actorTexture = new SFML.Graphics.Texture("../gfx/enemy_goblin_1.png");
                    break;
                case EnemyType.RAT:
                    _actorTexture = new SFML.Graphics.Texture("../gfx/enemy_rat.png");
                    break;
                default:
                case EnemyType.ENEMY:
                    _actorTexture = new SFML.Graphics.Texture("../gfx/enemy.png");
                    break;
            }

            _actorSprite = new Sprite(_actorTexture);
            _actorSprite.Scale = new Vector2f(2.0f, 2.0f);
        }

        public void Draw(RenderWindow rw, Vector2i CameraPosition)
        {
            _actorSprite.Position = new Vector2f(
                GameProperties.TileSizeInPixel * (ActorPosition.X - CameraPosition.X),
                GameProperties.TileSizeInPixel * (ActorPosition.Y - CameraPosition.Y)
            );

            DrawHealthBar(rw, CameraPosition);

            rw.Draw(this._actorSprite);
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

            DoAIOperations();

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

        private void DoAIOperations()
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
                        // do nothing here
                    }
                }
            }
        }

        protected override void DoBattleAction()
        {
            if (_battleAttack && !_battleBlock && !_battleMagic)
            {

            }
        }

        private void EnemyAttack()
        {
            Vector2i playerPos = _world._player.ActorPosition;
            //System.Console.Out.WriteLine(playerPos.ToString() + "\t" + ActorPosition.ToString());
            Vector2i difference = playerPos - ActorPosition;

            if (Math.Abs(difference.X) + Math.Abs(difference.Y) <= 1)
            {
                _hasSeenPlayer = true;
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
            IsDead = true;
            PlaySoundDie();
            _world._player.Gold += this.DropGold;
            _world._player.ActorAttributes.Experience += this.DropExperience;

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

        #endregion Methods
    }

    public enum EnemyStrength
    {
        EASY, NORMAL, HARD
    }

    public enum EnemyType
    {
        ENEMY, HEADLESS_GOBLIN, GOBLIN, RAT
    }
}

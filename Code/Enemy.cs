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

        private bool _hasSeenPlayer = false;

        #endregion Fields

        #region Methods

        public Enemy(World world, Vector2i initPosition)
        {
            _world = world;
            ActorPosition = initPosition;

            ActorAttributes = new Attributes();
            ActorAttributes.ResetHealth(4);
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
            _actorTexture = new SFML.Graphics.Texture("../gfx/enemy.png");

            _actorSprite = new Sprite(_actorTexture);
            _actorSprite.Scale = new Vector2f(2.0f, 2.0f);
        }

        public void Draw(RenderWindow rw, Vector2i CameraPosition)
        {
            _actorSprite.Position = new Vector2f(
                GameProperties.TileSizeInPixel * (ActorPosition.X - CameraPosition.X),
                GameProperties.TileSizeInPixel * (ActorPosition.Y - CameraPosition.Y)
            );
            rw.Draw(this._actorSprite);
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
                        // dont move
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
            if (Math.Abs((playerPos.X - ActorPosition.X) + (playerPos.Y - ActorPosition.Y)) <= 1)
            {
                BattleManager.DoBattleAction(this, _world._player, BattleAction.Attack);
            }
        }


        public override int GetBaseDamage()
        {
            return GameProperties.EnemyBaseDamage;
        }

        public override void Die()
        {
            IsDead = true;
        }

        #endregion Methods
    }
}

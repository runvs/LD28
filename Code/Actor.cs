using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    public abstract class Actor
    {
        protected float _movementTimer = 0.0f; // time between two successive movement commands
        protected float _battleTimer = 0.0f;

        protected World _world;

        protected bool _movingEast;
        protected bool _movingWest;
        protected bool _movingSouth;
        protected bool _movingNorth;

        protected bool _battleAttack;
        protected bool _battleMagic;
        protected bool _battleBlock;
        public bool IsBlocking { get; protected set; }

        public bool IsDead { get; set; }

        public abstract void Die();

        protected Texture _actorTexture;
        protected Sprite _actorSprite;

        public Attributes ActorAttributes { get; protected set; }
        public Vector2i ActorPosition { get; protected set; }
        public Direction Direction { get; protected set; }


        protected void DoMovement()
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

        protected abstract void DoBattleAction();


        protected void MoveRightAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingEast = true;
        }
        protected void MoveLeftAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingWest = true;
        }
        protected void MoveUpAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingNorth = true;
        }
        protected void MoveDownAction()
        {
            _movementTimer += GameProperties.PlayerMovementDeadZoneTimeInSeconds;
            _movingSouth = true;
        }

        protected void AttackAction()
        {
            _battleAttack = true;
            _battleMagic = false;
            _battleBlock = false;
        }
        protected void MagicAction()
        {
            _battleAttack = false;
            _battleMagic = true;
            _battleBlock = false;
        }
        protected void BlockAction()
        {
            _battleAttack = false;
            _battleMagic = false;
            _battleBlock = true;
        }

        protected void ResetBattleActions()
        {
            _battleAttack = false;
            _battleMagic = false;
            _battleBlock = false;

        }
        public abstract int GetBaseDamage();
        public void TakeDamage(int damage)
        {
            ActorAttributes.HealthCurrent -= damage;
            CheckIfActorDead();
        }

        private void CheckIfActorDead()
        {
            System.Console.Out.WriteLine("Remaining Health " + ActorAttributes.HealthCurrent);
            if (ActorAttributes.HealthCurrent <= 0)
            {

                Die();
            }
        }
    }

    public enum Direction
    {
        NORTH, EAST, SOUTH, WEST
    }
}

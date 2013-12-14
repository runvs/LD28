
namespace JamTemplate
{
    public abstract class Actor
    {
        protected float _movementTimer = 0.0f; // time between two successive movement commands
        protected World _world;

        protected bool _movingRight;
        protected bool _movingLeft;
        protected bool _movingDown;
        protected bool _movingUp;

        public Attributes PlayerAttributes { get; protected set; }
    }
}

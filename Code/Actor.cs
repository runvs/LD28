using SFML.Graphics;
using SFML.Window;

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

        protected Texture _actorTexture;
        protected Sprite _actorSprite;

        public Attributes ActorAttributes { get; protected set; }
        public Vector2i ActorPosition { get; protected set; }
    }
}

using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    public abstract class Actor
    {
        protected float _movementTimer = 0.0f; // time between two successive movement commands
        protected World _world;

        protected bool _movingEast;
        protected bool _movingWest;
        protected bool _movingSouth;
        protected bool _movingNorth;

        protected Texture _actorTexture;
        protected Sprite _actorSprite;

        public Attributes ActorAttributes { get; protected set; }
        public Vector2i ActorPosition { get; protected set; }
        public Direction Direction { get; protected set; }
    }

    public enum Direction
    {
        NORTH, EAST, SOUTH, WEST
    }
}

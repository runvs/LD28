/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net

using System.Collections.Generic;
using JamUtilities;
using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    public abstract class Actor
    {
        protected float _movementTimer = 0.0f; // time between two successive movement commands
        protected float _battleTimer = 0.0f;

        public World _world;

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

        //protected Texture _actorTexture;
        //protected Sprite _actorSprite;
        protected JamUtilities.SmartSprite _sprite;

        private SFML.Audio.SoundBuffer soundBufferPickUp;
        private SFML.Audio.Sound soundPickUp;

        private SFML.Audio.SoundBuffer soundBufferSpell;
        private SFML.Audio.Sound soundSpell;
        private SFML.Audio.SoundBuffer soundBufferHit;
        private SFML.Audio.Sound soundHit;
        private SFML.Audio.SoundBuffer soundBufferDie;
        private SFML.Audio.Sound soundDie;

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
            ResetMovementAction();
        }

        private void ResetMovementAction()
        {
            _movingEast = false;
            _movingWest = false;
            _movingSouth = false;
            _movingNorth = false;
        }

        protected abstract void DoBattleAction();


        protected void MoveRightAction()
        {
            _movementTimer += GetMovementTimerDeadZone();
            _movingEast = true;
        }
        protected void MoveLeftAction()
        {
            _movementTimer += GetMovementTimerDeadZone();
            _movingWest = true;
        }
        protected void MoveUpAction()
        {
            _movementTimer += GetMovementTimerDeadZone();
            _movingNorth = true;
        }
        protected void MoveDownAction()
        {
            _movementTimer += GetMovementTimerDeadZone();
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
        public abstract float GetMovementTimerDeadZone();
        public abstract int GetMagicBaseDamage();

        public Actor()
        {
            LoadSounds();
        }


        public void TakeDamage(int damage)
        {
            ActorAttributes.HealthCurrent -= damage;
            PlaySoundHit();
            _sprite.Flash(Color.Red, 0.15f);

            //_sprite.BreakPixels(0.5f, ColorList.GetColorListWithConstantAlpha(125, Color.Cyan, Color.Yellow, Color.Magenta), new Vector2f(1.0f, 0.0f), 8.0f);
            _sprite.Shake(0.2f, 0.02f, 1.5f);
            CheckIfActorDead();
            ReactOnDamage();
        }
        protected abstract void ReactOnDamage();

        private void CheckIfActorDead()
        {
            //System.Console.Out.WriteLine("Remaining Health " + ActorAttributes.HealthCurrent);
            if (ActorAttributes.HealthCurrent <= 0)
            {

                Die();
            }
        }

        public static Vector2i GetVectorFromDirection(Direction dir)
        {
            Vector2i ret = new Vector2i();

            if (dir == Direction.NORTH)
            {
                ret.Y--;
            }
            else if (dir == Direction.SOUTH)
            {
                ret.Y++;
            }
            if (dir == Direction.WEST)
            {
                ret.X--;
            }
            else if (dir == Direction.EAST)
            {
                ret.X++;
            }

            return ret;
        }


        private void LoadSounds()
        {
            try
            {
                soundBufferPickUp = new SFML.Audio.SoundBuffer("../SFX/pickup.wav");
                soundPickUp = new SFML.Audio.Sound(soundBufferPickUp);
                soundPickUp.Volume = 50.0f;

                soundBufferSpell = new SFML.Audio.SoundBuffer("../sfx/spell.wav");
                soundSpell = new SFML.Audio.Sound(soundBufferSpell);
                soundSpell.Volume = 20.0f;

                soundBufferHit = new SFML.Audio.SoundBuffer("../sfx/hit.wav");
                soundHit = new SFML.Audio.Sound(soundBufferHit);
                soundHit.Volume = 20.0f;

                soundBufferDie = new SFML.Audio.SoundBuffer("../sfx/die.wav");
                soundDie = new SFML.Audio.Sound(soundBufferDie);
                soundDie.Volume = 40.0f;

            }
            catch (SFML.LoadingFailedException ex)
            {
                System.Console.Out.WriteLine("error loading the world sounds");
                System.Console.Out.WriteLine(ex.Message);
            }
        }

        protected void PlaySoundPickup()
        {
            soundPickUp.Play();
        }

        protected void PlaySoundHit()
        {
            soundHit.Play();
        }
        protected void PlaySoundSpell()
        {
            soundSpell.Play();
        }
        protected void PlaySoundDie()
        {
            soundDie.Play();
        }

    }

    public enum Direction
    {
        NORTH, EAST, SOUTH, WEST
    }
}

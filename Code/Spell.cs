using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamTemplate
{
    public class Spell
    {
        public Vector2i PositionInTiles { get; private set; }
        public Direction Direction { get; private set; }

        private Texture _spellTexture1;
        private Texture _spellTexture2;
        private int _spellFrameNumber;
        private float _spellFrameTimer;

        private Sprite _spellSprite;

        private World _world;
        private Actor _caster;

        public bool IsSpellOver { get; private set; }

        private float _movementTimer;

        public Spell(World world, Actor caster, Vector2i position, Direction dir)
        {
            _spellFrameNumber = 0;
            _spellFrameTimer = 0.0f;
            _world = world;
            _caster = caster;
            _movementTimer = GameProperties.SpellMoveMentTime;
            PositionInTiles = position;
            IsSpellOver = false;
            this.Direction = dir;

            try
            {
                LoadGraphics();
            }
            catch (SFML.LoadingFailedException e)
            {
                Console.Out.WriteLine("Error loading Spell Graphics.");
                Console.Out.WriteLine(e.ToString());
            }
        }

        private void LoadGraphics()
        {
            _spellTexture1 = new Texture("../GFX/spell_bolt.png");
            _spellSprite = new Sprite(_spellTexture1);
            _spellSprite.Scale = new Vector2f(2.0f, 2.0f);

            _spellTexture2 = new Texture("../GFX/spell_bolt2.png");
        }

        private void SwitchSpellFrames()
        {
            if (_spellFrameNumber == 0)
            {
                _spellFrameNumber = 1;
            }
            else
            {
                _spellFrameNumber = 0;
            }
        }



        public void Draw(RenderWindow rw, Vector2i CameraPosition)
        {
            if (_spellFrameNumber == 0)
            {
                _spellSprite = new Sprite(_spellTexture1);
                _spellSprite.Scale = new Vector2f(2.0f, 2.0f);
            }
            else if (_spellFrameNumber == 1)
            {
                _spellSprite = new Sprite(_spellTexture2);
                _spellSprite.Scale = new Vector2f(2.0f, 2.0f);
            }

            _spellSprite.Position = new Vector2f(
               GameProperties.TileSizeInPixel * (PositionInTiles.X - CameraPosition.X),
               GameProperties.TileSizeInPixel * (PositionInTiles.Y - CameraPosition.Y)
           );
            rw.Draw(_spellSprite);
        }

        public void Update(float deltaT)
        {
            _movementTimer -= deltaT;
            if (_movementTimer <= 0.0f)
            {
                _movementTimer += GameProperties.SpellMoveMentTime;
                ProceedSpellPosition();
            }
            _spellFrameTimer -= deltaT;
            if (_spellFrameTimer <= 0.0f)
            {
                _spellFrameTimer += GameProperties.SpellFrameTime;
                SwitchSpellFrames();
            }
        }

        private void ProceedSpellPosition()
        {
            PositionInTiles += Actor.GetVectorFromDirection(this.Direction);

            Enemy myNemesis = _world.GetEnemyOnTile(PositionInTiles);
            if (myNemesis != null)
            {
                BattleManager.DoBattleAction(_caster, myNemesis, BattleAction.Magic);
                EndSpell();
                return;
            }

            if (_world.IsTileBlocked(PositionInTiles))
            {
                EndSpell();
            }
        }

        public void EndSpell()
        {
            IsSpellOver = true;
            PositionInTiles = new Vector2i(-500, -500);
        }


    }
}

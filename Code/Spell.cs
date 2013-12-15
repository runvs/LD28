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

        private Texture _spellTexture;
        private Sprite _spellSprite;

        private World _world;
        private Actor _caster;

        public bool IsSpellOver { get; private set; }

        private float _movementTimer;

        public Spell(World world, Actor caster, Vector2i position, Direction dir)
        {
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
            _spellTexture = new Texture("../GFX/spell_bolt.png");
            _spellSprite = new Sprite(_spellTexture);
            _spellSprite.Scale = new Vector2f(2.0f, 2.0f);
        }

        public void Draw(RenderWindow rw, Vector2i CameraPosition)
        {
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

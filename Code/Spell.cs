/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net

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

        private float _speedInPixelsPerSecond;

        private World _world;
        private Actor _caster;

        public bool IsSpellOver { get; private set; }

        private float _movementTimer;

        public Spell(World world, Actor caster, Vector2i position, Direction dir)
        {

            _speedInPixelsPerSecond = (float)GameProperties.TileSizeInPixel / GameProperties.SpellMoveMentTime;

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



        public void Draw(RenderWindow rw, Vector2f CameraPosition)
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


            Vector2f subTilePosition = - new Vector2f(  GameProperties.TileSizeInPixel * Actor.GetVectorFromDirection(Direction).X, 
                                                        GameProperties.TileSizeInPixel * Actor.GetVectorFromDirection(Direction).Y) * _movementTimer; // kinda buggy, but it looks cool

            _spellSprite.Origin = new Vector2f(0.0f, 0.0f);
            if (Direction == JamTemplate.Direction.SOUTH)
            {
                _spellSprite.Rotation = 90.0f;
                _spellSprite.Origin = new SFML.Window.Vector2f(0.0f, +_spellSprite.GetLocalBounds().Width);// rotate also rotates the coordinate system. Think about this crazy shit
            }
            else if (Direction == JamTemplate.Direction.NORTH)
            {
                _spellSprite.Rotation = -90.0f;
                _spellSprite.Origin = new SFML.Window.Vector2f(+_spellSprite.GetLocalBounds().Width, 0);// rotate also rotates the coordinate system. Think about this crazy shit
            }
            else if (Direction == JamTemplate.Direction.WEST)
            {
                _spellSprite.Rotation = 180.0f;
                _spellSprite.Origin = new SFML.Window.Vector2f(+_spellSprite.GetLocalBounds().Width, _spellSprite.GetLocalBounds().Width);// rotate also rotates the coordinate system. Think about this crazy shit
            }


            _spellSprite.Position = new Vector2f(
               GameProperties.TileSizeInPixel * PositionInTiles.X - CameraPosition.X + subTilePosition.X,
               GameProperties.TileSizeInPixel * PositionInTiles.Y - CameraPosition.Y + subTilePosition.Y
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

            Enemy myNemesis = _world.GetEnemyOnTile(PositionInTiles);
            if (myNemesis != null)
            {
                BattleManager.DoBattleAction(_caster, myNemesis, BattleAction.Magic);
                EndSpell();
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

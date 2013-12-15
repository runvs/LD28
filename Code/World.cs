using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace JamTemplate
{
    public class World
    {

        #region Fields

        Random _randomGenerator = new Random();

        public Player _player;
        Sidebar _sidebar;
        IList<Tile> _tileList;
        IList<Item> _itemList;  // free items in the World
        IList<Enemy> _enemyList; // currently active enemies
        IList<NomadsHouse> _houseList;
        IList<Spell> _spellList;

        private float _showIntroTimer;
        Texture _introTexture;
        Sprite _introSprite;

        Texture _itemTooltipTexture;
        Sprite _itemTooltipSprite;
        private bool _displayItemToolTip;
        private Item _tooltipItem;


        public SFML.Window.Vector2i CameraPosition { get; private set; }

        #endregion Fields


        #region Methods

        public World()
        {
            _displayItemToolTip = false;
            InitGame();
            LoadGraphics();
        }

        private void LoadGraphics()
        {
            _introTexture = new Texture("../GFX/sequence_intro.png");
            _introSprite = new Sprite(_introTexture);
            _introSprite.Scale = new Vector2f(2.0f, 2.0f);
            _introSprite.Position = new Vector2f(0.0f, 0.0f);


            _itemTooltipTexture = new Texture("../GFX/overlay_log.png");
            _itemTooltipSprite = new Sprite(_itemTooltipTexture);
            _itemTooltipSprite.Scale = new Vector2f(3.0f, 1.0f);
            _itemTooltipSprite.Position = new Vector2f(300.0f, 500.0f);

        }

        public void AddItem(Item it)
        {
            if (it != null)
            {
                _itemList.Add(it);
            }
        }

        public void AddSpell(Spell spell)
        {
            if (spell != null)
            {
                _spellList.Add(spell);
            }
        }

        public void GetInput()
        {
            if (!IsInSequence())
            {
                _player.GetInput();

                foreach (var h in _houseList)
                {
                    h.GetInput();
                }
            }
            else
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                {
                    _showIntroTimer = -1.0f;
                }
            }
        }

        public void Update(float deltaT)
        {
            if (_showIntroTimer >= 0.0f)
                _showIntroTimer -= deltaT;


            if (!IsInSequence())
            {
                foreach (var e in _enemyList)
                {
                    if (!e.IsDead)
                    {
                        e.Update(deltaT);
                    }
                }

                _player.Update(deltaT);

                foreach (var h in _houseList)
                {
                    if (h.IsActive)
                    {
                        h.Update(deltaT);

                        Vector2i housePos = h.PositionInTiles;
                        Vector2i playerPos = _player.ActorPosition;
                        Vector2i difference = housePos - playerPos;

                        if (Math.Abs(difference.X) + Math.Abs(difference.Y) >= 1)
                        {
                            h.IsActive = false;
                        }
                    }
                }
                _tooltipItem = null;
                foreach (var i in _itemList)
                {
                    if (_player.ActorPosition.Equals(i.ItemPositionInTiles))
                    {
                        _displayItemToolTip = true;
                        _tooltipItem = i;
                        break;
                    }

                }

                foreach (var s in _spellList)
                {
                    if (!s.IsSpellOver)
                    {
                        s.Update(deltaT);
                    }
                }


                DoCameraMovement();
            }

        }

        private void DoCameraMovement()
        {
            Vector2i newCamPos = new Vector2i(_player.ActorPosition.X - 6, _player.ActorPosition.Y - 6);
            if (newCamPos.X <= 0)
            {
                newCamPos.X = 0;
            }
            if (newCamPos.Y <= 0)
            {
                newCamPos.Y = 0;
            }

            if (newCamPos.X >= GameProperties.WorldSizeInTiles - 12)
            {
                newCamPos.X = GameProperties.WorldSizeInTiles - 12;
            }

            if (newCamPos.Y >= GameProperties.WorldSizeInTiles - 12)
            {
                newCamPos.Y = GameProperties.WorldSizeInTiles - 12;
            }
            CameraPosition = newCamPos;
        }

        public void Draw(RenderWindow rw)
        {


            if (!IsInSequence())
            {
                foreach (var t in _tileList)
                {
                    t.Draw(rw, CameraPosition);
                }
                foreach (var i in _itemList)
                {
                    i.Draw(rw, CameraPosition);
                }

                foreach (var e in _enemyList)
                {
                    if (!e.IsDead)
                    {
                        e.Draw(rw, CameraPosition);
                    }
                }

                _player.Draw(rw, CameraPosition);

                foreach (var h in _houseList)
                {
                    h.Draw(rw, CameraPosition);
                }

                foreach (var s in _spellList)
                {
                    if (!s.IsSpellOver)
                    {
                        s.Draw(rw, CameraPosition);
                    }
                }

                DrawItemToolTip(rw);
                _sidebar.Draw(rw);
            }
            else
            {
                if (_showIntroTimer >= 0.0f)
                {
                    rw.Draw(_introSprite);
                }
            }
        }

        private void DrawItemToolTip(RenderWindow rw)
        {
            if (_displayItemToolTip && _tooltipItem != null)
            {
                rw.Draw(_itemTooltipSprite);

                Text _tooltipText = new Text(_tooltipItem.Name, GameProperties.GameFont());
                _tooltipText.Position = new Vector2f(340, 510);
                _tooltipText.Scale = new Vector2f(0.5f, 0.55f);
                rw.Draw(_tooltipText);

                int i = 0;
                foreach (var kvp in _tooltipItem.Modifiers)
                {
                    _tooltipText = new Text(Attributes.GetAttributeNameFromEnum(kvp.Key) + " " + kvp.Value.ToString(), GameProperties.GameFont());
                    _tooltipText.Position = new Vector2f(340, 530 + i * 15);
                    _tooltipText.Scale = new Vector2f(0.6f, 0.6f);
                    rw.Draw(_tooltipText);
                    i++;


                }
            }

        }

        private void InitGame()
        {
            _player = new Player(this, 0);
            _sidebar = new Sidebar(_player);
            _itemList = new List<Item>();
            _enemyList = new List<Enemy>();
            _houseList = new List<NomadsHouse>();
            _spellList = new List<Spell>();

            _showIntroTimer = GameProperties.IntroDisplayTime;

            CreateWorld();
        }

        private void CreateWorld()
        {
            var parser = new MapParser("map.tmx", this);

            _tileList = parser.TerrainLayer;
            CameraPosition = new Vector2i(0, 0);

            _itemList.Add(ItemFactory.GetHeadItem(new Vector2i(2, 4)));
            _itemList.Add(ItemFactory.GetTorsoItem(new Vector2i(3, 4)));
            _itemList.Add(ItemFactory.GetFeetItem(new Vector2i(4, 4)));
            _itemList.Add(ItemFactory.GetHandItem(new Vector2i(5, 4)));

            Enemy enemy = new Enemy(this, new Vector2i(4, 4));
            _enemyList.Add(enemy);

            foreach (var item in parser.ObjectLayer)
            {
                if (item is NomadsHouse)
                {
                    _houseList.Add(item as NomadsHouse);
                }
            }

            //_houseList = parser.ObjectLayer;

        }

        internal bool IsTileBlocked(SFML.Window.Vector2i testPosition)
        {
            bool ret = true;
            foreach (var t in _tileList)
            {
                if (t.TilePosition.Equals(testPosition))
                {
                    ret = t.IsTileBlockd;
                    break;
                }
            }

            return ret;
        }

        internal Item GetItemOnTile(SFML.Window.Vector2i vector2i)
        {
            Item newItem = null;
            foreach (var i in _itemList)
            {
                if (i.ItemPositionInTiles.Equals(vector2i) && i.Picked == false)
                {
                    newItem = i;
                    break;
                }
            }
            return newItem;
        }

        internal Enemy GetEnemyOnTile(SFML.Window.Vector2i vector2i)
        {
            Enemy newEnemy = null;
            foreach (var e in _enemyList)
            {
                if (e.ActorPosition.Equals(vector2i))
                {
                    newEnemy = e;
                    break;
                }
            }
            return newEnemy;
        }

        internal NomadsHouse GetHouseOnTile(SFML.Window.Vector2i vector2i)
        {
            NomadsHouse newHouse = null;
            foreach (var h in _houseList)
            {
                if (h.PositionInTiles.Equals(vector2i))
                {
                    newHouse = h;
                    break;
                }
            }
            return newHouse;
        }


        public bool IsPlayerDead()
        {
            return _player.IsDead;
        }

        public Score EndThisRound()
        {
            return new Score();

        }

        private bool IsInSequence()
        {
            bool ret = false;
            if (_showIntroTimer >= 0)
            {
                ret = true;
            }

            return ret;

        }

        #endregion Methods

    }
}

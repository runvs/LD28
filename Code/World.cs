using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace JamTemplate
{
    class World
    {

        #region Fields

        Random _randomGenerator = new Random();

        Player _player;
        Sidebar _sidebar;
        IList<Tile> _tileList;

        #endregion Fields

        #region Methods

        public World()
        {
            InitGame();
        }

        public void GetInput()
        {
            _player.GetInput();
        }

        public void Update(float deltaT)
        {
            _player.Update(deltaT);
        }

        public void Draw(RenderWindow rw)
        {
            foreach (var t in _tileList)
            {
                t.Draw(rw);
            }
            _player.Draw(rw);
            _sidebar.Draw(rw);
        }

        private void InitGame()
        {
            _player = new Player(this, 0);
            _sidebar = new Sidebar();
            CreateWorld();
        }

        private void CreateWorld()
        {
            _tileList = new List<Tile>();

            for (int i =0; i != GameProperties.WorldSizeInTiles;i++)
            {
                for (int j =0; j != GameProperties.WorldSizeInTiles;j++)
                {
                    Tile newtile;
                    if (_randomGenerator.NextDouble() >= 0.75)
                    {
                        newtile = new Tile(i, j, Tile.TileType.Mountain);
                    }
                    else
                    {
                        newtile = new Tile(i, j, Tile.TileType.Grass);
                    }
                    _tileList.Add(newtile);
                }
            }
        }

        internal bool IsTileBlockd(SFML.Window.Vector2i testPosition)
        {
            bool ret=true;
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

        #endregion Methods



    }
}

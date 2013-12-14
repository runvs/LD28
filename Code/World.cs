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

            for (int i =0; i != GameProperties.WorldSizeInTiles();i++)
            {
                for (int j =0; j != GameProperties.WorldSizeInTiles();j++)
                {
                    Tile newtile = new Tile(i,j);
                    _tileList.Add(newtile);
                }
            }

            
        }

        #endregion Methods

    }
}

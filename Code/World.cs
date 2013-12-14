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

        Player _player;
        Sidebar _sidebar;
        IList<Tile> _tileList;
        IList<Item> _itemList;  // free items in the World
        IList<Enemy> _enemyList; // currently active enemies

        public SFML.Window.Vector2i CameraPosition { get; private set; }

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
            foreach (var e in _enemyList)
            {
                e.Update(deltaT);
            }

            _player.Update(deltaT);

            Vector2i newCamPos = new Vector2i(_player.ActorPosition.X - 7, _player.ActorPosition.Y - 7);
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
            foreach (var t in _tileList)
            {
                t.Draw(rw, CameraPosition);
            }
            foreach (var i in _itemList)
            {
                i.Draw(rw, CameraPosition);
            }

            _player.Draw(rw, CameraPosition);
            foreach (var e in _enemyList)
            {
                e.Draw(rw, CameraPosition);
            }

            _sidebar.Draw(rw);
        }

        private void InitGame()
        {
            _player = new Player(this, 0);
            _sidebar = new Sidebar(_player);
            _itemList = new List<Item>();
            _enemyList = new List<Enemy>();

            CreateWorld();
        }

        private void CreateWorld()
        {
            _tileList = new List<Tile>();
            CameraPosition = new Vector2i(0, 0);

            for (int i = 0; i != GameProperties.WorldSizeInTiles; i++)
            {
                for (int j = 0; j != GameProperties.WorldSizeInTiles; j++)
                {
                    Tile newtile;
                    if (_randomGenerator.NextDouble() >= 0.75)
                    {
                        if (_randomGenerator.NextDouble() >= 0.5)
                        {

                            newtile = new Tile(i, j, Tile.TileType.Water);
                        }
                        else
                        {
                            newtile = new Tile(i, j, Tile.TileType.Mountain);
                        }
                    }
                    else
                    {
                        newtile = new Tile(i, j, Tile.TileType.Grass);
                    }
                    _tileList.Add(newtile);
                }
            }

            Item newItem = new Item(ItemType.HAND, "sword", +1, new Vector2i(2, 4));
            _itemList.Add(newItem);

            Enemy enemy = new Enemy(this, new Vector2i(4, 4));
            _enemyList.Add(enemy);
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

        #endregion Methods

    }
}

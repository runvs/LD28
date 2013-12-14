using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamTemplate
{
    class Tile
    {
        #region Fields
        
        
        SFML.Graphics.Texture TileTexture;
        
        Sprite TileSprite;

        public SFML.Window.Vector2i TilePosition { get; private set; }

        #endregion Fields

        #region Methods

        public Tile(int posX, int posY)
        {
            TilePosition = new SFML.Window.Vector2i(posX, posY);
            LoadGraphics();
        }

        public void Draw(RenderWindow rw)
        {
            rw.Draw  (TileSprite);
        }

        public void LoadGraphics()
        {
            TileTexture = new Texture("../GFX/tile_grass.png");
            TileSprite = new Sprite(TileTexture);
            TileSprite.Scale = new SFML.Window.Vector2f(2.0f, 2.0f);
            TileSprite.Position = new SFML.Window.Vector2f(GameProperties.TileSizeInPixel() * TilePosition.X, GameProperties.TileSizeInPixel() * TilePosition.Y);
        }


        #endregion Methods

        
    }
}

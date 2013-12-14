using SFML.Graphics;

namespace JamTemplate
{
    class Tile
    {

        #region Enums
        public enum TileType
        {
            Grass,
            Water,
            Mountain
        }
        #endregion Enums

        #region Fields


        SFML.Graphics.Texture TileTexture;

        Sprite TileSprite;

        public SFML.Window.Vector2i TilePosition { get; private set; }
        public bool IsTileBlockd { get; private set; }
        private TileType _type;

        #endregion Fields

        #region Methods

        public Tile(int posX, int posY, TileType tt)
        {
            _type = tt;
            TilePosition = new SFML.Window.Vector2i(posX, posY);
            LoadGraphics();
            IsTileBlockd = false;

            if (_type == TileType.Mountain || _type == TileType.Water)
            {
                IsTileBlockd = true;
            }

        }

        public void Draw(RenderWindow rw, SFML.Window.Vector2i CameraPosition)
        {
            TileSprite.Position = new SFML.Window.Vector2f(
               GameProperties.TileSizeInPixel * (TilePosition.X - CameraPosition.X),
               GameProperties.TileSizeInPixel * (TilePosition.Y - CameraPosition.Y)
           );
            rw.Draw(TileSprite);
        }

        public void LoadGraphics()
        {
            if (_type == TileType.Grass)
            {
                if (GameProperties._randomGenerator.NextDouble() > 0.5)
                {
                    TileTexture = new Texture("../GFX/tile_grass.png");
                }
                else
                {
                    TileTexture = new Texture("../GFX/tile_grass2.png");
                }
            }
            else if (_type == TileType.Mountain)
            {
                TileTexture = new Texture("../GFX/tile_mountains.png");
            }
            if (_type == TileType.Water)
            {
                TileTexture = new Texture("../GFX/tile_water.png");
            }
            TileSprite = new Sprite(TileTexture);
            TileSprite.Scale = new SFML.Window.Vector2f(2.0f, 2.0f);

        }


        #endregion Methods

    }
}

using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamTemplate
{
    public class NomadsHouse
    {

        #region Fields

        public Vector2i PositionInTiles { get; private set; }
        private Texture _houseTexture;
        private Sprite _houseSprite;

        public bool IsActive { get; set; }

        #endregion Fields

        #region Methods

        public NomadsHouse(int p1, int p2)
        {
            PositionInTiles = new Vector2i(p1, p2);
            IsActive = false;

            try
            {
                LoadGraphics();
            }
            catch (SFML.LoadingFailedException e)
            {
                Console.Out.WriteLine("Error loading house Graphics.");
                Console.Out.WriteLine(e.ToString());
            }
        }


        public void Draw(RenderWindow rw, Vector2i CameraPosition)
        {
            _houseSprite.Position = new Vector2f(
                GameProperties.TileSizeInPixel * (PositionInTiles.X - CameraPosition.X),
                GameProperties.TileSizeInPixel * (PositionInTiles.Y - CameraPosition.Y)
            );

            rw.Draw(_houseSprite);

            if (IsActive)
            {


            }
        }

        public void GetInput()
        {
            if (IsActive)
            {


            }
        }

        private void LoadGraphics()
        {
            _houseTexture = new Texture("../GFX/house.png");
            _houseSprite = new Sprite(_houseTexture);
            _houseSprite.Scale = new Vector2f(2.0f, 2.0f);
        }

        #endregion Methods

    }
}

using SFML.Graphics;

namespace JamTemplate
{
    class Score
    {

        #region Fields

        private Font font;

        #endregion Fields

        #region Methods

        public Score()
        {
            font = new Font("../GFX/font.ttf");
        }

        public void Draw(RenderWindow rw)
        {
            Text text = new Text();
        }

        #endregion Methods

    }
}

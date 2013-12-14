using SFML.Graphics;
using System;

namespace JamTemplate
{
    class World
    {

        #region Fields

        Random _randomGenerator = new Random();

        Player _player;

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
            _player.Draw(rw);
        }

        private void InitGame()
        {
            _player = new Player(this, 0);
        }

        #endregion Methods

    }
}

using SFML.Graphics;
using SFML.Window;
using System;

namespace JamTemplate
{
    class Program
    {
        #region Event handlers

        static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            SFML.Graphics.RenderWindow window = (SFML.Graphics.RenderWindow)sender;
            window.Close();
        }

        static void OnKeyPress(object sender, SFML.Window.KeyEventArgs e)
        {
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                SFML.Graphics.RenderWindow window = (SFML.Graphics.RenderWindow)sender;
                window.Close();
            }
        }

        #endregion Event handlers

        static void Main(string[] args)
        {
            var applicationWindow = new RenderWindow(new VideoMode(800, 600, 32), "$WindowTitle$");

            applicationWindow.SetFramerateLimit(60);

            // fiddle with resizing the images later on
            applicationWindow.Closed += new EventHandler(OnClose);
            applicationWindow.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPress);

            Game myGame = new Game();



            int startTime = Environment.TickCount;
            int endTime = startTime;
            float time = 16.7f; // 60 fps -> 16.7 ms per frame

            while (applicationWindow.IsOpen())
            {
                if (startTime != endTime)
                {
                    time = (float)(endTime - startTime) / 1000.0f;
                }
                startTime = Environment.TickCount;

                applicationWindow.DispatchEvents();

                myGame.GetInput();

                myGame.Update(time);

                myGame.Draw(applicationWindow);

                applicationWindow.Display();
                endTime = Environment.TickCount;
            }
        }
    }
}

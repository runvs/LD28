﻿/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net

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
                //SFML.Graphics.RenderWindow window = (SFML.Graphics.RenderWindow)sender;
                //window.Close();
            }
        }

        #endregion Event handlers

        static void Main(string[] args)
        {
            var applicationWindow = new RenderWindow(new VideoMode(800, 600, 32), "SchrotBeton");

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

                if (myGame._gameState == JamTemplate.Game.State.Menu)
                {
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) && myGame._timeTilNextInput <= 0.0f)
                    {
                        applicationWindow.Close();
                        break;
                    }
                }

                myGame.Update(time);

                myGame.Draw(applicationWindow);

                //Console.WriteLine(time * 1000.0f);
                applicationWindow.Display();
                endTime = Environment.TickCount;
            }
        }
    }
}

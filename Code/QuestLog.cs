﻿using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamTemplate
{
    public class QuestLog
    {


        Texture _logBackgroundTexture;
        Sprite _logBackgroundSprite;

        public bool IsActive { get; set; }

        IList<String> _logMessages;

        public QuestLog()
        {
            _logMessages = new List<String>();

            _logMessages.Add("I need to find the city");
            try
            {
                LoadGraphics();
            }
            catch (SFML.LoadingFailedException e)
            {
                Console.Out.WriteLine("Error loading Log Graphics.");
                Console.Out.WriteLine(e.ToString());
            }
        }


        public void Draw(RenderWindow rw)
        {
            if (IsActive)
            {
                rw.Draw(_logBackgroundSprite);

                DrawCurrentQuestString(rw);
            }
        }

        private void DrawCurrentQuestString(RenderWindow rw)
        {
            DrawText(_logMessages.ElementAt(0), new Vector2f(250, 220), GameProperties.ColorWhite, rw);
        }

        private void DrawText(string s, Vector2f position, Color color, RenderWindow window)
        {
            if (s.Length >= 17)
            {
                String s1 = s.Substring(0, 9);
                String s2 = s.Substring(10);
                DrawText(s1, position, color, window);
                position.Y += 20;
                DrawText(s2, position, color, window);
                return;
            }

            Text text = new Text(s, GameProperties.GameFont());
            text.Position = position;
            text.Scale = new Vector2f(0.7f, 0.7f);
            text.Color = color;
            window.Draw(text);

        }


        private void LoadGraphics()
        {
            _logBackgroundTexture = new Texture("../GFX/overlay_log.png");
            _logBackgroundSprite = new Sprite(_logBackgroundTexture);
            _logBackgroundSprite.Scale = new Vector2f(3.0f, 2.0f);  // do not scale this
            _logBackgroundSprite.Position = new Vector2f(200.0f, 200.0f);
        }
    }
}

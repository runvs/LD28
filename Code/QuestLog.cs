/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net

using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamUtilities;

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
            _text = new Text("", GameProperties.GameFont());
            _logMessages = new List<String>();
            SmartText._lineLengthInChars = 18;
            SmartText._lineSpread = 1.2f;
            SmartText._font = GameProperties.GameFont();

            CreateQuestlogMessages();
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

        private void CreateQuestlogMessages()
        {
            _logMessages.Add("Move out to seek the goblin tribe. Move North to find them!");
            //_logMessages.Add("You took revenge, but the headless goblin, their Leader, was not there.");
            _logMessages.Add("Search the Blacksmith to ask him to forge an adamantium sword.");
            _logMessages.Add("The blacksmith is not there. Look for him in the West.");
            _logMessages.Add("You found the Blacksmith but still need the adamantium Ore.");
            _logMessages.Add("The blacksmith forged your adamantium Sword. Kill the headless goblin. (Go South!)");
        }

        public void CompleteCurrentQuest()
        {
            if (_logMessages.Count > 0)
            {
                _logMessages.RemoveAt(0);
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
            if (_logMessages.Count > 0)
            {
                SmartText.DrawTextWithLineBreaks(_logMessages.ElementAt(0), TextAlignment.LEFT, new Vector2f(250, 220), new Vector2f(0.7f, 0.7f), GameProperties.ColorWhite, rw);
            }
        }

        Text _text;

        //private void DrawText(string s, Vector2f position, Color color, RenderWindow window)
        //{
        //    if (s.Length >= 18)
        //    {
        //        int spacePos = s.IndexOf(" ", 9, s.Length - 9);
        //        if (spacePos == -1)
        //        {
        //            spacePos = 17;
        //        }
        //        DrawText(s.Substring(0, spacePos).TrimEnd(), position, color, window);
        //        position.Y += 20;
        //        DrawText(s.Substring(spacePos).TrimStart(), position, color, window);
        //        return;
        //    }

        //    _text.DisplayedString = s;
        //    _text.Position = position;
        //    _text.Scale = new Vector2f(0.7f, 0.7f);
        //    _text.Color = color;
        //    window.Draw(_text);

        //}


        private void LoadGraphics()
        {
            _logBackgroundTexture = new Texture("../GFX/overlay_log.png");
            _logBackgroundSprite = new Sprite(_logBackgroundTexture);
            _logBackgroundSprite.Scale = new Vector2f(3.0f, 2.0f);  // do not scale this
            _logBackgroundSprite.Position = new Vector2f(200.0f, 200.0f);
        }
    }
}

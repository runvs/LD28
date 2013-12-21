/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net

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

        public Player _player;
        Sidebar _sidebar;
        IList<Tile> _tileList;
        IList<Item> _itemList;  // free items in the World
        IList<Enemy> _enemyList; // currently active enemies
        IList<NomadsHouse> _houseList;
        IList<Spell> _spellList;
        public List<QuestItem> _questItemList;

        private float _showIntroTimer;
        private float _showSequence1Timer;
        private float _showSequence2Timer;
        private float _showSequence3Timer;
        private float _showSequence4Timer;
        private float _showSequence5Timer;
        private float _showSequence6Timer;

        Texture _introTexture;
        Sprite _introSprite;

        Text text;



        Texture _itemTooltipTexture;
        Sprite _itemTooltipSprite;
        private bool _displayItemToolTip;
        private Item _tooltipItem;


        public SFML.Window.Vector2i CameraPosition { get; private set; }

        #endregion Fields


        #region Methods

        public World()
        {
            _displayItemToolTip = false;
            text = new Text("", GameProperties.GameFont());
            InitGame();
            LoadGraphics();
        }

        private void LoadGraphics()
        {
            _introTexture = new Texture("../GFX/sequence_intro.png");
            _introSprite = new Sprite(_introTexture);
            _introSprite.Scale = new Vector2f(2.0f, 2.0f);
            _introSprite.Position = new Vector2f(0.0f, 0.0f);


            _itemTooltipTexture = new Texture("../GFX/overlay_log.png");
            _itemTooltipSprite = new Sprite(_itemTooltipTexture);
            _itemTooltipSprite.Scale = new Vector2f(3.0f, 1.0f);
            _itemTooltipSprite.Position = new Vector2f(300.0f, 500.0f);

        }

        public void AddItem(Item it)
        {
            if (it != null)
            {
                _itemList.Add(it);
            }
        }

        public void AddSpell(Spell spell)
        {
            if (spell != null)
            {
                _spellList.Add(spell);
            }
        }

        public void GetInput()
        {
            if (!IsInSequence())
            {
                _player.GetInput();

                foreach (var h in _houseList)
                {
                    if (h.IsActive)
                    {
                        h.GetInput();
                    }
                }
            }
            else
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                {
                    _showIntroTimer = -1.0f;
                    _showSequence1Timer = -1.0f;
                    _showSequence2Timer = -1.0f;
                    _showSequence3Timer = -1.0f;
                    _showSequence4Timer = -1.0f;
                    _showSequence5Timer = -1.0f;

                    if (_showSequence6Timer >= 0.0f)
                    {
                        _player.Die();
                    }
                }
            }
        }

        public void Update(float deltaT)
        {
            UpdateSequenceTimers(deltaT);


            if (!IsInSequence())
            {
                foreach (var e in _enemyList)
                {
                    if (!e.IsDead)
                    {
                        e.Update(deltaT);
                    }
                }

                _player.Update(deltaT);


                foreach (var h in _houseList)
                {
                    if (h.IsActive)
                    {
                        h.Update(deltaT);

                        Vector2i housePos = h.PositionInTiles;
                        Vector2i playerPos = _player.ActorPosition;
                        Vector2i difference = housePos - playerPos;

                        if (Math.Abs(difference.X) + Math.Abs(difference.Y) >= 1)
                        {
                            h.IsActive = false;
                        }
                    }
                }
                _tooltipItem = null;
                foreach (var i in _itemList)
                {
                    if (!i.Picked)
                    {
                        if (_player.ActorPosition.Equals(i.ItemPositionInTiles))
                        {
                            _displayItemToolTip = true;
                            _tooltipItem = i;
                            break;
                        }
                    }

                }

                var questItemsToAdd = new List<QuestItem>();
                foreach (var i in _questItemList)
                {
                    if (!i.Picked)
                    {
                        i.Update(deltaT, questItemsToAdd);
                    }
                }
                _questItemList.AddRange(questItemsToAdd);

                foreach (var s in _spellList)
                {
                    if (!s.IsSpellOver)
                    {
                        s.Update(deltaT);
                    }
                }


                DoCameraMovement();
            }

        }

        public void StartSequence(int questItemType)
        {
            if (questItemType == 0)
            {
                _showSequence1Timer = GameProperties.IntroDisplayTime;
            }
            else if (questItemType == 1)
            {
                _showSequence2Timer = GameProperties.IntroDisplayTime;
            }
            else if (questItemType == 2)
            {
                _showSequence3Timer = GameProperties.IntroDisplayTime;
            }
            else if (questItemType == 3)
            {
                _showSequence4Timer = GameProperties.IntroDisplayTime;
            }
            else if (questItemType == 4)
            {
                _showSequence5Timer = GameProperties.IntroDisplayTime;
            }
            else if (questItemType == 5)
            {
                _showSequence6Timer = GameProperties.IntroDisplayTime;
            }
        }

        private void UpdateSequenceTimers(float deltaT)
        {
            if (_showIntroTimer >= 0.0f)
            {
                _showIntroTimer -= deltaT;
            }
            if (_showSequence1Timer >= 0)
            {
                _showSequence1Timer -= deltaT;
            }
            if (_showSequence2Timer >= 0)
            {
                _showSequence2Timer -= deltaT;
            }
            if (_showSequence3Timer >= 0)
            {
                _showSequence3Timer -= deltaT;
            }
            if (_showSequence4Timer >= 0)
            {
                _showSequence4Timer -= deltaT;
            }
            if (_showSequence5Timer >= 0)
            {
                _showSequence5Timer -= deltaT;
            }
            if (_showSequence6Timer >= 0)
            {
                _showSequence6Timer -= deltaT;
            }
        }

        private void DoCameraMovement()
        {
            Vector2i newCamPos = new Vector2i(_player.ActorPosition.X - 6, _player.ActorPosition.Y - 6);
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
            if (!IsInSequence())
            {
                foreach (var t in _tileList)
                {
                    t.Draw(rw, CameraPosition);
                }
                foreach (var i in _itemList)
                {
                    if (!i.Picked)
                    {
                        i.Draw(rw, CameraPosition);
                    }
                }
                foreach (var i in _questItemList)
                {
                    if (!i.Picked)
                    {
                        i.Draw(rw, CameraPosition);
                    }
                }

                foreach (var e in _enemyList)
                {
                    if (!e.IsDead)
                    {
                        e.Draw(rw, CameraPosition);
                    }
                }

                _player.Draw(rw, CameraPosition);
                bool drawHouses = true;
                foreach (var h in _houseList)
                {
                    if (h.IsActive)
                    {
                        drawHouses = !h.IsActive;
                        h.DrawText(rw);
                        break;
                    }
                }
                if (drawHouses)
                {
                    foreach (var h in _houseList)
                    {
                        h.Draw(rw, CameraPosition);
                    }
                }

                

                foreach (var s in _spellList)
                {
                    if (!s.IsSpellOver)
                    {
                        s.Draw(rw, CameraPosition);
                    }
                }

                DrawItemToolTip(rw);
                _sidebar.Draw(rw);
            }
            else
            {
                if (_showIntroTimer >= 0.0f)
                {
                    DrawIntro(rw);
                }
                if (_showSequence1Timer >= 0)
                {
                    DrawSequence1(rw);
                }
                if (_showSequence2Timer >= 0)
                {
                    DrawSequence2(rw);
                }
                if (_showSequence3Timer >= 0)
                {
                    DrawSequence3(rw);
                }
                if (_showSequence4Timer >= 0)
                {
                    DrawSequence4(rw);
                }
                if (_showSequence5Timer >= 0)
                {
                    DrawSequence5(rw);
                }
                if (_showSequence6Timer >= 0)
                {
                    DrawSequence6(rw);
                }

            }
        }

        private void DrawIntro(RenderWindow rw)
        {
            rw.Draw(_introSprite);

            DrawText("As you come home from", new Vector2f(20, 25), GameProperties.ColorWhite, rw);
            DrawText("a trip to the city...", new Vector2f(20, 50), GameProperties.ColorWhite, rw);


            DrawText("A horde of goblins has", new Vector2f(20, 100), GameProperties.ColorWhite, rw);
            DrawText("devastated your farm", new Vector2f(20, 125), GameProperties.ColorWhite, rw);

            DrawText("and slaughtered your", new Vector2f(20, 150), GameProperties.ColorWhite, rw);
            DrawText("family and friends.", new Vector2f(20, 175), GameProperties.ColorWhite, rw); // friens

            DrawText("Find them and take ", new Vector2f(20, 225), GameProperties.ColorWhite, rw);
            DrawText("your revenge!", new Vector2f(20, 250), GameProperties.ColorWhite, rw);


            DrawText("You see", new Vector2f(20, 325), GameProperties.ColorWhite, rw);
            DrawText("'They went north'", new Vector2f(20, 350), GameProperties.ColorLightRed, rw);
            DrawText("scribbled on a wall in blood.", new Vector2f(20, 375), GameProperties.ColorWhite, rw);

            DrawText("[Space] Continue", new Vector2f(560, 555), GameProperties.ColorWhite, rw);
        }


        private void DrawSequence1(RenderWindow rw)
        {
            rw.Clear(GameProperties.ColorBlack);

            DrawText("You slaughtered the goblin pack", new Vector2f(20, 25), GameProperties.ColorWhite, rw);
            DrawText("with the warm feeling of revenge.", new Vector2f(20, 50), GameProperties.ColorWhite, rw);

            DrawText("But...", new Vector2f(20, 75), GameProperties.ColorWhite, rw);


            DrawText("A goblin cries out", new Vector2f(20, 125), GameProperties.ColorWhite, rw);
            DrawText("'Headless master! Help us in this misery!'", new Vector2f(20, 150), GameProperties.ColorLightRed, rw);

            DrawText("The goblin leader was not with them.", new Vector2f(20, 175), GameProperties.ColorWhite, rw);

            DrawText("'Good for you'", new Vector2f(20, 225), GameProperties.ColorLightRed, rw);
            DrawText("he whispers", new Vector2f(20, 250), GameProperties.ColorWhite, rw);
            DrawText("'He can only be slain by an adamantium sword.'", new Vector2f(20, 275), GameProperties.ColorLightRed, rw);
            DrawText("So head west and look for the forge!'", new Vector2f(20, 325), GameProperties.ColorWhite, rw);

            DrawText("[Space] Continue", new Vector2f(560, 555), GameProperties.ColorWhite, rw);
        }

        private void DrawSequence2(RenderWindow rw)
        {
            rw.Clear(GameProperties.ColorBlack);

            // draw Forge Texture

            DrawText("You found the forge,", new Vector2f(20, 25), GameProperties.ColorWhite, rw);
            DrawText("but the blacksmith is not there.", new Vector2f(20, 50), GameProperties.ColorWhite, rw);


            DrawText("'Looking for some potatoes ", new Vector2f(20, 125), GameProperties.ColorLightBlue, rw);
            DrawText("in the western mountains.'", new Vector2f(20, 150), GameProperties.ColorLightBlue, rw);

            DrawText("a note says.", new Vector2f(20, 175), GameProperties.ColorWhite, rw);

            DrawText("So head west and look for the blacksmith!", new Vector2f(20, 325), GameProperties.ColorWhite, rw);

            DrawText("[Space] Continue", new Vector2f(560, 555), GameProperties.ColorWhite, rw);
        }

        private void DrawSequence3(RenderWindow rw)
        {
            rw.Clear(GameProperties.ColorBlack);

            // draw BlackSmitz picture

            DrawText("You found the blacksmith,", new Vector2f(20, 25), GameProperties.ColorWhite, rw);

            DrawText("'I will forge your adamantium sword.", new Vector2f(20, 125), GameProperties.ColorLightBlue, rw);
            DrawText("Bring the adamantium ore to my forge.", new Vector2f(20, 150), GameProperties.ColorLightBlue, rw);
            DrawText("You can find some in the mountains in the south.", new Vector2f(20, 175), GameProperties.ColorLightBlue, rw);
            DrawText("When you have some, come back to my forge!.", new Vector2f(20, 200), GameProperties.ColorLightBlue, rw);


            DrawText("So head south and look for some adamantium ore!", new Vector2f(20, 325), GameProperties.ColorWhite, rw);

            DrawText("[Space] Continue", new Vector2f(560, 555), GameProperties.ColorWhite, rw);
        }

        private void DrawSequence4(RenderWindow rw)
        {
            rw.Clear(GameProperties.ColorBlack);

            // draw Adamantium Picture

            DrawText("You found the adamantium ore.", new Vector2f(20, 25), GameProperties.ColorWhite, rw);

            DrawText("Head back to the forge!", new Vector2f(20, 325), GameProperties.ColorWhite, rw);

            DrawText("[Space] Continue", new Vector2f(560, 555), GameProperties.ColorWhite, rw);
        }

        private void DrawSequence5(RenderWindow rw)
        {
            rw.Clear(GameProperties.ColorBlack);

            // draw Forge Texture with Blacksmiz

            DrawText("You bring the ore to the blacksmith.", new Vector2f(20, 25), GameProperties.ColorWhite, rw);
            DrawText("All you hear for the next hours is", new Vector2f(20, 50), GameProperties.ColorWhite, rw);
            DrawText("the sound of true metal hitting black metal.", new Vector2f(20, 75), GameProperties.ColorWhite, rw);


            DrawText("'Here is your sword!", new Vector2f(20, 125), GameProperties.ColorLightBlue, rw);
            DrawText("Use it with great responsibility.'", new Vector2f(20, 150), GameProperties.ColorLightBlue, rw);

            DrawText("You take the sword and embark on your journey.", new Vector2f(20, 300), GameProperties.ColorWhite, rw);
            DrawText("Head south to look for the headless goblin!", new Vector2f(20, 325), GameProperties.ColorWhite, rw);

            DrawText("[Space] Continue", new Vector2f(560, 555), GameProperties.ColorWhite, rw);
        }

        private void DrawSequence6(RenderWindow rw)
        {
            rw.Clear(GameProperties.ColorBlack);

            // draw headless goblin

            DrawText("You found the headless goblin and ", new Vector2f(20, 25), GameProperties.ColorWhite, rw);
            DrawText("split his head with your adamantium sword", new Vector2f(20, 50), GameProperties.ColorWhite, rw);
            DrawText("bruising some muscles and breaking some bones.", new Vector2f(20, 75), GameProperties.ColorWhite, rw);

            DrawText("Your quest for revenge is over.", new Vector2f(20, 125), GameProperties.ColorLightBlue, rw);
            //DrawText("You take a deep breath and release all the anger.", new Vector2f(20, 150), GameProperties.ColorLightBlue, rw);

            DrawText("[Space] To Main Menu", new Vector2f(470, 555), GameProperties.ColorWhite, rw);
        }


        private void DrawText(string s, Vector2f position, Color color, RenderWindow window)
        {
            text.DisplayedString = s;
            text.Position = position;
            text.Color = color;
            window.Draw(text);
        }

        private void DrawItemToolTip(RenderWindow rw)
        {
            if (_displayItemToolTip && _tooltipItem != null)
            {
                _itemTooltipSprite.Position = new Vector2f(300.0f, 500.0f);
                rw.Draw(_itemTooltipSprite);

                Text _tooltipText = new Text(_tooltipItem.Name, GameProperties.GameFont());
                _tooltipText.Position = new Vector2f(340, 510);
                _tooltipText.Scale = new Vector2f(0.5f, 0.55f);
                rw.Draw(_tooltipText);

                int i = 0;
                foreach (var kvp in _tooltipItem.Modifiers)
                {
                    _tooltipText = new Text(Attributes.GetAttributeNameFromEnum(kvp.Key) + " " + kvp.Value.ToString(), GameProperties.GameFont());
                    _tooltipText.Position = new Vector2f(340, 530 + i * 15);
                    _tooltipText.Scale = new Vector2f(0.6f, 0.6f);
                    rw.Draw(_tooltipText);
                    i++;
                }

                Item playersItem = null;
                if (_tooltipItem.ItemType == ItemType.FEET)
                {
                    playersItem = _player.FeetItem;
                }
                else if (_tooltipItem.ItemType == ItemType.HAND)
                {
                    playersItem = _player.HandItem;
                }
                else if (_tooltipItem.ItemType == ItemType.HEAD)
                {
                    playersItem = _player.HeadItem;
                }
                else if (_tooltipItem.ItemType == ItemType.TORSO)
                {
                    playersItem = _player.TorsoItem;
                }

                if (playersItem != null)
                {
                    _itemTooltipSprite.Position = new Vector2f(300.0f, 400.0f);
                    rw.Draw(_itemTooltipSprite);

                    _tooltipText = new Text("You own: " + playersItem.Name, GameProperties.GameFont());
                    _tooltipText.Position = new Vector2f(340, 410);
                    _tooltipText.Scale = new Vector2f(0.5f, 0.55f);
                    rw.Draw(_tooltipText);

                    i = 0;
                    foreach (var kvp in playersItem.Modifiers)
                    {
                        _tooltipText = new Text(Attributes.GetAttributeNameFromEnum(kvp.Key) + " " + kvp.Value.ToString(), GameProperties.GameFont());
                        _tooltipText.Position = new Vector2f(340, 430 + i * 15);
                        _tooltipText.Scale = new Vector2f(0.6f, 0.6f);
                        rw.Draw(_tooltipText);
                        i++;
                    }

                }
            }
            
        }

        public void InitGame()
        {
            _player = new Player(this, 0);
            _sidebar = new Sidebar(_player);
            _itemList = new List<Item>();
            _enemyList = new List<Enemy>();
            _houseList = new List<NomadsHouse>();
            _spellList = new List<Spell>();
            _questItemList = new List<QuestItem>();

            _showIntroTimer = GameProperties.IntroDisplayTime;

            _showSequence1Timer = 0.0f;
            _showSequence2Timer = 0.0f;
            _showSequence3Timer = 0.0f;
            _showSequence4Timer = 0.0f;
            _showSequence5Timer = 0.0f;
            _showSequence6Timer = 0.0f;


            CreateWorld();
        }

        private void CreateWorld()
        {
            var parser = new MapParser("map.tmx", this);

            _tileList = parser.TerrainLayer;
            CameraPosition = new Vector2i(0, 0);

            //_itemList.Add(ItemFactory.GetHeadItem(new Vector2i(2, 4)));
            //_itemList.Add(ItemFactory.GetTorsoItem(new Vector2i(3, 4)));
            //_itemList.Add(ItemFactory.GetFeetItem(new Vector2i(4, 4)));
            //_itemList.Add(ItemFactory.GetHandItem(new Vector2i(5, 4)));

            _enemyList = parser.EnemyLayer;
            _player.MovePlayer(parser.PlayerPosition);

            foreach (var item in parser.ObjectLayer)
            {
                if (item is NomadsHouse)
                {
                    _houseList.Add(item as NomadsHouse);
                }
                else if (item is QuestItem)
                {
                    _questItemList.Add(item as QuestItem);
                }
            }

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

        internal Enemy GetEnemyOnTile(SFML.Window.Vector2i vector2i)
        {
            Enemy newEnemy = null;
            foreach (var e in _enemyList)
            {
                if (e.ActorPosition.Equals(vector2i))
                {
                    newEnemy = e;
                    break;
                }
            }
            return newEnemy;
        }

        internal NomadsHouse GetHouseOnTile(SFML.Window.Vector2i vector2i)
        {
            NomadsHouse newHouse = null;
            foreach (var h in _houseList)
            {
                if (h.PositionInTiles.Equals(vector2i))
                {
                    newHouse = h;
                    break;
                }
            }
            return newHouse;
        }


        public bool IsPlayerDead()
        {
            return _player.IsDead;
        }

        public Score EndThisRound()
        {
            return new Score(_player);

        }

        private bool IsInSequence()
        {
            bool ret = false;
            if (_showIntroTimer >= 0)
            {
                ret = true;
            }
            if (_showSequence1Timer >= 0)
            {
                ret = true;
            }
            if (_showSequence2Timer >= 0)
            {
                ret = true;
            }
            if (_showSequence3Timer >= 0)
            {
                ret = true;
            }
            if (_showSequence4Timer >= 0)
            {
                ret = true;
            }
            if (_showSequence5Timer >= 0)
            {
                ret = true;
            }
            if (_showSequence6Timer >= 0)
            {
                ret = true;
            }


            return ret;

        }

        #endregion Methods


        internal int GetTotalGold()
        {
            int sum = 0;
            foreach (var e in _enemyList)
            {
                sum += e.DropGold;
            }
            return sum;
        }

        internal int GetTotalExperience()
        {
            int sum = 0;
            foreach (var e in _enemyList)
            {
                sum += e.DropExperience;
            }
            return sum;
        }
    }
}

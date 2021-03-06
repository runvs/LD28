﻿/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net
/// 
using SFML.Graphics;
using SFML.Window;
using System;
using JamUtilities;

namespace JamTemplate
{
    public class NomadsHouse : IGameObject
    {

        #region Fields

        public Vector2i PositionInTiles { get; private set; }
        private Texture _houseTexture;
        private Sprite _houseSprite;
        private HouseType _type;

        private float _buttonTimer;

        private Texture _overlayTexture;
        private Sprite _overlaySprite;
        private World _world;

        private Item item1;
        private Item item2;
        private Item item3;
        private Item item4;

        public bool IsActive { get; set; }


        #endregion Fields

        #region Methods

        public NomadsHouse(int posX, int posY, World world)
        {

            _world = world;
            _buttonTimer = 0.0f;
            PositionInTiles = new Vector2i(posX, posY);
            IsActive = false;
            double ran = GameProperties.RandomGenerator.NextDouble();
            if (ran <= 0.33)
            {
                _type = HouseType.MERCHANT;

                item1 = ItemFactory.GetHeadItem(new Vector2i(0, 0));
                item2 = ItemFactory.GetTorsoItem(new Vector2i(0, 0));
                item3 = ItemFactory.GetHandItem(new Vector2i(0, 0));
                item4 = ItemFactory.GetFeetItem(new Vector2i(0, 0));

            }
            else if (ran <= 0.66)
            {
                _type = HouseType.TEACHER;
            }
            else
            {
                _type = HouseType.HEALER;
            }
            try
            {
                LoadGraphics();
                text = new Text("", GameProperties.GameFont());
            }
            catch (SFML.LoadingFailedException e)
            {
                Console.Out.WriteLine("Error loading house Graphics.");
                Console.Out.WriteLine(e.ToString());
            }

        }


        public void Draw(RenderWindow rw, Vector2f CameraPosition)
        {
            _houseSprite.Position = new Vector2f(
                GameProperties.TileSizeInPixel * PositionInTiles.X - CameraPosition.X,
                GameProperties.TileSizeInPixel * PositionInTiles.Y - CameraPosition.Y
            );

            rw.Draw(_houseSprite);

            if (IsActive)
            {
                DrawText(rw);

            }
        }

        public void DrawText(RenderWindow rw)
        {
            rw.Draw(_overlaySprite);
            if (_type == HouseType.MERCHANT)
            {
                DrawText("What would you like to buy?", new Vector2f(200, 200), GameProperties.ColorWhite, rw);

                DrawText(item1.Name + " [U]", new Vector2f(240, 250), GameProperties.ColorWhite, rw);
                DrawText(item2.Name + " [I]", new Vector2f(240, 280), GameProperties.ColorWhite, rw);
                DrawText(item3.Name + " [O]", new Vector2f(240, 310), GameProperties.ColorWhite, rw);
                DrawText(item4.Name + " [P]", new Vector2f(240, 340), GameProperties.ColorWhite, rw);

                DrawText(string.Format("Cost per item: {0}; Current Gold {1}", GameProperties.BuyItemGoldCost, _world._player.Gold), new Vector2f(210, 365), GameProperties.ColorBeige, rw);
            }
            else if (_type == HouseType.TEACHER)
            {
                DrawText("What would you like", new Vector2f(210, 200), GameProperties.ColorWhite, rw);
                DrawText("to learn?", new Vector2f(210, 216), GameProperties.ColorWhite, rw);

                int strCost, agiCost, intCost, endCost;
                CalculateCosts(out strCost, out agiCost, out intCost, out endCost);

                DrawText(string.Format("Strength [U] Cost: {0}", strCost), new Vector2f(240, 250), GameProperties.ColorWhite, rw);
                DrawText(string.Format("Agility [I] Cost: {0}", agiCost), new Vector2f(240, 280), GameProperties.ColorWhite, rw);
                DrawText(string.Format("Intelligence [O] Cost: {0}", intCost), new Vector2f(240, 310), GameProperties.ColorWhite, rw);
                DrawText(string.Format("Endurance [P] Cost: {0}", endCost), new Vector2f(240, 340), GameProperties.ColorWhite, rw);

                DrawText(
                    string.Format("Experience {0}", _world._player.ActorAttributes.Experience),
                    new Vector2f(210, 365),
                    GameProperties.ColorLightGreen,
                    rw
                );

            }

            else if (_type == HouseType.HEALER)
            {
                DrawText("What would you like", new Vector2f(210, 200), GameProperties.ColorWhite, rw);
                DrawText("to do?", new Vector2f(210, 216), GameProperties.ColorWhite, rw);

                DrawText(string.Format("Heal [U] Cost: {0}", GameProperties.BuyHealGoldCost), new Vector2f(240, 250), GameProperties.ColorWhite, rw);
                DrawText(string.Format("Rest [I] Cost: {0}", GameProperties.BuyHealGoldCost), new Vector2f(240, 280), GameProperties.ColorWhite, rw);
                DrawText(string.Format("Heal and Rest[O] Cost: {0}", Math.Ceiling(GameProperties.BuyHealGoldCost * 1.5f)), new Vector2f(240, 310), GameProperties.ColorWhite, rw);

                DrawText("Gold " + _world._player.Gold, new Vector2f(210, 365), GameProperties.ColorBeige, rw);

            }

        }

        private void CalculateCosts(out int strCost, out int agiCost, out int intCost, out int endCost)
        {
            strCost = _world._player.ActorAttributes.BaseStrength - GameProperties.IncreaseAttributeExperienceCost;
            agiCost = _world._player.ActorAttributes.BaseAgility - GameProperties.IncreaseAttributeExperienceCost;
            intCost = _world._player.ActorAttributes.BaseIntelligence - GameProperties.IncreaseAttributeExperienceCost;
            endCost = _world._player.ActorAttributes.BaseEndurance - GameProperties.IncreaseAttributeExperienceCost;
        }
        Text text;

        private void DrawText(string s, Vector2f position, Color color, RenderWindow window)
        {
            SmartText.DrawText(s, position, color, new Vector2f(0.7f, 0.7f), window);
        }

        public void GetInput()
        {
            if (IsActive)
            {
                if (_buttonTimer <= 0.0f)
                {
                    if (_type == HouseType.TEACHER)
                    {
                        int strCost, agiCost, intCost, endCost;
                        CalculateCosts(out strCost, out agiCost, out intCost, out endCost);

                        if (Keyboard.IsKeyPressed(Keyboard.Key.U) && _world._player.ActorAttributes.Experience >= strCost)
                        {
                            _world._player.ActorAttributes.Experience -= strCost;
                            _world._player.ExperienceSpent += strCost;
                            _buttonTimer += 0.2f;
                            _world._player.ActorAttributes.BaseStrength++;
                        }
                        if (Keyboard.IsKeyPressed(Keyboard.Key.I) && _world._player.ActorAttributes.Experience >= agiCost)
                        {
                            _world._player.ActorAttributes.Experience -= agiCost;
                            _world._player.ExperienceSpent += agiCost;
                            _buttonTimer += 0.2f;
                            _world._player.ActorAttributes.BaseAgility++;
                        }
                        if (Keyboard.IsKeyPressed(Keyboard.Key.O) && _world._player.ActorAttributes.Experience >= intCost)
                        {
                            _world._player.ActorAttributes.Experience -= intCost;
                            _world._player.ExperienceSpent += intCost;
                            _buttonTimer += 0.2f;
                            _world._player.ActorAttributes.BaseIntelligence++;
                        }
                        if (Keyboard.IsKeyPressed(Keyboard.Key.P) && _world._player.ActorAttributes.Experience >= endCost)
                        {
                            _world._player.ActorAttributes.Experience -= endCost;
                            _world._player.ExperienceSpent += endCost;
                            _buttonTimer += 0.2f;
                            _world._player.ActorAttributes.BaseEndurance++;
                            _world._player.ActorAttributes.ReCalculateHealth();
                        }
                    }
                    else if (_type == HouseType.MERCHANT)
                    {
                        if (_world._player.Gold >= GameProperties.BuyItemGoldCost)
                        {
                            if (Keyboard.IsKeyPressed(Keyboard.Key.U))
                            {
                                _world._player.Gold -= GameProperties.BuyItemGoldCost;
                                _world._player.GoldSpent += GameProperties.BuyItemGoldCost;
                                _buttonTimer += 0.2f;
                                _world._player.PickupItem(item1);
                                _world._player.ActorAttributes.ReCalculateHealth();
                            }
                            if (Keyboard.IsKeyPressed(Keyboard.Key.I))
                            {
                                _world._player.Gold -= GameProperties.BuyItemGoldCost;
                                _world._player.GoldSpent += GameProperties.BuyItemGoldCost;
                                _buttonTimer += 0.2f;
                                _world._player.PickupItem(item2);
                                _world._player.ActorAttributes.ReCalculateHealth();
                            }
                            if (Keyboard.IsKeyPressed(Keyboard.Key.O))
                            {
                                _world._player.Gold -= GameProperties.BuyItemGoldCost;
                                _world._player.GoldSpent += GameProperties.BuyItemGoldCost;
                                _buttonTimer += 0.2f;
                                _world._player.PickupItem(item3);
                                _world._player.ActorAttributes.ReCalculateHealth();
                            }
                            if (Keyboard.IsKeyPressed(Keyboard.Key.P))
                            {
                                _world._player.Gold -= GameProperties.BuyItemGoldCost;
                                _world._player.GoldSpent += GameProperties.BuyItemGoldCost;
                                _buttonTimer += 0.2f;
                                _world._player.PickupItem(item4);
                                _world._player.ActorAttributes.ReCalculateHealth();
                            }
                        }

                    }
                    else if (_type == HouseType.HEALER)
                    {
                        if (_world._player.Gold >= GameProperties.BuyHealGoldCost)
                        {
                            if (Keyboard.IsKeyPressed(Keyboard.Key.U))
                            {
                                _world._player.Gold -= GameProperties.BuyHealGoldCost;
                                _world._player.GoldSpent += GameProperties.BuyHealGoldCost;
                                _buttonTimer += 0.2f;
                                _world._player.ActorAttributes.RefillHealth();
                            }
                            if (Keyboard.IsKeyPressed(Keyboard.Key.I))
                            {
                                _world._player.Gold -= GameProperties.BuyHealGoldCost;
                                _world._player.GoldSpent += GameProperties.BuyHealGoldCost;
                                _buttonTimer += 0.2f;
                                _world._player.ActorAttributes.RefillStamina();
                            }
                            if (Keyboard.IsKeyPressed(Keyboard.Key.O))
                            {
                                if (_world._player.Gold >= Math.Ceiling(GameProperties.BuyHealGoldCost * 1.5f))
                                {
                                    _world._player.Gold -= (int)Math.Ceiling(1.5f * GameProperties.BuyHealGoldCost);
                                    _world._player.GoldSpent += (int)Math.Ceiling(1.5f * GameProperties.BuyHealGoldCost);
                                    _buttonTimer += 0.2f;
                                    _world._player.ActorAttributes.RefillStamina();
                                    _world._player.ActorAttributes.RefillHealth();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Update(float deltaT)
        {
            if (_buttonTimer >= 0.0f)
            {
                _buttonTimer -= deltaT;
            }

        }

        private void LoadGraphics()
        {
            _houseTexture = new Texture("../GFX/house.png");
            _houseSprite = new Sprite(_houseTexture);
            _houseSprite.Scale = new Vector2f(2.0f, 2.0f);

            _overlayTexture = new Texture("../GFX/overlay_shop.png");
            _overlaySprite = new Sprite(_overlayTexture);
            _overlaySprite.Scale = new Vector2f(3.0f, 2.0f);  // do not scale this
            _overlaySprite.Position = new Vector2f(200.0f, 200.0f);
        }

        #endregion Methods

    }

    enum HouseType
    {
        MERCHANT,
        TEACHER,
        HEALER
    }

}

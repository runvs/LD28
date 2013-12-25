/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net
/// 

using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace JamTemplate
{
    public class QuestItem : IGameObject
    {

        #region Fields
        public int QuestItemType { get; private set; }
        private World _world;
        public Vector2i PositionInTiles { get; private set; }
        public bool Picked { get; private set; }

        private static Vector2i ForgePosition { get; set; }

        private Texture _itemTexture;
        private Sprite _itemSprite;

        #endregion Fields

        public QuestItem(World world, int type, Vector2i position)
        {
            _world = world;
            QuestItemType = type;
            PositionInTiles = position;
            Picked = false;

            try
            {
                LoadGraphics();
            }
            catch (SFML.LoadingFailedException e)
            {
                Console.Out.WriteLine("Error loading QuestItem Graphics.");
                Console.Out.WriteLine(e.ToString());
            }
        }

        public void PickUpItem(List<QuestItem> itemsToAdd)
        {
            if (QuestItemType == 1)
            {
                ForgePosition = this.PositionInTiles;
            }
            if (QuestItemType == 2)
            {

                itemsToAdd.Add(new QuestItem(_world, 4, ForgePosition));
            }

            Picked = true;
            PositionInTiles = new Vector2i(-500, -500);
            _world.StartSequence(QuestItemType);



        }

        public void Draw(RenderWindow rw, Vector2f CameraPosition)
        {
            _itemSprite.Position = new Vector2f(
                GameProperties.TileSizeInPixel * PositionInTiles.X - CameraPosition.X,
                GameProperties.TileSizeInPixel * PositionInTiles.Y - CameraPosition.Y
            );

            rw.Draw(_itemSprite);
        }

        public void Update(float deltaT, List<QuestItem> itemsToAdd)
        {
            Vector2i playerPosition = _world._player.ActorPosition;
            if (playerPosition.Equals(PositionInTiles))
            {
                _world._player._log.CompleteCurrentQuest();
                PickUpItem(itemsToAdd);

            }
        }


        public void LoadGraphics()
        {
            if (QuestItemType == 0)
            {
                _itemTexture = new Texture("../GFX/quest_camp.png");
            }
            else if (QuestItemType == 1)
            {
                _itemTexture = new Texture("../GFX/quest_forge.png");
            }
            else if (QuestItemType == 2)
            {
                _itemTexture = new Texture("../GFX/quest_blacksmith.png");
            }
            else if (QuestItemType == 3)
            {
                _itemTexture = new Texture("../GFX/quest_ore.png");
            }
            else if (QuestItemType == 4)
            {
                _itemTexture = new Texture("../GFX/quest_smithy.png");
            }

            _itemSprite = new Sprite(_itemTexture);
            _itemSprite.Scale = new Vector2f(2.0f, 2.0f);
        }





    }
}

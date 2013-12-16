using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamTemplate
{
    public class QuestItem
    {

        #region Fields
        public int QuestItemType { get; private set; }
        private World _world;
        public Vector2i PositionInTiles { get; private set; }
        public bool Picked { get; private set; }

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

        public void PickUpItem()
        {
            Picked = true;
            PositionInTiles = new Vector2i(-500, -500);
        }

        public void Draw(RenderWindow rw, Vector2i CameraPosition)
        {
            _itemSprite.Position = new Vector2f(
                GameProperties.TileSizeInPixel * (PositionInTiles.X - CameraPosition.X),
                GameProperties.TileSizeInPixel * (PositionInTiles.Y - CameraPosition.Y)
            );

            rw.Draw(_itemSprite);
        }

        public void Update(float deltaT)
        {
            Vector2i playerPosition = _world._player.ActorPosition;
            if (playerPosition.Equals(PositionInTiles))
            {
                _world._player._log.CompleteCurrentQuest();
                PickUpItem();

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
                _itemTexture = new Texture("../GFX/enemy_goblin_1.png");
            }
        }





    }
}

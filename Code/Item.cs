using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace JamTemplate
{
    public class Item
    {

        #region Fields


        public ItemType ItemType { get; private set; }
        public string Name { get; private set; }
        public Dictionary<AttributeType, int> Modifiers { get; set; }

        Texture ItemTexture;
        Sprite ItemSprite;
        public Vector2i ItemPositionInTiles { get; private set; }
        public bool Picked { get; private set; }

        private string _texturePath;
        private string p1;
        private int p2;

        #endregion Fields

        #region Methods


        public Item(ItemType t, string name, Dictionary<AttributeType, int> modifiers, Vector2i positionInTiles, string texturePath)
        {
            this.ItemType = t;
            this.Name = name;
            this.Modifiers = modifiers;
            _texturePath = texturePath;

            Picked = false;

            ItemPositionInTiles = positionInTiles;

            try
            {
                LoadGraphics();
            }
            catch (SFML.LoadingFailedException e)
            {
                System.Console.Out.WriteLine("Error loading Item Graphics.");
                System.Console.Out.WriteLine(e.ToString());
            }
        }


        public void PickUp()
        {
            Picked = true;
            ItemPositionInTiles = new Vector2i(-500, -500);
            if (ItemType == JamTemplate.ItemType.FEET)
            {
                ItemSprite.Position = GameProperties.InventoryFeetItemPosition;
            }
            else if (ItemType == JamTemplate.ItemType.HAND)
            {
                ItemSprite.Position = GameProperties.InventoryHandItemPosition;
            }
            else if (ItemType == JamTemplate.ItemType.HEAD)
            {
                ItemSprite.Position = GameProperties.InventoryHeadItemPosition;
            }
            else if (ItemType == JamTemplate.ItemType.TORSO)
            {
                ItemSprite.Position = GameProperties.InventoryTorsoItemPosition;
            }
        }


        private void LoadGraphics()
        {
            ItemTexture = new Texture(_texturePath);
            ItemSprite = new Sprite(ItemTexture);
            ItemSprite.Scale = new Vector2f(2.0f, 2.0f);
        }

        public void Draw(RenderWindow rw, Vector2i CameraPosition)
        {
            if (!Picked)
            {
                ItemSprite.Position = new Vector2f(
                    GameProperties.TileSizeInPixel * (ItemPositionInTiles.X - CameraPosition.X),
                    GameProperties.TileSizeInPixel * (ItemPositionInTiles.Y - CameraPosition.Y)
                    );
            }
            rw.Draw(ItemSprite);
        }

        #endregion Methods
    }

    public enum ItemType
    {
        HEAD, TORSO, HAND, FEET
    }

    public enum AttributeType
    {
        STRENGTH, INTELLIGENCE, AGILITY, ENDURANCE
    }
}
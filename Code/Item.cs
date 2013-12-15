
namespace JamTemplate
{
    public class Item
    {

        #region Fields


        public ItemType ItemType { get; private set; }
        public string Name { get; private set; }
        public int ItemModifier { get; set; }

        SFML.Graphics.Texture ItemTexture;
        SFML.Graphics.Sprite ItemSprite;
        public SFML.Window.Vector2i ItemPositionInTiles { get; private set; }
        public bool Picked { get; private set; }

        private string p1;
        private int p2;

        #endregion Fields

        #region Methods


        public Item(ItemType t, string name, int modifier, SFML.Window.Vector2i positionInTiles)
        {
            this.ItemType = t;
            this.Name = name;
            this.ItemModifier = modifier;

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
            ItemPositionInTiles = new SFML.Window.Vector2i(-500, -500);
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
            if (Name.Contains("sword"))
            {
                ItemTexture = new SFML.Graphics.Texture("../GFX/item_sword.png");
            }
            else if (Name.Contains("axe"))
            {
                ItemTexture = new SFML.Graphics.Texture("../GFX/item_axe.png");
            }
            else if (Name.Contains("hammer"))
            {
                ItemTexture = new SFML.Graphics.Texture("../GFX/item_hammer.png");
            }
            else if (Name.Contains("hammer"))
            {
                ItemTexture = new SFML.Graphics.Texture("../GFX/item_hammer.png");
            }
            else if (Name.Contains("helmet"))
            {
                ItemTexture = new SFML.Graphics.Texture("../GFX/item_helmet.png");
            }
            else if (Name.Contains("boots"))
            {
                ItemTexture = new SFML.Graphics.Texture("../GFX/item_boots.png");
            }
            else if (Name.Contains("greaves"))
            {
                ItemTexture = new SFML.Graphics.Texture("../GFX/item_greaves.png");
            }
            else if (Name.Contains("breastplate"))
            {
                ItemTexture = new SFML.Graphics.Texture("../GFX/item_breastplate.png");
            }

            ItemSprite = new SFML.Graphics.Sprite(ItemTexture);
            ItemSprite.Scale = new SFML.Window.Vector2f(2.0f, 2.0f);


        }

        public void Draw(SFML.Graphics.RenderWindow rw, SFML.Window.Vector2i CameraPosition)
        {
            if (!Picked)
            {
                ItemSprite.Position = new SFML.Window.Vector2f(
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
}

namespace JamTemplate
{
    public class Item
    {
        public ItemType ItemType { get; private set; }
        public string Name { get; private set; }

        public Item(ItemType t, string name, double modifier)
        {
            this.ItemType = t;
            this.Name = name;
        }
    }

    public enum ItemType
    {
        HEAD, TORSO, HAND, FEET
    }
}
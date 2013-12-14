
namespace JamTemplate
{
    public class Item
    {
        public ItemType ItemType { get; private set; }

        public Item(ItemType t)
        {
            this.ItemType = t;
        }
    }

    public enum ItemType
    {
        HEAD, TORSO, HAND, FEET
    }
}

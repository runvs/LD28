
namespace JamTemplate
{
    public class Attributes
    {
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Agility { get; set; }
        public int Endurance { get; set; }

        public int ModifierStrength { get; set; }
        public int ModifierIntelligence { get; set; }
        public int ModifierAgility { get; set; }
        public int ModifierEndurance { get; set; }

        public int HealthMaximum { get; set; }
        public int HealthCurrent { get; set; }

        public int StaminaMaximum { get; set; }
        public int StaminaCurrent { get; set; }

        public Attributes()
        {
            Strength = 3;
            Intelligence = 3;
            Agility = 3;
            Endurance = 3;

            ModifierAgility = 0;
            ModifierEndurance = 0;
            ModifierStrength = 0;
            ModifierIntelligence = 0;

            HealthCurrent = 17;
            HealthMaximum = 20;

            StaminaCurrent = 17;
            StaminaMaximum = 40;
        }

        public void ResetModifiers()
        {
            ModifierAgility = 0;
            ModifierEndurance = 0;
            ModifierStrength = 0;
            ModifierIntelligence = 0;
        }

        public void CalculateModifiersForItem(Item item)
        {
            if (item != null)
            {
                if (item.ItemType == ItemType.FEET)
                {
                    ModifierAgility += item.ItemModifier;
                }
                else if (item.ItemType == ItemType.HAND)
                {
                    ModifierStrength += item.ItemModifier;
                }
                if (item.ItemType == ItemType.HEAD)
                {
                    ModifierIntelligence += item.ItemModifier;
                }
                if (item.ItemType == ItemType.TORSO)
                {
                    ModifierStrength += item.ItemModifier;
                }
            }
        }

    }
}

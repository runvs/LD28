
namespace JamTemplate
{
    public class Attributes
    {





        // This are base values

        public int Strength { get { return BaseStrength + ModifierStrength; } }
        public int Intelligence { get { return BaseIntelligence + ModifierIntelligence; } }
        public int Agility { get { return BaseAgility + ModifierAgility; } }
        public int Endurance { get { return BaseEndurance + ModifierEndurance; } }

        public int BaseStrength { get; set; }
        public int BaseIntelligence { get; set; }
        public int BaseAgility { get; set; }
        public int BaseEndurance { get; set; }

        public int ModifierStrength { get; set; }
        public int ModifierIntelligence { get; set; }
        public int ModifierAgility { get; set; }
        public int ModifierEndurance { get; set; }

        public int HealthMaximum { get; set; }
        public int HealthCurrent { get; set; }

        public int StaminaMaximum { get; set; }
        public int StaminaCurrent { get; set; }

        public int Experience { get; set; }


        public void ResetHealth(int newHealthVal)
        {
            HealthCurrent = HealthMaximum = newHealthVal;
        }


        public Attributes()
        {
            BaseStrength = 3;
            BaseIntelligence = 3;
            BaseAgility = 3;
            BaseEndurance = 3;

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
                foreach (var kvp in item.Modifiers)
                {
                    if (kvp.Key == AttributeType.AGILITY)
                    {
                        ModifierAgility += kvp.Value;
                    }
                    else if (kvp.Key == AttributeType.ENDURANCE)
                    {
                        ModifierEndurance += kvp.Value;
                    }
                    else if (kvp.Key == AttributeType.INTELLIGENCE)
                    {
                        ModifierIntelligence += kvp.Value;
                    }
                    else if (kvp.Key == AttributeType.STRENGTH)
                    {
                        ModifierStrength += kvp.Value;
                    }
                }
            }
        }

    }
}

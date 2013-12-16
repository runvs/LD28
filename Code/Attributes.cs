
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
        public float HealthRegenfreuency { get; set; }

        public int StaminaMaximum { get; set; }
        public int StaminaCurrent { get; set; }
        public float StaminaRegenfreuency { get; set; }

        public int Experience { get; set; }


        public void ResetHealth(int newHealthVal)
        {
            HealthCurrent = HealthMaximum = newHealthVal;
        }

        public void ResetStamina(int newStaminaVal)
        {
            StaminaCurrent = StaminaMaximum = newStaminaVal;
        }

        public void RefillHealth()
        {
            HealthCurrent = HealthMaximum;
        }
        public void RefillStamina()
        {
            StaminaCurrent = StaminaMaximum;
        }

        public void AddToCurrentHealth(int value)
        {
            HealthCurrent += value;
            if (HealthCurrent >= HealthMaximum)
            {
                HealthCurrent = HealthMaximum;
            }
        }

        public void AddToCurrentStamina(int value)
        {
            StaminaCurrent += value;
            if (StaminaCurrent >= StaminaMaximum)
            {
                StaminaCurrent = StaminaMaximum;
            }
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

            ReCalculateHealth();
            ResetHealth(HealthMaximum);
            ResetStamina(StaminaMaximum);

            HealthRegenfreuency = 0.0f;
            StaminaRegenfreuency = 1.0f;
        }

        public void ResetModifiers()
        {
            ModifierAgility = 0;
            ModifierEndurance = 0;
            ModifierStrength = 0;
            ModifierIntelligence = 0;
        }

        public static string GetAttributeNameFromEnum(AttributeType at)
        {
            if (at == AttributeType.AGILITY)
            {
                return "Agility";
            }
            else if (at == AttributeType.ENDURANCE)
            {
                return "Endurance";
            }
            else if (at == AttributeType.INTELLIGENCE)
            {
                return "Intelligence";
            }
            else if (at == AttributeType.STRENGTH)
            {
                return "Strength";
            }
            return "";
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
                        ReCalculateHealth();
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

        public void ReCalculateHealth()
        {
            int newHealth = GameProperties.PlayerBaseHealth + 3 * Endurance;
            int newStamina = GameProperties.PlayerBaseStamina + 5 * Endurance;

            HealthMaximum = newHealth;
            StaminaMaximum = newStamina;
        }

    }
}

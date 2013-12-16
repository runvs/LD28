/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net
/// 

using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace JamTemplate
{
    public class ItemFactory
    {
        private static List<Affix> _prefixList;
        private static List<Affix> _suffixList;
        private static List<BaseType> _baseTypeList;

        public static Item GetRandomItem(Vector2i position, EnemyStrength strength)
        {
            if (_baseTypeList == null)
            {
                LoadXml();
            }

            var values = Enum.GetValues(typeof(ItemType));
            var itemType = (ItemType)values.GetValue(GameProperties.RandomGenerator.Next(values.Length));

            return GetItem(itemType, position, strength);
        }

        public static Item GetHandItem(Vector2i position)
        {
            if (_baseTypeList == null)
            {
                LoadXml();
            }

            return GetItem(ItemType.HAND, position);
        }

        public static Item GetHeadItem(Vector2i position)
        {
            if (_baseTypeList == null)
            {
                LoadXml();
            }

            return GetItem(ItemType.HEAD, position);
        }

        public static Item GetTorsoItem(Vector2i position)
        {
            if (_baseTypeList == null)
            {
                LoadXml();
            }

            return GetItem(ItemType.TORSO, position);
        }

        public static Item GetFeetItem(Vector2i position)
        {
            if (_baseTypeList == null)
            {
                LoadXml();
            }

            return GetItem(ItemType.FEET, position);
        }

        private static Item GetItem(ItemType type, Vector2i position, EnemyStrength strength = EnemyStrength.HARD)
        {
            var baseTypes = _baseTypeList.Where(x => x.ItemType == type);
            var baseType = baseTypes.ElementAt(GameProperties.RandomGenerator.Next(baseTypes.Count()));
            var modifiers = new Dictionary<AttributeType, int>();
            var normalStrengthPrefix = GameProperties.RandomGenerator.NextDouble() > 0.5d;

            string prefixDesc = "", suffixDesc = "";

            if (strength == EnemyStrength.HARD || (strength == EnemyStrength.NORMAL && normalStrengthPrefix))
            {
                // Determine if we generate a prefix
                if (GameProperties.RandomGenerator.NextDouble() >= 0.5d)
                {
                    var prefix = _prefixList.ElementAt(GameProperties.RandomGenerator.Next(_prefixList.Count));

                    if (modifiers.ContainsKey(prefix.AttributeType))
                    {
                        modifiers[prefix.AttributeType] += prefix.Modifier;
                    }
                    else
                    {
                        modifiers.Add(prefix.AttributeType, prefix.Modifier);
                    }
                    prefixDesc = prefix.Description;
                }
            }


            if (strength == EnemyStrength.HARD || (strength == EnemyStrength.NORMAL && !normalStrengthPrefix))
            {
                // Determine if we generate a suffix
                if (GameProperties.RandomGenerator.NextDouble() >= 0.5d)
                {
                    var suffix = _suffixList.ElementAt(GameProperties.RandomGenerator.Next(_suffixList.Count));

                    if (modifiers.ContainsKey(suffix.AttributeType))
                    {
                        modifiers[suffix.AttributeType] += suffix.Modifier;
                    }
                    else
                    {
                        modifiers.Add(suffix.AttributeType, suffix.Modifier);
                    }
                    suffixDesc = suffix.Description;
                }
            }

            if (modifiers.ContainsKey(baseType.AttributeType))
            {
                modifiers[baseType.AttributeType] += baseType.Modifier;
            }
            else
            {
                modifiers.Add(baseType.AttributeType, baseType.Modifier);
            }

            var itemName = string.Format(
                "{0} {1} {2}",
                prefixDesc,
                baseType.Name,
                suffixDesc
            ).Trim();


            return new Item(type, itemName, modifiers, position, baseType.FilePath);
        }

        private static void LoadXml()
        {
            _prefixList = new List<Affix>();
            _suffixList = new List<Affix>();
            _baseTypeList = new List<BaseType>();

            var doc = XDocument.Load("Items.xml");

            ParseAffixes("prefixes", doc, ref _prefixList);
            ParseAffixes("suffixes", doc, ref _suffixList);

            foreach (var baseType in doc.Root.Elements("baseTypes").Elements())
            {
                var attributes = baseType.Attributes();

                ItemType itemType;
                switch (attributes.Where(x => x.Name == "itemType").First().Value)
                {
                    case "HEAD":
                        itemType = ItemType.HEAD;
                        break;
                    case "HAND":
                        itemType = ItemType.HAND;
                        break;
                    case "TORSO":
                        itemType = ItemType.TORSO;
                        break;
                    default:
                    case "FEET":
                        itemType = ItemType.FEET;
                        break;
                }

                AttributeType attributeType;
                switch (attributes.Where(x => x.Name == "attribute").First().Value)
                {
                    default:
                    case "STR":
                        attributeType = AttributeType.STRENGTH;
                        break;
                    case "AGI":
                        attributeType = AttributeType.AGILITY;
                        break;
                    case "INT":
                        attributeType = AttributeType.INTELLIGENCE;
                        break;
                    case "END":
                        attributeType = AttributeType.ENDURANCE;
                        break;
                }

                int modifier = int.Parse(attributes.Where(x => x.Name == "modifier").First().Value);

                var filePath = attributes.Where(x => x.Name == "file").First().Value;

                _baseTypeList.Add(new BaseType(baseType.Value, itemType, filePath, attributeType, modifier));
            }
        }

        private static void ParseAffixes(string path, XDocument doc, ref List<Affix> list)
        {
            foreach (var affix in doc.Root.Elements("affixes").Elements(path).Elements())
            {
                var attributes = affix.Attributes();
                int modifier = int.Parse(attributes.Where(x => x.Name == "modifier").First().Value);
                var description = affix.Value;
                string attributeTypeStr = attributes.Where(x => x.Name == "attribute").First().Value;
                AttributeType attributeType;

                switch (attributeTypeStr)
                {
                    case "STR":
                        attributeType = AttributeType.STRENGTH;
                        break;

                    case "AGI":
                        attributeType = AttributeType.AGILITY;
                        break;

                    case "INT":
                        attributeType = AttributeType.INTELLIGENCE;
                        break;

                    default:
                    case "END":
                        attributeType = AttributeType.ENDURANCE;
                        break;
                }


                list.Add(new Affix(modifier, description, attributeType));
            }
        }

        private class Affix
        {
            public int Modifier { get; set; }
            public string Description { get; set; }
            public AttributeType AttributeType { get; set; }

            public Affix(int modifier, string description, AttributeType attributeType)
            {
                Modifier = modifier;
                Description = description;
                AttributeType = attributeType;
            }
        }

        private class BaseType
        {
            public string Name { get; set; }
            public string FilePath { get; set; }
            public ItemType ItemType { get; set; }
            public AttributeType AttributeType { get; set; }
            public int Modifier { get; set; }

            public BaseType(string name, ItemType itemType, string filePath, AttributeType attributeType, int modifier)
            {
                Name = name;
                ItemType = itemType;
                FilePath = filePath;
                AttributeType = attributeType;
                Modifier = modifier;
            }
        }
    }
}

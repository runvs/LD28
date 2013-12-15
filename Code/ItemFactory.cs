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

        private static Item GetItem(ItemType type, Vector2i position)
        {
            var baseTypes = _baseTypeList.Where(x => x.ItemType == type);
            var baseType = baseTypes.ElementAt(GameProperties.RandomGenerator.Next(baseTypes.Count())).Name;
            var prefix = _prefixList.ElementAt(GameProperties.RandomGenerator.Next(_prefixList.Count));
            var suffix = _suffixList.ElementAt(GameProperties.RandomGenerator.Next(_suffixList.Count));

            var itemName = string.Format(
                "{0} {1} {2}",
                prefix.Description,
                baseType,
                suffix.Description
            ).Trim();

            return new Item(type, itemName, prefix.Modifier + suffix.Modifier, position);
        }

        private static void LoadXml()
        {
            _prefixList = new List<Affix>();
            _suffixList = new List<Affix>();
            _baseTypeList = new List<BaseType>();

            var doc = XDocument.Load("Items.xml");

            foreach (var prefix in doc.Root.Elements("affixes").Elements("prefixes").Elements())
            {
                int modifier = int.Parse(prefix.FirstAttribute.Value);
                var description = prefix.Value;

                _prefixList.Add(new Affix(modifier, description));
            }

            foreach (var suffix in doc.Root.Elements("affixes").Elements("suffixes").Elements())
            {
                int modifier = int.Parse(suffix.FirstAttribute.Value);
                var description = suffix.Value;

                _suffixList.Add(new Affix(modifier, description));
            }

            foreach (var baseType in doc.Root.Elements("baseTypes").Elements())
            {
                ItemType itemType;
                switch (baseType.FirstAttribute.Value)
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

                _baseTypeList.Add(new BaseType(baseType.Value, itemType));
            }
        }

        private class Affix
        {
            public int Modifier { get; set; }
            public string Description { get; set; }

            public Affix(int modifier, string description)
            {
                Modifier = modifier;
                Description = description;
            }
        }

        private class BaseType
        {
            public string Name { get; set; }
            public ItemType ItemType { get; set; }

            public BaseType(string name, ItemType itemType)
            {
                Name = name;
                ItemType = itemType;
            }
        }
    }
}

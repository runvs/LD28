using SFML.Window;
using System.Collections.Generic;
using System.Xml;

namespace JamTemplate
{
    public class MapParser
    {
        public List<Tile> TerrainLayer { get; private set; }
        public List<IGameObject> ObjectLayer { get; private set; }
        public List<Enemy> EnemyLayer { get; private set; }

        public MapParser(string fileName, World world)
        {
            TerrainLayer = new List<Tile>();
            ObjectLayer = new List<IGameObject>();
            EnemyLayer = new List<Enemy>();

            var doc = new XmlDocument();
            doc.Load(fileName);

            int terrainOffset = int.Parse(doc.SelectSingleNode("/map/tileset[@name='Terrain']").Attributes["firstgid"].Value);
            int objectOffset = int.Parse(doc.SelectSingleNode("/map/tileset[@name='Objects']").Attributes["firstgid"].Value);
            int enemyOffset = int.Parse(doc.SelectSingleNode("/map/tileset[@name='Enemies']").Attributes["firstgid"].Value);

            GameProperties.WorldSizeInTiles = int.Parse(doc.SelectSingleNode("/map").Attributes["width"].Value);

            int xPos = 0, yPos = 0;

            // Get the terrain layer tiles
            foreach (XmlNode layerNode in doc.SelectNodes("/map/layer[@name='TerrainLayer']/data/tile"))
            {
                int gid = int.Parse(layerNode.Attributes["gid"].Value) - terrainOffset;
                JamTemplate.Tile.TileType type = Tile.TileType.Grass;
                switch (gid)
                {
                    case 0:
                        type = Tile.TileType.Grass;
                        break;
                    case 1:
                        type = Tile.TileType.Mountain;
                        break;
                    case 2:
                        type = Tile.TileType.Water;
                        break;
                    case 3:
                        type = Tile.TileType.Forest;
                        break;
                }

                TerrainLayer.Add(new Tile(xPos, yPos, type));

                if (xPos != 0 && (xPos + 1) % GameProperties.WorldSizeInTiles == 0)
                {
                    yPos++;
                }
                xPos = (xPos + 1) % GameProperties.WorldSizeInTiles;
            }

            xPos = 0;
            yPos = 0;

            // Get the object layer tiles
            foreach (XmlNode layerNode in doc.SelectNodes("/map/layer[@name='ObjectLayer']/data/tile"))
            {
                int gid = int.Parse(layerNode.Attributes["gid"].Value) - objectOffset;

                switch (gid)
                {
                    case 0:
                        ObjectLayer.Add(new NomadsHouse(xPos, yPos, world));
                        break;
                    case 1:
                        ObjectLayer.Add(new QuestItem(world, 2, new Vector2i(xPos, yPos)));
                        break;
                    case 2:
                        ObjectLayer.Add(new QuestItem(world, 0, new Vector2i(xPos, yPos)));
                        break;
                    case 3:
                        ObjectLayer.Add(new QuestItem(world, 1, new Vector2i(xPos, yPos)));
                        break;
                    case 4:
                        ObjectLayer.Add(new QuestItem(world, 3, new Vector2i(xPos, yPos)));
                        break;
                    case 5:
                        ObjectLayer.Add(new QuestItem(world, 4, new Vector2i(xPos, yPos)));
                        break;
                }

                if (xPos != 0 && (xPos + 1) % GameProperties.WorldSizeInTiles == 0)
                {
                    yPos++;
                }
                xPos = (xPos + 1) % GameProperties.WorldSizeInTiles;
            }

        }
    }
}

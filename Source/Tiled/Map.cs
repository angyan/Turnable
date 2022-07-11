using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Turnable.Tiled
{
    [XmlRoot("map")]
    public class Map
    {
        [XmlAttribute("version")]
        public string Version { get; set; }
        [XmlAttribute("tiledversion")]
        public string TiledVersion { get; set; }
        [XmlAttribute("orientation")]
        public Orientation Orientation { get; set; }
        [XmlAttribute("renderorder")]
        public RenderOrder RenderOrder { get; set; }
        [XmlAttribute("width")]
        public int Width { get; set; }
        [XmlAttribute("height")]
        public int Height { get; set; }
        [XmlAttribute("tilewidth")]
        public int TileWidth { get; set; }
        [XmlAttribute("tileheight")]
        public int TileHeight { get; set; }
        public bool Infinite { get; set; }
        [XmlAttribute("nextlayerid")]
        public int NextLayerId { get; set; }
        [XmlAttribute("nextobjectid")]
        public int NextObjectId { get; set; }
        [XmlElement("layer")]
        public List<Layer> Layers { get; set; }
        [XmlArray(ElementName = "properties")]
        [XmlArrayItem(ElementName = "property")]
        public List<Property> Properties { get; set; }
        [XmlElement("tileset")]
        public List<Tileset> Tilesets { get; set; }

        public static Map Load(string fullPath)
        {
            var directory = Path.GetDirectoryName(fullPath);
            Map map = null;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Map));

            using (var fileStream = new FileStream(fullPath, FileMode.Open))
            {
                map = (Map)xmlSerializer.Deserialize(fileStream);

                // Deserialize any external Tilesets
                // Tiled allows Maps to reference Tilesets that are stored in external files (i.e. not embedded within the Map XML itself)
                foreach (var tileset in map.Tilesets)
                {
                    if (tileset.Source != null)
                    {
                        var tilesetPath = directory + "\\" + tileset.Source;
                        using (var tilesetFileStream = new FileStream(tilesetPath, FileMode.Open))
                        {
                            XmlSerializer tilesetXmlSerializer = new XmlSerializer(typeof(Tileset));
                            var deserializedTileset = (Tileset)tilesetXmlSerializer.Deserialize(tilesetFileStream);

                            // Copy over properties from the deserialized Tileset
                            tileset.Name = deserializedTileset.Name;
                            tileset.TileWidth = deserializedTileset.TileWidth;
                            tileset.TileHeight = deserializedTileset.TileHeight;
                            tileset.TileCount = deserializedTileset.TileCount;
                            tileset.Columns = deserializedTileset.Columns;

                            // Copy over properties from the Image in the deserialized Tileset
                            tileset.Image = new Image();
                            tileset.Image.Source = deserializedTileset.Image.Source;
                            tileset.Image.Height = deserializedTileset.Image.Height;
                            tileset.Image.Width = deserializedTileset.Image.Width;
                        }
                    }
                }
            }

            return map;
        }
    }


}

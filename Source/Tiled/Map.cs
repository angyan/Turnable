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

        public static Map Load(string fullPath)
        {
            FileStream fileStream = new FileStream(fullPath, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Map));

            var map = (Map)xmlSerializer.Deserialize(fileStream);
            fileStream.Close();

            return map;
        }
    }


}

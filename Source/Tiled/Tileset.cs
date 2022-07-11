using Turnable.Tiled;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Turnable.Tiled
{
    [XmlType("tileset")]
    public class Tileset
    {
        [XmlAttribute("firstgid")]
        public int FirstGid { get; set; }
        [XmlAttribute("source")]
        public string Source { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("tilewidth")]
        public int TileWidth { get; set; }
        [XmlAttribute("tileheight")]
        public int TileHeight { get; set; }
        [XmlAttribute("tilecount")]
        public int TileCount { get; set; }
        [XmlAttribute("columns")]
        public int Columns { get; set; }
        [XmlElement("image")]
        public Image Image { get; set; }
    }
}
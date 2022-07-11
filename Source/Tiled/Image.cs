using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Turnable.Tiled
{
    [XmlType("image")]
    public class Image
    {
        [XmlAttribute("source")]
        public string Source { get; set; }
        [XmlAttribute("width")]
        public int Width { get; set; }
        [XmlAttribute("height")]
        public int Height { get; set; }
    }
}

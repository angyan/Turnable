using System.Xml.Serialization;

namespace Turnable.Tiled
{
    public class Layer
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("width")]
        public int Width { get; set; }
        [XmlAttribute("height")]
        public int Height { get; set; }
        [XmlElement("data")]
        public Data Data { get; set; }
        [XmlArray(ElementName = "properties")]
        [XmlArrayItem(ElementName = "property")]
        public List<Property> Properties { get; set; }
    }
}
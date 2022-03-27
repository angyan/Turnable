using System.Xml.Serialization;

namespace Turnable.Tiled
{
    [XmlType("property")]
    public class Property
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
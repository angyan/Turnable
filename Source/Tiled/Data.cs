using System.Xml.Serialization;

namespace Turnable.Tiled
{
    public class Data
    {
        private string _value;

        [XmlText]
        public string Value
        {
            get => _value;
            set => _value = value.Trim();
        }
        [XmlAttribute("encoding")]
        public Encoding Encoding { get; set; }
        [XmlAttribute("compression")]
        public Compression Compression { get; set; }

        public Data()
        {
        }

        public Data(string data) : this()
        {
            Value = data;
        }
    }
}
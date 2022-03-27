using System.Xml.Serialization;

namespace Turnable.Tiled
{
    public enum Compression
    {
        [XmlEnum(Name = "zlib")]
        Zlib,
        [XmlEnum(Name = "gzip")]
        Gzip,
        [XmlEnum(Name = "zstd")]
        Zstd
    }
}
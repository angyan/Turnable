using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Turnable.Tiled
{
    public enum MapOrientation
    {
        [XmlEnum(Name = "orthogonal")]
        Orthogonal,
        [XmlEnum(Name = "isometric")]
        Isometric,
        [XmlEnum(Name = "staggered")]
        Staggered,
        [XmlEnum(Name = "hexagonal")]
        Hexagonal
    }
}

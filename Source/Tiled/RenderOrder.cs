using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Turnable.Tiled
{
    public enum RenderOrder
    {
        [XmlEnum(Name = "right-down")]
        RightDown,
        [XmlEnum(Name = "right-up")]
        RightUp,
        [XmlEnum(Name = "left-down")]
        LeftDown,
        [XmlEnum(Name = "left-up")]
        LeftUp
    }
}

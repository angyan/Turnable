using System.Xml.Serialization;

namespace Turnable.TiledMap
{
    internal record Layer(int[] Data, int Height, int Id, string Name, int Opacity, string Type, bool Visible,
        int Width,
        int X, int Y);
}
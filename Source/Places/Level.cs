using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turnable.Tiled;

namespace ExtensionMethods
{
    public static class EnumerableExtensions
    {
        // REF: https://thomaslevesque.com/2019/11/18/using-foreach-with-index-in-c/
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}

namespace Turnable.Places
{
    public class Level
    {
        public Map Map { get; set; }
        private readonly Tiles[] _tiles;

        public Level(string fullPath)
        {
            Map = Map.Load(fullPath);
            _tiles = new Tiles[Map.Layers.Count];
            foreach (var (layer, index) in Map.Layers.WithIndex<Layer>())
            {
                _tiles[index] = new Tiles(layer.Width, layer.Height, layer.Data);
            }
        }

        public uint this[int x, int y, int z]
        {
            get
            {
                return _tiles[z][x, y];
            }

            set
            {

            }
        }
    }
}

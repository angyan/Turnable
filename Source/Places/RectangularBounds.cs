using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turnable.Places
{
    internal class RectangularBounds : IBounds
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public RectangularBounds(int width, int height) { Width = width; Height = height; }
    }
}

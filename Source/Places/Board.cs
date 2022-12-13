using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turnable.Places
{
    internal class Board
    {
        public IBounds Bounds { get; set; }

        public Board(int length)
        {
            Bounds = new SquareBounds(length);
        }

        public Board(int width, int height)
        {
            Bounds = new RectangularBounds(width, height);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turnable.Places
{
    internal class SquareBounds : IBounds
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public SquareBounds(int length) { Width = length; Height = length; }
    }
}

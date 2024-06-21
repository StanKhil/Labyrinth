using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Points
{
    public class Point : Coord
    {
        public Coord cord;
        public int dir;
        public Point()
        {
            cord = new Coord();
        }
    }
}

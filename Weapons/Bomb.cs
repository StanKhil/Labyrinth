using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labyrinth.Points;

namespace Labyrinth.Weapons
{
    public class Bomb
    {
        static Coord bombCoord = new Coord();
        static bool bombFlag = false;
        static int bombCount = 3;
        static int bomb_radius = 2;
        public int getBombRadius() { return bomb_radius; }
        public int getBombCount() { return bombCount; }
        public bool getBombFlag() { return bombFlag; }

        public Coord getBombCoord() { return bombCoord; }

        public void setBombRadius(int x) { bomb_radius = x; }

        public void setBombCoord(int x, int y) { bombCoord.x = x; bombCoord.y = y; }

        public void setBombCount(int x) { bombCount = x; }

        public void setBombFlag(bool x) { bombFlag = x; }

    }
}

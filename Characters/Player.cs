using Labyrinth.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Characters
{
    class Player
    {
        static Coord playerCoord = new Coord();
        static int money = 0;
        static int lives = 3;
        static int energy = 500;

        public Coord getPlayerCoord() { return playerCoord; }
        public int getMoney() { return money; }
        public int getLives() { return lives; }
        public int getEnergy() { return energy; }
        public void setPlayerCoordX(int x) { playerCoord.x = x; }
        public void setPlayerCoordY(int y) { playerCoord.y = y; }
        public void setMoney(int x) { money = x; }
        public void setLives(int x) { lives = x; }
        public void setEnergy(int x) { energy = x; }
    }
}

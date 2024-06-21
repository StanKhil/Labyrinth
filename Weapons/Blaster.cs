using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labyrinth;

namespace Labyrinth.Weapons
{
    internal class Blaster : Weapon
    {
        public Blaster()
        {
            cost = 20;
            purchased = false;
            energy = 20;
        }

        public int getCost() { return cost; }

        public bool getPurchased() { return purchased; }

        public int getEnergy() { return energy; }

        public void setCost(int x) { cost = x; }

        public void setPurchased(bool x) { purchased = x; }

        public void setEnergy(int x) { energy = x; }
    }
}

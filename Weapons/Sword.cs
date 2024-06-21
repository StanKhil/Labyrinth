using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Labyrinth.Weapons
{
    internal class Sword : Weapon
    {
        public Sword()
        {
            cost = 10;
            purchased = false;
            energy = 10;
        }

        public int getCost() { return cost; }

        public bool getPurchased() { return purchased; }

        public int getEnergy() { return energy; }

        public void setCost(int x) { cost = x; }

        public void setPurchased(bool x) { purchased = x; }

        public void setEnergy(int x) { energy = x; }
    }
}

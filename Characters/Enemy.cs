using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labyrinth.Points;

namespace Labyrinth.Characters
{
    public class Enemy
    {
        static int enemyCount = 0;
        static Point[] enemy = new Point[100];

        public int getEnemyCount() { return enemyCount; }

        public void setEnemyCount(int x) { enemyCount = x; }

        public Point getEnemyElement(int i) { return enemy[i]; }

        public void setEnemyElement(int i, Point x) { enemy[i] = x; }
    }
}

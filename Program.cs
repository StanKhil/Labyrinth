using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Xml.Schema;
using Labyrinth.Characters;
using Labyrinth.Points;
using Labyrinth.Weapons;
using System.Media;

class Sample
{
    static int width = 70;
    static int height = 20;

    static Bomb bomb=new Bomb();
    static Player player = new Player();
    static Enemy enemy = new Enemy();

    static Sword sword = new Sword();
    static Blaster blaster = new Blaster();
    
    static int[,] maze = new int[height, width];
    static int money_total = 0;
    static ConsoleKeyInfo k;

    static void ShowMoney()
    {
        Console.SetCursorPosition(width + 5, 5);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("$ -                                          ");

        Console.SetCursorPosition(width + 5, 5);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("$ - " + player.getMoney() + "   $$$ - " + money_total);
    }

    static void ShowLives()
    {
        Console.SetCursorPosition(width + 5, 7);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write((char)1 + " -    ");

        Console.SetCursorPosition(width + 5, 7);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write((char)1 + " - " + player.getLives());
    }

    static void ShowBombs()
    {
        Console.SetCursorPosition(width + 5, 9);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("* -    ");

        Console.SetCursorPosition(width + 5, 9);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("* - " + bomb.getBombCount());

        if (bomb.getBombRadius() == 2)Console.Write("   2x2       ");
        else if (bomb.getBombRadius() == 3)Console.Write("   3x3       ");
        else if (bomb.getBombRadius() == 4)Console.Write("   4x4       ");
        else if (bomb.getBombRadius() == 5)Console.Write("   5x5 (max)");
    }

    static void ShowEnemy()
    {
        Console.SetCursorPosition(width + 5, 11);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write((char)1 + " -    ");

        Console.SetCursorPosition(width + 5, 11);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write((char)1 + " - " + enemy.getEnemyCount());
    }

    static void ShowEnergy()
    {
        Console.SetCursorPosition(width + 5, 13);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("E" + " -    ");

        Console.SetCursorPosition(width + 5, 13);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("E" + " - " + player.getEnergy());
    }

    public static void BombExplode(object source, ElapsedEventArgs e)
    {
        bomb.setBombFlag(false);
        Console.BackgroundColor = ConsoleColor.DarkRed;         /// explode color                                                                           
        Console.SetCursorPosition(bomb.getBombCoord().x, bomb.getBombCoord().y);
        Console.Write(" ");
       
        maze[bomb.getBombCoord().x, bomb.getBombCoord().y] = 0;
        for (int n = -bomb.getBombRadius(); n <= bomb.getBombRadius(); n++)
        {
            if (n == 0) continue;
            if(bomb.getBombCoord().x + n >= width- 1 || bomb.getBombCoord().x + n <=0)break;
            if ((bomb.getBombCoord().x + n) > 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;         /// explode color
                Console.SetCursorPosition(bomb.getBombCoord().x + n, bomb.getBombCoord().y);
                Console.Write(" ");

                if (maze[bomb.getBombCoord().y, (bomb.getBombCoord().x + n)] == 2)                    // check if money
                {
                    money_total--;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Sample.ShowMoney();
                    Thread.Sleep(20);
                }

                else if (maze[bomb.getBombCoord().y, (bomb.getBombCoord().x + n)] == 3)                    // check if enemy
                {
                    for (int i = 0; i < enemy.getEnemyCount(); i++)
                    {
                        if ((bomb.getBombCoord().x + n) == enemy.getEnemyElement(i).cord.x && bomb.getBombCoord().y ==enemy.getEnemyElement(i).cord.y)
                        {
                            enemy.getEnemyElement(i).cord.x = enemy.getEnemyElement(enemy.getEnemyCount() - 1).cord.x;
                            enemy.getEnemyElement(i).cord.y = enemy.getEnemyElement(enemy.getEnemyCount() - 1).cord.y;
                            break;
                        }
                    }

                    enemy.setEnemyCount(enemy.getEnemyCount() - 1);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Sample.ShowEnemy();
                }

                maze[bomb.getBombCoord().y, bomb.getBombCoord().x + n] = 0;
            }
        }
        for (int n = -bomb.getBombRadius(); n <= bomb.getBombRadius(); n++)
        {
            if (n == 0) continue;
            if (bomb.getBombCoord().y + n >= width - 1 || bomb.getBombCoord().y + n <= 0) break;
            if ((bomb.getBombCoord().y + n) > 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;         /// explode color
                Console.SetCursorPosition(bomb.getBombCoord().x, bomb.getBombCoord().y + n);
                Console.Write(" ");

                if (maze[(bomb.getBombCoord().y + n), bomb.getBombCoord().x] == 2)
                {
                    money_total--;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Sample.ShowMoney();
                    Thread.Sleep(20);
                }

                else if (maze[(bomb.getBombCoord().y + n), bomb.getBombCoord().x] == 3)                    // check if enemy
                {
                    for (int i = 0; i < enemy.getEnemyCount(); i++)
                    {
                        if (bomb.getBombCoord().x == enemy.getEnemyElement(i).cord.x && (bomb.getBombCoord().y + n) == enemy.getEnemyElement(i).cord.y)
                        {
                            enemy.getEnemyElement(i).cord.x = enemy.getEnemyElement(enemy.getEnemyCount() + n).cord.x;
                            enemy.getEnemyElement(i).cord.y = enemy.getEnemyElement(enemy.getEnemyCount() + n).cord.y;
                            break;
                        }
                    }

                    enemy.setEnemyCount(enemy.getEnemyCount() - 1);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Sample.ShowEnemy();
                }

                maze[bomb.getBombCoord().y + n, bomb.getBombCoord().x] = 0;
            }
        }

        Thread.Sleep(150);
        Console.BackgroundColor = ConsoleColor.Black;

        Console.SetCursorPosition(bomb.getBombCoord().x, bomb.getBombCoord().y);
        Console.Write(" ");


        for (int n = -bomb.getBombRadius(); n <= bomb.getBombRadius(); n++)
        {
            if (bomb.getBombCoord().x + n > 0)
            {
                Console.SetCursorPosition(bomb.getBombCoord().x + n, bomb.getBombCoord().y);
                Console.Write(" ");
            }
        }
        for (int n = -bomb.getBombRadius(); n <= bomb.getBombRadius(); n++)
        {
            if (bomb.getBombCoord().y + n > 0)
            {
                Console.SetCursorPosition(bomb.getBombCoord().x, bomb.getBombCoord().y+n);
                Console.Write(" ");
            }
        }



        ///////////////////////  CHECK IF PLAYER  //////////////////////////////////////////////////////////////////

        for (int n = -bomb.getBombRadius(); n <= bomb.getBombRadius(); n++)
        {
            if (n == 0) continue;
            if(bomb.getBombCoord().x== player.getPlayerCoord().x && bomb.getBombCoord().y + n == player.getPlayerCoord().y)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Sample.Died();
            }
            
        }
        for (int n = -bomb.getBombRadius(); n <= bomb.getBombRadius(); n++)
        {
            if (n == 0) continue;
            if (bomb.getBombCoord().y == player.getPlayerCoord().y && bomb.getBombCoord().x + n == player.getPlayerCoord().x)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Sample.Died();
            }

        }
        maze[bomb.getBombCoord().y, bomb.getBombCoord().x] = 0;
    }

    static void Died()
    {
        player.setPlayerCoordX(1);
        player.setPlayerCoordY(1);
        player.setLives(player.getLives() - 1);
        ShowLives();
        bomb.setBombRadius(5);
        ShowBombs();

        Console.SetCursorPosition(player.getPlayerCoord().x, player.getPlayerCoord().y);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write((char)1);

        Console.SetCursorPosition(width + 5, 20);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("You died!!!");

        Thread.Sleep(2000);

        Console.SetCursorPosition(width + 5, 20);
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write("           ");
    }

    static void Attack(Weapon weapon)
    {
        if(weapon.GetType() == typeof(Sword)) {
            for (int n = -1; n < 2; n++)
            {
                for (int m = -1; m < 2; m++)
                {
                    if (maze[player.getPlayerCoord().y + n, (player.getPlayerCoord().x+m)] == 3)                    // check if enemy
                    {
                        for (int i = 0; i < enemy.getEnemyCount(); i++)
                        {
                            if ((player.getPlayerCoord().x+m) == enemy.getEnemyElement(i).cord.x && player.getPlayerCoord().y + n == enemy.getEnemyElement(i).cord.y)
                            {
                                enemy.getEnemyElement(i).cord.x =999 /*enemy.getEnemyElement(enemy.getEnemyCount() - 1).cord.x*/;
                                enemy.getEnemyElement(i).cord.y =999 /*enemy.getEnemyElement(enemy.getEnemyCount() - 1).cord.y*/;
                                maze[player.getPlayerCoord().y+n, (player.getPlayerCoord().x + m)] = 0;
                                break;
                            }
                        }

                        enemy.setEnemyCount(enemy.getEnemyCount() - 1);
                        maze[player.getPlayerCoord().y+n, (player.getPlayerCoord().x + m)] = 0;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Sample.ShowEnemy();
                    }
                }
            }
        }
        else
        {
            Random r = new Random();
            int mode = r.Next(0, 2);

            if (mode == 1)
            {
                int n = 1;
                bool right = false;
                bool left = false;
                while (true)
                {
                    int playerX = player.getPlayerCoord().x;
                    int playerY = player.getPlayerCoord().y;

                    if (playerX + n < maze.GetLength(1) && maze[playerY, playerX + n] != 1)
                    {
                        if (maze[playerY, playerX + n] == 3)
                        {
                            for (int i = 0; i < enemy.getEnemyCount(); i++)
                            {
                                if ((playerX + n) == enemy.getEnemyElement(i).cord.x && playerY == enemy.getEnemyElement(i).cord.y)
                                {
                                    enemy.getEnemyElement(i).cord.x = 999;
                                    enemy.getEnemyElement(i).cord.y = 999;
                                    maze[playerY, playerX + n] = 0;
                                    break;
                                }
                            }
                            maze[playerY, playerX + n] = 0;
                            enemy.setEnemyCount(enemy.getEnemyCount() - 1);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Sample.ShowEnemy();
                        }
                        Console.SetCursorPosition(playerX + n, playerY);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("-");
                        Thread.Sleep(20);
                        Console.Write(" ");
                        maze[playerY, playerX + n] = 0;
                    }
                    else right = true;

                    if (playerX - n >= 0 && maze[playerY, playerX - n] != 1)
                    {
                        if (maze[playerY, playerX - n] == 3)
                        {
                            for (int i = 0; i < enemy.getEnemyCount(); i++)
                            {
                                if ((playerX - n) == enemy.getEnemyElement(i).cord.x && playerY == enemy.getEnemyElement(i).cord.y)
                                {
                                    enemy.getEnemyElement(i).cord.x = 999;
                                    enemy.getEnemyElement(i).cord.y = 999;
                                    maze[playerY, playerX - n] = 0;
                                    break;
                                }
                            }
                            maze[playerY, playerX - n] = 0;
                            enemy.setEnemyCount(enemy.getEnemyCount() - 1);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Sample.ShowEnemy();
                        }
                        Console.SetCursorPosition(playerX - n, playerY);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("-");
                        Thread.Sleep(20);
                        Console.Write(" ");
                        maze[playerY, playerX - n] = 0;
                    }
                    else left = true;

                    if (right && left) break;
                    n++;
                }
            }
            else
            {
                int n = 1;
                bool up = false;
                bool down = false;
                while (true)
                {
                    int playerX = player.getPlayerCoord().x;
                    int playerY = player.getPlayerCoord().y;

                    if (playerY + n < maze.GetLength(0) && maze[playerY + n, playerX] != 1)
                    {
                        if (maze[playerY + n, playerX] == 3)
                        {
                            for (int i = 0; i < enemy.getEnemyCount(); i++)
                            {
                                if (playerX == enemy.getEnemyElement(i).cord.x && (playerY + n) == enemy.getEnemyElement(i).cord.y)
                                {
                                    enemy.getEnemyElement(i).cord.x = 999;
                                    enemy.getEnemyElement(i).cord.y = 999;
                                    maze[playerY + n, playerX] = 0;
                                    break;
                                }
                            }
                            maze[playerY + n, playerX] = 0;
                            enemy.setEnemyCount(enemy.getEnemyCount() - 1);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Sample.ShowEnemy();
                        }
                        Console.SetCursorPosition(playerX, playerY + n);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("|");
                        Thread.Sleep(20);
                        Console.Write(" ");
                        maze[playerY + n, playerX] = 0;
                    }
                    else down = true;

                    if (playerY - n >= 0 && maze[playerY - n, playerX] != 1)
                    {
                        if (maze[playerY - n, playerX] == 3)
                        {
                            for (int i = 0; i < enemy.getEnemyCount(); i++)
                            {
                                if (playerX == enemy.getEnemyElement(i).cord.x && (playerY - n) == enemy.getEnemyElement(i).cord.y)
                                {
                                    enemy.getEnemyElement(i).cord.x = 999;
                                    enemy.getEnemyElement(i).cord.y = 999;
                                    maze[playerY - n, playerX] = 0;
                                    break;
                                }
                            }
                            maze[playerY - n, playerX] = 0;
                            enemy.setEnemyCount(enemy.getEnemyCount() - 1);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Sample.ShowEnemy();
                        }
                        Console.SetCursorPosition(playerX, playerY - n);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("-");
                        Thread.Sleep(20);
                        Console.Write(" ");
                        maze[playerY - n, playerX] = 0;
                    }
                    else up = true;

                    if (up && down) break;
                    n++;
                }
            }
        }   
    }

    public static void Main()
    {
        Console.SetWindowSize(80, 35);
        Console.BufferWidth = 100;
        Console.BufferHeight = 50;

        Console.Title = "Лабиринт";
        Console.CursorVisible = false;

        player.setPlayerCoordX(1);
        player.setPlayerCoordY(1);
        Random r = new Random();
        player.setMoney(100);
        /////////////////  DRAW MAZE  /////////////////////////////////////////////////////////////////////////////
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int mode = r.Next(0, 101);
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1) { maze[y, x] = 1; mode = 1; }
                //else maze[y, x] = r.Next(0, 5);
                
                if (x == 1 && y == 1) maze[y, x] = 7;

                if (maze[y, x] == 7)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write((char)1);
                }
                else if (mode >= 1 && mode <= 35) // если стенка
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(Convert.ToChar(0x2593));
                    maze[y, x] = 1;
                }
                else if (mode > 40 && mode <= 60) // если монетка
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("$");
                    money_total++;
                    maze[y, x] = 2;
                }
                else if (mode > 60 && mode <= 75) // если враг
                {

                    if (r.Next(0, 15) == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write((char)1);
                        maze[y, x] = 3;
                    }
                    else
                    {
                        maze[y, x] = 0;
                        Console.Write(" ");
                    }
                }
                else if (mode > 75 && mode <= 78)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("+");
                    maze[y, x] = 4;
                }
                else if(mode>35 && mode <= 40)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("E");
                    maze[y, x] = 5;
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }

        //////////////////////////////////////////  ENEMY QUANT /////////////////////////////////////////////////////

        for (int j = 0; j < height; j++)
            for (int i = 0; i < width; i++)
                if (maze[j, i] == 3) enemy.setEnemyCount(enemy.getEnemyCount() + 1);


        int cur = 0; // заполнение информации о координатах и направлении врагов

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (maze[j, i] == 3)
                {
                    enemy.setEnemyElement(cur,new Point());
                    enemy.getEnemyElement(cur).dir = 2;
                    enemy.getEnemyElement(cur).cord.x = i;
                    enemy.getEnemyElement(cur).cord.y = j;
                    cur++;
                }
            }
        }

        ///////////////////////////////////////   SHOW INFO   /////////////////////////////////////////////////////////

        Sample.ShowMoney();
        Sample.ShowLives();
        Sample.ShowBombs();
        Sample.ShowEnemy();
        Sample.ShowEnergy();

        /* BUY bomb.getBombCount() TEXT*/
        Console.SetCursorPosition(5, height + 3);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("PRESS '");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Z");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("' TO BUY bomb.getBombCount() (");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("10$");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" FOR ONE)");

        /* BUY bomb.getBombCount() UPGRADE TEXT*/
        Console.SetCursorPosition(5, height + 5);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("PRESS '");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("X");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("' TO BUY BOMB UPGRADE (");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("15$");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" FOR ONE)");

        /* BUY LIFE TEXT*/
        Console.SetCursorPosition(5, height + 7);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("PRESS '");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("C");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("' TO BUY LIFE (");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("20$");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" FOR ONE)");



        Console.SetCursorPosition(5, height + 11);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("PRESS '");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("E");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("' TO BUY Sword (");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("10$");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(")");


        Console.SetCursorPosition(40, height + 11);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("PRESS '");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("W");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("' TO BUY Blaster (");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("20$");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(")");

        /* SET BOMB TEXT*/
        Console.SetCursorPosition(5, height + 9);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("PRESS '");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("SPACE");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("' TO SET BOMB");


        Console.SetCursorPosition(5, height + 13);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("PRESS '");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Q");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("' TO USE SWORD");


        Console.SetCursorPosition(40, height + 13);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("PRESS '");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("A");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("' TO USE BLASTER");


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        while (true)
        {
            if (player.getLives() == 0)         // END GAME
            {
                Console.SetCursorPosition(width + 5, 20);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("YOU LOSE, GOOD BUY!!!   ");
                Environment.Exit(0);

            }

            if (player.getEnergy() == 0)         // END GAME
            {
                Console.SetCursorPosition(width + 5, 20);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("YOU LOSE, GOOD BUY!!!   ");
                Environment.Exit(0);

            }

            if (enemy.getEnemyCount() == 0)         // WIN GAME
            {
                Console.SetCursorPosition(width + 5, 20);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("YOU WIN, THANK YOU!!!   ");

                Environment.Exit(0);
            }

            if (money_total == 0)         // WIN GAME
            {
                Console.SetCursorPosition(width + 5, 20);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("YOU WIN, THANK YOU!!!   ");
                Environment.Exit(0);
            }

            if (Console.KeyAvailable)
            {
                if (bomb.getBombFlag() == true);
                k = Console.ReadKey(true);

                Console.SetCursorPosition(player.getPlayerCoord().x, player.getPlayerCoord().y);

                if (maze[player.getPlayerCoord().y, player.getPlayerCoord().x] != 999)  // if bomb
                    Console.Write(" ");

                /////////////////////////////////////////////  ARROW KEYS ////////////////////////////////////////////////////////////

                if (k.Key == ConsoleKey.DownArrow && maze[player.getPlayerCoord().y + 1, player.getPlayerCoord().x] != 1 && maze[player.getPlayerCoord().y + 1, player.getPlayerCoord().x] != 999)
                {
                    if (maze[player.getPlayerCoord().y + 1, player.getPlayerCoord().x] == 2)
                    {
                        player.setMoney(player.getMoney() + 1);
                        money_total--;
                        Sample.ShowMoney();
                    }
                    else if(maze[player.getPlayerCoord().y + 1, player.getPlayerCoord().x] == 4)
                    {
                        player.setLives(player.getLives() + 1);
                        Sample.ShowLives();
                    }
                    else if(maze[player.getPlayerCoord().y + 1, player.getPlayerCoord().x] == 5)
                    {
                        player.setEnergy(player.getEnergy() + 11);
                        Sample.ShowEnergy();
                    }

                    player.setPlayerCoordY(player.getPlayerCoord().y + 1);
                    player.setEnergy(player.getEnergy() - 1);
                    Sample.ShowEnergy();
                }

                else if (k.Key == ConsoleKey.UpArrow && maze[player.getPlayerCoord().y - 1, player.getPlayerCoord().x] != 1 && maze[player.getPlayerCoord().y - 1, player.getPlayerCoord().x] != 999)
                {
                    if (maze[player.getPlayerCoord().y - 1, player.getPlayerCoord().x] == 2)
                    {
                        player.setMoney(player.getMoney() + 1);
                        money_total--;
                        Sample.ShowMoney();
                    }
                    else if (maze[player.getPlayerCoord().y - 1, player.getPlayerCoord().x] == 4)
                    {
                        player.setLives(player.getLives() + 1);
                        Sample.ShowLives();
                    }
                    else if (maze[player.getPlayerCoord().y - 1, player.getPlayerCoord().x] == 5)
                    {
                        player.setEnergy(player.getEnergy() + 11);
                        Sample.ShowEnergy();
                    }

                    player.setPlayerCoordY(player.getPlayerCoord().y-1);
                    player.setEnergy(player.getEnergy() - 1);
                    Sample.ShowEnergy();
                }

                else if (k.Key == ConsoleKey.LeftArrow && maze[player.getPlayerCoord().y, player.getPlayerCoord().x - 1] != 1 && maze[player.getPlayerCoord().y, player.getPlayerCoord().x - 1] != 999)
                {
                    if (maze[player.getPlayerCoord().y, player.getPlayerCoord().x - 1] == 2)
                    {
                        player.setMoney(player.getMoney()+1);
                        money_total--;
                        Sample.ShowMoney();
                    }
                    else if (maze[player.getPlayerCoord().y, player.getPlayerCoord().x -1 ] == 4)
                    {
                        player.setLives(player.getLives() + 1);
                        Sample.ShowLives();
                    }
                    else if (maze[player.getPlayerCoord().y, player.getPlayerCoord().x -1 ] == 5)
                    {
                        player.setEnergy(player.getEnergy() + 11);
                        Sample.ShowEnergy();
                    }

                    player.setPlayerCoordX(player.getPlayerCoord().x - 1);
                    player.setEnergy(player.getEnergy() - 1);
                    Sample.ShowEnergy();
                }

                else if (k.Key == ConsoleKey.RightArrow && maze[player.getPlayerCoord().y, player.getPlayerCoord().x + 1] != 1 && maze[player.getPlayerCoord().y, player.getPlayerCoord().x + 1] != 999)
                {
                    if (maze[player.getPlayerCoord().y, player.getPlayerCoord().x + 1] == 2)
                    {
                        player.setMoney(player.getMoney() + 1);
                        money_total--;
                        Sample.ShowMoney();
                    }
                    else if (maze[player.getPlayerCoord().y, player.getPlayerCoord().x + 1] == 4)
                    {
                        player.setLives(player.getLives() + 1);
                        Sample.ShowLives();
                    }
                    else if (maze[player.getPlayerCoord().y, player.getPlayerCoord().x + 1] == 5)
                    {
                        player.setEnergy(player.getEnergy() + 11);
                        Sample.ShowEnergy();
                    }

                    player.setPlayerCoordX(player.getPlayerCoord().x + 1);
                    player.setEnergy(player.getEnergy() - 1);
                    Sample.ShowEnergy();
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                Console.SetCursorPosition(player.getPlayerCoord().x, player.getPlayerCoord().y);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write((char)1);

                //////////////////////////////////// BUY KEYS ///////////////////////////////////////////////////////////////////////

                if (k.Key == ConsoleKey.Spacebar)   // bomb.getBombCount()
                {
                    if (bomb.getBombCount() != 0 && bomb.getBombFlag() != true)
                    {
                        bomb.setBombCount(bomb.getBombCount() - 1);
                        Sample.ShowBombs();

                        Console.SetCursorPosition(player.getPlayerCoord().x, player.getPlayerCoord().y);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("*");

                        maze[player.getPlayerCoord().y, player.getPlayerCoord().x] = 999;
                        bomb.setBombFlag(true);
                        bomb.getBombCoord().x = player.getPlayerCoord().x;
                        bomb.getBombCoord().y = player.getPlayerCoord().y;

                        System.Timers.Timer t = new System.Timers.Timer();
                        t.AutoReset = false;
                        t.Interval = 3000;
                        t.Elapsed += new ElapsedEventHandler(BombExplode);
                        t.Start();
                    }
                }

                else if (k.Key == ConsoleKey.Z)   // BUY bomb.getBombCount()
                {
                    if ((player.getMoney() - 10) >= 0)
                    {
                        player.setMoney(player.getMoney() - 10);
                        bomb.setBombCount(bomb.getBombCount() + 1);
                        ShowMoney();
                        ShowBombs();

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("$$$ BOMB + 1 $$$ ");

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                ");
                    }
                    else
                    {
                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("NOT ENOUGH");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" MONEY");

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                ");
                    }
                }

                else if (k.Key == ConsoleKey.X)   // BUY BOMB UPGRADE
                {
                    if ((player.getMoney() - 15) >= 0 && bomb.getBombRadius() != 5)
                    {
                        player.setMoney(player.getMoney() - 15);
                        bomb.setBombRadius(bomb.getBombRadius() + 1);
                        ShowMoney();
                        ShowBombs();

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("$$$ BOMB UPGRADE + 1 $$$ ");

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                           ");
                    }
                    else
                    {
                        if (bomb.getBombRadius() == 5)
                        {
                            Console.SetCursorPosition(width + 5, 15);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("MAX RADIUS");
                        }
                        else
                        {
                            Console.SetCursorPosition(width + 5, 15);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("NOT ENOUGH");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(" MONEY");
                        }

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                ");
                    }
                }

                else if (k.Key == ConsoleKey.C)   // BUY LIFE
                {
                    if ((player.getMoney() - 20) >= 0)
                    {
                        player.setMoney(player.getMoney() - 20);
                        player.setLives(player.getLives() + 1);
                        ShowMoney();
                        ShowLives();

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("$$$ LIFE + 1 $$$ ");

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                ");
                    }
                    else
                    {
                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("NOT ENOUGH");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" MONEY");

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                ");
                    }
                }

                else if (k.Key == ConsoleKey.E)
                {
                    if ((player.getMoney() - 10) >= 0 && !sword.getPurchased())
                    {
                        player.setMoney(player.getMoney() - 10);
                        sword.setPurchased(true);
                        ShowMoney();


                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("$$$ SWORD $$$ ");

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                ");
                    }
                    else if (sword.getPurchased())
                    {
                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("YOU ALREADY");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" BOUGHT SWORD");

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                ");
                    }
                    else
                    {
                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("NOT ENOUGH");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" MONEY");

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                ");
                    }
                }

                else if (k.Key == ConsoleKey.W)
                {
                    if (player.getMoney() - 20 >= 0 && !blaster.getPurchased())
                    {
                        player.setMoney(player.getMoney() - 20);
                        blaster.setPurchased(true);
                        ShowMoney();


                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("$$$ BLASTER $$$ ");

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                ");
                    }
                    else if (blaster.getPurchased())
                    {
                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("YOU ALREADY");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" BOUGHT BLASTER");

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                ");
                    }
                    else
                    {
                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("NOT ENOUGH");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" MONEY");

                        Thread.Sleep(600);

                        Console.SetCursorPosition(width + 5, 15);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("                ");
                    }
                }

                else if (k.Key == ConsoleKey.Q)
                {
                    if (sword.getPurchased())
                    {
                        if (player.getEnergy() > sword.getEnergy())
                        {
                            player.setEnergy(player.getEnergy() - sword.getEnergy());
                            ShowEnergy();
                            Attack(sword);
                        }
                        else
                        {
                            Console.SetCursorPosition(width + 5, 15);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("NOT ENOUGH");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(" ENERGY");

                            Thread.Sleep(600);

                            Console.SetCursorPosition(width + 5, 15);
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("                ");
                        }
                    }
                }

                else if (k.Key == ConsoleKey.A)
                {
                    if (blaster.getPurchased())
                    {
                        if (player.getEnergy() > blaster.getEnergy())
                        {
                            player.setEnergy(player.getEnergy() - blaster.getEnergy());
                            ShowEnergy();
                            Attack(blaster);
                        }
                        else
                        {
                            Console.SetCursorPosition(width + 5, 15);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("NOT ENOUGH");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(" ENERGY");

                            Thread.Sleep(600);

                            Console.SetCursorPosition(width + 5, 15);
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("                ");
                        }
                    }
                }

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }

            ///////////////////////////////////////////////////// ENEMY MOVES //////////////////////////////////////////////////////////////

            else
            {
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        if (maze[j, i] == 3)
                        {
                            int num = -1;
                            for (int q = 0; q < enemy.getEnemyCount(); q++)
                            {
                                if (enemy.getEnemyElement(q).cord.x == i && enemy.getEnemyElement(q).cord.y == j) { num = q; break; }
                            }
                            if (num < 0) continue;

                            if (enemy.getEnemyElement(num).cord.y == player.getPlayerCoord().y && enemy.getEnemyElement(num).cord.x == player.getPlayerCoord().x)
                            {
                                Sample.Died();
                            }

                            Console.SetCursorPosition(enemy.getEnemyElement(num).cord.x, enemy.getEnemyElement(num).cord.y);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" ");
                            maze[enemy.getEnemyElement(num).cord.y, enemy.getEnemyElement(num).cord.x] = 0;
                            if (enemy.getEnemyElement(num).dir == 1) enemy.getEnemyElement(num).cord.x--;
                            else if (enemy.getEnemyElement(num).dir == 2) enemy.getEnemyElement(num).cord.y--;
                            else if (enemy.getEnemyElement(num).dir == 3) enemy.getEnemyElement(num).cord.x++;
                            else if (enemy.getEnemyElement(num).dir == 4) enemy.getEnemyElement(num).cord.y++;

                            if (maze[enemy.getEnemyElement(num).cord.y, enemy.getEnemyElement(num).cord.x] == 2)
                            {
                                maze[enemy.getEnemyElement(num).cord.y, enemy.getEnemyElement(num).cord.x] = 0;
                                money_total--;
                                Sample.ShowMoney();
                            }

                            if (maze[enemy.getEnemyElement(num).cord.y, enemy.getEnemyElement(num).cord.x] != 0)
                            {
                                if (enemy.getEnemyElement(num).dir == 1) enemy.getEnemyElement(num).cord.x++;
                                else if (enemy.getEnemyElement(num).dir == 2) enemy.getEnemyElement(num).cord.y++;
                                else if (enemy.getEnemyElement(num).dir == 3) enemy.getEnemyElement(num).cord.x--;
                                else if (enemy.getEnemyElement(num).dir == 4) enemy.getEnemyElement(num).cord.y--;
                                enemy.getEnemyElement(num).dir = r.Next(1, 5);

                                maze[enemy.getEnemyElement(num).cord.y, enemy.getEnemyElement(num).cord.x] = 3;
                                Console.SetCursorPosition(enemy.getEnemyElement(num).cord.x, enemy.getEnemyElement(num).cord.y);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write((char)2);
                            }
                            else
                            {
                                Console.SetCursorPosition(enemy.getEnemyElement(num).cord.x, enemy.getEnemyElement(num).cord.y);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write((char)2);
                                maze[enemy.getEnemyElement(num).cord.y, enemy.getEnemyElement(num).cord.x] = 3;
                            }
                            if (enemy.getEnemyElement(num).dir == 3)
                                if (i < width - 1) i++;
                            else if (enemy.getEnemyElement(num).dir == 4)
                                if (j < height - 1) j++;
                        }
                    }
                }
                Thread.Sleep(60); // ENEMY SPEED
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
    }
}
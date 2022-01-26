using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class Map
    {
        /// <summary>
        /// Draws map of the world on screen
        /// </summary>
        /// <param name="player"></param>
        public static void DrawMap(Player player)
        {
            //Get x and y dimensions
            int xDim = World.MAX_WORLD_X;
            int yDim = World.MAX_WORLD_Y;

            //Get player position
            int playerXPos = player.xPos;
            int playerYPos = player.yPos;

            //Set text color
            Console.ForegroundColor = ConsoleColor.Red;

            /*
            //Set what the player can see from current loc
            location[playerXPos, pl].seen = true;
            if (playerX < xDim - 1)
            {
                location[playerX + 1, playerY].seen = true;
            }

            if (playerX > 0)
            {
                location[playerX - 1, playerY].seen = true;
            }

            if (playerY < yDim - 1)
            {
                location[playerX, playerY + 1].seen = true;
            }

            if (playerY > 0)
            {
                location[playerX, playerY - 1].seen = true;
            }
            */

            //Go through all locations
            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++)
                {
                    string locIcon = "  ";

                    //Write player loc
                    if (x == playerXPos && y == playerYPos)
                    {
                        locIcon = "XX";
                    }

                    //Color path tiles
                    if (!World.locations[x,y].IsWall)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    //Write out path title
                    Console.Write(locIcon);
                }
                //Next line
                Console.WriteLine("");
            }
            //Print key
            Console.ResetColor();
            Console.WriteLine("--Key--");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("  ");
            Console.ResetColor();
            Console.WriteLine(": Currently known passable squares");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("++");
            Console.ResetColor();
            Console.WriteLine(": Locations or enemies");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("XX");
            Console.ResetColor();
            Console.WriteLine(": You");
            Console.WriteLine("");
        }
    }
}

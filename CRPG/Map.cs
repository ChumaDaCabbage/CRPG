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
                    drawPoint(x, y, player);
                }
                //Next line
                Console.WriteLine("");
            }
            //Reset
            Console.ResetColor();
        }

        public static void redrawMapPoint(int xPos, int yPos, Player player)
        {
            Console.SetCursorPosition(xPos, yPos);
            drawPoint(xPos, yPos, player);

            Program.SetupWritingLine();
        }

        private static void drawPoint(int x, int y, Player player)
        {
            //Get player position
            int playerXPos = player.xPos;
            int playerYPos = player.yPos;

            //Set text color
            Console.ForegroundColor = ConsoleColor.Red;

            string locIcon = "  ";

            //Write player loc
            if (x == playerXPos && y == playerYPos)
            {
                locIcon = "XX";
            }

            //Color path tiles
            if (!World.locations[x, y].IsWall)
            {
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            //Write out icon
            Console.Write(locIcon);
        }
    }
}

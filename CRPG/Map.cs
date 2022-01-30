using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CRPG
{
    public static class Map
    {
        #region Virtual Terminal Sequences setup
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);
        #endregion

        /// <summary>
        /// Draws map of the world on screen
        /// </summary>
        /// <param name="player"></param>
        public static void DrawMap(Player player)
        {
            //Get x and y dimensions
            int xDim = World.MAX_WORLD_X;
            int yDim = World.MAX_WORLD_Y;

            //Turn of curser visibility
            Console.WriteLine("\x1b[?25l  ");

            //Go through all locations
            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++ )
                {
                    //Calls drawPoint
                    drawPoint(x, y, player);
                }
                //Next line
                Console.WriteLine("");
            }
        }

        public static void redrawMapPoint(int xPos, int yPos, Player player)
        {
            Console.SetCursorPosition(xPos * 2, yPos); //Moves cursor to wanted position on map
            drawPoint(xPos, yPos, player); //Calls drawPoint at wanted position

            //Resets cursor
            Program.SetupWritingLine();
        }

        public static void redrawMapPoint(Point point, Player player)
        {
            Console.SetCursorPosition(point.X * 2, point.Y); //Moves cursor to wanted position on map
            drawPoint(point.X, point.Y, player); //Calls drawPoint at wanted position

            //Resets cursor
            Program.SetupWritingLine();
        }

        private static void drawPoint(int x, int y, Player player)
        {
            //Get player position
            int playerXPos = player.xPos;
            int playerYPos = player.yPos;

            //Sets up default icon
            string locIcon = "  ";

            //Find wanted color
            if (x == playerXPos && y == playerYPos)
            {
                //Leather brown for player
                locIcon = "\x1b[48;2;255;255;255m  ";
            }
            else if (!World.locations[x, y].IsWall)
            {
                //Do darkest if not wall
                //locIcon = "\x1b[48;2;0;0;0m  ";
                locIcon = Lighting.getFloorTileColor(x, y).getExtendedColorsString();
            }
            else
            {
                locIcon = Lighting.getWallTileColor(x, y).getExtendedColorsString();
            }

            //Write out icon
            Console.Write(locIcon);
        }
    }
}

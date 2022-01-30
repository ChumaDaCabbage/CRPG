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
        public static void DrawMap()
        {
            //Get x and y dimensions
            int xDim = World.MAX_WORLD_X;
            int yDim = World.MAX_WORLD_Y;

            //Turn off curser visibility
            Console.WriteLine("\x1b[?25l  ");

            //Go through all locations
            for (int y = 0; y < yDim; y++)
            {
                for (int x = 0; x < xDim; x++ )
                {
                    //Calls drawPoint
                    DrawPoint(x, y);
                }
                //Next line
                Console.WriteLine("");
            }
        }

        public static void RedrawMapPoint(int xPos, int yPos)
        {
            Console.SetCursorPosition(xPos * 2, yPos); //Moves cursor to wanted position on map
            DrawPoint(xPos, yPos); //Calls drawPoint at wanted position

            //Resets cursor
            Program.SetupWritingLine();
        }

        public static void RedrawMapPoint(Point point)
        {
            Console.SetCursorPosition(point.X * 2, point.Y); //Moves cursor to wanted position on map
            DrawPoint(point.X, point.Y); //Calls drawPoint at wanted position

            //Resets cursor
            Program.SetupWritingLine();
        }

        private static void DrawPoint(int x, int y)
        {
            //Get player position
            int playerXPos = Program._player.xPos;
            int playerYPos = Program._player.yPos;

            //Sets up default icon
            string locIcon;

            //Get color
            if(Program._player.CheckForFlare(new Point(x, y))) //Check for flare
            {
                //Color flareRed = new Color(209, 56, 56);
                locIcon = Lighting.GetFlareColor(x, y).GetExtendedColorsString();//flareRed.GetExtendedColorsString();
            }
            else if (x == playerXPos && y == playerYPos) //Check for player
            {
                Color player = new Color(255, 255, 255);
                locIcon = player.GetExtendedColorsString(); ;
            }
            else if (!World.locations[x, y].IsWall) //Check for walls
            {
                locIcon = Lighting.GetFloorTileColor(x, y).GetExtendedColorsString();
            }
            else //Floor
            {
                locIcon = Lighting.GetWallTileColor(x, y).GetExtendedColorsString();
            }

            //Write out icon
            Console.Write(locIcon);
        }

        
    }
}

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
            Program.ResetCursor();
        }

        public static void RedrawMapPoint(Point point)
        {
            Console.SetCursorPosition(point.X * 2, point.Y); //Moves cursor to wanted position on map
            DrawPoint(point.X, point.Y); //Calls drawPoint at wanted position

            //Resets cursor
            Program.ResetCursor();
        }

        private static void DrawPoint(int x, int y)
        {
            //Get player position
            int playerXPos = Program._player.Pos.X;
            int playerYPos = Program._player.Pos.Y;

            //Sets up default icon
            string locIcon;


            if (World.locations[x, y].IfFlare()) //Check for flare
            {
                locIcon = Lighting.GetFlareColor(x, y).GetFullExtendedColorsString();
            }
            else if (x == playerXPos && y == playerYPos) //Check for player
            {
                TileVisuals player = new TileVisuals(new Color(255, 224, 189), "ºº", 120);
                locIcon = player.GetFullExtendedColorsString();
            }
            else if (World.locations[x, y].IfWall()) //Check for walls
            {
                locIcon = Lighting.GetWallTileColor(x, y).GetFullExtendedColorsString();
            }
            else if (World.locations[x, y].IfTorch()) //Torch
            {
                locIcon = Lighting.GetTorchTileColor(x, y).GetFullExtendedColorsString();
            }
            else if (World.locations[x, y].IfEnemy()) //Enemy
            {
                locIcon = Lighting.GetEnemyTileColor(x, y).GetFullExtendedColorsString();
            }
            else //Floor
            {
                if (((Floor)World.locations[x,y]).HasFlare)
                {
                    locIcon = Lighting.GetFloorTileColor(x, y).GetBackgroundColorString() + Lighting.GetFloorTileColor(x, y).GetForgroundColorString() + "▓\x1b[38;2;"+ Lighting.GetFlarePickupColor(x,y).R + ";0;0m║";
                }
                else
                {
                    locIcon = Lighting.GetFloorTileColor(x, y).GetFullExtendedColorsString();
                }
            }

            //Write out icon
            Console.Write(locIcon);
        }

        
    }
}

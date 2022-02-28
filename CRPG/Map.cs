using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class Map
    {
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
            string locIcon = null;


            if (Program._world.locations[x, y].IfFlare()) //Check for flare
            {
                locIcon = Lighting.GetFlareColor(x, y).GetFullExtendedColorsString();
            }
            else if (x == playerXPos && y == playerYPos) //Check for player
            {
                TileVisuals player = new TileVisuals(new Color(255, 224, 189), "ºº", 120);
                locIcon = player.GetFullExtendedColorsString();
            }
            else if (Program._world.locations[x, y].IfWall()) //Check for walls
            {
                locIcon = Lighting.GetWallTileColor(x, y).GetFullExtendedColorsString();
            }
            else if (Program._world.locations[x, y].IfTorch()) //Torch
            {
                locIcon = Lighting.GetTorchTileColor(x, y).GetFullExtendedColorsString();
            }
            else if (Program._world.locations[x, y].IfEnemy()) //Enemy
            {
                locIcon = Lighting.GetEnemyTileColor(x, y).GetFullExtendedColorsString();
            }
            else if (x == 57 && y == 27) //If level end
            {
                locIcon = Lighting.GetExitTileColor(x, y).GetFullExtendedColorsString();
            }
            else if (Program._world.locations[x, y].IfFloor()) //Floor
            {
                if (((Floor)Program._world.locations[x, y]).HasFlare)
                {
                    locIcon = Lighting.GetFloorTileColor(x, y).GetBackgroundColorString() + Lighting.GetFloorTileColor(x, y).GetForgroundColorString() + "▓\x1b[38;2;" + Lighting.GetFlarePickupColor(x, y).R + ";0;0m║";
                }
                else
                {
                    locIcon = Lighting.GetFloorTileColor(x, y).GetFullExtendedColorsString();
                }
            }
            else //If all checks fail (unknown tile)
            {
                //Log error message
                //System.Diagnostics.Debug.Fail($"Unknown Tile at [{x},{y}]", $"\tTile:        [{x},{y}]" +
                //                                                            $"\n\tThis:        {Program._world.locations[x, y]} " +
                //                                                            $"\n\tType:        {((LightSource)Program._world.locations[x, y]).lightType()} " +
                //                                                            $"\n\tLight Level: {Program._world.locations[x, y].CurrentLightLevel} " +
                //                                                            $"\n\tRedLight:    {Program._world.locations[x, y].RedLight}" +
                //                                                            $"\n\tOrangeLight: {Program._world.locations[x, y].OrangeLight}"
                //);     

                //Set broken tile to floor (All broken tiles found have been floors, so this should fix the issue)
                Program._world.locations[x, y] = new Floor();
            }

            //Write out icon if icon found
            if (locIcon != null) Console.Write(locIcon);
        }

        
    }
}

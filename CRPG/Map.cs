using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class Map
    {
        //Holds tiles to not draw over in death animation
        private static List<TileVisuals> forbiddenTiles = new List<TileVisuals>();

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
                //Log error message (Not active for final build)
                //System.Diagnostics.Debug.Fail($"Unknown Tile at [{x},{y}]", $"\tTile:        [{x},{y}]" +
                //                                                            $"\n\tThis:        {Program._world.locations[x, y]} " +
                //                                                            $"\n\tType:        {((LightSource)Program._world.locations[x, y]).lightType()} " +
                //                                                            $"\n\tLight Level: {Program._world.locations[x, y].CurrentLightLevel} " +
                //                                                            $"\n\tRedLight:    {Program._world.locations[x, y].RedLight}" +
                //                                                            $"\n\tOrangeLight: {Program._world.locations[x, y].OrangeLight}"
                //);     

                //Set broken tile to floor (All broken tiles found should have been floors, so this should fix the issue)
                Program._world.locations[x, y] = new Floor();
            }

            //Write out icon if icon found
            if (locIcon != null) Console.Write(locIcon);
        }

        private static TileVisuals GetTileVisuals(int x, int y)
        {
            if (Program._world.locations[x, y].IfFlare()) //Check for flare
            {
                return Lighting.GetFlareColor(x, y);
            }
            else if (x == Program._player.Pos.X && y == Program._player.Pos.Y) //Check for player
            {
                TileVisuals player = new TileVisuals(new Color(255, 224, 189), "ºº", 120);
                return player;
            }
            else if (Program._world.locations[x, y].IfWall()) //Check for walls
            {
                return Lighting.GetWallTileColor(x, y);
            }
            else if (Program._world.locations[x, y].IfTorch()) //Torch
            {
                return Lighting.GetTorchTileColor(x, y);
            }
            else if (Program._world.locations[x, y].IfEnemy()) //Enemy
            {
                return Lighting.GetEnemyTileColor(x, y);
            }
            else if (x == 57 && y == 27) //If level end
            {
                return Lighting.GetExitTileColor(x, y);
            }
            else if (Program._world.locations[x, y].IfFloor()) //Floor
            {
                return Lighting.GetFloorTileColor(x, y);
            }
            else //Broken tile
            {
                return new TileVisuals(new Color(0, 0, 0));
            }

        }

        public static void DrawDeathMap()
        {
            Console.ResetColor(); //Reset color
            DateTime DrawDelay = DateTime.Now; //Will hold delay for drawing lines
            Random r = new Random(); 

            //Fill out forbiddenTiles

            StringPointFinder("▓██   ██▓ ▒█████   █    ██    ▓█████▄  ██▓▓█████ ▓█████▄ ", 31, 10);
            StringPointFinder(" ▒██  ██▒▒██▒  ██▒ ██  ▓██▒   ▒██▀ ██▌▓██▒▓█   ▀ ▒██▀ ██▌", 31, 11);
            StringPointFinder("  ▒██ ██░▒██░  ██▒▓██  ▒██░   ░██   █▌▒██▒▒███   ░██   █▌", 31, 12);
            StringPointFinder("  ░ ▐██▓░▒██   ██░▓▓█  ░██░   ░▓█▄   ▌░██░▒▓█  ▄ ░▓█▄   ▌", 31, 13);
            StringPointFinder("  ░ ██▒▓░░ ████▓▒░▒▒█████▓    ░▒████▓ ░██░░▒████▒░▒████▓ ", 31, 14);
            StringPointFinder("   ██▒▒▒ ░ ▒░▒░▒░ ░▒▓▒ ▒ ▒     ▒▒▓  ▒ ░▓  ░░ ▒░ ░ ▒▒▓  ▒ ", 31, 15);
            StringPointFinder(" ▓██ ░▒░   ░ ▒ ▒░ ░░▒░ ░ ░     ░ ▒  ▒  ▒ ░ ░ ░  ░ ░ ▒  ▒ ", 31, 16);
            StringPointFinder(" ▒ ▒ ░░  ░ ░ ░ ▒   ░░░ ░ ░     ░ ░  ░  ▒ ░   ░    ░ ░  ░ ", 31, 17);
            StringPointFinder(" ░ ░         ░ ░     ░           ░     ░     ░  ░   ░    ", 31, 18);
            StringPointFinder(" ░ ░                           ░                  ░      ", 31, 19);

            //Goes through all y positions and a little extra
            for (int i = 0; i < World.MAX_WORLD_Y * 1.7f; i++)
            {
                //wait  
                while (DateTime.Now < DrawDelay.AddSeconds(0.1)) { }

                //Goes from 0 to i
                for (int y = 0; y <= i; y++)
                {
                    //Goes through all x positions (tiles are 2x wide)
                    for (int x = 0; x < World.MAX_WORLD_X * 2; x++)
                    {
                        //Holds if this tile needs to be drawn
                        bool draw = false;

                        //If within y bounds
                        if (y < World.MAX_WORLD_Y)
                        {
                            if ((x > 60 - ((i - r.Next(0, 2)) - (y + 9)) && x < 60 + ((i - r.Next(0, 2)) - (y + 9))) && (y - i) > -27) //If within the bounds of this spike at 60
                            {
                                draw = true;
                            }
                            else if ((x > 40 - ((i - r.Next(0, 2)) - (y + 3)) && x < 40 + ((i - r.Next(0, 2)) - (y + 3))) && (y - i) > -21) //If within the bounds of this spike at 40
                            {
                                draw = true;
                            }
                            else if ((x > 10 - ((i - r.Next(0, 2)) - (y + 6)) && x < 10 + ((i - r.Next(0, 2)) - (y + 6))) && (y - i) > -24) //If within the bounds of this spike at 10
                            {
                                draw = true;
                            }
                            else if ((x > 85 - ((i - r.Next(0, 2)) - (y + 1)) && x < 85 + ((i - r.Next(0, 2)) - (y + 1))) && (y - i) > -19) //If within the bounds of this spike at 85
                            {
                                draw = true;
                            }
                            else if ((x > 103 - ((i - r.Next(0, 2)) - y) && x < 103 + ((i - r.Next(0, 2)) - y)) && (y - i) > -18) //If within the bounds of this spike at 103
                            {
                                draw = true;
                            }
                        }

                        //If can draw
                        if (draw)
                        {
                            //Holds if this tile is forbidden
                            bool forbidden = false;

                            //Goes through all forbidden tiles
                            foreach (TileVisuals tile in forbiddenTiles)
                            {
                                //If tile pos equals current pos
                                if (tile.Pos.Equals(new Point(x, y)))
                                {
                                    forbidden = true; //Set forbidden to true
                                    Console.SetCursorPosition(tile.Pos.X, tile.Pos.Y); //Set cursor to current pos
                                    Console.Write(tile.GetFullExtendedColorsString()); //Draw forbidden tile
                                    break; //Stop loop
                                }
                            }

                            //If not forbidden
                            if (!forbidden)
                            {
                                Console.SetCursorPosition(x, y); //Go to currnet point
                                Console.Write(new TileVisuals(new Color(209, 56, 56)).GetFullExtendedColorsString()); //Draw red square
                            }
                        }
                        
                    }
                }
                //Get new delay
                DrawDelay = DateTime.Now;
            }

            //Fix cursor pos
            Console.SetCursorPosition(0, 30);
        }

        private static void StringPointFinder(string toDraw, int xStart, int y)
        {
            for (int i = 0; i < toDraw.Length; i++)
            {
                if (toDraw[i] != ' ')
                {
                    TileVisuals tile = new TileVisuals(new Color(209, 56, 56), toDraw[i].ToString(), GetTileVisuals((xStart + i) / 2, y).GetForgroundColor());
                    tile.Pos = new Point(xStart + i, y);
                    forbiddenTiles.Add(tile);
                }
            }
        }
    }
}

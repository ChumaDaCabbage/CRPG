using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class World
    {
        public static readonly string WorldName = "HelloWorld";
        //public static readonly List<Location> Locations = new List<Location>();
        public static Location[,] locations;

        //Setup max world sizes
        public const int MAX_WORLD_X = 60;
        public const int MAX_WORLD_Y = 30;

        //Constructor
        static World()
        {
            WorldSetup();
        }

        private static void WorldSetup()
        {
            //Creates empty array of wanted size
            locations = new Location[MAX_WORLD_X, MAX_WORLD_Y];

            //Goes through all locations
            for (int x = 0; x < MAX_WORLD_X; x++)
            {
                for (int y = 0; y < MAX_WORLD_Y; y++)
                {
                    //If edge set as wall
                    if (x == 0 || x == MAX_WORLD_X - 1 || y == 0 || y == MAX_WORLD_Y - 1)
                    {
                        locations[x, y] = new Location(true);
                    }
                    else //Set as empty
                    {
                        locations[x, y] = new Location();
                    }
                }
            }

            //Manual wall setup:
            locations[5, 1] = new Location(true);
            locations[5, 2] = new Location(true);
            locations[5, 5] = new Location(true);
            locations[5, 6] = new Location(true);
            locations[5, 7] = new Location(true);
            locations[4, 8] = new Location(true);
            locations[5, 8] = new Location(true);
            locations[1, 8] = new Location(true);
        }

        //Returns location object at position in locations array
        public static Location GetLocationByPos(int xPos, int yPos)
        {
            return locations[xPos, yPos];
        }

        //Returns location object at position in locations array using a point
        public static Location GetLocationByPos(Point pos)
        {
            return locations[pos.X, pos.Y];
        }
    }
}

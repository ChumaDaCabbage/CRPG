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
            locations[5, 5] = new Location(true);
        }

        //Returns location object at position in locations array
        public static Location GetLocationByPos(int xPos, int yPos)
        {
            return locations[xPos, yPos];
        }
    }
}

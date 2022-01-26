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
        public const int MAX_WORLD_X = 20;
        public const int MAX_WORLD_Y = 20;

        //Start providing IDs for locations
        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_FOREST_PATH = 2;
        public const int LOCATION_ID_LAB = 3;

        //Constructor
        static World()
        {
            //PopulateLocations();
            WorldSetup();
        }

        private static void WorldSetup()
        {
            locations = new Location[MAX_WORLD_X, MAX_WORLD_Y];
            for (int i = 0; i < MAX_WORLD_X; i++)
            {
                for (int j = 0; j < MAX_WORLD_Y; j++)
                {
                    locations[i, j] = new Location();
                }
            }

            locations[5, 5] = new Location(true);
        }

        public static Location GetLocationByPos(int xPos, int yPos)
        {
            return locations[xPos, yPos];
        }

        /*
        private static void PopulateLocations()
        {
            //Create location objects
            Location home = new Location(LOCATION_ID_HOME, "Home", "Your house is a mess");
            Location forestPath = new Location(LOCATION_ID_FOREST_PATH, "Forest Path", "A wooded path with lots of ferns");
            Location lab = new Location(LOCATION_ID_LAB, "Lab", "A strange smelling lab with potions and rat tails.");

            //Link the locations together
            home.LocationToNorth = forestPath;
            forestPath.LocationToEast = lab;
            lab.LocationToWest = forestPath;
            forestPath.LocationToSouth = home;

            //Create list of locations
            Locations.Add(home);
            Locations.Add(forestPath);
            Locations.Add(lab);

        }

        public static Location GetLocationByID(int id)
        {
            foreach (Location loc in Locations)
            {
                if (loc.ID == id)
                {
                    return loc;
                }
            }
            return null;
        }


        public static void ListLocations()
        {
            Console.WriteLine("These are the locations in the world:");
            foreach (Location loc in Locations)
            {
                Console.WriteLine("\t{0}", loc.Name);
            }
        }
        */
    }
}

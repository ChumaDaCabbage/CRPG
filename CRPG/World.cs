using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class World
    {
        public static Location[,] locations;
        public static List<Torch> _tourches = new List<Torch>();
        public static List<Enemy> _enemies = new List<Enemy>();

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
                        locations[x, y] = new Wall();
                    }
                    else //Set as floor
                    {
                        locations[x, y] = new Floor();
                    }
                }
            }

            //Draw custom world
            DrawAllWalls();
            DrawAllTorches();
            DrawAllPickups();

            locations[2, 13] = new Enemy(new Point(2, 13));
            locations[4, 14] = new Enemy(new Point(4, 14));
            locations[2, 15] = new Enemy(new Point(2, 15));
            locations[1, 16] = new Enemy(new Point(1, 16));
            locations[1, 11] = new Enemy(new Point(1, 11));
            locations[2, 18] = new Enemy(new Point(2, 18));
            locations[4, 18] = new Enemy(new Point(4, 18));
        }

        private static void DrawAllWalls()
        {
            //Manual wall setup (Lines put together are touching on the map):
            DrawWall(new Point(5, 1), new Point(11, 1));
            DrawWall(new Point(5, 2), new Point(11, 2));

            DrawWall(new Point(1, 8), new Point(1, 10));

            DrawWall(new Point(5, 5), new Point(5, 10));
            DrawWall(new Point(6, 5), new Point(7, 5));
            DrawWall(new Point(4, 8), new Point(4, 10));
            DrawWall(new Point(6, 10), new Point(11, 10));
            DrawWall(new Point(11, 5), new Point(11, 9));
            DrawWall(new Point(8, 11), new Point(8, 13));

            DrawWall(new Point(5, 13), new Point(5, 16));
            DrawWall(new Point(4, 16), new Point(3, 16));

            DrawWall(new Point(1, 19), new Point(2, 19));

            DrawWall(new Point(6, 19), new Point(8, 19));
            DrawWall(new Point(8, 19), new Point(8, 17));
        }

        private static void DrawAllTorches()
        {
            //Manual torch setup:
            locations[4, 6] = new Torch(new Point(4, 6));
            locations[7, 9] = new Torch(new Point(7, 9));
            locations[4, 15] = new Torch(new Point(4, 15));
        }

        private static void DrawAllPickups()
        {

            ((Floor)World.locations[10, 8]).HasFlare = true;
        }

        /// <summary>
        /// Returns location object at position in locations array
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <returns></returns>
        public static Location GetLocationByPos(int xPos, int yPos)
        {
            return locations[xPos, yPos];
        }

        /// <summary>
        /// Returns location object at position in locations array using a point
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Location GetLocationByPos(Point pos)
        {
            return locations[pos.X, pos.Y];
        }

        /// <summary>
        /// Returns lightsource object at position in locations array using a point
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static LightSource GetLightSourceByPos(Point pos)
        {
            if (GetLocationByPos(pos).IfLightSource())
            {
                return (LightSource)locations[pos.X, pos.Y];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets location object at position in locations array using a point
        /// </summary>
        /// <param name="pos"></param>
        public static void SetLocationByPos(Point pos, Location newLoc)
        {
           locations[pos.X, pos.Y] = newLoc;
        }

        /// <summary>
        /// Sets all points on a line between two passed points to walls
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private static void DrawWall(Point start, Point end)
        {
            //Get all points on line
            List<Point> walls = LineFinder.GetLinePoints(start, end);

            //Set all points to walls
            foreach (Point wall in walls)
            {
                locations[wall.X, wall.Y] = new Wall();
            }
        }
    }
}

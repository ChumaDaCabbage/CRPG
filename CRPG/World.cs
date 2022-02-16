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
            DrawAllEnemies();
        }

        private static void DrawAllWalls()
        {
            # region Manual wall setup 
            //(Lines put together are touching on the map):

            DrawWall(new Point(5, 1), new Point(16, 1));
            DrawWall(new Point(5, 2), new Point(15, 2));
            DrawWall(new Point(15, 3), new Point(15, 7));
            DrawWall(new Point(16, 6), new Point(16, 7));
            locations[17, 7] = new Wall();

            DrawWall(new Point(21, 1), new Point(22, 1));
            DrawWall(new Point(22, 2), new Point(22, 7));
            DrawWall(new Point(21, 6), new Point(21, 7));
            locations[20, 7] = new Wall();

            DrawWall(new Point(1, 8), new Point(1, 10));

            DrawWall(new Point(5, 5), new Point(5, 10));
            DrawWall(new Point(6, 5), new Point(7, 5));
            DrawWall(new Point(4, 8), new Point(4, 10));
            DrawWall(new Point(6, 10), new Point(11, 10));
            DrawWall(new Point(11, 5), new Point(11, 9));
            DrawWall(new Point(8, 11), new Point(8, 13));
            DrawWall(new Point(9, 11), new Point(9, 12));
            locations[10, 11] = new Wall();

            DrawWall(new Point(5, 13), new Point(5, 16));
            DrawWall(new Point(4, 16), new Point(3, 16));

            DrawWall(new Point(1, 19), new Point(1, 28));
            DrawWall(new Point(2, 19), new Point(2, 23));

            DrawWall(new Point(5, 19), new Point(5, 20));
            DrawWall(new Point(5, 22), new Point(5, 23));
            DrawWall(new Point(6, 19), new Point(8, 19));
            DrawWall(new Point(8, 17), new Point(8, 18));
            DrawWall(new Point(9, 18), new Point(9, 24));
            DrawWall(new Point(6, 23), new Point(8, 23));
            DrawWall(new Point(7, 24), new Point(8, 24));
            DrawWall(new Point(10, 19), new Point(10, 24));
            DrawWall(new Point(11, 20), new Point(11, 24));

            DrawWall(new Point(7, 28), new Point(23, 28));

            DrawWall(new Point(15, 10), new Point(23, 10));
            DrawWall(new Point(16, 11), new Point(23, 11));
            DrawWall(new Point(17, 12), new Point(23, 12));
            DrawWall(new Point(18, 13), new Point(23, 13));

            locations[15, 27] = new Wall();

            DrawWall(new Point(15, 23), new Point(15, 25));
            DrawWall(new Point(16, 23), new Point(18, 23));
            DrawWall(new Point(18, 24), new Point(18, 27));
            DrawWall(new Point(19, 25), new Point(19, 27));

            DrawWall(new Point(15, 20), new Point(18, 20));
            DrawWall(new Point(16, 19), new Point(18, 19));
            DrawWall(new Point(17, 18), new Point(23, 18));
            DrawWall(new Point(18, 17), new Point(23, 17));
            DrawWall(new Point(23, 19), new Point(23, 25));
            locations[22, 25] = new Wall();

            #endregion

            //---------------------------------------------

            #region Hallway columns:

            locations[8, 15] = new Wall();
            locations[11, 15] = new Wall();
            locations[7, 26] = new Wall();
            locations[10, 26] = new Wall();

            locations[15, 15] = new Wall();
            locations[18, 15] = new Wall();

            locations[13, 4] = new Wall();
            locations[13, 7] = new Wall();
            locations[13, 10] = new Wall();
            locations[13, 13] = new Wall();
            locations[13, 17] = new Wall();
            locations[13, 20] = new Wall();
            locations[13, 23] = new Wall();
            locations[13, 26] = new Wall();

            #endregion
        }

        private static void DrawAllTorches()
        {
            //Manual torch setup:
            locations[4, 6] = new Torch(new Point(4, 6));
            locations[7, 9] = new Torch(new Point(7, 9));
            locations[4, 15] = new Torch(new Point(4, 15));
            locations[4, 26] = new Torch(new Point(4, 26));
            locations[22, 19] = new Torch(new Point(22, 19));
            locations[16, 4] = new Torch(new Point(16, 4));
            locations[21, 15] = new Torch(new Point(21, 15));
        }

        private static void DrawAllPickups()
        {
            ((Floor)World.locations[10, 8]).HasFlare = true;
            ((Floor)World.locations[8, 22]).HasFlare = true;
            ((Floor)World.locations[17, 24]).HasFlare = true;
        }

        private static void DrawAllEnemies()
        {
            //Seperated into rooms
            locations[5, 11] = new Enemy(new Point(5, 11));
            locations[1, 17] = new Enemy(new Point(1, 17));
            locations[7, 18] = new Enemy(new Point(7, 18));

            locations[5, 25] = new Enemy(new Point(5, 25));
            locations[3, 28] = new Enemy(new Point(3, 28));

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

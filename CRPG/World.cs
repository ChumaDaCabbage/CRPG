using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class World
    {
        public Location[,] locations = new Location[MAX_WORLD_X, MAX_WORLD_Y];
        public List<Torch> _tourches = new List<Torch>();
        public List<Enemy> _enemies = new List<Enemy>();

        //Setup max world sizes
        public const int MAX_WORLD_X = 60;
        public const int MAX_WORLD_Y = 30;

        //Constructor
        public World()
        {
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
        }

        public void WorldSetup()
        {
            //Draw custom world
            DrawAllWalls();
            DrawAllTorches();
            DrawAllEnemies();
            DrawAllPickups();
        }

        private void DrawAllWalls()
        {
            # region Manual wall setup 
            //(Lines put together are touching on the map):

            DrawWall(new Point(5, 1), new Point(16, 1));
            DrawWall(new Point(5, 2), new Point(15, 2));
            DrawWall(new Point(15, 3), new Point(15, 7));
            DrawWall(new Point(16, 6), new Point(16, 7));
            locations[17, 7] = new Wall();

            DrawWall(new Point(21, 1), new Point(58, 1));
            DrawWall(new Point(22, 2), new Point(22, 7));
            DrawWall(new Point(23, 2), new Point(23, 4));
            DrawWall(new Point(24, 2), new Point(24, 4));
            DrawWall(new Point(25, 2), new Point(25, 4));
            DrawWall(new Point(21, 6), new Point(21, 7));
            locations[20, 7] = new Wall();
            DrawWall(new Point(42, 2), new Point(58, 2));
            DrawWall(new Point(42, 3), new Point(42, 15));
            DrawWall(new Point(43, 3), new Point(43, 15));
            DrawWall(new Point(36, 15), new Point(41, 15));
            DrawWall(new Point(35, 15), new Point(35, 20));
            DrawWall(new Point(34, 10), new Point(34, 20));
            DrawWall(new Point(33, 10), new Point(33, 20));
            DrawWall(new Point(37, 10), new Point(41, 10));

            DrawWall(new Point(49, 2), new Point(49, 5));
            DrawWall(new Point(53, 2), new Point(53, 5));
            DrawWall(new Point(47, 5), new Point(48, 5));
            DrawWall(new Point(54, 5), new Point(55, 5));

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

            DrawWall(new Point(25, 7), new Point(25, 9));
            DrawWall(new Point(15, 10), new Point(30, 10));
            DrawWall(new Point(16, 11), new Point(23, 11));
            DrawWall(new Point(17, 12), new Point(23, 12));
            DrawWall(new Point(18, 13), new Point(23, 13));

            DrawWall(new Point(28, 3), new Point(28, 4));
            DrawWall(new Point(29, 3), new Point(29, 4));

            DrawWall(new Point(33, 3), new Point(33, 4));
            DrawWall(new Point(34, 3), new Point(34, 4));

            DrawWall(new Point(38, 3), new Point(38, 4));
            DrawWall(new Point(39, 3), new Point(39, 4));

            DrawWall(new Point(28, 7), new Point(28, 8));
            DrawWall(new Point(29, 7), new Point(29, 8));

            DrawWall(new Point(33, 7), new Point(33, 8));
            DrawWall(new Point(34, 7), new Point(34, 8));

            DrawWall(new Point(38, 7), new Point(38, 8));
            DrawWall(new Point(39, 7), new Point(39, 8));

            locations[15, 27] = new Wall();

            locations[29, 28] = new Wall();

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
            DrawWall(new Point(24, 20), new Point(24, 24));
            DrawWall(new Point(25, 20), new Point(30, 20));
            DrawWall(new Point(29, 21), new Point(29, 25));
            DrawWall(new Point(30, 21), new Point(30, 23));
            DrawWall(new Point(31, 23), new Point(37, 23));
            DrawWall(new Point(35, 24), new Point(37, 24));
            DrawWall(new Point(35, 25), new Point(37, 25));
            locations[25, 24] = new Wall();
            locations[28, 24] = new Wall();

            locations[27, 13] = new Wall();
            locations[27, 17] = new Wall();
            locations[30, 14] = new Wall();
            locations[30, 16] = new Wall();
            DrawWall(new Point(26, 13), new Point(26, 17));
            DrawWall(new Point(29, 13), new Point(30, 13));
            DrawWall(new Point(29, 17), new Point(30, 17));

            DrawWall(new Point(35, 28), new Point(40, 28));
            DrawWall(new Point(40, 27), new Point(40, 19));
            DrawWall(new Point(39, 19), new Point(39, 20));
            DrawWall(new Point(41, 19), new Point(42, 19));
            DrawWall(new Point(41, 26), new Point(44, 26));

            DrawWall(new Point(43, 22), new Point(43, 23));
            DrawWall(new Point(44, 22), new Point(44, 23));

            DrawWall(new Point(47, 28), new Point(47, 14));
            DrawWall(new Point(45, 19), new Point(46, 19));
            DrawWall(new Point(48, 15), new Point(55, 15));
            DrawWall(new Point(48, 15), new Point(55, 15));
            DrawWall(new Point(51, 12), new Point(51, 14));
            DrawWall(new Point(55, 14), new Point(55, 17));

            DrawWall(new Point(47, 8), new Point(47, 11));
            DrawWall(new Point(55, 8), new Point(55, 11));
            DrawWall(new Point(48, 8), new Point(54, 8));

            DrawWall(new Point(50, 18), new Point(52, 18));
            DrawWall(new Point(50, 19), new Point(52, 19));

            DrawWall(new Point(55, 20), new Point(58, 20));
            DrawWall(new Point(55, 21), new Point(58, 21));
            DrawWall(new Point(50, 22), new Point(58, 22));

            DrawWall(new Point(51, 26), new Point(55, 26));
            DrawWall(new Point(51, 27), new Point(55, 27));
            DrawWall(new Point(51, 28), new Point(55, 28));
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

            locations[39, 17] = new Wall();
            locations[42, 17] = new Wall();

            locations[45, 17] = new Wall();
            locations[45, 14] = new Wall();
            locations[45, 11] = new Wall();
            locations[45, 8] = new Wall();
            locations[45, 5] = new Wall();

            locations[57, 17] = new Wall();
            locations[57, 14] = new Wall();
            locations[57, 11] = new Wall();
            locations[57, 8] = new Wall();
            locations[57, 5] = new Wall();

            locations[51, 24] = new Wall();
            locations[53, 24] = new Wall();
            locations[55, 24] = new Wall();

            #endregion
        }

        private void DrawAllTorches()
        {
            //Manual torch setup:
            locations[4, 6] = new Torch(new Point(4, 6));
            locations[7, 9] = new Torch(new Point(7, 9));
            locations[4, 15] = new Torch(new Point(4, 15));
            locations[4, 26] = new Torch(new Point(4, 26));
            locations[22, 19] = new Torch(new Point(22, 19));
            locations[16, 4] = new Torch(new Point(16, 4));
            locations[21, 15] = new Torch(new Point(21, 15));
            locations[28, 15] = new Torch(new Point(28, 15));
            locations[32, 25] = new Torch(new Point(32, 25));
            locations[41, 11] = new Torch(new Point(41, 11));
            locations[49, 27] = new Torch(new Point(49, 27));
            locations[51, 4] = new Torch(new Point(51, 4));
            locations[51, 9] = new Torch(new Point(51, 9));
        }

        private void DrawAllPickups()
        {
            ((Floor)locations[10, 8]).HasFlare = true;
            ((Floor)locations[8, 22]).HasFlare = true;
            ((Floor)locations[17, 24]).HasFlare = true;

            ((Floor)locations[37, 13]).HasFlare = true;
            ((Floor)locations[41, 14]).HasFlare = true;
            ((Floor)locations[41, 27]).HasFlare = true;
            ((Floor)locations[42, 28]).HasFlare = true;
            ((Floor)locations[48, 3]).HasFlare = true;
        }

        private void DrawAllEnemies()
        {
            //Seperated into rooms
            locations[5, 11] = new Enemy(new Point(5, 11));
            locations[1, 17] = new Enemy(new Point(1, 17));
            locations[7, 18] = new Enemy(new Point(7, 18));

            locations[5, 25] = new Enemy(new Point(5, 25));
            locations[3, 28] = new Enemy(new Point(3, 28));

            locations[6, 6] = new Enemy(new Point(6, 6));

            locations[14, 6] = new Enemy(new Point(14, 6));

            locations[17, 1] = new Enemy(new Point(17, 1));
            locations[21, 4] = new Enemy(new Point(21, 4));

            locations[19, 19] = new Enemy(new Point(19, 19));

            locations[25, 21] = new Enemy(new Point(25, 21));

            locations[34, 24] = new Enemy(new Point(34, 24));

            locations[28, 11] = new Enemy(new Point(28, 11));

            locations[26, 2] = new Enemy(new Point(26, 2));
            locations[41, 6] = new Enemy(new Point(41, 6));

            locations[39, 14] = new Enemy(new Point(39, 14));

            locations[41, 20] = new Enemy(new Point(41, 20));
            locations[41, 25] = new Enemy(new Point(41, 25));

            locations[44, 3] = new Enemy(new Point(44, 3));

            locations[53, 7] = new Enemy(new Point(53, 7));

            locations[52, 14] = new Enemy(new Point(52, 14));

            locations[48, 16] = new Enemy(new Point(48, 16));
            locations[49, 17] = new Enemy(new Point(49, 17));

            locations[53, 25] = new Enemy(new Point(53, 25));

            locations[24, 9] = new Enemy(new Point(24, 9));

            locations[24, 28] = new Enemy(new Point(24, 28));

            locations[36, 16] = new Enemy(new Point(36, 16));

            locations[45, 10] = new Enemy(new Point(45, 10));

            locations[48, 9] = new Enemy(new Point(48, 9));

            locations[57, 15] = new Enemy(new Point(57, 15));
        }

        /// <summary>
        /// Returns location object at position in locations array
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <returns></returns>
        public Location GetLocationByPos(int xPos, int yPos)
        {
            return locations[xPos, yPos];
        }

        /// <summary>
        /// Returns location object at position in locations array using a point
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Location GetLocationByPos(Point pos)
        {
            return locations[pos.X, pos.Y];
        }

        /// <summary>
        /// Returns lightsource object at position in locations array using a point
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public LightSource GetLightSourceByPos(Point pos)
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
        public void SetLocationByPos(Point pos, Location newLoc)
        {
           locations[pos.X, pos.Y] = newLoc;
        }

        /// <summary>
        /// Sets all points on a line between two passed points to walls
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void DrawWall(Point start, Point end)
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

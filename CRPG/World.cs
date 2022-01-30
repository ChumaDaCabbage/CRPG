﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class World
    {
        //public static readonly string WorldName = "HelloWorld";
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
            drawWall(new Point(5, 1), new Point(5, 2));
            drawWall(new Point(5, 5), new Point(5, 7));
            drawWall(new Point(4, 8), new Point(5, 8));
            locations[1, 8] = new Location(true);
            drawWall(new Point(6, 5), new Point(9, 5));
            drawWall(new Point(6, 2), new Point(9, 2));

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

        /// <summary>
        /// Sets all points on a line between two passed points to walls
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private static void drawWall(Point start, Point end)
        {
            //Get all points on line
            List<Point> walls = LineFinder.GetLinePoints(start, end);

            //Set all points to walls
            foreach (Point wall in walls)
            {
                locations[wall.X, wall.Y] = new Location(true);
            }
        }
    }
}

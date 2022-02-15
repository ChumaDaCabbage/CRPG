using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class LineFinder
    {
        //Gets all points on a line between two points
        public static List<Point> GetLinePoints(Point start, Point end)
        {
            //Gets x and y int's out of Points
            int x0 = start.X;
            int x1 = end.X;
            int y0 = start.Y;
            int y1 = end.Y;

            int dx = Math.Abs(x1 - x0); //Gets the x difference
            int sx = (x0 < x1) ? 1 : -1; //Gets the x direction of movement

            int dy = -Math.Abs(y1 - y0); //Gets the y difference
            int sy = (y0 < y1) ? 1 : -1; //Gets the y direction of movement

            //Gets the initial offset error
            int error = dx + dy;

            //Makes list to hold all found points
            List<Point> linePoints = new List<Point>();

            while (true)
            {
                //Adds current point to list
                linePoints.Add(new Point(x0, y0));

                //If at final point end loop
                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                //If 2xError is greater than y difference
                if (2 * error >= dy)
                {
                    //If at end x
                    if (x0 == x1)
                    {
                        //End loop
                        break;
                    }

                    error += dy;  //Get new error
                    x0 += sx;   //Increment x
                }
                if (2 * error <= dx) //If 2xError is less than x difference
                {
                    //If at end y
                    if (y0 == y1)
                    {
                        //End loop
                        break;
                    }

                    error += dx; //Get new error
                    y0 += sy; //Increment y
                }
            }

            //Return final points
            return linePoints;
        }

        public static bool BlockedCheck(int x, int y, Point end)
        {
            //Check for walls blocking
            List<Point> possibleBlockers = LineFinder.GetLinePoints(new Point(x, y), end);
            bool blocked = false;
            for (int i = 1; i < possibleBlockers.Count; i++)
            {
                if (World.GetLocationByPos(possibleBlockers[i]).IfWall())
                {
                    blocked = true;
                }
            }
            return blocked;
        }

        public static bool FullBlockedCheck(Point start, Point end)
        {
            //Check for walls blocking
            List<Point> possibleBlockers = LineFinder.GetLinePoints(start, end);
            bool blocked = false;
            for (int i = 1; i < possibleBlockers.Count; i++)
            {
                if (!World.GetLocationByPos(possibleBlockers[i]).IfFloor())
                {
                    blocked = true;
                }
            }

            return blocked;
        }
    }
}

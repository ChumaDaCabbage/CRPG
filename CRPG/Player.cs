using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Player
    {
        public string Name { set; get; }
        public int xPos = 0;
        public int yPos = 0;
        //public Location CurrentLocation { set; get; }

        public void MoveTo(int xpos, int ypos)
        {
            xPos = xpos;
            yPos = ypos;
        }

        public void MoveSouth()
        {
            if (yPos + 1 < World.MAX_WORLD_Y && !World.GetLocationByPos(xPos, yPos + 1).IsWall)
            {
                yPos++;
            }
            else
            {
                Console.WriteLine("You cannot move south");
            }
        }

        public void MoveEast()
        {
            if (xPos + 1 < World.MAX_WORLD_X && !World.GetLocationByPos(xPos + 1, yPos).IsWall)
            {
                xPos++;
            }
            else
            {
                Console.WriteLine("You cannot move east");
            }
        }

        public void MoveNorth()
        {
            if (yPos - 1 >= 0 && !World.GetLocationByPos(xPos, yPos - 1).IsWall)
            {
                yPos--;
            }
            else
            {
                Console.WriteLine("You cannot move north");
            }
        }

        public void MoveWest()
        {
            if (xPos - 1 >= 0 && !World.GetLocationByPos(xPos - 1, yPos).IsWall)
            {
                xPos--;
            }
            else
            {
                Console.WriteLine("You cannot move west");
            }
        }
    }
}

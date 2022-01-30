﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Player
    {
        public string Name { set; get; }
        public int xPos = 0;
        public int yPos = 0;

        const int PLAYER_LIGHT_LEVEL = 6;

        //public Location CurrentLocation { set; get; }

        //Teleports player to passed location and updates map
        public void MoveTo(int xpos, int ypos)
        {
            Point oldPos = new Point(xPos, yPos);

            xPos = xpos;
            yPos = ypos;

            movementUpdate(oldPos, new Point(xPos, yPos));
        }

        public void MoveSouth()
        {
            //If wanted location is available
            if (yPos + 1 < World.MAX_WORLD_Y && !World.GetLocationByPos(xPos, yPos + 1).IsWall)
            {
                //Move player
                yPos++;

                //Updates locations
                movementUpdate(new Point(xPos, yPos - 1), new Point(xPos, yPos));
            }
            else
            {
                //Inform player of failed move
                Console.Write("You cannot move south. >");
            }
        }

        public void MoveEast()
        {
            //If wanted location is available
            if (xPos + 1 < World.MAX_WORLD_X && !World.GetLocationByPos(xPos + 1, yPos).IsWall)
            {
                //Move player
                xPos++;

                //Updates locations
                movementUpdate(new Point(xPos - 1, yPos), new Point(xPos, yPos));
            }
            else
            {
                //Inform player of failed move
                Console.Write("You cannot move east. >");
            }
        }

        public void MoveNorth()
        {
            //If wanted location is available
            if (yPos - 1 >= 0 && !World.GetLocationByPos(xPos, yPos - 1).IsWall)
            {
                //Move player
                yPos--;

                //Updates locations
                movementUpdate(new Point(xPos, yPos + 1), new Point(xPos, yPos));
            }
            else
            {
                //Inform player of failed move
                Console.Write("You cannot move north. >");
            }
        }

        public void MoveWest()
        {
            //If wanted location is available
            if (xPos - 1 >= 0 && !World.GetLocationByPos(xPos - 1, yPos).IsWall)
            {
                //Move player
                xPos--;

                //Updates locations
                movementUpdate(new Point(xPos + 1, yPos), new Point(xPos, yPos));
            }
            else
            {
                //Inform player of failed move
                Console.Write("You cannot move west. >");
            }
        }

        private void movementUpdate(Point oldPos, Point newPos)
        {
            //Update location lightSources
            World.GetLocationByPos(oldPos).setLightSource(false, 0, false);
            World.GetLocationByPos(newPos).setLightSource(true, PLAYER_LIGHT_LEVEL, false);
            //Lighting.lightingUpdate(this);

            //Redraw new location and old location
            Map.redrawMapPoint(oldPos, this);
            Map.redrawMapPoint(newPos, this);
        }
    }
}

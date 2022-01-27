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

        //Teleports player to passed location and updates lighting
        public void MoveTo(int xpos, int ypos)
        {
            World.GetLocationByPos(xPos, yPos).IsLightSource = false;

            xPos = xpos;
            yPos = ypos;

            World.GetLocationByPos(xPos, yPos).IsLightSource = true;
        }

        public void MoveSouth()
        {
            //If wanted location is available
            if (yPos + 1 < World.MAX_WORLD_Y && !World.GetLocationByPos(xPos, yPos + 1).IsWall)
            {
                //Move player
                yPos++;

                //Update location lightSources
                World.GetLocationByPos(xPos, yPos - 1).IsLightSource = false;
                World.GetLocationByPos(xPos, yPos).IsLightSource = true;
                Lighting.lightingUpdate(this);

                //Redraw new location and old location
                Map.redrawMapPoint(xPos, yPos - 1, this);
                Map.redrawMapPoint(xPos, yPos, this);
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

                //Update ocation lightSources
                World.GetLocationByPos(xPos - 1, yPos).IsLightSource = false;
                World.GetLocationByPos(xPos, yPos).IsLightSource = true;
                Lighting.lightingUpdate(this);

                //Redraw new location and old location
                Map.redrawMapPoint(xPos - 1, yPos, this);
                Map.redrawMapPoint(xPos, yPos, this);
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

                //Update location lightSources
                World.GetLocationByPos(xPos, yPos + 1).IsLightSource = false;
                World.GetLocationByPos(xPos, yPos).IsLightSource = true;
                Lighting.lightingUpdate(this);

                //Redraw new location and old location
                Map.redrawMapPoint(xPos, yPos + 1, this);
                Map.redrawMapPoint(xPos, yPos, this);
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

                //Update ocation lightSources
                World.GetLocationByPos(xPos + 1, yPos).IsLightSource = false;
                World.GetLocationByPos(xPos, yPos).IsLightSource = true;
                Lighting.lightingUpdate(this);

                //Redraw new location and old location
                Map.redrawMapPoint(xPos + 1, yPos, this);
                Map.redrawMapPoint(xPos, yPos, this);
            }
            else
            {
                //Inform player of failed move
                Console.Write("You cannot move west. >");
            }
        }
    }
}

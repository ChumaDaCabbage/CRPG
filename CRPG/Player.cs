using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Player
    {
        public int xPos = 0;
        public int yPos = 0;

        public const int PLAYER_LIGHT_LEVEL = 4;
        public const int PLAYER_SHOOT_SPEED = 2;
        public List<Flare> _flares = new List<Flare>();

        //public Location CurrentLocation { set; get; }

        //Teleports player to passed location and updates map
        public void MoveTo(int xpos, int ypos)
        {
            Point oldPos = new Point(xPos, yPos);

            xPos = xpos;
            yPos = ypos;

            MovementUpdate(oldPos, new Point(xPos, yPos));
        }

        public void MoveSouth()
        {
            //If wanted location is available
            if (yPos + 1 < World.MAX_WORLD_Y && !World.GetLocationByPos(xPos, yPos + 1).IsWall)
            {
                //Move player
                yPos++;

                //Updates locations
                MovementUpdate(new Point(xPos, yPos - 1), new Point(xPos, yPos));
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
                MovementUpdate(new Point(xPos - 1, yPos), new Point(xPos, yPos));
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
                MovementUpdate(new Point(xPos, yPos + 1), new Point(xPos, yPos));
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
                MovementUpdate(new Point(xPos + 1, yPos), new Point(xPos, yPos));
            }
            else
            {
                //Inform player of failed move
                Console.Write("You cannot move west. >");
            }
        }

        public void Shoot(Point direction)
        {
            //Add to flare list
            Program._player._flares.Add(new Flare(direction, new Point(xPos, yPos)));
        }

        public bool CheckForFlare(Point pos)
        {
            bool isFlare = false;
            for (int i = 0; i < _flares.Count; i++)
            {
                //If position is a flare
                if (pos.X == _flares[i].Pos.X && pos.Y == _flares[i].Pos.Y)
                {
                    isFlare = true; //Set is flare to true
                }
            }

            return isFlare;
        }

        private void MovementUpdate(Point oldPos, Point newPos)
        {
            //Update location lightSources (Check for flares to not overwrite their light)
            if (!CheckForFlare(oldPos)) World.GetLocationByPos(oldPos).SetLightSource(false, 0, false);
            if (!CheckForFlare(newPos)) World.GetLocationByPos(newPos).SetLightSource(true, PLAYER_LIGHT_LEVEL, false);

            //Lighting.lightingUpdate(this);

            //Redraw new location and old location
            Map.RedrawMapPoint(oldPos);
            Map.RedrawMapPoint(newPos);
        }
    }
}

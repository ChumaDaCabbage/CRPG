using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Player
    {
        public Point Pos = new Point(1, 1);

        public const int PLAYER_LIGHT_LEVEL = 4;
        public const int PLAYER_SHOOT_SPEED = 2;
        public List<Flare> _flares = new List<Flare>();

        public Location on; //Holds what the player was standing on

        private DateTime LastShotTime = DateTime.Now; //Holds last time shot

        //Teleports player to passed point and updates map
        public void MoveTo(Point newPos)
        {
            Point oldPos = new Point(Pos.X, Pos.Y);

            Pos.X = newPos.X;
            Pos.Y = newPos.Y;

            on = World.GetLocationByPos(Pos);

            MovementUpdate(oldPos, new Point(Pos.X, Pos.Y));
        }

        public void MoveSouth()
        {
            //If wanted location is available
            if (Pos.Y + 1 < World.MAX_WORLD_Y && World.GetLocationByPos(Pos.X, Pos.Y + 1).IfFloor())
            {
                //Move player
                Pos.Y++;

                //Updates locations
                MovementUpdate(new Point(Pos.X, Pos.Y - 1), Pos);
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
            if (Pos.X + 1 < World.MAX_WORLD_X && World.GetLocationByPos(Pos.X + 1, Pos.Y).IfFloor())
            {
                //Move player
                Pos.X++;

                //Updates locations
                MovementUpdate(new Point(Pos.X - 1, Pos.Y), Pos);
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
            if (Pos.Y - 1 >= 0 && World.GetLocationByPos(Pos.X, Pos.Y - 1).IfFloor())
            {
                //Move player
                Pos.Y--;

                //Updates locations
                MovementUpdate(new Point(Pos.X, Pos.Y + 1), Pos);
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
            if (Pos.X - 1 >= 0 && World.GetLocationByPos(Pos.X - 1, Pos.Y).IfFloor())
            {
                //Move player
                Pos.X--;

                //Updates locations
                MovementUpdate(new Point(Pos.X + 1, Pos.Y), Pos);
            }
            else
            {
                //Inform player of failed move
                Console.Write("You cannot move west. >");
            }
        }

        public void LightTorches()
        {
            //Goes through all locations
            for (int x = 0; x < World.MAX_WORLD_X; x++)
            {
                for (int y = 0; y < World.MAX_WORLD_Y; y++)
                {
                    //Gets points inside lighting distance
                    if (World.GetLocationByPos(new Point(x, y)).IfTorch() && MathF.Sqrt(MathF.Pow(x - Pos.X, 2) + MathF.Pow(y - Pos.Y, 2)) < 1.5)
                    {
                        if (!((Torch)World.GetLightSourceByPos(new Point(x, y))).on)
                        {
                            ((Torch)World.GetLightSourceByPos(new Point(x, y))).TurnOnTorch();
                            return;
                        }
                    }
                }
            }

            //Inform player of failed light
            Console.Write("There are no torches to light. >");
        }


        public void Shoot(Point direction, Point point)
        {
            //If flare are owned
            if (FlareInventory.FlareCount > 0 && DateTime.Now >= LastShotTime.AddSeconds(0.5))
            {
                //Add to flare list
                Program._player._flares.Add(new Flare(direction, new Point(point.X, point.Y)));

                //Reset time
                LastShotTime = DateTime.Now;

                //Remove flare from inventory and redraw bar
                FlareInventory.FlareCount--;
                FlareInventory.DrawFlareBar();
            }
        }

        private void MovementUpdate(Point oldPos, Point newPos)
        {
            //Updates oldPos
            if(!World.GetLocationByPos(oldPos).IfFlare())
            {
                World.SetLocationByPos(oldPos, on);
            }

            //Start flare ground check
            GroundFlareCheck(newPos);

            on = World.GetLocationByPos(newPos); //Updates on

            //Update new location
            World.SetLocationByPos(newPos, new LightSource(newPos, PLAYER_LIGHT_LEVEL));

            Lighting.LightingUpdate(); //Updates lighting

            //Redraw new location and old location
            Map.RedrawMapPoint(oldPos);
            Map.RedrawMapPoint(newPos);
        }

        //Check for flare pickups
        private void GroundFlareCheck(Point worldPoint)
        {
            if (World.GetLocationByPos(worldPoint).IfFloor() && ((Floor)World.GetLocationByPos(worldPoint)).HasFlare && FlareInventory.FlareCount < 5)
            {
                //Updates flare info
                FlareInventory.FlareCount++;
                ((Floor)World.GetLocationByPos(worldPoint)).HasFlare = false;
                FlareInventory.DrawFlareBar();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Flare : LightSource
    {
        private int flareLightLevel = 5; //Starts with a default of 5

        DateTime lastMovedTime = DateTime.Now; //Holds last time moved
        Point Dir; //Holds direction of movement
        //public Point Pos; //Holds current position of flare
        bool moving = true; //Holds if flare is moveing

        public Flare(Point dir, Point pos) : base(pos)
        {
            Dir = dir;
            LightPower = flareLightLevel;

            //Redraw map at defualt point
            Map.RedrawMapPoint(Pos);

            //Set flare pos in Locations[] and upadte lighting
            World.SetLocationByPos(Pos, this);
            Lighting.LightingUpdate();
        }

        public void FlareUpdate()
        {
            if (moving && DateTime.Now >= lastMovedTime.AddSeconds(0.05f)) //If moving and wait time is over 0.05
            {
                Move();

                //Get new time
                lastMovedTime = DateTime.Now;
            }
            else if(DateTime.Now >= lastMovedTime.AddSeconds(3)) //If wait time is over 3
            {
                if (flareLightLevel > 2) //If flare light level is over 2
                {
                    flareLightLevel--; //Increment light level down

                    //Update lighting
                    LightPower = flareLightLevel;
                    Lighting.LightingUpdate();
                }
                else //Destroy self when light is off
                {
                    DestroySelf();
                }

                //Get new time
                lastMovedTime = DateTime.Now;
            }
        }

        private void Move()
        {
            Point newPos = Pos.Add(Dir);

            //If wanted location is available
            if (newPos.Y < World.MAX_WORLD_Y && newPos.Y >= 0
                && newPos.X < World.MAX_WORLD_X && newPos.X >= 0
                && !LineFinder.BlockedCheck(Pos, newPos))
            {
                Point oldPos = new Point(Pos.X, Pos.Y); //Save old pos
                Pos = newPos; //Update pos
                MovementUpdate(oldPos, Pos); //Updates location data
            }
            else if (Math.Abs(Dir.X) > 1 || Math.Abs(Dir.Y) > 1) //If not available and going a long way
            {
                //Shrink move size
                Dir.X /= 2;
                Dir.Y /= 2;
            }
            else //If going only one block and still not  available
            {
                //Stop movement
                moving = false;
            }
        }

        private void DestroySelf()
        {
            //Find self, destroy self, update map
            for (int i = 0; i < Program._player._flares.Count; i++)
            {
                if (Program._player._flares[i] == this)
                {
                    Program._player._flares.RemoveAt(i);
                    World.SetLocationByPos(Pos, new Floor());
                    Map.RedrawMapPoint(Pos);
                    Lighting.LightingUpdate();
                }
            }
        }

        private void MovementUpdate(Point oldPos, Point newPos)
        {
            //Update location lightSources
            if (!oldPos.Equals(Program._player.Pos)) World.SetLocationByPos(oldPos, new Floor()); //Remove light source
            else World.SetLocationByPos(oldPos, new LightSource(oldPos, Player.PLAYER_LIGHT_LEVEL)); //Give player light back

            //Set new flare pos in Locations[] and update lighting
            World.SetLocationByPos(newPos, this);
            Lighting.LightingUpdate();

            //Redraw new location and old location
            Map.RedrawMapPoint(oldPos);
            Map.RedrawMapPoint(newPos);
        }

        public override bool IfFlare()
        {
            return true;
        }
    }
}

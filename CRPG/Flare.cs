using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Flare : LightSource
    {
        private int flareLightLevel = 5; //Starts with a default of 5
        
        DateTime LastMovedTime = Program.CurrentTimeThisFrame; //Holds last time moved 
        public Point Dir; //Holds direction of movement

        private Location on; //Holds what the flare is on

        bool Moving = true; //Holds if flare is moving

        bool neverMoved = true; //Holds if flare never moved

        public Flare(Point dir, Point pos) : base(pos)
        {
            Dir = dir;
            LightPower = flareLightLevel;

            //Setup on info
            on = World.GetLocationByPos(Pos);

            //Redraw map at defualt point
            Map.RedrawMapPoint(Pos);

            //Set flare pos in Locations[] and update lighting
            World.SetLocationByPos(Pos, this);
            Lighting.LightingUpdate();
        }

        public void FlareUpdate()
        {
            if (Moving && Program.CurrentTimeThisFrame >= LastMovedTime.AddSeconds(0.05f)) //If moving and wait time is over 0.05
            {
                Move();

                //Get new time
                LastMovedTime = Program.CurrentTimeThisFrame;
            }
            else if(Program.CurrentTimeThisFrame >= LastMovedTime.AddSeconds(3)) //If wait time is over 3
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
                LastMovedTime = Program.CurrentTimeThisFrame;
            }
        }

        private void Move()
        {
            Point newPos = Pos.Add(Dir);

            //If wanted location is available
            if (newPos.Y < World.MAX_WORLD_Y && newPos.Y >= 0
                && newPos.X < World.MAX_WORLD_X && newPos.X >= 0
                && !LineFinder.FullBlockedCheck(Pos, newPos))
            {
                //Set neverMoved to false
                neverMoved = false;

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
                Moving = false;
            }
        }

        private void DestroySelf()
        {
            //If never moved
            if (neverMoved)
            {
                //Set on to floor
                on = new Floor();
            }

            //Find self, destroy self, update map
            for (int i = 0; i < Program._player._flares.Count; i++)
            {
                if (Program._player._flares[i] == this)
                {
                    //Turn of light and update lighting
                    LightPower = 0;
                    Lighting.LightingUpdate();

                    //Remove self from list, remove self from world
                    Program._player._flares.RemoveAt(i);
                    World.SetLocationByPos(Pos, on);
                }
            }
        }

        private void MovementUpdate(Point oldPos, Point newPos)
        {
            //Update location lightSources
            if (!oldPos.Equals(Program._player.Pos)) World.SetLocationByPos(oldPos, on); //Remove light source
            else World.SetLocationByPos(oldPos, new LightSource(oldPos, Player.PLAYER_LIGHT_LEVEL)); //Give player light back

            on = World.GetLocationByPos(newPos); //Updates on

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

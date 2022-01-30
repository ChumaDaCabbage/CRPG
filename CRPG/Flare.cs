﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Flare 
    {
        private int flareLightLevel = 5; //Starts with a default of 5

        DateTime lastMovedTime = DateTime.Now; //Holds last time moved
        Point Dir; //Holds direction of movement
        public Point Pos; //Holds current position of flare
        bool moving = true; //Holds if flare is moveing

        public Flare(Point dir, Point pos)
        {
            Dir = dir;
            Pos = pos;

            //Redraw map at defualt point
            Map.RedrawMapPoint(Pos);

            //Setup lighting for flare
            World.GetLocationByPos(Pos).SetLightSource(true, flareLightLevel, true);
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
                    World.GetLocationByPos(Pos).SetLightSource(true, flareLightLevel, true);
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
                    Map.RedrawMapPoint(Pos);
                    World.GetLocationByPos(Pos).SetLightSource(false, 0, false);
                    Lighting.LightingUpdate();
                }
            }
        }

        private void MovementUpdate(Point oldPos, Point newPos)
        {
            //Update location lightSources
            if (!oldPos.Equals(Program._player.Pos)) World.GetLocationByPos(oldPos).SetLightSource(false, 0, false); //Remove light source
            else World.GetLocationByPos(oldPos).SetLightSource(true, Player.PLAYER_LIGHT_LEVEL, false); //Give player light back

            World.GetLocationByPos(newPos).SetLightSource(true, flareLightLevel, true);
            Lighting.LightingUpdate();

            //Redraw new location and old location
            Map.RedrawMapPoint(oldPos);
            Map.RedrawMapPoint(newPos);
        }
    }
}
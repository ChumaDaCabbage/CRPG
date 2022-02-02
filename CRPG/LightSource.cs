using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class LightSource : Location
    {
        //LightSource info
        public int LightPower = 1;
        public Point Pos;

        public LightSource(Point pos, int lightPower)
        {
            Pos = pos;
            LightPower = lightPower;
        }

        public LightSource(Point pos)
        {
            Pos = pos;
        }

        public override bool IfWall()
        {
            return false;
        }

        public override bool IfLightSource()
        {
            return true;
        }

        public override bool IfFlare()
        {
            return false;
        }

        public override bool IfFloor()
        {
            return false;
        }

        public override bool IfTorch()
        {
            return false;
        }

        public override bool IfEnemy()
        {
            return false;
        }
    }
}

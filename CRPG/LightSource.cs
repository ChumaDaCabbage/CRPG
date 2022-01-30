using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class LightSource : Location
    {
        //LightSource info
        public bool IsLightSource = false;
        public bool RedLightSource = false;
        public Point Pos;
        public int LightPower = 1;

        public LightSource(Point pos,  bool isLightSource, int lightPower, bool redLightSource)
        {
            Pos = pos;
            IsLightSource = isLightSource;
            RedLightSource = redLightSource;
            LightPower = lightPower;
        }

        //public void SetLightSource(bool isLightSource, int lightPower, bool redLightSource)
        //{
        //    //Set lighting data
        //    this.IsLightSource = isLightSource;
        //    this.LightPower = lightPower;
        //    this.RedLightSource = redLightSource;
        //}

        public override bool IfWall()
        {
            return false;
        }

        public override bool IfLightSource()
        {
            return true;
        }
    }
}

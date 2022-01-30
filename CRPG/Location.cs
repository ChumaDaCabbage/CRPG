using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public abstract class Location
    {
        //Light Received info
        public int currentLightLevel = 1;
        public bool redLight = false;

        public abstract bool IfWall();

        public abstract bool IfLightSource();
    }
}

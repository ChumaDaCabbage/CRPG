using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public abstract class Location
    {
        //Light Received info
        public int CurrentLightLevel = 1;
        public bool RedLight = false;
        public bool OrangeLight = false;

        public abstract bool IfWall();

        public abstract bool IfLightSource();

        public abstract bool IfFlare();

        public abstract bool IfFloor();

        public abstract bool IfTorch();

        public abstract bool IfEnemy();

        public abstract bool IfPlayer();
    }
}

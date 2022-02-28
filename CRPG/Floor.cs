using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Floor : Location
    {
        public bool HasFlare = false;

        public override bool IfWall()
        {
            return false;
        }

        public override bool IfLightSource()
        {
            return false;
        }
        public override bool IfFlare()
        {
            return false;
        }

        public override bool IfFloor()
        {
            return true;
        }

        public override bool IfTorch()
        {
            return false;
        }

        public override bool IfEnemy()
        {
            return false;
        }

        public override bool IfPlayer()
        {
            return false;
        }
    }
}

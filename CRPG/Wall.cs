using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Wall : Location 
    {
        public override bool IfWall()
        {
            return true;
        }

        public override bool IfLightSource()
        {
            return false;
        }
    }
}

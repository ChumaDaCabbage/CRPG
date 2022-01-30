﻿using System;
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

        public override bool IfFlare()
        {
            return false;
        }

        public override bool IfFloor()
        {
            return false;
        }
    }
}

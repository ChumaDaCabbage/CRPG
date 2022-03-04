using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    [Obsolete("A simple lightsource worked better for the player", false)]
    class PlayerLight : LightSource
    {
        public PlayerLight(Point pos, int lightPower) : base(pos, lightPower) { }

        public override bool IfPlayer()
        {
            return true;
        }

    }
}
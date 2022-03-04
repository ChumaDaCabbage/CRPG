using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    class TutorialPlayer : LightSource
    {

        public TutorialPlayer(Point pos) : base(pos) 
        {
            LightPower = Player.PLAYER_LIGHT_LEVEL;
        }

        public override bool IfTutorialPlayer()
        {
            return true;
        }
    }
}


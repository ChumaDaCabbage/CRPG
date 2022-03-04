using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class TutorialEnemy : LightSource
    {
        const int ENEMY_LIGHT_LEVEL = 3;
        const float MOVE_DELAY = 0.17f;

        //Sleeping info
        public int AgitationLevel = 0; //Holds how angry the enemy is
        DateTime LastBlinkedTime = Program.CurrentTimeThisFrame; //Holds last time blinked
        public int blinkStatus = 0; //Holds if back should be yellow currently
        readonly Color[] backLightColors = new Color[] //Holds all colors for back light
       {
            new Color(0, 0, 0),
            new Color(64, 46, 19),
            new Color(127, 93, 38),
            new Color(191, 139, 58),
            new Color(255, 186, 77),
            new Color(255, 232, 96),
            new Color(255, 255, 115),
            new Color(255, 255, 135),
            new Color(255, 255, 154)
       }; //Holds colors to use for back

        public TutorialEnemy(Point pos, int agitationLevel) : base(pos)
        {
            LightPower = 0;
            AgitationLevel = agitationLevel;

            if (AgitationLevel >= 2) //If light should be on
            {
                LightPower = 2;
                Lighting.LightingUpdate();
                Map.RedrawMapPoint(Pos);
            }
            else if (AgitationLevel < 2) //If light should be off
            {
                LightPower = 1;
                Lighting.LightingUpdate();
                Map.RedrawMapPoint(Pos);
            }

        }

        public void EnemyUpdate()
        {
            //If blink time is up
            if (Program.CurrentTimeThisFrame >= LastBlinkedTime.AddSeconds(0.5f / AgitationLevel))
            {
                //If not blinking
                if (blinkStatus == 0)
                {
                    //Turn blink on
                    blinkStatus += AgitationLevel;
                }
                else
                {
                    //Turn blink off
                    blinkStatus = 0;
                }

                //Redraw enemy pos
                Map.RedrawMapPoint(Pos);

                //Get new time
                LastBlinkedTime = Program.CurrentTimeThisFrame;
            }
        } 

        public Color GetBackColor()
        {
            return backLightColors[Math.Clamp((CurrentLightLevel + blinkStatus) - 1, 0, 8)];
        }

        public override bool IfTutorialEnemy()
        {
            return true;
        }
    }
}
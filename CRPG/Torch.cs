using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Torch : LightSource
    {

        const int TORCH_LIGHT_LEVEL = 5;
        readonly Random RandomGenerator = new Random();

        public bool on = false;

        //Flicker data
        DateTime LastFlickeredTime = DateTime.Now;
        float FlickerTime = 0.5f;

        //Flicker color data
        Tile CurrentColor = new Tile(176, 40, 12);

        public Torch(Point pos) : base(pos) 
        {
            World._tourches.Add(this);
            LightPower = 0;
        }

        public void TorchEffectsUpdate()
        {
            if (LightPower > 1)
            {
                if (DateTime.Now >= LastFlickeredTime.AddSeconds(FlickerTime)) //If wait time is over
                {
                    int randomValue = RandomGenerator.Next(10);

                    if (randomValue < 5)
                    {
                        //Set color infor and get new wait time
                        CurrentColor = new Tile(255, 146, 0, "░░", -50);
                        FlickerTime = Math.Clamp((float)RandomGenerator.NextDouble(), 0.6f, 1);
                    }
                    else if (randomValue < 8)
                    {
                        CurrentColor = new Tile(237, 119, 0, "░░", -25);
                        FlickerTime = Math.Clamp((float)RandomGenerator.NextDouble() - 0.2f, 0.3f, 1);
                    }
                    else
                    {
                        CurrentColor = new Tile(255, 94, 0, "░░", -75);
                        FlickerTime = Math.Clamp((float)RandomGenerator.NextDouble() - 0.2f, 0.3f, 1);
                    }

                    //Redraw torch
                    Map.RedrawMapPoint(Pos);

                    //Get new time and update lighting
                    LastFlickeredTime = DateTime.Now;
                }
            }
        }

        public Tile GetCurrentTorchColor()
        {
            return CurrentColor;
        }

        public void TurnOnTorch()
        {
            //Turn on lights
            LightPower = TORCH_LIGHT_LEVEL;
            on = true;

            //Update lighting
            Lighting.LightingUpdate();
        }

        public override bool IfTorch()
        {
            return true;
        }
    }
}

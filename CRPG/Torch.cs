using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Torch : LightSource
    {

        const int TORCH_LIGHT_LEVEL = 5;
        //readonly Random RandomGenerator = new Random();

        public bool on = false;

        //Flicker data
        //DateTime LastFlickeredTime = DateTime.Now;
        //float FlickerTime = 0.5f;

        //Flicker color data
        TileVisuals CurrentColor = new TileVisuals(new Color(176, 40, 12));

        public Torch(Point pos) : base(pos) 
        {
            Program._world._tourches.Add(this);
            LightPower = 0;
        }

        /* To costly for minor effect (Not deleting just in case I want to add it back)
        public void TorchEffectsUpdate()
        {
            if (LightPower > 1)
            {
                if (DateTime.Now >= LastFlickeredTime.AddSeconds(FlickerTime)) //If wait time is over
                {
                    int randomValue = RandomGenerator.Next(10);

                    if (randomValue < 5)
                    {
                        //Set color info and get new wait time
                        CurrentColor = new TileVisuals(new Color(255, 146, 0), "░░", -50);
                        FlickerTime = Math.Clamp((float)RandomGenerator.NextDouble(), 0.6f, 1);
                    }
                    else if (randomValue < 8)
                    {
                        CurrentColor = new TileVisuals(new Color(237, 119, 0), "░░", -25);
                        FlickerTime = Math.Clamp((float)RandomGenerator.NextDouble() - 0.2f, 0.3f, 1);
                    }
                    else
                    {
                        CurrentColor = new TileVisuals(new Color(255, 94, 0), "░░", -75);
                        FlickerTime = Math.Clamp((float)RandomGenerator.NextDouble() - 0.2f, 0.3f, 1);
                    }

                    //Redraw torch
                    Map.RedrawMapPoint(Pos);

                    //Get new time and update lighting
                    LastFlickeredTime = DateTime.Now;
                }
            }
        }
        */

        public TileVisuals GetCurrentTorchColor()
        {
            return CurrentColor;
        }

        public void TurnOnTorch()
        {
            //Turn on lights
            LightPower = TORCH_LIGHT_LEVEL;
            CurrentColor = new TileVisuals(new Color(255, 146, 0), "░░", -50);
            on = true;

            //Update lighting and map pos
            Lighting.LightingUpdate();
            Map.RedrawMapPoint(Pos);
        }

        public override bool IfTorch()
        {
            return true;
        }

        //Returns type of lightsource
        public new string lightType()
        {
            return "Torch";
        }
    }
}

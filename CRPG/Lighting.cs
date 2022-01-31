using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class Lighting
    {
        private const int RED_BOOST_AMOUNT = 15;
        private const int ORANGE_BOOST_AMOUNT = 15;

        public static void LightingUpdate()
        {
            //Will hold all lightsources
            List<LightSource> foundLightSources = new List<LightSource>();

            //Goes through all locations
            for (int x = 0; x < World.MAX_WORLD_X; x++)
            {
                for (int y = 0; y < World.MAX_WORLD_Y; y++)
                {
                    //If lightSource is found
                    if (World.locations[x, y].IfLightSource())
                    {
                        //Add lightsource to list
                        foundLightSources.Add(World.GetLightSourceByPos(new Point(x, y)));
                    }
                }
            }

            SetLightLevels(foundLightSources);
        }

        /// <summary>
        /// Sets light levels from passed position of all light sources
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetLightLevels(List<LightSource> lightSources)
        {
            //Goes through all locations
            for (int x2 = 0; x2 < World.MAX_WORLD_X; x2++)
            {
                for (int y2 = 0; y2 < World.MAX_WORLD_Y; y2++)
                {
                    int greatestLightLevel = 1; //Holds greatest lightlevel found
                    bool isRedLight = false; //Holds if red light source
                    bool isYellowLight = false; //Holds if yellow light source

                    //Go through all lightsources
                    for (int i = 0; i < lightSources.Count; i++)
                    {
                        //Holds dist of current lightsource
                        float newDist = MathF.Sqrt(MathF.Pow(x2 - lightSources[i].Pos.X, 2) + MathF.Pow(y2 - lightSources[i].Pos.Y, 2));

                        //Holds possible light level from current source
                        int level = (int)Math.Clamp(lightSources[i].LightPower - ((newDist / 2.5f) - 1), 1, 9); 

                        //If new lightsource is giving better light
                        if (level > greatestLightLevel && !LineFinder.BlockedCheck(x2, y2, lightSources[i].Pos))
                        {
                            //Replace shortestDist and lightPower
                            greatestLightLevel = level;
                        }

                        //If red light is close enough to location and not blocked
                        if (lightSources[i].IfFlare() && (int)Math.Clamp(lightSources[i].LightPower - ((newDist / 2.5f) - 1), 1, 9) >= 2f && !LineFinder.BlockedCheck(x2, y2, lightSources[i].Pos))
                        {
                            //Set redlight to true
                            isRedLight = true;
                        }
                        else if (lightSources[i].IfTorch() && (int)Math.Clamp(lightSources[i].LightPower - ((newDist / 2.5f) - 1), 1, 9) >= 2f && !LineFinder.BlockedCheck(x2, y2, lightSources[i].Pos))
                        {
                            //Set yellowlight to true
                            isYellowLight = true;
                        }
                    }

                    //Sets up light level info
                    SetLightLevel(x2, y2, greatestLightLevel, isRedLight, isYellowLight);
                }
            }
        }

        private static void SetLightLevel(int x, int y, int level, bool newRedLight, bool newYellowLight)
        {
            //If light level is a new value
            if (World.locations[x, y].CurrentLightLevel != level || World.locations[x, y].RedLight != newRedLight || World.locations[x, y].RedLight != newYellowLight)
            {
                //Update lightlevel and redLight
                World.locations[x, y].CurrentLightLevel = level;
                World.locations[x, y].RedLight = newRedLight;
                World.locations[x, y].OrangeLight = newYellowLight;

                //Redraw map point
                Map.RedrawMapPoint(x, y);
            }
        }

        private static Color ColorBoosts(int x, int y, Color currentColor)
        {
            //Adds colorboosts if needed
            if (World.locations[x, y].RedLight)
            {
                currentColor.R += RED_BOOST_AMOUNT * (World.locations[x, y].CurrentLightLevel - 1);
            }
            else if (World.locations[x, y].OrangeLight)
            {
                currentColor.R += ORANGE_BOOST_AMOUNT * (World.locations[x, y].CurrentLightLevel - 1);
                currentColor.G += (ORANGE_BOOST_AMOUNT / 3) * (World.locations[x, y].CurrentLightLevel - 1);
            }

            return currentColor;
        }

        /// <summary>
        /// Returns flare color to be used based off light level
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Color GetFlareColor(int x, int y)
        {
            //Holds color for flare
            Color flareColor;

            //Gets color based off of light level
            switch (World.locations[x, y].CurrentLightLevel)
            {
                case 1:
                    flareColor = new Color(0, 0, 0);
                    break;
                case 2:
                    flareColor = new Color(51, 4, 4);
                    break;
                case 3:
                    flareColor = new Color(105, 15, 15);
                    break;
                case 4:
                    flareColor = new Color(156, 31, 31);
                    break;
                case 5:
                    flareColor = new Color(209, 56, 56);
                    break;
                case 6:
                    flareColor = new Color(222, 71, 71);
                    break;
                case 7:
                    flareColor = new Color(235, 87, 87);
                    break;
                case 8:
                    flareColor = new Color(247, 104, 104);
                    break;
                case 9:
                    flareColor = new Color(255, 120, 120);
                    break;
                default:
                    return null;
            }

            //Returns flareColor
            return flareColor;
        }

        /// <summary>
        /// Returns wall color to be used based off light level
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Color GetWallTileColor(int x, int y)
        {
            //Holds color for wall
            Color wallColor;

            //Gets color based off of light level
            switch (World.locations[x, y].CurrentLightLevel)
            {
                case 1:
                    wallColor = new Color(0, 0, 0);
                    break;
                case 2:
                    wallColor = new Color(13, 13, 13);
                    break;
                case 3:
                    wallColor = new Color(26, 26, 26);
                    break;
                case 4:
                    wallColor = new Color(38, 38, 38);
                    break;
                case 5:
                    wallColor = new Color(51, 51, 51);
                    break;
                case 6:
                    wallColor = new Color(64, 64, 64);
                    break;
                case 7:
                    wallColor = new Color(77, 77, 77);
                    break;
                case 8:
                    wallColor = new Color(89, 89, 89);
                    break;
                case 9:
                    wallColor = new Color(102, 102, 102);
                    break;
                default:
                    return null;
            }

            //Adds color boosts
            wallColor = ColorBoosts(x, y, wallColor);

            //Returns wallColor
            return wallColor;
        }

        /// <summary>
        /// Returns floor color to be used based off light level
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Color GetFloorTileColor(int x, int y)
        {
            //Holds color for wall
            Color wallColor;

            //Gets color based off of light level
            switch (World.locations[x, y].CurrentLightLevel)
            {
                case 1:
                    wallColor = new Color(0, 0, 0);
                    break;
                case 2:
                    wallColor = new Color(26, 15, 2);
                    break;
                case 3:
                    wallColor = new Color(51, 30, 7);
                    break;
                case 4:
                    wallColor = new Color(77, 48, 15);
                    break;
                case 5:
                    wallColor = new Color(103, 66, 27);
                    break;
                case 6:
                    wallColor = new Color(128, 86, 41);
                    break;
                case 7:
                    wallColor = new Color(153, 108, 58);
                    break;
                case 8:
                    wallColor = new Color(179, 132, 79);
                    break;
                case 9:
                    wallColor = new Color(204, 157, 102);
                    break;
                default:
                    return null;
            }

            //Adds color boosts
            wallColor = ColorBoosts(x, y, wallColor);

            //Returns wallColor
            return wallColor;
        }

        /// <summary>
        /// Returns torch color to be used based off light level
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Color GetTorchTileColor(int x, int y)
        {
            //Holds color for wall
            Color torchColor;

            if (((Torch)World.GetLightSourceByPos(new Point(x, y))).on)
            {
                torchColor = ((Torch)World.locations[x, y]).GetCurrentTorchColor();
            }
            else
            {
                //Gets color based off of light level
                switch (World.locations[x, y].CurrentLightLevel)
                {
                    case 1:
                        torchColor = new Color(0, 0, 0);
                        break;
                    case 2:
                        torchColor = new Color(46, 8, 0);
                        break;
                    case 3:
                        torchColor = new Color(69, 11, 0);
                        break;
                    case 4:
                        torchColor = new Color(105, 18, 0);
                        break;
                    case 5:
                        torchColor = new Color(140, 24, 3);
                        break;
                    case 6:
                        torchColor = new Color(176, 40, 12);
                        break;
                    case 7:
                        torchColor = new Color(189, 51, 23);
                        break;
                    case 8:
                        torchColor = new Color(201, 62, 34);
                        break;
                    case 9:
                        torchColor = new Color(214, 75, 47);
                        break;
                    default:
                        return null;
                }
            }

            //Returns torchColor
            return torchColor;
        }
    }
}

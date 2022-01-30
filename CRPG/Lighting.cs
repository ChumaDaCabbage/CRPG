using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class Lighting
    {
        private const int RED_BOOST_AMOUNT = 20;

        public static void lightingUpdate(Player player)
        {
            //Will hold all lightsources
            List<Point> foundLightSources = new List<Point>();

            //Goes through all locations
            for (int x = 0; x < World.MAX_WORLD_X; x++)
            {
                for (int y = 0; y < World.MAX_WORLD_Y; y++)
                {
                    //If lightSource is found
                    if (World.locations[x, y].IsLightSource == true)
                    {
                        //Add lightsource to list
                        foundLightSources.Add(new Point(x, y));
                    }
                }
            }

            setLightLevels(foundLightSources, player);
        }

        /// <summary>
        /// Sets light levels from passed position of all light sources
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void setLightLevels(List<Point> lightSources, Player player)
        {
            //Goes through all locations
            for (int x2 = 0; x2 < World.MAX_WORLD_X; x2++)
            {
                for (int y2 = 0; y2 < World.MAX_WORLD_Y; y2++)
                {
                    float shortestDist = 9999; //Holds shortest distance to a lightsource
                    int lightPower = 0; //Holds power of current light
                    bool isRedLight = false; //Holds if red light source

                    //Go through all lightsources
                    for (int i = 0; i < lightSources.Count; i++)
                    {
                        //Holds dist of current lightsource
                        float newDist = MathF.Sqrt(MathF.Pow(x2 - lightSources[i].X, 2) + MathF.Pow(y2 - lightSources[i].Y, 2));

                        //If distance to lightsource is shorter
                        if (newDist < shortestDist && !blockedCheck(x2, y2, lightSources[i]))
                        {
                            //Replace shortestDist and lightPower
                            shortestDist = MathF.Sqrt(MathF.Pow(x2 - lightSources[i].X, 2) + MathF.Pow(y2 - lightSources[i].Y, 2));
                            lightPower = World.GetLocationByPos(lightSources[i]).LightPower;
                        }

                        //If red light is close enough to player and not blocked
                        if (World.GetLocationByPos(lightSources[i]).RedLightSource && World.GetLocationByPos(lightSources[i]).LightPower - ((newDist / 2.5f) - 1) > 0 && !blockedCheck(x2, y2, lightSources[i]))
                        {
                            //Set redlight to true
                            isRedLight = true;
                        }
                    }

                    //Sets up light level info
                    setLightLevel(x2, y2, (int)Math.Clamp(lightPower - ((shortestDist / 2.5f) - 1), 1, 9), player, isRedLight);

                }
            }
        }

        private static bool blockedCheck(int x, int y, Point light)
        {
            //Check for walls blocking
            List<Point> possibleBlockers = LineFinder.getLinePoints(new Point(x, y), light);
            bool blocked = false;
            for (int j = 1; j < possibleBlockers.Count; j++)
            {
                if (World.GetLocationByPos(possibleBlockers[j]).IsWall)
                {
                    blocked = true;
                }
            }

            return blocked;
        }

        private static void setLightLevel(int x, int y, int level, Player player, bool newRedLight)
        {
            //If light level is a new value
            if (World.locations[x, y].currentLightLevel != level || World.locations[x, y].redLight != newRedLight)
            {
                //Update lightlevel and redLight
                World.locations[x, y].currentLightLevel = level;
                World.locations[x, y].redLight = newRedLight;

                //Redraw map point
                Map.redrawMapPoint(x, y, player);
            }
        }

        //Returns wall color to be used based off light level
        public static Color getWallTileColor(int x, int y)
        {
            //Holds color for wall
            Color wallColor;

            //Gets color based off of light level
            switch (World.locations[x, y].currentLightLevel)
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

            //Adds redboost if needed
            if (World.locations[x, y].redLight)
            {
                wallColor.R += RED_BOOST_AMOUNT * (World.locations[x, y].currentLightLevel - 1);
            }

            //Returns wallColor
            return wallColor;
        }

        //Returns floor color to be used based off light level
        public static Color getFloorTileColor(int x, int y)
        {
            //Holds color for wall
            Color wallColor;

            //Gets color based off of light level
            switch (World.locations[x, y].currentLightLevel)
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

            //Adds redboost if needed
            if (World.locations[x, y].redLight)
            {
                wallColor.R += RED_BOOST_AMOUNT * (World.locations[x, y].currentLightLevel - 1);
            }

            //Returns wallColor
            return wallColor;
        }
    }
}

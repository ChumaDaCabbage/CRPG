using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class Lighting
    {
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
                    //if (World.GetLocationByPos(x2,y2).IsWall) //CHeck is location is something that can be lit
                    //{
                        float shortestDist = 9999; //Holds shortest distance to a lightsource
                        int lightPower = 0;
                    
                        //Go through all lightsources
                        for (int i = 0; i < lightSources.Count; i++)
                        {

                            //If distance to lightsource is shorter
                            if (MathF.Sqrt(MathF.Pow(x2 - lightSources[i].X, 2) + MathF.Pow(y2 - lightSources[i].Y, 2)) < shortestDist)
                            {
                                //Check for walls blocking
                                List<Point> possibleBlockers = LineFinder.getLinePoints(new Point(x2, y2), lightSources[i]);
                                bool blocked = false;
                                for (int j = 1; j < possibleBlockers.Count; j++)
                                {
                                    if (World.GetLocationByPos(possibleBlockers[j]).IsWall)
                                    {
                                        blocked = true;
                                    }
                                }

                                if (!blocked)
                                {
                                    //Replace shortestDist and lightPower
                                    shortestDist = MathF.Sqrt(MathF.Pow(x2 - lightSources[i].X, 2) + MathF.Pow(y2 - lightSources[i].Y, 2));
                                    lightPower = World.GetLocationByPos(lightSources[i]).lightPower;
                                }
                            }
                        }

                        setLightLevel(x2, y2, (int)Math.Clamp(lightPower - ((shortestDist / 2.5f) - 1), 1, 9), player);

                        /*
                        //Set lightlevel based on distance
                        if (shortestDist < 2.5f)
                        {
                            setLightLevel(x2, y2, lightPower, player);
                        }
                        else if (shortestDist < 5f)
                        {
                            setLightLevel(x2, y2, lightPower - 1, player);
                        }
                        else if (shortestDist < 7.5f)
                        {
                            setLightLevel(x2, y2, 4, player);
                        }
                        else if (shortestDist < 10f)
                        {
                            setLightLevel(x2, y2, 3, player);
                        }
                        else if (shortestDist < 12.5f)
                        {
                            setLightLevel(x2, y2, 2, player);
                        }
                        else
                        {
                            setLightLevel(x2, y2, 1, player);
                        }
                        */
                    //}
                    //else //If cant be light just set to 1
                    //{
                    //    setLightLevel(x2, y2, 1, player);
                    //}
                }
            }
        }

        private static void setLightLevel(int x, int y, int level, Player player)
        {
            //If light level is a new value
            if (World.locations[x, y].lightLevel != level)
            {
                //Update lightlevel and redraw map point
                World.locations[x, y].lightLevel = level;
                Map.redrawMapPoint(x, y, player);
            }
        }

        /*Defualt walls Lighting colors reference
         * Darkest-
         * 1: "\x1b[48;2;0;0;0m  "
         * 2: "\x1b[48;2;13;13;13m  "
         * 3: "\x1b[48;2;26;26;26m  "
         * 4" "\x1b[48;2;38;38;38m  "
         * 5: "\x1b[48;2;51;51;51m  "
         * 6: "\x1b[48;2;64;64;64m  "
         * 7: "\x1b[48;2;77;77;77m  "
         * 8: "\x1b[48;2;89;89;89m  "
         * 9: "\x1b[48;2;102;102;102m  "
         * Lightest-
         **/

        //Returns wall color to be used based off light level
        public static string getWallTileColor(int x, int y)
        {
            switch (World.locations[x, y].lightLevel)
            {
                case 1:
                    return "\x1b[48;2;0;0;0m  ";
                case 2:
                    return "\x1b[48;2;13;13;13m  ";
                case 3:
                    return "\x1b[48;2;26;26;26m  ";
                case 4:
                    return "\x1b[48;2;38;38;38m  ";
                case 5:
                    return "\x1b[48;2;51;51;51m  ";
                case 6:
                    return "\x1b[48;2;64;64;64m  ";
                case 7:
                    return "\x1b[48;2;77;77;77m  ";
                case 8:
                    return "\x1b[48;2;89;89;89m  ";
                case 9:
                    return "\x1b[48;2;102;102;102m  ";
                default:
                    return null;
            }
        }

        //Returns floor color to be used based off light level
        public static string getFloorTileColor(int x, int y)
        {
            switch (World.locations[x, y].lightLevel)
            {
                case 1:
                    return "\x1b[48;2;0;0;0m  ";
                case 2:
                    return "\x1b[48;2;26;15;2m  ";
                case 3:
                    return "\x1b[48;2;51;30;7m  ";
                case 4:
                    return "\x1b[48;2;77;48;15m  ";
                case 5:
                    return "\x1b[48;2;103;66;27m  ";
                case 6:
                    return "\x1b[48;2;128;86;41m  ";
                case 7:
                    return "\x1b[48;2;153;108;58m  ";
                case 8:
                    return "\x1b[48;2;179;132;79m  ";
                case 9:
                    return "\x1b[48;2;204;157;102m  ";
                default:
                    return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class Lighting
    {
        public static void lightingUpdate(Player player)
        {
            List<Point> foundLightSources = new List<Point>();

            //Goes through all locations
            for (int x = 0; x < World.MAX_WORLD_X; x++)
            {
                for (int y = 0; y < World.MAX_WORLD_Y; y++)
                {
                    //If lightSource is found
                    if (World.locations[x, y].IsLightSource == true)
                    {
                        System.Diagnostics.Debug.WriteLine("{0}, {1}", x, y);

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
                    float shortestDist = 9999;

                    for (int i = 0; i < lightSources.Count; i++)
                    {
                        if (MathF.Sqrt(MathF.Pow(x2 - lightSources[i].X, 2) + MathF.Pow(y2 - lightSources[i].Y, 2)) < shortestDist)
                        {
                            shortestDist = MathF.Sqrt(MathF.Pow(x2 - lightSources[i].X, 2) + MathF.Pow(y2 - lightSources[i].Y, 2));
                        }
                    }

                    if (shortestDist < 2.5f)
                    {
                        setLightLevel(x2, y2, 6, player);
                    }
                    else if (shortestDist < 5f)
                    {
                        setLightLevel(x2, y2, 5, player);
                    }
                    else if (shortestDist < 7.5f)
                    {
                        setLightLevel(x2, y2, 4, player);
                    }
                    else if (shortestDist < 10f)
                    {
                        setLightLevel(x2, y2, 3, player);
                    }
                    else if(shortestDist < 12.5f)
                    {
                        setLightLevel(x2, y2, 2, player);
                    }
                    else
                    {
                        setLightLevel(x2, y2, 1, player);
                    }

                }
            }
        }

        private static void setLightLevel(int x, int y, int level, Player player)
        {
            if (World.locations[x, y].lightLevel != level)
            {
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

        public static string getTileColor(int x, int y)
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
    }
}

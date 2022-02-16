using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class Lighting
    {
        private const int RED_BOOST_AMOUNT = 15;
        private const int ORANGE_BOOST_AMOUNT = 7;

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
        /// Sets light levels from passed positions of all light sources
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void SetLightLevels(List<LightSource> lightSources)
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
                        int level = (int)Math.Clamp(lightSources[i].LightPower - ((Math.Clamp(newDist, 2.5, 999) / 2.5f) - 1), 1, 9);


                        //Stop enemy anger feedback loops
                        if (lightSources[i].IfEnemy() && World.GetLocationByPos(x2, y2).IfEnemy() && level >= 2)
                        {
                            //Holds if there is feedback
                            bool feedback = false;
                            foreach (Enemy currentEnemy in ((Enemy)lightSources[i]).beingLitBy) //Go through what this light is being lit by
                            {
                                //If one of those is current tile
                                if (((Enemy)World.GetLocationByPos(x2, y2)) == currentEnemy)
                                {
                                    //Toggle feedback
                                    feedback = true;
                                }
                            }

                            //If feedback
                            if (feedback)
                            {
                                //Turn on overrideLight and stop current loop
                                if(!((Enemy)World.GetLocationByPos(x2, y2)).fleeing) ((Enemy)World.GetLocationByPos(x2, y2)).OverrideLightLevel = 2;
                                continue;
                            }
                        }
                        

                        if (lightSources[i].IfEnemy() && lightSources[i].Pos.Equals(new Point(x2, y2))) //Make sure enemy cant anger itself with light
                        {
                            //Turn on overrideLight and stop current loop
                            if (!((Enemy)World.GetLocationByPos(x2, y2)).fleeing) ((Enemy)World.GetLocationByPos(x2, y2)).OverrideLightLevel = 2;
                            continue;
                        }
                        if (lightSources[i].IfEnemy() && World.GetLocationByPos(x2, y2).IfEnemy() && !((Enemy)lightSources[i]).lit) //Make that enemys fading out dont anger other enemies
                        {
                            //Turn on overrideLight and stop current loop
                            if (!((Enemy)World.GetLocationByPos(x2, y2)).fleeing) ((Enemy)World.GetLocationByPos(x2, y2)).OverrideLightLevel = 2;
                            continue;
                        }
                        else if (level > greatestLightLevel && !LineFinder.BlockedCheck(x2, y2, lightSources[i].Pos))   //If new lightsource is giving better light
                        {
                            //Replace greatestLightLevel and lighter
                            greatestLightLevel = level;
                        }


                        //Get if enemy is lighting enemy
                        if (lightSources[i].IfEnemy() && World.GetLocationByPos(x2, y2).IfEnemy() && level >= 2 && !LineFinder.BlockedCheck(x2, y2, lightSources[i].Pos))
                        {
                            //If neither enemy is fleeing
                            if (!((Enemy)lightSources[i]).fleeing && !((Enemy)World.GetLocationByPos(x2, y2)).fleeing)
                            {
                                //If lightsource enemy is awake, anger the enemy at x2,y2
                                if (((Enemy)lightSources[i]).AgitationLevel == 5)
                                {
                                    ((Enemy)World.GetLocationByPos(x2, y2)).AgitationLevel = Math.Clamp(((Enemy)World.GetLocationByPos(x2, y2)).AgitationLevel + 1, 0, 5);
                                }
                            }

                            //Holds if already logged in on of the enemies
                            bool notRepeat = true;

                            //Check all lit by lists
                            foreach (Enemy currentEnemy in ((Enemy)World.GetLocationByPos(x2, y2)).beingLitBy)
                            {
                                if (((Enemy)lightSources[i]) == currentEnemy)
                                {
                                    notRepeat = false;
                                }
                            }
                            foreach (Enemy currentEnemy in ((Enemy)lightSources[i]).beingLitBy)
                            {
                                if (((Enemy)World.GetLocationByPos(x2, y2)) == currentEnemy)
                                {
                                    notRepeat = false;
                                }
                            }

                            //If no repeated
                            if (notRepeat)
                            {
                                //Log light
                                ((Enemy)World.GetLocationByPos(x2, y2)).beingLitBy.Add((Enemy)lightSources[i]);
                            }
                        }

                        //If red light is close enough to location and not blocked
                        if (lightSources[i].IfFlare() && level >= 2 && !LineFinder.BlockedCheck(x2, y2, lightSources[i].Pos))
                        {
                            //Set redlight to true
                            isRedLight = true;
                        }
                        else if (lightSources[i].IfTorch() && level >= 2 && !LineFinder.BlockedCheck(x2, y2, lightSources[i].Pos))
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

        private static TileVisuals ColorBoosts(int x, int y, TileVisuals currentColor)
        {
            //Adds colorboosts if needed
            if (World.locations[x, y].RedLight)
            {
                currentColor.BackgroundColor.R += RED_BOOST_AMOUNT * (World.locations[x, y].CurrentLightLevel - 1);
            }
            else if (World.locations[x, y].OrangeLight)
            {
                currentColor.BackgroundColor.R += ORANGE_BOOST_AMOUNT * (World.locations[x, y].CurrentLightLevel - 1);
                currentColor.BackgroundColor.G += (ORANGE_BOOST_AMOUNT / 3) * (World.locations[x, y].CurrentLightLevel - 1);
            }

            return currentColor;
        }

        /// <summary>
        /// Returns flare color to be used based off light level
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static TileVisuals GetFlareColor(int x, int y)
        {
            //Holds color for flare
            TileVisuals flareColor;

            //Gets color based off of light level
            switch (World.locations[x, y].CurrentLightLevel)
            {
                case 1:
                    flareColor = new TileVisuals(new Color(0, 0, 0));
                    break;
                case 2:
                    flareColor = new TileVisuals(new Color(51, 4, 4));
                    break;
                case 3:
                    flareColor = new TileVisuals(new Color(105, 15, 15));
                    break;
                case 4:
                    flareColor = new TileVisuals(new Color(156, 31, 31));
                    break;
                case 5:
                    flareColor = new TileVisuals(new Color(209, 56, 56));
                    break;
                case 6:
                    flareColor = new TileVisuals(new Color(222, 71, 71));
                    break;
                case 7:
                    flareColor = new TileVisuals(new Color(235, 87, 87));
                    break;
                case 8:
                    flareColor = new TileVisuals(new Color(247, 104, 104));
                    break;
                case 9:
                    flareColor = new TileVisuals(new Color(255, 120, 120));
                    break;
                default:
                    return null;
            }

            //Returns flareColor
            return flareColor;
        }

        /// <summary>
        /// Returns flare pickup color (not in tile visual form) to be used based off light level
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Color GetFlarePickupColor(int x, int y)
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
                    flareColor = new Color(50, 0, 0);
                    break;
                case 3:
                    flareColor = new Color(100, 0, 0);
                    break;
                case 4:
                    flareColor = new Color(150, 0, 0);
                    break;
                case 5:
                    flareColor = new Color(200, 0, 0);
                    break;
                case 6:
                    flareColor = new Color(255, 0, 0);
                    break;
                case 7:
                    flareColor = new Color(255, 0, 0);
                    break;
                case 8:
                    flareColor = new Color(255, 0, 0);
                    break;
                case 9:
                    flareColor = new Color(255, 0, 0);
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
        public static TileVisuals GetWallTileColor(int x, int y)
        {
            //Holds color for wall
            TileVisuals wallColor;

            //Gets color based off of light level
            switch (World.locations[x, y].CurrentLightLevel)
            {
                case 1:
                    wallColor = new TileVisuals(new Color(0, 0, 0), "░░", new Color(0, 0, 0));
                    break;
                case 2:
                    wallColor = new TileVisuals(new Color(13, 13, 13), "░░", new Color(0, 0, 0));
                    break;
                case 3:
                    wallColor = new TileVisuals(new Color(26, 26, 26), "░░", new Color(0, 0, 0));
                    break;
                case 4:
                    wallColor = new TileVisuals(new Color(38, 38, 38), "░░", new Color(0, 0, 0));
                    break;
                case 5:
                    wallColor = new TileVisuals(new Color(51, 51, 51), "░░", new Color(0, 0, 0));
                    break;
                case 6:
                    wallColor = new TileVisuals(new Color(64, 64, 64), "░░", new Color(0, 0, 0));
                    break;
                case 7:
                    wallColor = new TileVisuals(new Color(77, 77, 77), "░░", new Color(0, 0, 0));
                    break;
                case 8:
                    wallColor = new TileVisuals(new Color(89, 89, 89), "░░", new Color(0, 0, 0));
                    break;
                case 9:
                    wallColor = new TileVisuals(new Color(102, 102, 102), "░░", new Color(0, 0, 0));
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
        public static TileVisuals GetFloorTileColor(int x, int y)
        {
            //Holds color for wall
            TileVisuals floorColor;

            //Gets color based off of light level
            switch (World.locations[x, y].CurrentLightLevel)
            {
                case 1:
                    floorColor = new TileVisuals(new Color(0, 0, 0), "▓▓", 9);
                    break;
                case 2:
                    floorColor = new TileVisuals(new Color(26, 15, 2), "▓▓", 9);
                    break;
                case 3:
                    floorColor = new TileVisuals(new Color(51, 30, 7), "▓▓", 9);
                    break;
                case 4:
                    floorColor = new TileVisuals(new Color(77, 48, 15), "▓▓", 9);
                    break;
                case 5:
                    floorColor = new TileVisuals(new Color(103, 66, 27), "▓▓", 9);
                    break;
                case 6:
                    floorColor = new TileVisuals(new Color(128, 86, 41), "▓▓", 9);
                    break;
                case 7:
                    floorColor = new TileVisuals(new Color(153, 108, 58), "▓▓", 9);
                    break;
                case 8:
                    floorColor = new TileVisuals(new Color(179, 132, 79), "▓▓", 9);
                    break;
                case 9:
                    floorColor = new TileVisuals(new Color(204, 157, 102), "▓▓", 9);
                    break;
                default:
                    return null;
            }

            //Adds color boosts
            floorColor = ColorBoosts(x, y, floorColor);

            //Returns wallColor
            return floorColor;
        }

        /// <summary>
        /// Returns torch color to be used based off light level
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static TileVisuals GetTorchTileColor(int x, int y)
        {
            //Holds color for wall
            TileVisuals torchColor;

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
                        torchColor = new TileVisuals(new Color(0, 0, 0));
                        break;
                    case 2:
                        torchColor = new TileVisuals(new Color(46, 8, 0));
                        break;
                    case 3:
                        torchColor = new TileVisuals(new Color(69, 11, 0));
                        break;
                    case 4:
                        torchColor = new TileVisuals(new Color(105, 18, 0));
                        break;
                    case 5:
                        torchColor = new TileVisuals(new Color(140, 24, 3));
                        break;
                    case 6:
                        torchColor = new TileVisuals(new Color(176, 40, 12));
                        break;
                    case 7:
                        torchColor = new TileVisuals(new Color(189, 51, 23));
                        break;
                    case 8:
                        torchColor = new TileVisuals(new Color(201, 62, 34));
                        break;
                    case 9:
                        torchColor = new TileVisuals(new Color(214, 75, 47));
                        break;
                    default:
                        return null;
                }
            }

            //Returns torchColor
            return torchColor;
        }

        /// <summary>
        /// Returns enemy color to be used based off light level
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static TileVisuals GetEnemyTileColor(int x, int y)
        {
            //Holds color for wall
            TileVisuals floorColor;

            int level = World.locations[x, y].CurrentLightLevel;

            if (((Enemy)World.locations[x, y]).OverrideLightLevel > level)
            {
                level = ((Enemy)World.locations[x, y]).OverrideLightLevel;
            }

            //Gets color based off of light level
            switch (level)
            {
                case 1:
                    floorColor = new TileVisuals(new Color(0, 0, 0), "■■", ((Enemy)World.locations[x, y]).GetBackColor());
                    break;
                case 2:
                    floorColor = new TileVisuals(new Color(58, 16, 5), "■■", ((Enemy)World.locations[x, y]).GetBackColor());
                    break;
                case 3:
                    floorColor = new TileVisuals(new Color(114, 46, 31), "■■", ((Enemy)World.locations[x, y]).GetBackColor());
                    break;
                case 4:
                    floorColor = new TileVisuals(new Color(170, 96, 79), "■■", ((Enemy)World.locations[x, y]).GetBackColor());
                    break;
                case 5:
                    floorColor = new TileVisuals(new Color(190, 116, 99), "■■", ((Enemy)World.locations[x, y]).GetBackColor());
                    break;
                case 6:
                    floorColor = new TileVisuals(new Color(210, 136, 119), "■■", ((Enemy)World.locations[x, y]).GetBackColor());
                    break;
                case 7:
                    floorColor = new TileVisuals(new Color(230, 156, 139), "■■", ((Enemy)World.locations[x, y]).GetBackColor());
                    break;
                case 8:
                    floorColor = new TileVisuals(new Color(250, 176, 169), "■■", ((Enemy)World.locations[x, y]).GetBackColor());
                    break;
                case 9:
                    floorColor = new TileVisuals(new Color(255, 196, 189), "■■", ((Enemy)World.locations[x, y]).GetBackColor());
                    break;
                default:
                    return null;
            }

            //Adds color boosts
            floorColor = ColorBoosts(x, y, floorColor);

            //Returns wallColor
            return floorColor;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class AStarPather
    {
        /// <summary>
        /// Returns file tile of the A* path
        /// </summary>
        /// <returns></returns>
        public static AstarTile PathFinder(Point Pos, Point target, Enemy agent)
        {
            //If pathing not currently delayed
            if (DateTime.Now > agent.PathedDelay)
            {
                //Holds starting tile and sets start to enemy current position
                AstarTile start = new AstarTile(Pos);

                //Holds ending tile and Sets finish to player currentPos
                AstarTile finish = new AstarTile(target);

                ///Sets start's distance to the distance to finish
                start.SetDistance(finish.Pos);

                //Will hold all currently active tiles and adds start to list
                List<AstarTile> activeTiles = new List<AstarTile>() { start };

                //Will hold all previously visited tiles
                List<AstarTile> visitedTiles = new List<AstarTile>();


                while (activeTiles.Count > 0)
                {
                    AstarTile checkTile = activeTiles[0];

                    //Check all active tiles
                    foreach (AstarTile tile in activeTiles)
                    {
                        //If current cost is lower
                        if (tile.Cost < checkTile.Cost)
                        {
                            checkTile = tile;
                        }
                    }

                    if (checkTile.Pos.X == finish.Pos.X && checkTile.Pos.Y == finish.Pos.Y)
                    {
                        return checkTile;
                    }

                    visitedTiles.Add(checkTile);
                    activeTiles.Remove(checkTile);

                    List<AstarTile> walkableTiles = GetWalkableTiles(checkTile, finish);

                    foreach (AstarTile walkableTile in walkableTiles)
                    {
                        //If we have already visited this tile
                        if (visitedTiles.Find(x => x.Pos.X == walkableTile.Pos.X && x.Pos.Y == walkableTile.Pos.Y) != null)
                        {
                            continue;
                        }

                        //If its already in the active list
                        if (activeTiles.Find(x => x.Pos.X == walkableTile.Pos.X && x.Pos.Y == walkableTile.Pos.Y) != null)
                        {
                            AstarTile existingTile = activeTiles.Find(x => x.Pos.X == walkableTile.Pos.X && x.Pos.Y == walkableTile.Pos.Y);
                            if (existingTile.CostDistance > checkTile.CostDistance)
                            {
                                activeTiles.Remove(existingTile);
                                activeTiles.Add(walkableTile);
                            }
                        }
                        else //If have never seen this tile before
                        {
                            activeTiles.Add(walkableTile);
                        }
                    }
                }

                //If failed to find path add one second delay
                agent.PathedDelay = DateTime.Now.AddSeconds(1);
            }

            //If no path found return null
            return null;
        }

        /// <summary>
        /// Returns file tile of the A* path to darkness
        /// </summary>
        /// <returns></returns>
        public static AstarTile DarknessPathFinder(Point Pos, Enemy agent, bool stuck)
        {
            //If pathing not currently delayed
            if (DateTime.Now > agent.PathedDelay)
            {
                //Holds starting tile and sets start to enemy current position
                AstarTile start = new AstarTile(Pos);

                ///Sets start's distance to the distance to finish
                start.SetDistance(Program._player.Pos);

                //Will hold all currently active tiles and adds start to list
                List<AstarTile> activeTiles = new List<AstarTile>() { start };

                //Will hold all previously visited tiles
                List<AstarTile> visitedTiles = new List<AstarTile>();


                while (activeTiles.Count > 0)
                {
                    AstarTile checkTile = activeTiles[0];

                    //Check all active tiles
                    foreach (AstarTile tile in activeTiles)
                    {
                        //If current cost is lower
                        if (tile.Cost < checkTile.Cost)
                        {
                            checkTile = tile;
                        }
                    }

                    //Done check
                    //If at dark tile and not in red light
                    if (World.GetLocationByPos(checkTile.Pos).CurrentLightLevel == 1 && !World.GetLocationByPos(checkTile.Pos).RedLight)
                    {
                        bool bunchedUp = false; //Will hold if enemies are too close to each other

                        //Goes through all enemies
                        foreach (Enemy enemy in World._enemies)
                        {
                            if (!start.Pos.Equals(enemy.Pos)) //If not this enemy
                            {
                                //If enemy is not moving
                                if (enemy.path == null)
                                {
                                    //If too close to enemy current pos
                                    if (MathF.Sqrt(MathF.Pow(checkTile.Pos.X - enemy.Pos.X, 2) + MathF.Pow(checkTile.Pos.Y - enemy.Pos.Y, 2)) < 2.5f)
                                    {
                                        //Set bunched up to true
                                        bunchedUp = true;

                                    }
                                }
                                else if (MathF.Sqrt(MathF.Pow(checkTile.Pos.X - enemy.path.Pos.X, 2) + MathF.Pow(checkTile.Pos.Y - enemy.path.Pos.Y, 2)) < 2.5) //If too close to final pos
                                {
                                    //Set bunched up to true
                                    bunchedUp = true;

                                }
                            }
                        }

                        //If not bunched up
                        if (!bunchedUp)
                        {
                            AstarTile currentTileChecking = checkTile; //Holds tile that will be checked
                            bool farFromLight = true; //Holds if far enough away from light

                            //Will run three steps back into the path
                            for (int i = 0; i < 3; i++)
                            {
                                //If another step exists and is dark
                                if (currentTileChecking.Parent != null && World.GetLocationByPos(currentTileChecking.Pos).CurrentLightLevel == 1)
                                {
                                    //Set new tile to check as tile one step back
                                    currentTileChecking = currentTileChecking.Parent;
                                }
                                else
                                {
                                    //Set farFromLight to false
                                    farFromLight = false;
                                    break;
                                }
                            }

                            //If the enemy is far enough from the light
                            if (farFromLight)
                            {
                                //Return final path
                                return checkTile;
                            }
                        }
                    }

                    visitedTiles.Add(checkTile);
                    activeTiles.Remove(checkTile);

                    //Will hold tiles that can be walked on from current tile
                    List<AstarTile> walkableTiles;

                    //Checks if stuck
                    if (!stuck)
                    {
                        walkableTiles = GetDarkWalkableTiles(checkTile, start);
                    }
                    else
                    {
                        walkableTiles = GetStuckDarkWalkableTiles(checkTile);
                    }

                    foreach (AstarTile walkableTile in walkableTiles)
                    {
                        //If we have already visited this tile
                        if (visitedTiles.Find(x => x.Pos.X == walkableTile.Pos.X && x.Pos.Y == walkableTile.Pos.Y) != null)
                        {
                            continue;
                        }

                        //If its already in the active list
                        if (activeTiles.Find(x => x.Pos.X == walkableTile.Pos.X && x.Pos.Y == walkableTile.Pos.Y) != null)
                        {
                            AstarTile existingTile = activeTiles.Find(x => x.Pos.X == walkableTile.Pos.X && x.Pos.Y == walkableTile.Pos.Y);
                            if (existingTile.InverseCostDistance > checkTile.InverseCostDistance)
                            {
                                activeTiles.Remove(existingTile);
                                activeTiles.Add(walkableTile);
                            }
                        }
                        else //If have never seen this tile before
                        {
                            activeTiles.Add(walkableTile);
                        }
                    }
                }

                //If failed to find path second time add delay
                if (stuck)
                {
                    agent.PathedDelay = DateTime.Now.AddSeconds(0.5f);
                }
            }

            //If no path found return null
            return null;
        }

        /// <summary>
        /// Returns walkable tiles one tile away from currentTile
        /// </summary>
        /// <param name="currentTile"></param>
        /// <param name="targetTile"></param>
        /// <returns></returns>
        public static List<AstarTile> GetWalkableTiles(AstarTile currentTile, AstarTile targetTile)
        {
            //Gets fours directions of tiles from current tile
            List<AstarTile> possibleTiles = new List<AstarTile>()
            {
                 new AstarTile {Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y - 1), Parent = currentTile, Cost = currentTile.Cost + 1 },
                 new AstarTile {Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y + 1), Parent = currentTile, Cost = currentTile.Cost + 1},
                 new AstarTile {Pos = new Point(currentTile.Pos.X - 1, currentTile.Pos.Y), Parent = currentTile, Cost = currentTile.Cost + 1 },
                 new AstarTile {Pos = new Point(currentTile.Pos.X + 1, currentTile.Pos.Y), Parent = currentTile, Cost = currentTile.Cost + 1 },
            };

            //Sets distance for all new tiles
            possibleTiles.ForEach(AstarTile => AstarTile.SetDistance(targetTile.Pos));

            //Go through possible tiles
            for (int i = 0; i < possibleTiles.Count; i++)
            {
                if ((possibleTiles[i].Pos.X >= 0 && possibleTiles[i].Pos.X <= World.MAX_WORLD_X) && //If in X bountds
                    (possibleTiles[i].Pos.Y >= 0 && possibleTiles[i].Pos.Y <= World.MAX_WORLD_Y) && //If in y bounds
                    possibleTiles[i].Cost <= 20 && //Cost not too high
                    (!World.GetLocationByPos(possibleTiles[i].Pos).IfWall()) && //If not wall
                    (!World.GetLocationByPos(possibleTiles[i].Pos).IfEnemy()) && //If not enemy
                    (!World.GetLocationByPos(possibleTiles[i].Pos).IfTorch()) && //If not torch
                    (!World.GetLocationByPos(possibleTiles[i].Pos).IfFlare())) //If not flare
                {
                    //Skip this tile
                    continue;
                }
                else
                {
                    //If not wanted remove this tile
                    possibleTiles.RemoveAt(i);
                    i--;
                }
            }

            //Return possible tiles
            return possibleTiles;
        }

        /// <summary>
        /// Returns walkable tiles one tile away from currentTile based on light
        /// </summary>
        /// <param name="currentTile"></param>
        /// <param name="targetTile"></param>
        /// <returns></returns>
        public static List<AstarTile> GetDarkWalkableTiles(AstarTile currentTile, AstarTile start)
        {
            //Gets fours directions of tiles from current tile
            List<AstarTile> possibleTiles = new List<AstarTile>()
            {
                 new AstarTile {Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y - 1), Parent = currentTile, Cost = currentTile.Cost + 1 },
                 new AstarTile {Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y + 1), Parent = currentTile, Cost = currentTile.Cost + 1},
                 new AstarTile {Pos = new Point(currentTile.Pos.X - 1, currentTile.Pos.Y), Parent = currentTile, Cost = currentTile.Cost + 1 },
                 new AstarTile {Pos = new Point(currentTile.Pos.X + 1, currentTile.Pos.Y), Parent = currentTile, Cost = currentTile.Cost + 1 },
            };

            //Sets distance for all new tiles
            possibleTiles.ForEach(AstarTile => AstarTile.SetDistance(Program._player.Pos));

            //Holds if all paths are lit up from start
            bool allLit = true;

            //Checks if start
            if (currentTile.Pos.Equals(start.Pos))
            {
                //Go through possible tiles
                for (int i = 0; i < possibleTiles.Count; i++)
                {
                    //Get if this tile is not lit
                    if (World.GetLocationByPos(possibleTiles[i].Pos).CurrentLightLevel < 5)
                    {
                        //Sets all lit to false
                        allLit = false;
                    }
                }
            }
            else
            {
                //Sets all lit to false
                allLit = false;
            }

            //Go through possible tiles
            for (int i = 0; i < possibleTiles.Count; i++)
            {
                //Add extra cost for walkthing through redlight
                if (World.GetLocationByPos(possibleTiles[i].Pos).RedLight)
                {
                    possibleTiles[i].Cost += 2;
                }

                //If not all lit
                if (!allLit)
                {
                    //Do normal checks
                    if ((possibleTiles[i].Pos.X >= 0 && possibleTiles[i].Pos.X <= World.MAX_WORLD_X) && //If in X bountds
                        (possibleTiles[i].Pos.Y >= 0 && possibleTiles[i].Pos.Y <= World.MAX_WORLD_Y) && //If in y bounds
                        (World.GetLocationByPos(possibleTiles[i].Pos).CurrentLightLevel < 5) &&
                        (!World.GetLocationByPos(possibleTiles[i].Pos).IfWall()) && //If not wall
                        (!World.GetLocationByPos(possibleTiles[i].Pos).IfTorch()) && //If not torch
                        (!World.GetLocationByPos(possibleTiles[i].Pos).IfFlare())) //If not flare
                    {
                        //Keep this tile
                        continue;
                    }
                    else
                    {
                        //If not wanted remove this tile
                        possibleTiles.RemoveAt(i);
                        i--;
                    }
                }
                else if (allLit) //If all lit
                { 
                    //Do checks without light check
                    if ((possibleTiles[i].Pos.X >= 0 && possibleTiles[i].Pos.X <= World.MAX_WORLD_X) && //If in X bountds
                        (possibleTiles[i].Pos.Y >= 0 && possibleTiles[i].Pos.Y <= World.MAX_WORLD_Y) && //If in y bounds
                        (!World.GetLocationByPos(possibleTiles[i].Pos).IfWall()) && //If not wall
                        (!World.GetLocationByPos(possibleTiles[i].Pos).IfEnemy()) && //If not enemy 
                        (!World.GetLocationByPos(possibleTiles[i].Pos).IfTorch()) && //If not torch
                        (!World.GetLocationByPos(possibleTiles[i].Pos).IfFlare())) //If not flare
                    {
                        //Skip this tile
                        continue;
                    }
                    else
                    {
                        //If not wanted remove this tile
                        possibleTiles.RemoveAt(i);
                        i--;
                    }
                }
            }
            //Return possible tiles
            return possibleTiles;
        }

        /// <summary>
        /// Returns walkable tiles one tile away from currentTile ignoring light and length
        /// </summary>
        /// <param name="currentTile"></param>
        /// <param name="targetTile"></param>
        /// <returns></returns>
        public static List<AstarTile> GetStuckDarkWalkableTiles(AstarTile currentTile)
        {
            //Gets fours directions of tiles from current tile
            List<AstarTile> possibleTiles = new List<AstarTile>()
            {
                 new AstarTile {Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y - 1), Parent = currentTile, Cost = currentTile.Cost + 1 },
                 new AstarTile {Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y + 1), Parent = currentTile, Cost = currentTile.Cost + 1},
                 new AstarTile {Pos = new Point(currentTile.Pos.X - 1, currentTile.Pos.Y), Parent = currentTile, Cost = currentTile.Cost + 1 },
                 new AstarTile {Pos = new Point(currentTile.Pos.X + 1, currentTile.Pos.Y), Parent = currentTile, Cost = currentTile.Cost + 1 },
            };

            //Sets distance for all new tiles
            possibleTiles.ForEach(AstarTile => AstarTile.SetDistance(Program._player.Pos));

            //Go through possible tiles
            for (int i = 0; i < possibleTiles.Count; i++)
            {
                //Add extra cost for walkthing through light
                if (World.GetLocationByPos(possibleTiles[i].Pos).CurrentLightLevel > 1)
                {
                    if (World.GetLocationByPos(possibleTiles[i].Pos).RedLight)
                    {
                        possibleTiles[i].Cost += 2;
                    }

                    possibleTiles[i].Cost += World.GetLocationByPos(possibleTiles[i].Pos).CurrentLightLevel - 1;
                }

                //Do checks
                if ((possibleTiles[i].Pos.X >= 0 && possibleTiles[i].Pos.X <= World.MAX_WORLD_X) && //If in X bountds
                    (possibleTiles[i].Pos.Y >= 0 && possibleTiles[i].Pos.Y <= World.MAX_WORLD_Y) && //If in y bounds
                    (!World.GetLocationByPos(possibleTiles[i].Pos).IfWall()) && //If not wall
                    (!World.GetLocationByPos(possibleTiles[i].Pos).IfEnemy()) && //If not enemy
                    (!World.GetLocationByPos(possibleTiles[i].Pos).IfTorch()) && //If not torch
                    (!World.GetLocationByPos(possibleTiles[i].Pos).IfFlare())) //If not flare
                {
                    //Keep this tile
                    continue;
                }
                else
                {
                    //If not wanted remove this tile
                    possibleTiles.RemoveAt(i);
                    i--;
                }
            }
            //Return possible tiles
            return possibleTiles;
        }
    }
}

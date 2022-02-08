using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Enemy : LightSource
    {
        const int ENEMY_LIGHT_LEVEL = 3;
        const float MOVE_DELAY = 0.17f;

        //Sleeping info
        public int AgitationLevel = 0; //Holds how angry the enemy is
        DateTime LastAgitatiedTime = DateTime.Now; //Holds last time agitatied
        DateTime LastBlinkedTime = DateTime.Now; //Holds last time blinked
        public int blinkStatus = 0; //Holds if back should be yellow currently
        public int OverrideLightLevel = 0; //Holds light level that doesnt effect agitation
        public bool lit = false; //Holds if this enemy is being directly lit
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
        public List<Enemy> beingLitBy = new List<Enemy>(); //Hold all enemies that are lighting this enemy
        
        //Pathing info
        AstarTile path = null; //Holds path to player
        private Location on; //Holds what the enemy is on
        DateTime LastMovedTime = DateTime.Now; //Holds last time moved
        DateTime PathedDelay = DateTime.Now; //Holds delay for pathfinding
        public bool fleeing = false; //Holds if enemy is fleeing light
        public Point fleeingLocation = null;

        DateTime fullDelay= DateTime.Now; //Delays everything

        public Enemy(Point pos) : base(pos)
        {
            LightPower = 0;
            on = new Floor();

            World._enemies.Add(this);
        }

        public void EnemyUpdate()
        {
            //If not currently delayed
            if (DateTime.Now > fullDelay)
            {
                if (fleeing) //Fleeing back to darkness
                {
                    //If move wait time is up
                    if (DateTime.Now >= LastMovedTime.AddSeconds(MOVE_DELAY))
                    {
                        //Lower agitation while fleeing
                        AgitationLevel--;

                        //If havent found fleeing location
                        if (fleeingLocation == null)
                        {
                            Point bestPoint = null;
                            float distance = 9999;

                            //Goes through all locations
                            for (int x2 = 0; x2 < World.MAX_WORLD_X; x2++)
                            {
                                for (int y2 = 0; y2 < World.MAX_WORLD_Y; y2++)
                                {
                                    if (World.GetLocationByPos(x2, y2).IfFloor()) //If floor
                                    {
                                        if (World.GetLocationByPos(x2, y2).CurrentLightLevel == 1 && //If in darkeness
                                            MathF.Sqrt(MathF.Pow(x2 - Pos.X, 2) + MathF.Pow(y2 - Pos.Y, 2)) < distance) //If new closest 
                                        {
                                            ///Will hold if location is taken
                                            bool taken = false;

                                            //Goes through all other enemies
                                            foreach (Enemy enemy in World._enemies)
                                            {
                                                //Checks if fleeing location matches current point
                                                if (enemy.fleeingLocation != null && enemy.fleeingLocation.Equals(new Point(x2, y2)))
                                                {
                                                    taken = true;
                                                }
                                            }

                                            //If that point is not already taken by another enemy
                                            if (!taken)
                                            {
                                                //Set bestPoint and distance to new tile's values
                                                bestPoint = new Point(x2, y2);
                                                distance = MathF.Sqrt(MathF.Pow(x2 - Pos.X, 2) + MathF.Pow(y2 - Pos.Y, 2));
                                            }
                                        }
                                    }
                                }
                            }

                            //Set fleeing location to new point and remove old path
                            fleeingLocation = bestPoint;
                            path = null;
                        }

                        System.Diagnostics.Debug.WriteLine($"{fleeingLocation.X}, {fleeingLocation.Y}");

                        //Runs movement
                        Move(fleeingLocation);

                        //Get new time if not already reset
                        if (DateTime.Now > LastMovedTime) LastMovedTime = DateTime.Now;
                    }
                }
                else if (AgitationLevel == 5) //Chasing
                {
                    //If too bright
                    if (CurrentLightLevel >= 5 || RedLight)
                    {
                        //Set fleeing to true
                        fleeing = true;
                    }
                    else
                    {
                        //If move wait time is up
                        if (DateTime.Now >= LastMovedTime.AddSeconds(MOVE_DELAY))
                        {
                            LightPower = ENEMY_LIGHT_LEVEL;

                            //Runs movement
                            Move(Program._player.Pos);

                            //Get new time if not already reset
                            if (DateTime.Now > LastMovedTime) LastMovedTime = DateTime.Now;
                        }
                    }
                }
                else //Sleeping
                {
                    //Empty path if sleeping
                    path = null;


                    if (CurrentLightLevel > 1)
                    {
                        lit = true;
                    }
                    else
                    {
                        lit = false;
                    }

                    if (DateTime.Now >= LastAgitatiedTime.AddSeconds(0.4f) && CurrentLightLevel > 1) //If in light and wait time is over
                    {
                        AgitationLevel = Math.Clamp(AgitationLevel + 1, 0, 5);
                        if (AgitationLevel >= 2)
                        {
                            LightPower = 2;
                            Lighting.LightingUpdate();
                            OverrideLightLevel = 2;
                            Map.RedrawMapPoint(Pos);
                        }

                        //Get new time
                        LastAgitatiedTime = DateTime.Now;
                    }
                    else if (DateTime.Now >= LastAgitatiedTime.AddSeconds(0.7f) && CurrentLightLevel == 1) //If not in light and wait time is over
                    {
                        AgitationLevel = Math.Clamp(AgitationLevel - 1, 0, 5);
                        if (AgitationLevel < 2)
                        {
                            LightPower = 1;
                            Lighting.LightingUpdate();
                            OverrideLightLevel = 0;
                            Map.RedrawMapPoint(Pos);
                            beingLitBy.Clear();
                        }

                        //Get new time
                        LastAgitatiedTime = DateTime.Now;
                    }
                }
            }
            else 
            {
                //Turn of fleeling
                fleeing = false;

                //Reset angry info
                AgitationLevel = 0;
                fleeingLocation = null;
                path = null;
            }

            //If agitated and blink time is up
            if (AgitationLevel > 0 && DateTime.Now >= LastBlinkedTime.AddSeconds(0.5f / AgitationLevel))
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
                LastBlinkedTime = DateTime.Now;
            }
            else if (AgitationLevel == 0) //If not agitated
            {
                //Turn off blink
                blinkStatus = 0;


                //Turn everything else off
                LightPower = 1;
                Lighting.LightingUpdate();
                OverrideLightLevel = 0;
                Map.RedrawMapPoint(Pos);
                beingLitBy.Clear();
            }
        }

        //Runs movement for current path
        private void Move(Point location)
        {
            if (path != null)
            {
                //If player not at end of path refind path
                if (!Program._player.Pos.Equals(path.Pos))
                {
                    getPath(location);
                }

                AstarTile nextMove = path; //Will hold next move

                //Get move that is one after current pos
                while (nextMove.Parent != null && !nextMove.Parent.Pos.Equals(Pos))
                {
                    nextMove = nextMove.Parent;
                }

                //Check for player
                if (nextMove.Pos.X == Program._player.Pos.X && nextMove.Pos.Y == Program._player.Pos.Y)
                {
                    //End game
                    while (true)
                    {
                        Console.ReadKey();
                    }
                }

                if (!World.GetLocationByPos(nextMove.Pos).IfEnemy()) //If not moving into enemy
                {
                    //Move enemy
                    MovementUpdate(Pos, nextMove.Pos);
                }
                else
                {
                    getPath(location);
                }

                //Check for end point
                if (fleeingLocation != null && nextMove.Pos.X == fleeingLocation.X && nextMove.Pos.Y == fleeingLocation.Y)
                {
                    //Delay everything to make sure it stays calm
                    fullDelay = DateTime.Now.AddSeconds(10);
                }
            }
            else
            {
                getPath(location);
            }
        }

        private void getPath(Point location)
        {
            //Gets new path
            AstarTile newPath = PathFinder(location);
            if (newPath != null) //If new path found
            {
                //Update path and light
                path = newPath;
            }
        }

        public Color GetBackColor()
        {
            return backLightColors[Math.Clamp((CurrentLightLevel + blinkStatus) - 1, 0, 8)];
        }

        /// <summary>
        /// Returns file tile of the A* path
        /// </summary>
        /// <returns></returns>
        private AstarTile PathFinder(Point target)
        {
            //If pathing not currently delayed
            if (DateTime.Now > PathedDelay)
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
                    AstarTile checkTile = activeTiles[0];//.OrderBy(x => x.CostDistance).First();

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
                PathedDelay = DateTime.Now.AddSeconds(1);
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
        private static List<AstarTile> GetWalkableTiles(AstarTile currentTile, AstarTile targetTile)
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
                    possibleTiles[i] .Cost <= 20 && //Cost not too high
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

        private void MovementUpdate(Point oldPos, Point newPos)
        {
            Pos = newPos;

            //Update location lightSources
            World.SetLocationByPos(oldPos, on); //Remove light source

            on = World.GetLocationByPos(newPos); //Updates on

            //Set new enemy pos in Locations[] and update lighting
            World.SetLocationByPos(newPos, this);
            Lighting.LightingUpdate();

            //Redraw new location and old location
            Map.RedrawMapPoint(oldPos);
            Map.RedrawMapPoint(newPos);
        }

        public override bool IfEnemy()
        {
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Enemy : LightSource
    {
        const int ENEMY_LIGHT_LEVEL = 3; 

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
        bool fleeing = false; //Holds if enemy is fleeing light
        Point fleeingLocation = null;

        public Enemy(Point pos) : base(pos)
        {
            LightPower = 0;
            on = new Floor();

            World._enemies.Add(this);
        }

        public void EnemyUpdate()
        {
            if(fleeing) //Fleeing back to darkness
            {
                //If havent found fleeing location
                if (fleeingLocation == null)
                { 
                    
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
                    if (DateTime.Now >= LastMovedTime.AddSeconds(0.15f))
                    {
                        //Update path and light
                        path = PathFinder();
                        LightPower = ENEMY_LIGHT_LEVEL;

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
                        else //If not player
                        {
                            //Move enemy
                            MovementUpdate(Pos, nextMove.Pos);
                        }

                        //Get new time
                        LastMovedTime = DateTime.Now;
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

                if (DateTime.Now >= LastAgitatiedTime.AddSeconds(0.55f) && CurrentLightLevel > 1) //If in light and wait time is over
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
                        LightPower = 0;
                        Lighting.LightingUpdate();
                        OverrideLightLevel = 0;
                        Map.RedrawMapPoint(Pos);
                        beingLitBy.Clear();
                    }

                    //Get new time
                    LastAgitatiedTime = DateTime.Now;
                }
            }

            //If agitated and blick time is up
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
        private AstarTile PathFinder()
        {
            //Holds starting tile and sets start to enemy current position
            AstarTile start = new AstarTile(Pos);

            //Holds ending tile and Sets finish to player currentPos
            AstarTile finish = new AstarTile(Program._player.Pos);

            ///Sets start's distance to the distance to finish
            start.SetDistance(finish.Pos);

            //Will hold all currently active tiles and adds start to list
            List<AstarTile> activeTiles = new List<AstarTile>() {start};

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
                    //Console.WriteLine("We are at the destination!");
                    //We can actually loop through the parents of each tile to find our exact path which we will show shortly. 
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

                    //It's already in the active list
                    if (activeTiles.Find(x => x.Pos.X == walkableTile.Pos.X && x.Pos.Y == walkableTile.Pos.Y) != null)
                    {
                        AstarTile existingTile = activeTiles.Find(x => x.Pos.X == walkableTile.Pos.X && x.Pos.Y == walkableTile.Pos.Y);
                        if (existingTile.CostDistance > checkTile.CostDistance)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else //If never seen this tile before
                    {
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            //If no path found return empty path on self to stop movement
            return new AstarTile(Pos);
        }

        /// <summary>
        /// Returns walkable tiles one tile away from currentTile
        /// </summary>
        /// <param name="currentTile"></param>
        /// <param name="targetTile"></param>
        /// <returns></returns>
        private static List<AstarTile> GetWalkableTiles(AstarTile currentTile, AstarTile targetTile)
        {
            List<AstarTile> possibleTiles = new List<AstarTile>()
            {
                 new AstarTile {Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y - 1), Parent = currentTile, Cost = currentTile.Cost + 1 },
                 new AstarTile {Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y + 1), Parent = currentTile, Cost = currentTile.Cost + 1},
                 new AstarTile {Pos = new Point(currentTile.Pos.X - 1, currentTile.Pos.Y), Parent = currentTile, Cost = currentTile.Cost + 1 },
                 new AstarTile {Pos = new Point(currentTile.Pos.X + 1, currentTile.Pos.Y), Parent = currentTile, Cost = currentTile.Cost + 1 },
	        };

	        possibleTiles.ForEach(AstarTile => AstarTile.SetDistance(targetTile.Pos));

            for (int i = 0; i < possibleTiles.Count; i++)
            {
                if ((possibleTiles[i].Pos.X >= 0 && possibleTiles[i].Pos.X <= World.MAX_WORLD_X) &&
                    (possibleTiles[i].Pos.Y >= 0 && possibleTiles[i].Pos.Y <= World.MAX_WORLD_Y) &&
                    (!World.GetLocationByPos(possibleTiles[i].Pos).IfWall()) && 
                    (!World.GetLocationByPos(possibleTiles[i].Pos).IfEnemy()))
                {
                    continue;
                }
                else
                {
                    possibleTiles.RemoveAt(i);
                    i--; //This line might not work
                }
            }

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

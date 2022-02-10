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
        public DateTime PathedDelay = DateTime.Now; //Holds delay for pathfinding

        //Fleeing info
        public bool fleeing = false; //Holds if enemy is fleeing light
        DateTime fullDelay = DateTime.Now; //Delays everything
        DateTime fleeDelay = DateTime.MinValue; //Delays fleeing

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

                        //Runs fleeing movement
                        FleeingMove();

                        //Get new time if not already reset
                        if (DateTime.Now > LastMovedTime) LastMovedTime = DateTime.Now;
                    }
                }
                else if (AgitationLevel == 5) //Chasing
                {
                    //If too bright
                    if ((CurrentLightLevel >= 5 || RedLight) && fleeDelay == DateTime.MinValue)
                    {
                        //Starts fleeing
                        selfFleeing();

                        //Starts fleeDelay
                        fleeDelay = DateTime.Now.AddSeconds(2);
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

            if (fleeDelay != DateTime.MinValue && DateTime.Now > fleeDelay)
            {
                AllFleeing();
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
                    GetPath(location);
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
                    //If player is in light when about to kill them
                    if (World.GetLocationByPos(Program._player.Pos).CurrentLightLevel >= 5 && fleeDelay == DateTime.MinValue)
                    {
                        //Starts fleeDelay
                        fleeDelay = DateTime.Now.AddSeconds(2);
                    }
                    else if (World.GetLocationByPos(Program._player.Pos).RedLight && fleeDelay == DateTime.MinValue) //If player is in redlight when about to kill them
                    {
                        //Starts fleeDelay
                        fleeDelay = DateTime.Now.AddSeconds(0);
                    }

                    else //If player is in darkness too
                    {
                        //End game
                        while (true)
                        {
                            Console.ReadKey();
                        }
                    }
                }
                else if (!World.GetLocationByPos(nextMove.Pos).IfEnemy()) //If not moving into enemy
                {
                    //If not about to move into light
                    if (World.GetLocationByPos(nextMove.Pos).CurrentLightLevel < 5)
                    {
                        //Move enemy
                        MovementUpdate(Pos, nextMove.Pos);
                    }
                    else if(fleeDelay == DateTime.MinValue)
                    {
                        //If redlight
                        if (World.GetLocationByPos(nextMove.Pos).RedLight)
                        {
                            //Starts fleeDelay
                            fleeDelay = DateTime.Now.AddSeconds(0);
                        }
                        else
                        {
                            //Starts fleeDelay
                            fleeDelay = DateTime.Now.AddSeconds(2);
                        }
                    }
                }
                else
                {
                    GetPath(location);
                }
            }
            else
            {
                GetPath(location);
            }
        }

        //Runs movement for fleeing path
        private void FleeingMove()
        {
            if (path != null)
            {
                AstarTile nextMove = path; //Will hold next move

                //Get move that is one after current pos
                while (nextMove.Parent != null && !nextMove.Parent.Pos.Equals(Pos))
                {
                    nextMove = nextMove.Parent;
                }

                if (!World.GetLocationByPos(nextMove.Pos).IfEnemy()) //If not moving into enemy
                {
                    //Move enemy
                    MovementUpdate(Pos, nextMove.Pos);
                }
                else
                {
                    GetDarkPath();
                }

                //Check for end point
                if (nextMove.Pos.X == path.Pos.X && nextMove.Pos.Y == path.Pos.Y)
                {
                    //Delay everything to make sure it stays calm
                    fullDelay = DateTime.Now.AddSeconds(2);
                }
            }
            else
            {
                GetDarkPath();
            }
        }

        private void GetDarkPath()
        {
            //Gets new path
            AstarTile newPath = AStarPather.DarknessPathFinder(Pos, this);
            if (newPath != null) //If new path found
            {
                //Update path and light
                path = newPath;
            }
        }

        private void GetPath(Point location)
        {
            //Gets new path
            AstarTile newPath = AStarPather.PathFinder(Pos, location, this);
            if (newPath != null) //If new path found
            {
                //Update path and light
                path = newPath;
            }
        }

        private void AllFleeing()
        {
            Random r = new Random();

            //Go through all enemies
            foreach (Enemy enemy in World._enemies)
            {
                if (enemy.AgitationLevel == 5)
                {
                    //Start fleeing and Reset path and flee delay
                    enemy.fleeDelay = DateTime.MinValue;
                    enemy.fleeing = true;
                    enemy.path = null;
                }
            }
        }

        private void selfFleeing()
        {
            //Start fleeing and Reset path
            fleeing = true;
            path = null;
        }

        public Color GetBackColor()
        {
            return backLightColors[Math.Clamp((CurrentLightLevel + blinkStatus) - 1, 0, 8)];
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

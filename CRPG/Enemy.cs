using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Enemy : LightSource
    {
        //private int enemyLightLevel = 5; //Starts with a default of 5

        //bool Moving = true; //Holds if enemy is moveing

        int AgitationLevel = 0; //Holds how angry the enemy is
        DateTime LastAgitatiedTime = DateTime.Now; //Holds last time agitatied
        DateTime LastBlinkedTime = DateTime.Now; //Holds last time blinked
        public int blinkStatus = 0;
        public int OverrideLightLevel = 0;
        public bool lit = false;

        List<AstarTile> currentPath = null;
        public List<Enemy> beingLitBy = new List<Enemy>();

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
        };

        private Location on; //Holds what the enemy is on

        public Enemy(Point pos) : base(pos)
        {
            LightPower = 0;
            on = new Floor();

            World._enemies.Add(this);
        }

        public void EnemyUpdate()
        {
            if (CurrentLightLevel > 1)
            {
                lit = true;
            }
            else
            {
                lit = false;
            }


            if (DateTime.Now >= LastAgitatiedTime.AddSeconds(0.5f) && CurrentLightLevel > 1) //If in light and wait time is over
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
            else if (DateTime.Now >= LastAgitatiedTime.AddSeconds(0.7f) && CurrentLightLevel == 1) //If in light and wait time is over
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

            if (AgitationLevel > 0 && DateTime.Now >= LastBlinkedTime.AddSeconds(0.5f / AgitationLevel))
            {
                if (blinkStatus == 0)
                {
                    blinkStatus += AgitationLevel;
                }
                else
                {
                    blinkStatus = 0;
                }

                Map.RedrawMapPoint(Pos);

                //Get new time
                LastBlinkedTime = DateTime.Now;
            }
            else if (AgitationLevel == 0)
            {
                blinkStatus = 0;
            }
        }

        public Color GetBackColor()
        {
            return backLightColors[Math.Clamp((CurrentLightLevel + blinkStatus) - 1, 0, 8)];
        }


        private void MoveSetup()
        {
            //Holds starting tile 
            AstarTile start = new AstarTile();
            start.Pos = Pos; //Sets start to enemy current position

            //Holds ending tile
            AstarTile finish = new AstarTile();
            finish.Pos = Program._player.Pos; //Sets finish to player currentPos

            ///Sets start's distance to the distance to finish
            start.SetDistance(finish.Pos);

            //Will hold all currently active tiles
            List<AstarTile> activeTiles = new List<AstarTile>();
            activeTiles.Add(start); //Adds start to activeTiles list

            //Will hold all previously visited tiles
            List<AstarTile> visitedTiles = new List<AstarTile>();
        }

        /*
        private static List<AstarTile> getWalkableTiles(AstarTile currentTile, AstarTile targetTile)
        {
            List<AstarTile> possibleTiles = new List<AstarTile>()
            {
                 new AstarTile {Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y - 1), Parent = currentTile, Cost = currentTile.Cost + 1 },
                 new AstarTile {Pos = new Point(currentTile.Pos.X, currentTile.Pos.Y + 1), Parent = currentTile, Cost = currentTile.Cost + 1},
                 new AstarTile {Pos = new Point(currentTile.Pos.X - 1, currentTile.Pos.Y), Parent = currentTile, Cost = currentTile.Cost + 1 },
                 new AstarTile {Pos = new Point(currentTile.Pos.X + 1, currentTile.Pos.Y), Parent = currentTile, Cost = currentTile.Cost + 1 },
	        };

	        possibleTiles.ForEach(tile => tile.SetDistance(targetTile.Pos));

            

	        return possibleTiles
			        .Where(tile => tile.X >= 0 && tile.X <= maxX)
			        .Where(tile => tile.Y >= 0 && tile.Y <= maxY)
			        .Where(tile => map[tile.Y][tile.X] == ' ' || map[tile.Y][tile.X] == 'B')
			        .ToList();
        }

        private List<AstarTile> getFinalPath()
        {
            return null;
        }
        */

        /*
        private void MovementUpdate(Point oldPos, Point newPos)
        {
            //Update location lightSources
            if (!oldPos.Equals(Program._player.Pos)) World.SetLocationByPos(oldPos, on); //Remove light source
            else World.SetLocationByPos(oldPos, new LightSource(oldPos, Player.PLAYER_LIGHT_LEVEL)); //Give player light back

            on = World.GetLocationByPos(newPos); //Updates on

            //Set new flare pos in Locations[] and update lighting
            World.SetLocationByPos(newPos, this);
            Lighting.LightingUpdate();

            //Redraw new location and old location
            Map.RedrawMapPoint(oldPos);
            Map.RedrawMapPoint(newPos);
        }
        */

        public override bool IfEnemy()
        {
            return true;
        }
    }
}

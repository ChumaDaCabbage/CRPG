using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class Tutorial
    {
        public static bool tutorial = true;

        static int playerDirection = 1;
        static int FlareDirection = 1;
        static int counter = 5;
        static Player tutorialRealPlayer;
        static Player flarePlayer;
        public static Torch tutorialTorch;
        public static TutorialEnemy[] tEnemies = new TutorialEnemy[2];
        static World tutorialWorld = new World();

        public static void StartTutorial()
        {
            #region Setup
            //Credits to https://patorjk.com/software/taag/ for the awesome ascii art
            Console.SetCursorPosition(29, 3);
            Console.WriteLine("████████ ██    ██ ████████  ██████  ██████  ██  █████  ██      ");
            Console.SetCursorPosition(29, 4);
            Console.WriteLine("   ██    ██    ██    ██    ██    ██ ██   ██ ██ ██   ██ ██      ");
            Console.SetCursorPosition(29, 5);
            Console.WriteLine("   ██    ██    ██    ██    ██    ██ ██████  ██ ███████ ██      ");
            Console.SetCursorPosition(29, 6);
            Console.WriteLine("   ██    ██    ██    ██    ██    ██ ██   ██ ██ ██   ██ ██      ");
            Console.SetCursorPosition(29, 7);
            Console.WriteLine("   ██     ██████     ██     ██████  ██   ██ ██ ██   ██ ███████ ");

            //Creates real players
            tutorialRealPlayer = new Player();
            flarePlayer = new Player();
            Program._player = tutorialRealPlayer;

            //Sets up world
            tutorialWorld.TutorialWorldSetup();
            Program._world = tutorialWorld;

            //Sets players in world
            tutorialRealPlayer.MoveTo(new Point(7, 4));
            flarePlayer.MoveTo(new Point(52, 4));
            Program._world.locations[7, 14] = new TutorialPlayer(new Point(7, 14));
            Program._world.locations[5, 23] = new TutorialPlayer(new Point(5, 23));
            Program._world.locations[52, 4] = new TutorialPlayer(new Point(52, 4));
            Program._world.locations[52, 25] = new TutorialPlayer(new Point(52, 25));
            Program._world.locations[20, 18] = new TutorialPlayer(new Point(20, 18));
            Program._world.locations[36, 18] = new TutorialPlayer(new Point(36, 18));

            //Starts lighting 
            Lighting.LightingUpdate();

            //Writes tutorial text over world
            Console.SetCursorPosition(6, 8);
            Console.WriteLine("WASD to move player");
            Console.SetCursorPosition(1, 19);
            Console.WriteLine("Space to light adjacent torches");
            Console.SetCursorPosition(5, 29);
            Console.WriteLine("Find the green square");
            Console.SetCursorPosition(8, 30);
            Console.WriteLine("Touch it to win");
            Console.SetCursorPosition(96, 8);
            Console.WriteLine("Use the Arrow keys");
            Console.SetCursorPosition(97, 9);
            Console.WriteLine("to shoot flares");
            Console.SetCursorPosition(95, 17);
            Console.WriteLine("You have five flares");
            Console.SetCursorPosition(96, 18);
            Console.WriteLine("Shown in this bar");
            Console.SetCursorPosition(90, 29);
            Console.WriteLine("Flare pickups restore a flare");
            Console.SetCursorPosition(92, 30);
            Console.WriteLine("(Single use in real game)");
            Console.SetCursorPosition(37, 26);
            Console.WriteLine("Enemies start to");
            Console.SetCursorPosition(37, 27);
            Console.WriteLine("wake up in light");
            Console.SetCursorPosition(60, 26);
            Console.WriteLine("Enemies are stopped and scared");
            Console.SetCursorPosition(67, 27);
            Console.WriteLine("by bright light");

            FlareInventory.InventorySetup(); //Sets up flare inventory

            #endregion

            #region  Update
            DateTime delayTime = DateTime.Now.AddSeconds(0.75f); //Holds delay between text changes
            bool visable = true; //Holds if text is visable

            //Start delay to prevent accidental skip
            while (delayTime > DateTime.Now) {}

            //While no player input
            while (!Console.KeyAvailable)
            {
                //Start uutorial update
                TutorialUpdate();

                if (delayTime <= DateTime.Now && visable)  //If delay is over and text is visable
                {
                    //Start player tutorial update
                    TutorialPlayerUpdate();

                    Console.SetCursorPosition(47, 11); //Set curser pos
                    Console.WriteLine("Press any Key to begin!"); //Write text
                    delayTime = DateTime.Now.AddSeconds(1f); //Get new delay
                    visable = !visable; //Flip visable
                }
                else if (delayTime <= DateTime.Now && !visable) //If delay is over and text is not visable
                {
                    //Start player tutorial update
                    TutorialPlayerUpdate();

                    Console.SetCursorPosition(47, 11); //Set curser pos
                    Console.WriteLine("                       "); //Remove text
                    delayTime = DateTime.Now.AddSeconds(1f); //Get new delay
                    visable = !visable; //Flip visable
                }
            }
            #endregion

            //Read key and clear console
            Console.ReadKey();
            Console.Clear();

            //Set tutorial to false
            tutorial = false;
        }

        private static void TutorialUpdate()
        {
            //Get current time for flares
            Program.CurrentTimeThisFrame = DateTime.Now;

            //Update all flares
            for (int i = 0; i < flarePlayer._flares.Count; i++)
            {
                flarePlayer._flares[i].FlareUpdate(flarePlayer);
            }

            //Update all Enemies
            for (int i = 0; i < tEnemies.Length; i++)
            {
               tEnemies[i].EnemyUpdate();
            }
        }

        private static void TutorialPlayerUpdate()
        {
            //Cycle counter 1-6
            counter++;
            if (counter > 6) counter = 1;

            #region Movement Tutorial
            //Move player in + shape repeatly
            switch (playerDirection)
            {
                case 1:
                    tutorialRealPlayer.MoveWest();
                    playerDirection++;
                    break;

                case 2:
                    tutorialRealPlayer.MoveEast();
                    playerDirection++;
                    break;

                case 3:
                    tutorialRealPlayer.MoveNorth();
                    playerDirection++;
                    break;

                case 4:
                    tutorialRealPlayer.MoveSouth();
                    playerDirection++;
                    break;

                case 5:
                    tutorialRealPlayer.MoveEast();
                    playerDirection++;
                    break;

                case 6:
                    tutorialRealPlayer.MoveWest();
                    playerDirection++;
                    break;

                case 7:
                    tutorialRealPlayer.MoveSouth();
                    playerDirection++;
                    break;

                case 8:
                    tutorialRealPlayer.MoveNorth();
                    playerDirection = 1;
                    break;
            }
            #endregion

            #region Torch Tutorial
            //Toggle torch on and off
            if (!tutorialTorch.on)
            {
                tutorialTorch.TurnOnTorch();
            }
            else
            {
                tutorialTorch.TurnOffTorch();
            }
            #endregion

            #region flare shooting
            //On six
            if (counter == 6)
            {
                //Shot flares in a + shape
                switch(FlareDirection)
                {
                    case 1:
                        flarePlayer.Shoot(new Point(-2, 0), new Point(flarePlayer.Pos.X, flarePlayer.Pos.Y));
                        FlareDirection++;
                        break;

                    case 2:
                        flarePlayer.Shoot(new Point(0, 2), new Point(flarePlayer.Pos.X, flarePlayer.Pos.Y));
                        FlareDirection++;
                        break;

                    case 3:
                        flarePlayer.Shoot(new Point(2, 0), new Point(flarePlayer.Pos.X, flarePlayer.Pos.Y));
                        FlareDirection++;
                        break;

                    case 4:
                        flarePlayer.Shoot(new Point(0, -2), new Point(flarePlayer.Pos.X, flarePlayer.Pos.Y));
                        FlareDirection = 1;
                        break;
                }               
            }
            #endregion

            #region flare Picking Up
            if (counter == 2) //On 2
            {
                Program._world.locations[52, 25] = new Floor(); //Set floor
                Program._world.locations[53, 25] = new TutorialPlayer(new Point(53, 25)); //Set player

                //Redraw map points
                Map.RedrawMapPoint(new Point(52, 25));
                Map.RedrawMapPoint(new Point(53, 25));

                //Update lighting
                Lighting.LightingUpdate();

                //Add flare
                FlareInventory.FlareCount++;
                FlareInventory.DrawFlareBar(FlareInventory.tutorialBarPos);
            }
            else if (counter == 3) //On 3
            {
                Program._world.locations[52, 25] = new TutorialPlayer(new Point(52, 25)); //Set player
                Program._world.locations[53, 25] = new Floor(); //Set floor
                ((Floor)Program._world.locations[53, 25]).HasFlare = true; //Give floor a flare

                //Redraw map points
                Map.RedrawMapPoint(new Point(52, 25));
                Map.RedrawMapPoint(new Point(53, 25));

                //Update lighting
                Lighting.LightingUpdate();
            }
            #endregion
        }
    }
}

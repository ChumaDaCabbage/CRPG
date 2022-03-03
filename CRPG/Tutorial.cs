using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class Tutorial
    {

        //WASD to move player

        //Space to light adjacent torches

        //Find the green square, stand on it to win

        //Arrow keys to shoot flares
        //You have five flares, shown in this bar
        //Flare pickups can replish a flare

        //Enemies start to wake up in light

        //Enemies are stopped and scared of bright light

        //Fully awake enemies will try to kill you

        static Player tutorialPlayer;
        static World tutorialWorld = new World();

        public static void StartTutorial()
        {
            //Credits to https://patorjk.com/software/taag/ for the awesome ascii art
            Console.SetCursorPosition(28, 3);
            Console.WriteLine("████████ ██    ██ ████████  ██████  ██████  ██  █████  ██      ");
            Console.SetCursorPosition(28, 4);
            Console.WriteLine("   ██    ██    ██    ██    ██    ██ ██   ██ ██ ██   ██ ██      ");
            Console.SetCursorPosition(28, 5);
            Console.WriteLine("   ██    ██    ██    ██    ██    ██ ██████  ██ ███████ ██      ");
            Console.SetCursorPosition(28, 6);
            Console.WriteLine("   ██    ██    ██    ██    ██    ██ ██   ██ ██ ██   ██ ██      ");
            Console.SetCursorPosition(28, 7);
            Console.WriteLine("   ██     ██████     ██     ██████  ██   ██ ██ ██   ██ ███████ ");

            tutorialPlayer = new Player(); //Creates new player
            Program._player = tutorialPlayer;
            tutorialWorld.TutorialWorldSetup();
            Program._world = tutorialWorld;
            tutorialPlayer.MoveTo(new Point(7, 4)); //Set player start point
            Lighting.LightingUpdate(); //Starts lighting 

            Console.SetCursorPosition(48, 13);
            Console.WriteLine("Press any Key to Continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

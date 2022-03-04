using System;
using System.Collections.Generic;
using System.Text;


namespace CRPG
{
    public static class GameEngine
    {
        public static string Version = "0.2.5";

        public static void Initialize()
        {
            //Set screen size
            Console.SetWindowSize(120, 42);

            //Turn off curser visibility
            Console.WriteLine("\x1b[?25l  ");

            //Start message
            Console.SetCursorPosition(41, 0);
            Console.WriteLine("Initializing Game Engine Version {0}", Version);


            //Credits to https://patorjk.com/software/taag/ for the awesome ascii art
            Console.SetCursorPosition(22, 3);
            Console.WriteLine("   ▄████████    ▄████████  ▄████████    ▄████████    ▄███████▄    ▄████████ ");
            Console.SetCursorPosition(22, 4);
            Console.WriteLine("  ███    ███   ███    ███ ███    ███   ███    ███   ███    ███   ███    ███ ");
            Console.SetCursorPosition(22, 5);
            Console.WriteLine("  ███    █▀    ███    █▀  ███    █▀    ███    ███   ███    ███   ███    █▀  ");
            Console.SetCursorPosition(22, 6);
            Console.WriteLine(" ▄███▄▄▄       ███        ███          ███    ███   ███    ███  ▄███▄▄▄     ");
            Console.SetCursorPosition(22, 7);
            Console.WriteLine("▀▀███▀▀▀     ▀███████████ ███        ▀███████████ ▀█████████▀  ▀▀███▀▀▀     ");
            Console.SetCursorPosition(22, 8);
            Console.WriteLine("  ███    █▄           ███ ███    █▄    ███    ███   ███          ███    █▄  ");
            Console.SetCursorPosition(22, 9);
            Console.WriteLine("  ███    ███    ▄█    ███ ███    ███   ███    ███   ███          ███    ███ ");
            Console.SetCursorPosition(22, 10);
            Console.WriteLine("  ██████████  ▄████████▀  ████████▀    ███    █▀   ▄████▀        ██████████ ");
            Console.SetCursorPosition(51, 12);
            Console.WriteLine("Made by Evan Gray"); //Write text

            DateTime delayTime = DateTime.Now; //Holds delay between text changes
            bool visable = true; //Holds if text is visable

            //While no player input
            while (!Console.KeyAvailable)
            {
                if (delayTime <= DateTime.Now && visable)  //If delay is over and text is visable
                {
                    Console.SetCursorPosition(48, 16); //Set curser pos
                    Console.WriteLine("Press any Key to begin!"); //Write text
                    delayTime = DateTime.Now.AddSeconds(0.75f); //Get new delay
                    visable = !visable; //Flip visable
                }
                else if(delayTime <= DateTime.Now && !visable) //If delay is over and text is not visable
                {
                    Console.SetCursorPosition(48, 16); //Set curser pos
                    Console.WriteLine("                       "); //Remove text
                    delayTime = DateTime.Now.AddSeconds(0.75f); //Get new delay
                    visable = !visable; //Flip visable
                }
            }

            //Read key and clear console
            Console.ReadKey();
            Console.Clear();
            
        }
    }
}

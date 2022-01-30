using System;
using System.Collections.Generic;
using System.Text;


namespace CRPG
{
    public static class GameEngine
    {
        public static string Version = "0.0.7";

        public static void Initialize()
        {
            //Set screen size
            Console.SetWindowSize(Console.LargestWindowWidth / 2, Console.LargestWindowHeight - (Console.LargestWindowHeight / 3));

            //Start message
            Console.WriteLine("                                       Initializing Game Engine Version {0}", Version);
            Console.WriteLine();
            Console.WriteLine();

            //Credits to https://patorjk.com/software/taag/ for the awesome ascii art

            Console.WriteLine("                       ▄████████    ▄████████  ▄████████    ▄████████    ▄███████▄    ▄████████ ");
            Console.WriteLine("                      ███    ███   ███    ███ ███    ███   ███    ███   ███    ███   ███    ███ ");
            Console.WriteLine("                      ███    █▀    ███    █▀  ███    █▀    ███    ███   ███    ███   ███    █▀  ");
            Console.WriteLine("                     ▄███▄▄▄       ███        ███          ███    ███   ███    ███  ▄███▄▄▄     ");
            Console.WriteLine("                    ▀▀███▀▀▀     ▀███████████ ███        ▀███████████ ▀█████████▀  ▀▀███▀▀▀     ");
            Console.WriteLine("                      ███    █▄           ███ ███    █▄    ███    ███   ███          ███    █▄  ");
            Console.WriteLine("                      ███    ███    ▄█    ███ ███    ███   ███    ███   ███          ███    ███ ");
            Console.WriteLine("                      ██████████  ▄████████▀  ████████▀    ███    █▀   ▄████▀        ██████████ ");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("                                              Press any Key to begin!");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

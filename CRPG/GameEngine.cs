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
            Console.SetWindowSize(120, 42);

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

            Console.SetCursorPosition(48, 13);
            Console.WriteLine("Press any Key to begin!");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

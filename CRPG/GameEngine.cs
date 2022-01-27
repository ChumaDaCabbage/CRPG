using System;
using System.Collections.Generic;
using System.Text;


namespace CRPG
{
    public static class GameEngine
    {
        public static string Version = "0.0.3";

        public static void Initialize()
        {
            //Set screen size
            Console.SetWindowSize(Console.LargestWindowWidth / 2, Console.LargestWindowHeight - (Console.LargestWindowHeight / 3));

            //Start message
            Console.WriteLine("Initializing Game Engine Version {0}", Version);
            Console.WriteLine("\n\nWelcome to the World of {0}", World.WorldName);
            Console.WriteLine();
            Console.WriteLine("Press any Key to begin!");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

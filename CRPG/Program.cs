using System;

//Evan Gray :D 2022
namespace CRPG
{
    class Program
    {

        private static Player _player = new Player();

        static void Main(string[] args)
        {
            GameEngine.Initialize();
            _player.Name = "Fred the fearful";
            // _player.MoveTo(World.GetLocationByID(World.LOCATION_ID_HOME));

            DateTime lastPressedTime = DateTime.MinValue;
            while (true)
            {
                ConsoleKey userInput = ConsoleKey.H;
                while (DateTime.Now < lastPressedTime.AddSeconds(.15))
                {
                    userInput = Console.ReadKey().Key;
                }
                Console.Clear();
                Map.DrawMap(_player);
                ParseInput(userInput);
                lastPressedTime = DateTime.Now;


                /*
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    continue;
                }
                string cleanedInput = userInput.ToLower();

                if (cleanedInput == "exit")
                {
                    break;
                }
                ParseInput(cleanedInput);
                */
            }
        }

        public static void ParseInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.H:
                    Console.WriteLine("Help is coming later... stay tuned.");
                    break;

                case ConsoleKey.W:
                    _player.MoveNorth();
                    break;

                case ConsoleKey.D:
                    _player.MoveEast();
                    break;

                case ConsoleKey.S:
                    _player.MoveSouth();
                    break;

                case ConsoleKey.A:
                    _player.MoveWest();
                    break;

                default:
                    Console.WriteLine("I don't understand. Sorry!");
                    break;

            }
            /*
            if (input.Contains("h"))
            {
                Console.WriteLine("Help is coming later... stay tuned.");
            }
            else if (input.Contains("look"))
            {
                DisplayCurrentLocation();
            }
            else if (input.Equals("w")) //Movement start
            {
                _player.MoveNorth();
            }
            else if (input.Equals("d"))
            {
                _player.MoveEast();
            }
            else if (input.Equals("s"))
            {
                _player.MoveSouth();
            }
            else if (input.Equals("a"))
            {
                _player.MoveWest();
            }
            else
            {
                Console.WriteLine("I don't understand. Sorry!");
            }
            */
        }

        /*
        private static void DisplayCurrentLocation()
        {
            Console.WriteLine("You are at: {0}", _player.CurrentLocation.Name);
            if(_player.CurrentLocation.Description != "")
            {
                Console.Write("\t{0}\n", _player.CurrentLocation.Description);
            }
        }
        */
    }
}

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

            Console.WriteLine("Press any Key to begin!");
            Console.ReadKey();
            Console.Clear();
            Map.DrawMap(_player);
            Console.SetCursorPosition(0, World.MAX_WORLD_Y + 2);

            DateTime lastPressedTime = DateTime.MinValue;
            while (true)
            {
                ConsoleKey userInput = ConsoleKey.F4; //Defualt value that I wont use in game
                while (userInput == ConsoleKey.F4)
                {
                    if (DateTime.Now >= lastPressedTime.AddSeconds(0.1))
                    {
                        ClearKeyBuffer();
                        userInput = Console.ReadKey().Key;
                    }
                    else
                    {
                        Console.Write("");
                    }
                }
                SetupWritingLine();
                ParseInput(userInput);
                lastPressedTime = DateTime.Now;
            }
        }

        public static void ParseInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.H:
                    Console.Write("Help is coming later... stay tuned.");
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
        }

        private static void ClearKeyBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
        }

        public static void SetupWritingLine()
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, World.MAX_WORLD_Y + 2);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, World.MAX_WORLD_Y + 2);
            Console.WriteLine("> ");
            Console.SetCursorPosition(2, World.MAX_WORLD_Y + 2);
        }
    }
}

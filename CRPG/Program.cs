using System;

//Evan Gray :D 2022
namespace CRPG
{
    class Program
    {

        private static Player _player = new Player();

        static void Main(string[] args)
        {
            //Start Game engine
            GameEngine.Initialize();

            //Set up default player info
            _player.Name = "Fred the fearful";
            _player.MoveTo(1, 1);

            //Setup default map
            Map.DrawMap(_player);

            //Starts lighting
            Lighting.lightingUpdate(_player);

            DateTime lastPressedTime = DateTime.MinValue; //Get current time
            while (true)
            {
                ConsoleKey userInput = ConsoleKey.H; //Defualt value
                while (true)
                {
                    if (DateTime.Now >= lastPressedTime.AddSeconds(0.1)) //If time greater than input delay
                    {
                        ClearKeyBuffer(); //Start clearKeyBuffer
                        userInput = Console.ReadKey().Key; //Get input
                        break; //Leave loop
                    }
                    else
                    {
                        Console.Write("");
                    }
                }
                SetupWritingLine(); //Starts setup writing line
                ParseInput(userInput); //Starts ParseInput and gives it the user input
                Lighting.lightingUpdate(_player); //Updates lighting
                lastPressedTime = DateTime.Now; //Gets current time
            }
        }

        public static void ParseInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.H: //Help
                    Console.Write("Help is coming later... stay tuned. >");
                    break;

                case ConsoleKey.W: //Move up
                    _player.MoveNorth();
                    break;

                case ConsoleKey.D: //Move left
                    _player.MoveEast();
                    break;

                case ConsoleKey.S: //Move down
                    _player.MoveSouth();
                    break;

                case ConsoleKey.A: //Move right
                    _player.MoveWest();
                    break;

                default:
                    Console.Write("I don't understand. Sorry! >");
                    break;

            }
        }

        //Clears buffered key inputs
        private static void ClearKeyBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
        }

        //Clears input line and move curser back to input position
        public static void SetupWritingLine()
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, World.MAX_WORLD_Y + 2);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(40, World.MAX_WORLD_Y + 2);
            Console.WriteLine("> ");
            Console.SetCursorPosition(42, World.MAX_WORLD_Y + 2);
        }
    }
}

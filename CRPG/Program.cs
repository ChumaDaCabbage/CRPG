using System;
using System.Collections.Generic;
using System.Text;

//Evan Gray :D 2022
namespace CRPG
{
    class Program
    {

        public static Player _player = new Player();

        static void Main(string[] args)
        {
            //Start Game engine
            GameEngine.Initialize();

            //Setup default map
            Map.DrawMap();

            //Set player start point
            _player.MoveTo(new Point(1, 1));

            //Starts lighting
            Lighting.LightingUpdate();

            DateTime lastPressedTime = DateTime.MinValue; //Holds time buttons were last pressed
            while (true)
            {
                ConsoleKey userInput; //Holds user input
                while (true)
                {
                    if (DateTime.Now >= lastPressedTime.AddSeconds(0.1) && Console.KeyAvailable) //If time greater than input delay and key is being pressed
                    {
                        userInput = Console.ReadKey().Key; //Get input
                        ClearKeyBuffer(); //Start clearKeyBuffer
                        break; //Leave loop
                    }

                    //Update all flares
                    /*
                    foreach (Flare flare in _player._flares)
                    {
                        flare.FlareUpdate();  
                    }
                    */

                    for (int i = 0; i < _player._flares.Count; i++)
                    {
                        _player._flares[i].FlareUpdate();
                    }
                }
                SetupWritingLine(); //Starts setup writing line
                ParseInput(userInput); //Starts ParseInput and gives it the user input
                Lighting.LightingUpdate(); //Updates lighting
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

                case ConsoleKey.UpArrow: //Shoot up
                    _player.Shoot(new Point(0, -Player.PLAYER_SHOOT_SPEED));
                    break;

                case ConsoleKey.LeftArrow: //Shoot left
                    _player.Shoot(new Point(-Player.PLAYER_SHOOT_SPEED, 0));
                    break;

                case ConsoleKey.DownArrow: //Shoot down
                    _player.Shoot(new Point(0, Player.PLAYER_SHOOT_SPEED));
                    break;

                case ConsoleKey.RightArrow: //Shoot right
                    _player.Shoot(new Point(Player.PLAYER_SHOOT_SPEED, 0));
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

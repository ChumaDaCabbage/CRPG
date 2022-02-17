﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

//Evan Gray :D 2022
namespace CRPG
{
    class Program
    {
        public static Player _player = new Player();

        private static ConsoleKey userInput = ConsoleKey.F12; //Holds user input

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

            //Sets up line for player input
            SetupWritingLine();

            //Starts thread
            Thread inputThread = new Thread(new ThreadStart(GetInput));
            inputThread.Start();

            while (true)
            {
                //Update world
                WorldUpdate();

                if (userInput != ConsoleKey.F12)
                {
                    SetupWritingLine(); //Starts setup writing line
                    ParseInput(userInput); //Starts ParseInput and gives it the user input
                    userInput = ConsoleKey.F12;
                }
            }
        }

        //Gets input in different thread to use in main thread
        private static void GetInput()
        {
            //Game loop
            while (true)
            {
                //If input is defualt
                if (userInput == ConsoleKey.F12)
                {
                    userInput = Console.ReadKey(true).Key; //Get input
                    ClearKeyBuffer(); //Start clearKeyBuffer
                    Thread.Sleep(TimeSpan.FromSeconds(0.15));
                }
            }
        }

        public static void ParseInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.H: //Help
                    Console.Write("Help is coming later... stay tuned.");
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
                    _player.Shoot(new Point(0, -Player.PLAYER_SHOOT_SPEED), _player.Pos);
                    break;

                case ConsoleKey.LeftArrow: //Shoot left
                    _player.Shoot(new Point(-Player.PLAYER_SHOOT_SPEED, 0), _player.Pos);
                    break;

                case ConsoleKey.DownArrow: //Shoot down
                    _player.Shoot(new Point(0, Player.PLAYER_SHOOT_SPEED), _player.Pos);
                    break;

                case ConsoleKey.RightArrow: //Shoot right
                    _player.Shoot(new Point(Player.PLAYER_SHOOT_SPEED, 0), _player.Pos);
                    break;

                case ConsoleKey.Spacebar: //Light torches
                    _player.LightTorches();
                    break;

                default:
                    Console.Write("I don't understand. Sorry!");
                    break;
            }
        }

        private static void WorldUpdate()
        {
            //Update all flares
            for (int i = 0; i < _player._flares.Count; i++)
            {
                _player._flares[i].FlareUpdate();
            }

            //Update all Enemies
            for (int i = 0; i < World._enemies.Count; i++)
            {
                World._enemies[i].EnemyUpdate();
            }
        }

        //Clears buffered key inputs
        private static void ClearKeyBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        //Clears input line and move curser back to input position
        public static void SetupWritingLine()
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, World.MAX_WORLD_Y + 2);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(40, World.MAX_WORLD_Y + 2);
            FlareInventory.DrawFlareBar();
            Console.WriteLine("> ");
            Console.SetCursorPosition(42, World.MAX_WORLD_Y + 2);
        }

        public static void ResetCursor()
        {
            Console.ResetColor();
            Console.SetCursorPosition(42, World.MAX_WORLD_Y + 2);
        }
    }
}

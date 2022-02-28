using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public static class FlareInventory
    {
        private const string flareColor = "\x1b[38;2;255;0;0m";
        public static int FlareCount = 5;
        static readonly Point barPos = new Point(82, World.MAX_WORLD_Y + 2);

        //Sets up inventory on game startup
        public static void InventorySetup()
        {
            FlareCount = 5;
            DrawFlareBar();
        }

        public static void DrawFlareBar()
        {
            Console.Write(flareColor);
            Console.SetCursorPosition(barPos.X, barPos.Y - 1);
            Console.WriteLine("╔═══════════════════╗");

            string currentFlareReadout = "║ ";
            int flaresDrawn = 0;

            for (int i = 0; i <= 17; i++)
            {
                if (flaresDrawn < FlareCount && (i == 0 || i % 4 == 0))
                {
                    currentFlareReadout += "║";
                    flaresDrawn++;
                }
                else
                {
                    currentFlareReadout += " ";
                }
            }
            currentFlareReadout += "║";

            Console.SetCursorPosition(barPos.X, barPos.Y);
            Console.WriteLine(currentFlareReadout);

            Console.SetCursorPosition(barPos.X, barPos.Y + 1);
            Console.WriteLine("╚═══════════════════╝");

            Program.ResetCursor();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class TileVisuals
    {
        public int R = 0;
        public int G = 0;
        public int B = 0;

        //Holds foroground tile info
        public string Forground = "  ";
        public int ForgroundShift = 0;

        public TileVisuals(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
    }

        public TileVisuals(int r, int g, int b, string forground, int forgroundShift)
        {
            R = r;
            G = g;
            B = b;

            Forground = forground;
            ForgroundShift = forgroundShift;
        }

        public string GetExtendedColorsString()
        {
            //Gets wanted icon string using the extended colors strings
            string extendedForground = "m\x1b[38;2;" + Math.Clamp((R - ForgroundShift), 0, 255) + ";" + Math.Clamp((G - ForgroundShift), 0, 255) + ";" + Math.Clamp((B - ForgroundShift), 0, 255) + "m" + Forground;
            string extendedBackground = "\x1b[48;2;" + R + ";" + G + ";" + B;

            return extendedBackground + extendedForground;
        }
    }
}

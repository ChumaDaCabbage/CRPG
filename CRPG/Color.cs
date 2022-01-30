using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Color
    {
        public int R = 0;
        public int G = 0;
        public int B = 0;

        public Color(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public string GetExtendedColorsString()
        { 
            return "\x1b[48;2;" + R + ";" + G + ";" + B + "m  ";
        }
    }
}

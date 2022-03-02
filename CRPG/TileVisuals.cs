using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class TileVisuals
    {
        public Color BackgroundColor;
        private Color ForgroundColor = null;

        //Holds pos (used in animations)
        public Point Pos;

        //Holds foroground tile info
        public string ForGroundTile = "  ";
        public int ForgroundShift = 0;

        public TileVisuals(Color tileColor)
        {
            BackgroundColor = tileColor;
        }

        public TileVisuals(Color backgroundColor, string forGroundTile, int forgroundShift)
        {
            BackgroundColor = backgroundColor;

            ForGroundTile = forGroundTile;
            ForgroundShift = forgroundShift;
        }
        public TileVisuals(Color backgroundColor, string forGroundTile, Color forgroundColor)
        {
            BackgroundColor = backgroundColor;

            ForGroundTile = forGroundTile;
            ForgroundColor = forgroundColor;
        }

        //Gets wanted icon string using the extended colors strings
        public string GetFullExtendedColorsString()
        {
            //Will hold froground
            string extendedForground;

            if (ForgroundColor == null) //If no forground color
            {
                //Uses forgroundShift to make froground color
                extendedForground = "m\x1b[38;2;" + Math.Clamp((BackgroundColor.R - ForgroundShift), 0, 255) + ";" + Math.Clamp((BackgroundColor.G - ForgroundShift), 0, 255) + ";" + Math.Clamp((BackgroundColor.B - ForgroundShift), 0, 255) + "m" + ForGroundTile;
            }
            else
            {
                //Sets forground to forground color
                extendedForground = "m\x1b[38;2;" + Math.Clamp(ForgroundColor.R, 0, 255) + ";" + Math.Clamp(ForgroundColor.G, 0, 255) + ";" + Math.Clamp(ForgroundColor.B, 0, 255) + "m" + ForGroundTile;
            }

            //Makes background color from BackgroundColor
            string extendedBackground = "\x1b[48;2;" + BackgroundColor.R + ";" + BackgroundColor.G + ";" + BackgroundColor.B;


            //Return combined string
            return extendedBackground + extendedForground;
        }

        public string GetBackgroundColorString()
        {
            //Return BackgroundColor string
            return "\x1b[48;2;" + BackgroundColor.R + ";" + BackgroundColor.G + ";" + BackgroundColor.B;
        }

        public string GetForgroundColorString()
        {
            //Will hold froground
            string extendedForground;

            if (ForgroundColor == null) //If no forground color
            {
                //Uses forgroundShift to make froground color
                extendedForground = "m\x1b[38;2;" + Math.Clamp((BackgroundColor.R - ForgroundShift), 0, 255) + ";" + Math.Clamp((BackgroundColor.G - ForgroundShift), 0, 255) + ";" + Math.Clamp((BackgroundColor.B - ForgroundShift), 0, 255) + "m";
            }
            else
            {
                //Sets forground to forground color
                extendedForground = "m\x1b[38;2;" + Math.Clamp(ForgroundColor.R, 0, 255) + ";" + Math.Clamp(ForgroundColor.G, 0, 255) + ";" + Math.Clamp(ForgroundColor.B, 0, 255) + "m" + ForGroundTile;
            }

            //Return ForgroundColor string
            return extendedForground;
        }


        public Color GetForgroundColor()
        {
            if (ForgroundColor == null) //If no forground color
            {
                //Uses forgroundShift to make froground color
                return new Color(Math.Clamp((BackgroundColor.R - ForgroundShift), 0, 255),  Math.Clamp((BackgroundColor.G - ForgroundShift), 0, 255), Math.Clamp((BackgroundColor.B - ForgroundShift), 0, 255));
            }
            else
            {
                //Sets forground to forground color
                return new Color(Math.Clamp(ForgroundColor.R, 0, 255), Math.Clamp(ForgroundColor.G, 0, 255), Math.Clamp(ForgroundColor.B, 0, 255));
            }
        }
    }
}

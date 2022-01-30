using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Location
    {
        //Wall info
        public bool IsWall;

        //LightSource info
        public bool IsLightSource = false;
        public bool RedLightSource = false;
        public int LightPower = 1;

        //Light Received info
        public int currentLightLevel = 1;
        public bool redLight = false;

        //Constructors
        /// <summary>
        /// Makes a wall
        /// </summary>
        /// <param IsWall ="isWall"></param>
        public Location( bool isWall)
        {
            //Wall location
            IsWall = isWall;
        }

        /// <summary>
        /// Makes defualt location
        /// </summary>
        public Location()
        {
            //Empty location
        }

        public void setLightSource(bool isLightSource, int lightPower, bool redLightSource)
        {
            //Set lighting data
            this.IsLightSource = isLightSource;
            this.LightPower = lightPower;
            this.RedLightSource = redLightSource;
        }
    }
}

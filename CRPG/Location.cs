using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Location
    {
        //Wall info
        public bool IsWall;

        //Light info
        public bool IsLightSource;
        public int lightPower = 1;
        public int lightLevel = 1;

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

        public void setLightSource(bool isLightSource, int power)
        {
            //Set lighting data
            this.IsLightSource = isLightSource;
            this.lightPower = power;
        }
    }
}

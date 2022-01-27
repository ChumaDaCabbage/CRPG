using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Location
    {
        public bool IsWall;
        public bool IsLightSource;
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
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Location
    {
        public int ID;
        public string Name;
        public string Description;
        public bool IsWall;
        //public Location LocationToNorth;
        //public Location LocationToEast;
        //public Location LocationToSouth;
        //public Location LocationToWest;

        //Constructors

        /// <summary>
        /// Makes a special location
        /// </summary>
        /// <param LocationID="iD"></param>
        /// <param LocationName="name"></param>
        /// <param Locationdescription="description"></param>
        public Location(int iD, string name, string description)
        {
            //Special location
            ID = iD;
            Name = name;
            Description = description;
            IsWall = false;
        }

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

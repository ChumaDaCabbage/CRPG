﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Adds passed point to this point and returns this new value
        /// </summary>
        /// <returns></returns>
        public Point Add(Point point2)
        {
            return new Point(X + point2.X, Y + point2.Y);
        }

        /// <summary>
        /// Returns if passed value is equal to this point
        /// </summary>
        public bool Equals(Point point2)
        {
            return (X == point2.X && Y == point2.Y);
        }

        public static Point Zero()
        {
            return new Point(0, 0);
        }

        public Point ScuffedNormalize()
        {
            return new Point(Math.Clamp(X, -1, 1), Math.Clamp(Y, -1, 1));
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}

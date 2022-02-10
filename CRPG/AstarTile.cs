using System;
using System.Collections.Generic;
using System.Text;

namespace CRPG
{
    public class AstarTile
    {
        public Point Pos { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public int InverseCostDistance => Cost - Distance;
        public AstarTile Parent { get; set; }

        public AstarTile() { }

        public AstarTile(Point pos) 
        {
            Pos = pos;
        }

        /// <summary>
        /// Sets Distance to the distance between this point and passed point (ignoring walls)
        /// </summary>
        /// <param name=""></param>
        public void SetDistance(Point targetPoint)
        {
            this.Distance = Math.Abs(targetPoint.X - Pos.X) + Math.Abs(targetPoint.Y - Pos.Y);
        }
    }
}

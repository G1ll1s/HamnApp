using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamnApp.Models.Entities
{
    public class RowBoat : Boat
    {
        private static Random random = new Random();
        public RowBoat() : base("R")
        {
            BoatType = "RowBoat";
            Weight = random.Next(100, 301);
            MaxSpeedKnop = random.Next(1, 4);
            DaysInHarbor = 1;
            UniqueProperty = $"Max antal passagerare: {random.Next(1, 7)}";
            OccupiedSpots = 1;
        }
    }
}

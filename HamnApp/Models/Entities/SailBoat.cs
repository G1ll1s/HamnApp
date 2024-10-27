using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamnApp.Models.Entities
{
    public class SailBoat : Boat
    {
        private static readonly Random random = new Random();
        public SailBoat() : base("S")
        {
            BoatType = "SailBoat";
            Weight = random.Next(800, 6001);
            MaxSpeedKnop = random.Next(1, 13);
            DaysInHarbor = 4;
            UniqueProperty = $"Båtlängd: {random.Next(10, 61)}";
            OccupiedSpots = 2;
        }
    }
       
}

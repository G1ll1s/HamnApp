using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamnApp.Models.Entities
{
    public class MotorBoat : Boat
    {
        private static Random random = new Random();
        public MotorBoat() : base("M")
        {
            BoatType = "MotorBoat";
            Weight = random.Next(200, 3001);
            MaxSpeedKnop = random.Next(1, 61);
            DaysInHarbor = 3;
            UniqueProperty = $"Antal hästkrafter: {random.Next(10, 1001)}";
            OccupiedSpots = 1;
        }
    }
}

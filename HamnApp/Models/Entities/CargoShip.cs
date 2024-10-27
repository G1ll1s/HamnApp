using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamnApp.Models.Entities
{
    public class CargoShip : Boat
    {
        private static readonly Random random = new Random();
        public CargoShip() : base("L")
        {
            BoatType = "CargoShip";
            Weight = random.Next(3000, 20001);
            MaxSpeedKnop = random.Next(1, 21);
            DaysInHarbor = 6;
            UniqueProperty = $"Antal containers på fartyget just nu: {random.Next(0, 501)}";
            OccupiedSpots = 4;
        }
    }    
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamnApp.Models.Entities
{
    public class Boat
    {
        private static readonly Random random = new Random();

        [Key]
        public int Id { get; set; }
        public int SpotId { get; set; }
        public string BoatType { get; set; }
        public string IdentityNumber { get; set; }
        public int Weight { get; set; }
        public int MaxSpeedKnop { get; set; }
        public int DaysInHarbor { get; set; }
        public string? UniqueProperty { get; set; }
        public int OccupiedSpots { get; set; }

        public Boat()
        {
        }
        public Boat(string prefix)
        {
            IdentityNumber = GenerateIdentityNumber(prefix);
        }

        public static string GenerateIdentityNumber(string prefix)
        {
            int identityNumber = random.Next(100, 1000);
            return $"{prefix}-{identityNumber}";
        }

        public double GetSpeedInKmH()
        {
            return MaxSpeedKnop * 1.852;
        }


    }
}

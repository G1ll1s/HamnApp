using HamnApp.Models;
using HamnApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamnApp
{
        public class HarborStatusDisplay
    {
        private readonly HarborManager _harborManager;
        private readonly HarborContext _context;

        public HarborStatusDisplay(HarborManager harborManager, HarborContext context)
        {
            _harborManager = harborManager;
            _context = context;
        }

        // Display the layout of the harbor with occupied and empty spots
        public void DisplayHarbor()
        {
            var boatsInHarbor = _context.Boats.OrderBy(b => b.SpotId).ToList();
            int currentSpot = 1;

            for (int i = 0; i < boatsInHarbor.Count; i++)
            {
                var boat = boatsInHarbor[i];

                // Display empty spots until reaching the current boat's SpotId
                while (currentSpot < boat.SpotId)
                {
                    Console.WriteLine($"Plats {currentSpot}: Tom");
                    currentSpot++;
                }

                // Handle consecutive spots for the same boat (e.g., 2-5 for a cargo ship)
                int startSpot = boat.SpotId;
                int endSpot = startSpot;

                // Find the end of the consecutive spots occupied by the same boat
                while (i + 1 < boatsInHarbor.Count &&
                       boatsInHarbor[i + 1].IdentityNumber == boat.IdentityNumber &&
                       boatsInHarbor[i + 1].SpotId == endSpot + 1)
                {
                    endSpot++;
                    i++;
                }

                if (boat.BoatType == "RowBoat")
                {
                    // For rowboats, display one entry if they share a single spot
                    Console.WriteLine($"Plats {startSpot}: {boat.BoatType} (ID: {boat.IdentityNumber}), " +
                                      $"Vikt: {boat.Weight}, Hastighet: {boat.GetSpeedInKmH()} " +
                                      $"Dagar i hamn: {boat.DaysInHarbor}, {boat.UniqueProperty}");


                    // Check if there's a second rowboat sharing the same spot
                    if (i + 1 < boatsInHarbor.Count &&
                        boatsInHarbor[i + 1].SpotId == startSpot &&
                        boatsInHarbor[i + 1].BoatType == "RowBoat")
                    {
                        var secondRowBoat = boatsInHarbor[++i];
                        Console.WriteLine($"Plats {startSpot}: {secondRowBoat.BoatType} (ID: {secondRowBoat.IdentityNumber}), " +
                                          $"Vikt: {secondRowBoat.Weight}, Hastighet: {secondRowBoat.GetSpeedInKmH()}, " +
                                          $"Dagar i hamn: {secondRowBoat.DaysInHarbor}, {secondRowBoat.UniqueProperty}");

                    }
                }
                else
                {
                    // Display the range of spots for larger boats occupying multiple spots
                    string spotRange = startSpot == endSpot ? $"{startSpot}" : $"{startSpot}-{endSpot}";
                    Console.WriteLine($"Plats {spotRange}: {boat.BoatType} (ID: {boat.IdentityNumber}), " +
                                      $"Vikt: {boat.Weight}, Hastighet: {boat.GetSpeedInKmH()}, " +
                                      $"Dagar i hamn: {boat.DaysInHarbor}, {boat.UniqueProperty}");
                }

                // Update current spot to the next available spot after the current boat
                currentSpot = endSpot + 1;
            }

            // Display any remaining empty spots after the last boat
            while (currentSpot <= HarborManager.TotalSpots)
            {
                Console.WriteLine($"plats {currentSpot}: Tom");
                currentSpot++;
            }
        }

        // Display statistics about the harbor
        public void DisplayStatistic()
        {
            double availableSpots = _harborManager.GetAvailableSpots();
            int totalBoats = _context.Boats.Select(b => b.IdentityNumber).Distinct().Count();
            double totalWeight = _context.Boats.Sum(b => b.Weight);
            double averageMaxSpeed = _context.Boats.Average(b => b.MaxSpeedKnop);

            // Count the number of each type of boat
            var boatTypeCounts = _context.Boats
                .GroupBy(b => b.BoatType)
                .Select(group => new { Type = group.Key, Count = group.Select(b => b.IdentityNumber).Distinct().Count() })
                .ToList();

            // Display the statistics
            Console.WriteLine("\n=== Hamn Statistik ===");
            Console.WriteLine($"Lediga Platser: {availableSpots}");
            Console.WriteLine($"Båtar i hamnen: {totalBoats}");
            Console.WriteLine($"Totala vikten av alla båtar: {totalWeight} kg");
            Console.WriteLine($"Medelvärdet för hastigheten av alla båtar: {averageMaxSpeed:F2} knots");

            Console.WriteLine("\nVilka olika sorters båtar är i hamnen:");
            foreach (var typeCount in boatTypeCounts)
            {
                Console.WriteLine($" - {typeCount.Type}: {typeCount.Count}");
            }

            // Display the total number of rejected boats
            Console.WriteLine($"Totala avisade båtar: {_harborManager.RejectedBoatsCount}");
        }
    }

}

using HamnApp.Models;
using HamnApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamnApp
{
    public class HarborManager
    {
        private readonly HarborContext _context;
        public const int TotalSpots = 64;
        public int RejectedBoatsCount = 0; // Track rejected boats

        public HarborManager(HarborContext context)
        {
            _context = context;
        }


        // Calculate how many harbor spots are available
        public int GetAvailableSpots()
        {
            int occupiedSpots = _context.Boats.Sum(b => b.OccupiedSpots);
            return TotalSpots - occupiedSpots; // Return the number of free spots
        }

        // Assign all incoming boats to the harbor
        public void AssignIncomingBoats(List<Boat> boats)
        {
            int availableSpots = GetAvailableSpots(); // Get available space
            foreach (var boat in boats)
            {
                int boatSpots = GetRequiredSpots(boat); // Get the number of spots required by the boat
                //if(availableSpots < boatSpots)
                if (availableSpots >= boat.OccupiedSpots)
                {
                    AssignBoatToHarbor(boat); // Try to assign the boat to a spot
                }
                else
                {
                    RejectedBoatsCount++; // Increment rejected boats count
                    Console.WriteLine($"Båt {boat.BoatType} {boat.IdentityNumber} får inte plats i hamnen.");
                    continue;
                }
            }

            // Persist changes in the database
            _context.SaveChanges();
            Console.WriteLine($"Totalt antal avvisade båtar: {RejectedBoatsCount}");
        }

        // Assign a boat to a spot in the harbor
        public void AssignBoatToHarbor(Boat boat)
        {
            bool assigned = false;
            int requiredSpots = GetRequiredSpots(boat); // Get the required number of spots based on boat type

            // Generate a list of spots from 1 to TotalSpots
            var spots = Enumerable.Range(1, TotalSpots).ToList();

            foreach (var spot in spots)
            {
                if (assigned) break; // Exit if the boat is already assigned

                // Check if we have enough room for larger boats with multiple spots
                if (spot + requiredSpots - 1 > TotalSpots)
                    break; // Not enough consecutive spots available at the end of the list

                // Special handling for RowBoat: Check sharing rules
                if (boat.BoatType == "RowBoat")
                {
                    // RowBoats can share a spot with one other RowBoat, but not with other boat types
                    int rowBoatCount = _context.Boats.Count(b => b.SpotId == spot && b.BoatType == "RowBoat");
                    bool isOccupiedByOtherType = _context.Boats.Any(b => b.SpotId == spot && b.BoatType != "RowBoat");

                    // Assign RowBoat only if it can share with another RowBoat or is in an empty spot
                    if (rowBoatCount < 2 && !isOccupiedByOtherType)
                    {
                        boat.SpotId = spot;
                        boat.OccupiedSpots =+ 1;
                        _context.Boats.Add(boat);
                        Console.WriteLine($"{boat.BoatType} {boat.IdentityNumber} assigned to shared Spot {spot}");
                        assigned = true;
                    }
                }
                else
                {
                    // For non-RowBoats, check if a range of consecutive spots is available
                    bool spotsAvailable = !_context.Boats
                        .Any(b => b.SpotId >= spot && b.SpotId < spot + requiredSpots);

                    if (spotsAvailable)
                    {
                        // Assign the boat to each spot in the required range
                        for (int j = 0; j < requiredSpots; j++)
                        {

                            _context.Boats.Add(new Boat
                            {
                                SpotId = spot + j,
                                BoatType = boat.BoatType,
                                IdentityNumber = boat.IdentityNumber,
                                Weight = boat.Weight,
                                MaxSpeedKnop = boat.MaxSpeedKnop,
                                DaysInHarbor = boat.DaysInHarbor,
                                UniqueProperty = boat.UniqueProperty,
                                OccupiedSpots = +1 
                            });
                        }

                        Console.WriteLine($"{boat.BoatType} {boat.IdentityNumber} assigned to spots {spot} to {spot + requiredSpots - 1}");
                        assigned = true;
                    }
                }
            }

            //if (!assigned)
            //{
            //    // Track rejected boats if no suitable spots were found
            //    Console.WriteLine($"{boat.BoatType} {boat.IdentityNumber} could not be assigned to a spot.");
            //}

            _context.SaveChanges();
        }

        // Helper method to define required spots based on boat type
        private int GetRequiredSpots(Boat boat)
        {
            return boat switch
            {
                RowBoat => 1,           // RowBoat can share one spot with another rowboat
                MotorBoat => 1,         // MotorBoat occupies one spot
                SailBoat => 2,          // SailBoat occupies one spot
                CargoShip => 4,         // CargoShip occupies four spots
                _ => throw new ArgumentException("Unknow boat") // Default to 1 spot if type is unknown
            };
        }



        // Update the harbor for the next day (decrement days and remove boats that need to leave)
        public void UpdateHarborForNextDay()
        {
            var boatsToRemove = _context.Boats
                .Where(b => b.DaysInHarbor == 0)
                .ToList();
            foreach (var boat in boatsToRemove)
            {
                _context.Boats.Remove(boat);
                Console.WriteLine($"{boat.BoatType} {boat.IdentityNumber} lämnar hamnen");
            }

            foreach (var boat in _context.Boats)
            {
                if (boat.DaysInHarbor > 0)
                {
                    boat.DaysInHarbor -= 1; // Decrement the number of days in harbor
                }
            }

            // Save the updated state of the harbor
            _context.SaveChanges();
        }
    }


}

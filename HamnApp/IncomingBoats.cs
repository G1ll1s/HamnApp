using HamnApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamnApp
{
    public class IncomingBoats
    {
        private static readonly Random random = new Random();
        public List<Boat> Boats { get; private set; }

        public IncomingBoats()
        {
            Boats = GenerateRandomBoats();
        }

        public List<Boat> GenerateRandomBoats()
        {
            List<Boat> boats = new List<Boat>();

            var boatTypes = new List<Func<Boat>>()
            {
                () => new RowBoat(),
                () => new MotorBoat(),
                () => new SailBoat(),
                () => new CargoShip()
            };

            for (int i = 0; i < 5; i++)
            {
                var randomBoat = boatTypes[random.Next(boatTypes.Count)]();
                boats.Add(randomBoat);
            }

            return boats;
        }
    }
}

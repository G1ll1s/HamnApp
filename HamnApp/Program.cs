using HamnApp.Models;

namespace HamnApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HarborContext context = new HarborContext();
            HarborManager harborManager = new HarborManager(context);
            IncomingBoats incomingBoats = new IncomingBoats();
            HarborStatusDisplay harborStatusDisplay = new HarborStatusDisplay(harborManager, context);

            bool isRunning = true;

            while (isRunning)
            {

                Console.WriteLine("=== Hamn Administrations App ===");
                Console.WriteLine("1. Båtar som kommer till hamnen");
                Console.WriteLine("2. Visa Hamn Status");
                Console.WriteLine("3. Visa Hamn Statistik");
                Console.WriteLine("4. Nästa Dag");
                Console.WriteLine("5. Exit");
                Console.WriteLine("Skriv in ditt val: ");

                string input = Console.ReadLine();
                int choice = int.TryParse(input, out int result) ? result : -1;

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\nBåtar kommer in till hamnen");
                        var boats = incomingBoats.GenerateRandomBoats();
                        harborManager.AssignIncomingBoats(boats);
                        break;

                    case 2:
                        Console.WriteLine("\nHamn Status");
                        harborStatusDisplay.DisplayHarbor();
                        break;

                    case 3:
                        Console.WriteLine("\nHamn Statistik");
                        harborStatusDisplay.DisplayStatistic();
                        break;

                    case 4:
                        Console.WriteLine("\nNästa dag....");
                        harborManager.UpdateHarborForNextDay();
                        break;

                    case 5:
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("\nOgiltigt val, Testa igen..");
                        break;
                }
            }

        }

    }
}

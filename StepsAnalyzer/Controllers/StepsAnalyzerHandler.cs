using Newtonsoft.Json;

using StepsAnalyzer.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepsAnalyzer.Controllers
{
    public static class StepsAnalyzerHandler
    {
        private static int day = 0;

        private static readonly string pathToFolder = "TestData";

        public static readonly List<List<Walker>> walkersByUsers = new();

        public static string GetWalkersByNames()
        {
            string[] allFiles = Directory.GetFiles(pathToFolder, "*.json");
            Array.Sort(allFiles);

            foreach (var filename in allFiles)
            {
                day++;

                using StreamReader r = new(filename);
                string json = r.ReadToEnd();

                List<Walker>? walkers = JsonConvert.DeserializeObject<List<Walker>>(json);

                if (walkers == null) return "One or more files aren't readible";

                bool itContains = false;

                foreach (var walker in walkers)
                {
                    walker.Day = day;
                    foreach (var walkerByUser in walkersByUsers)
                        if (walkerByUser.Any(n => n.User == walker.User) || walkerByUser.Count == 0)
                        {
                            walkerByUser.Add(walker);
                            itContains = true;
                            break;
                        }

                    if (!itContains)
                        walkersByUsers.Add(new List<Walker>() { walker });

                }
            }

            Console.WriteLine("   Name                  Average       Best       Worst");
            Console.WriteLine("-------------------------------------------------------");
            foreach (var walkerByUser in walkersByUsers)
            {
                int bestresult, worstresult;
                (bestresult, worstresult) = BestWorstResults(walkerByUser);
                Console.WriteLine($"{walkerByUser[0].User,-25} {AverageAmountOfSteps(walkerByUser),-10}  {bestresult, -10}  {worstresult}");
            }


            return "Data read successfully";
        }

        public static int AverageAmountOfSteps(List<Walker> walkDays)
        {
            int amountOfSteps = 0;
            foreach (var walkDay in walkDays)
                amountOfSteps += walkDay.Steps;

            return amountOfSteps / day;
        }

        public static (int, int) BestWorstResults(List<Walker> walkerByUser)
        {
            var results = new List<int>();
            foreach (var walkDay in walkerByUser)
                results.Add(walkDay.Steps);

            int[] resultsArr = results.ToArray();
            Array.Sort(resultsArr);

            var bestResult = resultsArr[resultsArr.Count() - 1];
            var worstResult = resultsArr[0];

            return (bestResult, worstResult);
        }
    }
}

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

        private static readonly string pathToFolder = @"..\TestData";

        public static readonly List<List<Walker>> walkersByNames = new();

        public static string GetWalkersByNames()
        {
            string[] allFiles = Directory.GetFiles(pathToFolder, "*.json");
            Array.Sort(allFiles);

            foreach (var filename in allFiles)
            {
                day++;

                Console.WriteLine(filename);        //test output

                string pathToJsonFile = pathToFolder + filename;
                using StreamReader r = new(pathToJsonFile);
                string json = r.ReadToEnd();

                List<Walker>? walkers = JsonConvert.DeserializeObject<List<Walker>>(json);

                if (walkers == null) return "One or more files aren't readible";

                bool itContains = false;

                foreach (var walker in walkers)
                {
                    walker.Day = day;
                    foreach (var walkerByName in walkersByNames)
                        if (walkerByName.Any(n => n.Name == walker.Name))
                        {
                            walkerByName.Add(walker);
                            itContains = true;
                            break;
                        }

                    if (!itContains)
                        walkersByNames.Add(new List<Walker>() { walker });

                }
            }

            return "Data read successfully";
        }

        public static int AverageAmountOfSteps(List<Walker> walkDays)
        {
            int amountOfSteps = 0;
            foreach (var walkDay in walkDays)
                amountOfSteps += walkDay.AmountOfSteps;

            return amountOfSteps / day;
        }

        public static (int, int) BestWorstResults(List<Walker> walkDays)
        {
            var results = new List<int>();
            foreach (Walker walkDay in walkDays)
                results.Add(walkDay.AmountOfSteps);

            int[] resultsArr = results.ToArray();
            Array.Sort(resultsArr);

            var bestResult = resultsArr[resultsArr.Count() - 1];
            var worstResult = resultsArr[0];

            return (bestResult, worstResult);
        }
    }
}

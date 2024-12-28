using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2024._22.Part2
{
    public class Part2
    {
        private Dictionary<string, int> _sequenceSums = new();

        public async Task<string> SolveAsync(string filePath)
        {
            var lines = await File.ReadAllLinesAsync(filePath); // Read all lines from the input file
            foreach (var line in lines)
            {
                var currentSums = new Dictionary<string, int>();
                var lastDiffs = new LinkedList<int>();

                ulong secretNumber = ulong.Parse(line);
                var previousLastDigit = GetLastDigit(secretNumber);
                for (int i = 0; i < 2000; i++)
                {
                    secretNumber = Evolve(secretNumber);
                    var newLastDigit = GetLastDigit(secretNumber);

                    var diff = (int)(newLastDigit - previousLastDigit);
                    lastDiffs.AddLast(diff);
                    if (lastDiffs.Count == 4)
                    {
                        var diffString = string.Join(",", lastDiffs.Select(d => d.ToString()));
                        if (!currentSums.ContainsKey(diffString))
                        {
                            currentSums[diffString] = newLastDigit;
                        }
                        lastDiffs.RemoveFirst();
                    }

                    previousLastDigit = newLastDigit;
                }

                foreach (var (key, value) in currentSums)
                {
                    if (!_sequenceSums.ContainsKey(key))
                    {
                        _sequenceSums[key] = value;
                    }
                    else
                    {
                        _sequenceSums[key] += value;
                    }
                }
            }

            return _sequenceSums.Values.Max().ToString();
        }

        private int GetLastDigit(ulong number) => (int)(number % 10);

        private ulong Mix(ulong a, ulong b) => a ^ b;

        private ulong Prune(ulong a) => a % 16777216;

        private ulong Evolve(ulong secretNumber)
        {
            var multiple = secretNumber * 64UL;
            secretNumber = Mix(secretNumber, multiple);
            secretNumber = Prune(secretNumber);

            var divide = secretNumber / 32UL;
            secretNumber = Mix(secretNumber, divide);
            secretNumber = Prune(secretNumber);

            multiple = secretNumber * 2048;
            secretNumber = Mix(secretNumber, multiple);
            secretNumber = Prune(secretNumber);

            return secretNumber;
        }
    }

    // Entry point for the program
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var part2 = new Part2();

            // Specify the input file path
            string filePath = "day22.txt"; // Change this to the actual file path

            // Solve the puzzle and print the result
            var result = await part2.SolveAsync(filePath);
            Console.WriteLine($"Result: {result}");
        }
    }
}

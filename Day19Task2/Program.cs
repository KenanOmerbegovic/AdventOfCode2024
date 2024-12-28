using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2024._19.Part2
{
    public class Part2
    {
        private HashSet<string> _patterns;

        public async Task<string> SolveAsync(string filePath)
        {
            // Read all lines from the file
            var lines = await File.ReadAllLinesAsync(filePath);

            // Parse the patterns (first line)
            _patterns = new HashSet<string>(lines[0].Split(", "));

            // Skip the empty line and process towels
            ulong possibleCount = 0;
            for (int i = 2; i < lines.Length; i++) // Start from the third line (index 2)
            {
                var towel = lines[i].Trim();
                possibleCount += CountPossible(towel);
            }

            return possibleCount.ToString();
        }

        private ulong CountPossible(string towel)
        {
            ulong[] ways = new ulong[towel.Length + 1];
            ways[0] = 1; // There's one way to form an empty prefix

            // Iterate through all lengths of the towel
            for (var length = 1; length <= towel.Length; length++)
            {
                for (int previousLength = 0; previousLength < length; previousLength++)
                {
                    if (ways[previousLength] == 0)
                    {
                        continue;
                    }

                    // Extract the substring between previousLength and the current length
                    var current = towel.Substring(previousLength, length - previousLength);

                    // If the substring is a valid pattern, add the ways
                    if (_patterns.Contains(current))
                    {
                        ways[length] += ways[previousLength];
                    }
                }
            }

            return ways[towel.Length];
        }

        // Entry point for the program
        public static async Task Main(string[] args)
        {
            var part2 = new Part2();

            // Specify the path to your input file
            string filePath = "day19.txt";

            // Solve the puzzle and output the result
            var result = await part2.SolveAsync(filePath);
            Console.WriteLine($"Result: {result}");
        }
    }
}

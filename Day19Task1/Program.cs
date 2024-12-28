using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2024._19.Part1
{
    public class Part1
    {
        private string[] _patterns = Array.Empty<string>();
        private Dictionary<int, bool> _memo = new();

        public async Task<string> SolveAsync(string filePath)
        {
            // Read the input from the file
            var lines = await File.ReadAllLinesAsync(filePath);

            // First line contains patterns
            _patterns = lines[0].Split(", ");
            int possibleCount = 0;

            // Start from the second line, which contains towels
            for (int i = 2; i < lines.Length; i++) // Skipping the empty line between patterns and towels
            {
                var towel = lines[i].Trim();
                _memo.Clear(); // Clear memoization for each towel
                if (IsPossible(0, towel))
                {
                    possibleCount++;
                }
            }

            return possibleCount.ToString();
        }

        private bool IsPossible(int index, string towel)
        {
            // If we have matched the entire towel, return true
            if (index == towel.Length)
            {
                return true;
            }

            // Check if the result for this index is already computed
            if (_memo.TryGetValue(index, out var cachedResult))
            {
                return cachedResult;
            }

            // Try to match each pattern starting from the current index
            foreach (var pattern in _patterns)
            {
                if (towel.Substring(index).StartsWith(pattern) && IsPossible(index + pattern.Length, towel))
                {
                    _memo[index] = true; // Store the result in memoization table
                    return true;
                }
            }

            // If no match is found, mark this index as false
            _memo[index] = false;
            return false;
        }

        // Entry point for the program
        public static async Task Main(string[] args)
        {
            var part1 = new Part1();
            string filePath = "day19aj.txt"; // Set the path to your .txt file
            var result = await part1.SolveAsync(filePath);
            Console.WriteLine($"Result: {result}");
        }
    }
}

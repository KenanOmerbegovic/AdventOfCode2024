using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // File path for the input stones
        string filePath = "Day11Text.txt";

        // Read the stones from the file
        Dictionary<long, long> stoneCounts;
        try
        {
            stoneCounts = File.ReadAllText(filePath)
                .Split(new[] { ' ', '\n', '\r', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => (long)g.Count());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading or parsing the file: " + ex.Message);
            return;
        }

        // Number of blinks
        int totalBlinks = 75;

        for (int blink = 1; blink <= totalBlinks; blink++)
        {
            Dictionary<long, long> newStoneCounts = new Dictionary<long, long>();

            foreach (var kvp in stoneCounts)
            {
                long stone = kvp.Key;
                long count = kvp.Value;

                if (stone == 0)
                {
                    // Rule 1: Replace 0 with 1
                    AddToDictionary(newStoneCounts, 1, count);
                }
                else if (HasEvenDigits(stone))
                {
                    // Rule 2: Split the stone into two
                    var (left, right) = SplitStone(stone);
                    AddToDictionary(newStoneCounts, left, count);
                    AddToDictionary(newStoneCounts, right, count);
                }
                else
                {
                    // Rule 3: Multiply the stone by 2024
                    AddToDictionary(newStoneCounts, stone * 2024, count);
                }
            }

            stoneCounts = newStoneCounts;

            // Print progress at intervals
            if (blink % 10 == 0 || blink == totalBlinks)
            {
                long totalStones = stoneCounts.Values.Sum();
                Console.WriteLine($"After {blink} blink(s): {totalStones} stones");
            }
        }

        long finalCount = stoneCounts.Values.Sum();
        Console.WriteLine($"Total stones after {totalBlinks} blinks: {finalCount}");
    }

    // Helper to add stone counts to the dictionary
    static void AddToDictionary(Dictionary<long, long> dict, long key, long count)
    {
        if (dict.ContainsKey(key))
            dict[key] += count;
        else
            dict[key] = count;
    }

    // Check if a number has an even number of digits
    static bool HasEvenDigits(long num)
    {
        int digits = num.ToString().Length;
        return digits % 2 == 0;
    }

    // Split a number into two parts
    static (long, long) SplitStone(long num)
    {
        string numStr = num.ToString();
        int mid = numStr.Length / 2;

        long left = long.Parse(numStr.Substring(0, mid));
        long right = long.Parse(numStr.Substring(mid));

        return (left, right);
    }
}

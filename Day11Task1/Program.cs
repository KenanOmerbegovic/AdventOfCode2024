using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // File path for the input stones
        string filePath = "blinkstones.txt";

        // Read the stones from the file
        List<long> stones;
        try
        {
            stones = File.ReadAllText(filePath)
                         .Split(new[] { ' ', '\n', '\r', ',' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(long.Parse)
                         .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading or parsing the file: " + ex.Message);
            return;
        }

        // Number of blinks
        int totalBlinks = 25;

        for (int blink = 1; blink <= totalBlinks; blink++)
        {
            List<long> newStones = new List<long>();

            foreach (var stone in stones)
            {
                if (stone == 0)
                {
                    // Rule 1: Replace 0 with 1
                    newStones.Add(1);
                }
                else if (HasEvenDigits(stone))
                {
                    // Rule 2: Split the stone into two
                    var (left, right) = SplitStone(stone);
                    newStones.Add(left);
                    newStones.Add(right);
                }
                else
                {
                    // Rule 3: Multiply the stone by 2024
                    newStones.Add(stone * 2024);
                }
            }

            stones = newStones;

            // Print intermediate results (optional)
            Console.WriteLine($"After {blink} blink(s): {stones.Count} stones");
        }

        Console.WriteLine($"Total stones after {totalBlinks} blinks: {stones.Count}");
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

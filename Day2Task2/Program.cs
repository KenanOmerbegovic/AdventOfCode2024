using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Input file path
        string filePath = "Numbers2.txt";

        // Read all lines from the file
        string[] lines = File.ReadAllLines(filePath);

        int safeCount = 0;

        foreach (var line in lines)
        {
            // Parse the line into a list of integers
            string[] parts = line.Split(' ');
            List<int> levels = new List<int>();
            foreach (var part in parts)
            {
                if (int.TryParse(part, out int level))
                {
                    levels.Add(level);
                }
            }

            // Check if the report is safe or can be made safe
            if (IsSafe(levels) || CanBeMadeSafe(levels))
            {
                safeCount++;
            }
        }

        // Output the total number of safe reports
        Console.WriteLine($"Total safe reports: {safeCount}");
    }

    static bool IsSafe(List<int> levels)
    {
        if (levels.Count < 2)
        {
            return false; // A single level cannot be safe
        }

        bool isIncreasing = levels[1] > levels[0];
        bool isConsistent = true;

        for (int i = 1; i < levels.Count; i++)
        {
            int diff = levels[i] - levels[i - 1];

            if (diff < -3 || diff > 3 || diff == 0)
            {
                // Difference is out of range or no change
                isConsistent = false;
                break;
            }

            if (isIncreasing && diff < 0)
            {
                // Transition from increasing to decreasing
                isConsistent = false;
                break;
            }

            if (!isIncreasing && diff > 0)
            {
                // Transition from decreasing to increasing
                isConsistent = false;
                break;
            }
        }

        return isConsistent;
    }

    static bool CanBeMadeSafe(List<int> levels)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            // Create a new list excluding the current level
            List<int> reducedLevels = new List<int>(levels);
            reducedLevels.RemoveAt(i);

            // Check if the reduced list is safe
            if (IsSafe(reducedLevels))
            {
                return true; // Found a way to make the report safe
            }
        }

        return false; // No single removal can make it safe
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        // Input file path
        string filePath = "day3.txt";

        // Read the entire corrupted memory from the file
        string corruptedMemory = File.ReadAllText(filePath);

        // Regular expression to find valid mul(X,Y) instructions
        string pattern = @"mul\((\d+),(\d+)\)";
        Regex regex = new Regex(pattern);

        int totalSum = 0;

        // Find all matches in the corrupted memory
        MatchCollection matches = regex.Matches(corruptedMemory);

        foreach (Match match in matches)
        {
            // Extract the numbers from the match
            int x = int.Parse(match.Groups[1].Value);
            int y = int.Parse(match.Groups[2].Value);

            // Compute the multiplication and add to the total sum
            totalSum += x * y;
        }

        // Output the result
        Console.WriteLine($"Total sum of all valid mul instructions: {totalSum}");
    }
}

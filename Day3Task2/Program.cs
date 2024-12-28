using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "day3.txt";

        string corruptedMemory = File.ReadAllText(filePath);

        string pattern = @"mul\((\d+),(\d+)\)|do\(\)|don't\(\)";
        Regex regex = new Regex(pattern);

        int totalSum = 0;
        bool mulEnabled = true;
        MatchCollection matches = regex.Matches(corruptedMemory);

        foreach (Match match in matches)
        {
            if (match.Value == "do()")
            {
                mulEnabled = true;
            }
            else if (match.Value == "don't()")
            {
                mulEnabled = false;
            }
            else if (match.Groups[1].Success && match.Groups[2].Success)
            {
                if (mulEnabled)
                {
                    int x = int.Parse(match.Groups[1].Value);
                    int y = int.Parse(match.Groups[2].Value);
                    totalSum += x * y;
                }
            }
        }

        // Output the result
        Console.WriteLine($"{totalSum}");
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Specify the path to the input file
        string filePath = "Day5NumberSet.txt";

        // Check if the file exists
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File not found at path {filePath}");
            return;
        }

        // Read the entire content of the file
        string input = File.ReadAllText(filePath);

        // Split input into sections
        var sections = input
            .Trim()
            .Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None);

        // Debug: Log the sections
        Console.WriteLine($"Sections Length: {sections.Length}");
        foreach (var section in sections)
        {
            Console.WriteLine($"Section Content:\n{section}");
        }

        // Validate sections length
        if (sections.Length < 2 || string.IsNullOrWhiteSpace(sections[1]))
        {
            Console.WriteLine("Error: Input does not contain valid updates section.");
            return;
        }

        var rules = sections[0].Split('\n', StringSplitOptions.RemoveEmptyEntries);

        var updates = sections[1]
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(u => u.Split(',')
                          .Where(s => int.TryParse(s, out _))
                          .Select(int.Parse)
                          .ToList())
            .Where(update => update.Count > 0)
            .ToList();

        var precedenceRules = new List<(int before, int after)>();
        foreach (var rule in rules)
        {
            var parts = rule.Split('|');
            if (parts.Length == 2 && int.TryParse(parts[0], out int before) && int.TryParse(parts[1], out int after))
            {
                precedenceRules.Add((before, after));
            }
            else
            {
                Console.WriteLine($"Skipping invalid rule: {rule}");
            }
        }

        int middleSum = 0;
        foreach (var update in updates)
        {
            if (IsUpdateValid(update, precedenceRules))
            {
                int middleIndex = update.Count / 2;
                middleSum += update[middleIndex];
            }
        }

        Console.WriteLine($"Sum of middle pages: {middleSum}");
    }

    static bool IsUpdateValid(List<int> update, List<(int before, int after)> rules)
    {
        foreach (var (before, after) in rules)
        {
            int beforeIndex = update.IndexOf(before);
            int afterIndex = update.IndexOf(after);

            if (beforeIndex != -1 && afterIndex != -1 && beforeIndex > afterIndex)
            {
                return false;
            }
        }
        return true;
    }
}

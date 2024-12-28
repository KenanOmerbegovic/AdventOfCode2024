using System;
using System.Collections.Generic;

class WordSearchXPattern
{
    static void Main()
    {
        string filePath = "day4stringXMAS.txt"; // Path to the text file

        // Read the grid from the file
        if (!System.IO.File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        string[] grid = System.IO.File.ReadAllLines(filePath);

        // Validate the grid
        if (grid.Length == 0 || grid[0].Length == 0)
        {
            Console.WriteLine("The grid file is empty or invalid.");
            return;
        }

        int rows = grid.Length;
        int cols = grid[0].Length;
        int count = 0;

        var detectedPatterns = new List<(int x, int y, string type)>();

        // Check all cells in the grid
        for (int i = 1; i < rows - 1; i++) // Avoid edges
        {
            for (int j = 1; j < cols - 1; j++) // Avoid edges
            {
                string patternType = GetPatternType(grid, i, j);
                if (patternType != null)
                {
                    count++;

                    // Check for overlap
                    bool overlap = false;
                    foreach (var pattern in detectedPatterns)
                    {
                        if (pattern.x == i && pattern.y == j)
                        {
                            overlap = true;
                            Console.WriteLine($"Overlap detected at ({i}, {j}) for patterns: {pattern.type} and {patternType}");
                        }
                    }

                    detectedPatterns.Add((i, j, patternType));
                    Console.WriteLine($"Found {patternType} pattern at ({i}, {j}){(overlap ? " (overlap)" : "")}");
                }
            }
        }

        Console.WriteLine($"Total occurrences of 'MAS' in the shape of an 'X': {count}");
    }

    static string GetPatternType(string[] grid, int x, int y)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;

        // Check Forward Diagonal (Top-left to Bottom-right)
        if (x - 1 >= 0 && y - 1 >= 0 && x + 1 < rows && y + 1 < cols &&
            grid[x][y] == 'A' &&         // Center is 'A'
            grid[x - 1][y - 1] == 'S' && // Top-left is 'M'
            grid[x + 1][y + 1] == 'M' &&   // Bottom-right is 'S'
            grid[x - 1][y + 1] == 'M' && // Top-right is 'M'
            grid[x + 1][y - 1] == 'S')    // Bottom-left is 'S'
        {
            return "Forward Diagonal";
        }

        // Check Backward Diagonal (Top-right to Bottom-left)
        if (x - 1 >= 0 && y + 1 < cols && x + 1 < rows && y - 1 >= 0 &&
            grid[x][y] == 'A' &&         // Center is 'A'
            grid[x - 1][y + 1] == 'S' && // Top-right is 'M'
            grid[x + 1][y - 1] == 'M' &&   // Bottom-left is 'S'
            grid[x - 1][y - 1] == 'M' && // Top-left is 'M'
            grid[x + 1][y + 1] == 'S')   // Bottom-right is 'S'
        {
            return "Backward Diagonal";
        }

        // Check Reverse Forward Diagonal (Bottom-left to Top-right)
        if (x + 1 < rows && y - 1 >= 0 && x - 1 >= 0 && y + 1 < cols &&
            grid[x][y] == 'A' &&         // Center is 'A'
            grid[x + 1][y - 1] == 'M' && // Bottom-left is 'M'
            grid[x - 1][y + 1] == 'S' &&  // Top-right is 'S'
            grid[x + 1][y + 1] == 'M' && // Bottom-right is 'M'
            grid[x - 1][y - 1] == 'S')   // Top-left is 'S'
        
        {
            return "Reverse Forward Diagonal";
        }

        // Check Reverse Backward Diagonal (Bottom-right to Top-left)
        if (x + 1 < rows && y + 1 < cols && x - 1 >= 0 && y - 1 >= 0 &&
            grid[x][y] == 'A' &&         // Center is 'A'
            grid[x + 1][y + 1] == 'S' && // Bottom-right is 'M'
            grid[x - 1][y - 1] == 'M' &&   // Top-left is 'S'
            grid[x + 1][y - 1] == 'S' && // Bottom-left is 'M'
            grid[x - 1][y + 1] == 'M')   // Top-right is 'S'
        
        {
            return "Reverse Backward Diagonal";
        }

        return null; // No pattern found
    }
}

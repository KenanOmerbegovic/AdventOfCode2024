using System;
using System.IO;

class WordSearchXPattern
{
    static void Main()
    {
        string filePath = "XMAS.txt"; // Path to the text file

        // Read the grid from the file
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        string[] grid = File.ReadAllLines(filePath);

        // Validate the grid
        if (grid.Length == 0 || grid[0].Length == 0)
        {
            Console.WriteLine("The grid file is empty or invalid.");
            return;
        }

        int rows = grid.Length;
        int cols = grid[0].Length;
        int count = 0;

        // Check all cells in the grid
        for (int i = 1; i < rows - 1; i++) // Avoid edges
        {
            for (int j = 1; j < cols - 1; j++) // Avoid edges
            {
                string patternType = GetPatternType(grid, i, j);
                if (patternType != null)
                {
                    count++;
                    Console.WriteLine($"Found {patternType} pattern at ({i}, {j})");
                }
            }
        }

        Console.WriteLine($"Total occurrences of 'MAS' in the shape of an 'X': {count}");
    }

    static string GetPatternType(string[] grid, int x, int y)
    {
        // Ensure bounds for all patterns
        int rows = grid.Length;
        int cols = grid[0].Length;
        // Check Forward Diagonal (Top-left to Bottom-right)
        if (x - 1 >= 0 && y - 1 >= 0 && x + 1 < rows && y + 1 < cols &&
            grid[x][y] == 'A' &&         // Center is 'A'
            grid[x - 1][y - 1] == 'M' && // Top-left is 'M'
            grid[x + 1][y + 1] == 'S')   // Bottom-right is 'S'
        {
            return "Forward Diagonal";
        }

        // Check Backward Diagonal (Top-right to Bottom-left)
        if (x - 1 >= 0 && y + 1 < cols && x + 1 < rows && y - 1 >= 0 &&
            grid[x][y] == 'A' &&         // Center is 'A'
            grid[x - 1][y + 1] == 'S' && // Top-right is 'S'
            grid[x + 1][y - 1] == 'M')   // Bottom-left is 'M'
        {
            return "Backward Diagonal";
        }
        if (x - 1 >= 0 && y + 1 < cols && x + 1 < rows && y - 1 >= 0 &&
            grid[x][y] == 'A' &&         // Center is 'A'
            grid[x - 1][y + 1] == 'M' && // Top-right is 'M'
            grid[x + 1][y - 1] == 'S')   // Bottom-left is 'S'
        {
            return "Backward2 Diagonal";
        }

        // Check Pattern 1: 'S M S' in top-left, top-right, bottom-left, bottom-right
        if (x - 1 >= 0 && x + 1 < rows && y - 1 >= 0 && y + 1 < cols &&
            grid[x][y] == 'A' &&         // Center is 'A'
            grid[x - 1][y - 1] == 'S' && // Top-left is 'S'
            grid[x - 1][y + 1] == 'S' && // Top-right is 'S'
            grid[x + 1][y - 1] == 'M' && // Bottom-left is 'M'
            grid[x + 1][y + 1] == 'M')   // Bottom-right is 'M'
        {
            return "Pattern 1";
        }

        // Check Pattern 2: 'M S M' in top-left, top-right, bottom-left, bottom-right
        if (x - 1 >= 0 && x + 1 < rows && y - 1 >= 0 && y + 1 < cols &&
            grid[x][y] == 'A' &&         // Center is 'A'
            grid[x - 1][y - 1] == 'M' && // Top-left is 'M'
            grid[x - 1][y + 1] == 'M' && // Top-right is 'M'
            grid[x + 1][y - 1] == 'S' && // Bottom-left is 'S'
            grid[x + 1][y + 1] == 'S')   // Bottom-right is 'S'
        {
            return "Pattern 2";
        }

        return null; // No pattern found
    }
}

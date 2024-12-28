using System;
using System.IO;

class WordSearch
{
    static void Main()
    {
        // File path for the grid
        string filePath = "XMAS.txt";

        // Read the grid from the file
        if (!File.Exists(filePath))
        {
            Console.WriteLine("The grid file does not exist.");
            return;
        }

        string[] grid = File.ReadAllLines(filePath);

        string target = "XMAS";
        int rows = grid.Length;
        int cols = grid[0].Length;
        int count = 0;

        // Directions (dx, dy) for horizontal, vertical, diagonal, and reversed
        int[][] directions = new int[][]
        {
            new int[] {0, 1},  // Right
            new int[] {1, 0},  // Down
            new int[] {0, -1}, // Left
            new int[] {-1, 0}, // Up
            new int[] {1, 1},  // Down-Right
            new int[] {1, -1}, // Down-Left
            new int[] {-1, 1}, // Up-Right
            new int[] {-1, -1} // Up-Left
        };

        // Check all cells
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                foreach (var dir in directions)
                {
                    if (FindWord(grid, i, j, dir[0], dir[1], target))
                    {
                        count++;
                    }
                }
            }
        }

        Console.WriteLine($"Total occurrences of '{target}': {count}");
    }

    static bool FindWord(string[] grid, int x, int y, int dx, int dy, string target)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;
        int len = target.Length;

        for (int k = 0; k < len; k++)
        {
            int nx = x + k * dx;
            int ny = y + k * dy;

            if (nx < 0 || ny < 0 || nx >= rows || ny >= cols || grid[nx][ny] != target[k])
            {
                return false;
            }
        }

        return true;
    }
}

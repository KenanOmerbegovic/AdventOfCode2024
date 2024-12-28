using System;
using System.Collections.Generic;
using System.IO;

class AntinodeCalculator
{
    static void Main(string[] args)
    {
        // Reading the input grid from a file
        string filePath = "Antenas.txt";
        string[] grid;

        try
        {
            grid = File.ReadAllLines(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            return;
        }

        //Parsing the grid to identify antennas and their positions
        Dictionary<char, List<(int x, int y)>> antennas = new Dictionary<char, List<(int x, int y)>>();

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                char cell = grid[y][x];
                if (char.IsLetterOrDigit(cell)) // Antennas are letters or digits
                {
                    if (!antennas.ContainsKey(cell))
                        antennas[cell] = new List<(int x, int y)>();
                    antennas[cell].Add((x, y));
                }
            }
        }

        //Initializeing a grid to mark antinodes
        char[,] outputGrid = new char[grid.Length, grid[0].Length];
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                outputGrid[y, x] = grid[y][x];
            }
        }

        //Calculating antinodes for each frequency
        HashSet<(int x, int y)> antinodes = new HashSet<(int x, int y)>();
        foreach (var frequency in antennas.Keys)
        {
            var positions = antennas[frequency];

            // Iterate through all pairs of antennas with the same frequency
            for (int i = 0; i < positions.Count; i++)
            {
                for (int j = i + 1; j < positions.Count; j++)
                {
                    var p1 = positions[i];
                    var p2 = positions[j];

                    // Calculate the vector difference
                    int dx = p2.x - p1.x;
                    int dy = p2.y - p1.y;

                    // Calculate antinode positions
                    (int x, int y) antinode1 = (p1.x - dx, p1.y - dy);
                    (int x, int y) antinode2 = (p2.x + dx, p2.y + dy);

                    // Add antinodes to the set if they are within bounds
                    if (IsInBounds(antinode1, grid))
                        antinodes.Add(antinode1);
                    if (IsInBounds(antinode2, grid))
                        antinodes.Add(antinode2);
                }
            }
        }

        //Mark all antinodes on the output grid
        foreach (var antinode in antinodes)
        {
            if (outputGrid[antinode.y, antinode.x] == '.')
                outputGrid[antinode.y, antinode.x] = '#';
        }

        //Printing out the result grid to check for mistakes
        Console.WriteLine("Output Grid with Antinodes:");
        for (int y = 0; y < outputGrid.GetLength(0); y++)
        {
            for (int x = 0; x < outputGrid.GetLength(1); x++)
            {
                Console.Write(outputGrid[y, x]);
            }
            Console.WriteLine();
        }

        //Prints out the total number
        Console.WriteLine($"Total number of unique antinodes: {antinodes.Count}");
    }

    private static bool IsInBounds((int x, int y) pos, string[] grid)
    {
        return pos.x >= 0 && pos.x < grid[0].Length && pos.y >= 0 && pos.y < grid.Length;
    }
}

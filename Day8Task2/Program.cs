using System;

namespace AdventOfCode.Days.Day8
{
    public class Day8Task2
    {
        public static void Main(string[] args) // Entry point
        {
            Run(); // Call the Run method
        }

        public static void Run()
        {
            string filePath = "Antenas.txt"; // Replace with your file path.

            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine($"File not found: {filePath}");
                    return;
                }

                var lines = System.IO.File.ReadAllLines(filePath);
                if (lines.Length == 0)
                {
                    Console.WriteLine("The file is empty.");
                    return;
                }

                int rows = lines.Length;
                int cols = lines[0].Length;

                var antennaPositions = new System.Collections.Generic.Dictionary<char, System.Collections.Generic.List<(int x, int y)>>();

                // Parse the input grid
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < cols; x++)
                    {
                        char cell = lines[y][x];
                        if (char.IsLetterOrDigit(cell))
                        {
                            if (!antennaPositions.ContainsKey(cell))
                                antennaPositions[cell] = new System.Collections.Generic.List<(int, int)>();
                            antennaPositions[cell].Add((x, y));
                        }
                    }
                }

                var antinodeLocations = new System.Collections.Generic.HashSet<(int, int)>();

                // Process each frequency group
                foreach (var frequency in antennaPositions.Keys)
                {
                    var positions = antennaPositions[frequency];

                    // Check all pairs of antennas
                    for (int i = 0; i < positions.Count; i++)
                    {
                        for (int j = i + 1; j < positions.Count; j++)
                        {
                            var (x1, y1) = positions[i];
                            var (x2, y2) = positions[j];

                            // Add all points along the infinite line between (x1, y1) and (x2, y2)
                            AddCollinearPoints(antinodeLocations, x1, y1, x2, y2, rows, cols);
                        }
                    }

                    // Add all antennas themselves if part of collinear relationships
                    foreach (var pos in positions)
                    {
                        antinodeLocations.Add(pos);
                    }
                }

                Console.WriteLine($"Number of unique antinode locations: {antinodeLocations.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void AddCollinearPoints(System.Collections.Generic.HashSet<(int, int)> antinodeLocations, int x1, int y1, int x2, int y2, int rows, int cols)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int gcd = GCD(Math.Abs(dx), Math.Abs(dy)); // Simplify the step sizes

            dx /= gcd;
            dy /= gcd;

            // Extend in both directions
            int cx = x1, cy = y1;

            // Traverse backward
            while (cx >= 0 && cx < cols && cy >= 0 && cy < rows)
            {
                antinodeLocations.Add((cx, cy));
                cx -= dx;
                cy -= dy;
            }

            // Traverse forward from x1, y1 again
            cx = x1;
            cy = y1;
            while (cx >= 0 && cx < cols && cy >= 0 && cy < rows)
            {
                antinodeLocations.Add((cx, cy));
                cx += dx;
                cy += dy;
            }
        }

        // Helper to calculate GCD
        private static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}

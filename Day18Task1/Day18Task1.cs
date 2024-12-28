using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Day18
{
    public class Day18Task1
    {
        private const int Width = 71;
        private const int Height = 71;
        private const int ByteCount = 1024;

        private char[,] _map = new char[Width, Height];

        public static async Task Main()
        {
            // Open the file stream
            string filePath = "day18.txt";  // Adjust the path if necessary
            using (var reader = new StreamReader(filePath))
            {
                var task = new Day18Task1();
                string result = await task.SolveAsync(reader);
                Console.WriteLine(result); // Only output the final result
            }
        }

        public async Task<string> SolveAsync(StreamReader inputReader)
        {
            // Initialize the grid
            _map = new char[Width, Height];

            // Read input and mark corrupted spaces
            for (int i = 0; i < ByteCount; i++)
            {
                var line = await inputReader.ReadLineAsync();
                if (line == null)
                {
                    break;
                }

                var parts = line.Split(",");
                var x = int.Parse(parts[0]);
                var y = int.Parse(parts[1]);

                if (x < 0 || x >= Width || y < 0 || y >= Height)
                {
                    continue;
                }

                _map[x, y] = '#'; // Mark corrupted
            }

            // Find shortest path
            var length = FindPath(new Point(0, 0), new Point(Width - 1, Height - 1));
            if (length == -1)
            {
                return "No path found.";
            }

            return length.ToString();
        }

        private int FindPath(Point start, Point end)
        {
            var queue = new Queue<(Point position, int steps)>();
            queue.Enqueue((start, 0));
            var visited = new HashSet<Point>();
            visited.Add(start);

            while (queue.Count > 0)
            {
                var (position, steps) = queue.Dequeue();
                
                // If we reached the destination, return the steps
                if (position.Equals(end))
                {
                    return steps;
                }

                foreach (var direction in Directions.WithoutDiagonals)
                {
                    var next = position + direction;

                    // Skip out-of-bounds, corrupted or visited positions
                    if (next.X < 0 || next.X >= Width || next.Y < 0 || next.Y >= Height ||
                        _map[next.X, next.Y] == '#' || visited.Contains(next))
                    {
                        continue;
                    }

                    visited.Add(next);
                    queue.Enqueue((next, steps + 1));
                }
            }

            return -1; // No path found
        }
    }

    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

        public override bool Equals(object? obj) =>
            obj is Point point && point.X == X && point.Y == Y;

        public override int GetHashCode() => HashCode.Combine(X, Y);
    }

    public static class Directions
    {
        public static readonly Point[] WithoutDiagonals = new[] {
            new Point(0, 1), // Up
            new Point(1, 0), // Right
            new Point(0, -1), // Down
            new Point(-1, 0) // Left
        };
    }
}

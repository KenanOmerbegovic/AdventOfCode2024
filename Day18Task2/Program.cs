using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2024._18.Part2
{
    public class Part2
    {
        private const int Width = 71;
        private const int Height = 71;

        private char[,] _map = new char[Width, Height];

        public async Task<string> SolveAsync(string filePath)
        {
            // Initialize the map
            _map = new char[Width, Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    _map[x, y] = '.';

            var lines = await File.ReadAllLinesAsync(filePath);

            HashSet<Point> lastPath = new();
            foreach (var line in lines)
            {
                var parts = line.Split(",");
                var x = int.Parse(parts[0]);
                var y = int.Parse(parts[1]);

                _map[x, y] = '#';

                if (lastPath.Count == 0 || lastPath.Contains(new Point(x, y)))
                {
                    var (length, path) = FindPath(new Point(0, 0), new Point(Width - 1, Height - 1));

                    if (length == -1)
                    {
                        return $"{x},{y}";
                    }

                    lastPath = path;
                }
            }

            return "Path Found!";
        }

        private (int length, HashSet<Point> points) FindPath(Point start, Point end)
        {
            var queue = new Queue<(Point position, int steps)>();
            queue.Enqueue((start, 0));
            var visited = new HashSet<Point>();
            var previous = new Dictionary<Point, Point>();
            visited.Add(start);

            while (queue.Count > 0)
            {
                var (position, steps) = queue.Dequeue();

                if (position == end)
                {
                    var path = new HashSet<Point>();
                    var current = position;
                    while (current != start)
                    {
                        path.Add(current);
                        current = previous[current];
                    }

                    return (steps, path);
                }

                foreach (var direction in Directions.WithoutDiagonals)
                {
                    var next = position + direction;

                    if (next.X < 0 || next.X >= Width || next.Y < 0 || next.Y >= Height ||
                        _map[next.X, next.Y] == '#' ||
                        visited.Contains(next))
                    {
                        continue;
                    }

                    visited.Add(next);
                    previous[next] = position;
                    queue.Enqueue((next, steps + 1));
                }
            }

            return (-1, null);
        }

        // Entry point for the program
        public static async Task Main(string[] args)
        {
            var part2 = new Part2();

            // Specify the input file path
            string filePath = "day18.txt";

            // Solve the puzzle and print the result
            var result = await part2.SolveAsync(filePath);
            Console.WriteLine($"Result: {result}");
        }
    }

    // Utility structure for points
    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object obj)
        {
            return obj is Point point && this == point;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    // Directions utility class
    public static class Directions
    {
        public static readonly Point[] WithoutDiagonals =
        {
            new Point(0, -1), // Up
            new Point(0, 1),  // Down
            new Point(-1, 0), // Left
            new Point(1, 0)   // Right
        };
    }
}

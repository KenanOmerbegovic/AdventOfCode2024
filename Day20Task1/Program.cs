using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2024._20.Part1
{
    public class Part1
    {
        private int _width;
        private int _height;
        private int[,] _map;
        private Point _start;
        private Point _end;

        private const int WantToSave = 100;
        private const int CheatTime = 2;

        public async Task<string> SolveAsync(string filePath)
        {
            // Read all lines from the file
            var lines = await File.ReadAllLinesAsync(filePath);
            _height = lines.Length;
            _width = lines[0].Length;

            // Initialize the map
            _map = new int[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _map[x, y] = lines[y][x] == '#' ? -1 : 0;
                    if (lines[y][x] == 'S')
                    {
                        _start = new Point(x, y);
                    }
                    else if (lines[y][x] == 'E')
                    {
                        _end = new Point(x, y);
                    }
                }
            }

            FindDistances(_start, _end);

            return CountCheats().ToString();
        }

        private int CountCheats()
        {
            var cheats = 0;
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_map[x, y] == -1)
                    {
                        continue;
                    }
                    var start = new Point(x, y);
                    cheats += CountCheats(start);
                }
            }

            return cheats;
        }

        private int CountCheats(Point start)
        {
            HashSet<Point> targets = new();
            Queue<(Point position, int steps)> queue = new();
            queue.Enqueue((start, 0));
            while (queue.Count > 0)
            {
                var (position, steps) = queue.Dequeue();

                if (steps == CheatTime && _map[position.X, position.Y] != -1)
                {
                    var saved = _map[position.X, position.Y] - _map[start.X, start.Y] - CheatTime;
                    if (saved >= WantToSave)
                    {
                        targets.Add(position);
                    }
                }

                if (steps >= CheatTime)
                {
                    continue;
                }

                foreach (var direction in Directions.WithoutDiagonals)
                {
                    var next = position + direction;
                    if (next.X < 0 || next.X >= _width || next.Y < 0 || next.Y >= _height)
                    {
                        continue;
                    }

                    queue.Enqueue((next, steps + 1));
                }
            }

            return targets.Count;
        }

        private void FindDistances(Point start, Point end)
        {
            var currentPoint = start;

            while (currentPoint != end)
            {
                foreach (var direction in Directions.WithoutDiagonals)
                {
                    var next = currentPoint + direction;
                    if (_map[next.X, next.Y] == 0 && next != start)
                    {
                        _map[next.X, next.Y] = _map[currentPoint.X, currentPoint.Y] + 1;
                        currentPoint = next;
                        break;
                    }
                }
            }
        }

        // Entry point for the program
        public static async Task Main(string[] args)
        {
            var part1 = new Part1();

            // Specify the input file path
            string filePath = "day20.txt";

            // Solve the puzzle and print the result
            var result = await part1.SolveAsync(filePath);
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

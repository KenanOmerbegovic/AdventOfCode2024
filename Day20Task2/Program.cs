using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2024._20.Part2
{
    public class Part2
    {
        private int _width;
        private int _height;
        private int[,] _map;
        private Point _start;
        private Point _end;

        private const int WantToSave = 100;
        private const int CheatTime = 20;

        public async Task<string> SolveAsync(string filePath)
        {
            var lines = await File.ReadAllLinesAsync(filePath);
            _height = lines.Length;
            _width = lines[0].Length;

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
            HashSet<Point> targets = new HashSet<Point>();

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var distance = Math.Abs(start.X - x) + Math.Abs(start.Y - y);
                    if (distance > CheatTime)
                    {
                        continue;
                    }

                    var saved = _map[x, y] - _map[start.X, start.Y] - distance;
                    if (saved >= WantToSave)
                    {
                        targets.Add(new Point(x, y));
                    }
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

        // Entry point
        public static async Task Main(string[] args)
        {
            var part2 = new Part2();

            // Specify the input file path
            string filePath = "day20.txt";

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

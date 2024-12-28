using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2024._21.Part1
{
    public class Part1
    {
        private char[,] _numericKeypad = new char[,]
        {
            { '7', '8', '9' },
            { '4', '5', '6' },
            { '1', '2', '3' },
            { '#', '0', 'A' }
        };

        private char[,] _directionalKeypad = new char[,]
        {
            { '#', '^', 'A' },
            { '<', 'v', '>' }
        };

        private Dictionary<(char from, char to), List<string>> _shortestSequences = new();

        public async Task<string> SolveAsync(string filePath)
        {
            CacheAllShortestSequences();

            int result = 0;
            var codes = await File.ReadAllLinesAsync(filePath);
            foreach (var code in codes)
            {
                var numericCode = int.Parse(string.Join("", code.Where(char.IsDigit)));
                var shortestSequence = GetShortestSequence(code, 0);
                result += shortestSequence.Length * numericCode;
            }

            return result.ToString();
        }

        private string GetShortestSequence(string code, int layer)
        {
            if (layer == 3)
            {
                return code;
            }

            string best = "";
            char previous = 'A';
            for (int codeIndex = 0; codeIndex < code.Length; codeIndex++)
            {
                var current = code[codeIndex];

                var keypad = layer == 0 ? _numericKeypad : _directionalKeypad;
                var paths = _shortestSequences;

                var shortestPaths = paths[(previous, current)];

                string currentPairBest = null;
                foreach (var path in shortestPaths)
                {
                    var entry = GetShortestSequence(path, layer + 1);
                    if (currentPairBest is null || currentPairBest.Length > entry.Length)
                    {
                        currentPairBest = entry;
                    }
                }

                best += currentPairBest;

                previous = current;
            }

            return best;
        }

        private void CacheAllShortestSequences()
        {
            CacheAllShortestSequences(_numericKeypad);
            CacheAllShortestSequences(_directionalKeypad);
        }

        private void CacheAllShortestSequences(char[,] keypad)
        {
            foreach (var character in keypad)
            {
                foreach (var otherCharacter in keypad)
                {
                    _shortestSequences[(character, otherCharacter)] = new List<string>();

                    if (character == otherCharacter)
                    {
                        _shortestSequences[(character, otherCharacter)].Add("A");
                        continue;
                    }

                    CacheShortestSequence(character, otherCharacter, keypad);
                }
            }
        }

        private void CacheShortestSequence(char from, char to, char[,] keypad)
        {
            var fromPoint = FindCharacter(from, keypad);
            var toPoint = FindCharacter(to, keypad);

            var shortestPathLength = Math.Abs(fromPoint.X - toPoint.X) + Math.Abs(fromPoint.Y - toPoint.Y);

            Queue<(Point point, string sequence)> queue = new();
            queue.Enqueue((fromPoint, ""));
            while (queue.Count > 0)
            {
                var (point, sequence) = queue.Dequeue();

                if (point == toPoint && sequence.Length == shortestPathLength)
                {
                    _shortestSequences[(from, to)].Add(sequence + "A");
                    continue;
                }

                if (sequence.Length >= shortestPathLength)
                {
                    continue;
                }

                foreach (var direction in Directions.WithoutDiagonals)
                {
                    var nextPoint = point + direction;
                    if (nextPoint.X >= 0 && nextPoint.X < keypad.GetLength(1) &&
                        nextPoint.Y >= 0 && nextPoint.Y < keypad.GetLength(0) &&
                        keypad[nextPoint.Y, nextPoint.X] != '#')
                    {
                        var directionalCharater = direction switch
                        {
                            (0, 1) => 'v',
                            (1, 0) => '>',
                            (0, -1) => '^',
                            (-1, 0) => '<',
                            _ => throw new InvalidOperationException("Invalid direction.")
                        };
                        queue.Enqueue((nextPoint, sequence + directionalCharater));
                    }
                }
            }
        }

        private Point FindCharacter(char character, char[,] keypad)
        {
            for (int y = 0; y < keypad.GetLength(0); y++)
            {
                for (int x = 0; x < keypad.GetLength(1); x++)
                {
                    if (keypad[y, x] == character)
                    {
                        return new Point(x, y);
                    }
                }
            }
            throw new InvalidOperationException($"Character '{character}' not found in keypad.");
        }
    }

    // Modified Point struct with Deconstruct method
    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Define the Deconstruct method
        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
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
    
    // Entry point for the program
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var part1 = new Part1();

            // Specify the input file path
            string filePath = "day21.txt"; // Change this to the actual file path

            // Solve the puzzle and print the result
            var result = await part1.SolveAsync(filePath);
            Console.WriteLine($"Result: {result}");
        }
    }
}

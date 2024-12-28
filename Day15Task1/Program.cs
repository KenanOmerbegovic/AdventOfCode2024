using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Days.Day15
{
    public class Day15Task1 // Declare the class Day15Task1
    {
        private int _width;
        private int _height;
        private char[,] _map;
        private string _instructions;
        private Point _robotPosition;

        public Day15Task1()
        {
            _map = new char[0, 0]; // Correctly initialize the 2D array
            _instructions = string.Empty;
            _robotPosition = new Point(0, 0);
        }

        public void Solve(string[] mapInput, string instructionsInput)
        {
            ParseMap(mapInput);
            ParseInstructions(instructionsInput);

            foreach (var instruction in _instructions)
            {
                MoveRobot(instruction);
            }

            var total = CalculateTotal();
            Console.WriteLine($"Total: {total}");
        }

        private void ParseMap(string[] mapLines)
        {
            _width = mapLines[0].Length;
            _height = mapLines.Length;
            _map = new char[_width, _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _map[x, y] = mapLines[y][x];

                    if (_map[x, y] == '@')
                    {
                        _robotPosition = new Point(x, y);
                    }
                }
            }
        }

        private void ParseInstructions(string instructions)
        {
            _instructions = instructions;
        }

        private void MoveRobot(char instruction)
        {
            var direction = instruction switch
            {
                '^' => new Point(0, -1),
                '>' => new Point(1, 0),
                'v' => new Point(0, 1),
                '<' => new Point(-1, 0),
                _ => throw new InvalidOperationException("Invalid instruction")
            };

            if (CanMove(_robotPosition, direction))
            {
                Move(_robotPosition, direction);
                _robotPosition += direction;
            }
        }

        private bool CanMove(Point position, Point direction)
        {
            var newPosition = position + direction;

            if (newPosition.X < 0 || newPosition.X >= _width || newPosition.Y < 0 || newPosition.Y >= _height)
            {
                return false;
            }

            var newTile = _map[newPosition.X, newPosition.Y];
            return newTile switch
            {
                '#' => false,
                'O' => CanMove(newPosition, direction),
                '.' => true,
                _ => throw new InvalidOperationException("Invalid tile")
            };
        }

        private void Move(Point position, Point direction)
        {
            var newPosition = position + direction;
            var newTile = _map[newPosition.X, newPosition.Y];
            if (newTile != '.')
            {
                Move(newPosition, direction);
            }

            _map[newPosition.X, newPosition.Y] = _map[position.X, position.Y];
            _map[position.X, position.Y] = '.';
        }

        private int CalculateTotal()
        {
            var total = 0;
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_map[x, y] == 'O')
                    {
                        total += y * 100 + x;
                    }
                }
            }

            return total;
        }
    }

    public class Program
    {
        public static void Main()
        {
            const string inputFilePath = "day15gps.txt"; // Path to your input file

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("Input file not found. Please create a day15gps.txt file in the program's directory.");
                return;
            }

            var lines = File.ReadAllLines(inputFilePath);

            // Separate map and instructions
            var mapLines = new List<string>();
            var instructions = string.Empty;

            bool readingInstructions = false;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    readingInstructions = true;
                    continue;
                }

                if (!readingInstructions)
                {
                    mapLines.Add(line);
                }
                else
                {
                    instructions += line.Trim();
                }
            }

            var task = new Day15Task1();
            task.Solve(mapLines.ToArray(), instructions);
        }
    }

    public record Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
    }
}

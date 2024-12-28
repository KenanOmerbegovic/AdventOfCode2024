using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Days.Day15
{
    public class Day15Task2
    {
        private int _width;
        private int _height;
        private char[,] _map; // Declare without initializing
        private string _instructions = string.Empty; // Initialize as empty string
        private Point _robotPosition = new Point(0, 0); // Initialize to default position

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
            _width = mapLines[0].Length * 2; // Each character is expanded into two columns
            _height = mapLines.Length;
            _map = new char[_width, _height]; // Initialize with the correct dimensions

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < mapLines[0].Length; x++)
                {
                    var character = mapLines[y][x];

                    if (character == '@')
                    {
                        _robotPosition = new Point(x * 2, y);
                        _map[x * 2, y] = '@';
                        _map[x * 2 + 1, y] = '.';
                    }
                    else if (character == '#')
                    {
                        _map[x * 2, y] = '#';
                        _map[x * 2 + 1, y] = '#';
                    }
                    else if (character == '.')
                    {
                        _map[x * 2, y] = '.';
                        _map[x * 2 + 1, y] = '.';
                    }
                    else if (character == 'O')
                    {
                        _map[x * 2, y] = '[';
                        _map[x * 2 + 1, y] = ']';
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
            var currentTile = _map[position.X, position.Y];

            if (currentTile == '.')
            {
                return true;
            }

            if (currentTile == '#')
            {
                return false;
            }

            var newTile = _map[newPosition.X, newPosition.Y];

            if (currentTile == '[' || currentTile == ']')
            {
                if ((currentTile == ']' && direction == new Point(-1, 0)) ||
                    (currentTile == '[' && direction == new Point(1, 0)))
                {
                    return CanMove(newPosition, direction);
                }

                if (currentTile == ']' && (direction == new Point(0, -1) || direction == new Point(0, 1)))
                {
                    return CanMove(newPosition, direction) && CanMove(newPosition + new Point(-1, 0), direction);
                }

                if (currentTile == '[' && (direction == new Point(0, -1) || direction == new Point(0, 1)))
                {
                    return CanMove(newPosition, direction) && CanMove(newPosition + new Point(1, 0), direction);
                }

                return CanMove(newPosition, direction);
            }

            if (currentTile == '@')
            {
                return CanMove(newPosition, direction);
            }

            throw new InvalidOperationException();
        }

        private void Move(Point position, Point direction)
        {
            var newPosition = position + direction;

            var currentTile = _map[position.X, position.Y];
            var newTile = _map[newPosition.X, newPosition.Y];

            if (direction == new Point(-1, 0) || direction == new Point(1, 0))
            {
                if (newTile != '.')
                {
                    Move(newPosition, direction);
                }
            }
            else
            {
                if (newTile == '[')
                {
                    Move(newPosition + new Point(1, 0), direction);
                    Move(newPosition, direction);
                }
                else if (newTile == ']')
                {
                    Move(newPosition + new Point(-1, 0), direction);
                    Move(newPosition, direction);
                }
                else if (newTile != '.')
                {
                    Move(newPosition, direction);
                }
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
                    if (_map[x, y] == '[')
                    {
                        total += y * 100 + x;
                    }
                }
            }

            return total;
        }

        public static void Main()
        {
            const string inputFilePath = "day15gps.txt"; // Path to your input file

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("Input file not found. Please create a Days/Day15/Day15.txt file.");
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

            var task = new Day15Task2();
            task.Solve(mapLines.ToArray(), instructions);
        }
    }

    public record Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
    }
}

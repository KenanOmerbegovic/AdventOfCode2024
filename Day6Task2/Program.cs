using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Days.Day6
{
    public class Day6Task2
    {
        // Entry point for the application
        public static void Main()
        {
            Run();
        }

        public static void Run()
        {
            // File path to the map file
            string filePath = "MAP.txt";

            try
            {
                // Read the file and validate input
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"File not found: {filePath}");
                    return;
                }

                var lines = File.ReadAllLines(filePath);
                if (lines.Length == 0)
                {
                    Console.WriteLine("The file is empty.");
                    return;
                }

                // Parse the map into a 2D array
                int rows = lines.Length;
                int cols = lines[0].Length;
                char[,] map = new char[rows, cols];
                (int x, int y) guardPosition = (0, 0);
                char guardDirection = '^';

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        map[i, j] = lines[i][j];
                        if ("^>v<".Contains(map[i, j]))
                        {
                            guardPosition = (i, j);
                            guardDirection = map[i, j];
                            map[i, j] = '.'; // Replace guard symbol with empty space
                        }
                    }
                }

                // Directions and their corresponding movements
                Dictionary<char, (int dx, int dy)> directions = new Dictionary<char, (int, int)> {
                    { '^', (-1, 0) }, // Up
                    { '>', (0, 1) },  // Right
                    { 'v', (1, 0) },  // Down
                    { '<', (0, -1) }  // Left
                };

                char[] directionOrder = { '^', '>', 'v', '<' }; // Clockwise order for turning

                // Find all valid positions to place an obstruction
                int validObstructionCount = 0;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        // Skip positions that are not empty or are the starting position
                        if (map[i, j] != '.' || (i == guardPosition.x && j == guardPosition.y))
                            continue;

                        // Place a hypothetical obstruction
                        map[i, j] = 'O';

                        // Check if this causes a loop
                        if (DoesGuardGetStuck(map, guardPosition, guardDirection, directions, directionOrder))
                        {
                            validObstructionCount++;
                        }

                        // Remove the hypothetical obstruction
                        map[i, j] = '.';
                    }
                }

                Console.WriteLine($"Number of valid obstruction positions: {validObstructionCount}");
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static bool DoesGuardGetStuck(
            char[,] map,
            (int x, int y) startPosition,
            char startDirection,
            Dictionary<char, (int dx, int dy)> directions,
            char[] directionOrder)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            HashSet<(int x, int y, char dir)> visitedStates = new HashSet<(int, int, char)>();
            (int x, int y) guardPosition = startPosition;
            char guardDirection = startDirection;

            while (true)
            {
                // Record the current state
                if (!visitedStates.Add((guardPosition.x, guardPosition.y, guardDirection)))
                {
                    // If the state has been seen before, the guard is stuck in a loop
                    return true;
                }

                // Calculate the next position based on the current direction
                (int dx, int dy) = directions[guardDirection];
                (int nextX, int nextY) = (guardPosition.x + dx, guardPosition.y + dy);

                // Check if the next position is out of bounds
                if (nextX < 0 || nextX >= rows || nextY < 0 || nextY >= cols)
                    return false;

                // Check if the next position is an obstacle
                if (map[nextX, nextY] == '#' || map[nextX, nextY] == 'O')
                {
                    // Turn right 90 degrees
                    int currentDirectionIndex = Array.IndexOf(directionOrder, guardDirection);
                    guardDirection = directionOrder[(currentDirectionIndex + 1) % 4];
                }
                else
                {
                    // Move forward
                    guardPosition = (nextX, nextY);
                }
            }
        }
    }
}

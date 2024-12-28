using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Days.Day6
{
    public class Day6Task1
    {
        public static void Main() // <-- This is the entry point
        {
            Run();
        }

        public static void Run()
        { 
            string filePath = "MAP.txt";

            try
            {
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
                            map[i, j] = '.';
                        }
                    }
                }
                Dictionary<char, (int dx, int dy)> directions = new Dictionary<char, (int, int)> {
                    { '^', (-1, 0) },
                    { '>', (0, 1) },
                    { 'v', (1, 0) },
                    { '<', (0, -1) }
                };

                char[] directionOrder = { '^', '>', 'v', '<' };
                HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>();
                visited.Add(guardPosition);
                while (true)
                {
                    (int dx, int dy) = directions[guardDirection];
                    (int nextX, int nextY) = (guardPosition.x + dx, guardPosition.y + dy);
                    if (nextX < 0 || nextX >= rows || nextY < 0 || nextY >= cols)
                        break;
                    if (map[nextX, nextY] == '#')
                    {
                        int currentDirectionIndex = Array.IndexOf(directionOrder, guardDirection);
                        guardDirection = directionOrder[(currentDirectionIndex + 1) % 4];
                    }
                    else
                    {
                        guardPosition = (nextX, nextY);
                        visited.Add(guardPosition);
                    }
                }
                foreach (var pos in visited)
                {
                    map[pos.x, pos.y] = 'X';
                }

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        Console.Write(map[i, j]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine($"Distinct positions visited: {visited.Count}");

            }

            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

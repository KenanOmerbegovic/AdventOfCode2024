using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    const int MaxSeconds = 20000; // Safety limit to avoid infinite runs

    static void Main()
    {
        // Read input from the file
        string[] input = File.ReadAllLines("Robots.txt");

        // Parse input
        List<(int x, int y, int vx, int vy)> robots = ParseInput(input);

        int smallestArea = int.MaxValue;
        int timeAtSmallest = 0;

        for (int t = 0; t < MaxSeconds; t++)
        {
            var positions = GetPositionsAtTime(robots, t);
            var boundingBox = GetBoundingBox(positions);

            int area = boundingBox.width * boundingBox.height;

            if (area < smallestArea)
            {
                smallestArea = area;
                timeAtSmallest = t;
            }
            else
            {
                // Bounding box starts increasing; print the result
                Console.WriteLine($"\nSmallest bounding box found at t = {timeAtSmallest} seconds.");
                PrintGridAtTime(robots, timeAtSmallest);
                break;
            }
        }
    }

    static List<(int x, int y, int vx, int vy)> ParseInput(string[] input)
    {
        var robots = new List<(int, int, int, int)>();

        foreach (string line in input)
        {
            string[] parts = line.Split(' ');
            string[] posParts = parts[0].Substring(2).Split(',');
            string[] velParts = parts[1].Substring(2).Split(',');

            int x = int.Parse(posParts[0]);
            int y = int.Parse(posParts[1]);
            int vx = int.Parse(velParts[0]);
            int vy = int.Parse(velParts[1]);

            robots.Add((x, y, vx, vy));
        }

        return robots;
    }

    static List<(int x, int y)> GetPositionsAtTime(List<(int x, int y, int vx, int vy)> robots, int time)
    {
        var positions = new List<(int x, int y)>();
        foreach (var robot in robots)
        {
            int newX = robot.x + time * robot.vx;
            int newY = robot.y + time * robot.vy;
            positions.Add((newX, newY));
        }
        return positions;
    }

    static (int minX, int minY, int width, int height) GetBoundingBox(List<(int x, int y)> positions)
    {
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (var pos in positions)
        {
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y < minY) minY = pos.y;
            if (pos.y > maxY) maxY = pos.y;
        }

        return (minX, minY, maxX - minX + 1, maxY - minY + 1);
    }

    static void PrintGridAtTime(List<(int x, int y, int vx, int vy)> robots, int time)
    {
        var positions = GetPositionsAtTime(robots, time);
        var boundingBox = GetBoundingBox(positions);

        bool[,] grid = new bool[boundingBox.width, boundingBox.height];

        foreach (var pos in positions)
        {
            int x = pos.x - boundingBox.minX;
            int y = pos.y - boundingBox.minY;
            grid[x, y] = true;
        }

        for (int y = 0; y < boundingBox.height; y++)
        {
            for (int x = 0; x < boundingBox.width; x++)
            {
                Console.Write(grid[x, y] ? "#" : ".");
            }
            Console.WriteLine();
        }
    }
}

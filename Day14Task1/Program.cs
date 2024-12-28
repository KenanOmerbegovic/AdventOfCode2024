using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    const int Width = 101;
    const int Height = 103;
    const int Time = 100;

    static void Main()
    {
        // Read input from a text file
        string[] input = File.ReadAllLines("Robots.txt");
        
        List<(int x, int y, int vx, int vy)> robots = ParseInput(input);

        // Simulate robot positions after 100 seconds
        List<(int x, int y)> positions = new List<(int, int)>();

        Console.WriteLine("=== Robot Final Positions After 100 Seconds ===");
        foreach (var robot in robots)
        {
            int newX = ((robot.x + Time * robot.vx) % Width + Width) % Width;
            int newY = ((robot.y + Time * robot.vy) % Height + Height) % Height;

            positions.Add((newX, newY));
            Console.WriteLine($"Robot starting at ({robot.x}, {robot.y}) with velocity ({robot.vx}, {robot.vy}) -> Final Position: ({newX}, {newY})");
        }

        // Count robots in quadrants
        int q1 = 0, q2 = 0, q3 = 0, q4 = 0;

        foreach (var pos in positions)
        {
            if (pos.x == Width / 2 || pos.y == Height / 2)
                continue; // Ignore robots exactly on center lines

            if (pos.x < Width / 2 && pos.y < Height / 2) q1++; // Top-left
            else if (pos.x >= Width / 2 && pos.y < Height / 2) q2++; // Top-right
            else if (pos.x < Width / 2 && pos.y >= Height / 2) q3++; // Bottom-left
            else if (pos.x >= Width / 2 && pos.y >= Height / 2) q4++; // Bottom-right
        }

        // Debug: Print quadrant counts
        Console.WriteLine("\n=== Quadrant Counts ===");
        Console.WriteLine($"Q1 (Top-Left): {q1}");
        Console.WriteLine($"Q2 (Top-Right): {q2}");
        Console.WriteLine($"Q3 (Bottom-Left): {q3}");
        Console.WriteLine($"Q4 (Bottom-Right): {q4}");

        // Calculate the safety factor
        int safetyFactor = q1 * q2 * q3 * q4;

        Console.WriteLine($"\nSafety Factor: {safetyFactor}");
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
}

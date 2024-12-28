using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Read input from a text file
        string filePath = "topographic.txt";
        string[] input;

        try
        {
            input = File.ReadAllLines(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            return;
        }

        // Parse the input into a 2D array of integers
        int rows = input.Length;
        int cols = input[0].Length;
        int[,] map = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                map[i, j] = input[i][j] - '0';
            }
        }

        // Directions for up, down, left, right
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        // Function to perform DFS and find all distinct paths to reachable 9s
        int CountDistinctPaths(int x, int y, HashSet<(int, int)> visited, List<(int, int)> path)
        {
            if (map[x, y] == 9) // Reached a 9
            {
                return 1;
            }

            int pathCount = 0;

            for (int dir = 0; dir < 4; dir++)
            {
                int nx = x + dx[dir];
                int ny = y + dy[dir];

                // Check bounds
                if (nx < 0 || ny < 0 || nx >= rows || ny >= cols)
                    continue;

                // Check if the move is valid (increasing by exactly 1) and not revisiting
                if (!visited.Contains((nx, ny)) && map[nx, ny] == map[x, y] + 1)
                {
                    visited.Add((nx, ny));
                    path.Add((nx, ny));
                    pathCount += CountDistinctPaths(nx, ny, visited, path);
                    visited.Remove((nx, ny));
                    path.RemoveAt(path.Count - 1);
                }
            }

            return pathCount;
        }

        // Iterate through the map to find all trailheads and calculate their ratings
        int totalRating = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (map[i, j] == 0) // Trailhead found
                {
                    HashSet<(int, int)> visited = new HashSet<(int, int)> { (i, j) };
                    List<(int, int)> path = new List<(int, int)> { (i, j) };
                    int rating = CountDistinctPaths(i, j, visited, path);
                    Console.WriteLine($"Trailhead at ({i}, {j}) has a rating of {rating}");
                    totalRating += rating;
                }
            }
        }

        // Output the total rating
        Console.WriteLine("Total Rating: " + totalRating);
    }
}

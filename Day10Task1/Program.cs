using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Read input from a text file
        string filePath = "topographic.txt"; // Ensure the file is in the same directory as the executable
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

        // Function to perform DFS and find all reachable 9s
        HashSet<(int, int)> FindReachableNines(int startX, int startY)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            HashSet<(int, int)> nines = new HashSet<(int, int)>();

            stack.Push((startX, startY));
            visited.Add((startX, startY));

            while (stack.Count > 0)
            {
                var (x, y) = stack.Pop();
                for (int dir = 0; dir < 4; dir++)
                {
                    int nx = x + dx[dir];
                    int ny = y + dy[dir];

                    // Check bounds
                    if (nx < 0 || ny < 0 || nx >= rows || ny >= cols)
                        continue;

                    // Check if the move is valid (increasing by exactly 1)
                    if (!visited.Contains((nx, ny)) && map[nx, ny] == map[x, y] + 1)
                    {
                        visited.Add((nx, ny));
                        stack.Push((nx, ny));

                        // If it's a 9, add to the set of reachable 9s
                        if (map[nx, ny] == 9)
                        {
                            nines.Add((nx, ny));
                        }
                    }
                }
            }

            return nines;
        }

        // Iterate through the map to find all trailheads and calculate their scores
        int totalScore = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (map[i, j] == 0) // Trailhead found
                {
                    var reachableNines = FindReachableNines(i, j);
                    int score = reachableNines.Count;
                    Console.WriteLine($"Trailhead at ({i}, {j}) has a score of {score}");
                    totalScore += score;
                }
            }
        }

        // Output the total score
        Console.WriteLine("Total Score: " + totalScore);
    }
}

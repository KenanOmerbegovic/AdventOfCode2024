using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    // Directions: North, East, South, West
    static readonly (int dr, int dc)[] Directions = { (-1, 0), (0, 1), (1, 0), (0, -1) };
    static readonly string[] DirectionNames = { "N", "E", "S", "W" };

    static void Main()
    {
        // Load the maze from the input file
        var maze = File.ReadAllLines("maze16.txt");
        int rows = maze.Length, cols = maze[0].Length;

        // Find Start and End positions
        (int startRow, int startCol) = FindStart(maze, 'S');
        (int endRow, int endCol) = FindStart(maze, 'E');

        // Priority Queue for Dijkstra's algorithm
        var pq = new PriorityQueue<(int row, int col, int dir, int score), int>();
        var visited = new HashSet<(int, int, int)>(); // To track (row, col, direction)

        // Initialize the queue with the starting position facing East (1)
        pq.Enqueue((startRow, startCol, 1, 0), 0);

        while (pq.Count > 0)
        {
            var (row, col, dir, score) = pq.Dequeue();

            // If we reach the End Tile, print the result and exit
            if (row == endRow && col == endCol)
            {
                Console.WriteLine($"Lowest Score: {score}");
                return;
            }

            // Skip if this state has already been visited
            if (visited.Contains((row, col, dir))) continue;
            visited.Add((row, col, dir));

            // 1. Move Forward
            var (dr, dc) = Directions[dir];
            int newRow = row + dr, newCol = col + dc;
            if (IsInBounds(newRow, newCol, rows, cols) && maze[newRow][newCol] != '#')
            {
                pq.Enqueue((newRow, newCol, dir, score + 1), score + 1);
            }

            // 2. Rotate Clockwise and Counterclockwise
            int clockwiseDir = (dir + 1) % 4; // Turn right
            int counterClockwiseDir = (dir + 3) % 4; // Turn left
            pq.Enqueue((row, col, clockwiseDir, score + 1000), score + 1000);
            pq.Enqueue((row, col, counterClockwiseDir, score + 1000), score + 1000);
        }
    }

    // Find the starting position based on a character (e.g., 'S' or 'E')
    static (int, int) FindStart(string[] maze, char target)
    {
        for (int r = 0; r < maze.Length; r++)
            for (int c = 0; c < maze[0].Length; c++)
                if (maze[r][c] == target) return (r, c);
        throw new Exception($"{target} not found in the maze.");
    }

    // Check if a position is within maze bounds
    static bool IsInBounds(int row, int col, int rows, int cols)
    {
        return row >= 0 && row < rows && col >= 0 && col < cols;
    }
}

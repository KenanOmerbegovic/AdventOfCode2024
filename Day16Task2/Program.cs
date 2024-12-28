using System;
using System.Collections.Generic;
using System.IO;

static class Day16Task2
{
    public static void Main()
    {
        var input = File.ReadAllLines("maze16.txt");
        int result = SolveMazeForPart2(input);
        Console.WriteLine($"Total Unique Tiles on Shortest Paths: {result}");
    }

    static int SolveMazeForPart2(string[] map)
    {
        int height = map.Length;
        int width = map[0].Length;

        // Find start position (S)
        int startX = 0, startY = 0;
        for (int y = 0; y < height; y++)
        {
            int x = map[y].IndexOf('S');
            if (x != -1)
            {
                startX = x;
                startY = y;
                break;
            }
        }

        var visited = new Dictionary<int, int>(); // (cell + direction) -> score
        var uniquePaths = new HashSet<int>();     // To track unique tiles in paths
        var trailheads = new Queue<(int x, int y, Direction d, int score, List<int> path)>();

        // Start facing East
        visited[CellDirectionIndex(startX, startY, Direction.E, height)] = 0;
        trailheads.Enqueue((startX, startY, Direction.E, 0, new List<int> { CellIndex(startX, startY, height) }));

        int lowestScore = int.MaxValue;
        var lowestScorePaths = new List<List<int>>();

        while (trailheads.Count > 0)
        {
            var trail = trailheads.Dequeue();
            var (dxcc, dycc, dx, dy, dxc, dyc) = trail.d switch
            {
                Direction.E => (0, -1, +1, 0, 0, +1),
                Direction.S => (+1, 0, 0, +1, -1, 0),
                Direction.W => (0, +1, -1, 0, 0, -1),
                Direction.N or _ => (-1, 0, 0, -1, +1, 0)
            };
            Extend(trail.x + dxcc, trail.y + dycc, RotateCC90(trail.d), trail.score + 1001);
            Extend(trail.x + dx, trail.y + dy, trail.d, trail.score + 1);
            Extend(trail.x + dxc, trail.y + dyc, RotateC90(trail.d), trail.score + 1001);

            void Extend(int x, int y, Direction d, int score)
            {
                if (y < 0 || y >= height || x < 0 || x >= width || map[y][x] == '#') return;

                if (map[y][x] == 'E')
                {
                    if (score <= lowestScore)
                    {
                        if (score < lowestScore) lowestScorePaths.Clear();
                        lowestScore = score;
                        lowestScorePaths.Add(new List<int>(trail.path));
                    }
                    return;
                }

                int cellDirectionKey = CellDirectionIndex(x, y, d, height);
                if (visited.TryGetValue(cellDirectionKey, out var visitedScore) && visitedScore < score) return;

                visited[cellDirectionKey] = score;
                var newPath = new List<int>(trail.path) { CellIndex(x, y, height) };
                trailheads.Enqueue((x, y, d, score, newPath));
            }
        }

        foreach (var path in lowestScorePaths)
        {
            foreach (var cell in path)
            {
                uniquePaths.Add(cell);
            }
        }

        return uniquePaths.Count + 1; // +1 for the 'E' position

        int CellIndex(int x, int y, int h) => y * h + x;
        int CellDirectionIndex(int x, int y, Direction d, int h) => y * 4 + x * h * 4 + (int)d;
    }

    static Direction RotateC90(Direction d) => (Direction)(((int)d + 1) % 4);
    static Direction RotateCC90(Direction d) => (Direction)((((int)d - 1) + 4) % 4);

    enum Direction { N, E, S, W }
}

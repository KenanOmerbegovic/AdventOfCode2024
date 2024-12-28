using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Hardcoded file path
        string filePath = "Day9Text.txt"; // Update this to your actual file path

        // Read the disk map from the file
        string diskMap;
        try
        {
            diskMap = File.ReadAllText(filePath).Trim();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading the file: " + ex.Message);
            return;
        }

        // Validate that the input is numeric
        if (string.IsNullOrEmpty(diskMap) || !IsNumeric(diskMap))
        {
            Console.WriteLine("Error: Disk map must be a non-empty string of digits.");
            return;
        }

        // Parse disk map into file lengths and free space lengths
        List<int> fileLengths = new List<int>();
        List<int> freeSpaceLengths = new List<int>();

        for (int i = 0; i < diskMap.Length; i += 2)
        {
            fileLengths.Add(diskMap[i] - '0'); // File length
            if (i + 1 < diskMap.Length)
            {
                freeSpaceLengths.Add(diskMap[i + 1] - '0'); // Free space length
            }
            else
            {
                freeSpaceLengths.Add(0); // Default to 0 free space if no paired digit
            }
        }

        // Build the initial disk layout
        List<char> diskLayout = new List<char>();
        int fileId = 0;
        for (int i = 0; i < fileLengths.Count; i++)
        {
            for (int j = 0; j < fileLengths[i]; j++)
                diskLayout.Add((char)('0' + fileId)); // Add file blocks
            for (int j = 0; j < freeSpaceLengths[i]; j++)
                diskLayout.Add('.'); // Add free space blocks
            fileId++;
        }

        Console.WriteLine("Initial Disk Layout: " + string.Join("", diskLayout));

        // Simulate compaction (whole files)
        for (int currentFileId = fileLengths.Count - 1; currentFileId >= 0; currentFileId--)
        {
            // Find the file's start and end positions
            int fileStart = diskLayout.IndexOf((char)('0' + currentFileId));
            if (fileStart == -1) continue; // File not found

            int fileEnd = fileStart;
            while (fileEnd < diskLayout.Count && diskLayout[fileEnd] == (char)('0' + currentFileId))
                fileEnd++;

            int fileLength = fileEnd - fileStart;

            // Find the leftmost span of free space large enough to fit the file
            int freeSpaceStart = -1;
            int freeSpaceLength = 0;

            for (int i = 0; i < diskLayout.Count; i++)
            {
                if (diskLayout[i] == '.')
                {
                    if (freeSpaceStart == -1) freeSpaceStart = i;
                    freeSpaceLength++;

                    if (freeSpaceLength >= fileLength)
                        break;
                }
                else
                {
                    freeSpaceStart = -1;
                    freeSpaceLength = 0;
                }
            }

            // Move the file if a suitable free space span is found
            if (freeSpaceLength >= fileLength && freeSpaceStart != -1 && freeSpaceStart < fileStart)
            {
                for (int i = 0; i < fileLength; i++)
                {
                    diskLayout[freeSpaceStart + i] = (char)('0' + currentFileId);
                    diskLayout[fileStart + i] = '.';
                }
            }
        }

        Console.WriteLine("Compacted Disk Layout: " + string.Join("", diskLayout));

        // Calculate the checksum
        long checksum = 0; // Use long to prevent overflow
        for (int i = 0; i < diskLayout.Count; i++)
        {
            if (diskLayout[i] != '.')
            {
                int fileIdAtPosition = diskLayout[i] - '0';
                checksum += (long)i * fileIdAtPosition; // Explicitly cast to long
            }
        }

        Console.WriteLine("Filesystem Checksum: " + checksum);
    }

    // Helper function to check if a string is numeric
    static bool IsNumeric(string str)
    {
        foreach (char c in str)
        {
            if (!char.IsDigit(c)) return false;
        }
        return true;
    }
}

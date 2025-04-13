﻿namespace G0tLib.Common;
public static class Utils
{
    public static Dictionary<string, string> ReadIndex()
    {
        if (File.Exists(".minigit/index"))
        {
            var content = File.ReadAllLines(".minigit/index");
            return content.ToDictionary(line => line.Split(' ')[1], line => line.Split(' ')[0]);
        }
        return new Dictionary<string, string>();
    }

    public static void WriteIndex(Dictionary<string, string> index)
    {
        var lines = index.Select(entry => $"{entry.Value} {entry.Key}").ToArray();
        File.WriteAllLines(".minigit/index", lines);
    }
}

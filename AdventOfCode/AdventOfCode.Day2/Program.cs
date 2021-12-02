using System.Text.RegularExpressions;

var regex = new Regex(@"^(forward|down|up)\s+(\d+)$");

var lines = File.ReadAllLines("input.txt");

var horizontalPosition = 0;
var oldDepth = 0;
var depth = 0;
var aim = 0;

foreach (var line in lines)
{
    var m = regex.Match(line);
    var val = int.Parse(m.Groups[2].Value);

    switch (m.Groups[1].Value.ToLowerInvariant())
    {
        case "forward":
            horizontalPosition += val;
            depth += aim * val;
            break;

        case "up":
            oldDepth -= val;
            aim -= val;
            break;

        case "down":
            oldDepth += val;
            aim += val;
            break;
    }
}

Console.WriteLine($"Task1: {horizontalPosition} depth: {oldDepth} result: {horizontalPosition * oldDepth}");
Console.WriteLine($"Task2: {horizontalPosition} depth: {depth} result: {horizontalPosition * depth}");

using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var dotRegex = new Regex(@"^(\d+),(\d+)$");
var foldRegex = new Regex(@"^fold along (x|y)=(\d+)$");

var dotCoords = new List<(int X, int Y)>();
var folds = new List<(string Axis, int N)>();

var xCount = 0;
var yCount = 0;

foreach (var line in lines)
{
    var m = dotRegex.Match(line);
    if (m.Success)
    {
        var xVal = int.Parse(m.Groups[1].Value);
        var yVal = int.Parse(m.Groups[2].Value);

        if (xVal >= xCount)
            xCount = xVal + 1;

        if (yVal >= yCount)
            yCount = yVal + 1;

        dotCoords.Add((xVal, yVal));
        continue;
    }

    m = foldRegex.Match(line);
    if (m.Success)
    {
        folds.Add((m.Groups[1].Value, int.Parse(m.Groups[2].Value)));
    }
}

var dots = new bool[xCount, yCount];

foreach (var (x, y) in dotCoords)
    dots[x, y] = true;

static void Fold(bool[,] dots, string axis, int line, ref int xCount, ref int yCount)
{
    var isXFold = axis == "x";

    var fromX = 0;
    var fromY = 0;

    if (isXFold)
        fromX = line + 1;
    else
        fromY = line + 1;

    for (var y = fromY; y < yCount; ++y)
    {
        for (var x = fromX; x < xCount; ++x)
        {
            if (!dots[x, y])
                continue;
            
            var newY = isXFold ? y : 2 * line - y;
            var newX = isXFold ? 2 * line - x : x;

            dots[newX, newY] = true;
        }
    }

    if (isXFold)
        xCount = line;
    else
        yCount = line;
    
}

Fold(dots, folds[0].Axis, folds[0].N, ref xCount, ref yCount);

var count = 0;

for (var y = 0; y < yCount; ++y)
    for (var x = 0; x < xCount; ++x)
        if (dots[x, y])
            ++count;

Console.WriteLine($"Task1: {count}");

for (var i = 1; i < folds.Count; ++i)
    Fold(dots, folds[i].Axis, folds[i].N, ref xCount, ref yCount);

Console.WriteLine($"Task2: ");

for (var y = 0; y < yCount; ++y)
{
    for (var x = 0; x < xCount; ++x)
        Console.Write(dots[x,y] ? "#" : " ");

    Console.WriteLine();
}

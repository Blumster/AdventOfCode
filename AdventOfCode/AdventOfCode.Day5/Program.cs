using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var lineRegex = new Regex(@"^(\d+),(\d+)\s+->\s+(\d+),(\d+)$", RegexOptions.Compiled);

var ventLines = lines.Select(l =>
{
    var m = lineRegex.Match(l);

    return new VentLine
    {
        X1 = int.Parse(m.Groups[1].Value),
        Y1 = int.Parse(m.Groups[2].Value),
        X2 = int.Parse(m.Groups[3].Value),
        Y2 = int.Parse(m.Groups[4].Value)
    };
}).ToList();

var coverage = new Dictionary<(int X, int Y), int>();

void IterateVentLine(VentLine line)
{
    var xIncrement = 1;
    var yIncrement = 1;

    if (line.X1 == line.X2)
        xIncrement = 0;
    else if (line.X2 < line.X1)
        xIncrement = -1;

    if (line.Y1 == line.Y2)
        yIncrement = 0;
    else if (line.Y2 < line.Y1)
        yIncrement = -1;

    var xItr = line.X1;
    var yItr = line.Y1;

    while (true)
    {
        var key = (xItr, yItr);

        if (coverage.ContainsKey(key))
            ++coverage[key];
        else
            coverage[key] = 1;

        if (xItr == line.X2 && yItr == line.Y2)
            break;

        xItr += xIncrement;
        yItr += yIncrement;
    }
}

foreach (var ventLine in ventLines)
{
    if (!ventLine.IsHorizontal() && !ventLine.IsVertical())
        continue;

    IterateVentLine(ventLine);
}

Console.WriteLine($"Task1: overlap count: {coverage.Count(p => p.Value > 1)}");

coverage.Clear();

foreach (var ventLine in ventLines)
{
    IterateVentLine(ventLine);
}

Console.WriteLine($"Task2: overlap count: {coverage.Count(p => p.Value > 1)}");

class VentLine
{
    public int X1 { get; set;}
    public int X2 { get; set; }
    public int Y1 { get; set; }
    public int Y2 { get; set; }

    public bool IsHorizontal()
    {
        return X1 == X2;
    }

    public bool IsVertical()
    {
        return Y1 == Y2;
    }

    public override string ToString()
    {
        return $"{X1},{Y1} -> {X2},{Y2}";
    }
}

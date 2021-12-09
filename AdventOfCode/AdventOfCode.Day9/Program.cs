var lines = File.ReadAllLines("input.txt");

var heights = new int[lines.Length][];

for (int i = 0; i < lines.Length; i++)
{
    heights[i] = lines[i].Select(c => int.Parse(c.ToString())).ToArray();
}

var lowpoints = new List<(int X, int Y)>();

for (var i = 0; i < heights.Length; ++i)
{
    for (var j = 0; j < heights[i].Length; ++j)
    {
        var valid = true;

        if (i > 0)
        {
            valid &= heights[i][j] < heights[i - 1][j];
        }

        if (i < heights.Length - 1)
        {
            valid &= heights[i][j] < heights[i + 1][j];
        }

        if (j > 0)
        {
            valid &= heights[i][j] < heights[i][j - 1];
        }

        if (j < heights[i].Length - 1)
        {
            valid &= heights[i][j] < heights[i][j + 1];
        }

        if (valid)
        {
            lowpoints.Add((i, j));
        }
    }
}

var sum = 0;

foreach (var (X, Y) in lowpoints)
{
    sum += heights[X][Y] + 1;
}

Console.WriteLine($"Task1: sum: {sum}");

int Visit(int x, int y, HashSet<(int X, int Y)> visited, int[][] heights)
{
    if (heights[x][y] == 9)
        return 0;

    if (visited.Contains((x, y)))
        return 0;

    visited.Add((x, y));

    var val = 1;

    if (x > 0)
        val += Visit(x - 1, y, visited, heights);

    if (x < heights.Length - 1)
        val += Visit(x + 1, y, visited, heights);

    if (y > 0)
        val += Visit(x, y - 1, visited, heights);

    if (y < heights[x].Length - 1)
        val += Visit(x, y + 1, visited, heights);

    return val;
}

var basins = new List<int>(lowpoints.Count);

foreach (var (X, Y) in lowpoints)
{
    var visited = new HashSet<(int X, int Y)>();
    basins.Add(Visit(X, Y, visited, heights));
}

basins.Sort();

Console.WriteLine($"Task2: result: {basins[^1] * basins[^2] * basins[^3]}");

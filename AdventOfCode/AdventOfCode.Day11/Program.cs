var lines = File.ReadAllLines("input.txt");

var octopuses = new int[lines.Length][];

for (int i = 0; i < lines.Length; i++)
{
    octopuses[i] = lines[i].Select(c => int.Parse(c.ToString())).ToArray();
}

int Flash(int x, int y, HashSet<(int X, int Y)> flashedOctopuses, int[][] octopuses)
{
    if (octopuses[x][y] <= 9 || flashedOctopuses.Contains((x, y)))
    {
        return 0;
    }

    flashedOctopuses.Add((x, y));

    var val = 1;

    for (var i = x - 1; i <= x + 1; ++i)
    {
        if (i < 0 || i >= octopuses.Length)
            continue;

        for (var j = y - 1; j <= y + 1; ++j)
        {
            if (j < 0 || j >= octopuses[i].Length)
                continue;

            ++octopuses[i][j];

            val += Flash(i, j, flashedOctopuses, octopuses);
        }
    }

    return val;
}

var flashCount = 0;

for (int i = 0; i < 100; ++i)
{
    for (var x = 0; x < octopuses.Length; ++x)
    {
        for (var y = 0; y < octopuses[x].Length; ++y)
        {
            ++octopuses[x][y];
        }
    }

    var flashedOctopuses = new HashSet<(int X, int Y)>();

    for (var x = 0; x < octopuses.Length; ++x)
    {
        for (var y = 0; y < octopuses[x].Length; ++y)
        {
            flashCount += Flash(x, y, flashedOctopuses, octopuses);
        }
    }

    foreach (var octopus in flashedOctopuses)
    {
        octopuses[octopus.X][octopus.Y] = 0;
    }
}

Console.WriteLine($"Task1: flash count: {flashCount}");

var j = 100;

while (true)
{
    flashCount = 0;

    for (var x = 0; x < octopuses.Length; ++x)
    {
        for (var y = 0; y < octopuses[x].Length; ++y)
        {
            ++octopuses[x][y];
        }
    }

    var flashedOctopuses = new HashSet<(int X, int Y)>();

    for (var x = 0; x < octopuses.Length; ++x)
    {
        for (var y = 0; y < octopuses[x].Length; ++y)
        {
            flashCount += Flash(x, y, flashedOctopuses, octopuses);
        }
    }

    foreach (var octopus in flashedOctopuses)
    {
        octopuses[octopus.X][octopus.Y] = 0;
    }

    if (flashedOctopuses.Count == octopuses.Length * octopuses[0].Length)
        break;

    ++j;
}

Console.WriteLine($"Task2: first synchronized flash: {j + 1}");

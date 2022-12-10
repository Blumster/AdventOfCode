var lines = File.ReadAllLines("input.txt");

static int Simulate(int partCount, string[] commands)
{
    var parts = new List<Coord>(partCount);
    for (var i = 0; i < partCount; ++i)
        parts.Add(new Coord(0, 0));

    var visited = new HashSet<Coord>
    {
        parts[^1]
    };

    foreach (var cmd in commands)
    {
        var direction = cmd[0];
        var cmdCount = int.Parse(cmd[2..]);

        for (var i = 0; i < cmdCount; ++i)
        {
            parts[0] = parts[0].Step(direction);

            for (var p = 1; p < parts.Count; ++p)
            {
                var previous = parts[p - 1];
                var current = parts[p];
                
                if (previous.Adjescent(current))
                    continue;

                parts[p] = current.Follow(previous);
            }

            visited.Add(parts[^1]);
        }
    }

    return visited.Count;
}

// Part1
Console.WriteLine(Simulate(2, lines));

// Part2
Console.WriteLine(Simulate(10, lines));

record class Coord(int X, int Y)
{
    public (int xDiff, int yDiff) Distance(Coord other)
    {
        return (Math.Abs(X - other.X), Math.Abs(Y - other.Y));
    }

    public bool Adjescent(Coord other)
    {
        (var xDiff, var yDiff) = Distance(other);

        return xDiff <= 1 && yDiff <= 1;
    }

    public Coord Step(char direction)
    {
        return direction switch
        {
            'U' => new Coord(X - 1, Y),
            'R' => new Coord(X, Y + 1),
            'D' => new Coord(X + 1, Y),
            'L' => new Coord(X, Y - 1),
            _ => throw new InvalidOperationException(),
        };
    }

    public Coord Follow(Coord other)
    {
        var newX = X;
        var newY = Y;

        (var xDiff, var yDiff) = other.Distance(this);
        if (xDiff == 2 && yDiff == 2)
        {
            newX = (X + other.X) / 2;
            newY = (Y + other.Y) / 2;
        }
        else if (xDiff == 2)
        {
            if (yDiff == 1)
                newY = other.Y;

            newX = (X + other.X) / 2;
        }
        else if (yDiff == 2)
        {
            if (xDiff == 1)
                newX = other.X;

            newY = (Y + other.Y) / 2;
        }

        return new Coord(newX, newY);
    }
}
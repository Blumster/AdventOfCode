using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var field = new SensorField();

foreach (var line in lines)
{
    var parts = line.Split(':');

    var sensorCoord = Coord.Parse(parts[0][10..]);
    var closestBeaconCoord = Coord.Parse(parts[1][22..]);

    field.AddSensor(sensorCoord, closestBeaconCoord);
}

// Part1
Console.WriteLine(field.OccupiedSlotsOnRow(2_000_000));

// Part2
var hole = field.FindHole();

Console.WriteLine((long)hole.Y * 4_000_000 + hole.X);

class SensorField
{
    public Dictionary<Coord, int> Sensors { get; } = new();
    public HashSet<Coord> Beacons { get; } = new();
    public Coord TopLeft { get; private set; }
    public Coord BottomRight { get; private set; }

    public void AddSensor(Coord coord, Coord closestBeaconCoord)
    {
        Sensors.Add(coord, coord.Distance(closestBeaconCoord));
        Beacons.Add(closestBeaconCoord);

        var distance = coord.Distance(closestBeaconCoord);

        ExtendBoundaries(new Coord(coord.X + distance, coord.Y));
        ExtendBoundaries(new Coord(coord.X - distance, coord.Y));
        ExtendBoundaries(new Coord(coord.X, coord.Y + distance));
        ExtendBoundaries(new Coord(coord.X, coord.Y - distance));
    }

    public long OccupiedSlotsOnRow(int row)
    {
        var result = 0;

        for (var y = TopLeft.Y; y <= BottomRight.Y; ++y)
            if (IsInRangeOfASensor(row, y) && !Beacons.Contains(new Coord(row, y)))
                ++result;

        return result;
    }

    public bool IsInRangeOfASensor(Coord coord) => IsInRangeOfASensor(coord.X, coord.Y);

    public bool IsInRangeOfASensor(int x, int y)
    {
        foreach (var sensor in Sensors)
            if (Coord.Distance(sensor.Key.X, sensor.Key.Y, x, y) <= sensor.Value)
                return true;

        return false;
    }

    public Coord FindHole()
    {
        foreach (var sensor in Sensors)
            foreach (var possibleCoord in sensor.Key.CoordsAround(sensor.Value + 1))
                if (IsValidCoord(possibleCoord) && !IsInRangeOfASensor(possibleCoord))
                    return possibleCoord;

        throw new InvalidDataException();
    }

    private static bool IsValidCoord(Coord coord)
    {
        return coord.X >= 0 && coord.X <= 4_000_000 && coord.Y >= 0 && coord.Y <= 4_000_000;
    }

    private void ExtendBoundaries(Coord coord)
    {
        var top = Math.Min(coord.X, TopLeft.X);
        var left = Math.Min(coord.Y, TopLeft.Y);
        var bottom = Math.Max(coord.X, BottomRight.X);
        var right = Math.Max(coord.Y, BottomRight.Y);

        if (top != TopLeft.X || left != TopLeft.Y)
            TopLeft = new Coord(top, left);

        if (bottom != BottomRight.X || right != BottomRight.Y)
            BottomRight = new Coord(bottom, right);
    }
}

record struct Coord(int X, int Y)
{
    private static readonly Regex CoordRegex = new(@"^x=(-?\d+),\s+y=(-?\d+)$");

    public static Coord Parse(string input)
    {
        var m = CoordRegex.Match(input);
        if (!m.Success)
            throw new ArgumentException("Invalid coordinate string!", nameof(input));

        return new Coord(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[1].Value));
    }

    public int Distance(Coord other)
    {
        return Distance(X, Y, other.X, other.Y);
    }

    public static int Distance(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }

    public IEnumerable<Coord> CoordsAround(int distance)
    {
        for (var i = 0; i < distance; ++i)
        {
            yield return new Coord(X + distance - i, Y + i);
            yield return new Coord(X + distance - i, Y - i);
            yield return new Coord(X - distance + i, Y + i);
            yield return new Coord(X - distance + i, Y - i);
        }
    }
}

using System.Text;

var lines = File.ReadAllLines("input.txt");

var cave = new Cave();

foreach (var line in lines)
{
    var parts = line.Split(" -> ");
    var previous = Coord.Parse(parts[0]);

    for (var i = 1; i < parts.Length; ++i)
    {
        var current = Coord.Parse(parts[i]);

        cave.AddWall(previous, current);

        previous = current;
    }
}

// Part1
var part1Count = 0;
var prevResult = true;

while (true)
{
    var result = cave.DropSand();
    if (!result && !prevResult)
        break;

    prevResult = result;

    if (result)
        ++part1Count;
}

Console.WriteLine(part1Count);

// Part 2
cave.AddBottomPlane(cave.BottomRight.X + 2);

var part2Count = part1Count;

while (true)
{
    var result = cave.DropSand();
    if (!result)
        break;

    ++part2Count;
}

Console.WriteLine(part2Count);

enum BlockType
{
    Air,
    Rock,
    Sand,
    SandSource
}

[Flags]
enum MoveDirection
{
    None      = 0,
    Down      = 1,
    DownLeft  = 2,
    DownRight = 4
}

class Cave
{
    private Dictionary<Coord, BlockType> Blocks { get; } = new();

    public Coord SandSource { get; } = new(0, 500);
    public Coord TopLeft { get; private set; } = new(int.MaxValue, int.MaxValue);
    public Coord BottomRight { get; private set; } = new(int.MinValue, int.MinValue);
    public int BottomPlane { get; private set; } = int.MaxValue;

    public Cave()
    {
        ExtendBoundaries(SandSource);
    }

    public void AddWall(Coord from, Coord to)
    {
        ExtendBoundaries(from);
        ExtendBoundaries(to);

        if (from.X == to.X)
        {
            for (var i = Math.Min(from.Y, to.Y); i <= Math.Max(from.Y, to.Y); ++i)
                Blocks[new Coord(from.X, i)] = BlockType.Rock;
        }
        else if (from.Y == to.Y)
        {
            for (var i = Math.Min(from.X, to.X); i <= Math.Max(from.X, to.X); ++i)
                Blocks[new Coord(i, from.Y)] = BlockType.Rock;
        }
        else
            throw new InvalidDataException();
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

    public void AddBottomPlane(int planeX)
    {
        BottomPlane = planeX;

        ExtendBoundaries(new Coord(BottomPlane, BottomRight.Y));
    }

    public string Draw()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < 3; ++i)
        {
            Console.Write("    ");

            for (var y = TopLeft.Y; y <= BottomRight.Y; ++y)
            {
                if (y == TopLeft.Y || y == SandSource.Y || y == BottomRight.Y)
                {
                    Console.Write(y / (int)Math.Pow(10, 2 - i) % 10);
                }
                else
                    Console.Write(' ');
            }

            Console.WriteLine();
        }

        for (var x = TopLeft.X; x <= BottomRight.X; ++x)
        {
            Console.Write($"{x,3} ");

            for (var y = TopLeft.Y; y <= BottomRight.Y; ++y)
            {
                Console.Write(GetBlockTypeAt(new Coord(x, y)) switch
                {
                    BlockType.Air => '.',
                    BlockType.Rock => '#',
                    BlockType.Sand => 'o',
                    BlockType.SandSource => '+',
                    _ => throw new InvalidDataException()
                });
            }

            Console.WriteLine();
        }

        return sb.ToString();
    }

    private BlockType GetBlockTypeAt(Coord coord)
    {
        if (Blocks.ContainsKey(coord))
            return Blocks[coord];

        if (coord == SandSource)
            return BlockType.SandSource;

        if (BottomPlane != int.MaxValue && coord.X == BottomPlane)
            return BlockType.Rock;

        return BlockType.Air;
    }

    private bool IsOutOfBounds(Coord coord)
    {
        if (BottomPlane != int.MaxValue)
            return coord.X >= BottomPlane;

        return coord.X < TopLeft.X || coord.X > BottomRight.X || coord.Y < TopLeft.Y || coord.Y > BottomRight.Y;
    }

    public bool DropSand()
    {
        var sandCoord = SandSource;

        if (GetBlockTypeAt(SandSource) == BlockType.Sand)
            return false;

        while (true)
        {
            // TODO: extend bounds, if there is a bottomplane and we are over it
            var directions = GetMoveDirections(sandCoord);
            if (directions == MoveDirection.None || IsOutOfBounds(sandCoord))
                break;

            sandCoord = MoveInDirection(sandCoord, directions);

            if (BottomPlane != int.MaxValue)
                ExtendBoundaries(sandCoord);
        }

        if (!IsOutOfBounds(sandCoord))
        {
            Blocks[sandCoord] = BlockType.Sand;

            return true;
        }

        return false;
    }

    public MoveDirection GetMoveDirections(Coord coord)
    {
        var directions = MoveDirection.None;

        if (GetBlockTypeAt(new Coord(coord.X + 1, coord.Y)) == BlockType.Air)
            directions |= MoveDirection.Down;

        if (GetBlockTypeAt(new Coord(coord.X + 1, coord.Y - 1)) == BlockType.Air)
            directions |= MoveDirection.DownLeft;

        if (GetBlockTypeAt(new Coord(coord.X + 1, coord.Y + 1)) == BlockType.Air)
            directions |= MoveDirection.DownRight;

        return directions;
    }

    public static Coord MoveInDirection(Coord coord, MoveDirection direction)
    {
        if ((direction & MoveDirection.Down) == MoveDirection.Down)
            return new Coord(coord.X + 1, coord.Y);

        if ((direction & MoveDirection.DownLeft) == MoveDirection.DownLeft)
            return new Coord(coord.X + 1, coord.Y - 1);

        if ((direction & MoveDirection.DownRight) == MoveDirection.DownRight)
            return new Coord(coord.X + 1, coord.Y + 1);

        throw new InvalidOperationException();
    }
}

record struct Coord(int X, int Y)
{
    public static Coord Parse(string input)
    {
        var parts = input.Split(',');

        return new Coord(int.Parse(parts[1]), int.Parse(parts[0]));
    }
}
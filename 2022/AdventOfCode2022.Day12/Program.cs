var lines = File.ReadAllLines("input.txt");

var width = lines[0].Length;
var height = lines.Length;

var map = new byte[height, width];
var part1Costs = new int[height, width];
var part2Costs = new int[height, width];

var (startX, startY) = (-1, -1);
var (endX, endY) = (-1, -1);

for (var x = 0; x < height; ++x)
{
    for (var y = 0; y < width; ++y)
    {
        part1Costs[x, y] = int.MaxValue;
        part2Costs[x, y] = int.MaxValue;

        switch (lines[x][y])
        {
            case 'S':
                (startX, startY) = (x, y);
                map[x, y] = 'a' - 'a';
                break;

            case 'E':
                (endX, endY) = (x, y);
                map[x, y] = 'z' - 'a';
                break;

            default:
                map[x, y] = (byte)(lines[x][y] - 'a');
                break;
        }
    }
}

// Part1
Calculate(startX, startY, map, part1Costs, false);

Console.WriteLine(part1Costs[endX, endY]);

// Part2
Calculate(endX, endY, map, part2Costs, true);

var min = int.MaxValue;

for (var x = 0; x < height; ++x)
    for (var y = 0; y < width; ++y)
        if (map[x, y] == 0 && min > part2Costs[x, y])
            min = part2Costs[x, y];

Console.WriteLine(min);

static void Calculate(int startX, int startY, byte[,] map, int[,] costs, bool reverse)
{
    var queue = new Queue<(int x, int y, int steps)>();
    queue.Enqueue((startX, startY, 0));

    while (queue.Count > 0)
    {
        var (x, y, steps) = queue.Dequeue();
        if (steps >= costs[x, y])
            continue;

        costs[x, y] = steps;

        var height = map[x, y];

        var lowestPossible = reverse ? height - 1 : 0;
        var highestPossible = reverse ? int.MaxValue : height + 1;

        if (x > 0 && lowestPossible <= map[x - 1, y] && map[x - 1, y] <= highestPossible)
            queue.Enqueue((x - 1, y, steps + 1));

        if (y < map.GetLength(1) - 1 && lowestPossible <= map[x, y + 1] && map[x, y + 1] <= highestPossible)
            queue.Enqueue((x, y + 1, steps + 1));

        if (x < map.GetLength(0) - 1 && lowestPossible <= map[x + 1, y] && map[x + 1, y] <= highestPossible)
            queue.Enqueue((x + 1, y, steps + 1));

        if (y > 0 && lowestPossible <= map[x, y - 1] && map[x, y - 1] <= highestPossible)
            queue.Enqueue((x, y - 1, steps + 1));
    }
}

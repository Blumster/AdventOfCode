var lines = File.ReadAllLines("input.txt");

var width = lines[0].Length;
var height = lines.Length;

var trees = new int[width, height];
var visibilityCache = new VisibilityCacheEntry[width, height];

for (var x = 0; x < height; ++x)
{
    for (var y = 0; y < width; ++y)
    {
        trees[x, y] = lines[x][y] - '0';

        visibilityCache[x, y] = new VisibilityCacheEntry
        {
            IsVisible = x == 0 || x == height - 1 || y == 0 || y == width - 1
        };
    }
}

for (var x = 1; x < height - 1; ++x)
    for (var y = 1; y < width - 1; ++y)
        CalculateVisibility(x, y);

void CalculateVisibility(int x, int y)
{
    var height = trees[x, y];

    var isVisible = false;
    var scenicScore = 1;

    // Top
    {
        var allShorter = true;
        var treeCounter = 0;

        for (var xi = x - 1; xi >= 0; --xi)
        {
            ++treeCounter;

            if (trees[xi, y] >= height)
            {
                allShorter = false;
                break;
            }
        }

        scenicScore *= treeCounter;
        isVisible |= allShorter;
    }

    // Right
    {
        var allShorter = true;
        var treeCounter = 0;

        for (var yi = y + 1; yi < trees.GetLength(1); ++yi)
        {
            ++treeCounter;

            if (trees[x, yi] >= height)
            {
                allShorter = false;
                break;
            }
        }

        scenicScore *= treeCounter;
        isVisible |= allShorter;
    }

    // Bottom
    {
        var allShorter = true;
        var treeCounter = 0;

        for (var xi = x + 1; xi < trees.GetLength(0); ++xi)
        {
            ++treeCounter;

            if (trees[xi, y] >= height)
            {
                allShorter = false;
                break;
            }
        }

        scenicScore *= treeCounter;
        isVisible |= allShorter; 
    }

    // Left
    {
        var allShorter = true;
        var treeCounter = 0;

        for (var yi = y - 1; yi >= 0; --yi)
        {
            ++treeCounter;

            if (trees[x, yi] >= height)
            {
                allShorter = false;
                break;
            }
        }

        scenicScore *= treeCounter;
        isVisible |= allShorter;
    }

    visibilityCache[x, y].IsVisible = isVisible;
    visibilityCache[x, y].ScenicScore = scenicScore;
}

// Part1
var part1Count = 0;

for (var x = 0; x < height; ++x)
    for (var y = 0; y < width; ++y)
        if (visibilityCache[x, y].IsVisible)
            ++part1Count;

Console.WriteLine(part1Count);

// Part2
var max = 0;

for (var x = 0; x < height; ++x)
    for (var y = 0; y < width; ++y)
        if (visibilityCache[x, y].ScenicScore > max)
            max = visibilityCache[x, y].ScenicScore;

Console.WriteLine(max);

class VisibilityCacheEntry
{
    public bool IsVisible { get; set; } = false;
    public int ScenicScore { get; set; } = 1;
}

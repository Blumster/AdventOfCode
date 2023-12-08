using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var r = new Regex(@"(\d+)", RegexOptions.Compiled);

var part1Parts = new List<int>();
var part2Gears = new Dictionary<(int Row, int Col), List<int>>();

for (var i = 0; i < lines.Length; ++i)
{
    var off = 0;
    do
    {
        var m = r.Match(lines[i], off);
        if (!m.Success)
            break;

        if (TouchesSymbol(i, m.Index, m.Length))
            part1Parts.Add(int.Parse(m.Groups[1].Value));

        foreach (var gear in TouchesGear(i, m.Index, m.Length))
        {
            if (!part2Gears.ContainsKey(gear))
                part2Gears.Add(gear, new());

            part2Gears[gear].Add(int.Parse(m.Groups[1].Value));
        }

        off = m.Index + m.Length;
    }
    while (true);
}

// Part1
Console.WriteLine(part1Parts.Sum());

// Part2
Console.WriteLine(part2Gears.Where(g => g.Value.Count == 2).Select(g => g.Value[0] * g.Value[1]).Sum());

bool TouchesSymbol(int line, int start, int length)
{
    for (var l = Math.Max(0, line - 1); l <= Math.Min(line + 1, lines.Length - 1); ++l)
    {
        for (var c = Math.Max(0, start - 1); c <= Math.Min(start + length, lines[l].Length - 1); ++c)
        {
            var charAtPos = lines[l][c];

            if (!char.IsDigit(charAtPos) && charAtPos != '.')
                return true;
        }
    }

    return false;
}

IEnumerable<(int Row, int Column)> TouchesGear(int line, int start, int length)
{
    for (var l = Math.Max(0, line - 1); l <= Math.Min(line + 1, lines!.Length - 1); ++l)
        for (var c = Math.Max(0, start - 1); c <= Math.Min(start + length, lines[l].Length - 1); ++c)
            if (lines[l][c] == '*')
                yield return (l, c);

    yield break;
}

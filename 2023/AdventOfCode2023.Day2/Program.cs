using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

var gameIdRegex = new Regex(@"^Game (\d+):(.*)$", RegexOptions.Compiled);
var redRegex = new Regex(@"(\d+) red", RegexOptions.Compiled);
var greenRegex = new Regex(@"(\d+) green", RegexOptions.Compiled);
var blueRegex = new Regex(@"(\d+) blue", RegexOptions.Compiled);

var games = new Dictionary<int, List<(int Red, int Green, int Blue)>>();

var limits = (12, 13, 14);

foreach (var game in input)
{
    var m = gameIdRegex.Match(game);
    var id = int.Parse(m.Groups[1].Value);
    var drawStr = m.Groups[2].Value;

    games.Add(id, new());

    var draws = drawStr.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    foreach (var draw in draws)
    {
        var rm = redRegex.Match(draw);
        var gm = greenRegex.Match(draw);
        var bm = blueRegex.Match(draw);

        games[id].Add(
            (
            rm.Success ? int.Parse(rm.Groups[1].Value) : 0,
            gm.Success ? int.Parse(gm.Groups[1].Value) : 0,
            bm.Success ? int.Parse(bm.Groups[1].Value) : 0
            )
        );
    }
}

var part1Result = games.Where(g => IsPossible(g.Value, limits)).Select(g => g.Key).Sum();

// Part 1
Console.WriteLine(part1Result);

var part2Result = games.Select(g => CalcMinimumLimitPower(g.Value)).Sum();

// Part 2
Console.WriteLine(part2Result);

bool IsPossible(IEnumerable<(int Red, int Green, int Blue)> draws, (int Red, int Green, int Blue) limit)
{
    return !draws.Any(draw => draw.Red > limit.Red || draw.Green > limit.Green || draw.Blue > limit.Blue);
}

long CalcMinimumLimitPower(IEnumerable<(int Red,  int Green, int Blue)> draws)
{
    var maxRed = draws.Select(d => d.Red).Max();
    var maxGreen = draws.Select(d => d.Green).Max();
    var maxBlue = draws.Select(d => d.Blue).Max();

    return maxRed * maxGreen * maxBlue;
}
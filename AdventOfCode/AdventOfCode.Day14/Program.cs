var lines = File.ReadAllLines("input.txt");

var polymer = lines[0];

var rules = new Dictionary<string, char>();
var ruleCounts = new Dictionary<string, long>();
var elementCounts = new Dictionary<char, long>();

for (var i = 1; i < lines.Length; ++i)
{
    if (string.IsNullOrEmpty(lines[i]))
        continue;

    var parts = lines[i].Split(" -> ", 2);

    rules.Add(parts[0], parts[1][0]);
    ruleCounts.Add(parts[0], 0);

    elementCounts[parts[0][0]] = 0;
    elementCounts[parts[0][1]] = 0;
    elementCounts[parts[1][0]] = 0;
}

for (var i = 0; i < polymer.Length - 1; ++i)
{
    ++ruleCounts[$"{polymer[i]}{polymer[i + 1]}"];

    ++elementCounts[polymer[i]];

    if (i == polymer.Length - 2)
        ++elementCounts[polymer[i + 1]];
}

for (var i = 0; i < 10; ++i)
{
    foreach (var rule in ruleCounts)
        elementCounts[rules[rule.Key]] += rule.Value;

    ruleCounts = CalculateRuleCounts(ruleCounts, rules);
}

Console.WriteLine($"Task1: result {elementCounts.Values.Max() - elementCounts.Values.Min()}");

for (var i = 0; i < 30; ++i)
{
    foreach (var rule in ruleCounts)
        elementCounts[rules[rule.Key]] += rule.Value;

    ruleCounts = CalculateRuleCounts(ruleCounts, rules);
}

static Dictionary<string, long> CalculateRuleCounts(Dictionary<string, long> counts, Dictionary<string, char> rules)
{
    var newCounts = new Dictionary<string, long>();

    foreach (var rule in rules)
        newCounts.Add(rule.Key, 0);

    foreach (var rule in counts)
    {
        var newElement = rules[rule.Key];
        var newRule1 = $"{rule.Key[0]}{newElement}";
        var newRule2 = $"{newElement}{rule.Key[1]}";

        newCounts[newRule1] += rule.Value;
        newCounts[newRule2] += rule.Value;
    }

    return newCounts;
}

Console.WriteLine($"Task2: result {elementCounts.Values.Max() - elementCounts.Values.Min()}");

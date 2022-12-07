using System.Text.RegularExpressions;

var cmdRegex = new Regex(@"^move (\d+) from (\d+) to (\d+)$", RegexOptions.Compiled);

var lines = File.ReadAllLines("input.txt");

var colCount = (lines[0].Length + 1) / 4;

var loadStacks = new List<Stack<string>>(colCount);
var part1Stacks = new List<Stack<string>>(colCount);
var part2Stacks = new List<Stack<string>>(colCount);

for (var k = 0; k < colCount; ++k)
    loadStacks.Add(new());

var startLine = 0;

for (var i = 0; i < lines.Length; ++i)
{
    if (string.IsNullOrEmpty(lines[i]))
    {
        startLine = i + 1;
        break;
    }

    if (!lines[i].Contains('['))
        continue;

    for (var c = 0; c < colCount; ++c)
    {
        var name = lines[i][(c * 4 + 1)..(c * 4 + 2)];
        if (!string.IsNullOrWhiteSpace(name))
            loadStacks[c].Push(name);
    }
}

foreach (var stack in loadStacks)
{
    part1Stacks.Add(new(stack));
    part2Stacks.Add(new(stack));
}

for (var i = startLine; i < lines.Length; ++i)
{
    var m = cmdRegex.Match(lines[i]);
    if (!m.Success)
        throw new InvalidDataException();

    var amount = int.Parse(m.Groups[1].Value);
    var from = int.Parse(m.Groups[2].Value) - 1;
    var to = int.Parse(m.Groups[3].Value) - 1;

    MoveBetweenStacks(part1Stacks, amount, from, to, true);
    MoveBetweenStacks(part2Stacks, amount, from, to, false);
}

// Part1
foreach (var stack in part1Stacks)
    Console.Write(stack.Peek());

Console.WriteLine();

// Part2
foreach (var stack in part2Stacks)
    Console.Write(stack.Peek());

Console.WriteLine();

static void MoveBetweenStacks(List<Stack<string>> stacks, int amount, int from, int to, bool oneByOne)
{
    if (oneByOne)
    {
        for (var c = 0; c < amount; ++c)
            stacks[to].Push(stacks[from].Pop());
    }
    else
    {
        var tempStack = new Stack<string>();

        for (var c = 0; c < amount; ++c)
            tempStack.Push(stacks[from].Pop());

        for (var c = 0; c < amount; ++c)
            stacks[to].Push(tempStack.Pop());
    }
}

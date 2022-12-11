using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var part1Monkies = new Dictionary<int, Monkey>();
var part2Monkies = new Dictionary<int, Monkey>();

for (var i = 0; i < lines.Length;)
{
    if (string.IsNullOrWhiteSpace(lines[i]))
    {
        ++i;
        continue;
    }

    if (Monkey.IdInputRegex.IsMatch(lines[i]))
    {
        var monkey = new Monkey();
        monkey.SetId(lines[i]);
        monkey.SetStartingItems(lines[i + 1]);
        monkey.SetOperation(lines[i + 2]);
        monkey.SetTest(lines[i + 3], lines[i + 4], lines[i + 5]);

        part1Monkies.Add(monkey.Id, monkey);

        monkey = new Monkey();
        monkey.SetId(lines[i]);
        monkey.SetStartingItems(lines[i + 1]);
        monkey.SetOperation(lines[i + 2]);
        monkey.SetTest(lines[i + 3], lines[i + 4], lines[i + 5]);

        Monkey.Part2Modulo *= monkey.TestDivisor;

        part2Monkies.Add(monkey.Id, monkey);

        i += 6;
    }
}

// Part1
for (var round = 1; round <= 20; ++round)
{
    for (var m = 0; m < part1Monkies.Count; ++m)
    {
        var monkey = part1Monkies[m];

        while (monkey.HasItems())
        {
            var newValue = monkey.Inspect(true);

            var nextMonkey = monkey.Throw(newValue);

            part1Monkies[nextMonkey].Items!.Enqueue(newValue);
        }
    }
}

var inspection1Counts = part1Monkies.Select(m => m.Value.InspectionCount).OrderBy(i => i).TakeLast(2).ToList();

Console.WriteLine(inspection1Counts[0] * inspection1Counts[1]);

// Part2
for (var round = 1; round <= 10_000; ++round)
{
    for (var m = 0; m < part2Monkies.Count; ++m)
    {
        var monkey = part2Monkies[m];

        while (monkey.HasItems())
        {
            var newValue = monkey.Inspect(false);

            var nextMonkey = monkey.Throw(newValue);

            part2Monkies[nextMonkey].Items!.Enqueue(newValue);
        }
    }
}

var inspection2Counts = part2Monkies.Select(m => (long)m.Value.InspectionCount).OrderBy(i => i).TakeLast(2).ToList();

Console.WriteLine(inspection2Counts[0] * inspection2Counts[1]);

class Operation
{
    private static readonly Regex InputRegex = new(@"^\s*Operation:\snew\s=\sold\s([+*])\s(old|\d+)\s*$", RegexOptions.Compiled);

    public enum Method
    {
        Add,
        Multiply
    }

    public enum TargetType
    {
        Int,
        Old
    }

    public Method Oper { get; set; }
    public TargetType Target { get; set; }
    public ulong TargetValue { get; set; }

    private Operation(Method oper, TargetType target, ulong targetValue)
    {
        Oper = oper;
        Target = target;
        TargetValue = targetValue;
    }

    public ulong Run(ulong old)
    {
        var targetValue = Target switch
        {
            TargetType.Int => TargetValue,
            TargetType.Old => old,
            _ => throw new InvalidDataException()
        };

        return Oper switch
        {
            Method.Add => old + targetValue,
            Method.Multiply => old * targetValue,
            _ => throw new InvalidOperationException()
        };
    }

    public static Operation Parse(string input)
    {
        var m = InputRegex.Match(input);
        if (!m.Success)
            throw new ArgumentException("Invalid input data for Operation!", nameof(input));

        var oper = m.Groups[1].Value switch
        {
            "+" => Method.Add,
            "*" => Method.Multiply,
            _ => throw new InvalidDataException()
        };

        var target = m.Groups[2].Value == "old" ? TargetType.Old : TargetType.Int;
        var targetValue = target == TargetType.Int ? ulong.Parse(m.Groups[2].Value) : 0;

        return new Operation(oper, target, targetValue);
    }
}

class Monkey
{
    public static ulong Part2Modulo = 1;

    public static readonly Regex IdInputRegex = new(@"^\s*Monkey\s(\d+):\s*$", RegexOptions.Compiled);

    private static readonly Regex ItemsInputRegex = new(@"^\s*Starting\sitems:\s((?:\d+)(?:\s*,\s*\d+)*)\s*$", RegexOptions.Compiled);
    private static readonly Regex TestInputRegex = new(@"^\s*Test:\sdivisible\sby\s(\d+)\s*$", RegexOptions.Compiled);
    private static readonly Regex TrueCaseInputRegex = new(@"^\s*If\strue:\sthrow\sto\smonkey\s(\d+)\s*$", RegexOptions.Compiled);
    private static readonly Regex FlaseCaseInputRegex = new(@"^\s*If\sfalse:\sthrow\sto\smonkey\s(\d+)\s*$", RegexOptions.Compiled);

    public int Id { get; private set; }
    [NotNull]
    public Queue<ulong>? Items { get; private set; }
    [NotNull]
    public Operation? Operation { get; private set; }
    public ulong TestDivisor { get; private set; }
    public int TrueCaseDestination { get; private set; }
    public int FalseCaseDestination { get; private set; }
    public int InspectionCount { get; private set; }

    public void SetId(string input)
    {
        var m = IdInputRegex.Match(input);
        if (!m.Success)
            throw new ArgumentException("Invalid input data for Starting items!", nameof(input));

        Id = int.Parse(m.Groups[1].Value);
    }

    public void SetStartingItems(string input)
    {
        var m = ItemsInputRegex.Match(input);
        if (!m.Success)
            throw new ArgumentException("Invalid input data for Starting items!", nameof(input));

        Items = new(m.Groups[1].Value.Split(',', StringSplitOptions.TrimEntries).Select(ulong.Parse));
    }

    public void SetOperation(string input)
    {
        Operation = Operation.Parse(input);
    }

    public void SetTest(string testInput, string trueInput, string falseInput)
    {
        var m = TestInputRegex.Match(testInput);
        if (!m.Success)
            throw new ArgumentException("Invalid input data for Test!", nameof(testInput));

        TestDivisor = ulong.Parse(m.Groups[1].Value);

        var trueCaseMatch = TrueCaseInputRegex.Match(trueInput);
        if (!m.Success)
            throw new ArgumentException("Invalid input data for True case!", nameof(trueInput));

        TrueCaseDestination = int.Parse(trueCaseMatch.Groups[1].Value);

        var falseCaseMatch = FlaseCaseInputRegex.Match(falseInput);
        if (!m.Success)
            throw new ArgumentException("Invalid input data for False case!", nameof(falseInput));

        FalseCaseDestination = int.Parse(falseCaseMatch.Groups[1].Value);
    }

    public bool HasItems()
    {
        return Items.Count > 0;
    }

    public ulong Inspect(bool noWorries)
    {
        ++InspectionCount;

        var newValue = Operation.Run(Items.Dequeue());

        return noWorries ? (newValue / 3) : (newValue % Part2Modulo);
    }

    public int Throw(ulong value)
    {
        return (value % TestDivisor) == 0 ? TrueCaseDestination : FalseCaseDestination;
    }
}

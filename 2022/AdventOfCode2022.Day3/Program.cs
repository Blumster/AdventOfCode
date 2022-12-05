var lines = File.ReadAllLines("input.txt");

var rucksacks = lines.Select(l => new Rucksack(new Compartment(l[0..(l.Length / 2)]), new Compartment(l[(l.Length / 2)..])));

// Part 1
var commonItems = rucksacks.Select(r => r.CommonItems());

var sum = 0;
var allCommonTypes = new HashSet<char>();

foreach (var item in commonItems)
    foreach (var c in item)
        sum += PriorityOf(c);

Console.WriteLine(sum);

// Part 2
var sum2 = 0;

for (var i = 0; i < rucksacks.Count() / 3; ++i)
{
    var sacks = rucksacks.Skip(i * 3).Take(3).ToList();

    var shared = sacks[0].CommonItems(sacks[1]);

    shared.IntersectWith(sacks[0].CommonItems(sacks[2]));
    shared.IntersectWith(sacks[1].CommonItems(sacks[2]));

    sum2 += PriorityOf(shared.First());
}

Console.WriteLine(sum2);

static int PriorityOf(char item)
{
    return item switch
    {
        >= 'a' and <= 'z' => item - 'a' + 1,
        >= 'A' and <= 'Z' => item - 'A' + 27,
        _ => throw new InvalidOperationException()
    };
}

class Compartment
{
    public string Content { get; }

    public Compartment(string content)
    {
        Content = content;
    }

    public HashSet<char> SharedWith(Compartment other)
    {
        var common = new HashSet<char>();

        foreach (var c in other.Content)
            if (Content.Contains(c))
                common.Add(c);

        return common;
    }
}

class Rucksack
{
    public Compartment Left { get; }
    public Compartment Right { get; }

    public Rucksack(Compartment left, Compartment right)
    {
        Left = left;
        Right = right;
    }

    public HashSet<char> CommonItems()
    {
        return Left.SharedWith(Right);
    }

    public HashSet<char> CommonItems(Rucksack rucksack)
    {
        var common = new HashSet<char>();

        common.UnionWith(Left.SharedWith(rucksack.Left));
        common.UnionWith(Left.SharedWith(rucksack.Right));
        common.UnionWith(Right.SharedWith(rucksack.Left));
        common.UnionWith(Right.SharedWith(rucksack.Right));

        return common;
    }
}

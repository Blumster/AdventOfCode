var lines = File.ReadAllLines("input.txt");
var pairs = lines.Select(l => l.Split(',')).Select(p => new SectionPair(Section.Parse(p[0]), Section.Parse(p[1])));

// Part1
Console.WriteLine(pairs.Count(p => p.OneFullyContainsTheOther()));

// Part2
Console.WriteLine(pairs.Count(p => p.Overlaps()));

enum OverlapState
{
    None,
    Partial,
    Contains
}

record class Section(int Start, int End)
{
    public OverlapState CalcOverlap(Section other)
    {
        if (other.End < Start || End < other.Start)
            return OverlapState.None;

        if (other.Start <= Start && other.End >= End)
            return OverlapState.Contains;

        return OverlapState.Partial;
    }

    public static Section Parse(string range)
    {
        var parts = range.Split('-');

        return new Section(int.Parse(parts[0]), int.Parse(parts[1]));
    }
}

record class SectionPair(Section Left, Section Right)
{
    public bool OneFullyContainsTheOther()
    {
        return Left.CalcOverlap(Right) == OverlapState.Contains || Right.CalcOverlap(Left) == OverlapState.Contains;
    }

    public bool Overlaps()
    {
        return Left.CalcOverlap(Right) != OverlapState.None && Right.CalcOverlap(Left) != OverlapState.None;
    }
}

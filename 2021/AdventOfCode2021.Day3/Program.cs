int[] GetCounts(IEnumerable<string> lines, int bitCount)
{
    var counts = new int[bitCount];

    foreach (var line in lines)
    {
        for (var i = 0; i < bitCount; ++i)
        {
            counts[i] += line[i] == '1' ? 1 : 0;
        }
    }

    return counts;
}

var lines = File.ReadAllLines("input.txt");

var bitCount = lines[0].Length;

var counts = GetCounts(lines, bitCount);

var gammaRate = 0;

for (var i = 0; i < bitCount; ++i)
{
    if (counts[i] > lines.Length / 2)
    {
        gammaRate |= 1 << (bitCount - i - 1);
    }
}

var epsilonRate = ~gammaRate & ((1 << bitCount) - 1);

Console.WriteLine($"Task1: Gamma rate: {gammaRate} epsilon rate: {epsilonRate} power consumption: {gammaRate * epsilonRate}");

(int count, int matchCount, string lastMatchedValue) GetMaskedCount(IEnumerable<string> lines, int bit, string mask)
{
    var count = 0;
    var matchCount = 0;
    string lastMatchedValue = "";

    foreach (var line in lines)
    {
        if (mask.Length > 0 && !line.StartsWith(mask))
            continue;

        count += line[bit] == '1' ? 1 : 0;

        lastMatchedValue = line;

        ++matchCount;
    }

    return (count, matchCount, lastMatchedValue);
}

int CalculateRating(IEnumerable<string> lines, int bitCount, bool mostCommon)
{
    var mask = "";

    for (var i = 0; i < bitCount; ++i)
    {
        (var count, var matchCount, var lastMatchedValue) = GetMaskedCount(lines, i, mask);
        if (matchCount == 1)
        {
            return Convert.ToInt32(lastMatchedValue, 2);
        }

        if (count * 2 >= matchCount)
        {
            mask = $"{mask}{(mostCommon ? 1 : 0)}";
        }
        else
        {
            mask = $"{mask}{(mostCommon ? 0 : 1)}";
        }
    }

    return Convert.ToInt32(mask, 2);
}

var oxygenRating = CalculateRating(lines, bitCount, true);
var co2ScrubberRating = CalculateRating(lines, bitCount, false);

Console.WriteLine($"Task2: O2 rating: {oxygenRating} CO2 scrubber rating: {co2ScrubberRating} life support rating: {oxygenRating * co2ScrubberRating}");
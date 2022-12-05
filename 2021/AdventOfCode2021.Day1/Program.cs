static int CalcIncreaseCount(IEnumerable<string> lines, int windowSize)
{
    var increaseCount = 0;
    var values = new List<int>();

    foreach (var line in lines)
    {
        var lineValue = int.Parse(line);

        if (values.Count < windowSize)
        {
            values.Add(lineValue);
            continue;
        }

        var currentSum = values.Sum();

        values.RemoveAt(0);
        values.Add(lineValue);

        if (values.Sum() > currentSum)
            ++increaseCount;
    }

    return increaseCount;
}

var lines = File.ReadAllLines("input.txt");

Console.WriteLine($"Increases(1): {CalcIncreaseCount(lines, 1)}");
Console.WriteLine($"Increases(3): {CalcIncreaseCount(lines, 3)}");


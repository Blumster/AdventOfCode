var lines = File.ReadAllLines("input.txt");

var risks = new byte[lines.Length][];

for (var i = 0; i < lines.Length; ++i)
{
    risks[i] = lines[i].Select(i => byte.Parse(i.ToString())).ToArray();
}

var lowestRisks = new int[lines.Length][];

for (var i = 0; i < lines.Length; ++i)
{
    lowestRisks[i] = new int[lines.Length];

    for (var j = 0; j < lines.Length; ++j)
    {
        lowestRisks[i][j] = int.MaxValue;
    }
}

static void Calculate(int x, int y, int riskValue, int[][] lowestRisks, byte[][] risks)
{
    var processQueue = new Queue<(int x, int y, int riskValue)>();
    processQueue.Enqueue((0, 0, 0));

    while (processQueue.Count > 0)
    {
        var entry = processQueue.Dequeue();

        if (entry.riskValue >= lowestRisks[entry.x][entry.y])
            continue;

        lowestRisks[entry.x][entry.y] = entry.riskValue;

        if (entry.x < lowestRisks.Length - 1)
            processQueue.Enqueue((entry.x + 1, entry.y, entry.riskValue + risks[entry.x + 1][entry.y]));

        if (entry.y < lowestRisks[entry.x].Length - 1)
            processQueue.Enqueue((entry.x, entry.y + 1, entry.riskValue + risks[entry.x][entry.y + 1]));

        if (entry.x > 0)
            processQueue.Enqueue((entry.x - 1, entry.y, entry.riskValue + risks[entry.x - 1][entry.y]));

        if (entry.y > 0)
            processQueue.Enqueue((entry.x, entry.y - 1, entry.riskValue + risks[entry.x][entry.y - 1]));
    }
}

Calculate(0, 0, 0, lowestRisks, risks);

Console.WriteLine($"Task1: lowest risk: {lowestRisks[risks.Length - 1][risks.Length - 1]}");

var lowestRisks2 = new int[lowestRisks.Length * 5][];

for (var i = 0; i < lowestRisks2.Length; ++i)
{
    lowestRisks2[i] = new int[lowestRisks.Length * 5];

    for (var j = 0; j < lowestRisks2[i].Length; ++j)
    {
        lowestRisks2[i][j] = int.MaxValue;
    }
}

var risks2 = new byte[risks.Length * 5][];
for (var i = 0; i < risks2.Length; ++i)
{
    risks2[i] = new byte[risks.Length * 5];
}

for (var i = 0; i < 5; ++i)
{
    for (var j = 0; j < 5; ++j)
    {
        var toAdd = (byte)(i + j);

        for (var x = 0; x < risks.Length; ++x)
        {
            for (var y = 0; y < risks[x].Length; ++y)
            {
                var newVal = (byte)(risks[x][y] + toAdd);
                if (newVal > 9)
                    newVal -= 9;

                risks2[x + risks.Length * i][y + risks.Length * j] = newVal;
            }
        }
    }
}

Calculate(0, 0, 0, lowestRisks2, risks2);

Console.WriteLine($"Task1: lowest risk: {lowestRisks2[risks2.Length - 1][risks2.Length - 1]}");

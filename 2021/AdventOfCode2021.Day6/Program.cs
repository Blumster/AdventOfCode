var lines = File.ReadAllLines("input.txt");

var fishes = lines[0].Split(',').Select(x => int.Parse(x)).ToList();
var fishContainers = new List<FishContainer>();

for (var i = 0; i <= 8; ++i)
{
    fishContainers.Add(new FishContainer
    {
        Day = i,
        Count = fishes.Count(f => f == i)
    });
}

void SimulateDays(int days)
{
    for (var d = 0; d < days; ++d)
    {
        var newCycleFishes = 0L;

        foreach (var fishCont in fishContainers)
        {
            if (--fishCont.Day == -1)
            {
                fishCont.Day = 8;
                newCycleFishes += fishCont.Count;
            }
        }

        foreach (var fishCont in fishContainers)
        {
            if (fishCont.Day == 6)
            {
                fishCont.Count += newCycleFishes;
                break;
            }
        }
    }
}

SimulateDays(80);

Console.WriteLine($"Task1: lanternfish count: {fishContainers.Sum(fc => fc.Count)}");

SimulateDays(256 - 80);

Console.WriteLine($"Task2: lanternfish count: {fishContainers.Sum(fc => fc.Count)}");

class FishContainer
{
    public int Day { get; set; }
    public long Count { get; set; }
}

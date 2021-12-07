var lines = File.ReadAllLines("input.txt");

var positions = lines[0].Split(',').Select(x => int.Parse(x)).ToList();
var lowestPosition = positions.Min();
var highestPosition = positions.Max();

var lowestFuel = int.MaxValue;
var position = 0;

for (var i = lowestPosition; i <= highestPosition; ++i)
{
    var fuel = positions.Sum(x => (int)Math.Abs(x - i));
    if (fuel < lowestFuel)
    {
        lowestFuel = fuel;
        position = i;
    }
}

Console.WriteLine($"Task1: total fuel to reach position {position}: {lowestFuel}");

lowestFuel = int.MaxValue;
position = 0;

for (var i = lowestPosition; i <= highestPosition; ++i)
{
    var fuel = positions.Sum(x =>
    {
        var distance = (int)Math.Abs(x - i);

        return distance * (distance + 1) / 2;
    });
    if (fuel < lowestFuel)
    {
        lowestFuel = fuel;
        position = i;
    }
}

Console.WriteLine($"Task2: total fuel to reach position {position}: {lowestFuel}");

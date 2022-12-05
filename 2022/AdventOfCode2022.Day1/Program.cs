var lines = File.ReadAllLines("input.txt");

var caloriesByElf = new List<int>();
var endedWithEmptyLine = false;
var sum = 0;

foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line))
    {
        endedWithEmptyLine = true;

        caloriesByElf.Add(sum);

        sum = 0;
        continue;
    }

    endedWithEmptyLine = false;

    sum += int.Parse(line);
}

// Add last elf's calories (no new line at the end)
if (!endedWithEmptyLine)
    caloriesByElf.Add(sum);

caloriesByElf.Sort();

// Part1
Console.WriteLine(caloriesByElf[^1]);

// Part2
Console.WriteLine(caloriesByElf.TakeLast(3).Sum());

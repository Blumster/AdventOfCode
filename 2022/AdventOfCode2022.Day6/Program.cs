var input = File.ReadAllText("input.txt");

// Part1
Console.WriteLine(FirstNUniqueChars(input, 4));

// Part2
Console.WriteLine(FirstNUniqueChars(input, 14));

static int FirstNUniqueChars(string input, int numUniqueChars)
{
    var chars = new HashSet<char>();

    for (var i = 0; i < input.Length - numUniqueChars + 1; ++i)
    {
        chars.Clear();

        for (var j = 0; j < numUniqueChars; ++j)
            chars.Add(input[i + j]);

        if (chars.Count != numUniqueChars)
            continue;

        return i + numUniqueChars;
    }

    return -1;
}

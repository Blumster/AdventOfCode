var lines = File.ReadAllLines("input.txt");

bool IsUniqueDigit(string text)
{
    return text.Length == 2 || text.Length == 4 || text.Length == 7 || text.Length == 3;
}

var count = 0;

foreach (var line in lines)
{
    var pieces = line.Split(' ');
    var found = false;

    foreach (var piece in pieces)
    {
        if (piece == "|")
        {
            found = true;
            continue;
        }

        if (found && IsUniqueDigit(piece))
        {
            ++count;
        }
    }
}

Console.WriteLine($"Task1: count: {count}");

var sum = 0;
var digits = new Dictionary<int, string>();

int IntersectionCount(string text1, string text2)
{
    var count = 0;

    foreach (var c in text1)
    {
        if (text2.Contains(c))
            ++count;
    }

    return count;
}

int GetDigit(string text)
{
    if (text.Length == 2) // 1
        return 1;

    if (text.Length == 3) // 7
        return 7;

    if (text.Length == 4) // 4
        return 4;

    if (text.Length == 7) // 8
        return 8;

    if (text.Length == 6) // 0, 6, 9
    {
        if (digits.ContainsKey(1) && IntersectionCount(text, digits[1]) == 1) // 6
        {
            return 6;
        }

        if (digits.ContainsKey(4) && IntersectionCount(text, digits[4]) == 4) // 9
        {
            return 9;
        }

        if (digits.ContainsKey(4) && IntersectionCount(text, digits[4]) == 3 &&
            digits.ContainsKey(1) && IntersectionCount(text, digits[1]) == 2) // 0
        {
            return 0;
        }
    }
    else if (text.Length == 5) // 2, 3, 5
    {
        if (digits.ContainsKey(6) && IntersectionCount(text, digits[6]) == 5) // 5
        {
            return 5;
        }

        if (digits.ContainsKey(5) && IntersectionCount(text, digits[5]) == 4) // 3
        {
            return 3;
        }

        if (digits.ContainsKey(5) && IntersectionCount(text, digits[5]) == 3) // 2
        {
            return 2;
        }
    }

    return -1;
}

foreach (var line in lines)
{
    digits.Clear();

    var halves = line.Split('|', StringSplitOptions.RemoveEmptyEntries);

    var pieces = halves[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

    while (digits.Count != 10)
    {
        for (var i = 0; i < pieces.Length && digits.Count < 10; ++i)
        {
            int digit = GetDigit(pieces[i]);
            if (digit != -1 && !digits.ContainsKey(digit))
            {
                digits.Add(digit, string.Concat(pieces[i].OrderBy(c => c)));
            }
        }
    }

    var num = 0;
    var resultPieces = halves[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

    for (var i = 0; i < 4; ++i)
    {
        var value = string.Concat(resultPieces[i].OrderBy(c => c));

        foreach (var digit in digits)
        {
            if (digit.Value == value)
            {
                num += (int)Math.Pow(10, 3 - i) * digit.Key;
                break;
            }
        }
    }

    sum += num;
}

Console.WriteLine($"Task2: sum: {sum}");

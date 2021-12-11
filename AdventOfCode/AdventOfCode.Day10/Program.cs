var lines = File.ReadAllLines("input.txt");

char GetProperCloseChar(char c)
{
    if (c == '(')
        return ')';

    if (c == '[')
        return ']';

    if (c == '{')
        return '}';

    if (c == '<')
        return '>';

    return '\0';
}

bool IsOpenChar(char c)
{
    return c == '(' || c == '[' || c == '{' || c == '<';
}

bool IsCloseChar(char c)
{
    return !IsOpenChar(c);
}

var corruptedChars = new List<char>();
var incompleteStacks = new List<Stack<char>>();

foreach (var line in lines)
{
    var corrupted = false;
    var stack = new Stack<char>();

    foreach (var c in line)
    {
        if (IsOpenChar(c))
            stack.Push(c);

        if (IsCloseChar(c))
        {
            var top = stack.Pop();

            if (GetProperCloseChar(top) != c)
            {
                corruptedChars.Add(c);
                corrupted = true;
                break;
            }
        }
    }

    if (!corrupted && stack.Count > 0)
    {
        incompleteStacks.Add(stack);
    }
}

var charValues = new Dictionary<char, int>()
{
    { ')', 3 },
    { ']', 57 },
    { '}', 1197 },
    { '>', 25137 }
};

var errorScore = 0;

foreach (var c in corruptedChars)
{
    errorScore += charValues[c];
}

Console.WriteLine($"Task1: error score: {errorScore}");

var errorValues = new Dictionary<char, int>()
{
    { ')', 1 },
    { ']', 2 },
    { '}', 3 },
    { '>', 4 }
};

var scores = new List<long>();

foreach (var stack in incompleteStacks)
{
    var score = 0L;

    while (stack.Count > 0)
    {
        var c = stack.Pop();

        score *= 5;
        score += errorValues[GetProperCloseChar(c)];
    }

    scores.Add(score);
}

scores.Sort();

Console.WriteLine($"Task2: middle score: {scores[scores.Count / 2]}");

var lines = File.ReadAllLines("input.txt");

var num = Number.Parse(lines[0])!;

for (var i = 1; i < lines.Length; ++i)
{
    var num2 = Number.Parse(lines[i])!;

    num += num2;
}

Console.WriteLine($"Task1 Result: {num}");
Console.WriteLine($"Task1 Magnitude: {num.Magnitude()}");

var maxMagnitude = 0L;

for (var i = 0; i < lines.Length; ++i)
{
    for (var j = 0; j < lines.Length; ++j)
    {
        if (i == j)
            continue;

        var num1 = Number.Parse(lines[i])! + Number.Parse(lines[j])!;
        var num2 = Number.Parse(lines[j])! + Number.Parse(lines[i])!;

        if (num1.Magnitude() > maxMagnitude)
            maxMagnitude = num1.Magnitude();

        if (num2.Magnitude() > maxMagnitude)
            maxMagnitude = num2.Magnitude();
    }
}

Console.WriteLine($"Task2: largest magnitude: {maxMagnitude}");

enum NumberType
{
    Pair,
    Value
}

class Number
{
    public Number? Parent { get; set; }
    public Number? Left { get; set; }
    public Number? Right { get; set; }
    public NumberType Type { get; set; }
    public int Value { get; set; }

    public Number(int value)
    {
        Type = NumberType.Value;
        Value = value;
    }

    public Number(Number left, Number right)
    {
        Type = NumberType.Pair;
        Left = left;
        Right = right;

        Left.Parent = this;
        Right.Parent = this;
    }

    public static Number operator+(Number left, Number right)
    {
        var result = new Number(left, right);

        result.Reduce();

        return result;
    }

    public static Number? Parse(string input)
    {
        var offset = 0;
        return ParseInternal(input, ref offset);
    }

    private static Number? ParseInternal(string input, ref int offset)
    {
        if (input[offset] != '[')
            return null;

        offset += 1;

        Number? num1;
        if (input[offset] == '[')
        {
            num1 = ParseInternal(input, ref offset);
        }
        else
        {
            num1 = new Number(input[offset] - '0');

            offset += 1;
        }

        if (input[offset] != ',')
            return null;

        offset += 1;

        Number? num2;
        if (input[offset] == '[')
        {
            num2 = ParseInternal(input, ref offset);
        }
        else
        {
            num2 = new Number(input[offset] - '0');

            offset += 1;
        }

        if (input[offset] != ']')
            return null;

        offset += 1;

        if (num1 == null || num2 == null)
            return null;

        return new Number(num1, num2);
    }

    public Number? GetFirstRegularNumber(bool toLeft)
    {
        if ((toLeft && Parent?.Left == this) || (!toLeft && Parent?.Right == this))
            return Parent.GetFirstRegularNumber(toLeft);

        if (toLeft && Parent?.Left != this)
            return Parent?.Left?.GetRegularNumber(!toLeft);

        if (!toLeft && Parent?.Right != this)
            return Parent?.Right?.GetRegularNumber(!toLeft);
        
        return null;
    }

    public Number? GetRegularNumber(bool toLeft)
    {
        if (Type == NumberType.Value)
            return this;

        if (Type != NumberType.Pair)
            return null;

        if (toLeft)
        {
            return Left?.GetRegularNumber(toLeft);
        }

        return Right?.GetRegularNumber(toLeft);
    }

    public void Reduce()
    {
        while (true)
        {
            var res = ExplodeInternal(this);
            if (res)
                continue;

            res = SplitInternal(this);
            if (!res)
                break;
        }
    }

    private static bool ExplodeInternal(Number number, int depth = 0)
    {
        if (number.Type == NumberType.Pair)
        {
            if (depth == 4)
            {
                number.Explode();
                return true;
            }

            if (number.Left == null || number.Right == null)
                return true;

            if (ExplodeInternal(number.Left, depth + 1))
                return true;

            if (ExplodeInternal(number.Right, depth + 1))
                return true;
        }

        return false;
    }

    private static bool SplitInternal(Number number)
    {
        if (number.Type == NumberType.Pair)
        {
            if (number.Left == null || number.Right == null)
                return true;

            if (SplitInternal(number.Left))
                return true;

            if (SplitInternal(number.Right))
                return true;
        }

        if (number.Type == NumberType.Value && number.Value > 9)
        {
            number.Split();
            return true;
        }

        return false;

    }

    private void Explode()
    {
        if (Left == null || Right == null)
            return;

        var leftRegularNumber = GetFirstRegularNumber(true);
        if (leftRegularNumber != null)
            leftRegularNumber.Value += Left.Value;

        var lrightRegularNumber = GetFirstRegularNumber(false);
        if (lrightRegularNumber != null)
            lrightRegularNumber.Value += Right.Value;

        Type = NumberType.Value;
        Value = 0;
        Left = null;
        Right = null;
    }

    private void Split()
    {
        Left = new Number((int)Math.Round(Value / 2.0f, MidpointRounding.ToNegativeInfinity))
        {
            Parent = this
        };

        Right = new Number((int)Math.Round(Value / 2.0f, MidpointRounding.ToPositiveInfinity))
        {
            Parent = this
        };

        Type = NumberType.Pair;
        Value = 0;
    }

    public long Magnitude()
    {
        if (Type == NumberType.Value)
            return Value;

        return Left!.Magnitude() * 3 + Right!.Magnitude() * 2;
    }

    public override string ToString()
    {
        if (Type == NumberType.Value)
            return Value.ToString();

        return $"[{Left},{Right}]";
    }
}
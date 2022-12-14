using System.Text;

var lines = File.ReadAllLines("input.txt");
var packets = lines.Where(l => !string.IsNullOrWhiteSpace(l)).Select(DataList.Parse).ToList();

// Part1
var indexSum = 0;

for (var i = 0; i < packets.Count / 2; ++i)
{
    var cmdIndex = i * 2;

    var result = packets[cmdIndex].Compare(packets[cmdIndex + 1]);
    if (result <= 0)
        indexSum += i + 1;
}

Console.WriteLine(indexSum);

// Part2
packets.Add(DataList.Parse("[[2]]"));
packets.Add(DataList.Parse("[[6]]"));

packets.Sort((x, y) => x.Compare(y));

var dividerProduct = 1;

for (var i = 0; i < packets.Count; ++i)
    if (packets[i].ToString() == "[[2]]" || packets[i].ToString() == "[[6]]")
        dividerProduct *= i + 1;

Console.WriteLine(dividerProduct);

class DataList
{
    public enum DataType
    {
        Integer,
        List
    }

    public const int EmptyValue = int.MinValue;

    public DataType Type => Value == EmptyValue ? DataType.List : DataType.Integer;
    public bool IsEmpty => Type == DataType.List && SubLists.Count == 0;

    public int Value { get; protected set; } = EmptyValue;
    public List<DataList> SubLists { get; } = new();

    protected DataList(int value = EmptyValue)
    {
        Value = value;
    }

    public static DataList Parse(string input)
    {
        if (!input.StartsWith('[') || !input.EndsWith(']'))
            throw new ArgumentException("List must start with '[' and end with ']'!", nameof(input));

        var list = new DataList();

        if (input == "[]")
            return list;        

        var body = input[1..^1];
        var parts = new List<string>();
        var partStart = 0;
        var depth = 0;

        for (var i = 0; i < body.Length; ++i)
        {
            if (body[i] == '[')
                ++depth;
            else if (body[i] == ']')
                --depth;
            else if (body[i] == ',')
            {
                if (depth > 0)
                    continue;

                parts.Add(body[partStart..i]);
                partStart = i + 1;
            }
        }

        parts.Add(body[partStart..]);
        foreach (var part in parts)
        {
            if (!part.StartsWith('['))
                list.SubLists.Add(new DataList(int.Parse(part)));
            else
                list.SubLists.Add(Parse(part));
        }

        return list;
    }

    public int Compare(DataList other)
    {
        if (Type == DataType.List && other.Type == DataType.List)
        {
            if (IsEmpty)
            {
                if (other.IsEmpty)
                    return 0;

                return -1;
            }
            else if (other.IsEmpty)
                return 1;

            for (var i = 0; i < SubLists.Count; ++i)
            {
                if (i >= other.SubLists.Count)
                    break;

                var result = SubLists[i].Compare(other.SubLists[i]);
                if (result == 1)
                    return 1;

                if (result == -1)
                    return -1;
            }

            if (SubLists.Count != other.SubLists.Count)
                return SubLists.Count < other.SubLists.Count ? -1 : 1;

            return 0;
        }

        if (Type == DataType.Integer && other.Type == DataType.Integer)
        {
            if (Value == other.Value)
                return 0;

            return Value > other.Value ? 1 : -1;
        }

        if (Type == DataType.Integer && other.Type == DataType.List)
        {
            var list = new DataList();

            list.SubLists.Add(this);

            return list.Compare(other);
        }

        if (Type == DataType.List && other.Type == DataType.Integer)
        {
            var list = new DataList();

            list.SubLists.Add(other);

            return Compare(list);
        }

        throw new NotSupportedException();
    }

    public override string ToString()
    {
        if (Type == DataType.Integer)
            return Value.ToString();

        var sb = new StringBuilder("[");

        var first = true;
        foreach (var list in SubLists)
        {
            if (first)
                first = false;
            else
                sb.Append(',');

            sb.Append(list.ToString());
        }

        return sb.Append(']').ToString();
    }
}

using System.Text;

var lines = File.ReadAllLines("input.txt");

var values = lines.Select(l =>
{
    var digits = l.Where(char.IsDigit);
    return int.Parse($"{digits.First()}{digits.Last()}");
});

// Part1
Console.WriteLine(values.Sum());

values = lines.Select(l =>
{
    var digits = ProcessLine(l).Where(char.IsDigit);
    return int.Parse($"{digits.First()}{digits.Last()}");
});

// Part2
Console.WriteLine(values.Sum());

static string ProcessLine(string line)
{
    var sb = new StringBuilder();

    for (var i = 0; i < line.Length; ++i)
    {
        switch (line[i])
        {
            case 'o':
                if (i + 2 < line.Length && line[i..(i + 3)] == "one")
                    sb.Append('1');

                break;

            case 't':
                if (i + 2 < line.Length && line[i..(i + 3)] == "two")
                    sb.Append('2');
                else if (i + 4 < line.Length && line[i..(i + 5)] == "three")
                    sb.Append('3');

                break;

            case 'f':
                if (i + 3 < line.Length && line[i..(i + 4)] == "four")
                    sb.Append('4');
                else if (i + 3 < line.Length && line[i..(i + 4)] == "five")
                    sb.Append('5');

                break;

            case 's':
                if (i + 2 < line.Length && line[i..(i + 3)] == "six")
                    sb.Append('6');
                else if (i + 4 < line.Length && line[i..(i + 5)] == "seven")
                    sb.Append('7');

                break;

            case 'e':
                if (i + 4 < line.Length && line[i..(i + 5)] == "eight")
                    sb.Append('8');

                break;

            case 'n':
                if (i + 3 < line.Length && line[i..(i + 4)] == "nine")
                    sb.Append('9');

                break;

            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                sb.Append(line[i]);
                break;

            default:
                break;
        }
    }

    return sb.ToString();
}
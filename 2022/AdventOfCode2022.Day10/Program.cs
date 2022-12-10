using System.Text;

var opcodes = File.ReadAllLines("input.txt");
var cpu = new CPU();

foreach (var opcode in opcodes)
    cpu.Process(opcode);

// Part1
Console.WriteLine(cpu.SignalStrength);

// Part2
Console.WriteLine(cpu.CRT.ToString());

class CPU
{
    public int X { get; set; } = 1;
    public int Cycle { get; set; } = 0;
    public int SignalStrength { get; set; } = 0;
    public StringBuilder CRT { get; set; } = new();

    public void Process(string opcode)
    {
        DoCycle();

        switch (opcode[0..4])
        {
            case "addx":
                DoCycle();
                
                X += int.Parse(opcode[5..]);
                
                break;

            case "noop":
                break;

            default:
                throw new InvalidOperationException();
        }
    }

    private void DoCycle()
    {
        var pixel = Cycle % 40;

        if (Cycle > 0 && pixel == 0)
            CRT.AppendLine();

        if (pixel == X - 1 || pixel == X || pixel == X + 1)
            CRT.Append('#');
        else
            CRT.Append('.');

        if ((++Cycle - 20) % 40 == 0)
            SignalStrength += X * Cycle;
    }
}

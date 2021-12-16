using System.Globalization;
using System.Text;

var lines = File.ReadAllLines("input.txt");

var input = lines[0];

var data = input.Chunk(2).Select(x => byte.Parse($"{x[0]}{x[1]}", NumberStyles.HexNumber)).ToArray();

static bool ReadBit(byte[] data, ref int offset)
{
    var byteOffset = offset >> 3;
    var bitOffset = 7 - (offset % 8);

    var mask = 1 << bitOffset;
    var val = (data[byteOffset] & mask) == mask;

    ++offset;

    return val;
}

static long ReadBits(byte[] data, ref int offset, int bitCount)
{
    var result = 0L;

    for (var i = 0; i < bitCount; ++i)
    {
        if (ReadBit(data, ref offset))
            result |= 1;

        if (i < bitCount - 1)
            result <<= 1;
    }

    return result;
}

static long Read5BitBlockLong(byte[] data, ref int offset)
{
    var nextBlock = false;
    var result = 0L;

    do
    {
        nextBlock = ReadBit(data, ref offset);
        result |= ReadBits(data, ref offset, 4);

        if (nextBlock)
            result <<= 4;
    }
    while (nextBlock);

    return result;
}

static Packet ReadPacket(byte[] data, ref int offset, Packet? parent = null)
{
    var version = ReadBits(data, ref offset, 3);
    var typeID = (PacketType)ReadBits(data, ref offset, 3);

    var literal = 0L;
    if (typeID == PacketType.Literal)
        literal = Read5BitBlockLong(data, ref offset);

    var packet = new Packet(version, typeID, literal, new());

    if (parent != null)
        parent.SubPackets.Add(packet);

    if (typeID != PacketType.Literal)
    {
        var lengthTypeID = ReadBit(data, ref offset);
        if (lengthTypeID)
        {
            var subPacketCount = ReadBits(data, ref offset, 11);

            for (var i = 0; i < subPacketCount; ++i)
                ReadPacket(data, ref offset, packet);
        }
        else
        {
            var subPacketsBitLength = ReadBits(data, ref offset, 15);

            while (subPacketsBitLength > 0)
            {
                var beforeOffset = offset;

                ReadPacket(data, ref offset, packet);

                subPacketsBitLength -= offset - beforeOffset;
            }
        }
    }

    return packet;
}

int offset = 0;
var packet = ReadPacket(data, ref offset);

static long CalcVersionSum(Packet root)
{
    var sum = root.Version;

    foreach (var subPacket in root.SubPackets)
        sum += CalcVersionSum(subPacket);

    return sum;
}

Console.WriteLine($"Task1: total versions: {CalcVersionSum(packet)}");

static long CalcValue(Packet packet)
{
    switch (packet.TypeID)
    {
        case PacketType.Sum:
            return packet.SubPackets.Select(sp => CalcValue(sp)).Sum();

        case PacketType.Product:
            return packet.SubPackets.Select(sp => CalcValue(sp)).Aggregate(1L, (acc, val) => acc * val);

        case PacketType.Minimum:
            return packet.SubPackets.Select(sp => CalcValue(sp)).Min();

        case PacketType.Maximum:
            return packet.SubPackets.Select(sp => CalcValue(sp)).Max();

        case PacketType.Literal:
            return packet.Literal;

        case PacketType.GreaterThan:
            return CalcValue(packet.SubPackets[0]) > CalcValue(packet.SubPackets[1]) ? 1 : 0;

        case PacketType.LessThen:
            return CalcValue(packet.SubPackets[0]) < CalcValue(packet.SubPackets[1]) ? 1 : 0;

        case PacketType.EqualTo:
            return CalcValue(packet.SubPackets[0]) == CalcValue(packet.SubPackets[1]) ? 1 : 0;

        default:
            return -1;
    }
}

Console.WriteLine($"Task2: value: {CalcValue(packet)}");

enum PacketType
{
    Sum = 0,
    Product = 1,
    Minimum = 2,
    Maximum = 3,
    Literal = 4,
    GreaterThan = 5,
    LessThen = 6,
    EqualTo = 7
}

record Packet(long Version, PacketType TypeID, long Literal, List<Packet> SubPackets)
{
    public string ToString(int depth)
    {
        var sb = new StringBuilder();

        var depthStr = new string(' ', depth);

        sb.Append(depthStr).Append($"Packet(Version: {Version}, TypeID: {TypeID}");

        if (TypeID == PacketType.Literal)
            sb.Append($", Literal: {Literal})");
        else
            sb.Append(')');

        if (SubPackets.Count == 0)
            return sb.ToString();

        sb.AppendLine().Append(depthStr).AppendLine("{");

        foreach (var packet in SubPackets)
        {
            sb.AppendLine(packet.ToString(depth + 2));
        }

        sb.Append(depthStr).Append('}');

        return sb.ToString();
    }

    public override string ToString()
    {
        return ToString(0);
    }
}

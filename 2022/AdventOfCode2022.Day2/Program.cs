var lines = File.ReadAllLines("input.txt");

// Part 1
var part1Sum = lines.Select(line => GameRound.Read(line, (type, _) => type switch
{
    "X" => ShapeType.Rock,
    "Y" => ShapeType.Paper,
    "Z" => ShapeType.Scissors,
    _ => throw new InvalidOperationException()
})).Select(gr => gr.Score()).Sum();

Console.WriteLine(part1Sum);

// Part 2
var part2Sum = lines.Select(line => GameRound.Read(line, (type, opponentShape) => type switch
{
    "X" => opponentShape.WinsAgainst(),
    "Y" => opponentShape,
    "Z" => opponentShape.LosesAgainst(),
    _ => throw new InvalidOperationException()
})).Select(gr => gr.Score()).Sum();

Console.WriteLine(part2Sum);

enum ShapeType
{
    Rock,
    Paper,
    Scissors
}

static class ShapeTypeHelpers
{
    public static ShapeType WinsAgainst(this ShapeType type) => type switch
    {
        ShapeType.Rock => ShapeType.Scissors,
        ShapeType.Paper => ShapeType.Rock,
        ShapeType.Scissors => ShapeType.Paper,
        _ => throw new InvalidOperationException()
    };

    public static ShapeType LosesAgainst(this ShapeType type) => type switch
    {
        ShapeType.Rock => ShapeType.Paper,
        ShapeType.Paper => ShapeType.Scissors,
        ShapeType.Scissors => ShapeType.Rock,
        _ => throw new InvalidOperationException()
    };

    public static int Score(this ShapeType type) => type switch
    {
        ShapeType.Rock => 1,
        ShapeType.Paper => 2,
        ShapeType.Scissors => 3,
        _ => throw new InvalidOperationException()
    };

    public static int ScoreAgainst(this ShapeType player, ShapeType opponent)
    {
        if (opponent.WinsAgainst() == player)
            return 0;

        if (player.WinsAgainst() == opponent)
            return 6;

        return 3;
    }
}

class GameRound
{
    public ShapeType Opponent { get; }
    public ShapeType Player { get; }

    private GameRound(ShapeType opponent, ShapeType player)
    {
        Opponent = opponent;
        Player = player;
    }

    public int Score()
    {
        return Player.Score() + Player.ScoreAgainst(Opponent);
    }

    public static GameRound Read(string line, Func<string, ShapeType, ShapeType> playerTypeLookup)
    {
        var parts = line.Split(' ');
        var opponentShape = BaseFindType(parts[0]);

        return new GameRound(opponentShape, playerTypeLookup(parts[1], opponentShape));
    }

    private static ShapeType BaseFindType(string value)
    {
        return value switch
        {
            "A" => ShapeType.Rock,
            "B" => ShapeType.Paper,
            "C" => ShapeType.Scissors,
            _ => throw new InvalidOperationException()
        };
    }
}

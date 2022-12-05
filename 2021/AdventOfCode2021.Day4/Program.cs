var lines = File.ReadAllLines("input.txt");

var drawnNumbers = lines[0].Split(',').Select(x => int.Parse(x)).ToList();

var boardIndex = -1;
var lineIndex = 0;
var boards = new List<Board>();

for (var i = 1; i < lines.Length; ++i)
{
    if (lines[i].Length == 0)
    {
        lineIndex = 0;

        ++boardIndex;

        boards.Add(new Board(boardIndex, new int[5][], new bool[5][]));
        continue;
    }

    boards[boardIndex].Marked[lineIndex] = new bool[5];
    boards[boardIndex].Values[lineIndex++] = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
}

bool IsWinningBoard(Board board)
{
    for (var i = 0; i < 5; ++i)
    {
        var allMarkedRow = true;
        var allMarkedCol = true;

        for (var j = 0; j < 5 && (allMarkedRow || allMarkedCol); ++j)
        {
            if (!board.Marked[i][j])
                allMarkedRow = false;

            if (!board.Marked[j][i])
                allMarkedCol = false;
        }

        if (allMarkedRow || allMarkedCol)
            return true;
    }

    return false;
}

int GetUnmarkedSum(Board board)
{
    var sum = 0;

    for (var i = 0; i < 5; ++i)
    {
        for (var j = 0; j < 5; ++j)
        {
            sum += board.Marked[i][j] ? 0 : board.Values[i][j];
        }
    }

    return sum;
}

var finalScores = new List<int>();

foreach (var draw in drawnNumbers)
{
    foreach (var board in boards)
    {
        if (IsWinningBoard(board))
            continue;

        for (var i = 0; i < 5; ++i)
        {
            for (var j = 0; j < 5; ++j)
            {
                if (board.Values[i][j] == draw)
                    board.Marked[i][j] = true;
            }
        }

        if (IsWinningBoard(board))
        {
            finalScores.Add(GetUnmarkedSum(board) * draw);
        }
    }
}

Console.WriteLine($"Task1: First winning board's final score {finalScores[0]}");
Console.WriteLine($"Task2: Last winning board's final score {finalScores[^1]}");

record Board(int Index, int[][] Values, bool[][] Marked);

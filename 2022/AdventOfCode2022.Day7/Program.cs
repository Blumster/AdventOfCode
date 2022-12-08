using System.Text.RegularExpressions;

var cdCommand = new Regex(@"^\$ cd (.*)$", RegexOptions.Compiled);
var dirData = new Regex(@"^dir (.*)$", RegexOptions.Compiled);
var fileData = new Regex(@"^(\d+) (.*)$", RegexOptions.Compiled);

var lines = File.ReadAllLines("input.txt");

var root = new DirectoryEntry("/", null, new(), new());
var currentDirectory = root;

foreach (var line in lines)
{
    if (line == "$ ls")
        continue;

    var m = cdCommand.Match(line);
    if (m.Success)
    {
        var dest = m.Groups[1].Value;
        if (dest == "/")
        {
            currentDirectory = root;
        }
        else if (dest == "..")
        {
            currentDirectory = currentDirectory.Parent ?? throw new InvalidOperationException();
        }
        else if (currentDirectory.SubDirectories.ContainsKey(dest))
        {
            currentDirectory = currentDirectory.SubDirectories[dest];
        }
        else
            throw new InvalidDataException();

        continue;
    }

    m = dirData.Match(line);
    if (m.Success)
    {
        currentDirectory.SubDirectories.Add(m.Groups[1].Value, new(m.Groups[1].Value, currentDirectory, new(), new()));
        continue;
    }

    m = fileData.Match(line);
    if (m.Success)
    {
        currentDirectory.Files.Add(m.Groups[2].Value, new(m.Groups[2].Value, int.Parse(m.Groups[1].Value)));
        continue;
    }

    throw new InvalidDataException();
}

// Part1
var part1Dirs = root.GetDirectoriesUnderSize(100000);

Console.WriteLine(part1Dirs.Sum(d => d.CalcSize()));

// Part2
const int totalSize = 70_000_000;
const int updateSize = 30_000_000;

var currentSize = root.CalcSize();

var part2Dirs = root.GetDirectoriesOverSize(currentSize + updateSize - totalSize);

Console.WriteLine(part2Dirs.Min(d => d.CalcSize()));

record class FileEntry(string Name, int Size);

record class DirectoryEntry(string Name, DirectoryEntry? Parent, Dictionary<string, DirectoryEntry> SubDirectories, Dictionary<string, FileEntry> Files)
{
    private int SizeCache { get; set; } = -1;

    public int CalcSize()
    {
        if (SizeCache == -1)
            SizeCache = SubDirectories.Sum(d => d.Value.CalcSize()) + Files.Sum(f => f.Value.Size);

        return SizeCache;
    }

    public IEnumerable<DirectoryEntry> GetDirectoriesUnderSize(int size)
    {
        foreach (var subDir in SubDirectories)
            foreach (var dir in subDir.Value.GetDirectoriesUnderSize(size))
                yield return dir;

        if (CalcSize() < size)
            yield return this;
    }

    public IEnumerable<DirectoryEntry> GetDirectoriesOverSize(int size)
    {
        foreach (var subDir in SubDirectories)
            foreach (var dir in subDir.Value.GetDirectoriesOverSize(size))
                yield return dir;

        if (CalcSize() >= size)
            yield return this;
    }
}

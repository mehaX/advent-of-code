var fileSystem = new FileSystem();
var terminal = new Terminal(fileSystem);
var input = File.ReadLines("input.txt").ToList();

terminal.Deserialize(input).Execute();

Console.WriteLine("Part 1: " + Part1());
Console.WriteLine("Part 2: " + Part2());

long Part1()
{
    var maxDirSize = 100_000L;
    
    return fileSystem
        .ListDirectories()
        .Select(fileSystem.CalculateDirSize)
        .Where(dirSize => dirSize <= maxDirSize)
        .Sum();
}

long Part2()
{
    var threshold = 30_000_000L;
    var totalSpace = 70_000_000L;

    var used = fileSystem.CalculateDirSize();
    var sizes = fileSystem.ListDirectories().Select(fileSystem.CalculateDirSize).ToList();
    return sizes.Order().First(size => used - size < totalSpace - threshold);
}
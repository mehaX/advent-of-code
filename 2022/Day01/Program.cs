static List<List<int>> GetElves()
{
    var inputs = File.ReadAllText("input.txt");

    var segments = inputs.Split("\r\n\r\n");

    return segments
        .Select(segment => segment
            .Split("\n")
            .Select(int.Parse)
            .ToList())
        .ToList();
}

static int Part1()
{
    var elves = GetElves();

    return elves.Select(calories => calories.Sum()).OrderDescending().First();
}

static int Part2()
{
    var elves = GetElves();

    return elves.Select(calories => calories.Sum()).OrderDescending().Take(3).Sum();
}

Console.WriteLine("Part1: " + Part1());
Console.WriteLine("Part2: " + Part2());

IEnumerable<(Interval pair1, Interval pair2)> GetInput()
{
    var input = File.ReadLines("input.txt");

    return input.Select(line => line.Split(",").Select(strPair =>
            {
                var intInterval = strPair.Split('-').Select(int.Parse).ToArray();
                return new Interval(intInterval[0], intInterval[1]);
            })
            .ToArray())
        .Select(pairs => (pairs[0], pairs[1]))
        .ToList();
}

int Part1()
{
    var pairs = GetInput();
    return pairs.Count(p => p.pair1.FullOverlaps(p.pair2) || p.pair2.FullOverlaps(p.pair1));
}

int Part2()
{
    var pairs = GetInput();
    return pairs.Count(p => p.pair1.Overlaps(p.pair2) || p.pair2.Overlaps(p.pair1));
}

Console.WriteLine("Part 1: " + Part1());
Console.WriteLine("Part 2: " + Part2());

record Interval(int From, int To);

static class Extensions
{
    public static bool Overlaps(this Interval pair1, Interval pair2)
    {
        return pair1.From <= pair2.To && pair1.To >= pair2.From;
    }
    
    public static bool FullOverlaps(this Interval pair1, Interval pair2)
    {
        return pair2.From >= pair1.From && pair2.From <= pair1.To && pair2.To >= pair1.From && pair2.To <= pair1.To;
    }
}

int EvaluateItem(char item)
{
    return item switch
    {
        >= 'a' and <= 'z' => item - 'a' + 1,
        >= 'A' and <= 'Z' => item - 'A' + 27,
        _ => 0,
    };
}

int Part1()
{
    var rucksacks = File.ReadLines("input.txt").Select(line =>
    {
        var middle = line.Length / 2;
        return (line.Substring(0, middle), line.Substring(middle));
    }).ToList();
    
    var matches = rucksacks.Select(r => r.Item1.Intersect(r.Item2).First());
    var totalValuation = matches.Select(EvaluateItem).Sum();
    return totalValuation;
}

int Part2()
{
    var rucksacks = File.ReadLines("input.txt").ToList();
    var groups = new List<string[]>();
    for (var i = 0; i < rucksacks.Count; i += 3)
    {
        groups.Add(rucksacks.Skip(i).Take(3).ToArray());
    }

    const string allItems = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    var mostCommonItems = groups.Select(g =>
    {
        var maxItem = '\0';
        var maxCount = 0;
        foreach (var item in allItems)
        {
            var counts = g.Select(line => line.Count(i => i == item)).ToList();
            if (counts.Contains(0))
            {
                continue;
            }
            if (counts.Sum() > maxCount)
            {
                maxCount = counts.Sum();
                maxItem = item;
            }
        }

        return maxItem;
    });

    var totalValuation = mostCommonItems.Select(EvaluateItem).Sum();
    return totalValuation;
}
    
Console.WriteLine("Part 1: " + Part1());
Console.WriteLine("Part 2: " + Part2());
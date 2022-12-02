
static void Calculate(int maxSteps)
{
    var time = DateTime.Now;
    ReadInput(out var template, out var pairInsertion);
    var counts = string.Join("", pairInsertion.Select(p => p.Item1)).ToCharArray().Distinct().ToDictionary(c => c, _ => (long)0);
    List<(int depth, char[] pair, Dictionary<char, long> counts)> track = new();
    
    foreach (var t in template)
    {
        counts[t]++;
    }
    for (var i = 0; i < template.Length - 1; i++)
    {
        CalculateDepth(template.Substring(i, 2).ToCharArray(), 1, maxSteps, pairInsertion, ref counts, ref track);
    }

    var highest = counts.Max(e => e.Value);
    var lowest = counts.Min(e => e.Value);
    var output = highest - lowest;
    var timestamp = DateTime.Now.Subtract(time).TotalSeconds;
    Console.WriteLine($"Result with steps: {maxSteps}, result {output}, time: {timestamp}");
}

static Dictionary<char, long> CalculateDepth(char[] pair, int depth, int maxDepth, List<(string, char)> pairInsertion, ref Dictionary<char, long> counts,
    ref List<(int depth, char[] pair, Dictionary<char, long> counts)> track)
{
    var existing = track.FirstOrDefault(t => t.depth == depth && t.pair[0] == pair[0] && t.pair[1] == pair[1]);
    if (existing.depth != 0)
    {
        foreach (var count in existing.counts)
        {
            counts[count.Key] += count.Value;
        }
    
        return existing.counts;
    }
    
    var newElement = GetElementFromPair(pair, pairInsertion);
    counts[newElement]++;

    Dictionary<char, long> newTrack = counts.ToDictionary(d => d.Key, _ => (long)0);
    newTrack[newElement]++;
    
    if (depth < maxDepth)
    {
        var leftBranch = CalculateDepth(new []{pair[0], newElement}, depth + 1, maxDepth, pairInsertion, ref counts, ref track);
        var rightBranch = CalculateDepth(new[]{newElement, pair[1]}, depth + 1, maxDepth, pairInsertion, ref counts, ref track);
        
        foreach (var newTrackKey in newTrack.Keys)
        {
            newTrack[newTrackKey] += leftBranch[newTrackKey] + rightBranch[newTrackKey];
        }
    }

    track.Add((depth, pair, newTrack));
    return newTrack;
}

static char GetElementFromPair(char[] pair, List<(string, char)> pairInsertion)
{
    return pairInsertion.First(p => p.Item1[0] == pair[0] && p.Item1[1] == pair[1]).Item2;
}

static void ReadInput(out string template, out List<(string, char)> pairInsertion)
{
    var input = File.ReadAllText("input.txt").Split("\n\n");

    template = input[0];

    pairInsertion = input[1].Split("\n").Select(row =>
    {
        var segments = row.Split(" -> ");
        return (segments[0], segments[1][0]);
    }).ToList();
}

for (int steps = 1; steps <= 60; steps++)
{
    Calculate(steps);
}

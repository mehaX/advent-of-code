using System.Text.RegularExpressions;

Console.WriteLine("Part 1: " + Part(30, false));
Console.WriteLine("Part 2: " + Part(26, true));


int Part(int remainingTime, bool hasElephant)
{
    var valves = GetInput();
    var human = valves.First(v => v.Identifier == "AA");
    var elephant = hasElephant ? human : null;
    
    var cache = new Dictionary<CacheKey, int>();
    var opened = new Dictionary<Valve, int>();

    return DFS(human, elephant, remainingTime - 1, opened, cache);
}

int DFS(Valve human, Valve? elephant, int time,
    Dictionary<Valve, int> opened,
    Dictionary<CacheKey, int> cache)
{
    var flowed = opened.Select(kv => kv.Key.Rate * kv.Value).Sum();
    if (time == 0)
    {
        return flowed;
    }

    var key = new CacheKey(human, elephant, time);
    if (cache.GetValueOrDefault(key, int.MinValue) >= flowed)
    {
        return 0;
    }

    if (!cache.TryAdd(key, flowed))
    {
        cache[key] = flowed;
    }
    
    var best = 0;

    foreach (var nextHuman in new[] { human }.Concat(human.LeadingValves))
    {
        if (nextHuman == human)
        {
            if (opened.ContainsKey(nextHuman) || nextHuman.Rate == 0) continue;
            opened.Add(nextHuman, time);
        }

        if (elephant == null)
        {
            best = Math.Max(best, DFS(nextHuman, elephant, time - 1, opened, cache));
        }
        else
        {
            foreach (var nextElephant in new[] { elephant }.Concat(elephant.LeadingValves))
            {
                if (nextElephant == elephant)
                {
                    if (opened.ContainsKey(nextElephant) || nextElephant.Rate == 0) continue;
                    opened.Add(nextElephant, time);
                }

                best = Math.Max(best, DFS(nextHuman, nextElephant, time - 1, opened, cache));

                if (nextElephant == elephant)
                {
                    opened.Remove(nextElephant);
                }
            }
        }

        if (human == nextHuman)
        {
            opened.Remove(nextHuman);
        }
    }

    return best;
}

List<Valve> GetInput()
{
    var strInput = File.ReadAllLines("input.txt");
    var regex = new Regex(@"Valve (\w\w) has flow rate=(\d+); tunnels? leads? to valves? (.+)");
    var captures = strInput.Select(line => regex.Match(line)).ToList();

    var valves = (from capture in captures
            let identifier = capture.Groups[1].Value
            let rate = int.Parse(capture.Groups[2].Value)
            select new Valve(identifier, rate)
        ).ToList();

    foreach (var capture in captures)
    {
        var valve = valves.First(v => v.Identifier == capture.Groups[1].Value);

        capture.Groups[3].Value
            .Trim().Split(", ")
            .Select(valve => valves.First(v => v.Identifier == valve)).ToList()
            .ForEach(v => valve.WithLeadingValve(v));
    }

    return valves;
}

record CacheKey(Valve Human, Valve? Elephant, int Time);

class Valve
{
    public string Identifier { get; init; }
    public int Rate { get; init; }

    public List<Valve> LeadingValves { get; init; }

    public Valve(string identifier, int rate)
    {
        Identifier = identifier;
        Rate = rate;
        LeadingValves = new();
    }

    public void WithLeadingValve(Valve valve)
    {
        LeadingValves.Add(valve);
    }
}
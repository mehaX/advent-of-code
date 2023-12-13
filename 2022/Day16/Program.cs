using System.Text.RegularExpressions;

var allValves = GetInput();
var startValve = allValves.First(v => v.Identifier == "AA");
var workingValves = allValves.Where(v => v.Rate > 0).ToList();
var pathCosts = new[] { startValve }.Concat(workingValves).Distinct()
    .Select(start => new KeyValuePair<Valve, Dictionary<Valve, int>>(start, workingValves
        .Where(end => end != start)
        .Select(end => new KeyValuePair<Valve, int>(end, GetLowestCost(start, end)))
        .ToDictionary(kv => kv.Key, kv => kv.Value)))
    .ToDictionary(kv => kv.Key, kv => kv.Value);

var allPaths = MakeAllPaths(30);
var scoredPaths = ScorePaths(allPaths, 30);
Console.WriteLine("Part 1: " + scoredPaths.First().Value);

int GetLowestCost(Valve start, Valve end)
{
    var queue = new Queue<QueueKey>();
    queue.Enqueue(new QueueKey(start, 0, new() { start }));

    while (queue.Any())
    {
        var node = queue.Dequeue();
        if (node.Valve == end)
        {
            return node.Cost;
        }

        if (node.Valve.LeadingValves.Contains(end))
        {
            return node.Cost + 1;
        }

        foreach (var edge in node.Valve.LeadingValves)
        {
            if (!node.Visited.Contains(edge))
            {
                queue.Enqueue(new QueueKey(
                    edge,
                    node.Cost + 1,
                    node.Visited.Concat(new[] { edge }).ToList())
                );
            }
        }
    }

    return -1;
}

void GetRemainingPath(List<Valve> steps, List<Valve> left, int costThusFar, List<List<Valve>> result, int time)
{
    var last = steps.Last();
    if (left.Any())
    {
        result.Add(steps);

        foreach (var next in left)
        {
            var cost = pathCosts[last][next];
            if (cost + 1 + costThusFar >= time)
            {
                result.Add(steps);
                continue;
            }

            GetRemainingPath(steps.Concat(new[] { next }).ToList(),
                left.Where(pos => pos != next).ToList(),
                costThusFar + cost + 1,
                result, time
            );
        }
    }
}

List<List<Valve>> MakeAllPaths(int time)
{
    var result = new List<List<Valve>>();

    GetRemainingPath(new List<Valve>() { startValve },
        workingValves, 0,
        result, time
    );

    return result;
}

Dictionary<List<Valve>, int> ScorePaths(List<List<Valve>> paths, int time)
{
    return paths.Select(path => (path,
            ScorePath(path.Take(1).ToList(), path.Skip(1).ToList(), time)))
        .ToDictionary(kv => kv.path, kv => kv.Item2);
}

int ScorePath(List<Valve> opened, List<Valve> path, int timeLeft)
{
    var nextStep = path[0];
    var remainingSteps = path.Skip(1).ToList();
    var nextStepCost = pathCosts[opened[0]][nextStep];
    var flowForStep = opened[0] == startValve ? 0 : opened[0].Rate * timeLeft;
    if (path.Any())
    {
        return flowForStep;
    }

    var pressureReleased = ScorePath(
        new[] { path[0] }.Concat(opened).ToList(),
        remainingSteps,
        timeLeft - nextStepCost - 1
    );

    return pressureReleased + flowForStep;
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

record QueueKey(Valve Valve, int Cost, List<Valve> Visited);

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
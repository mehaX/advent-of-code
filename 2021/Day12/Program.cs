
using Edges = System.Collections.Generic.List<(string, string)>;
using Nodes = System.Collections.Generic.List<string>;

static void Part(int smallCavesCount = 1)
{
    var edges = GetEdges();
    var startNode = "start";
    var solutions = new List<Nodes>();
    var track = new Nodes(){startNode};
    
    FindPath(edges, startNode, ref track, ref solutions, smallCavesCount);
    var result = solutions.Count;

    Console.WriteLine($"With Small caves count of {smallCavesCount}: {result}");
}

static Edges GetEdges()
{
    var input = File.ReadLines("input.txt");
    Edges edges = new();
    
    foreach (var s in input)
    {
        var nodes = s.Split("-");
        edges.Add((nodes[0], nodes[1]));
    }

    return edges;
}

static void FindPath(Edges edges, string sourceNode, ref Nodes track, ref List<Nodes> solutions, int smallCavesCount)
{
    var targetNodes = GetConnectedNodes(edges, sourceNode, track, smallCavesCount);
    
    foreach (var targetNode in targetNodes)
    {
        var newTrack = track.ToList(); // clone
        newTrack.Add(targetNode);
        if (targetNode.IsEnd())
        {
            solutions.Add(newTrack);
        }
        else
        {
            FindPath(edges, targetNode, ref newTrack, ref solutions, smallCavesCount);
        }
    }
}

static Nodes GetConnectedNodes(Edges edges, string node, Nodes track, int smallCavesCount)
{
    var connectedNodes = edges
        .Where(e => e.HasConnection(node))
        .Select(e => e.TargetNode(node))
        .Where(n => !n.IsStart())
        .Where(n => n.IsBigCave() || track.RepeatedSmallCavesCount() <= smallCavesCount)
        .ToList();

    return connectedNodes;
}

Part(1);
Part(2);


public static class Extensions
{
    public static bool IsBigCave(this string cave)
    {
        return cave[0] >= 'A' && cave[0] <= 'Z';
    }
    
    public static bool IsSmallCave(this string cave)
    {
        return !cave.IsStart() && !cave.IsEnd() && cave[0] >= 'a' && cave[0] <= 'z';
    }

    public static bool IsStart(this string cave)
    {
        return cave == "start";
    }

    public static bool IsEnd(this string cave)
    {
        return cave == "end";
    }

    public static bool HasConnection(this (string, string) edge, string node)
    {
        return edge.Item1 == node || edge.Item2 == node;
    }

    public static string TargetNode(this (string, string ) edge, string sourceNode)
    {
        return edge.Item1 == sourceNode ? edge.Item2 : edge.Item1;
    }

    public static bool IsInTrack(this Nodes track, string node)
    {
        return track.Any(n => n == node);
    }

    public static int RepeatedSmallCavesCount(this Nodes track)
    {
        return track.Where(node => node.IsSmallCave()).Count(smallCave => track.Count(node => node == smallCave) > 1);
    }
}
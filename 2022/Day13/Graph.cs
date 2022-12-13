namespace Day13;

internal class Graph
{
    private Dictionary<ListElement, List<ListElement>> mAllElements = new();

    private IEnumerable<ListElement> mNodes => mAllElements.Keys;

    public void AddElement(ListElement node, List<ListElement> edges)
    {
        mAllElements.Add(node, edges);
    }

    public List<ListElement> FindNodePath()
    {
        var result = new List<ListElement>();

        var index = 1;
        foreach (var startNode in mNodes)
        {
            var solution = Dijkstra(startNode);
            
            if (solution.Count == mNodes.Count())
            {
                result = solution;
                break;
            }

            index++;
        }

        return result;
    }

    private List<ListElement> Dijkstra(ListElement startNode)
    {
        var queue = new Queue<ListElement>();
        var paths = new Dictionary<ListElement, List<ListElement>>();
        foreach (var node in mNodes)
        {
            paths.Add(node, new() { node });
        }
        
        queue.Enqueue(startNode);
        while (queue.Any())
        {
            var node = queue.Dequeue();
            var path = paths[node];
            if (path.Count == mNodes.Count())
            {
                return path;
            }

            var edges = mAllElements[node];
            foreach (var edge in edges)
            {
                if (path.Count + 1 <= paths[edge].Count) continue;
                
                // memory is crying (eating almost 8 gigs like nothing), need to do better approach
                // but still it's way faster than DFS + recursion (recursion sucks)
                var newPath = path.ToList();
                newPath.Add(edge);
                paths[edge] = newPath;
                queue.Enqueue(edge);
            }
        }

        return new();
    }
}
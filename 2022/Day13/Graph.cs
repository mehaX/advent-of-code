namespace Day13;

internal class Graph
{
    private Dictionary<ListElement, List<ListElement>> mAllElements = new();

    private IEnumerable<ListElement> mNodes => mAllElements.Keys;

    public void AddNodeWithEdges(ListElement node, List<ListElement> edges)
    {
        mAllElements.Add(node, edges);
    }

    /**
     * Find path that goes through all nodes
     */
    public List<ListElement> FindEulerianPath()
    {
        var result = new List<ListElement>();

        foreach (var startNode in mNodes)
        {
            var solution = Dijkstra(startNode);
            
            if (solution.Count == mNodes.Count())
            {
                result = solution;
                break;
            }
        }

        return result;
    }

    /**
     * Actually this is kinda the reverse of Dijkstra, since we need the longest path instead of shortest
     */
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
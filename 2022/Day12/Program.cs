Console.WriteLine("Part 1: " + Part1());
Console.WriteLine("Part 2: " + Part2());

int Part1()
{
    var matrix = GetInput(out var start, out var end);
    var solution = Dijkstra(start!, end!, matrix);

    return solution.GetValueOrDefault();
}

int Part2()
{
    var matrix = GetInput(out var start, out var end);
    var solutions = new List<int>();
    for (int row = 0; row < matrix.GetLength(0); row++)
    {
        for (int col = 0; col < matrix.GetLength(1); col++)
        {
            if (matrix[row, col] != 'a')
            {
                continue;
            }

            start = new Position(row, col, matrix[row, col]);
            var solution = Dijkstra(start, end, matrix);
            if (solution != null)
            {
                solutions.Add(solution.Value);
            }
        }
    }

    return solutions.Order().First();
}

int? Dijkstra(Position start, Position end, char[,] matrix)
{
    var queue = new Queue<Position>();
    var distances = new Dictionary<Position, int>();
    queue.Enqueue(start);
    distances.Add(start, 0);

    while (queue.Any())
    {
        queue.TryDequeue(out var current);
        var distance = distances[current];
        if (current == end)
        {
            return distance;
        }
        
        var edges = GetEdges(current!, matrix);
        foreach (var edge in edges)
        {
            var newDist = distance + 1;
            if (newDist >= distances.GetValueOrDefault(edge, Int32.MaxValue)) continue;
            queue.Enqueue(edge);
            if (!distances.TryAdd(edge, newDist))
            {
                distances[edge] = newDist;
            }
        }
    }

    return null;
}

char[,] GetInput(out Position? start, out Position? end)
{
    var strInput = File.ReadLines("input.txt").ToList();
    start = null;
    end = null;
    var rows = strInput.Count;
    var cols = strInput.First().Length;
    var result = new char[rows, cols];
    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            if (strInput[row][col] == 'S')
            {
                result[row, col] = 'a';
                start = new Position(row, col, 'a');
            }
            else if (strInput[row][col] == 'E')
            {
                result[row, col] = 'z';
                end = new Position(row, col, 'z');
            }
            else
            {
                result[row, col] = strInput[row][col];
            }
        }
    }

    return result;
}

List<Position> GetEdges(Position basePosition, char[,] matrix)
{
    return new List<(int row, int col)>()
        {
            (basePosition.Row - 1, basePosition.Col),
            (basePosition.Row + 1, basePosition.Col),
            (basePosition.Row, basePosition.Col - 1),
            (basePosition.Row, basePosition.Col + 1),
        }
        .Where(pos => pos.row >= 0 && pos.row < matrix.GetLength(0)
                                       && pos.col >= 0 && pos.col < matrix.GetLength(1))
        .Where(pos => matrix[pos.row, pos.col] <= basePosition.Value + 1)
        .Select(pos => new Position(pos.row, pos.col, matrix[pos.row, pos.col]))
        .ToList();
}

record Position(int Row, int Col, char Value);
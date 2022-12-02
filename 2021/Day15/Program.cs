
static (int r, int c)[] AdjacentNodes(int r, int c)
{
    return new (int r, int c)[]
    {
        (r + 1, c),
        (r - 1, c),
        (r, c + 1),
        (r, c - 1),
    };
}

static int CalculateRisk(int[,] graph)
{
    var totalRows = graph.GetLength(0);
    var totalCols = graph.GetLength(1);

    var queue = new PriorityQueue<(int r, int c), int>();
    queue.Enqueue((0, 0), 0);
    var locked = new List<(int r, int c)>();
    var result = 0;
    
    while (queue.Count > 0)
    {
        queue.TryDequeue(out var pos, out var risk);
        
        if (pos.r == totalRows - 1 && pos.c == totalCols - 1)
        {
            result = risk;
            break;
        }

        if (locked.Contains(pos))
        {
            continue;
        }
        locked.Add(pos);
        
        foreach (var (r, c) in AdjacentNodes(pos.r, pos.c))
        {
            if (r >= 0 && r < totalRows && c >= 0 && c < totalCols)
            {
                queue.Enqueue((r, c), risk + graph[r, c]);
            }
        }
    }

    return result;
}

static void Part1()
{
    var input = ReadInput();
    var cTime = DateTime.Now;
    
    var result = CalculateRisk(input);
    var timestamp = DateTime.Now.Subtract(cTime).TotalSeconds;

    Console.WriteLine($"Part1: {result} in {timestamp} seconds");
}

static void Part2()
{
    var input = ReadInput();
    var cTime = DateTime.Now;

    var totalRows = input.GetLength(0);
    var totalCols = input.GetLength(1);
    
    var graph = new int[input.GetLength(0) * 5, input.GetLength(1) * 5];

    for (int row = 0; row < graph.GetLength(0); row++)
    {
        for (int col = 0; col < graph.GetLength(1); col++)
        {
            var originalValue = input[row % totalRows, col % totalCols];
            var newValue = originalValue + row / totalRows + col / totalCols;
            newValue = (newValue - 1) % 9 + 1; // between 1-9
            graph[row, col] = newValue;
        }
    }
    
    var result = CalculateRisk(graph);
    var timestamp = DateTime.Now.Subtract(cTime).TotalSeconds;

    Console.WriteLine($"Part2: {result} in {timestamp} seconds");
}


static int[,] ReadInput()
{
    var input = File.ReadLines("input.txt").ToList();
    var matrix = new int[input.Count, input[0].Length];

    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        for (var col = 0; col < matrix.GetLength(1); col++)
        {
            matrix[row, col] = input[row][col] - '0';
        }
    }

    return matrix;
}

Part1();
Part2();


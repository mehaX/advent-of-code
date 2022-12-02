
static void Part1()
{
    var heightMap = GetHeightMap();
    var lowestPoints = GetLowestPoints(heightMap);

    var result = lowestPoints.Select(pt => heightMap[pt.Item1, pt.Item2] + 1).Sum();

    Console.WriteLine($"Part1: {result}");
}

static void Part2()
{
    var heightMap = GetHeightMap();
    var lowestPoints = GetLowestPoints(heightMap);
    List<List<(int, int)>> basins = new();
    
    foreach (var lowestPoint in lowestPoints)
    {
        var basin = new List<(int, int)>();
        GetBasinFromLowestPoint(heightMap, lowestPoint.Item1, lowestPoint.Item2, ref basin);
        basins.Add(basin);
    }

    var result = basins.Select(b => b.Count).OrderByDescending(b => b).Take(3).Aggregate((a, b) => a * b);
    Console.WriteLine($"Part2: {result}");
}

static int[,] GetHeightMap()
{
    var input = File.ReadLines("input.txt").ToArray();
    var heightMap = new int[input.Length, input.First().Length];

    for (var row = 0; row < heightMap.GetLength(0); row++)
    {
        for (var col = 0; col < heightMap.GetLength(1); col++)
        {
            heightMap[row, col] = input[row][col] - '0';
        }
    }

    return heightMap;
}

static List<(int, int)> GetLowestPoints(int[,] heightMap)
{
    var lowestPoints = new List<(int, int)>();
    for (var row = 0; row < heightMap.GetLength(0); row++)
    {
        for (var col = 0; col < heightMap.GetLength(1); col++)
        {
            var point = LowestPointFromPosition(heightMap, row, col, new List<(int, int)>());
            if (!lowestPoints.Any(lp => lp.Item1 == point.Item1 && lp.Item2 == point.Item2))
            {
                lowestPoints.Add(point);
            }
        }
    }

    return lowestPoints;
}

static (int, int) LowestPointFromPosition(int[,] heightMap, int row, int col, List<(int,int)> track)
{
    var points = new List<(int, int)>();
    var currentValue = heightMap[row, col];
    
    points.Add((row, col));
    
    var newTrack = track.ToList();
    newTrack.Add((row, col));
    
    if (row > 0 && heightMap[row - 1, col] <= currentValue && !track.Any(t => t.Item1 == row - 1 && t.Item2 == col))
    {
        var newPoint = LowestPointFromPosition(heightMap, row - 1, col, newTrack);
        if (!points.Any(c => c.Item1 == newPoint.Item1 && c.Item2 == newPoint.Item2))
        {
            points.Add(newPoint);
        }
    }

    if (row < heightMap.GetLength(0) - 1 && heightMap[row + 1, col] <= currentValue && !track.Any(t => t.Item1 == row + 1 && t.Item2 == col))
    {
        var newPoint = LowestPointFromPosition(heightMap, row + 1, col, newTrack);
        if (!points.Any(c => c.Item1 == newPoint.Item1 && c.Item2 == newPoint.Item2))
        {
            points.Add(newPoint);
        }
    }
    if (col > 0 && heightMap[row, col - 1] <= currentValue && !track.Any(t => t.Item1 == row && t.Item2 == col - 1))
    {
        var newPoint = LowestPointFromPosition(heightMap, row, col - 1, newTrack);
        if (!points.Any(c => c.Item1 == newPoint.Item1 && c.Item2 == newPoint.Item2))
        {
            points.Add(newPoint);
        }
    }

    if (col < heightMap.GetLength(1) - 1 && heightMap[row, col + 1] <= currentValue && !track.Any(t => t.Item1 == row && t.Item2 == col + 1))
    {
        var newPoint = LowestPointFromPosition(heightMap, row, col + 1, newTrack);
        if (!points.Any(c => c.Item1 == newPoint.Item1 && c.Item2 == newPoint.Item2))
        {
            points.Add(newPoint);
        }
    }

    (int, int) lowestPoint = points.First();
    int lowestValue = 9;
    foreach (var point in points)
    {
        if (heightMap[point.Item1, point.Item2] < lowestValue)
        {
            lowestPoint = point;
            lowestValue = heightMap[point.Item1, point.Item2];
        }
    }

    return lowestPoint;
}

static void GetBasinFromLowestPoint(int[,] heightMap, int row, int col, ref List<(int, int)> track)
{
    if (heightMap[row, col] == 9 || track.Any(t => t.Item1 == row && t.Item2 == col))
    {
        return;
    }

    track.Add((row, col));
    
    if (row > 0 && heightMap[row - 1, col] < 9)
    {
        GetBasinFromLowestPoint(heightMap, row - 1, col, ref track);
    }

    if (row < heightMap.GetLength(0) - 1 && heightMap[row + 1, col] < 9)
    {
        GetBasinFromLowestPoint(heightMap, row + 1, col, ref track);
    }
    if (col > 0 && heightMap[row, col - 1] < 9)
    {
        GetBasinFromLowestPoint(heightMap, row, col - 1, ref track);
    }

    if (col < heightMap.GetLength(1) - 1 && heightMap[row, col + 1] < 9)
    {
        GetBasinFromLowestPoint(heightMap, row, col + 1, ref track);
    }
}

Part1();
Part2();

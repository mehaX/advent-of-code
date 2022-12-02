static void Part1(int totalIntervals)
{
    var matrix = GetMatrix();

    var count = 0;
    for (int interval = 0; interval < totalIntervals; interval++)
    {
        for (var row = 0; row < matrix.GetLength(0); row++)
        {
            for (var col = 0; col < matrix.GetLength(1); col++)
            {
                IncreaseValue(ref matrix, row, col);
            }
        }

        count += Flash(ref matrix);
    }

    Console.WriteLine($"Part1: {count}");
}
static void Part2()
{
    var matrix = GetMatrix();

    var simul = false;
    int interval;
    for (interval = 0; !simul; interval++)
    {
        for (var row = 0; row < matrix.GetLength(0); row++)
        {
            for (var col = 0; col < matrix.GetLength(1); col++)
            {
                IncreaseValue(ref matrix, row, col);
            }
        }

        var newCount = Flash(ref matrix);
        if (newCount == matrix.Length)
        {
            simul = true;
        }
    }

    Console.WriteLine($"Part2: {interval}");
}

static void IncreaseValue(ref int[,] matrix, int row, int col)
{
    if (matrix[row, col] == 10)
    {
        return;
    }
    
    matrix[row, col] = Math.Min(matrix[row, col] + 1, 10);
    if (matrix[row, col] == 10)
    {
        var adjacentPositions = GetAdjacentPositions(matrix, row, col);
        foreach (var adjacentPosition in adjacentPositions)
        {
            IncreaseValue(ref matrix, adjacentPosition.Item1, adjacentPosition.Item2);
        }
    }
}

static int Flash(ref int[,] matrix)
{
    var count = 0;
    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        for (var col = 0; col < matrix.GetLength(1); col++)
        {
            if (matrix[row, col] == 10)
            {
                matrix[row, col] = 0;
                count++;
            }
        }
    }

    return count;
}

static List<(int, int)> GetAdjacentPositions(int[,] matrix, int row, int col)
{
    List<(int, int)> points = new();

    var topCorner = row == 0;
    var bottomCorner = row == matrix.GetLength(0) - 1;
    var leftCorner = col == 0;
    var rightCorner = col == matrix.GetLength(1) - 1;

    if (!topCorner) points.Add((row - 1, col));
    if (!bottomCorner) points.Add((row + 1, col));
    if (!leftCorner) points.Add((row, col - 1));
    if (!rightCorner) points.Add((row, col + 1));

    if (!topCorner && !leftCorner) points.Add((row - 1, col - 1));
    if (!topCorner && !rightCorner) points.Add((row - 1, col + 1));
    if (!bottomCorner && !leftCorner) points.Add((row + 1, col - 1));
    if (!bottomCorner && !rightCorner) points.Add((row + 1, col + 1));

    return points;
}

static int[,] GetMatrix()
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

Part1(100);
Part2();
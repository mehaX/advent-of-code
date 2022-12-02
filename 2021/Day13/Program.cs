
static (int result, bool[,] matrix) Part(int? maxIntervals = int.MaxValue)
{
    GetInput(out var points, out var instructions);
    var matrix = GetMatrixFromPoints(points);

    var intervalCount = 0;
    foreach (var instruction in instructions)
    {
        SplitMatrix(ref matrix, out var flipMatrix, instruction);
        FlipMatrices(ref matrix, flipMatrix, instruction.Item1);
        if (++intervalCount >= maxIntervals)
        {
            break;
        }
    }

    return (CountPoints(matrix), matrix);
}

static void Part1()
{
    var result = Part(1).result;
    Console.WriteLine($"Part1 result: {result}");
}

static void Part2()
{
    var result = Part().matrix;

    Console.WriteLine("Part2 password:");
    for (var row = 0; row < result.GetLength(0); row++)
    {
        for (var col = 0; col < result.GetLength(1); col++)
        {
            Console.Write(result[row, col] ? '#' : ' ');
        }

        Console.WriteLine();
    }

    Console.WriteLine();
}

static void GetInput(out List<(int, int)> points, out List<(char, int)> instructions)
{
    var input = File.ReadAllText("input.txt").Split("\n\n");

    points = input[0].Split("\n").Select(row =>
    {
        var coordinates = row.Split(",").Select(c => Convert.ToInt32(c)).ToArray();
        return (coordinates[0], coordinates[1]);
    }).ToList();

    instructions = input[1].Split("\n").Select(row =>
    {
        var segments = row.Replace("fold along ", "").Split("=");
        return (segments[0][0], Convert.ToInt32(segments[1]));
    }).ToList();
}

static bool[,] GetMatrixFromPoints(List<(int, int)> points)
{
    var maxRow = 0;
    var maxCol = 0;
    foreach (var point in points)
    {
        if (point.Item2 > maxRow)
        {
            maxRow = point.Item2;
        }

        if (point.Item1 > maxCol)
        {
            maxCol = point.Item1;
        }
    }

    maxRow++;
    maxCol++;

    var matrix = new bool[maxRow, maxCol];
    foreach (var point in points)
    {
        matrix[point.Item2, point.Item1] = true;
    }

    return matrix;
}

static void SplitMatrix(ref bool[,] matrix, out bool[,] flipMatrix, (char, int) instruction)
{
    int maxRow = matrix.GetLength(0);
    int maxCol = matrix.GetLength(1);
    int offsetRow = 0;
    int offsetCol = 0;

    if (instruction.Item1 == 'x') // horizontal line
    {
        maxCol = instruction.Item2;
        flipMatrix = new bool[matrix.GetLength(0), matrix.GetLength(1) - instruction.Item2 - 1];
        offsetCol = instruction.Item2 + 1;
    }
    else // vertical line
    {
        maxRow = instruction.Item2;
        flipMatrix = new bool[matrix.GetLength(0) - instruction.Item2 - 1, matrix.GetLength(1)];
        offsetRow = instruction.Item2 + 1;
    }

    var newMatrix = new bool[maxRow, maxCol];
    for (var row = 0; row < newMatrix.GetLength(0); row++)
    {
        for (var col = 0; col < newMatrix.GetLength(1); col++)
        {
            newMatrix[row, col] = matrix[row, col];
        }
    }
    
    for (var row = 0; row < flipMatrix.GetLength(0); row++)
    {
        for (var col = 0; col < flipMatrix.GetLength(1); col++)
        {
            flipMatrix[row, col] = matrix[row + offsetRow, col + offsetCol];
        }
    }

    matrix = newMatrix;
}

static void FlipMatrices(ref bool[,] matrix, bool[,] flipMatrix, char axis)
{
    for (var row = 0; row < flipMatrix.GetLength(0); row++)
    {
        for (var col = 0; col < flipMatrix.GetLength(1); col++)
        {
            var newRow = axis == 'y' ? matrix.GetLength(0) - row - 1 : row;
            var newCol = axis == 'x' ? matrix.GetLength(1) - col - 1 : col;

            matrix[newRow, newCol] = matrix[newRow, newCol] || flipMatrix[row, col];
        }
    }
}

static int CountPoints(bool[,] matrix)
{
    var count = 0;
    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        for (var col = 0; col < matrix.GetLength(1); col++)
        {
            if (matrix[row, col])
            {
                count++;
            }
        }
    }

    return count;
}

Part1();
Part2();
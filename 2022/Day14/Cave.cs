using System.Text;

namespace Day14;

internal class Cave
{
    private readonly bool mDebug;
    private readonly Dictionary<Point, PointType> mMatrix = new();
    private Point? mSandUnit = null;
    public int IntoAbyssCount { get; private set; } = 0;

    private Point mSandSourcePoint => mMatrix.FirstOrDefault(kv => kv.Value == PointType.SandSource).Key;
    public bool IsBlocked { get; private set; } = false;
    public int SandCount { get; private set; } = 0;
    private bool mHasFloor = false;

    public Cave(List<Point> rocks, bool hasFloor, bool debug = false)
    {
        mHasFloor = hasFloor;
        mDebug = debug;
        var sandSourcePoint = new Point(500, 0);
        var minX = rocks.Select(s => s.X).Min();
        var maxX = rocks.Select(s => s.X).Max();
        var minY = rocks.Concat(new[] { sandSourcePoint }).Select(s => s.Y).Min();
        var maxY = rocks.Concat(new[] { sandSourcePoint }).Select(s => s.Y).Max();

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var point = new Point(x, y);
                var pointType = PointType.Air;
                if (point == sandSourcePoint) // Sand source
                {
                    pointType = PointType.SandSource;
                }
                else if (rocks.Contains(point))
                {
                    pointType = PointType.Rock;
                }

                if (pointType != PointType.Air)
                {
                    mMatrix.Add(point, pointType);
                }
            }
        }
    }


    public void Step()
    {
        if (mDebug) Console.Write("Status: ");
        if (mSandUnit == null)
        {
            mSandUnit = new Point(mSandSourcePoint.X, mSandSourcePoint.Y);
            if (mDebug) Console.WriteLine("new sand");
            return;
        }

        switch (FallSand())
        {
            case 0:
                if (mDebug) Console.WriteLine("rest");
                SandCount++;
                if (mSandSourcePoint == mSandUnit)
                {
                    IsBlocked = true;
                }
                mMatrix.TryAdd(mSandUnit, PointType.Sand);
                mSandUnit = null;
                break;
            
            case 2:
                if (mDebug) Console.WriteLine("fell into abyss");
                mSandUnit = null;
                IntoAbyssCount++;
                break;
            
            default:
                if (mDebug) Console.WriteLine("falling down");
                break;
        }
    }

    private int mMinX => mMatrix.Keys.Select(pt => pt.X).Min();
    private int mMaxX => mMatrix.Keys.Select(pt => pt.X).Max();
    private int mMinY => mMatrix.Keys.Select(pt => pt.Y).Min();
    private int mMaxY => mMatrix
        .Where(kv => kv.Value == PointType.Rock)
        .Select(kv => kv.Key.Y)
        .Max() + (mHasFloor ? 2 : 0);

    public int FallSand()
    {
        var downPoint = new Point(mSandUnit.X, mSandUnit.Y + 1);
        var leftPoint = new Point(mSandUnit.X - 1, mSandUnit.Y + 1);
        var rightPoint = new Point(mSandUnit.X + 1, mSandUnit.Y + 1);

        if (mHasFloor && downPoint.Y + 1 == mMaxY)
        {
            if (!mMatrix.ContainsKey(downPoint) || mMatrix[downPoint] == PointType.Air)
            {
                mSandUnit = downPoint;
                return 0;
            }

            if (!mMatrix.ContainsKey(leftPoint) || mMatrix[leftPoint] == PointType.Air)
            {
                mSandUnit = leftPoint;
                return 0;
            }

            if (!mMatrix.ContainsKey(rightPoint) || mMatrix[rightPoint] == PointType.Air)
            {
                mSandUnit = rightPoint;
                return 0;
            }

            return 0;
        }

        if (downPoint.Y > mMaxY)
        {
            return 2; // fell into abyss
        }

        if (!mMatrix.ContainsKey(downPoint) || mMatrix[downPoint] == PointType.Air) // Can move down
        {
            mSandUnit = downPoint;
            return 1;
        }

        if (!mMatrix.ContainsKey(leftPoint) || mMatrix[leftPoint] == PointType.Air) // Can move down-left
        {
            mSandUnit = leftPoint;
            return 1;
        }

        if (!mMatrix.ContainsKey(rightPoint) || mMatrix[rightPoint] == PointType.Air) // Can move down-right
        {
            mSandUnit = rightPoint;
            return 1;
        }

        return 0; // rest
    }

    public void Print()
    {
        Console.WriteLine($"Sand count: {SandCount}");
        Console.WriteLine($"Fell into abyss count: {IntoAbyssCount}");
        Console.WriteLine();
        
        for (var y = mMinY; y <= mMaxY; y++)
        {
            for (var x = mMinX - 10; x <= mMaxX + 10; x++)
            {
                var pt = new Point(x, y);
                if (mSandUnit != null && mSandUnit == pt)
                {
                    Console.Write("o");
                    continue;
                }

                mMatrix.TryGetValue(pt, out var pointType);
                Console.Write(pointType switch
                {
                    PointType.SandSource => '+',
                    PointType.Rock => '#',
                    PointType.Sand => 'o',
                    _ => '.', // air
                });
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public void WriteOutput(string file)
    {
        var output = new StringBuilder();
        output.AppendLine($"Sand count: {SandCount}");
        output.AppendLine($"Fell into abyss count: {IntoAbyssCount}");
        output.AppendLine();
        
        for (var y = mMinY; y <= mMaxY; y++)
        {
            for (var x = mMinX - 10; x <= mMaxX + 10; x++)
            {
                var pt = new Point(x, y);
                if (mSandUnit != null && mSandUnit == pt)
                {
                    output.Append("o");
                    continue;
                }

                mMatrix.TryGetValue(pt, out var pointType);
                output.Append(pointType switch
                {
                    PointType.SandSource => '+',
                    PointType.Rock => '#',
                    PointType.Sand => 'o',
                    _ => '.', // air
                });
            }

            output.AppendLine();
        }

        output.AppendLine();
        
        File.WriteAllText(file, output.ToString());
    }
}
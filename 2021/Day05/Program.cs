
static void Part1()
{
    var input = File.ReadLines("input.txt");
    var lines = input.Select(row => new Line(row))
        .Where(l => l.A.X == l.B.X || l.A.Y == l.B.Y).ToList();

    var count = CalculateCount(lines);

    Console.WriteLine($"Part1: {count}");
}
static void Part2()
{
    var input = File.ReadLines("input.txt");
    var lines = input.Select(row => new Line(row)).ToList();

    var count = CalculateCount(lines);

    Console.WriteLine($"Part2: {count}");
}

Part1();
Part2();

static int CalculateCount(List<Line> lines)
{
    var countPoints = new Dictionary<(int, int), int>();

    var count = 0;
    foreach (var line in lines)
    {
        var points = line.GeneratePoints();
        foreach (var point in points)
        {
            if (countPoints.ContainsKey((point.X, point.Y)))
            {
                var newValue =++countPoints[(point.X, point.Y)];
                if (newValue == 2)
                {
                    count++;
                }
            }
            else
            {
                countPoints.Add((point.X, point.Y), 1);
            }
        }
    }

    return count;
}

class Point
{
    public int X { get; }
    public int Y { get; }
    public int Count { get; set; } = 0;

    public Point(string input)
    {
        var parts = input.Split(",");
        X = Convert.ToInt32(parts[0]);
        Y = Convert.ToInt32(parts[1]);
    }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Line
{
    public Point A { get; }
    public Point B { get; }

    public Line(string input)
    {
        var segments = input.Split(" -> ");
        A = new Point(segments[0]);
        B = new Point(segments[1]);
    }

    public List<Point> GeneratePoints()
    {
        var result = new List<Point>();
        var increaseX = Math.Sign(B.X - A.X);
        var increaseY = Math.Sign(B.Y - A.Y);
        
        var nextPoint = new Point(A.X, A.Y);
        while (nextPoint.X != B.X || nextPoint.Y != B.Y)
        {
            result.Add(nextPoint);
            nextPoint = new Point(nextPoint.X + increaseX, nextPoint.Y + increaseY);
        }
        result.Add(B);

        return result;
    }
}

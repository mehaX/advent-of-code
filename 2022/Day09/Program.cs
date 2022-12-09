Console.WriteLine("Part 1: " + Part(2));
Console.WriteLine("Part 2: " + Part(10));

int Part(int nrOfKnots)
{
    var knots = new List<Knot>();
    for (var i = 0; i < nrOfKnots; i++)
    {
        knots.Add(new Knot(0, 0));
    }
    var lastKnotPositions = new List<Knot>();

    GetInput().ForEach(motion => ExecuteMotion(knots, motion, lastKnotPositions));

    return lastKnotPositions.Count;
}

void ExecuteMotion(IList<Knot> knots, Motion motion, IList<Knot> lastKnotPositions)
{
    for (var count = 1; count <= motion.Count; count++)
    {
        MoveHead(knots, motion.Direction);
        for (var knotIndex = 1; knotIndex < knots.Count; knotIndex++)
        {
            FollowKnot(knots, knotIndex);
        }

        var lastKnot = knots.Last();
        if (!lastKnotPositions.Contains(lastKnot))
        {
            lastKnotPositions.Add(lastKnot);
        }
    }
}

void MoveHead(IList<Knot> knots, Direction direction)
{
    var head = knots[0];
    var offsetX = direction switch
    {
        Direction.Left => -1,
        Direction.Right => 1,
        _ => 0,
    };
    
    var offsetY = direction switch
    {
        Direction.Up => 1,
        Direction.Down => -1,
        _ => 0,
    };

    knots[0] = new Knot(head.X + offsetX, head.Y + offsetY);
}

void FollowKnot(IList<Knot> knots, int knotIndex)
{
    var knot = knots[knotIndex];
    var prevKnot = knots[knotIndex - 1];
    
    if (Distance(knot, prevKnot) <= 1)
    {
        return;
    }

    var newX = knot.X;
    if (prevKnot.X < knot.X)
    {
        newX--;
    }
    else if (prevKnot.X > knot.X)
    {
        newX++;
    }

    var newY = knot.Y;
    if (prevKnot.Y < knot.Y)
    {
        newY--;
    }
    else if (prevKnot.Y > knot.Y)
    {
        newY++;
    }

    knots[knotIndex] = new Knot(newX, newY);
}

int Distance(Knot c1, Knot c2)
{
    var dist = Math.Sqrt(Math.Pow(c2.Y - c1.Y, 2) + Math.Pow(c2.X - c1.X, 2));
    return (int)Math.Round(dist, 0);
}

List<Motion> GetInput()
{
    return File.ReadAllLines("input.txt")
        .Select(row => new Motion(
            (Direction)row[0],
            int.Parse(row.Split(" ")[1])))
        .ToList();
}

record Knot(int X, int Y);

record Motion(Direction Direction, int Count);

enum Direction
{
    Up = 'U',
    Down = 'D',
    Left = 'L',
    Right = 'R',
}
namespace Day09;

internal static class Extensions
{
    public static void MoveHead(this IList<Knot> knots, Direction direction)
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

    public static void FollowNextKnots(this IList<Knot> knots)
    {
        for (var knotIndex = 1; knotIndex < knots.Count; knotIndex++)
        {
            knots.FollowKnot(knotIndex);
        }
    }

    public static void FollowKnot(this IList<Knot> knots, int knotIndex)
    {
        var knot = knots[knotIndex];
        var prevKnot = knots[knotIndex - 1];
    
        if (knot.Distance(prevKnot) <= 1)
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

    public static int Distance(this Knot source, Knot target)
    {
        var dist = Math.Sqrt(Math.Pow(target.Y - source.Y, 2) + Math.Pow(target.X - source.X, 2));
        return (int)Math.Round(dist, 0);
    }

    public static void RegisterKnotPosition(this IList<Knot> knotPositions, Knot knot)
    {
        if (!knotPositions.Contains(knot))
        {
            knotPositions.Add(knot);
        }
    }
}
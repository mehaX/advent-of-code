using Day09;

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
        knots.MoveHead(motion.Direction);
        knots.FollowNextKnots();
        lastKnotPositions.RegisterKnotPosition(knots.Last());
    }
}

List<Motion> GetInput()
{
    return File.ReadAllLines("input.txt")
        .Select(row => new Motion(
            (Direction)row[0],
            int.Parse(row.Split(" ")[1])))
        .ToList();
}
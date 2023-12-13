using Day14;

// Debug();

Console.WriteLine("Part 1: " + Part1());
Console.WriteLine("Part 2: " + Part2());


int Part1()
{
    var rocks = GetInput();
    var cave = new Cave(rocks, false);
    
    while (cave.IntoAbyssCount == 0)
    {
        cave.Step();
    }

    return cave.SandCount;
}

int Part2()
{
    var rocks = GetInput();
    var cave = new Cave(rocks, true);

    var count = 0;
    while (cave is { IntoAbyssCount: 0, IsBlocked: false })
    {
        if (count % 10 == 0)
        {
            cave.WriteOutput("output.txt");
        }
        
        cave.Step();
        count++;
    }

    return cave.SandCount;
}

void Debug()
{
    var rocks = GetInput();
    var cave = new Cave(rocks, true, true);
    
    while (cave.IsBlocked == false)
    {
        Console.Clear();
        cave.Step();
        if (cave.SandCount < 23)
        {
            continue;
        }
        
        cave.Print();
        // Console.Read();
    }
    cave.Print();
}

List<Point> GetInput(bool addFloor = false)
{
    var lines = File.ReadAllLines("input.txt");
    var sandSourcePoint = new Point(500, 0);
    var rocks = new List<Point>();
    foreach (var line in lines)
    {
        var points = line
            .Split(" -> ")
            .Select(strPoint => strPoint.Split(',').Select(int.Parse))
            .Select(coordinate => new Point(coordinate.ElementAt(0), coordinate.ElementAt(1)))
            .ToList();

        for (var i = 0; i < points.Count - 1; i++)
        {
            var pair = points.Skip(i).Take(2);
            var minX = pair.Select(p => p.X).Min();
            var maxX = pair.Select(p => p.X).Max();
            var minY = pair.Select(p => p.Y).Min();
            var maxY = pair.Select(p => p.Y).Max();

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    rocks.Add(new Point(x, y));
                }
            }
        }
    }

    if (addFloor)
    {
        var height = rocks.Select(p => p.Y).Max() - rocks.Select(p => p.Y).Min() + 3;
        var width = height * 4;
        var offsetY = rocks.Select(p => p.Y).Max() + 2;
        var offsetX = sandSourcePoint.X - width / 2;

        for (int i = 0; i < width; i++)
        {
            rocks.Add(new Point(offsetX + i, offsetY));
        }
    }

    return rocks;
}

enum PointType
{
    Air, // .
    Rock, // #
    SandSource, // +
    Sand, // o
}

record Point(int X, int Y);

Console.WriteLine("Part 1: " + Part1());
Console.WriteLine("Part 2: " + Part2());

int Part1()
{
    var map = GetInput();
    var y = 10;
    
    var allPositions = map.AllPositions.ToList();
    
    return map.Sensors
        .SelectMany(s => s.ScannedPositionsAtY(y))
        .Distinct()
        .Count(p => !allPositions.Contains(p));
}

long Part2()
{
    var maxValue = 4_000_000L;
    var map = GetInput();
    var offsets = new[] { -1, 1 };

    foreach (var sensor in map.Sensors)
    {
        foreach (var xOffset in offsets)
        {
            foreach (var yOffset in offsets)
            {
                for (var dx = 0; dx <= sensor.Distance + 1; dx++)
                {
                    var dy = sensor.Distance + 1 - dx;
                    var x = sensor.Position.X + dx * xOffset;
                    var y = sensor.Position.Y + dy * yOffset;

                    if (x >= 0 && x <= maxValue && y >= 0 && y <= maxValue && map.IsOutOfRange(x, y))
                    {
                        return x * maxValue + y;
                    }
                }
            }
        }
    }

    return 0;
}

Map GetInput()
{
    var map = new Map();

    var strInput = File.ReadAllLines("input.txt");
    foreach (var row in strInput)
    {
        var cols = row.Split(' ');
        var positions = new[]
            {
                cols[2].Replace("x=", "").Replace(",", ""),
                cols[3].Replace("y=", "").Replace(":", ""),
                cols[8].Replace("x=", "").Replace(",", ""),
                cols[9].Replace("y=", ""),
            }
            .Select(int.Parse)
            .Chunk(2)
            .Select(coordinate => new Position(coordinate[0], coordinate[1]))
            .ToArray();

        var beacon = map.Beacons.FirstOrDefault(b => b.Position == positions[1]);
        if (beacon == null)
        {
            beacon = new Beacon(positions[1]);
            map.Beacons.Add(beacon);
        }

        var sensor = new Sensor(positions[0], beacon);
        map.Sensors.Add(sensor);
    }

    return map;
}

record Position(int X, int Y)
{
    public int Distance(Position p)
    {
        return Math.Abs(p.Y - Y) + Math.Abs(p.X - X);
    }

    public int Distance(int x, int y)
    {
        return Math.Abs(y - Y) + Math.Abs(x - X);
    }
}

class Beacon
{
    public Position Position { get; set; }

    public Beacon(Position position)
    {
        Position = position;
    }
}

class Sensor
{
    public Position Position { get; set; }
    public Beacon ClosestBeacon { get; set; }

    public Sensor(Position position, Beacon closestBeacon)
    {
        Position = position;
        ClosestBeacon = closestBeacon;
    }

    public int Distance => Position.Distance(ClosestBeacon.Position);
    
    public int MinX => Position.X - Distance;
    public int MaxX => Position.X + Distance;
    public int MinY => Position.Y - Distance;
    public int MaxY => Position.Y + Distance;


    public IEnumerable<Position> ScannedPositionsAtY(int y)
    {
        var offset = Math.Abs(Position.Y - y);
        if (offset > Distance) // Not in radius
        {
            yield break;
        }

        for (var x = MinX + offset; x <= MaxX - offset; x++)
        {
            yield return new Position(x, y);
        }
    }

    public IEnumerable<Position> ScannedPositions(int min = int.MinValue, int max = int.MaxValue)
    {
        for (var offset = 0; offset <= Distance; offset++)
        {
            for (var x = MinX + offset; x <= MaxX - offset; x++)
            {
                if (x >= min && x <= max && Position.Y - offset >= 0 && Position.Y - offset <= max)
                {
                    yield return new Position(x, Position.Y - offset);
                }
                if (offset > 0 && x >= min && x <= max && Position.Y + offset >= 0 && Position.Y + offset <= max)
                {
                    yield return new Position(x, Position.Y + offset);
                }
            }
        }
    }
}

class Map
{
    public List<Sensor> Sensors { get; set; }
    public List<Beacon> Beacons { get; set; }

    public IEnumerable<Position> AllPositions =>
        Sensors.Select(s => s.Position).Concat(Beacons.Select(b => b.Position));

    public Map()
    {
        Sensors = new();
        Beacons = new();
    }

    public bool IsOutOfRange(int x, int y)
    {
        return Sensors.All(s => s.Position.Distance(x, y) > s.Distance);
    }

    public void Print()
    {
        var scannedPositions = Sensors.SelectMany(s => s.ScannedPositions()).Distinct().ToList();
        var allPositions = Sensors.Select(s => s.Position).ToList();

        var minX = allPositions.Select(p => p.X).Min();
        var maxX = allPositions.Select(p => p.X).Max();
        var minY = allPositions.Select(p => p.Y).Min();
        var maxY = allPositions.Select(p => p.Y).Max();

        Console.Write("   ");
        for (var x = minX; x <= maxX; x++)
        {
            if (x % 5 == 0)
            {
                if (x < 0)
                {
                    Console.Write('-');
                    continue;
                }

                if (x >= 10)
                {
                    Console.Write(x.ToString().First());
                    continue;
                }
            }

            Console.Write(' ');
        }

        Console.WriteLine();

        Console.Write("   ");
        for (var x = minX; x <= maxX; x++)
        {
            if (x % 5 == 0)
            {
                Console.Write(x.ToString().Last());
                continue;
            }

            Console.Write(' ');
        }

        Console.WriteLine();

        var sensorPositions = Sensors.Select(s => s.Position).ToList();
        var beaconPositions = Beacons.Select(b => b.Position).ToList();
        for (var y = minY; y <= maxY; y++)
        {
            Console.Write((y < 10 && y >= 0 ? " " : "") + y + " ");
            for (var x = minX; x <= maxX; x++)
            {
                var pos = new Position(x, y);

                if (sensorPositions.Contains(pos))
                {
                    Console.Write("S");
                    continue;
                }

                if (beaconPositions.Contains(pos))
                {
                    Console.Write("B");
                    continue;
                }

                if (scannedPositions.Contains(pos))
                {
                    Console.Write("#");
                    continue;
                }

                Console.Write(".");
            }

            Console.WriteLine();
        }
    }
}
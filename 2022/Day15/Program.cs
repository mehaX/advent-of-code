Console.WriteLine("Part 1: " + Part1());

int Part1()
{
    var map = GetInput();
    var y = 10;
    map.Print();
    var allPositions = map.AllPositions;

    return map.Sensors
        .SelectMany(s => s.ScannedPositionsAtY(y))
        .Distinct()
        .Count(p => !allPositions.Contains(p));
}

Map GetInput()
{
    var map = new Map();

    var strInput = File.ReadAllLines("input.txt");
    foreach (var row in strInput)
    {
        var cols = row.Split(' ');
        var positions = new string[]
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
        return (int)Math.Floor(Math.Sqrt(Math.Pow(p.Y - Y, 2) + Math.Pow(p.X - X, 2)));
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

    public int Radius => Math.Abs(ClosestBeacon.Position.X - Position.X) + Math.Abs(ClosestBeacon.Position.Y - Position.Y);
    
    public int MinX => Position.X - Radius;
    public int MaxX => Position.X + Radius;
    public int MinY => Position.Y - Radius;
    public int MaxY => Position.Y + Radius;


    public List<Position> ScannedPositionsAtY(int y)
    {
        var positions = new List<Position>();

        var offset = Math.Abs(Position.Y - y);
        if (offset > Radius) // Not in radius
        {
            return positions;
        }

        for (var x = MinX + offset; x <= MaxX - offset; x++)
        {
            positions.Add(new Position(x, y));
        }

        return positions;
    }

    public List<Position> ScannedPositions()
    {
        var positions = new List<Position>();

        for (var offset = 0; offset <= Radius; offset++)
        {
            for (var x = MinX + offset; x <= MaxX - offset; x++)
            {
                positions.Add(new Position(x, Position.Y - offset));
                if (offset > 0)
                {
                    positions.Add(new Position(x, Position.Y + offset));
                }
            }
        }


        return positions;
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

    public void Print()
    {
        var scannedPositions = Sensors.SelectMany(s => s.ScannedPositions());
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
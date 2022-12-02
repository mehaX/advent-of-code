
static void Part1()
{
    var input = File.ReadAllText("input.txt").Split(",");
    var crabs = new List<Crab>();

    foreach (var number in input)
    {
        crabs.Add(new Crab(Convert.ToInt32(number)));
    }
    
    foreach (var crab1 in crabs)
    {
        foreach (var crab2 in crabs)
        {
            var distance = crab2.CalcDistance(crab1);
            crab1.AddFuel(distance);
        }
    }

    var bestPosition = crabs.OrderBy(pos => pos.TotalFuel).First();

    Console.WriteLine($"Part1: Crab {bestPosition.HPosition} with total fuel {bestPosition.TotalFuel}");
}

static void Part2()
{
    var input = File.ReadAllText("input.txt").Split(",");
    var crabs = new List<Crab>();

    foreach (var number in input)
    {
        crabs.Add(new Crab(Convert.ToInt32(number)));
    }
    
    foreach (var crab1 in crabs)
    {
        foreach (var crab2 in crabs)
        {
            var distance = crab2.CalcFuel(crab1);
            crab1.AddFuel(distance);
        }
    }

    var bestPosition = crabs.OrderBy(pos => pos.TotalFuel).First();

    Console.WriteLine($"Part2: Crab {bestPosition.HPosition} with total fuel {bestPosition.TotalFuel}");
}

Part1();
Part2();

class Crab
{
    public int HPosition { get; }
    public int TotalFuel { get; private set; }

    public Crab(int hPosition)
    {
        HPosition = hPosition;
        TotalFuel = 0;
    }

    public void AddFuel(int fuel)
    {
        TotalFuel += fuel;
    }

    public int CalcDistance(Crab crab)
    {
        return Math.Abs(crab.HPosition - this.HPosition);
    }

    public int CalcFuel(Crab crab)
    {
        var n = CalcDistance(crab);
        var a = 1;
        var l = n;
        return n * (a + l) / 2;
    }
}


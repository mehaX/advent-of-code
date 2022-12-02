
static void Part1()
{
    var commands = File.ReadLines("input.txt");

    int h = 0;
    int d = 0;

    foreach (var command in commands)
    {
        var segments = command.Split(" ");
        var c = segments[0];
        var v = Convert.ToInt32(segments[1]);

        switch (c)
        {
            case "forward":
                h += v;
                break;
            
            case "up":
                d -= v;
                break;
            
            case "down":
                d += v;
                break;
        }
    }

    var result = h * d;

    Console.WriteLine($"Result for part 1 {result}");
}

static void Part2()
{
    var commands = File.ReadLines("input.txt");

    int h = 0;
    int d = 0;
    int a = 0;

    foreach (var command in commands)
    {
        var segments = command.Split(" ");
        var c = segments[0];
        var v = Convert.ToInt32(segments[1]);

        switch (c)
        {
            case "forward":
                h += v;
                d += (v * a);
                break;
            
            case "up":
                a -= v;
                break;
            
            case "down":
                a += v;
                break;
        }
    }

    var result = h * d;

    Console.WriteLine($"Result for part 2 {result}");
}

Part1();
Part2();
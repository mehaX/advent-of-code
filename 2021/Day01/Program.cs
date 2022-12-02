static void Part1()
{
    var input = File.ReadLines("input.txt").ToList();

    int? prevNumber = null;
    var count = 0;
    foreach(var measurement in input)
    {
        var nextNumber = Convert.ToInt32(measurement);
        if (prevNumber != null && nextNumber > prevNumber)
        {
            count++;
        }

        prevNumber = nextNumber;
    }

    Console.WriteLine($"Day 1, part 1: {count}");
}

static void Part2()
{
    var input = File.ReadLines("input.txt").ToList();

    int? prevSum = null;
    var count = 0;
    for (var index = 0; index < input.Count; index++)
    {
        var nextSum = input.Skip(index).Take(3).Select(m => Convert.ToInt32(m)).Sum();
        if (prevSum != null && nextSum > prevSum)
        {
            count++;
        }

        prevSum = nextSum;
    }

    Console.WriteLine($"Day 1, part 2: {count}");
}

Part1();
Part2();
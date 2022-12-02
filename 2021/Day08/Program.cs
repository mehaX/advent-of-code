
static void Part1()
{
    var input = File.ReadLines("input.txt");
    var outputCount = 0;
    var filteredLengths = new[] { 2, 4, 3, 7 }; // only for 1, 4, 7, 8

    foreach (var row in input)
    {
        var output = row.Split(" | ")[1];
        foreach (var number in output.Split(" "))
        {
            if (filteredLengths.Contains(number.Length))
            {
                outputCount++;
            }
        }
    }

    Console.WriteLine($"Part1 result: {outputCount}");
}

static void Part2()
{
    var input = File.ReadLines("input.txt");

    var result = 0;
    foreach (var row in input)
    {
        var numbers = new Dictionary<int, string>();
        var parts = row.Split(" | ");
        var combinations = parts[0].Split(" ").ToList();
        var outputs = parts[1].Trim().Split(" ");
        
        numbers.Add(1, combinations.First(c => c.Length == 2));
        combinations.Remove(numbers[1]);
        numbers.Add(4, combinations.First(c => c.Length == 4));
        combinations.Remove(numbers[4]);
        numbers.Add(7, combinations.First(c => c.Length == 3));
        combinations.Remove(numbers[7]);
        numbers.Add(8, combinations.First(c => c.Length == 7));
        combinations.Remove(numbers[8]);
        numbers.Add(3, combinations.First(c => c.Length == 5 && DistinctSegments(c, numbers[7]).Length == 2));
        combinations.Remove(numbers[3]);
        numbers.Add(9, combinations.First(c => c.Length == 6 && ContainsSegment(c, numbers[3] + numbers[4])));
        combinations.Remove(numbers[9]);
        numbers.Add(6, combinations.First(c => c.Length == 6 && DistinctSegments(c, numbers[1]).Length == 5));
        combinations.Remove(numbers[6]);
        numbers.Add(0, combinations.First(c => c.Length == 6));
        combinations.Remove(numbers[0]);
        numbers.Add(5, combinations.First(c => c.Length == 5 && ContainsSegment(c, numbers[6])));
        combinations.Remove(numbers[5]);
        numbers.Add(2, combinations.First(c => c.Length == 5));
        combinations.Remove(numbers[2]);

        var output = outputs.Select(o => numbers.First(n => InnerContains(o, n.Value)).Key.ToString()).ToArray();
        var number = Convert.ToInt32(string.Join("", output));
        result += number;
    }

    Console.WriteLine($"Part1 result: {result}");
}

Part1();
Part2();

static bool ContainsSegment(string n1, string n2)
{
    foreach (var c in n1)
    {
        if (!n2.Contains(c))
        {
            return false;
        }
    }

    return true;
}

static bool InnerContains(string n1, string n2)
{
    return ContainsSegment(n1, n2) && ContainsSegment(n2, n1);
}

static string DistinctSegments(string n1, string n2)
{
    var s = "";
    foreach (var c in n1)
    {
        if (!n2.Contains(c))
        {
            s += c;
        }
    }

    return s;
}


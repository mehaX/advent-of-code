
static void Part1()
{
    var input = File.ReadLines("input.txt");
    var strGamma = "";
    var strEpsilon = "";

    var length = input.First().Length;
    for (var index = 0; index < length; index++)
    {
        var counts = new[] { 0, 1 }
            .Select(bin => input.Select(line => line[index] - '0').Count(b => b == bin))
            .ToArray();

        strGamma += counts[0] > counts[1] ? '0' : '1';
        strEpsilon += counts[0] > counts[1] ? '1' : '0';
    }

    var gamma = Convert.ToInt32(strGamma, 2);
    var epsilon = Convert.ToInt32(strEpsilon, 2);
    var result = gamma * epsilon;

    Console.WriteLine($"Part1 result: {result}");
}

static string[] Part2Calculator(string[] input, bool most = true)
{
    var length = input.First().Length;
    
    for (var index = 0; index < length && input.Length > 1; index++)
    {
        var counts = new[] { 0, 1 }
            .Select(bin => input.Select(line => line[index] - '0').Count(b => b == bin))
            .ToArray();

        input = input.Where(line => line[index] == ((counts[0] > counts[1] && most) || (counts[0] <= counts[1] && !most) ? '0' : '1')).ToArray();
    }

    return input;
}

static void Part2()
{
    var input = File.ReadLines("input.txt");
    
    var arrOxygenRating = input.ToArray();
    while (arrOxygenRating.Length > 1)
    {
        arrOxygenRating = Part2Calculator(arrOxygenRating);
    }
    var arrCO2Rating = input.ToArray();
    while (arrCO2Rating.Length > 1)
    {
        arrCO2Rating = Part2Calculator(arrCO2Rating, false);
    }

    var oxygenRating = Convert.ToInt32(string.Join("", arrOxygenRating.First()), 2);
    var co2Rating = Convert.ToInt32(string.Join("", arrCO2Rating.First()), 2);

    var result = oxygenRating * co2Rating;
    Console.WriteLine($"Part2 result: {result}");
}

Part1();
Part2();
using Day16;

var inputHex = File.ReadAllText("input.txt");
var inputBin = string.Join("", inputHex.ToCharArray().Select(c =>
{
    var number = Convert.ToInt32(c.ToString(), 16);
    var result = Convert.ToString(number, 2);
    if (number < 8)
    {
        result = "0" + result;
    }

    if (number < 4)
    {
        result = "0" + result;
    }

    if (number < 2)
    {
        result = "0" + result;
    }
    return result;
}));
    

var rootPackage = PackageParser.Parse(ref inputBin);
var part1 = rootPackage.CalculateVersionSum();
var part2 = rootPackage.CalculateResult();
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
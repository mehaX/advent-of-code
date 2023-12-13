var input = File.ReadAllLines("input.txt")
    .Select(row => row.Split(": ")[1])
    .Select(row => row.Split(" | ").ToArray())
    .Select(cols => cols.Select(col => col.Split(" ").Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList()).ToArray())
    .ToList();

int Part1()
{
    var result = 0;

    foreach (var row in input)
    {
        var left = row[0];
        var right = row[1];

        result += (int)Math.Pow(2, left.Intersect(right).Count() - 1);
    }

    return result;
}

int Part2()
{
    var result = 0;

    for (var i = 0; i < input.Count; i++)
    {
        result += Rec(i);
    }

    return result;
}

int Rec(int index)
{
    var result = 1;
    var row = input[index];
    var left = row[0];
    var right = row[1];

    var count = left.Intersect(right).Count();
    var c = 0;
    for (var j = index + 1; j < input.Count && c < count; j++)
    {
        result += Rec(j);
        c++;
    }

    return result;
}

Console.WriteLine("Part1 " + Part1());
Console.WriteLine("Part2 " + Part2());


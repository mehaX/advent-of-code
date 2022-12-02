using Day18;

var origInputs = File.ReadLines("input.txt").Select(Deserializer.DeserializePair);

var inputs = origInputs.ToList();
var output = Aggregate(inputs);
var magnitude = Actions.CalculateMagnitude(output);
Console.WriteLine($"Part1: {magnitude}");

inputs = origInputs.ToList();
var highestMagnitude = 0;
for (int i = 0; i < inputs.Count; i++)
{
    for (int j = 0; j < inputs.Count; j++)
    {
        if (i == j)
        {
            continue;
        }

        var o = Aggregate(new() { inputs[i], inputs[j] });
        var m = Actions.CalculateMagnitude(o);
        if (m > highestMagnitude)
        {
            highestMagnitude = m;
        }
    }
}

Console.WriteLine($"Part2: {highestMagnitude}");

static List<(int depth, int value)> Aggregate(List<List<(int depth, int value)>> inputs)
{
    var output = inputs.First();
    inputs.RemoveAt(0);
    Print(output, "Initial state:\t\t");

    Reduce(ref output);

    while (inputs.Any())
    {
        var second = inputs.First();
        inputs.RemoveAt(0);

        Reduce(ref second);

        Print(second, "Add:\t\t\t");
        output = Actions.Addition(output, second);
        Print(output, "After addition:\t\t");
        
        Reduce(ref output);
        Print(output, "Result:\t\t\t");
    }

    return output;
}

static void Reduce(ref List<(int depth, int value)> elements)
{
    while(true)
    {
        var toExplode = elements.FindIndex(e => e.value == -1 && e.depth > 4);
        var toSplit = elements.FindIndex(e => e.value != -1 && e.value >= 10);

        if (toExplode != -1 && elements[toExplode - 1].value != -1 && elements[toExplode - 2].value != -1)
        {
            Actions.Explode(toExplode, ref elements);
        }
        else if (toSplit != -1)
        {
            Actions.Split(toSplit, ref elements);
        }
        else
        {
            break;
        }
        
        Print(elements);
    }
}

static void Print(List<(int depth, int value)> elements, string prefix = "")
{
    #if DEBUG
    Console.Write(prefix + string.Join(",", elements.Where(e => e.value != -1).Select(e => e.value)));
    Console.Write("\t\t");
    foreach (var element in elements)
    {
        Console.Write(element.value != -1 ? $"({element.value})" : $"[{element.depth}]");
    }
    Console.WriteLine();
    #endif
}

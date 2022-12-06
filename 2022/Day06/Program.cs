
int Part(int count)
{
    var input = File.ReadAllText("input.txt");
    int index;
    for (index = count - 1;
         index < input.Length && input.Skip(index - (count - 1)).Take(count).Distinct().Count() != count;
         index++)
    {
        // nothing to do here, move on
    }

    return index + 1;
}
Console.WriteLine("Part 1: " + Part(4));
Console.WriteLine("Part 2: " + Part(14));

int Part(int count)
{
    var input = File.ReadAllText("input.txt");
    var index = count - 1;
    while (index < input.Length)
    {
        if (input.Skip(index - (count - 1)).Take(count).Distinct().Count() == count)
        {
            break;
        }
        
        index++;
    }

    index++;

    return index;
}
Console.WriteLine("Part 1: " + Part(4));
Console.WriteLine("Part 2: " + Part(14));
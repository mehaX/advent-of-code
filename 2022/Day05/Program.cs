Console.WriteLine("Part 1: " + Part(false));
Console.WriteLine("Part 2: " + Part(true));

string Part(bool hasSameOrder)
{
    GetInput(out var stacks, out var commands);
    
    var stackTable = new StackTable(stacks);
    commands.ForEach(command => stackTable.RunCommand(command, hasSameOrder));
    
    return stackTable.GetResult();
}

void GetInput(out List<string> stacks, out List<Command> commands)
{
    stacks = new List<string>();
    var inputStr = File.ReadAllText("input.txt");
    var chunks = inputStr.Split("\r\n\r\n").Select(chunk => chunk.Split("\r\n")).ToArray();
    var table = chunks[0].Reverse().Skip(1).ToList();
    var totalCols = table.First().Split(" ").Length;
    for (var i = 0; i < totalCols; i++)
    {
        var stack = "";
        var index = i * 4 + 1;
        for (var j = 0; j < table.Count && table[j][index] != ' '; j++)
        {
            stack += table[j][index].ToString();
        }
        stacks.Add(stack);
    }

    commands = chunks[1].Select(row =>
    {
        var segments = row.Split(" ");
        var count = int.Parse(segments[1]);
        var from = int.Parse(segments[3]);
        var to = int.Parse(segments[5]);
        return new Command(count, from, to);
    }).ToList();
}

class StackTable
{
    private Dictionary<int, Stack<char>> mStacks = new();

    public StackTable(IList<string> input)
    {
        for (var i = 0; i < input.Count; i++)
        {
            mStacks.Add(i + 1, new Stack<char>(input[i]));
        }
    }

    public void RunCommand(Command command, bool hasSameOrder)
    {
        var items = new List<char>();
        for (var i = 0; i < command.Count; i++)
        {
            items.Add(mStacks[command.From].Pop());
        }
        
        if (hasSameOrder)
        {
            items.Reverse();
        }

        foreach (var item in items)
        {
            mStacks[command.To].Push(item);
        }
    }

    public string GetResult()
    {
        return string.Join("", mStacks.Select(kv => kv.Value.Peek()));
    }
}

public record Command(int Count, int From, int To);
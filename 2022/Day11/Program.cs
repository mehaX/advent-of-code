List<Monkey> monkeys;

monkeys = GetInput();
Console.WriteLine("Part 1: " + Part(20, true));

monkeys = GetInput();
Console.WriteLine("Part 2: " + Part(10_000, false));

long Part(int maxRounds, bool shouldDivide)
{
    for (int round = 1; round <= maxRounds; round++)
    {
        StartRound(shouldDivide);
    }

    return monkeys.Select(m => m.OperationCount).OrderDescending().Take(2).Aggregate((a, b) => a * b);
}

List<Monkey> GetInput()
{
    var strMonkeys = File.ReadAllText("input.txt").Split("\r\n\r\n");

    var result = new List<Monkey>();
    
    foreach (var strMonkey in strMonkeys)
    {
        var rows = strMonkey.Split("\r\n");
        var monkey = new Monkey();
        monkey.Items = rows[1].Substring(18).Split(", ").Select(long.Parse).ToList();
        monkey.Operator = rows[2][23];
        
        monkey.Operand = int.TryParse(rows[2].Split(" ").Last(), out var operand) ? operand : null;
        monkey.DivisibleBy = int.Parse(rows[3].Split(" ").Last());
        monkey.ToMonkey1 = int.Parse(rows[4].Split(" ").Last());
        monkey.ToMonkey0 = int.Parse(rows[5].Split(" ").Last());
        monkey.OperationCount = 0;
        result.Add(monkey);
    }
    
    return result;
}

void StartRound(bool shouldDivide)
{
    var supermodule = monkeys.Select(m => m.DivisibleBy).Aggregate((a, b) => a * b);
    foreach (var monkey in monkeys)
    {
        while (monkey.Items.Any())
        {
            var item = monkey.Items.First();
            monkey.Items.RemoveAt(0);
            var worryLevel = item;
            var op = ((long?)monkey.Operand).GetValueOrDefault(worryLevel);
            worryLevel = monkey.Operator switch
            {
                '*' => worryLevel * op,
                '+' => worryLevel + op,
                _ => throw new Exception("Operator not found"),
            };
            if (shouldDivide)
            {
                worryLevel /= 3;
            }
            else
            {
                worryLevel %= supermodule;
            }

            var isDivisible = worryLevel % monkey.DivisibleBy == 0;
            if (isDivisible)
            {
                monkeys[monkey.ToMonkey1].Items.Add(worryLevel);
            }
            else
            {
                monkeys[monkey.ToMonkey0].Items.Add(worryLevel);
            }

            monkey.OperationCount++;
        }
    }
}

class Monkey
{
    public List<long> Items { get; set; }
    public char Operator { get; set; } // either * or +
    public int? Operand { get; set; } // if null then 'old' else number
    public long DivisibleBy { get; set; }
    public int ToMonkey0 { get; set; } // if false
    public int ToMonkey1 { get; set; } // if true

    public long OperationCount { get; set; } = 0;
}

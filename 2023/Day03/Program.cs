var input = File.ReadAllLines("input.txt").ToList();
var engine = new char[input.Count, input[0].Length];
var numbers = new List<Number>();

for (var i = 0; i < engine.GetLength(0); i++)
{
    for (int j = 0; j < engine.GetLength(1); j++)
    {
        engine[i, j] = input[i][j];
    }
}

for (var i = 0; i < engine.GetLength(0); i++)
{
    for (int j = 0; j < engine.GetLength(1); j++)
    {
        if (!Char.IsNumber(engine[i, j]))
        {
            continue;
        }

        var nr = "";
        var positions = new List<Pos>();
        for (int k = j; k < engine.GetLength(1) && Char.IsNumber(engine[i, k]); k++)
        {
            nr += engine[i, k].ToString();
            positions.Add(new Pos(i, k));
        }
        
        numbers.Add(new Number(int.Parse(nr), positions));

        j += nr.Length;
    }
}

int Part1()
{
    return numbers.Where(nr => nr.HasSymbolAsNeighbor(engine)).Select(nr => nr.Nr).Sum();
}

int Part2()
{
    var result = 0;
    
    for (var i = 0; i < engine.GetLength(0); i++)
    {
        for (int j = 0; j < engine.GetLength(1); j++)
        {
            if (engine[i, j] == '.' || Char.IsNumber(engine[i, j]))
            {
                continue;
            }

            var pos = new Pos(i, j);

            var nrs = numbers.Where(nr => nr.NeighborPositions(engine).Contains(pos)).ToList();
            if (nrs.Count == 2)
            {
                result += nrs[0].Nr * nrs[1].Nr;
            }
        }
    }

    return result;
}

Console.WriteLine("Part1: " + Part1());
Console.WriteLine("Part2: " + Part2());

class Number
{
    public int Nr { get; set; }
    public List<Pos> Positions { get; set; }

    public Number(int nr, List<Pos> positions)
    {
        Nr = nr;
        Positions = positions;
    }

    public List<Pos> NeighborPositions(char[,] engine)
    {
        var result = new List<Pos>();

        foreach (var pos in Positions)
        {
            result.Add(new Pos(pos.row - 1, pos.col - 1));
            result.Add(new Pos(pos.row - 1, pos.col));
            result.Add(new Pos(pos.row - 1, pos.col + 1));
            result.Add(new Pos(pos.row, pos.col - 1));
            result.Add(new Pos(pos.row, pos.col));
            result.Add(new Pos(pos.row, pos.col + 1));
            result.Add(new Pos(pos.row + 1, pos.col - 1));
            result.Add(new Pos(pos.row + 1, pos.col));
            result.Add(new Pos(pos.row + 1, pos.col + 1));
        }

        result = result
            .Where(pos => pos.row >= 0 && pos.col >= 0 && pos.row < engine.GetLength(0) && pos.col < engine.GetLength(1))
            .Where(pos => !Positions.Contains(pos))
            .ToList();
        
        return result;
    }

    public bool HasSymbolAsNeighbor(char[,] engine)
    {
        return NeighborPositions(engine)
            .Any(pos => engine[pos.row, pos.col] != '.');
    }
}

record Pos(int row, int col);






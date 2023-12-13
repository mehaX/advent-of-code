var games = File.ReadAllLines("input.txt").Select(Game.Deserialize).ToList() ?? throw new Exception("");


int Part1()
{
    var ids = new List<int>();
    foreach (var game in games)
    {
        var redCubes = game.Sets.Max(s => s.Cubes.FirstOrDefault(c => c.Key == "red").Value);
        var greenCubes = game.Sets.Max(s => s.Cubes.FirstOrDefault(c => c.Key == "green").Value);
        var blueCubes = game.Sets.Max(s => s.Cubes.FirstOrDefault(c => c.Key == "blue").Value);

        if (redCubes <= 12 && greenCubes <= 13 && blueCubes <= 14)
        {
            ids.Add(game.Index);
        }
    }

    return ids.Sum();
}

int Part2()
{
    return games.Select(game =>
    {
        var redCubes = game.Sets.Max(s => s.Cubes.FirstOrDefault(c => c.Key == "red").Value);
        var greenCubes = game.Sets.Max(s => s.Cubes.FirstOrDefault(c => c.Key == "green").Value);
        var blueCubes = game.Sets.Max(s => s.Cubes.FirstOrDefault(c => c.Key == "blue").Value);

        return redCubes * greenCubes * blueCubes;
    }).Sum();
}

Console.WriteLine("Part1 " + Part1());
Console.WriteLine("Part2 " + Part2());

class Game
{
    public int Index;
    public List<Set> Sets = new();

    public static Game Deserialize(string row)
    {
        var game = new Game();
        var chunks = row.Split(":").ToArray();
        game.Index = int.Parse(chunks[0].Replace("Game ", ""));
        game.Sets = chunks[1].Trim().Split("; ").Select(Set.DeserializeSet).ToList();

        return game;
    }
}

class Set
{
    public Dictionary<string, int> Cubes = new();

    public static Set DeserializeSet(string input)
    {
        var set = new Set();

        var chunks = input.Split(", ").ToList();
        foreach (var chunk in chunks)
        {
            var cube = DeserializeCube(chunk);
            set.Cubes.Add(cube.Item1, cube.Item2);
        }

        return set;
    }

    public static (string, int) DeserializeCube(string input)
    {
        var chunks = input.Split(" ").ToArray();

        return (chunks[1], int.Parse(chunks[0]));
    }
}

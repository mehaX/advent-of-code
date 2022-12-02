var totalResults = new Dictionary<(char opponent, char player), int>()
{
    { ('X', 'X'), 3 },
    { ('X', 'Y'), 6 },
    { ('X', 'Z'), 0 },
    { ('Y', 'X'), 0 },
    { ('Y', 'Y'), 3 },
    { ('Y', 'Z'), 6 },
    { ('Z', 'X'), 6 },
    { ('Z', 'Y'), 0 },
    { ('Z', 'Z'), 3 },
};

var encryptedPlays = new Dictionary<char, char>()
{
    { 'A', 'X' },
    { 'B', 'Y' },
    { 'C', 'Z' },
};

var optionScores = new Dictionary<char, int>()
{
    { 'X', 1 },
    { 'Y', 2 },
    { 'Z', 3 },
};

List<(char, char)> GetGuides()
{
    return File.ReadAllLines("input.txt").Select(row => (row[0], row[2])).ToList();
}

int GetScore(char opponent, char player)
{
    var result = totalResults[(opponent, player)];

    return optionScores[player] + result;
}

int Part1()
{
    var guides = GetGuides();
    var totalScore = guides.Select(guide => GetScore(encryptedPlays[guide.Item1], guide.Item2)).Sum();
    return totalScore;
}

int Part2()
{
    var shouldScoreOptions = new Dictionary<char, int>()
    {
        { 'X', 0 },
        { 'Y', 3 },
        { 'Z', 6 },
    };
        
    var guides = GetGuides();
    var totalScore = guides.Select(guide =>
    {
        var opponent = encryptedPlays[guide.Item1];
        var shouldScore = shouldScoreOptions[guide.Item2];
        var shouldPlay = totalResults.First(kv => kv.Key.opponent == opponent && kv.Value == shouldScore).Key.player;
        return GetScore(opponent, shouldPlay);
    }).Sum();
    return totalScore;
}

Console.WriteLine("Part1: " + Part1());
Console.WriteLine("Part2: " + Part2());
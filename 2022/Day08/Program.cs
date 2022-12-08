using Day08;

var input = GetInput();
Console.WriteLine("Part 1: " + new Part1().Calculate(input));
Console.WriteLine("Part 2: " + new Part2().Calculate(input));

int[,] GetInput()
{
    var strInput = File.ReadLines("input.txt").ToList();
    var rows = strInput.Count;
    var cols = strInput.First().Length;
    var result = new int[rows, cols];

    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            result[row, col] = strInput[row][col] - '0';
        }
    }

    return result;
}
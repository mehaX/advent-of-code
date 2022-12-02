using Day04;

var input = File.ReadAllText("input.txt");
var part1 = new Part1(input).Calculate();
var part2 = new Part2(input).Calculate();

Console.WriteLine($"Part 1 result: {part1}");
Console.WriteLine($"Part 2 result: {part2}");

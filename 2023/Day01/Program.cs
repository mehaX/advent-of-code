using System.Text.RegularExpressions;

var rows = File.ReadAllLines("input.txt").ToList();

string RemoveLetters(string input)
{
    return new Regex("[a-zAZ]+").Replace(input, "");
}

int Part1()
{
    return rows
        .Select(RemoveLetters)
        .Select(row => row[0] + "" + row[^1])
        .Select(int.Parse)
        .Sum();
}

int Part2()
{
    var numbers = new Dictionary<string, int>()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 },
    };

    return rows.Select(row =>
        {
            var originalRow = row.Clone();
            var firstLetter = "";
            var secondLetter = "";

            for (var i = 0; i < row.Length; i++)
            {
                if (row[i] >= '0' && row[i] <= '9')
                {
                    firstLetter = row[i].ToString();
                    break;
                }

                foreach (var number in numbers)
                {
                    if (i + number.Key.Length <= row.Length && row.Substring(i, number.Key.Length).Equals(number.Key))
                    {
                        firstLetter = number.Value.ToString();
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(firstLetter))
                {
                    break;
                }
            }
            

            for (var i = row.Length - 1; i >= 0; i--)
            {
                if (row[i] >= '0' && row[i] <= '9')
                {
                    secondLetter = row[i].ToString();
                    break;
                }

                foreach (var number in numbers)
                {
                    if (i + number.Key.Length <= row.Length && row.Substring(i, number.Key.Length).Equals(number.Key))
                    {
                        secondLetter = number.Value.ToString();
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(secondLetter))
                {
                    break;
                }
            }

            var combined = firstLetter + secondLetter;
            var result = int.Parse(combined);

            // Console.WriteLine($"{row} {combined} {result}");
            
            return result;
        })
        .Sum();
}

Console.WriteLine("Part1: " + Part1());
Console.WriteLine("Part2: " + Part2());

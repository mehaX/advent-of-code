namespace Day04;

public class Part1 : Part
{
    public Part1(string input) : base(input)
    {
    }
    
    public override int Calculate()
    {
        foreach (var number in mNumbers)
        {
            var res = MarkBoardNumbers(number);
            if (res != 0)
            {
                return res * number;
            }
        }

        return 0;
    }
}

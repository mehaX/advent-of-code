namespace Day04;

public class Part2 : Part
{
    public Part2(string input) : base(input)
    {
    }

    public override int Calculate()
    {
        var lastResult = 0;
        foreach (var number in mNumbers)
        {
            _ = MarkBoardNumbers(number);
            int? winnerBoardIndex;
            while((winnerBoardIndex = HasWinnerBoard()) != null)
            {
                lastResult = mBoards[winnerBoardIndex.Value].SumUnmarked() * number;
                mBoards.RemoveAt(winnerBoardIndex.Value);
            }
        }

        return lastResult;
    }
}
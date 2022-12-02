namespace Day04;

public abstract class Part
{
    protected List<Board> mBoards;
    protected List<int> mNumbers;

    public Part(string input)
    {
        var segments = input.Split("\n\n");
        mNumbers = segments[0].Split(",").Select(nr => Convert.ToInt32(nr)).ToList();

        mBoards = segments.Skip(1).Select(strBoard => new Board(strBoard)).ToList();
    }
    
    protected int MarkBoardNumbers(int nr)
    {
        var result = 0;
        foreach (var board in mBoards)
        {
            board.MarkByNumber(nr);
            if (result == 0 && board.IsWinner())
            {
                result = board.SumUnmarked();
            }
        }

        return result;
    }

    protected int? HasWinnerBoard()
    {
        for (var i = 0; i < mBoards.Count; i++)
        {
            if (mBoards[i].IsWinner())
            {
                return i;
            }
        }

        return null;
    }

    public abstract int Calculate();
}
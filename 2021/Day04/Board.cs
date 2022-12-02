namespace Day04;

public class Board
{
    private int[,] mContent = new int[5, 5];
    private bool[,] mMarked = new bool[5, 5];

    public Board(string content)
    {
        var rowIndex = 0;
        foreach (var row in content.Split("\n"))
        {
            var colIndex = 0;
            foreach (var col in row.Trim().Split(" "))
            {
                if (col == "")
                {
                    continue;
                }
                
                mContent[rowIndex, colIndex] = Convert.ToInt32(col);
                mMarked[rowIndex, colIndex] = false;
                colIndex++;
            }
            rowIndex++;
        }
    }

    public void MarkByNumber(int number)
    {
        for (var row = 0; row < 5; row++)
        {
            for (var col = 0; col < 5; col++)
            {
                if (mContent[row, col] == number)
                {
                    mMarked[row, col] = true;
                }
            }
        }
    }

    public bool IsWinner()
    {
        var rowCount = new [] { 0, 0, 0, 0, 0 };
        var colCount = new [] { 0, 0, 0, 0, 0 };
        for (var rowIndex = 0; rowIndex < 5; rowIndex++)
        {
            for (var colIndex = 0; colIndex < 5; colIndex++)
            {
                if (mMarked[rowIndex, colIndex])
                {
                    rowCount[rowIndex]++;
                    colCount[colIndex]++;

                    if (rowCount[rowIndex] == 5 || colCount[colIndex] == 5)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public int SumUnmarked()
    {
        if (!IsWinner())
        {
            return 0;
        }
        
        var result = 0;
        for (var rowIndex = 0; rowIndex < 5; rowIndex++)
        {
            for (var colIndex = 0; colIndex < 5; colIndex++)
            {
                if (!mMarked[rowIndex, colIndex])
                {
                    result += mContent[rowIndex, colIndex];
                }
            }
        }

        return result;
    }
}
namespace Day08;

public class Part1
{
    public int Calculate(int[,] input)
    {
        var count = 0;
        for (int row = 0; row < input.GetLength(0); row++)
        {
            for (int col = 0; col < input.GetLength(1); col++)
            {
                if (IsVisible(row, col, input))
                {
                    count++;
                }
            }
        }

        return count;
    }

    private bool IsVisible(int row, int col, int[,] input)
    {
        var rows = input.GetLength(0);
        var cols = input.GetLength(1);
        if (row == 0 || col == 0 || row == rows - 1 || col == cols - 1)
        {
            return true;
        }

        return IsVisibleInDirection(row, col, input, Direction.Top)
               || IsVisibleInDirection(row, col, input, Direction.Bottom)
               || IsVisibleInDirection(row, col, input, Direction.Left)
               || IsVisibleInDirection(row, col, input, Direction.Right);
    }

    private bool IsVisibleInDirection(int row, int col, int[,] input, Direction direction)
    {
        var value = input[row, col];
        var rows = input.GetLength(0);
        var cols = input.GetLength(1);
        var offsetRow = direction switch
        {
            Direction.Left => -1,
            Direction.Right => 1,
            _ => 0,
        };
        var offsetCol = direction switch
        {
            Direction.Top => -1,
            Direction.Bottom => 1,
            _ => 0,
        };

        while (true)
        {
            row += offsetRow;
            col += offsetCol;

            if (row < 0 || row >= rows || col < 0 || col >= cols)
            {
                break;
            }

            if (input[row, col] >= value)
            {
                return false;
            }
        }

        return true;
    }
}
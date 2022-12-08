namespace Day08;

public class Part2
{
    public int Calculate(int[,] input)
    {
        var maxValue = 0;

        for (int row = 0; row < input.GetLength(0); row++)
        {
            for (int col = 0; col < input.GetLength(1); col++)
            {
                var value = CalculateVisibility(row, col, input);
                if (value > maxValue)
                {
                    maxValue = value;
                }
            }
        }

        return maxValue;
    }

    private int CalculateVisibility(int row, int col, int[,] input)
    {
        var counts = new[]
        {
            CalculateVisibilityInDirection(row, col, input, Direction.Top),
            CalculateVisibilityInDirection(row, col, input, Direction.Bottom),
            CalculateVisibilityInDirection(row, col, input, Direction.Left),
            CalculateVisibilityInDirection(row, col, input, Direction.Right),
        };

        return counts.Aggregate((a, b) => a * b);
    }

    private int CalculateVisibilityInDirection(int row, int col, int[,] input, Direction direction)
    {
        var value = input[row, col];
        var rows = input.GetLength(0);
        var cols = input.GetLength(1);
        var count = 0;
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

            count++;

            if (input[row, col] >= value)
            {
                break;
            }
        }

        return count;
    }
}
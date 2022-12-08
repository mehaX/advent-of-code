Console.WriteLine("Part 1: " + Part1(2));
Console.WriteLine("Part 2: " + Part1(50));

int Part1(int count)
{
    GetInput(out var image, out var imageEnhancement);
    
    for (var i = 0; i < count; i++)
    {
        image = EnhanceImage(image, imageEnhancement, i);
    }

    return image.CountLitPixels();
}

void GetInput(out ushort[,] imageInput, out ushort[] imageEnhancement)
{
    var inputStr = File.ReadAllLines("input.txt").ToList();
    imageEnhancement = inputStr.First().Select(c => c == '#' ? (ushort)1 : (ushort)0).ToArray();
    inputStr.RemoveRange(0, 2);

    var rows = inputStr.Count;
    var cols = inputStr.First().Length;

    imageInput = new ushort[rows, cols];
    for (var row = 0; row < rows; row++)
    {
        for (var col = 0; col < cols; col++)
        {
            imageInput[row, col] = inputStr[row][col] == '#' ? (ushort)1 : (ushort)0;
        }
    }
}

ushort[,] EnhanceImage(ushort[,] imageInput, ushort[] imageEnhancement, int round)
{
    imageInput = imageInput.AddBorder(5);
    var result = new ushort[imageInput.GetLength(0), imageInput.GetLength(1)];

    for (var row = 0; row < result.GetLength(0); row++)
    {
        for (var col = 0; col < result.GetLength(1); col++)
        {
            var index = imageInput.GetPixelIndex(row, col, round);
            var newValue = imageEnhancement[index];
            result[row, col] = newValue;
        }
    }

    return result;
}

void Print(ushort[,] image)
{
    for (var row = 0; row < image.GetLength(0); row++)
    {
        for (var col = 0; col < image.GetLength(1); col++)
        {
            Console.Write(image[row, col] == 1 ? "#" : ".");
        }

        Console.WriteLine();
    }
}

static class Extensions
{
    public static ushort GetValueFromPixel(this ushort[,] image, int row, int col, int round)
    {
        if (row < 0 || col < 0
                    || row >= image.GetLength(0)
                    || col >= image.GetLength(1))
        {
            // return (ushort)(round % 2);
            return 0;
        }

        return image[row, col];
    }
    
    public static ushort[] GetPixelsFromPixel(this ushort[,] image, int row, int col, int round)
    {
        return new ushort[]
        {
            image.GetValueFromPixel(row - 1, col - 1, round),
            image.GetValueFromPixel(row - 1, col, round),
            image.GetValueFromPixel(row - 1, col + 1, round),
            image.GetValueFromPixel(row, col - 1, round),
            image.GetValueFromPixel(row, col, round),
            image.GetValueFromPixel(row, col + 1, round),
            image.GetValueFromPixel(row + 1, col - 1, round),
            image.GetValueFromPixel(row + 1, col, round),
            image.GetValueFromPixel(row + 1, col + 1, round),
        };
    }

    public static int GetPixelIndex(this ushort[,] image, int row, int col, int round)
    {
        var values = image.GetPixelsFromPixel(row, col, round);
        var strBinaryIndex = string.Join("", values.Select(p => p.ToString()));
        var index = Convert.ToInt32(strBinaryIndex, 2);
        return index;
    }

    public static void Print(this ushort[,] image)
    {
        for (var row = 0; row < image.GetLength(0); row++)
        {
            for (var col = 0; col < image.GetLength(1); col++)
            {
                Console.Write(image[row, col] == 1 ? "#" : ".");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public static int CountLitPixels(this ushort[,] sourceImage)
    {
        var count = 0;
        for (int row = 0; row < sourceImage.GetLength(0); row++)
        {
            for (int col = 0; col < sourceImage.GetLength(1); col++)
            {
                if (sourceImage[row, col] == 1)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public static ushort[,] AddBorder(this ushort[,] image, int count)
    {
        var newRows = image.GetLength(0) + count * 2;
        var newCols = image.GetLength(1) + count * 2;
        var result = new ushort[newRows, newCols];

        for (int row = 0; row < image.GetLength(0); row++)
        {
            for (int col = 0; col < image.GetLength(1); col++)
            {
                result[row + count, col + count] = image[row, col];
            }
        }

        return result;
    }
}
namespace Day18;

public class Actions
{
    public static List<(int depth, int value)> Addition(List<(int depth, int value)> pair1, List<(int depth, int value)> pair2)
    {
        var result = new List<(int depth, int value)>();
        
        foreach (var element in pair1)
        {
            result.Add((element.Item1 + 1, element.Item2));
        }
        foreach (var element in pair2)
        {
            result.Add((element.Item1 + 1, element.Item2));
        }
        
        result.Add((1, -1));
        return result;
    }

    public static void Explode(int index, ref List<(int depth, int value)> elements)
    {
        var xIndex = index - 2;
        var yIndex = index - 1;
        
        var element = elements[index];
    
        var newDepth = element.depth;

        var leftLiteralIndex = elements.Take(xIndex).ToList().FindLastIndex(e => e.value != -1);
        var rightLiteralIndex = elements.Skip(yIndex + 1).ToList().FindIndex(e => e.value != -1) + yIndex + 1;

        if (leftLiteralIndex != -1)
        {
            elements[leftLiteralIndex] = (elements[leftLiteralIndex].depth, elements[leftLiteralIndex].value + elements[xIndex].value);
        }
        if (rightLiteralIndex != -1)
        {
            elements[rightLiteralIndex] = (elements[rightLiteralIndex].depth, elements[rightLiteralIndex].value + elements[yIndex].value);
        }

        elements[index] = (newDepth, 0);
        elements.RemoveAt(yIndex);
        elements.RemoveAt(xIndex);
    }

    public static void Split(int index, ref List<(int depth, int value)> elements)
    {
        var literalElement = elements[index];
        var newDepth = literalElement.depth + 1;

        (int depth, int value) xLiteral = (newDepth, literalElement.value / 2);
        (int depth, int value) yLiteral = (newDepth, literalElement.value - xLiteral.value);

        elements[index] = xLiteral;
        elements.Insert(index + 1, yLiteral);
        elements.Insert(index + 2, (newDepth - 1, -1));
    }

    public static int CalculateMagnitude(List<(int depth, int value)> elements)
    {
        while (elements.Count > 1)
        {
            for (int i = 2; i < elements.Count; i++)
            {
                if (elements[i].value != -1 || elements[i - 1].value == -1 || elements[i - 2].value == -1)
                {
                    continue;
                }
                
                elements[i - 2] = (elements[i - 2].depth, elements[i - 2].value * 3 + elements[i - 1].value * 2);
                elements.RemoveAt(i);
                elements.RemoveAt(i - 1);
            }
        }

        return elements.First().value;
    }
}

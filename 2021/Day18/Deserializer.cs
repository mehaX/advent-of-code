namespace Day18;

public class Deserializer
{
    const int Closing = -1;
    
    public static List<(int depth, int value)> DeserializePair(string input)
    {
        var result = new List<(int, int)>();
        var depth = 1;

        var tempNumber = "";

        foreach (var c in input.ToCharArray())
        {
            if (c is >= '0' and <= '9')
            {
                tempNumber += c;
            }
            else if (c == ',')
            {
                if (tempNumber != "")
                {
                    result.Add((depth, Convert.ToInt32(tempNumber)));
                    tempNumber = "";
                }
            }
            else if (c == '[')
            {
                depth++;
            }
            else if (c == ']')
            {
                if (tempNumber != "")
                {
                    result.Add((depth, Convert.ToInt32(tempNumber)));
                    tempNumber = "";
                }

                depth--;
                result.Add((depth, Closing));
            }
        }

        return result;
    }

}
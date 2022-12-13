using Day13;

Console.WriteLine("Part 1: " + Part1());
Console.WriteLine("Part 2: " + Part2());

int Part1()
{
    var input = GetInput();
    
    var result = 0;
    var pairIndex = 1;
    foreach (var pair in input.Chunk(2))
    {
        var compareResult = CompareList(pair[0], pair[1]);
        if (compareResult == CompareResult.RightOrder)
        {
            result += pairIndex;
        }

        pairIndex++;
    }

    return result;
}

int Part2()
{
    var additionals = new[] { "[[2]]", "[[6]]" }
        .Select(line => line.Substring(1, line.Length - 2)) // remove outer brackets
        .Select(DeserializeStringList)
        .Select(x => (ListElement)x)
        .ToList();
    
    var input = GetInput().Concat(additionals).ToList();

    var graph = new Graph();

    foreach (var key in input)
    {
        var value = input.Except(new[] { key })
            .Where(x => CompareList(key, x) == CompareResult.RightOrder)
            .ToList();
        
        graph.AddElement(key, value);
    }

    var path = graph.FindNodePath();
    if (!path.Any())
    {
        return 0;
    }
    return path
        .Where(n => additionals.Contains(n))
        .Select(n => path.IndexOf(n) + 1)
        .Aggregate((a, b) => a * b);
}

List<ListElement> GetInput()
{
    return File.ReadLines("input.txt")
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line => line.Substring(1, line.Length - 2)) // remove outer brackets
        .Select(DeserializeStringList)
        .Select(x => (ListElement)x)
        .ToList();
}

IElement DeserializeStringList(string input)
{
    var result = new ListElement();

    var tempNumber = "";
    for (int index = 0; index < input.Length; index++)
    {
        var c = input[index];
        if (c is >= '0' and <= '9')
        {
            tempNumber += c;
        }
        else if (c == ',')
        {
            if (tempNumber != "")
            {
                result.Elements.Add(new LiteralElement(tempNumber));
                tempNumber = "";
            }
        }
        else if (c == '[')
        {
            var depth = 1;
            var subIndex = index + 1;
            while (subIndex < input.Length)
            {
                if (input[subIndex] == '[')
                {
                    depth++;
                }
                else if (input[subIndex] == ']')
                {
                    depth--;
                    if (depth == 0)
                    {
                        break;
                    }
                }

                subIndex++;
            }

            if (subIndex == input.Length)
            {
                throw new Exception("No ] found");
            }

            var subLength = subIndex - index - 1;
            var element = DeserializeStringList(input.Substring(index + 1, subLength));
            result.Elements.Add(element);
            index = subIndex + 1;
        }
    }
        
    if (tempNumber != "")
    {
        result.Elements.Add(new LiteralElement(tempNumber));
    }

    return result;
}

CompareResult CompareList(ListElement left, ListElement right)
{
    var leftQueue = new Queue<IElement>(left.Elements);
    var rightQueue = new Queue<IElement>(right.Elements);
    var result = CompareResult.Continue;

    while (leftQueue.Any() && rightQueue.Any())
    {
        var leftElement = leftQueue.Dequeue();
        var rightElement = rightQueue.Dequeue();

        var areLiterals = leftElement is LiteralElement && rightElement is LiteralElement;
        if (areLiterals)
        {
            result = CompareLiterals(leftElement as LiteralElement, rightElement as LiteralElement);
        }
        else
        {
            if (leftElement is LiteralElement)
            {
                leftElement = new ListElement()
                    { Elements = new() { new LiteralElement((leftElement as LiteralElement).Value) } };
            }

            if (rightElement is LiteralElement)
            {
                rightElement = new ListElement()
                    { Elements = new() { new LiteralElement((rightElement as LiteralElement).Value) } };
            }

            result = CompareList(leftElement as ListElement, rightElement as ListElement);
        }

        if (result is CompareResult.RightOrder or CompareResult.NotRightOrder)
        {
            return result;
        }
    }

    if (leftQueue.Count() == rightQueue.Count())
    {
        return CompareResult.Continue;
    }

    if (!leftQueue.Any())
    {
        return CompareResult.RightOrder;
    }

    if (!rightQueue.Any())
    {
        return CompareResult.NotRightOrder;
    }

    return result;
}

CompareResult CompareLiterals(LiteralElement left, LiteralElement right)
{
    if (left.Value == right.Value)
    {
        return CompareResult.Continue;
    }

    return left.Value < right.Value ? CompareResult.RightOrder : CompareResult.NotRightOrder;
}
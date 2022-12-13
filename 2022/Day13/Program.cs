using Day13;

Console.WriteLine("Part 1: " + Part1());
Console.WriteLine("Part 2: " + Part2());

int Part1()
{
    var input = GetInput();
    
    var groups = input.Chunk(2).ToList();
    return groups.Select(pair =>
            CompareList(pair[0], pair[1]) == CompareResult.RightOrder
                ? groups.IndexOf(pair) + 1
                : 0)
        .Sum();
}

int Part2()
{
    var additionalPackets = GetInput("[[2]]", "[[6]]").ToList();
    var input = GetInput().Concat(additionalPackets).ToList();

    var graph = new Graph();

    foreach (var node in input)
    {
        var edges = input.Except(new[] { node })
            .Where(x => CompareList(node, x) == CompareResult.RightOrder)
            .ToList();
        
        graph.AddNodeWithEdges(node, edges);
    }

    var path = graph.FindEulerianPath();
    
    return path
        .Where(n => additionalPackets.Contains(n))
        .Select(n => path.IndexOf(n) + 1) // Get indexes of the additional packets
        .Aggregate((a, b) => a * b);
}

IEnumerable<ListElement> GetInput(params string[] args)
{
    return (args.Length > 0 ? args : File.ReadLines("input.txt"))
        .Where(line => !string.IsNullOrEmpty(line)) // get rid of empty lines
        .Select(line => line.Substring(1, line.Length - 2)) // remove outer brackets
        .Select(DeserializeStringToListElement)
        .ToList();
}

ListElement DeserializeStringToListElement(string input)
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
            var element = DeserializeStringToListElement(input.Substring(index + 1, subLength));
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
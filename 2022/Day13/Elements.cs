namespace Day13;

internal interface IElement
{
}

internal class LiteralElement : IElement
{
    public int Value { get; set; }

    public LiteralElement(int value)
    {
        Value = value;
    }

    public LiteralElement(string strValue)
    {
        Value = int.Parse(strValue);
    }
}

internal class ListElement : IElement
{
    public List<IElement> Elements { get; set; }

    public ListElement()
    {
        Elements = new();
    }
}

internal enum CompareResult
{
    Continue,
    RightOrder,
    NotRightOrder,
}

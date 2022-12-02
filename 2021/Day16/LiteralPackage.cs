namespace Day16;

public class LiteralPackage : Package
{
    private long mValue;

    public LiteralPackage() : base(4)
    {
    }
    
    public override void RegisterPayload(ref string payload)
    {
        var literal = "";
        while (payload.Length >= 5)
        {
            PackageParser.SpliceNumber(ref payload, out var prefix, 1);
            PackageParser.SpliceString(ref payload, out var value, 4);
            literal += value;
            if (prefix == 0)
            {
                break;
            }
        }
        mValue = Convert.ToInt64(literal, 2);
    }

    public override long CalculateResult()
    {
        return mValue;
    }
}
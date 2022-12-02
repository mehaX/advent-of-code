namespace Day16;

public class OperatorPackage : Package
{
    private readonly List<Package> mSubPackages = new();

    public OperatorPackage(int id) : base(id)
    {
    }
    
    public override void RegisterPayload(ref string payload)
    {
        PackageParser.SpliceNumber(ref payload, out var lengthTypeId, 1);
        if (lengthTypeId == 0)
        {
            PackageParser.SpliceNumber(ref payload, out var length, 15);
            var newBin = payload.Substring(0, length);
            while (newBin.Length > 0)
            {
                mSubPackages.Add(PackageParser.Parse(ref newBin));
            }

            payload = payload.Substring(length);
        }
        else
        {
            PackageParser.SpliceNumber(ref payload, out var count, 11);
            for (var i = 0; i < count; i++)
            {
                mSubPackages.Add(PackageParser.Parse(ref payload));
            }
        }
    }

    public override int CalculateVersionSum()
    {
        return Version + mSubPackages.Select(p => p.CalculateVersionSum()).Sum();
    }

    public override long CalculateResult()
    {
        var packageValues = mSubPackages.Select(p => p.CalculateResult()).ToArray();
        switch (Id)
        {
            case 0: // ADD
                return packageValues.Sum();
            
            case 1: // MULTIPLY
                return packageValues.Aggregate((a, b) => a * b);
            
            case 2: // MIN
                return packageValues.Min();
            
            case 3: // MAX
                return packageValues.Max();
            
            case 5: // GREATER THAN
                return packageValues.Length >= 2 && packageValues[0] > packageValues[1] ? 1 : 0;
            
            case 6: // LESS THAN
                return packageValues.Length >= 2 && packageValues[0] < packageValues[1] ? 1 : 0;
            
            case 7: // EQUALS
                return packageValues.Length >= 2 && packageValues[0] == packageValues[1] ? 1 : 0;
            
            default:
                return 0;
        }
    }
}
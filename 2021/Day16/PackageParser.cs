namespace Day16;

public static class PackageParser
{
    public static Package Parse(ref string inputBin)
    {
        Package package;
        SpliceNumber(ref inputBin, out var version, 3);
        SpliceNumber(ref inputBin, out var packageId, 3);

        if (packageId == 4) // literal number
        {
            package = new LiteralPackage();
        }
        else // operator
        {
            package = new OperatorPackage(packageId);
        }
        package.SetVersion(version);
        package.RegisterPayload(ref inputBin);
        
        return package;
    }

    public static void SpliceNumber(ref string inputBin, out int result, int length)
    {
        var cLength = length < inputBin.Length ? length : inputBin.Length;
        var strResult = inputBin.Substring(0, cLength);
        inputBin = inputBin.Substring(cLength);
        if (inputBin == "")
        {
            inputBin = "0";
        }
        result = Convert.ToInt32(strResult, 2);
    }

    public static void SpliceString(ref string inputBin, out string result, int length)
    {
        var cLength = length < inputBin.Length ? length : inputBin.Length;
        result = inputBin.Substring(0, cLength);
        inputBin = inputBin.Substring(cLength);
    }
}
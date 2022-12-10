namespace Day10;

public class Memory
{
    private IList<string> mInstructions;

    public Memory()
    {
        mInstructions = File.ReadAllLines("input.txt").ToList();
    }

    public string PopNextInstruction()
    {
        var instruction = mInstructions.First();
        mInstructions.RemoveAt(0);
        return instruction;
    }

    public bool HasInstructions => mInstructions.Any();
}
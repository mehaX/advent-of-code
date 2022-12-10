namespace Day10.Models;

public class Memory
{
    private readonly IList<string> mInstructions;

    public Memory(IList<string> instructions)
    {
        mInstructions = instructions;
    }

    public string PopNextInstruction()
    {
        var instruction = mInstructions.First();
        mInstructions.RemoveAt(0);
        return instruction;
    }

    public bool HasInstructions => mInstructions.Any();
}
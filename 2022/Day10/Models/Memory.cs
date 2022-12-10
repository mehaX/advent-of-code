namespace Day10.Models;

public class Memory
{
    private readonly Queue<string> mInstructions;

    public Memory(IEnumerable<string> instructions)
    {
        mInstructions = new Queue<string>(instructions);
    }

    public string PopNextInstruction()
    {
        return mInstructions.Dequeue();
    }

    public bool HasInstructions => mInstructions.Any();
}
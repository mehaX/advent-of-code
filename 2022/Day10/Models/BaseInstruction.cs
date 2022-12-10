namespace Day10.Models;

internal abstract class BaseInstruction
{
    private readonly int mMaxCount;
    private int mCount = 0;

    public bool IsComplete => mCount == mMaxCount;

    protected BaseInstruction(int count)
    {
        mMaxCount = count;
    }
    
    public void Run()
    {
        mCount++;
        if (IsComplete)
        {
            Execute();
        }
    }

    protected abstract void Execute();
}
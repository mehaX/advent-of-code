internal abstract class BaseInstruction
{
    private readonly int mMaxCount;
    private int mCount = 0;

    protected BaseInstruction(int count)
    {
        mMaxCount = count;
    }
    
    public bool Run()
    {
        mCount++;
        if (mCount == mMaxCount)
        {
            Execute();
        }

        return mCount == mMaxCount;
    }

    protected abstract void Execute();
}
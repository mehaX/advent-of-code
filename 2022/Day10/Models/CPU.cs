namespace Day10;

internal class CPU
{
    private readonly Memory mMemory;
    private readonly Register mValueRegister;
    private readonly Register mCycleRegister;
    
    private BaseInstruction? mStoredComponent;

    public bool InProgress => mStoredComponent != null;

    public CPU(Memory memory, Register valueRegister, Register cycleRegister)
    {
        mMemory = memory;
        mValueRegister = valueRegister;
        mCycleRegister = cycleRegister;
    }
    
    public void BeginCycle()
    {
        if (mStoredComponent == null)
        {
            GenerateStoredComponent();
        }
    }

    public void EndCycle()
    {
        var done = mStoredComponent.Run();
        if (done)
        {
            mStoredComponent = null;
        }

        mCycleRegister.Add(1);
    }

    private void GenerateStoredComponent()
    {
        var nextInstruction = mMemory.PopNextInstruction();
        
        if (nextInstruction == "noop")
        {
            mStoredComponent = new NoopInstruction();
        }
        else
        {
            var value = int.Parse(nextInstruction.Split(" ")[1]);
            mStoredComponent = new AddInstruction(mValueRegister, value);
        }
    }
}
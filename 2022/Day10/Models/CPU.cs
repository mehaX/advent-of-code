namespace Day10;

internal class CPU
{
    private readonly Memory mMemory;
    private readonly Register mValueRegister;
    private readonly Register mCycleRegister;
    
    private BaseComponent? mStoredComponent;

    public bool InProgress => mStoredComponent != null;

    public CPU(Memory memory, Register valueRegister, Register cycleRegister)
    {
        mMemory = memory;
        mValueRegister = valueRegister;
        mCycleRegister = cycleRegister;
    }
    
    public void RunCycle()
    {
        if (mStoredComponent == null)
        {
            GenerateStoredComponent();
        }

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
            mStoredComponent = new NoopBaseComponent();
        }
        else
        {
            var value = int.Parse(nextInstruction.Split(" ")[1]);
            mStoredComponent = new AddBaseComponent(mValueRegister, value);
        }
    }
}
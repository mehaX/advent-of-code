namespace Day10.Models;

internal class CPU
{
    private readonly Memory mMemory;
    private readonly Register mValueRegister;
    private readonly Register mCycleRegister;
    
    private BaseInstruction? mRunningInstruction;

    public bool IsRunning => mRunningInstruction != null || mMemory.HasInstructions;

    public CPU(Memory memory, Register valueRegister, Register cycleRegister)
    {
        mMemory = memory;
        mValueRegister = valueRegister;
        mCycleRegister = cycleRegister;
    }
    
    public void BeginCycle()
    {
        if (mRunningInstruction == null)
        {
            GenerateInstruction();
        }
    }

    public void EndCycle()
    {
        var done = mRunningInstruction.Run();
        if (done)
        {
            mRunningInstruction = null;
        }

        mCycleRegister.Increase(1);
    }

    private void GenerateInstruction()
    {
        var nextInstruction = mMemory.PopNextInstruction();
        
        if (nextInstruction == "noop")
        {
            mRunningInstruction = new NoopInstruction();
        }
        else
        {
            var value = int.Parse(nextInstruction.Split(" ")[1]);
            mRunningInstruction = new AddInstruction(mValueRegister, value);
        }
    }
}
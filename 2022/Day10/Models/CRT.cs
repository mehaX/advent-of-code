namespace Day10;

internal class CRT
{
    private readonly Register mValueRegister;
    private readonly Register mCycleRegister;
    public CRT(Register valueRegister, Register cycleRegister)
    {
        mValueRegister = valueRegister;
        mCycleRegister = cycleRegister;
    }

    public string Draw()
    {
        var sprite = "";
        var crtPos = (mCycleRegister.Value - 1) % 40;
        if (crtPos == 0)
        {
            sprite += "\n";
        }
        
        sprite += (crtPos >= mValueRegister.Value - 1 && crtPos <= mValueRegister.Value + 1 ? "#" : ".");
        return sprite;
    }
}
namespace Day10.Models;

internal class AddInstruction : BaseInstruction
{
    private readonly Register mRegister;
    private readonly int mValue;

    public AddInstruction(Register register, int value) : base(2)
    {
        mRegister = register;
        mValue = value;
    }

    protected override void Execute()
    {
        mRegister.Increase(mValue);
    }
}
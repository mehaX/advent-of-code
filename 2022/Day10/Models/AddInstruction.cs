namespace Day10;

internal class AddInstruction : BaseInstruction
{
    private readonly Register mRegister;
    private int mValue;

    public AddInstruction(Register register, int value) : base(2)
    {
        mRegister = register;
        mValue = value;
    }

    protected override void Execute()
    {
        mRegister.Add(mValue);
    }
}
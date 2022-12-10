namespace Day10;

internal class AddBaseComponent : BaseComponent
{
    private readonly Register mRegister;
    private int mValue;

    public AddBaseComponent(Register register, int value) : base(2)
    {
        mRegister = register;
        mValue = value;
    }

    protected override void Execute()
    {
        mRegister.Add(mValue);
    }
}
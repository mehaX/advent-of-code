namespace Day10.Models;

internal class NoopInstruction : BaseInstruction
{
    public NoopInstruction() : base(1)
    {
    }

    protected override void Execute()
    {
        // totally ignored
    }
}
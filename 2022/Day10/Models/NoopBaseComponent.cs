internal class NoopBaseComponent : BaseComponent
{
    public NoopBaseComponent() : base(1)
    {
    }

    protected override void Execute()
    {
        // nothing to do here
    }
}
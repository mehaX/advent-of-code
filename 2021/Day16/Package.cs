namespace Day16;

public abstract class Package
{
    protected readonly int Id;
    protected int Version { get; private set; }

    protected Package(int id)
    {
        Id = id;
    }

    public void SetVersion(int version)
    {
        Version = version;
    }

    public abstract void RegisterPayload(ref string payload);

    public virtual int CalculateVersionSum()
    {
        return Version;
    }

    public abstract long CalculateResult();
}
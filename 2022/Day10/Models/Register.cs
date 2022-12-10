namespace Day10.Models;

internal class Register
{
    public int Value { get; private set; }

    public Register()
    {
        Value = 1;
    }

    public void Increase(int value)
    {
        Value += value;
    }
}
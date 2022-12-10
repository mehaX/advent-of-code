namespace Day10.Models;

internal class Register
{
    public int Value { get; private set; }

    public Register(int initValue)
    {
        Value = initValue;
    }

    public void Increase(int value)
    {
        Value += value;
    }
}
public class Command
{
    public string Cmd { get; set; }
    public string Params { get; set; }
    public List<string> Output { get; set; }

    public Command(string input, List<string> output)
    {
        var inputChunks = input.Split(" ", 2);
        Cmd = inputChunks.First();
        Params = inputChunks.ElementAtOrDefault(1) ?? "";
        Output = output;
    }
}
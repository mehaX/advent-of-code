public class Terminal
{
    private readonly List<Command> mCommands = new();
    private readonly FileSystem mFileSystem;

    public Terminal(FileSystem fileSystem)
    {
        mFileSystem = fileSystem;
    }
    
    public Terminal Deserialize(List<string> rows)
    {
        var input = "";
        var output = new List<string>();

        foreach (var row in rows)
        {
            if (!row.StartsWith("$"))
            {
                output.Add(row);
                continue;
            }
            
            if (!string.IsNullOrEmpty(input))
            {
                mCommands.Add(new Command(input, output));
            }

            input = row.Replace("$ ", "");
            output = new();
        }
        
        if (!string.IsNullOrEmpty(input))
        {
            mCommands.Add(new Command(input, output));
        }

        return this;
    }

    public Terminal Execute()
    {
        foreach (var command in mCommands)
        {
            switch (command.Cmd)
            {
                case "cd":
                    mFileSystem.ChangeDir(command.Params);
                    break;
                
                case "ls":
                {
                    var dirs = command.Output.Where(row => row.StartsWith("dir")).ToList();
                    var files = command.Output.Where(row => !row.StartsWith("dir")).ToList();
                    
                    mFileSystem.AddDirs(dirs);
                    mFileSystem.AddFiles(files);
                    break;
                }
                
                default:
                    throw new Exception($"Command not found {command.Cmd}");
            }
        }

        return this;
    }
}
Console.WriteLine("Part 1: " + Part1());

long Part1()
{
    var fileSystem = new FileSystem();
    var terminal = new Terminal(fileSystem);
    var input = File.ReadLines("input.txt").ToList();

    terminal.Deserialize(input).Execute();

    var result = 0L;
    foreach (var dir in fileSystem.ListDirectories())
    {
        var dirSize = fileSystem.DeepListFiles(dir).Sum(file => file.Size);
        if (dirSize <= 100_000)
        {
            result += dirSize;
        }
    }
    return result;
}

class Terminal
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
            if (row.StartsWith("$"))
            {
                if (!string.IsNullOrEmpty(input))
                {
                    mCommands.Add(new Command(input, output));
                }

                input = row.Replace("$ ", "");
                output = new();
                continue;
            }
            
            output.Add(row);
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
                    var files = command.Output.Where(row => !row.StartsWith("dir")).ToList();
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

class Command
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

class FileSystem
{
    private List<InputFile> Files { get; } = new();
    private List<string> mCurrentDir = new();

    public string CurrentDir => "/" + (mCurrentDir.Any() ? string.Join("/", mCurrentDir) + "/" : "");
    
    public void ChangeDir(string dir)
    {
        switch (dir)
        {
            case "..":
                if (mCurrentDir.Any())
                {
                    mCurrentDir.RemoveAt(mCurrentDir.Count - 1);
                }
                break;
            
            case "/":
                mCurrentDir.Clear();
                break;
            
            default:
                mCurrentDir.Add(dir);
                break;
        }
    }

    public void AddFiles(IEnumerable<string> files)
    {
        Files.AddRange(files.Select(fileRow =>
        {
            var chunks = fileRow.Split(" ");
            var fileSize = long.Parse(chunks[0]);
            var fileName = chunks[1];
            return new InputFile(fileName, CurrentDir, fileSize);
        }));
    }

    public IEnumerable<InputFile> DeepListFiles(string dir)
    {
        return Files.Where(file => file.DirPath.StartsWith(dir));
    }

    public IEnumerable<string> ListDirectories()
    {
        return Files.Select(file => file.DirPath).Distinct();
    }
}

class InputFile
{
    public string FileName { get; }
    public string DirPath { get; }
    
    public long Size { get; }

    public string FilePath => $"{DirPath}{FileName}";

    public InputFile(string fileName, string dirPath, long size)
    {
        FileName = fileName;
        DirPath = dirPath;
        Size = size;
    }
}
Console.WriteLine("Part 1: " + Part1());
Console.WriteLine("Part 2: " + Part2());

long Part1()
{
    var fileSystem = new FileSystem();
    var terminal = new Terminal(fileSystem);
    var input = File.ReadLines("input.txt").ToList();

    terminal.Deserialize(input).Execute();

    return fileSystem
        .ListDirectories()
        .Select(fileSystem.CalculateDirSize)
        .Where(dirSize => dirSize <= 100_000)
        .Sum();
}

long Part2()
{
    var threshold = 30_000_000L;
    var totalSpace = 70_000_000L;
    var fileSystem = new FileSystem();
    var terminal = new Terminal(fileSystem);
    var input = File.ReadLines("input.txt").ToList();

    terminal.Deserialize(input).Execute();

    var used = fileSystem.CalculateDirSize();
    var sizes = fileSystem.ListDirectories().Select(fileSystem.CalculateDirSize).ToList();
    return sizes.Order().First(size => used - size < totalSpace - threshold);
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
    private readonly List<string> mDirs = new(); // stupid idea, because we have to store empty dirs too (thank you debugger for wasting my time)
    private readonly List<InputFile> mFiles = new();
    private readonly List<string> mCurrentDir = new();

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

    public void AddDirs(IEnumerable<string> dirs)
    {
        mDirs.AddRange(dirs.Select(dir => CurrentDir + dir.Replace("dir ", "") + "/"));
    }

    public void AddFiles(IEnumerable<string> files)
    {
        mFiles.AddRange(files.Select(fileRow =>
        {
            var chunks = fileRow.Split(" ");
            var fileSize = long.Parse(chunks[0]);
            var fileName = chunks[1];
            return new InputFile(fileName, CurrentDir, fileSize);
        }));
    }

    public long CalculateDirSize(string dir = "/")
    {
        return mFiles.Where(file => file.DirPath.StartsWith(dir)).Sum(file => file.Size);
    }

    public IEnumerable<string> ListDirectories()
    {
        return mDirs;
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
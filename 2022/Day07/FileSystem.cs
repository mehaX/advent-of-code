public class FileSystem
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
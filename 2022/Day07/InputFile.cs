public class InputFile
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
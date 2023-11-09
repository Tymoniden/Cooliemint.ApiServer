namespace Cooliemint.ApiServer.Services;

public interface IFileSystemService
{
    void WriteAllText(string relativePath, string content);
    string ReadAllText(string relativePath);
}
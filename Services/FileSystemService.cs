namespace Cooliemint.ApiServer.Services
{
    public class FileSystemService : IFileSystemService
    {
        public void WriteAllText(string relativePath, string content)
        {
            File.WriteAllText(Path.Combine(GetRoot(), relativePath), content);
        }

        public string ReadAllText(string relativePath)
        {
            return File.ReadAllText(Path.Combine(GetRoot(), relativePath));
        }

        private static string GetRoot() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "cooliemint");
    }
}

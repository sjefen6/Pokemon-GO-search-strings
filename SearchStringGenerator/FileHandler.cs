namespace SearchStringGenerator;

public class FileHandler
{
    public static void SaveFile(string content, string filename)
    {
        var filepath = Path.IsPathRooted(filename) ? filename : PathFromSolution(filename);
        File.WriteAllText(filepath, content);
    }

    public static string PathFromSolution(string path)
    {
        return Path.Combine(FindSolutionDirectory(), path);
    }

    private static string FindSolutionDirectory()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory != null)
        {
            if (Directory.GetFiles(directory.FullName, "*.sln").Length > 0)
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Solution directory not found.");
    }

    public static string Readme => PathFromSolution("README.md");
}
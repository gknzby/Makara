
namespace Makara.Helpers
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            return System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);
        }

        public static string GetProjectPath(string filename)
        {
            return System.IO.Path.Combine("C:\\dev\\Makara\\Data\\", filename);
        }
    }
}

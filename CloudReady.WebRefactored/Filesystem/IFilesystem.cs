using System.IO;
using System.Threading.Tasks;

namespace CloudReady.WebRefactored.Filesystem
{
    public interface IFilesystemWriter
    {
        Task SaveAsync(string path, Stream file);
    }

    public interface IFilesystemReader
    {
        Task<string> GetUrlAsync(string path);
    }
}

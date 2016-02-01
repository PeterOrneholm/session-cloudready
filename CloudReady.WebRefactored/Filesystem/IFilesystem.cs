using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CloudReady.WebRefactored.Filesystem
{
    public interface IFilesystemWriter
    {
        Task SaveAsync(string path, Stream file);
    }

    public interface IFilesystemReader
    {
        Task<Stream> ReadAsync(string path);

        Task<string> GetUrlAsync(string path);
    }
}

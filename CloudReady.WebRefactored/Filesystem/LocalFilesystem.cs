using System.IO;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Web;

namespace CloudReady.WebRefactored.Filesystem
{
    public class FilesystemWriter : IFilesystemWriter, IFilesystemReader
    {
        public async Task SaveAsync(string path, Stream file)
        {
            var localpath = HttpContext.Current.Server.MapPath("~" + path);
            using (var localFileStream = new FileStream(localpath, FileMode.Create, FileAccess.Write))
            {
                file.Seek(0, SeekOrigin.Begin);
                await file.CopyToAsync(localFileStream);
            }
        }

        public async Task<Stream> ReadAsync(string path)
        {
            var localpath = HttpContext.Current.Server.MapPath("~" + path);

            var stream = new MemoryStream();
            using (var fileStream = new FileStream(localpath, FileMode.Open, FileAccess.Read))
            {
                await fileStream.CopyToAsync(stream);
            }

            return stream;
        }

        public Task<string> GetUrlAsync(string path)
        {
            return Task.FromResult(path);
        }
    }
}
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace CloudReady.WebRefactored.Filesystem
{
    public class LocalFilesystem : IFilesystemWriter, IFilesystemReader
    {
        public async Task SaveAsync(string path, Stream file)
        {
            var localpath = HttpContext.Current.Server.MapPath("~/" + path);
            using (var localFileStream = new FileStream(localpath, FileMode.Create, FileAccess.Write))
            {
                file.Seek(0, SeekOrigin.Begin);
                await file.CopyToAsync(localFileStream);
            }
        }

        public Task<string> GetUrlAsync(string path)
        {
            return Task.FromResult("/" + path);
        }
    }
}
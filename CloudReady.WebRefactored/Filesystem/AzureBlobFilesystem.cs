using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CloudReady.WebRefactored.Filesystem
{
    public class AzureBlobFilesystem : IFilesystemWriter, IFilesystemReader
    {
        private const string ConnectionString = "[Secret]";

        private readonly CloudBlobContainer _container;

        public AzureBlobFilesystem()
        {
            var storageAccount = CloudStorageAccount.Parse(ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference("images");
        }

        public async Task SaveAsync(string path, Stream file)
        {
            var blockBlob = _container.GetBlockBlobReference(path);

            using (var azureFileStream = new MemoryStream())
            {
                file.Seek(0, SeekOrigin.Begin);
                await file.CopyToAsync(azureFileStream);
                azureFileStream.Seek(0, SeekOrigin.Begin);

                await blockBlob.UploadFromStreamAsync(azureFileStream);
            }

            await blockBlob.SetPropertiesAsync();
        }
        
        public Task<string> GetUrlAsync(string path)
        {
            var blockBlob = _container.GetBlockBlobReference(path);
            return Task.FromResult(blockBlob.Uri.AbsoluteUri);
        }
    }
}
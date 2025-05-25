using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Infra.Data.Services
{
    public class GoogleCloudStorageService : IGoogleCloudStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public GoogleCloudStorageService(IConfiguration config)
        {
            _storageClient = StorageClient.Create();
            _bucketName = config["GoogleCloud:BucketName"];
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileNameForStorage, string contentType)
        {
            var result = await _storageClient.UploadObjectAsync(
                bucket: _bucketName,
                objectName: fileNameForStorage,
                contentType: contentType,
                source: fileStream);

            return $"https://storage.googleapis.com/{_bucketName}/{fileNameForStorage}";
        }

        public async Task DeleteFileAsync(string fileNameForStorage)
        {
            await _storageClient.DeleteObjectAsync(_bucketName, fileNameForStorage);
        }
    }
}

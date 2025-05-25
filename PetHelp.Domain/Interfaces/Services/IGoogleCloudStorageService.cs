namespace PetHelp.Domain.Interfaces.Services;

public interface IGoogleCloudStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileNameForStorage, string contentType);
    Task DeleteFileAsync(string fileNameForStorage);
}

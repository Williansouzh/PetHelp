using PetHelp.Application.Interfaces;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Application.Services;

public class ImageService : IImageService
{
    private readonly IGoogleCloudStorageService _googleCloudStorageService;

    public ImageService(IGoogleCloudStorageService googleCloudStorageService)
    {
        _googleCloudStorageService = googleCloudStorageService;
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType)
    {
        var uniqueFileName = GenerateUniqueFileName(fileName);
        return await _googleCloudStorageService.UploadFileAsync(imageStream, uniqueFileName, contentType);
    }
    public async Task DeleteImageAsync(string fileNameForStorage)
    {
        await _googleCloudStorageService.DeleteFileAsync(fileNameForStorage);
    }
    private string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var uniqueName = $"{Guid.NewGuid()}{extension}";
        return $"images/{uniqueName}";
    }
}

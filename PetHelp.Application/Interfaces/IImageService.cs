namespace PetHelp.Application.Interfaces;

public interface IImageService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType);
    Task DeleteImageAsync(string fileNameForStorage);
}

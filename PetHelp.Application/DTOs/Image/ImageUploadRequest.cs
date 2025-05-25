using Microsoft.AspNetCore.Http;

namespace PetHelp.Application.DTOs.Image;

public class ImageUploadRequest
{
    public IFormFile Image { get; set; } = null!;
}
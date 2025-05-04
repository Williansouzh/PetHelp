using System.ComponentModel.DataAnnotations;

namespace PetHelp.API.DTOs.UserDTOs;

public class UserLoginDTO
{
    public String Role { get; set; }
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [StringLength(100, ErrorMessage = "Email length can't be more than 100.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "Password length can't be more than 100.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

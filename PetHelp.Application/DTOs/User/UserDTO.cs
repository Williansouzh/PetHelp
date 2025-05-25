using System.ComponentModel.DataAnnotations;

namespace PetHelp.API.DTOs.UserDTOs;

public class UserDTO
{
    [Required]
    public string Role { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [StringLength(100, ErrorMessage = "Email length can't be more than 100.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "Password length can't be more than 100.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [StringLength(50, ErrorMessage = "Name can't be more than 50 characters.")]
    public string? Name { get; set; }

    [StringLength(50, ErrorMessage = "Last name can't be more than 50 characters.")]
    public string? LastName { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(20)]
    public string? Phone { get; set; }
}

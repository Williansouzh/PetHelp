using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PetHelp.API.DTOs.UserDTOs;
using PetHelp.Domain.Account;
using PetHelp.Domain.Interfaces.Services;
using PetHelp.Infra.Data.Identity;

namespace PetHelp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticate _authentication;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    public AuthController(
        IAuthenticate authentication,
        ITokenService tokenService,
        ILogger<AuthController> logger,
        UserManager<ApplicationUser> userManager)
    {
        _authentication = authentication;
        _tokenService = tokenService;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] UserDTO userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authentication.RegisterUser(
            userDto.Email,
            userDto.Password,
            userDto.Role,
            userDto.Name,
            userDto.LastName,
            userDto.Phone);

        if (!result)
            return Conflict(new { Message = "User already exists" });

        _logger.LogInformation($"New user registered: {userDto.Email}");

        return Ok(new AuthResponse
        {
            Message = "User created successfully",
            Success = true
        });
    }

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<ActionResult<AuthTokenResponse>> Login([FromBody] UserLoginDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new AuthTokenResponse
            {
                Success = false,
                Message = "Invalid request"
            });

        var authUser = await _authentication.Authenticate(dto.Email, dto.Password);
        if (authUser == null)
            return Unauthorized(new AuthTokenResponse
            {
                Success = false,
                Message = "Invalid credentials"
            });

        return new AuthTokenResponse
        {
            AccessToken = _tokenService.GenerateToken(authUser),
            RefreshToken = await _tokenService.GenerateAndStoreRefreshToken(authUser.Email),
            ExpiresIn = 3600,
            Success = true
        };
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var user = await _authentication.GetCurrentUser();
            if (user != null)
            {
                await _tokenService.RevokeRefreshToken(user.Email);
            }

            await _authentication.Logout();
            return Ok(new { Message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return StatusCode(500, new { Message = "Internal server error" });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new AuthTokenResponse { Success = false, Message = "Invalid request" });

        // Validate refresh token
        var isValid = await _authentication.ValidateRefreshToken(request.Email, request.RefreshToken);
        if (!isValid)
            return Unauthorized(new AuthTokenResponse { Success = false, Message = "Invalid refresh token" });

        // Get user details
        var user = await _authentication.GetCurrentUser();
        if (user == null || user.Email != request.Email)
            return Unauthorized(new AuthTokenResponse { Success = false, Message = "User not found" });

        // Generate new tokens
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        await _authentication.UpdateRefreshToken(request.Email, newRefreshToken);

        return Ok(new AuthTokenResponse
        {
            AccessToken = _tokenService.GenerateToken(user),
            RefreshToken = newRefreshToken,
            ExpiresIn = 3600,
            Success = true
        });
    }
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<object>> GetLoggedUser()
    {
        if (User?.Claims == null || !User.Claims.Any())
        {
            return Unauthorized(new { Message = "User not logged in" });
        }

        var claims = User.Claims;

        var userInfo = new
        {
            Id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
            Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            Role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
            IssuedAt = GetDateTimeFromUnix(claims.FirstOrDefault(c => c.Type == "iat")?.Value),
            ExpiresAt = GetDateTimeFromUnix(claims.FirstOrDefault(c => c.Type == "exp")?.Value)
        };

        return Ok(userInfo);
    }


    private DateTime? GetDateTimeFromUnix(string? unixTime)
    {
        if (long.TryParse(unixTime, out var seconds))
        {
            return DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
        }
        return null;
    }


}

// Supporting DTOs
public class AuthResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
}

public class AuthTokenResponse : AuthResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
}

public class RefreshTokenRequest
{
    public string Email { get; set; }
    public string RefreshToken { get; set; }
}
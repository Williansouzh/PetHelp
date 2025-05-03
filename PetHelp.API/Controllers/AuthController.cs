using Microsoft.AspNetCore.Mvc;
using PetHelp.API.DTOs.UserDTOs;
using PetHelp.Domain.Account;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticate _authentication;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthenticate authentication,
        ITokenService tokenService,
        ILogger<AuthController> logger)
    {
        _authentication = authentication;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] UserDTO userDto)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return StatusCode(500, new { Message = "Internal server error" });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthTokenResponse>> Login([FromBody] UserLoginDTO userLoginDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authResult = await _authentication.Authenticate(userLoginDto.Email, userLoginDto.Password);

            if (!authResult)
                return Unauthorized(new { Message = "Invalid credentials" });

            var token = _tokenService.GenerateAccessToken(userLoginDto.Email);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Store refresh token (implementation depends on your storage)
            await _authentication.StoreRefreshToken(userLoginDto.Email, refreshToken);

            _logger.LogInformation($"User logged in: {userLoginDto.Email}");

            return Ok(new AuthTokenResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresIn = 3600 // 1 hour in seconds
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return StatusCode(500, new { Message = "Internal server error" });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _authentication.Logout();
            // Optionally revoke refresh token here
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
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isValid = await _authentication.ValidateRefreshToken(request.Email, request.RefreshToken);
            if (!isValid)
                return Unauthorized(new { Message = "Invalid refresh token" });

            var newToken = _tokenService.GenerateRefreshToken();
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            await _authentication.UpdateRefreshToken(request.Email, newRefreshToken);

            return Ok(new AuthTokenResponse
            {
                AccessToken = newToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = 3600
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode(500, new { Message = "Internal server error" });
        }
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
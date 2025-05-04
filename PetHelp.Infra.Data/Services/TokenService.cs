using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PetHelp.Domain.Account;

namespace PetHelp.Infra.Data.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IAuthenticate _authentication;
    private readonly ILogger<TokenService> _logger;
    private readonly SymmetricSecurityKey _signingKey;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenService(
    IConfiguration configuration,
    IAuthenticate authentication,
    ILogger<TokenService> logger)
    {
        _configuration = configuration;
        _authentication = authentication;
        _logger = logger;

        var secretKey = _configuration["Jwt:SecretKey"];
        if (string.IsNullOrEmpty(secretKey) || Encoding.UTF8.GetByteCount(secretKey) < 32)
        {
            throw new ArgumentException("JWT SecretKey must be at least 256 bits (32 characters)");
        }

        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public string GenerateToken(AuthUser user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim("id", user.Id),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Roles.FirstOrDefault() ?? "User")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(GetAccessTokenExpirationMinutes()),
            SigningCredentials = new SigningCredentials(
                _signingKey,
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<string> GenerateAndStoreRefreshToken(string email)
    {
        var refreshToken = GenerateRefreshToken();
        var stored = await _authentication.StoreRefreshToken(email, refreshToken);
        return stored ? refreshToken : null;
    }

    public bool ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        try
        {
            _tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogWarning("Token validation failed: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<bool> ValidateRefreshToken(string email, string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(refreshToken))
            return false;

        return await _authentication.ValidateRefreshToken(email, refreshToken);
    }

    public string GetEmailFromToken(string token)
    {
        var principal = GetPrincipalFromToken(token);
        return principal?.FindFirstValue(ClaimTypes.Email);
    }

    public string GetUserIdFromToken(string token)
    {
        var principal = GetPrincipalFromToken(token);
        return principal?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public string GetRoleFromToken(string token)
    {
        var principal = GetPrincipalFromToken(token);
        return principal?.FindFirstValue(ClaimTypes.Role);
    }

    public async Task<bool> UpdateRefreshToken(string email, string newRefreshToken)
    {
        return await _authentication.UpdateRefreshToken(email, newRefreshToken);
    }

    public async Task<bool> RevokeRefreshToken(string email)
    {
        return await _authentication.UpdateRefreshToken(email, null);
    }

    public int GetAccessTokenExpirationMinutes()
    {
        return _configuration.GetValue("Jwt:AccessTokenExpirationMinutes", 15);
    }

    public int GetRefreshTokenExpirationDays()
    {
        return _configuration.GetValue("Jwt:RefreshTokenExpirationDays", 7);
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            return _tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get principal from token");
            return null;
        }
    }
}
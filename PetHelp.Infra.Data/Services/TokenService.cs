using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Infra.Data.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _signingKey;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public string GenerateAccessToken(string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, email)
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

    public string GenerateEmailConfirmationToken(string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("token_type", "email_confirmation")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24), // 24 hour expiration
            SigningCredentials = new SigningCredentials(
                _signingKey,
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public string GeneratePasswordResetToken(string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("token_type", "password_reset")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1), // 1 hour expiration
            SigningCredentials = new SigningCredentials(
                _signingKey,
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public bool ValidateAccessToken(string token)
    {
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
        catch
        {
            return false;
        }
    }

    public bool ValidateRefreshToken(string token)
    {
        // For refresh tokens, you might want to:
        // 1. Check if it exists in your database
        // 2. Check if it's revoked
        // 3. Check expiration if stored with expiry date
        // This is a basic implementation
        return !string.IsNullOrWhiteSpace(token);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _signingKey,
            ValidateIssuer = false, // Might want to validate in some cases
            ValidateAudience = false,
            ValidateLifetime = false // We're checking expired tokens
        };

        return _tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
    }

    public string GetEmailFromToken(string token)
    {
        var principal = GetPrincipalFromExpiredToken(token);
        return principal.FindFirstValue(ClaimTypes.Email);
    }

    public int GetAccessTokenExpirationMinutes()
    {
        return _configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15);
    }

    public int GetRefreshTokenExpirationDays()
    {
        return _configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7);
    }
}
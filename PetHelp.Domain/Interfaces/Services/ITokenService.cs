using System.Security.Claims;

namespace PetHelp.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(string email);
    string GenerateRefreshToken();

    bool ValidateAccessToken(string token);
    bool ValidateRefreshToken(string token);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    string GetEmailFromToken(string token);

    string GenerateEmailConfirmationToken(string email);
    string GeneratePasswordResetToken(string email);

    int GetAccessTokenExpirationMinutes();
    int GetRefreshTokenExpirationDays();
}
using PetHelp.Domain.Account;

public interface ITokenService
{
    // Token Generation
    string GenerateToken(AuthUser user);
    string GenerateRefreshToken();
    Task<string> GenerateAndStoreRefreshToken(string email);

    // Token Validation
    bool ValidateToken(string token);
    Task<bool> ValidateRefreshToken(string email, string refreshToken);

    // Token Claims
    string GetEmailFromToken(string token);
    string GetUserIdFromToken(string token);
    string GetRoleFromToken(string token);

    // Token Management
    Task<bool> UpdateRefreshToken(string email, string newRefreshToken);
    Task<bool> RevokeRefreshToken(string email);
}
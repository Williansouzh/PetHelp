using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetHelp.Domain.Account;

namespace PetHelp.Infra.Data.Identity;

public class AuthenticateService : IAuthenticate
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    ILogger<AuthenticateService> _logger;
    public AuthenticateService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<AuthenticateService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<bool> RegisterUser(string email, string password, string role, string name, string lastName, string phone)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            Name = name,
            LastName = lastName,
            Phone = phone,
        };

        var result = await _userManager.CreateAsync(applicationUser, password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Falha ao criar usuário: {ErrorDescription}", error.Description);
            }
            return false;
        }

        var user = await _userManager.FindByEmailAsync(email);
        var roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            _logger.LogError("A role '{Role}' não existe.", role);
            return false;
        }

        var roleResult = await _userManager.AddToRoleAsync(user, role);
        return roleResult.Succeeded;
    }
    public async Task<AuthUser?> Authenticate(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            password,
            lockoutOnFailure: true);

        if (!result.Succeeded) return null;

        var roles = await _userManager.GetRolesAsync(user);
        _logger.LogInformation("Login attempt for {Email} from {IP}", email, _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress);
        return new AuthUser
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            LastName = user.LastName,
            Phone = user.Phone,
            Roles = await _userManager.GetRolesAsync(user)
        };
    }

    public async Task<ApplicationUser?> RegisterUser(string email, string password, string role)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(applicationUser, password);
        if (!result.Succeeded) return null;

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }

        var roleResult = await _userManager.AddToRoleAsync(user, role);
        return roleResult.Succeeded ? user : null;
    }
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> StoreRefreshToken(string email, string refreshToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var existingClaims = await _userManager.GetClaimsAsync(user);
        var existingRefreshToken = existingClaims.FirstOrDefault(c => c.Type == "refreshToken");

        if (existingRefreshToken != null)
            await _userManager.RemoveClaimAsync(user, existingRefreshToken);

        var newClaim = new Claim("refreshToken", refreshToken);
        var result = await _userManager.AddClaimAsync(user, newClaim);

        return result.Succeeded;
    }


    public async Task<bool> UpdateRefreshToken(string email, string newRefreshToken)
    {
        return await StoreRefreshToken(email, newRefreshToken);
    }

    public async Task<bool> ValidateRefreshToken(string email, string refreshToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var claims = await _userManager.GetClaimsAsync(user);
        var storedRefreshToken = claims.FirstOrDefault(c => c.Type == "refreshToken")?.Value;

        return storedRefreshToken == refreshToken;
    }
    public async Task<AuthUser?> GetCurrentUser()
    {
        var principal = _httpContextAccessor.HttpContext?.User;
        if (principal == null) return null;

        var user = await _userManager.GetUserAsync(principal);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new AuthUser
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            LastName = user.LastName,
            Phone = user.Phone,
            Roles = roles
        };
    }
    public async Task<bool> IsUserLoggedIn()
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        return user != null;
    }
}

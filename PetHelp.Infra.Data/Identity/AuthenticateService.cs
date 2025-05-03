using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetHelp.Domain.Account;

namespace PetHelp.Infra.Data.Identity;

public class AuthenticateService : IAuthenticate
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    ILogger<AuthenticateService> _logger;
    public AuthenticateService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<AuthenticateService> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }
    public async Task<bool> Authenticate(string email, string password)
    {
        _logger.LogInformation("Tentando autenticar o usuário: {Email}", email);
        var user = await _userManager.FindByEmailAsync(email);
        if(user == null)
        {
            Console.WriteLine($"[ERRO] Usuário não encontrado: {email}");
            return false;
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        _logger.LogError("Usuário não encontrado: {Email}", email);
        return result.Succeeded;

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
            Role = role
        };

        var result = await _userManager.CreateAsync(applicationUser, password);
        if (!result.Succeeded)
        {
            foreach(var error in result.Errors)
            {
                Console.WriteLine($"[ERRO] Falha ao criar usuário: {error.Description}");
            }
            return false;
        }
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            Console.WriteLine($"[ERRO] Usuário não encontrado no banco, após criação.");
            return false;
        }
        var roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            _logger.LogError("A role '{Role}' não existe.", role);
            return false;
        }
        var roleResult = await _userManager.AddToRoleAsync(user, role);
        if(!roleResult.Succeeded)
        {
            foreach (var error in roleResult.Errors)
            {
                Console.WriteLine($"[ERRO] Falha ao adicionar usuário à role: {error.Description}");
            }
            return false;
        }
        await _signInManager.SignInAsync(user, isPersistent: false);
        Console.WriteLine("[SUCESSO] Usuário registrado e autenticado.");
        return true;
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
}

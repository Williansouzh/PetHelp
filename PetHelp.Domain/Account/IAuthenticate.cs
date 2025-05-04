using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHelp.Domain.Account;

public interface IAuthenticate
{
    Task<bool> RegisterUser(string email, string password, string role, string name, string lastName, string phone);
    Task<AuthUser> Authenticate(string email, string password);
    Task Logout();
    Task<bool> StoreRefreshToken(string email, string refreshToken);
    Task<bool> UpdateRefreshToken(string email, string newRefreshToken);
    Task<bool> ValidateRefreshToken(string email, string refreshToken);
    Task<bool> IsUserLoggedIn();
    Task<AuthUser?> GetCurrentUser();
}
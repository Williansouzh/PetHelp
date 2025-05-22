using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PetHelp.Domain.Account;

namespace PetHelp.Infra.Data.Identity;

public  class SeedUserRoleInitial : ISeedUserRoleInitial
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedUserRoleInitial(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    public async Task SeedUsersAsync()
    {
        if(await _userManager.FindByEmailAsync("adopt@localhost") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "adopt@localhost",
                LastName = "",
                Name="",
                Phone = "98777654324",
                Email = "adopt@localhost",
                EmailConfirmed = true,
            };
            var resul = await _userManager.CreateAsync(user, "FB1mF@ln*"); 
            if(resul.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Adopter");
            }
        }
        if (await _userManager.FindByEmailAsync("ong@localhost") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "ong@localhost",
                Email = "ong@localhost",
                Name = "Ong",
                Phone = "98777654324",
                LastName = "Lastname",
                EmailConfirmed = true,
            };
            var resul = await _userManager.CreateAsync(user, "FB1mF@ln*");
            if (resul.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "ONG");
            }
        }
    }

    public async Task SeedRolesAsync()
    {
        if(!await _roleManager.RoleExistsAsync("Adopter"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Adopter"));
        }
        if (!await _roleManager.RoleExistsAsync("ONG"))
        {
            await _roleManager.CreateAsync(new IdentityRole("ONG"));
        }
    }
}

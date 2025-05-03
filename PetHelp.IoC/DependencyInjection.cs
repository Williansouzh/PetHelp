using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHelp.Domain.Account;
using PetHelp.Domain.Interfaces.Services;
using PetHelp.Infra.Data.Context;
using PetHelp.Infra.Data.Identity;
using PetHelp.Infra.Data.Persistence;
using PetHelp.Infra.Data.Services;

namespace PetHelp.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //DbContext with Postgress Config
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), b =>
                b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        Console.WriteLine(configuration.GetConnectionString("DefaultConnection"));
        //Identity Config
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        //Register repositories 
        services.AddScoped<IAuthenticate, AuthenticateService>();
        services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
        services.AddScoped<ITokenService, TokenService>();
        //Register Unit Of Work 
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //services.AddAutoMapper(typeof(DTOToCommandMappingProfile).Assembly);
        //var myHandles = AppDomain.CurrentDomain.Load("BarberFlow.Application");
        //services.AddMediatR(myHandles);
        return services;
    }
}

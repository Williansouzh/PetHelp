using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHelp.Application.Interfaces;
using PetHelp.Application.Services;
using PetHelp.Domain.Account;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;
using PetHelp.Infra.Data.Context;
using PetHelp.Infra.Data.Identity;
using PetHelp.Infra.Data.Migrations;
using PetHelp.Infra.Data.Persistence;
using PetHelp.Infra.Data.Repositories;
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
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ONG", policy => policy.RequireRole("ONG"));
            options.AddPolicy("Adopter", policy => policy.RequireRole("Adopter"));
        });

        //Register repositories 
        services.AddScoped<IAdoptionRepository, AdoptionRepository>();
        services.AddScoped<IAdoptionService, AdoptionService>();
        services.AddScoped<IAnimalRepository, AnimalRepository>();
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IAdoptionRepository, AdoptionRepository>();
        services.AddScoped<IAdoptionService, AdoptionService>();
        services.AddScoped<IAuthenticate, AuthenticateService>();
        services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
        services.AddScoped<ITokenService, TokenService>();
        //Register Unit Of Work 
        services.AddScoped<IMigrationService, MigrationService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGoogleCloudStorageService, GoogleCloudStorageService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IDialogFlowService, DialogflowService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        var myHandles = AppDomain.CurrentDomain.Load("PetHelp.Application");
        services.AddMediatR(myHandles);
        return services;
    }
}

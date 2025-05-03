using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace PetHelp.IoC;

public static class DependencyInjectionJWT
{
    public static IServiceCollection AddInfrastructureJWT(this IServiceCollection services,
          IConfiguration configuration)
    {
        //informar o tipo de autenticacao JWT-Bearer
        //definir o modelo de desafio de autenticacao
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        //habilita a autenticacao JWT usando o esquema e desafio definidos
        //validar o token
        .AddJwtBearer(options =>
        {
            var secretKey = configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("Jwt:SecretKey is not configured in the application settings.");
            }

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                //valores validos
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });
        return services;
    }
}

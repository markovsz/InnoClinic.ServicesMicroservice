using Application.Abstractions;
using Application.Services;
using Domain.Abstractions;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDb(this IServiceCollection services)
        {
            services.AddScoped<ISqlDataAccess, SqlDataAccess>();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IServicesRepository, ServicesRepository>();
            services.AddScoped<ISpecializationsRepository, SpecializationsRepository>();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<ISpecializationsService, SpecializationsService>();
        }

        public static void ConfigureValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly);
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var identityServerConfig = configuration
                        .GetSection("IdentityServer");

            var scopes = identityServerConfig
                        .GetSection("Scopes");

            services.AddAuthentication(config =>
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config =>
            {
                config.Authority = identityServerConfig
                    .GetSection("Address").Value;
                config.Audience = identityServerConfig
                    .GetSection("Audience").Value;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = identityServerConfig
                        .GetSection("Address").Value,
                    ValidateIssuer = true,
                    ValidAudience = scopes.GetSection("Basic").Value,

                    ValidateAudience = true
                };
            });
        }
    }
}

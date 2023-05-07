using Application.Abstractions;
using Application.Services;
using Domain.Abstractions;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using MassTransit;
using Microsoft.OpenApi.Models;

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

        public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(e =>
            {
                e.UsingRabbitMq((_, cfg) => { 
                    cfg.Host(new Uri(configuration.GetSection("RabbitMq:ConnectionString").Value ??
                                 throw new NotImplementedException()));
                });
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }
    }
}

using Application.Abstractions;
using Application.Services;
using Domain.Abstractions;
using Infrastructure;
using Infrastructure.Repositories;

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
    }
}

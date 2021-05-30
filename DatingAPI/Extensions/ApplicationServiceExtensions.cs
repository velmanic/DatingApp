using API.DatingAPI.Data;
using DatingAPI.Interfaces;
using DatingAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServies(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DataContext>(options => 
                        options.UseSqlite(config.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
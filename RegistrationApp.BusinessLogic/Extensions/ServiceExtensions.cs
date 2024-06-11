using Microsoft.Extensions.DependencyInjection;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.BusinessLogic.Services;

namespace RegistrationApp.BusinessLogic.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.BusinessLogic.Services;

namespace RegistrationApp.BusinessLogic.Extensions
{
    public static class ServiceExtensions
    {
        // Extension method to add business logic-related services to IServiceCollection
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            // Register Interfaces with their implementation services
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();

            // Return IServiceCollection to allow further chaining of method calls
            return services;
        }
    }
}

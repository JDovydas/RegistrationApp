using Microsoft.Extensions.DependencyInjection;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.BusinessLogic.Services;

namespace RegistrationApp.BusinessLogic.Extensions
{
    public static class ServiceExtensions
    {
        // Extension method for IServiceCollection to add business logic services
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            // Register IPersonService with its implementation PersonService
            services.AddScoped<IPersonService, PersonService>();
            
            // Register IUserService with its implementation UserService
            services.AddScoped<IUserService, UserService>();
            
            // Register IPlaceOfResidenceService with its implementation PlaceOfResidenceService
            services.AddScoped<IPlaceOfResidenceService, PlaceOfResidenceService>();
            
            // Register IJwtService with its implementation JwtService
            services.AddScoped<IJwtService, JwtService>();
            
            // Return IServiceCollection to allow further chaining of method calls
            return services;
        }
    }
}

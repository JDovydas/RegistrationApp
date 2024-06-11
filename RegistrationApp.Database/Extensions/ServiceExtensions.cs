using Microsoft.Extensions.DependencyInjection;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace RegistrationApp.Database.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPlaceOfResidenceRepository, PlaceOfResidenceRepository>();

            services.AddDbContext<RegistrationAppContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace RegistrationApp.Database.Extensions
{
    public static class ServiceExtensions
    {
        // Extension method to add database-related services to IServiceCollection
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, string connectionString)
        {
            // Register Interfaces to be resolved as Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPlaceOfResidenceRepository, PlaceOfResidenceRepository>();

            // Configure the DbContext to use a SQL Server database with the provided connection string
            services.AddDbContext<RegistrationAppContext>(options =>
                options.UseSqlServer(connectionString));

            // Return the modified IServiceCollection to support method chaining
            return services;
        }
    }
}

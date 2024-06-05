using Microsoft.EntityFrameworkCore;
using RegistrationApp.Shared.Models;

namespace RegistrationApp.Database
{
    public class RegistrationAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PlaceOfResidence> PlacesOfResidence { get; set; }

        public RegistrationAppContext(DbContextOptions<RegistrationAppContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.People)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.PlaceOfResidence)
                .WithOne(pr => pr.Person)
                .HasForeignKey<PlaceOfResidence>(pr => pr.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using DogsHouse.Domain.Entities;
using DogsHouse.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DogsHouse.Persistence.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration _configuration) : base(options)
        {
            configuration = _configuration;
        }

        public DbSet<Dog> Dogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DogConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
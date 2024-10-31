using DogsHouse.Domain.Entities;
using DogsHouse.Persistence.Data;
using DogsHouse.Persistence.Repositories;
using DogsHouse.Persistence.Repositories.DogRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DogsHouse.RepositoriesTests
{
    public class BaseRepositoryTests : IDisposable
    {
        private readonly DogRepository _dogRepository;
        private readonly AppDbContext _context;
        private readonly DbContextOptions<AppDbContext> _options;

        public BaseRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            _context = new AppDbContext(_options, new ConfigurationBuilder().Build());
            _dogRepository = new DogRepository(_context);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEntityToDatabase()
        {
            // Arrange
            using var context = new AppDbContext(_options, new ConfigurationBuilder().Build());
            var repository = new BaseRepository<Dog>(context);
            var dog = new Dog { Name = "By", Color = "Brown", TailLength = 10, Weight = 20 };

            // Act
            await repository.CreateAsync(dog);

            // Assert
            var savedDog = await context.Dogs.FindAsync("By");
            Assert.NotNull(savedDog);
            Assert.Equal("By", savedDog.Name);
        }

        [Fact]
        public async Task ReadAsync_ShouldReturnAllEntities()
        {
            // Arrange
            using (var context = new AppDbContext(_options, new ConfigurationBuilder().Build()))
            {
                context.Dogs.AddRange(new List<Dog>
                {
                    new Dog { Name = "Buddy", Color = "Brown", TailLength = 10, Weight = 20 },
                    new Dog { Name = "Max", Color = "Black", TailLength = 12, Weight = 22 }
                });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(_options, new ConfigurationBuilder().Build()))
            {
                var repository = new BaseRepository<Dog>(context);

                // Act
                var dogs = await repository.ReadAsync();

                // Assert
                Assert.NotNull(dogs);
                Assert.Equal(2, dogs.Count);
                Assert.Contains(dogs, d => d.Name == "Buddy" && d.TailLength == 10);
                Assert.Contains(dogs, d => d.Name == "Max" && d.TailLength == 12);
            }
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
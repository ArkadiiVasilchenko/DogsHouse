using DogsHouse.Domain.Entities;
using DogsHouse.Domain.Models.RequestDtos;
using DogsHouse.Persistence.Data;
using DogsHouse.Persistence.Repositories.DogRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DogsHouse.RepositoriesTests
{
    public class DogRepositoryTests : IDisposable
    {
        private readonly DogRepository _dogRepository;
        private readonly AppDbContext _context;
        private readonly DbContextOptions<AppDbContext> _options;

        public DogRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(_options, new ConfigurationBuilder().Build());
            _dogRepository = new DogRepository(_context);
        }

        private List<Dog> GetTestDogs()
        {
            return new List<Dog>
            {
                new Dog { Name = "Buddy", Color = "Brown", TailLength = 10, Weight = 20 },
                new Dog { Name = "Max", Color = "Black", TailLength = 12, Weight = 22 },
                new Dog { Name = "Bella", Color = "White", TailLength = 14, Weight = 18 },
                new Dog { Name = "Lucy", Color = "Golden", TailLength = 15, Weight = 25 }
            };
        }

        [Fact]
        public async Task ReadDogsAsync_ReturnsPaginatedDogs_WhenCalled()
        {
            // Arrange
            _context.Dogs.AddRange(GetTestDogs());
            _context.SaveChanges();

            var requestDto = new GetDogsRequestDto
            {
                PageNumber = 1,
                PageSize = 2,
                Attribute = "TailLength",
                Order = "asc"
            };

            // Act
            var result = await _dogRepository.ReadDogsAsync(requestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Buddy", result.First().Name);
        }

        [Fact]
        public async Task ReadDogsAsync_ReturnsEmptyList_WhenNoDogsMatch()
        {
            // Arrange
            var requestDto = new GetDogsRequestDto
            {
                PageNumber = 10,
                PageSize = 2,
                Attribute = "TailLength",
                Order = "asc"
            };

            // Act
            var result = await _dogRepository.ReadDogsAsync(requestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}

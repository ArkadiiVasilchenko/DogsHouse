using DogsHouse.Application.Services;
using DogsHouse.Persistence.Repositories;
using Moq;

namespace DogsHouse.ServicesTests
{
    public class BaseServiceTests
    {
        private readonly Mock<IBaseRepository<TestEntity>> _mockRepository;
        private readonly BaseService<TestEntity> _baseService;

        public BaseServiceTests()
        {
            _mockRepository = new Mock<IBaseRepository<TestEntity>>();
            _baseService = new BaseService<TestEntity>(_mockRepository.Object);
        }
        public class TestEntity { }

        [Fact]
        public async Task ReadAsync_ShouldReturnListOfEntities_WhenRepositoryReturnsEntities()
        {
            // Arrange
            var entities = new List<TestEntity> { new TestEntity(), new TestEntity() };
            _mockRepository.Setup(r => r.ReadAsync()).ReturnsAsync(entities);

            // Act
            var result = await _baseService.ReadAsync();

            // Assert
            Assert.Equal(entities, result);
            _mockRepository.Verify(r => r.ReadAsync(), Times.Once);
        }
    }
}

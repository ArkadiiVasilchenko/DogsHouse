using DogsHouse.Application.Services.DogServices;
using DogsHouse.Domain.Entities;
using DogsHouse.Domain.Models.RequestDtos;
using DogsHouse.Persistence.Repositories.DogRepository;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace DogsHouse.ServicesTests
{
    public class DogServiceTests
    {
        private readonly Mock<IDogRepository> _mockDogRepository;
        private readonly Mock<IValidator<GetDogsRequestDto>> _mockValidator;
        private readonly DogService _dogService;

        public DogServiceTests()
        {
            _mockDogRepository = new Mock<IDogRepository>();
            _mockValidator = new Mock<IValidator<GetDogsRequestDto>>();
            _dogService = new DogService(_mockDogRepository.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task СreateAsync_ShouldCreateDog()
        {
            // Arrange
            var requestDto = new CreateDogRequestDto
            {
                Name = "Buddy",
                Color = "Brown",
                TailLength = 15,
                Weight = 25
            };

            // Act
            await _dogService.СreateAsync(requestDto);

            // Assert
            _mockDogRepository.Verify(repo => repo.CreateAsync(It.Is<Dog>(d =>
                d.Name == requestDto.Name &&
                d.Color == requestDto.Color &&
                d.TailLength == requestDto.TailLength &&
                d.Weight == requestDto.Weight
            )), Times.Once);
        }

        [Fact]
        public async Task ReadDogsAsync_ShouldReturnDogs_WhenValidationSucceeds()
        {
            // Arrange
            var requestDto = new GetDogsRequestDto();
            var dogsList = new List<Dog> { new Dog("Buddy", "Brown", 15, 25) };

            _mockValidator.Setup(v => v.ValidateAsync(requestDto, default))
                .ReturnsAsync(new ValidationResult());
            _mockDogRepository.Setup(repo => repo.ReadDogsAsync(requestDto))
                .ReturnsAsync(dogsList);

            // Act
            var result = await _dogService.ReadDogsAsync(requestDto);

            // Assert
            Assert.Equal(dogsList, result);
            _mockDogRepository.Verify(repo => repo.ReadDogsAsync(requestDto), Times.Once);
        }

        [Fact]
        public async Task ReadDogsAsync_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var requestDto = new GetDogsRequestDto();
            var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Attribute", "Attribute is required")
        };
            var validationResult = new ValidationResult(validationFailures);

            _mockValidator.Setup(v => v.ValidateAsync(requestDto, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _dogService.ReadDogsAsync(requestDto));
        }
    }
}
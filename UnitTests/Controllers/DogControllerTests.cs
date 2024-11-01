using Microsoft.Extensions.Logging;
using DogsHouse.API.Controllers;
using DogsHouse.Application.Services.DogServices.DogServicesInterfaces;
using DogsHouse.Domain.Entities;
using DogsHouse.Domain.Models.RequestDtos;
using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace DogsHouse.ControllersTests
{
    public class DogControllerTests
    {
        private readonly DogController _controller;
        private readonly IDogService _dogService;
        private readonly IValidator<CreateDogRequestDto> _createDogRequestValidator;
        private readonly ILogger<DogController> _logger;

        public DogControllerTests()
        {
            _dogService = A.Fake<IDogService>();
            _createDogRequestValidator = A.Fake<IValidator<CreateDogRequestDto>>();
            _logger = A.Fake<ILogger<DogController>>();
            _controller = new DogController(_dogService, _createDogRequestValidator, _logger);
        }

        [Fact]
        public async Task CreateDog_ShouldReturnBadRequest_WhenRequestDtoIsNull()
        {
            // Act
            var result = await _controller.CreateDog(null);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task CreateDog_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var requestDto = new CreateDogRequestDto { Name = null, Color = null, TailLength = 0, Weight = 0 };
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Name", "Name is required") };
            var validationResult = new ValidationResult(validationFailures);

            A.CallTo(() => _createDogRequestValidator.ValidateAsync(requestDto, default))
                .Returns(Task.FromResult(validationResult));

            // Act
            Func<Task> act = async () => await _controller.CreateDog(requestDto);

            //Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Validation failed");
        }

        [Fact]
        public async Task CreateDog_ShouldReturnOk_WhenValidationSucceeds()
        {
            // Arrange
            var requestDto = new CreateDogRequestDto
            {
                Name = "Buddy",
                Color = "Brown",
                TailLength = 20,
                Weight = 30
            };
            var validationResult = new ValidationResult();

            A.CallTo(() => _createDogRequestValidator.ValidateAsync(requestDto, default))
                .Returns(Task.FromResult(validationResult));

            // Act
            var result = await _controller.CreateDog(requestDto);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task GetDogs_ShouldReturnBadRequest_WhenRequestDtoIsNull()
        {
            // Act
            var result = await _controller.GetDogs(null);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task GetDogs_ShouldReturnOk_WhenRequestDtoIsEmpty()
        {
            // Arrange
            var requestDto = new GetDogsRequestDto();
            var dogs = new List<Dog>
            {
                new Dog("Buddy", "Brown", 20, 30),
                new Dog("Max", "Black", 25, 35)
            };

            A.CallTo(() => _dogService.ReadAsync()).Returns(dogs);

            // Act
            var result = await _controller.GetDogs(requestDto) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(dogs);
        }

        [Fact]
        public async Task GetDogs_ShouldReturnOk_WhenDogsExist()
        {
            // Arrange
            var requestDto = new GetDogsRequestDto
            {
                Attribute = "SomeAttribute",
                Order = "asc",
                PageNumber = 1,
                PageSize = 10
            };
            var dogs = new List<Dog>
            {
                new Dog("Buddy", "Brown", 20, 30),
                new Dog("Max", "Black", 25, 35)
            };

            A.CallTo(() => _dogService.ReadDogsAsync(requestDto)).Returns(dogs);

            // Act
            var result = await _controller.GetDogs(requestDto) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(dogs);
        }

        [Fact]
        public void Ping_ShouldReturnOk()
        {
            // Act
            var result = _controller.Ping() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be("Dogshouseservice.Version1.0.1");
        }
    }
}

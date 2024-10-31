using Microsoft.AspNetCore.Mvc;
using DogsHouse.Application.Services.DogServices.DogServicesInterfaces;
using DogsHouse.Domain.Models.RequestDtos;
using FluentValidation;

namespace DogsHouse.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly IDogService _dogService;
        private readonly ILogger<DogController> _logger;
        private readonly IValidator<CreateDogRequestDto> _createDogRequestValidator;

        public DogController(IDogService dogService, IValidator<CreateDogRequestDto> createDogRequestValidator, ILogger<DogController> logger)
        {
            _dogService = dogService;
            _createDogRequestValidator = createDogRequestValidator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDog([FromBody] CreateDogRequestDto requestDto)
        {
            if (requestDto == null)
            {
                return BadRequest();
            }

            var validationResult = await _createDogRequestValidator.ValidateAsync(requestDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("Validation failed", validationResult.Errors);
            }

            try
            {
                await _dogService.СreateAsync(requestDto);
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the dog: {DogName}.", requestDto.Name);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDogs([FromQuery] GetDogsRequestDto requestDto)
        {
            if (requestDto == null)
            {
                return BadRequest();
            }

            if (requestDto.IsEmpty)
            {
                return Ok(await _dogService.ReadAsync());
            }

            var dogs = await _dogService.ReadDogsAsync(requestDto);

            return Ok(dogs);
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Dogshouseservice.Version1.0.1");
        }
    }
}

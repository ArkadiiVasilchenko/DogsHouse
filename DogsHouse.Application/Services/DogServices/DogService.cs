using DogsHouse.Application.Services.DogServices.DogServicesInterfaces;
using DogsHouse.Domain.Entities;
using DogsHouse.Domain.Models.RequestDtos;
using DogsHouse.Persistence.Repositories.DogRepository;
using FluentValidation;

namespace DogsHouse.Application.Services.DogServices
{
    public class DogService : BaseService<Dog>, IDogService
    {
        private readonly IDogRepository dogRepository;
        private readonly IValidator<GetDogsRequestDto> getDogsRequestValidator;

        public DogService(IDogRepository _dogRepository,
            IValidator<GetDogsRequestDto> _getDogsRequestValidator) : base(_dogRepository)
        {
            dogRepository = _dogRepository;
            getDogsRequestValidator = _getDogsRequestValidator;
        }

        public async Task СreateAsync(CreateDogRequestDto requestDto)
        {
            Dog dog = new Dog(requestDto.Name, requestDto.Color, requestDto.TailLength, requestDto.Weight);

            await dogRepository.CreateAsync(dog);
        }

        public async Task<List<Dog>> ReadDogsAsync(GetDogsRequestDto requestDto)
        {
            var validationResult = await getDogsRequestValidator.ValidateAsync(requestDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("Validation failed", validationResult.Errors);
            }

            var dogs = await dogRepository.ReadDogsAsync(requestDto);

            return dogs;
        }
    }
}
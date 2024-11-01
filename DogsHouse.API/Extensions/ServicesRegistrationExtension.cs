using DogsHouse.Application.Services.DogServices.DogServicesInterfaces;
using DogsHouse.Application.Services.DogServices;
using DogsHouse.Application.Services;
using DogsHouse.Domain.Entities;
using DogsHouse.Domain.Models.RequestDtos;
using DogsHouse.Persistence.Repositories.DogRepository;
using DogsHouse.Persistence.Repositories;
using FluentValidation;
using DogsHouse.Application.Validations;

namespace DogsHouse.API.Extensions
{
    public static class ServicesRegistrationExtension
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IDogService, DogService>();
            services.AddScoped<IBaseService<Dog>, BaseService<Dog>>();

            services.AddScoped<IDogRepository, DogRepository>();
            services.AddScoped<IBaseRepository<Dog>, BaseRepository<Dog>>();

            services.AddScoped<IValidator<GetDogsRequestDto>, DogQueryValidator>();
            services.AddScoped<IValidator<CreateDogRequestDto>, CreateDogValidator>();
        }
    }
}

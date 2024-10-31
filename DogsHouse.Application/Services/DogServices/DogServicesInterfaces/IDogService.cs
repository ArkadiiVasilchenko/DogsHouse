using DogsHouse.Domain.Entities;
using DogsHouse.Domain.Models.RequestDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Application.Services.DogServices.DogServicesInterfaces
{
    public interface IDogService : IBaseService<Dog>
    {
        Task СreateAsync(CreateDogRequestDto requestDto);
        Task<List<Dog>> ReadDogsAsync(GetDogsRequestDto requestDto);
    }
}

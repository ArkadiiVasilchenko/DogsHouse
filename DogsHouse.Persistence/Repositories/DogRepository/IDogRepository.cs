using DogsHouse.Domain.Entities;
using DogsHouse.Domain.Models.RequestDtos;

namespace DogsHouse.Persistence.Repositories.DogRepository
{
    public interface IDogRepository : IBaseRepository<Dog>
    {
        Task<List<Dog>> ReadDogsAsync(GetDogsRequestDto requestDto);
    }
}

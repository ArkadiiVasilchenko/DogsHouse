using DogsHouse.Persistence.Repositories;

namespace DogsHouse.Application.Services
{
    public interface IBaseService<TModel> where TModel : class
    {
        Task<List<TModel>> ReadAsync();
    }
    public class BaseService<TModel> : IBaseService<TModel> where TModel : class
    {
        private readonly IBaseRepository<TModel> repository;
        public BaseService(IBaseRepository<TModel> _repository)
        {
            repository = _repository;
        }

        public async Task<List<TModel>> ReadAsync()
        {
            var dogs = await repository.ReadAsync();

            return dogs ?? new List<TModel>();
        }
    }
}

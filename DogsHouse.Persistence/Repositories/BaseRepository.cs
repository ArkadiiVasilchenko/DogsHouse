using DogsHouse.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace DogsHouse.Persistence.Repositories
{
    public interface IBaseRepository<TModel> where TModel : class
    {
        Task CreateAsync(TModel entity);
        Task<List<TModel>> ReadAsync();
    }

    public class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : class
    {
        private readonly AppDbContext dbContext;
        public BaseRepository(AppDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public async Task CreateAsync(TModel entity)
        {
            await dbContext.Set<TModel>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<TModel>> ReadAsync()
        {
            return await dbContext.Set<TModel>().AsNoTracking().ToListAsync();
        }
    }
}
using DogsHouse.Domain.Entities;
using DogsHouse.Domain.Models.RequestDtos;
using DogsHouse.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DogsHouse.Persistence.Repositories.DogRepository
{
    public class DogRepository : BaseRepository<Dog>, IDogRepository
    {
        private readonly AppDbContext dbContext;
        public DogRepository(AppDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<List<Dog>> ReadDogsAsync(GetDogsRequestDto requestDto)
        {
            int skip = (requestDto.PageNumber - 1) * requestDto.PageSize;
            var dogsQuery = ApplyOrdering(dbContext.Dogs.AsNoTracking(), requestDto.Attribute, requestDto.Order);

            return await dogsQuery
                .Skip(skip)
                .Take(requestDto.PageSize)
                .ToListAsync();
        }

        private IQueryable<Dog> ApplyOrdering(IQueryable<Dog> source, string attribute, string order)
        {
            var parameter = Expression.Parameter(typeof(Dog), "dog");
            var property = Expression.Property(parameter, attribute);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase)
                       ? nameof(Queryable.OrderByDescending)
                       : nameof(Queryable.OrderBy);

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(Dog), property.Type },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<Dog>(resultExpression);
        }
    }
}
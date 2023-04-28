using System.Linq.Expressions;
using SogetiTODO.Domain.Filters;
using SogetiTODO.Domain.POCOs;

namespace SogetiTODO.Repositories.Abstractions;

public interface ITodoRepository
{
    Task<List<Todo>> GetAllAsync(PaginationFilter? pagination,
        Expression<Func<Todo, bool>> predicate = null);

    Task<Todo?> GetAsync(Expression<Func<Todo, bool>> predicate = null);
    Task<Todo> AddAsync(Todo todo);
    Task<Guid> UpdateStateAsync(Todo obj, bool isCompleted);
    Task DeleteAsync(Todo obj);
}
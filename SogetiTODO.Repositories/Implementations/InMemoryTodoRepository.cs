using System.Linq.Expressions;
using SogetiTODO.Domain.Filters;
using SogetiTODO.Domain.POCOs;
using SogetiTODO.Repositories.Abstractions;

namespace SogetiTODO.Repositories.Implementations;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<Todo> _todos;

    public InMemoryTodoRepository(IDataSeeder dataSeeder)
    {
        _todos = dataSeeder.SeedTodos().Result;
    }

    public async Task<List<Todo>> GetAllAsync(PaginationFilter pagination = null,
        Expression<Func<Todo, bool>> predicate = null)
    {
        var query = _todos.AsQueryable()
            .OrderBy(x => x.FinishUntil)
            .Where(x => !x.IsDeleted);

        if (predicate != null) query = query.Where(predicate);

        if (pagination != null)
            query = query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize);

        return await Task.FromResult(query.ToList());
    }

    public async Task<Todo?> GetAsync(Expression<Func<Todo, bool>> predicate = null)
    {
        var query = _todos.AsQueryable().Where(x => !x.IsDeleted);

        if (predicate != null) query = query.Where(predicate);

        return await Task.FromResult(query.SingleOrDefault());
    }

    public async Task<Todo> AddAsync(Todo todo)
    {
        todo.Id = Guid.NewGuid();
        _todos.Add(todo);
        return await Task.FromResult(todo);
    }

    public async Task<Guid> UpdateStateAsync(Todo obj, bool isCompleted)
    {
        obj.IsCompleted = isCompleted;
        return await Task.FromResult(obj.Id);
    }

    public async Task DeleteAsync(Todo obj)
    {
        obj.IsDeleted = true;
        await Task.CompletedTask;
    }
}
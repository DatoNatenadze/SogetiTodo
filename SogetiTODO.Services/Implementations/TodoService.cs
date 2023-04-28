using Mapster;
using SogetiTODO.Domain.Filters;
using SogetiTODO.Domain.POCOs;
using SogetiTODO.Repositories.Abstractions;
using SogetiTODO.Services.Abstractions;
using SogetiTODO.Services.Exceptions;
using SogetiTODO.Services.Localisations;
using SogetiTODO.Services.Models.ServiceModels;

namespace SogetiTODO.Services.Implementations;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IEnumerable<TodoServiceModel>> GetAllAsync(PaginationFilter paginationFilter)
    {
        var todos = await _todoRepository.GetAllAsync(paginationFilter);
        return todos.Adapt<List<TodoServiceModel>>();
    }

    public async Task<Guid> UpdateStateAsync(Guid id, bool isCompleted)
    {
        var todo = await _todoRepository.GetAsync(x => x.Id == id);
        if (todo == null)
            throw new TodoNotFoundException(ExceptionMessages.TodoNotFound);

        if (todo.IsCompleted == isCompleted)
            throw new TodoAlreadyInStateException(ExceptionMessages.TodoAlreadyInState);

        return await _todoRepository.UpdateStateAsync(todo, isCompleted);
    }

    public async Task<TodoServiceModel> GetDetailsAsync(Guid id)
    {
        var todo = await _todoRepository.GetAsync(x => x.Id == id);
        if (todo == null)
            throw new TodoNotFoundException(ExceptionMessages.TodoNotFound);
        return todo.Adapt<TodoServiceModel>();
    }

    public async Task<TodoServiceModel> AddAsync(TodoServiceModel todo)
    {
        var obj = await _todoRepository.AddAsync(todo.Adapt<Todo>());
        return obj.Adapt<TodoServiceModel>();
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var todo = await _todoRepository.GetAsync(x => x.Id == id);
        if (todo == null)
            throw new TodoNotFoundException(ExceptionMessages.TodoNotFound);
        await _todoRepository.DeleteAsync(todo);
    }
}
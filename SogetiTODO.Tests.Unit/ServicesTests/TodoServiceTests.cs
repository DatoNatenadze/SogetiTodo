using System.Linq.Expressions;
using NSubstitute;
using SogetiTODO.Domain.Filters;
using SogetiTODO.Domain.POCOs;
using SogetiTODO.Repositories.Abstractions;
using SogetiTODO.Services.Abstractions;
using SogetiTODO.Services.Exceptions;
using SogetiTODO.Services.Implementations;
using SogetiTODO.Services.Models.ServiceModels;

namespace SogetiTODO.Tests.Unit.ServicesTests;

public class TodoServiceTests
{
    private readonly ITodoRepository _todoRepository;
    private readonly ITodoService _todoService;

    public TodoServiceTests()
    {
        _todoRepository = Substitute.For<ITodoRepository>();
        _todoService = new TodoService(_todoRepository);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsCorrectList()
    {
        // Arrange
        var paginationFilter = new PaginationFilter();
        var todos = new List<Todo>
        {
            new() { Id = Guid.NewGuid(), Description = "Todo 1", IsCompleted = false },
            new() { Id = Guid.NewGuid(), Description = "Todo 2", IsCompleted = true }
        };
        _todoRepository.GetAllAsync(paginationFilter).Returns(todos);

        // Act
        var result = await _todoService.GetAllAsync(paginationFilter);

        // Assert
        await _todoRepository.Received(1).GetAllAsync(paginationFilter);
        Assert.NotNull(result);
        Assert.Equal(todos.Count, result.Count());
    }

    [Fact]
    public async Task UpdateStateAsync_ThrowsTodoNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<TodoNotFoundException>(async () => await _todoService.UpdateStateAsync(id, true));
    }

    [Fact]
    public async Task UpdateStateAsync_ThrowsTodoAlreadyInStateException_WhenTodoStateIsSameAsNewState()
    {
        // Arrange
        var id = Guid.NewGuid();
        var todo = new Todo { Id = id, Description = "Todo 1", IsCompleted = true };
        _todoRepository.GetAsync(Arg.Is<Expression<Func<Todo, bool>>>(x => x.Compile().Invoke(todo))).Returns(todo);

        // Act & Assert
        await Assert.ThrowsAsync<TodoAlreadyInStateException>(async () =>
            await _todoService.UpdateStateAsync(id, true));
    }

    [Fact]
    public async Task UpdateStateAsync_CallsUpdateStateAsync_WhenTodoStateIsNotSameAsNewState()
    {
        // Arrange
        var id = Guid.NewGuid();
        var todo = new Todo { Id = id, Description = "Todo 1", IsCompleted = false };
        _todoRepository.GetAsync(Arg.Is<Expression<Func<Todo, bool>>>(x => x.Compile().Invoke(todo))).Returns(todo);
        _todoRepository.UpdateStateAsync(todo, true).Returns(id);

        // Act
        var result = await _todoService.UpdateStateAsync(id, true);

        // Assert
        await _todoRepository.Received(1).GetAsync(Arg.Is<Expression<Func<Todo, bool>>>(x => x.Compile().Invoke(todo)));
        await _todoRepository.Received(1).UpdateStateAsync(todo, true);
        Assert.Equal(id, result);
    }

    [Fact]
    public async Task GetDetailsAsync_ReturnsCorrectTodoServiceModel_WhenTodoIsFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var todo = new Todo { Id = id, Description = "Todo 1", IsCompleted = false };
        _todoRepository.GetAsync(Arg.Is<Expression<Func<Todo, bool>>>(x => x.Compile().Invoke(todo))).Returns(todo);

        // Act
        var result = await _todoService.GetDetailsAsync(id);

        // Assert
        await _todoRepository.Received(1).GetAsync(Arg.Is<Expression<Func<Todo, bool>>>(x => x.Compile().Invoke(todo)));
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(todo.Description, result.Description);
        Assert.Equal(todo.IsCompleted, result.IsCompleted);
    }

    [Fact]
    public async Task GetDetailsAsync_ThrowsTodoNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<TodoNotFoundException>(async () => await _todoService.GetDetailsAsync(id));
    }

    [Fact]
    public async Task AddAsync_ReturnsCorrectlyMappedObject()
    {
        // Arrange
        var todoServiceModel = new TodoServiceModel { Description = "New Todo" };
        _todoRepository.AddAsync(Arg.Any<Todo>()).Returns(x => new Todo
        {
            Id = Guid.NewGuid(),
            Description = x.Arg<Todo>().Description,
            IsCompleted = x.Arg<Todo>().IsCompleted,
            FinishUntil = DateTime.UtcNow.AddDays(1)
        });

        // Act
        var result = await _todoService.AddAsync(todoServiceModel);

        // Assert
        await _todoRepository.Received(1).AddAsync(Arg.Any<Todo>());
        Assert.Equal(todoServiceModel.Description, result.Description);
    }


    [Fact]
    public async Task DeleteByIdAsync_CallsDeleteAsync_WhenTodoIsFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var todo = new Todo { Id = id, Description = "Todo 1", IsCompleted = false };
        _todoRepository.GetAsync(Arg.Is<Expression<Func<Todo, bool>>>(x => x.Compile().Invoke(todo))).Returns(todo);

        // Act
        await _todoService.DeleteByIdAsync(id);

        // Assert
        await _todoRepository.Received(1).GetAsync(Arg.Is<Expression<Func<Todo, bool>>>(x => x.Compile().Invoke(todo)));
        await _todoRepository.Received(1).DeleteAsync(todo);
    }

    [Fact]
    public async Task DeleteByIdAsync_ThrowsTodoNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<TodoNotFoundException>(async () => await _todoService.DeleteByIdAsync(id));
    }
}
using System.Linq.Expressions;
using NSubstitute;
using SogetiTODO.Domain.Filters;
using SogetiTODO.Domain.POCOs;
using SogetiTODO.Repositories.Abstractions;
using SogetiTODO.Repositories.Implementations;

namespace SogetiTODO.Tests.Unit.RepositoriesTests;

public class InMemoryTodoRepositoryTests
{
    private readonly IDataSeeder _dataSeeder;
    private readonly ITodoRepository _todoRepository;

    public InMemoryTodoRepositoryTests()
    {
        _dataSeeder = Substitute.For<IDataSeeder>();
        _dataSeeder.SeedTodos().Returns(new List<Todo>());
        _todoRepository = new InMemoryTodoRepository(_dataSeeder);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsTodosList()
    {
        // Arrange
        var todo = new Todo
            { Id = Guid.NewGuid(), Description = "Test Todo", FinishUntil = DateTime.Now, IsDeleted = false };
        await _todoRepository.AddAsync(todo);

        // Act
        var todos = await _todoRepository.GetAllAsync(null);

        // Assert
        Assert.NotNull(todos);
        Assert.Single(todos);
        Assert.Equal(todo.Id, todos.First().Id);
        Assert.Equal(todo.Description, todos.First().Description);
    }

    [Fact]
    public async Task GetAllAsync_AppliesPagination()
    {
        // Arrange
        var todo1 = new Todo { Description = "Test Todo 1", FinishUntil = DateTime.Now, IsDeleted = false };
        var todo2 = new Todo { Description = "Test Todo 2", FinishUntil = DateTime.Now.AddHours(1), IsDeleted = false };
        await _todoRepository.AddAsync(todo1);
        await _todoRepository.AddAsync(todo2);

        var pagination = new PaginationFilter { PageNumber = 1, PageSize = 1 };

        // Act
        var todos = await _todoRepository.GetAllAsync(pagination);

        // Assert
        Assert.NotNull(todos);
        Assert.Single(todos);
        Assert.Equal(todo1.Id, todos.First().Id);
        Assert.Equal(todo1.Description, todos.First().Description);
    }

    [Fact]
    public async Task GetAllAsync_AppliesPredicate()
    {
        // Arrange
        var todo1 = new Todo
            { Description = "Test Todo 1", FinishUntil = DateTime.Now, IsDeleted = false, IsCompleted = false };
        var todo2 = new Todo
        {
            Description = "Test Todo 2", FinishUntil = DateTime.Now.AddHours(1), IsDeleted = false, IsCompleted = true
        };
        await _todoRepository.AddAsync(todo1);
        await _todoRepository.AddAsync(todo2);

        Expression<Func<Todo, bool>> predicate = x => x.IsCompleted;

        // Act
        var todos = await _todoRepository.GetAllAsync(null, predicate);

        // Assert
        Assert.NotNull(todos);
        Assert.Single(todos);
        Assert.Equal(todo2.Id, todos.First().Id);
        Assert.Equal(todo2.Description, todos.First().Description);
    }

    [Fact]
    public async Task GetAsync_ReturnsTodoWithGivenPredicate()
    {
        // Arrange
        var todo = new Todo
            { Id = Guid.NewGuid(), Description = "Test Todo", FinishUntil = DateTime.Now, IsDeleted = false };
        await _todoRepository.AddAsync(todo);

        // Act
        var fetchedTodo = await _todoRepository.GetAsync(x => x.Id == todo.Id);

        // Assert
        Assert.NotNull(fetchedTodo);
        Assert.Equal(todo.Id, fetchedTodo.Id);
        Assert.Equal(todo.Description, fetchedTodo.Description);
    }

    [Fact]
    public async Task AddAsync_AddsTodoToList()
    {
        // Arrange
        var todo = new Todo { Description = "Test Todo", FinishUntil = DateTime.Now, IsDeleted = false };

        // Act
        var addedTodo = await _todoRepository.AddAsync(todo);

        // Assert
        Assert.NotNull(addedTodo);
        Assert.NotEqual(Guid.Empty, addedTodo.Id);
        Assert.Equal(todo.Description, addedTodo.Description);
    }

    [Fact]
    public async Task UpdateStateAsync_UpdatesTodoState()
    {
        // Arrange
        var todo = new Todo
            { Description = "Test Todo", FinishUntil = DateTime.Now, IsDeleted = false, IsCompleted = false };
        var addedTodo = await _todoRepository.AddAsync(todo);

        // Act
        await _todoRepository.UpdateStateAsync(addedTodo, true);

        // Assert
        Assert.True(addedTodo.IsCompleted);
    }

    [Fact]
    public async Task DeleteAsync_SetsIsDeletedToTrue()
    {
        // Arrange
        var todo = new Todo { Description = "Test Todo", FinishUntil = DateTime.Now, IsDeleted = false };
        var addedTodo = await _todoRepository.AddAsync(todo);

        // Act
        await _todoRepository.DeleteAsync(addedTodo);

        // Assert
        Assert.True(addedTodo.IsDeleted);
    }
}
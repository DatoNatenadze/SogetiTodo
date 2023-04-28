using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using SogetiTODO.Controllers;
using SogetiTODO.Models;
using SogetiTODO.Services.Abstractions;
using SogetiTODO.Services.Models.ServiceModels;

namespace SogetiTODO.Tests.Unit.ControllersTests;

public class HomeControllerTests
{
    private readonly HomeController _homeController;
    private readonly ITodoService _todoService;

    public HomeControllerTests()
    {
        _todoService = Substitute.For<ITodoService>();
        _homeController = new HomeController(_todoService);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfTodoViewModels()
    {
        // Arrange
        var todos = new List<TodoServiceModel>
        {
            new() { Description = "Test Todo 1" },
            new() { Description = "Test Todo 2" }
        };

        _todoService.GetAllAsync().Returns(todos);

        // Act
        var result = await _homeController.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var returnedTodos = result.Model as List<TodoViewModel>;
        Assert.NotNull(returnedTodos);
        Assert.Equal(todos.Count, returnedTodos.Count);
        Assert.Equal(todos[0].Description, returnedTodos[0].Description);
        Assert.Equal(todos[1].Description, returnedTodos[1].Description);
    }
}
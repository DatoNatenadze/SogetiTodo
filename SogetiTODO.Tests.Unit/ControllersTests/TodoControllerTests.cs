using Mapster;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using SogetiTODO.Controllers;
using SogetiTODO.Models;
using SogetiTODO.Models.RequestModels;
using SogetiTODO.Services.Abstractions;
using SogetiTODO.Services.Models.ServiceModels;

namespace SogetiTODO.Tests.Unit.ControllersTests;

public class TodoControllerTests
{
    private readonly TodoController _todoController;
    private readonly ITodoService _todoService;

    public TodoControllerTests()
    {
        _todoService = Substitute.For<ITodoService>();
        _todoController = new TodoController(_todoService);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithCorrectTodoViewModel()
    {
        // Arrange
        var id = Guid.NewGuid();
        var todoServiceModel = new TodoServiceModel { Id = id, Description = "Test Todo" };
        _todoService.GetDetailsAsync(id).Returns(todoServiceModel);

        // Act
        var result = await _todoController.Index(id.ToString()) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var returnedTodo = result.Model as TodoViewModel;
        Assert.NotNull(returnedTodo);
        Assert.Equal(todoServiceModel.Id, returnedTodo.Id);
        Assert.Equal(todoServiceModel.Description, returnedTodo.Description);
    }

    [Fact]
    public async Task Create_ReturnsPartialView_WithCorrectTodoViewModel()
    {
        // Arrange
        var createTodoRequest = new CreateTodoRequestModel { Description = "Test Todo" };
        var todoServiceModel = createTodoRequest.Adapt<TodoServiceModel>();
        _todoService.AddAsync(Arg.Any<TodoServiceModel>()).Returns(todoServiceModel);
        // Act
        var result = await _todoController.Create(createTodoRequest) as PartialViewResult;

        // Assert
        Assert.NotNull(result);
        var returnedTodo = result.Model as TodoViewModel;
        Assert.NotNull(returnedTodo);
        Assert.Equal(createTodoRequest.Description, returnedTodo.Description);
    }

    [Fact]
    public async Task Update_ReturnsNoContentResultResult_WhenUpdateIsSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateTodoRequest = new UpdateTodoRequestModel { Id = id.ToString(), IsCompleted = true };
        _todoService.UpdateStateAsync(id, updateTodoRequest.IsCompleted).Returns(Task.FromResult(id));

        // Act
        var result = await _todoController.Update(updateTodoRequest) as NoContentResult;

        // Assert
        Assert.IsType<NoContentResult>(result);
        await _todoService.Received(1).UpdateStateAsync(id, updateTodoRequest.IsCompleted);
    }
    
    [Fact]
    public async Task GivenValidId_Delete_ReturnsNoContentResult_WhenDeleteIsSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        _todoService.DeleteByIdAsync(Guid.Parse(id)).Returns(Task.CompletedTask); // set up the service method to return a completed task

        // Act
        var result = await _todoController.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await _todoService.Received(1).DeleteByIdAsync(Guid.Parse(id));
    }
}
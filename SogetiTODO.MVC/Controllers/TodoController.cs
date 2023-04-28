using Mapster;
using Microsoft.AspNetCore.Mvc;
using SogetiTODO.Contracts;
using SogetiTODO.Models;
using SogetiTODO.Models.RequestModels;
using SogetiTODO.Services.Abstractions;
using SogetiTODO.Services.Models.ServiceModels;

namespace SogetiTODO.Controllers;

/// <summary>
///     Provides API endpoints for managing TODO items.
/// </summary>
public class TodoController : Controller
{
    private readonly ITodoService _todoService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TodoController" /> class.
    /// </summary>
    /// <param name="todoService">The service to manage TODO items.</param>
    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    /// <summary>
    ///     Retrieves details of the specified TODO item.
    /// </summary>
    /// <param name="todoItemId"> The unique identifier of the TODO item></param>
    /// <returns>A view with the TODO item details.</returns>
    [HttpGet(Routes.Todo.Index)]
    public async Task<IActionResult> Index(string todoItemId)
    {
        var todo = await _todoService.GetDetailsAsync(Guid.Parse(todoItemId));
        return View(todo.Adapt<TodoViewModel>());
    }

    /// <summary>
    ///     Creates a new TODO item.
    /// </summary>
    /// <param name="newTodo">The data for the new TODO item.</param>
    /// <returns>A partial view with the new TODO item.</returns>
    [HttpPost(Routes.Todo.Add)]
    public async Task<IActionResult> Create([FromBody] CreateTodoRequestModel newTodo)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var obj = await _todoService.AddAsync(newTodo.Adapt<TodoServiceModel>());
        var partView = PartialView("_TodoItem", obj.Adapt<TodoViewModel>());
        return partView;
    }

    /// <summary>
    ///     Updates the state of a specified TODO item.
    /// </summary>
    /// <param name="updateTodo">The data for updating the TODO item's state.</param>
    /// <returns>An IActionResult indicating the success of the operation.</returns>
    [HttpPut(Routes.Todo.Update)]
    public async Task<IActionResult> Update([FromBody] UpdateTodoRequestModel updateTodo)
    {
        await _todoService.UpdateStateAsync(Guid.Parse(updateTodo.Id), updateTodo.IsCompleted);
        return NoContent();
    }

    /// <summary>
    ///     Deletes a specified TODO item.
    /// </summary>
    /// <param name="id">The unique identifier of the TODO item to delete.</param>
    /// <returns>An IActionResult indicating the success of the operation.</returns>
    [HttpDelete(Routes.Todo.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        await _todoService.DeleteByIdAsync(Guid.Parse(id));
        return NoContent();
    }
}
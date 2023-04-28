using Mapster;
using Microsoft.AspNetCore.Mvc;
using SogetiTODO.Models;
using SogetiTODO.Services.Abstractions;

namespace SogetiTODO.Controllers;

[Controller]
public class HomeController : Controller
{
    private readonly ITodoService _todoService;

    public HomeController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var todos = await _todoService.GetAllAsync();
        return View(todos.Adapt<List<TodoViewModel>>());
    }
}
using Microsoft.AspNetCore.Mvc;

namespace SogetiTODO.Controllers;

public class ErrorController : Controller
{
    public async Task<IActionResult> Index(string errorCode)
    {
        ViewData["errorCode"] = errorCode;
        return View();
    }
}
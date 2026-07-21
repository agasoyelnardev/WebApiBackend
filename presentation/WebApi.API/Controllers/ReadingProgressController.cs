using Microsoft.AspNetCore.Mvc;

namespace WebApi.API.Controllers;

public class ReadingProgressController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
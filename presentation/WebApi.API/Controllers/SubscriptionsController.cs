using Microsoft.AspNetCore.Mvc;

namespace WebApi.API.Controllers;

public class SubscriptionsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
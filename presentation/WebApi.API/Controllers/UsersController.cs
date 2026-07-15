using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.Entities;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public UsersController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userManager.Users
            .Select(x => new
            {
                x.Id,
                x.UserName,
                x.Avatar
            })
            .ToList();

        return Ok(users);
    }
}
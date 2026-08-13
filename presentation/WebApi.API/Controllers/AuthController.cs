using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebApi.Application.Features.Auth.Commands;
using WebApi.Application.Features.Auth.Commands.ChangePassword;
using WebApi.Application.Features.Auth.Commands.ExternalLogin;
using WebApi.Application.Features.Auth.Commands.Login;
using WebApi.Application.Features.Auth.Commands.Register;
using WebApi.Application.Features.Auth.Commands.RefreshToken;
using WebApi.Application.Features.Auth.Commands.Logout;
using WebApi.Application.Features.Users.Queries.GetCurrentUser;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public AuthController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [EnableRateLimiting("AuthPolicy")]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [EnableRateLimiting("AuthPolicy")]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { Message = "Uğurla çıxış edildi." });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var result = await _mediator.Send(new GetCurrentUserQuery(_currentUserService.UserId));
        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { Message = "Şifrə uğurla dəyişdirildi." });
    }
    
    [HttpPost("external-login")]
    public async Task<ActionResult<AuthResultDto>> ExternalLogin(
        [FromBody] ExternalLoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
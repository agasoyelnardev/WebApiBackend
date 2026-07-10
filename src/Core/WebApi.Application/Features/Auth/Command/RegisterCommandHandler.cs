using MediatR;
using Microsoft.AspNetCore.Identity;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
{
    private readonly UserManager<AppUser> _userManager;

    public RegisterCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            throw new Exception("Bu email artıq istifadə olunur.");
        }

        var existingUsername =
            await _userManager.FindByNameAsync(request.UserName);

        if (existingUsername is not null)
        {
            throw new Exception("Bu istifadəçi adı artıq mövcuddur.");
        }
        
        var user = new AppUser
        {
            FullName = request.FullName,
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(
            user,
            request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }
        
        if (!result.Succeeded)
        {
            throw new Exception(
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return user.Id;
    }
}
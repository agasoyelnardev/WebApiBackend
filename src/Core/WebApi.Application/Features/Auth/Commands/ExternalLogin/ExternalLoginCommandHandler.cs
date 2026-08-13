using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Auth.Commands.ExternalLogin;

public class ExternalLoginCommandHandler : IRequestHandler<ExternalLoginCommand, AuthResultDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;
    private readonly IAppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public ExternalLoginCommandHandler(
        UserManager<AppUser> userManager,
        IJwtService jwtService,
        IConfiguration configuration,
        IAppDbContext context,
        IHttpClientFactory httpClientFactory)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _configuration = configuration;
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AuthResultDto> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Provider))
            throw new BadRequestException("Provayder qeyd olunmalıdır.");

        if (string.IsNullOrWhiteSpace(request.IdToken))
            throw new BadRequestException("ID Token boş ola bilməz.");

        var (email, name, providerKey) = request.Provider.ToLower() switch
        {
            "google" => await ValidateGoogleTokenAsync(request.IdToken),
            "facebook" => await ValidateFacebookTokenAsync(request.IdToken),
            "apple" => await ValidateAppleTokenAsync(request.IdToken, request.FullName),
            _ => throw new BadRequestException("Dəstəklənməyən provayder.")
        };

        // 1. Provayder vasitəsilə əvvəlcədən bağlanmış istifadəçi var?
        var user = await _userManager.FindByLoginAsync(request.Provider, providerKey);

        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
               
                user = new AppUser
                {
                    UserName = await GenerateUniqueUsernameAsync(email),
                    Email = email,
                    FullName = name,
                    Avatar = string.Empty,
                    EmailConfirmed = true 
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new BadRequestException($"İstifadəçi yaradılarkən xəta baş verdi: {errors}");
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new BadRequestException($"İstifadəçiyə rol təyin edilə bilmədi: {roleErrors}");
                }
            }
            
            var loginResult = await _userManager.AddLoginAsync(
                user, new UserLoginInfo(request.Provider, providerKey, request.Provider));

            if (!loginResult.Succeeded)
            {
                var errors = string.Join(", ", loginResult.Errors.Select(e => e.Description));
                throw new BadRequestException($"Provayder hesaba bağlanarkən xəta baş verdi: {errors}");
            }
        }

        if (user.IsBanned)
            throw new UnauthorizedAccessException($"Hesabınız bloklanıb. Səbəb: {user.BanReason ?? "qeyd olunmayıb"}");

        var accessToken = await _jwtService.GenerateToken(user);
        var refreshTokenValue = _jwtService.GenerateRefreshToken();

        var refreshToken = new Domain.Entities.RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResultDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue
        };
    }

    // ---------------- GOOGLE ----------------

    private async Task<(string Email, string Name, string ProviderKey)> ValidateGoogleTokenAsync(string idToken)
    {
        var googleClientId = _configuration["Authentication:Google:ClientId"]
            ?? throw new InvalidOperationException("Google Client ID konfiqurasiya edilməyib.");

        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { googleClientId }
            });
        }
        catch (InvalidJwtException)
        {
            throw new BadRequestException("Google ID Token etibarsızdır.");
        }

        if (!payload.EmailVerified)
            throw new BadRequestException("Google hesabınızın email ünvanı təsdiqlənməyib.");

        var name = payload.Name ?? payload.Email.Split('@')[0];

        return (payload.Email, name, payload.Subject);
    }

    // ---------------- FACEBOOK ----------------

    private async Task<(string Email, string Name, string ProviderKey)> ValidateFacebookTokenAsync(string accessToken)
    {
        var httpClient = _httpClientFactory.CreateClient();

        HttpResponseMessage response;
        try
        {
            response = await httpClient.GetAsync(
                $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}");
        }
        catch (HttpRequestException)
        {
            throw new BadRequestException("Facebook ilə əlaqə qurularkən xəta baş verdi.");
        }

        if (!response.IsSuccessStatusCode)
            throw new BadRequestException("Facebook Access Token etibarsızdır.");

        var content = await response.Content.ReadAsStringAsync();
        var payload = System.Text.Json.JsonSerializer.Deserialize<FacebookUserPayload>(content,
            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (payload is null || string.IsNullOrEmpty(payload.Id))
            throw new BadRequestException("Facebook hesab məlumatları alına bilmədi.");

        if (string.IsNullOrEmpty(payload.Email))
            throw new BadRequestException("Facebook hesabınızdan email məlumatı alına bilmədi. Zəhmət olmasa email icazəsini verin.");

        var name = payload.Name ?? payload.Email.Split('@')[0];

        return (payload.Email, name, payload.Id);
    }

    private class FacebookUserPayload
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

    // ---------------- APPLE ----------------

    private async Task<(string Email, string Name, string ProviderKey)> ValidateAppleTokenAsync(string idToken, string? fallbackName)
    {
        var appleClientId = _configuration["Authentication:Apple:ClientId"]
            ?? throw new InvalidOperationException("Apple Client ID konfiqurasiya edilməyib.");

        var httpClient = _httpClientFactory.CreateClient();

        string appleKeysJson;
        try
        {
            appleKeysJson = await httpClient.GetStringAsync("https://appleid.apple.com/auth/keys");
        }
        catch (HttpRequestException)
        {
            throw new BadRequestException("Apple açar məlumatları alına bilmədi.");
        }

        var appleKeys = new JsonWebKeySet(appleKeysJson);
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidIssuer = "https://appleid.apple.com",
            ValidAudience = appleClientId,
            IssuerSigningKeys = appleKeys.GetSigningKeys(),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };

        ClaimsPrincipal principal;
        try
        {
            principal = tokenHandler.ValidateToken(idToken, validationParameters, out _);
        }
        catch (Exception)
        {
            throw new BadRequestException("Apple ID Token etibarsızdır.");
        }

        var email = principal.FindFirst("email")?.Value
            ?? throw new BadRequestException("Apple hesabınızdan email məlumatı alına bilmədi.");
        var providerKey = principal.FindFirst("sub")?.Value
            ?? throw new BadRequestException("Apple istifadəçi identifikatoru tapılmadı.");
        
        var name = !string.IsNullOrWhiteSpace(fallbackName) ? fallbackName : email.Split('@')[0];

        return (email, name, providerKey);
    }

    // ---------------- ORTAQ ----------------

    private async Task<string> GenerateUniqueUsernameAsync(string email)
    {
        var baseUsername = email.Split('@')[0];
        var username = baseUsername;
        var counter = 1;

        while (await _userManager.FindByNameAsync(username) != null)
        {
            username = $"{baseUsername}{counter}";
            counter++;
        }

        return username;
    }
}
namespace WebApi.Application.Common;

public static class AvatarDefaults
{
    public const string FallbackUrl =
        "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=150&auto=format&fit=crop&q=80";

    public static string Resolve(string? avatar) =>
        string.IsNullOrWhiteSpace(avatar) ? FallbackUrl : avatar.Trim();
}

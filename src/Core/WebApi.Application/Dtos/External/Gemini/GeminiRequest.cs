namespace WebApi.Application.Dtos.External.Gemini;

public class GeminiRequest
{
    public List<GeminiContent> Contents { get; set; } = new();
    public SystemInstruction? SystemInstruction { get; set; }
}

public class GeminiContent
{
    public string Role { get; set; } = "user";
    public List<GeminiPart> Parts { get; set; } = new();
}

public class GeminiPart
{
    public string Text { get; set; } = string.Empty;
}

public class SystemInstruction
{
    public List<GeminiPart> Parts { get; set; } = new();
}

public class GeminiResponse
{
    public List<GeminiCandidate>? Candidates { get; set; }
}

public class GeminiCandidate
{
    public GeminiContent? Content { get; set; }
}
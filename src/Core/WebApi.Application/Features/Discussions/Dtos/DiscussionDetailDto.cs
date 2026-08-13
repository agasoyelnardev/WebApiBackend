namespace WebApi.Application.Features.Discussions.Dtos;

public class DiscussionDetailDto : DiscussionDto
{
    public List<CommentDto> Comments { get; set; } = new();
}
using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Base;
using WebApi.Domain.Enum;

public class Friendship:BaseEntity
{

    public string SenderId { get; set; } = string.Empty;
    public AppUser Sender { get; set; } = null!;

    public string ReceiverId { get; set; } = string.Empty;
    public AppUser Receiver { get; set; } = null!;

    public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;


}
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Application.Hubs;

public class ChatHub : Hub
{
    // İstifadəçi bir canlı yayım və ya Watch Party otağına daxil olduqda çağırılır
    public async Task JoinRoom(string roomId)
    {
        // İstifadəçini həmin otağın xüsusi SignalR qrupuna əlavə edirik
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }

    // İstifadəçi otaqdan çıxdıqda və ya səhifəni bağlayanda qrupdan çıxarılır
    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }

    // Yeni mesaj göndərildikdə yalnız həmin otaqdakı insanlara anlıq ötürülür
    public async Task SendMessage(string roomId, string username, string message)
    {
        // "ReceiveMessage" metodu ilə otaqdakı hər kəsin ekranına mesajı anında göndəririk
        await Clients.Group(roomId).SendAsync("ReceiveMessage", username, message, DateTime.UtcNow);
    }
}
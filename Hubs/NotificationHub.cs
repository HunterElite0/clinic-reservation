using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;

namespace clinic_reservation;

public class NotificationHub : Hub
{
    public async Task Notify(string user, string message)
    {
        await Clients.User(user).SendAsync("Notify", user, message);
    }
}

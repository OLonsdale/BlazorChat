using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorChat.Shared;

// Methods the server can call on connected clients
public interface IChatClient
{
    Task ReceiveMessage(Message message);
    Task UsersUpdated(List<User> users);
}

// Methods the client can call on the server hub
public interface IChatHub
{
    Task SendMessage(Message message);
    Task Join(User user);
    Task UpdateUser(User user);
    Task<List<User>> GetUsers();
    Task ClearOfflineUsers();
}

public static class HubRoutes
{
    public const string ChatHub = "/chatHub";
}

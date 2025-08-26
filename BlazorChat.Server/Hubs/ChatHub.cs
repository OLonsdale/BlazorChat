using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorChat.Shared;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChat.Server.Hubs;

public class ChatHub : Hub<IChatClient>
{
    
    /*
     *      Any public method here can be called by name, by the client.
     */
    
    
    private static readonly List<User> Users = [];

    [HubMethodName(nameof(IChatHub.SendMessage))]
    public async Task SendMessage(Message message)
    {
        await Clients.All.ReceiveMessage(message);
    }
    
    [HubMethodName(nameof(IChatHub.ClearOfflineUsers))]
    public async Task ClearOfflineUsers()
    {
        Users.RemoveAll(x => !x.Online);
        await BroadcastUsers();
    }
    
    private async Task BroadcastUsers()
    {
        var list = await GetUsers();
        await Clients.All.UsersUpdated(list);
    }

    [HubMethodName(nameof(IChatHub.Join))]
    public async Task Join(User user)
    {
        var existingUser = Users.FirstOrDefault(x => x.Id == user.Id) ?? user;
        
        Users.Remove(existingUser);
        existingUser.Name = user.Name;
        existingUser.ColorHex = user.ColorHex;
        existingUser.Online = true;
        existingUser.ActiveConnections.Add(Context.ConnectionId);
        Users.Add(existingUser);

        await BroadcastUsers();
    }

    [HubMethodName(nameof(IChatHub.UpdateUser))]
    public async Task UpdateUser(User user)
    {
        var existingUser = Users.FirstOrDefault(x => x.Id == user.Id);
        if (existingUser != null)
        {
            existingUser.Name = user.Name;
            existingUser.ColorHex = user.ColorHex;
        }
        await BroadcastUsers();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = Users.FirstOrDefault(x => x.ActiveConnections.Contains(Context.ConnectionId));
        if (user != null)
        {
            user.ActiveConnections.Remove(Context.ConnectionId);
            user.Online = user.ActiveConnections.Any();
            await BroadcastUsers();
        }
        await base.OnDisconnectedAsync(exception);
    }

    [HubMethodName(nameof(IChatHub.GetUsers))]
    public async Task<List<User>> GetUsers()
    {
        return Users.ToList();
    }


}

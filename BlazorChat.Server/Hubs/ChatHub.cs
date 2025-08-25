using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorChat.Shared;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChat.Server.Hubs;

public class ChatHub : Hub
{
    // In-memory stores (no persistence)
    private static readonly ConcurrentDictionary<Guid, User> _usersById = new();
    private static readonly ConcurrentDictionary<string, Guid> _connectionToUserId = new();

    public async Task SendMessage(Message message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    public async Task Join(Guid userId, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            username = "anon";

        // create or update user by id
        var user = _usersById.GetOrAdd(userId, id => new User
        {
            Id = id,
            Name = username,
            ColorHex = Statics.ColorForId(id),
            Online = true
        });

        // update name and online status if already exists
        user.Name = username;
        user.Online = true;
        _usersById[userId] = user;

        _connectionToUserId[Context.ConnectionId] = userId;
        await BroadcastActiveUsers();
    }

    public Task<List<User>> GetActiveUsers()
    {
        var list = new List<User>(_usersById.Values);
        return Task.FromResult(list);
    }

    public async Task SetColor(string colorHex)
    {
        if (!_connectionToUserId.TryGetValue(Context.ConnectionId, out var userId))
            return;

        if (string.IsNullOrWhiteSpace(colorHex))
            return;

        // Only allow from unified palette for now
        if (Array.IndexOf(Statics._palette, colorHex) < 0)
            return;

        if (_usersById.TryGetValue(userId, out var user))
        {
            user.ColorHex = colorHex;
            _usersById[userId] = user;
            await BroadcastActiveUsers();
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (_connectionToUserId.TryRemove(Context.ConnectionId, out var userId))
        {
            // Determine if any other active connections still exist for this userId
            var stillConnected = false;
            foreach (var kv in _connectionToUserId)
            {
                if (kv.Value == userId) { stillConnected = true; break; }
            }

            if (!stillConnected && _usersById.TryGetValue(userId, out var user))
            {
                user.Online = false;
                _usersById[userId] = user;
            }
            await BroadcastActiveUsers();
        }
        await base.OnDisconnectedAsync(exception);
    }

    private Task BroadcastActiveUsers()
    {
        var list = new List<User>(_usersById.Values);
        return Clients.All.SendAsync("ActiveUsersUpdated", list);
    }
}

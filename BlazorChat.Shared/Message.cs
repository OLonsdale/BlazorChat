using System;

namespace BlazorChat.Shared;

public class Message
{
    public string MessageText { get; set; } = string.Empty;
    public Guid SenderUserId { get; set; }
    public DateTime TimeSent { get; set; } = DateTime.Now;
    public DateTime? TimeReceived { get; set; }
}
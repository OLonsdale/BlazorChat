using System;

namespace BlazorChat.Shared;

public class Message
{
    public string MessageText { get; set; }
    public string Sender { get; set; }
    public DateTime TimeSent { get; set; } = DateTime.Now;
    public DateTime? TimeReceived { get; set; }
}
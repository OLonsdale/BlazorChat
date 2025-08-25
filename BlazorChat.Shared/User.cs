using System;

namespace BlazorChat.Shared;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#121212";
    public bool Online { get; set; }
}
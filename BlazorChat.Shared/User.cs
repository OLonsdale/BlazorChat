namespace BlazorChat.Shared;

public class User
{
    public List<string> ActiveConnections { get; set; } = new();
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = "Anonymous";
    public string ColorHex { get; set; } = Statics.Palette[Random.Shared.Next(Statics.Palette.Length)];
    public bool Online { get; set; }
}
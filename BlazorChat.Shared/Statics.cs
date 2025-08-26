namespace BlazorChat.Shared;

public static class Statics
{
    public static readonly string[] Palette = new[]
    {
        "#E91E63", "#9C27B0", "#3F51B5", "#03A9F4", "#009688", "#4CAF50", "#8BC34A", "#FFC107", "#FF9800", "#FF5722", "#795548", "#607D8B"
    };

    public static string ColorForId(Guid id)
    {
        unchecked
        {
            var hash = id.ToString().GetHashCode();
            var idx = Math.Abs(hash) % Palette.Length;
            return Palette[idx];
        }
    }
}
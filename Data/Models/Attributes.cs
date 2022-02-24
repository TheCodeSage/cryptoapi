public record Attributes(string Background, string Body, string Head, string Legs, string Arms)
{
    public string? Foreground { get; init; }
    public string? AdditionalFilter { get; init; }
}
public record Attributes(string Background, string Body, string Head, string ArmsLegs)
{
    public string? Foreground { get; init; }
    public string? Additional { get; init; }
}
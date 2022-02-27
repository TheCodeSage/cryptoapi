namespace cryptoapi.Data.Models;

public record Stats(double Health, double Defense, double Speed, double AttackMin, double AttackMax)
{
    public Skills? Skill { get; init; }
};
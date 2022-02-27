namespace cryptoapi.Data.Models;
public record Creature(string Name, string Image, int Tier, Stats Stats, IEnumerable<Trait>? Traits);
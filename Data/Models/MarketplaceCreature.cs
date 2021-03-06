namespace cryptoapi.Data.Models;

public record MarketplaceCreature : Creature
{
    public double Price { get; init; }
    public bool Sold { get; set; }

    public MarketplaceCreature(Creature creature, double price) : 
    base(creature.Name, creature.Image, creature.Tier, creature.Stats, creature.Traits)
    {
        Price = price;
    }

}
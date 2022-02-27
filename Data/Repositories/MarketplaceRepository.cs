using cryptoapi.Data.Models;
using cryptoapi.Data.Interfaces;

namespace cryptoapi.Data.Repositories;

public class MarketplaceRepository : IRepository<MarketplaceCreature>
{
    private readonly IEnumerable<MarketplaceCreature> staticData = MockData.GetMockMarketplaceCreatures();
    public IQueryable<MarketplaceCreature> Table => staticData.AsQueryable();
    public MarketplaceCreature GetByName(string name) => staticData.Single(c => c.Name == name);
}
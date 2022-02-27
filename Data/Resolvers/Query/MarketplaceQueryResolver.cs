using cryptoapi.Data.Resolvers;
using cryptoapi.Data.Interfaces;
using cryptoapi.Data.Models;

namespace cryptoapi.Data.Resolvers;

[ExtendObjectType(name: "Query")]
public class MarketplaceQueryResolver
{
    private readonly IRepository<MarketplaceCreature> _marketplaceRepository;

    public MarketplaceQueryResolver([Service] IRepository<MarketplaceCreature> marketplaceRepository) =>
    _marketplaceRepository = marketplaceRepository;

    public IQueryable<MarketplaceCreature> activeMarketplaceCreatures() => _marketplaceRepository.Table.Where(i => !i.Sold);
    public IQueryable<MarketplaceCreature> soldMarketplaceCreatures() => _marketplaceRepository.Table.Where(i => i.Sold);
}
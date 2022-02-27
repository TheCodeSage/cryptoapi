using cryptoapi.Data.Models;
using cryptoapi.CreatureGeneration;

namespace cryptoapi.Data;
public class MockData
{
    static CreatureGenerator _generator = new CreatureGenerator();
    static List<Creature> mockCreatures = new List<Creature>();
    static List<MarketplaceCreature> mockMarketplaceCreatures = new List<MarketplaceCreature>();

    public static void generateMockCreatures()
    {
        if (mockCreatures.Count == 0)
        {
            for (int i = 0; i < 20; i++)
                mockCreatures.Add(_generator.Generate());

            var rand = new Random();
            mockMarketplaceCreatures = mockCreatures.Select(i => new MarketplaceCreature(i, rand.Next(0, 100)) { Sold = rand.Next(0, 2) == 1 }).ToList();
        }
    }

    public static IEnumerable<MarketplaceCreature> GetMockMarketplaceCreatures()
    {
        if (mockMarketplaceCreatures.Count == 0)
            generateMockCreatures();


        return mockMarketplaceCreatures;
    }
}
public class MockData
{
    public static IEnumerable<Creature> GetMockCreatures => new List<Creature>
    {
        new Creature("Dargon", "base64://9u932jf3...", 1, new Stats(120, 6, 1, 3, 5), new Attributes("./background-forest1","body-normal1", "head-normal1", "armslegs-normal1"){Foreground="grass"}),
        new Creature("Wolf", "base64://as31jf3...", 1, new Stats(100, 5, 3, 3, 4), new Attributes("./background-forest1","body-tanktop2", "head-hat1", "legs-baggypants1")),
        new Creature("Shrimp", "base64://stf93jf3...", 1, new Stats(80, 2, 6, 1, 3), new Attributes("./background-forest2","body-normal1", "head-normal1", "legs-normal2")),
        new Creature("PentaPenii Cow", "base64://fg2j13...",1, new Stats(150, 1, 1, 1, 10), new Attributes("./background-mountain1","body-gross2", "head-normal1", "arms-normal2"){Additional="radial-blur"}),
        new Creature("Dargon", "base64://9u932jf3...", 1, new Stats(120, 6, 1, 3, 5), new Attributes("./background-forest1","body-normal1", "head-normal1", "arms-rolex1"){Foreground="grass"}),
        new Creature("Wolf", "base64://as31jf3...", 1, new Stats(100, 5, 3, 3, 4), new Attributes("./background-forest1","body-tanktop2", "head-hat1", "arms-normal1")),
        new Creature("Shrimp", "base64://stf93jf3...", 1, new Stats(80, 2, 6, 1, 3), new Attributes("./background-forest2","body-normal1", "head-normal1", "arms-sweatbands2")),
        new Creature("PentaPenii Cow", "base64://fg2j13...", 1, new Stats(150, 1, 1, 1, 10), new Attributes("./background-mountain1","body-gross2", "head-normal1", "legs-baggypants1"){Additional="radial-blur"}),
    };

    public static IEnumerable<MarketplaceCreature> GetMockMarketplaceCreatures()
    {
        var rand = new Random();
        return GetMockCreatures.Select(i => new MarketplaceCreature(i, rand.Next(0, 100)) { Sold = rand.Next(0, 2) == 1 });
    }
}
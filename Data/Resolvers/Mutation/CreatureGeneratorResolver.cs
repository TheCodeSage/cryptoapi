using cryptoapi.CreatureGeneration;
using cryptoapi.Data.Models;

namespace cryptoapi.Data.Resolvers;

[ExtendObjectType(name: "Mutation")]
public class CreatureGeneratorResolver
{
    private readonly CreatureGenerator _Generator;
    public CreatureGeneratorResolver(CreatureGenerator generator)
    {
        _Generator = generator;
    }

    public Creature Generate => _Generator.Generate();
    public Creature Fuse(Creature c1, Creature c2) => _Generator.Fuse(c1, c2);
}
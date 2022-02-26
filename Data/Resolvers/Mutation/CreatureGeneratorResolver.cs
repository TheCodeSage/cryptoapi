[ExtendObjectType(name: "Mutation")]
public class CreatureGeneratorResolver
{
    private readonly CreatureGenerator _Generator;
    public CreatureGeneratorResolver(CreatureGenerator generator)
    {
        _Generator = generator;
    }

    public Creature Generate => _Generator.Generate();
}
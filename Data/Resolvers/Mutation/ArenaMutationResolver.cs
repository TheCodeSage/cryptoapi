[ExtendObjectType(name: "Mutation")]
public class ArenaMutationResolver
{
    public string Battle(Creature c1, Creature c2) => BattleSimulator.Simulate(c1, c2);
}
using cryptoapi.Data.Models;

namespace cryptoapi.Data.Resolvers.Mutation;
public class BattleSimulator
{
    public static string Simulate(Creature creature1, Creature creature2)
    {
        var (first, second) = getBattleOrder(creature1, creature2);

        while (first.IsAlive && second.IsAlive)
        {
            first.Attack(second);

            if (first.IsAlive && second.IsAlive)
            {
                second.Attack(first);
            }
            else break;
        }

        return first.Stats.Health > 0 ? first.Name : second.Name;
    }

    private static (BattleCreature first, BattleCreature second) getBattleOrder(Creature c1, Creature c2)
    {
        var bc1 = new BattleCreature(c1);
        var bc2 = new BattleCreature(c2);

        if (bc1.Stats.Speed > bc2.Stats.Speed)
        {
            return (bc1, bc2);
        }
        else if (bc2.Stats.Speed > bc1.Stats.Speed)
        {
            return (bc2, bc1);
        }
        else
        {
            var rand = new Random();
            var randVal = rand.Next(0, 2);

            if (randVal == 0)
                return (bc1, bc2);
            else
                return (bc2, bc1);
        }
    }
}
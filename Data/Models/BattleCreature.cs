namespace cryptoapi.Data.Models;
public record BattleCreature : Creature
{
    public bool IsAlive => currentHealth > 0;

    private double currentHealth;
    private double currentAttackMin;
    private double currentAttackMax;
    private double currentDefense;
    private double currentSpeed;

    //Currently, we increase attack damage based on this ratio, 
    //which is calculated from the difference in creatures speed
    private double currentRatio;

    public BattleCreature(Creature creature) :
     base(creature.Name, creature.Image, creature.Tier, creature.Stats, creature.Traits)
    {
        currentHealth = creature.Stats.Health;
        currentDefense = creature.Stats.Defense;
        currentAttackMin = creature.Stats.AttackMin;
        currentAttackMax = creature.Stats.AttackMax;
        currentSpeed = creature.Stats.Speed;
    }

    public void Attack(BattleCreature enemy)
    {
        currentRatio = Math.Max(Stats.Speed / enemy.Stats.Speed, 1);

        attemptToUseSkill();

        var rand = new Random();
        var damage = (rand.NextDouble() * (currentAttackMax - currentAttackMin) + (currentAttackMin)) * currentRatio;

        enemy.TakeDamage(damage);

        resetAttackStats();
    }

    private void TakeDamage(double damage)
    {
        var defenseModifier = ((100 - currentDefense) / 100);

        currentHealth = Math.Max(0, currentHealth - (damage * defenseModifier));

        resetDefenseStats();
    }

    private void attemptToUseSkill()
    {
        var rand = new Random();
        var useSkill = rand.Next(0, 50) == 1;

        if (useSkill)
        {
            switch (Stats.Skill)
            {
                case Skills.IncreaseAttack:
                    currentAttackMin += Stats.AttackMin * .25;
                    currentAttackMax += Stats.AttackMax * .25;
                    break;
                case Skills.IncreaseDefense:
                    currentDefense += Stats.Defense * .25;
                    break;
                case Skills.IncreaseSpeed:
                    currentRatio += currentRatio * .25;
                    break;
                case Skills.RecoverHealth:
                    var newHealth = currentHealth + Stats.Health * .10;
                    currentHealth = newHealth > Stats.Health ?
                    Stats.Health :
                    newHealth;
                    break;
            }
        }
    }

    private void resetDefenseStats()
    {
        currentDefense = Stats.Defense;
    }

    private void resetAttackStats()
    {
        currentAttackMin = Stats.AttackMin;
        currentAttackMax = Stats.AttackMax;
        currentSpeed = Stats.Speed;
    }

}
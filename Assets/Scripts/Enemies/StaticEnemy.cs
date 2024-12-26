using UnityEngine;

public class StaticEnemy : Enemy
{
    protected override void Update()
    {
        base.Update();

        if (stunTime > 0) stunTime = 0;

        if (PlayerInAttackRange() && attackCD <= attackTimer)
        {
            player.GetComponent<HP>().TakeDamage(DamageCalculator());
            attackTimer = 0;
        }
    }

    protected override float DamageCalculator()
    {
        return GetComponent<Stat>().GetInt();
    }
}

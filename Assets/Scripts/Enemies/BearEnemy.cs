using UnityEngine;

public class BearEnemy : MeleeEnemy
{
    [SerializeField] private float attackRateSelect = 0.4f;
    private float attackRate;

    // Attack Stats
    [SerializeField] private float mod_str;
    [SerializeField] private float stunDuration;

    public override void Attack()
    {
        isAttacking = true;
        attackRate = Random.Range(0.0f, 1.0f);

        transform.LookAt(player.transform);

        if (attackRate > attackRateSelect)
            Attack_01();
        else
            Attack_02();
    }

    private void Attack_01()
    {
        animator.SetTrigger("attack_01");
    }

    private void Attack_01_Damage()
    {
        if (playerInAttackRange != null)
        {
            player.GetComponent<HP>().TakeDamage(DamageCalculator());
        }
    }

    private void Attack_02()
    {
        animator.SetTrigger("attack_02");
    }

    private void Attack_02_Damage()
    {
        if (playerInAttackRange != null)
        {
            if (player.GetComponent<HP>().defeat) return;

            player.GetComponent<CharacterMovement>().getStun(stunDuration);
            player.GetComponent<HP>().TakeDamage(DamageCalculator());
        }
    }

    protected override bool InAttackAnimation()
    {
        return animator.GetBool("attack_01") || animator.GetBool("attack_02");
    }

    protected override float DamageCalculator()
    {
        return GetComponent<Stat>().GetStr() * mod_str;
    }
}

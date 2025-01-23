using UnityEngine;
using System.Collections;

public class MeleeEnemy : Enemy
{
    bool isChasing = false;

    protected override void Update() {
        
        base.Update();
        if (animator.GetBool("stun") || isDead)
            return;
            
        if (InAttackAnimation() || isAttacking)
        {
            isChasing = false;
            animator.SetBool("moving", false);
            return;
        }

        if (PlayerInRange() && !PlayerInAttackRange())
        {
            isChasing = true;
        }
        else if (!PlayerInRange())
        {
            isChasing = false;
            isAttacking = false;

            animator.SetBool("moving", false);
        }

        if (PlayerInAttackRange())
        {
            isChasing = false;
            animator.SetBool("moving", false);
            if (attackTimer >= attackCD)
            {
                attackTimer = 0;
                Attack();
            }
        }
        else if (!PlayerInAttackRange())
        {
            isAttacking = false;
        }   
        
        if (isChasing)
        {
            Chase();
        }
    }

    private void Chase()
    {
        animator.SetBool("moving", true);
        transform.LookAt(player.transform.position);
        controller.SimpleMove(transform.forward * speed);
    }   

    public override void Attack()
    {
        animator.SetTrigger("meleeAttack");
    }

    void DamagePlayer()
    {
        if (PlayerInAttackRange() /* && !Skill.unstoppable */)
        {
            player.GetComponent<HP>().TakeDamage(DamageCalculator());
        }
    }
}

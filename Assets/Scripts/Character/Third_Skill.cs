using UnityEngine;

public class Third_Skill : Skill
{
    [SerializeField] private float stun_dur;

    /*
        Third Skill: Jump and slam the ground
        Modifier: 1.2*str
        Stun Duration: 1 seconds
        CD: 10 seconds
    */
    protected override void UseSkill()
    {
        isUsingSkill = true;
        CharacterMovement.isAttacking = true;

        animator.SetTrigger("skill_3");
    }

    // For Animation Event
    void ActivateSkillEffect_3()
    {
        SkillEffect();
    }

    protected override void SkillEffect()
    {
        Instantiate(effects[0], transform.position, Quaternion.identity);

        if (enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                Instantiate(effects[1], 
                    new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, enemy.transform.position.z), 
                    Quaternion.identity);
                enemy.GetComponent<Enemy>().getStun(stun_dur);
                enemy.GetComponent<HP>().TakeDamage(GetComponent<Stat>().GetStr()*mod);
            }
        }

        FinishSkill();
    }

}

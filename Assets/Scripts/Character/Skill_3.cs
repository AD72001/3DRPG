using UnityEngine;

public class Skill_3 : Skill
{
    [SerializeField] private float stun_dur;

    /*
        Third Skill: Jump and slam the ground
        Modifier: 1.2*str
        Stun Duration: 1 seconds
        CD: 10 seconds
    */
    public string GetDescription()
    {
        string desc = $"Jump up and slam the ground, deal {DamageCalculator()} damage and stun the enemies in range for {stun_dur} second(s).";
        return desc;
    }

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
                enemy.GetComponent<HP>().TakeDamage(DamageCalculator());
            }
        }

        FinishSkill();
    }

    private float DamageCalculator()
    {
        return GetComponent<Stat>().GetStr()*mod_str + GetComponent<Stat>().GetInt()*mod_int;
    }

}

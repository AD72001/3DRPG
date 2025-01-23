using System.Collections;
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
    private void Start() {
        skillName = "Heroic Slam";
        desc = $"Jump up and slam the ground, deal {DamageCalculator()} damage and stun the enemies in range for {stun_dur} second(s).";
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
        AudioManager.instance.PlaySound(skillSound);

        stun_dur = stun_dur*LevelScale()[1];

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
        return GetComponent<Stat>().GetStr()*mod_str*(float)LevelScale()[0] 
            + GetComponent<Stat>().GetInt()*mod_int*(float)LevelScale()[1];
    }

    private float[] LevelScale()
    {
        float[] scale = new float[2];
        // Str - Int
        switch (level)
        {
            case 1: 
                scale[0] = 1.0f;
                scale[1] = 0.7f;
                break;
            case 2: 
                scale[0] = 1.1f;
                scale[1] = 0.8f;
                break;
            case 3: 
                scale[0] = 1.2f;
                scale[1] = 0.9f;
                break;
            case 4: 
                scale[0] = 1.3f;
                scale[1] = 1.2f;
                break;
            default:
                scale[0] = 0.0f;
                scale[1] = 0.0f;
                break;
        }

        return scale;
    }
}

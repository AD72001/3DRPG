using System.Collections;
using UnityEngine;

public class Skill_4 : Skill {
    [SerializeField] private float duration;

    /*
        Fourth Skill: Spinning
        Modifier: 0.8*str
        Stun Duration: 0.5 seconds
        CD: 25 seconds
    */
    private void Start() {
        skillName = "Slashing Tornado";
        desc = $"Spin the blade for {duration} seconds, deal damage each seconds to enemies in range.";
    }

    protected override void UseSkill()
    {
        isUsingSkill = true;
        unstoppable = true;

        animator.SetBool("skill_4", true);
        AudioManager.instance.PlaySound(skillSound);

        StartCoroutine(UseSkillAsync());
    }

    IEnumerator UseSkillAsync()
    {
        Physics.IgnoreLayerCollision(3, 6, true);

        yield return new WaitForSeconds(duration);

        animator.SetBool("skill_4", false);

        Physics.IgnoreLayerCollision(3, 6, false);
        unstoppable = false;

        FinishSkill();
    }

    // For Animation Event
    void ActivateSkillEffect_4()
    {
        SkillEffect();
    }

    protected override void SkillEffect()
    {
        if (enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                Instantiate(effects[0], 
                    new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, enemy.transform.position.z), 
                    Quaternion.identity);

                enemy.GetComponent<HP>().TakeDamage(DamageCalculator());
            }
        }
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
                scale[1] = 0.8f;
                break;
            case 2: 
                scale[0] = 1.1f;
                scale[1] = 1.0f;
                break;
            case 3: 
                scale[0] = 1.2f;
                scale[1] = 1.2f;
                break;
            case 4: 
                scale[0] = 1.5f;
                scale[1] = 1.3f;
                break;
            default:
                scale[0] = 0.0f;
                scale[1] = 0.0f;
                break;
        }

        return scale;
    }

}

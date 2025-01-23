using System.Collections;
using UnityEngine;

public class Skill_1 : Skill
{
    [SerializeField] private float stun_dur;

    
    /* First Skill: Melee Attack to stun, 
        Modifier: 1.5 str
        Stun Duration: 1 seconds
        CD: 5 seconds
    */
    private void Start() {
        skillName = "Straight Slash";
        desc = $"Slash forward, deal {DamageCalculator()} damage to enemy in range, stun them for {stun_dur} second(s)";
    }

    protected override void UseSkill()
    {
        isUsingSkill = true;
        CharacterMovement.isAttacking = true;

        LookAtPosition();

        Instantiate(effects[0], range.transform.position, Quaternion.LookRotation(transform.forward));
        animator.SetTrigger("skill_1");
        AudioManager.instance.PlaySound(skillSound);
    }

    void ActivateSkillEffect_1()
    {
        SkillEffect();
    }
    
    protected override void SkillEffect()
    {
        if (enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                Instantiate(effects[1], 
                    new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, enemy.transform.position.z),
                    Quaternion.identity);
                enemy.GetComponent<HP>().TakeDamage(DamageCalculator());
                enemy.GetComponent<Enemy>().getStun(stun_dur*(float)LevelScale()[1]);
            }
        }

        FinishSkill();
    }

    private float DamageCalculator()
    {
        return GetComponent<Stat>().GetStr() * mod_str * (float)LevelScale()[0] 
            + GetComponent<Stat>().GetInt() * mod_int * (float)LevelScale()[1];
    }

    private float[] LevelScale()
    {
        float[] scale = new float[2];
        // Str - Int
        switch (level)
        {
            case 1: 
                scale[0] = 1.0f;
                scale[1] = 1.0f;
                break;
            case 2: 
                scale[0] = 1.2f;
                scale[1] = 1.1f;
                break;
            case 3: 
                scale[0] = 1.5f;
                scale[1] = 1.2f;
                break;
            case 4: 
                scale[0] = 1.8f;
                scale[1] = 1.3f;
                break;
            default:
                scale[0] = 0.0f;
                scale[1] = 0.0f;
                break;
        }

        return scale;
    }

    void LookAtPosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 2000))
        {
            transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        }
    }
}

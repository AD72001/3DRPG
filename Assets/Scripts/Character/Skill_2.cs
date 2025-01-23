using System.Collections;
using UnityEngine;

public class Skill_2 : Skill
{
    [SerializeField] private float duration;

    /* Second Skill: Buff user's stats, 
        Modifier: def, str, vit, int
        Duration: 10 seconds
        CD: 20 seconds
    */
    private void Start() {
        skillName = "Iron Will";
        desc = $"Increase player's strength, defense for a duration of {duration} seconds, restore {HPRestoreCalculator()} HP.";
    }

    protected override void UseSkill()
    {
        isUsingSkill = true;

        animator.SetTrigger("skill_2");
        AudioManager.instance.PlaySound(skillSound);
        
        GameObject initEffect = Instantiate(effects[0], transform.position, Quaternion.identity);
        GameObject shieldEffect = Instantiate(effects[1], transform.position, Quaternion.identity);

        shieldEffect.transform.SetParent(transform);

        Destroy(initEffect, 1);
        Destroy(shieldEffect, duration);

        SkillEffect();
    }

    protected override void SkillEffect()
    {
        StartCoroutine(BuffEffect());
    }

    IEnumerator BuffEffect()
    {
        int amountStr = (int) (GetComponent<Stat>().GetStr() * mod_str * (float) LevelScale()[0] - GetComponent<Stat>().GetStr());
        int amountDef = (int) (GetComponent<Stat>().GetDef() * mod_def * (float) LevelScale()[1] - GetComponent<Stat>().GetDef());

        GetComponent<Stat>().AddStr(amountStr);
        GetComponent<Stat>().AddDef(amountDef);
        GetComponent<HP>().AddHP(HPRestoreCalculator());

        yield return new WaitForSeconds(duration);

        GetComponent<Stat>().AddStr(-amountStr);
        GetComponent<Stat>().AddDef(-amountDef);
    }

    private float[] LevelScale()
    {
        float[] scale = new float[2];
        // Str - Def
        switch (level)
        {
            case 1: 
                scale[0] = 1.0f;
                scale[1] = 0.5f;
                break;
            case 2: 
                scale[0] = 1.1f;
                scale[1] = 0.7f;
                break;
            case 3: 
                scale[0] = 1.2f;
                scale[1] = 1.0f;
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

    private float HPRestoreCalculator()
    {
        return GetComponent<Stat>().GetVit()*mod_vit + GetComponent<Stat>().GetInt()*mod_int;
    }
}

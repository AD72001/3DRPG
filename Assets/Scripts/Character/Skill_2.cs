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
    public string GetDescription()
    {
        string desc = $"Increase player's strength, defense for a duration of {duration} seconds, restore {HPRestoreCalculator()} HP.";
        return desc;
    }

    protected override void UseSkill()
    {
        isUsingSkill = true;

        animator.SetTrigger("skill_2");

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
        int amountStr = (int) (GetComponent<Stat>().GetStr() * mod_str - GetComponent<Stat>().GetStr());
        int amountDef = (int) (GetComponent<Stat>().GetDef() * mod_def - GetComponent<Stat>().GetDef());

        GetComponent<Stat>().AddStr(amountStr);
        GetComponent<Stat>().AddDef(amountDef);
        GetComponent<HP>().AddHP(HPRestoreCalculator());

        yield return new WaitForSeconds(duration);

        GetComponent<Stat>().AddStr(-amountStr);
        GetComponent<Stat>().AddDef(-amountDef);
    }

    private float HPRestoreCalculator()
    {
        return GetComponent<Stat>().GetVit()*mod_vit + GetComponent<Stat>().GetInt()*mod_int;
    }
}

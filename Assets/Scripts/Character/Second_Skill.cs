using System.Collections;
using UnityEngine;

public class Second_Skill : Skill
{
    [SerializeField] private float duration;
    [SerializeField] private float mod_str;
    [SerializeField] private float mod_def;
    [SerializeField] private float mod_vit;

    
    /* First Skill: Melee Attack to stun, 
        Modifier: 1.5 str
        Stun Duration: 1 seconds
        CD: 5 seconds
    */
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
        int amountStr = (int) (GetComponent<Stat>().str * mod_str - GetComponent<Stat>().str);
        int amountDef = (int) (GetComponent<Stat>().def * mod_def - GetComponent<Stat>().def);

        GetComponent<Stat>().AddStr(amountStr);
        GetComponent<Stat>().AddDef(amountDef);
        GetComponent<HP>().AddHP(mod_vit);

        yield return new WaitForSeconds(duration);

        GetComponent<Stat>().AddStr(-amountStr);
        GetComponent<Stat>().AddDef(-amountDef);
    }
}

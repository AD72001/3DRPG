using System.Collections;
using UnityEngine;

public class Fourth_Skill : Skill {
    [SerializeField] private float duration;

    /*
        Fourth Skill: Spinning
        Modifier: 0.8*str
        Stun Duration: 0.5 seconds
        CD: 25 seconds
    */
    protected override void UseSkill()
    {
        isUsingSkill = true;
        unstoppable = true;

        animator.SetBool("skill_4", true);

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

                enemy.GetComponent<HP>().TakeDamage(GetComponent<Stat>().str*mod);
            }
        }
    }

}

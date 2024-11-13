using UnityEngine;

public class First_Skill : Skill
{
    [SerializeField] private float first_stun_dur;

    
    /* First Skill: Melee Attack to stun, 
        Modifier: 1.5 str
        Stun Duration: 1 seconds
        CD: 5 seconds
    */
    protected override void UseSkill()
    {
        isUsingSkill = true;
        CharacterMovement.isAttacking = true;

        LookAtPosition();

        Instantiate(effects[0], range.transform.position, Quaternion.LookRotation(transform.forward));
        animator.SetTrigger("skill_1");
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
                enemy.GetComponent<Enemy>().getStun(first_stun_dur);
            }
        }

        FinishSkill();
    }

    private float DamageCalculator()
    {
        return GetComponent<Stat>().GetStr() * mod_str + GetComponent<Stat>().GetInt() * mod_int;
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

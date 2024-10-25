using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public static bool isUsingSkill;

    // Enemy being hit list
    public List<GameObject> enemies;

    // Components
    private Animator animator;    
    [SerializeField] private LayerMask enemyLayer;

    // First Skill Related Stats
    [SerializeField] private GameObject first_effect;
    [SerializeField] private Transform first_range;
    [SerializeField] private float first_mod;
    [SerializeField] private float stunTime;
    [SerializeField] private float first_CD;
    private float first_timer;


    void Awake() {
        animator = GetComponent<Animator>();
        enemies = new List<GameObject>();

        first_timer = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Skill on Q
            if (first_timer <= 0)
            {
                FirstSkill();
                first_timer = first_CD;
            }
            else
            {
                Debug.Log("First Skill not available");
            }

        }

        first_timer = Mathf.Clamp(first_timer - Time.deltaTime, 0, first_CD);
    }


    /* First Skill: Melee Attack to stun, 
        Modifier: 1.5 str
        Stun Duration: 2 seconds
        CD: 10 seconds
    */
    void FirstSkill()
    {
        isUsingSkill = true;

        CharacterMovement.isAttacking = true;
        animator.SetTrigger("attack_02");
    }

    void FirstSkillDamage()
    {
        if (enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                Instantiate(first_effect, 
                    new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, enemy.transform.position.z),
                    Quaternion.identity);
                enemy.GetComponent<HP>().TakeDamage(GetComponent<Stat>().str * first_mod);
                enemy.GetComponent<Enemy>().getStun(stunTime);
            }
        }
    }

    void FinishSkill()
    {
        CharacterMovement.isAttacking = false;
        isUsingSkill = false;
    }
}

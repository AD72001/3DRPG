using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public static bool isUsingSkill;

    // Enemy being hit list
    public List<GameObject> first_enemies;
    public List<GameObject> third_enemies;
    public List<GameObject> fourth_enemies;

    // Components
    private Animator animator;    
    [SerializeField] private LayerMask enemyLayer;

    // First Skill Related Stats
    [Header("First Skill")]
    [SerializeField] private GameObject[] first_effect; // 0: slash 1: hit
    [SerializeField] private Collider first_range;
    [SerializeField] private float first_mod;
    [SerializeField] private float first_stun_dur;
    [SerializeField] private float first_cost; // energy cost
    [SerializeField] public float first_CD;
    public float first_timer;

    [Header("Second SKill")]
    [SerializeField] private GameObject[] second_effect; // 0: buff 1: shield
    [SerializeField] private float second_mod_vit;
    [SerializeField] private float second_mod_def;
    [SerializeField] private float second_mod_str;
    [SerializeField] private float second_duration;
    [SerializeField] private float second_cost; // energy cost
    [SerializeField] public float second_CD;
    public float second_timer;

    [Header("Third Skill")]
    [SerializeField] private GameObject[] third_effect;
    [SerializeField] private Collider third_range;
    [SerializeField] private float third_mod_str;
    [SerializeField] private float third_stun_dur;
    [SerializeField] private float third_cost; // energy cost
    [SerializeField] public float third_CD;
    public float third_timer;

    [Header("Fourth Skill")]
    [SerializeField] private GameObject[] fourth_effect;
    [SerializeField] private Collider fourth_range;
    [SerializeField] private float fourth_mod_str;
    [SerializeField] private float fourth_dur;
    [SerializeField] private float fourth_cost; // energy cost
    [SerializeField] public float fourth_CD;
    public float fourth_timer;
    public bool unstoppable = false;


    void Awake() {
        animator = GetComponent<Animator>();

        first_enemies = new List<GameObject>();
        third_enemies = new List<GameObject>();

        first_timer = 0;
        second_timer = 0;
        third_timer = 0;
    }

    void Update()
    {
        if (isUsingSkill)
        {
            Debug.Log("Can't use skill right now");
            return;
        }

        if  (GetComponent<Energy>().GetEnergy() <= 0)
        {
            Debug.Log("Out of energy");
            return;
        }

        // First skill key
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (first_timer <= 0 && GetComponent<Energy>().GetEnergy() >= first_cost)
            {
                CharacterCombat.normalAtk = false;
                FirstSkill();
                first_timer = first_CD;
                GetComponent<Energy>().AddEnergy(-first_cost);
            }
            else
            {
                Debug.Log("First skill not available");
            }
        }

        // Second skill key
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (second_timer <= 0 && GetComponent<Energy>().GetEnergy() >= second_cost)
            {
                CharacterCombat.normalAtk = false;
                SecondSkill();
                second_timer = second_CD;
                GetComponent<Energy>().AddEnergy(-second_cost);
            }
            else
            {
                Debug.Log("Second skill not available");
            }
        }

        // Third skill key
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (third_timer <= 0 && GetComponent<Energy>().GetEnergy() >= third_cost)
            {
                CharacterCombat.normalAtk = false;
                ThirdSkill();
                third_timer = third_CD;
                GetComponent<Energy>().AddEnergy(-third_cost);
            }
            else
            {
                Debug.Log("Third skill not available");
            }
        }

        // Fourth skill key
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (fourth_timer <= 0 && GetComponent<Energy>().GetEnergy() >= fourth_cost)
            {
                CharacterCombat.normalAtk = false;
                StartCoroutine(FourthSkill());
                fourth_timer = fourth_CD;
                GetComponent<Energy>().AddEnergy(-fourth_cost);
            }
            else
            {
                Debug.Log("Fourth skill not available");
            }
        }

        first_timer = Mathf.Clamp(first_timer - Time.deltaTime, 0, first_CD);
        second_timer = Mathf.Clamp(second_timer - Time.deltaTime, 0, second_CD);
        third_timer = Mathf.Clamp(third_timer - Time.deltaTime, 0, third_CD);
        fourth_timer = Mathf.Clamp(fourth_timer - Time.deltaTime, 0, fourth_CD);
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


    /* First Skill: Melee Attack to stun, 
        Modifier: 1.5 str
        Stun Duration: 1 seconds
        CD: 5 seconds
    */
    void FirstSkill()
    {
        isUsingSkill = true;
        CharacterMovement.isAttacking = true;

        LookAtPosition();

        Instantiate(first_effect[0], first_range.transform.position, Quaternion.LookRotation(transform.forward));
        animator.SetTrigger("skill_1");
    }

    void FirstSkillDamage()
    {
        if (first_enemies.Count > 0)
        {
            foreach (var enemy in first_enemies)
            {
                Instantiate(first_effect[1], 
                    new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, enemy.transform.position.z),
                    Quaternion.identity);
                enemy.GetComponent<HP>().TakeDamage(GetComponent<Stat>().str * first_mod);
                enemy.GetComponent<Enemy>().getStun(first_stun_dur);
            }
        }

        FinishSkill();
    }

    /* 
        Second Skill: Buff self's stats for a duration of time
        Modifier: 0.8def 0.5str 
        Buff Duration: 10 seconds
        CD: 20 seconds
    */
    void SecondSkill()
    {
        isUsingSkill = true;

        animator.SetTrigger("skill_2");

        GameObject initEffect = Instantiate(second_effect[0], transform.position, Quaternion.identity);
        GameObject shieldEffect = Instantiate(second_effect[1], transform.position, Quaternion.identity);

        shieldEffect.transform.SetParent(transform);

        Destroy(initEffect, 1);
        Destroy(shieldEffect, second_duration);

        StartCoroutine(SecondSkillBuff());
    }

    IEnumerator SecondSkillBuff()
    {
        int amountStr = (int) (GetComponent<Stat>().str * second_mod_str - GetComponent<Stat>().str);
        int amountDef = (int) (GetComponent<Stat>().def * second_mod_def - GetComponent<Stat>().def);

        GetComponent<Stat>().AddStr(amountStr);
        GetComponent<Stat>().AddStr(amountDef);
        GetComponent<HP>().AddHP(second_mod_vit);

        yield return new WaitForSeconds(second_duration);

        GetComponent<Stat>().AddStr(-amountStr);
        GetComponent<Stat>().AddStr(-amountDef);
    }

    /*
        Third Skill: Jump and slam the ground
        Modifier: 1.2*str
        Stun Duration: 1 seconds
        CD: 10 seconds
    */
    void ThirdSkill()
    {
        isUsingSkill = true;
        CharacterMovement.isAttacking = true;

        animator.SetTrigger("skill_3");
    }

    void ThirdSkillDamage()
    {
        Instantiate(third_effect[0], transform.position, Quaternion.identity);

        if (third_enemies.Count > 0)
        {
            foreach (var enemy in third_enemies)
            {
                Instantiate(first_effect[1], 
                    new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, enemy.transform.position.z), 
                    Quaternion.identity);
                enemy.GetComponent<Enemy>().getStun(third_stun_dur);
                enemy.GetComponent<HP>().TakeDamage(GetComponent<Stat>().str*third_mod_str);
            }
        }

        FinishSkill();
    }

    /*
        Fourth Skill: Spinning
        Modifier: 0.8*str
        Stun Duration: 0.5 seconds
        CD: 25 seconds
    */
    IEnumerator FourthSkill()
    {
        isUsingSkill = true;
        unstoppable = true;

        animator.SetBool("skill_4", true);

        Physics.IgnoreLayerCollision(3, 6, true);

        yield return new WaitForSeconds(fourth_dur);

        animator.SetBool("skill_4", false);

        Physics.IgnoreLayerCollision(3, 6, false);
        unstoppable = false;

        FinishSkill();
    }

    void FourthSkillDamage()
    {
        // Instantiate(fourth_effect[0], transform.position, Quaternion.identity);

        if (fourth_enemies.Count > 0)
        {
            foreach (var enemy in fourth_enemies)
            {
                Instantiate(first_effect[1], 
                    new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, enemy.transform.position.z), 
                    Quaternion.identity);

                enemy.GetComponent<HP>().TakeDamage(GetComponent<Stat>().str*fourth_mod_str);
            }
        }

        fourth_enemies.Clear();
    }
    
    void FinishSkill()
    {
        CharacterMovement.isAttacking = false;
        isUsingSkill = false;

        unstoppable = false;
    }
}

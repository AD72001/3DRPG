using UnityEngine;
using UnityEngine.AI;

public class WizardEnemy: MonoBehaviour
{
    // Player Component
    protected GameObject player;

    // Enemy Stat
    public string enemyName;
    [SerializeField] private float safeRange;
    private int exp;

    [SerializeField] private Transform[] safePoints;
    private int safePointIndex = 0;

    private float stunTime = 0;
    
    // Skills Rate
    [SerializeField] private float attackRateSelect = 0.5f;
    private float attackRate;

    // Skills effect
    [SerializeField] private GameObject spell1;
    [SerializeField] private GameObject spell2;
    [SerializeField] private GameObject spell3;

    // Cooldown
    [SerializeField] private float cd1;
    private float timer1 = Mathf.Infinity;
    [SerializeField] private float cd2;
    private float timer2 = Mathf.Infinity;

    bool isFleeing = false;
    bool isAttacking = false;
    bool isDead = false;
    public bool active = false;

    // Component
    private CharacterController controller;
    private NavMeshAgent agent;
    private Animator animator;

    private MouseUI mouseUI;

    
    void Start()
    {
        exp = GetComponent<Stat>().level * 5;

        player = GameObject.FindGameObjectWithTag("Player");
        //controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        mouseUI = GameObject.FindGameObjectWithTag("CursorUI").GetComponent<MouseUI>();
    }

    void Update()
    {
        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (!active) return;

        if (GetComponent<HP>().defeat)
        {
            if (!isDead) 
                Dead();
            return;
        }

        // Stun
        if (stunTime > 0)
        {
            Stun();
            return;
        }

        if (InAttackAnimation() || isAttacking)
        {
            return;
        }

        //Fleeing if player too close
        if (Vector3.Distance(player.transform.position, transform.position) <= safeRange)
        {
            isFleeing = true;
        }

        if (isFleeing)
        {
            Flee();
            return;
        }

        //Attack
        Attack();
    }

    // Stun effect
    public void getStun(float time)
    {
        if (GetComponent<HP>().defeat)
            return;
        
        stunTime = time;
        animator.SetBool("stun", true);
        isAttacking = false;
    }

    private void Stun()
    {
        stunTime -= Time.deltaTime;

        if (stunTime <= 0)
        {
            animator.SetBool("stun", false);
        }
    }

    void Flee()
    {
        if (Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(safePoints[safePointIndex].position.x, 0, 
                    safePoints[safePointIndex].position.z)) < 0.2f)
        {
            animator.SetBool("moving", false);
            isFleeing = false;
            safePointIndex = Mathf.Clamp(safePointIndex+1, 0, safePoints.Length - 1);
            return;
        }

        animator.SetBool("moving", true);
        agent.destination = safePoints[safePointIndex].position;
    }

    void Dead()
    {
        isDead = true;

        player.GetComponent<Stat>().AddExp(exp);
        exp = 0;

        player.GetComponent<CharacterCombat>().opponent = null;
        
        animator.SetBool("stun", false);

        CharacterMovement.isAttacking = false;
    }

    public bool getDeadStatus()
    {
        return isDead;
    }

    private void OnMouseOver()
    {
        Cursor.SetCursor(mouseUI.mouseOnEnemy, Vector3.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
    }

    private void OnMouseDown() {
        if (!isDead)
            player.GetComponent<CharacterCombat>().opponent = gameObject;
    }

    public void FinishAttack()
    {
        isAttacking = false;
        animator.SetBool("attacking", false);
    }

    public void Attack()
    {
        isAttacking = true;
        animator.SetBool("attacking", true);

        attackRate = Random.Range(0.0f, 1.0f);

        transform.LookAt(player.transform);

        if (attackRate > attackRateSelect && timer1 >= cd1)
        {            
            Attack_01();
            timer1 = 0;
        }
        else if (timer2 >= cd2)
        {
            Attack_02();
            timer2 = 0;
        }
        else
            Attack_03();
    }

    private void Attack_01()
    {
        animator.SetTrigger("attack_01");
    }

    private void Attack_01_Damage()
    {
        spell1.SetActive(true);
        spell1.transform.position = player.transform.position;

        Invoke("FinishAttack_01", spell1.GetComponentInChildren<ParticleSystem>().main.duration);
    }

    private void FinishAttack_01()
    {
        spell1.SetActive(false);
    }

    private void Attack_02()
    {
        animator.SetTrigger("attack_02");
    }

    private void Attack_02_Damage()
    {
        spell2.SetActive(true);
        spell2.transform.position = player.transform.position;

        Invoke("FinishAttack_02", spell2.GetComponentInChildren<ParticleSystem>().main.duration);
    }

    private void FinishAttack_02()
    {
        spell2.SetActive(false);
    }

    private void Attack_03()
    {
        animator.SetTrigger("attack_03");
    }

    private void Attack_03_Damage()
    {
        spell3.SetActive(true);
        spell3.transform.position = player.transform.position;

        Invoke("FinishAttack_03", spell3.GetComponentInChildren<ParticleSystem>().main.duration);
    }

    private void FinishAttack_03()
    {
        spell3.SetActive(false);
    }

    private bool InAttackAnimation()
    {
        return animator.GetBool("attack_01") || animator.GetBool("attack_02") || animator.GetBool("attack_03");
    }

}

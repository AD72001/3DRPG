using UnityEngine;
using UnityEngine.AI;

public class WizardEnemy: Enemy
{
    [SerializeField] private float safeRange;

    [SerializeField] private Transform[] safePoints;
    private int safePointIndex = 0;
    
    // Skills Rate
    [SerializeField] private float skillRate = 0.5f;
    [SerializeField] private float attackRate;

    // Skills effect
    [SerializeField] private GameObject spell1;
    [SerializeField] private GameObject spell2;
    [SerializeField] private GameObject spell3;

    // Audio
    [SerializeField] private AudioClip spell1Sound;
    [SerializeField] private AudioClip spell2Sound;
    [SerializeField] private AudioClip spell3Sound;

    // Cooldown
    [SerializeField] private float cd1;
    [SerializeField] private float timer1 = Mathf.Infinity;
    [SerializeField] private float cd2;
    [SerializeField] private float timer2 = Mathf.Infinity;

    bool isFleeing = false;
    public bool active = false;

    // Component
    private NavMeshAgent agent;
    
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        base.Update();

        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (!active || stunTime > 0) return;

        //Fleeing if player too close
        if (Vector3.Distance(player.transform.position, transform.position) <= safeRange)
        {
            if (!isAttacking)
                isFleeing = true;
        }

        if (isFleeing)
        {
            Flee();
            return;
        }

        //Attack
        if (!isAttacking) Attack();
    }

    private void Flee()
    {
        if (Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(safePoints[safePointIndex].position.x, 0, 
                    safePoints[safePointIndex].position.z)) < 0.5f)
        {
            animator.SetBool("moving", false);
            isFleeing = false;
            safePointIndex = Mathf.Clamp(safePointIndex+1, 0, safePoints.Length - 1);

            if (safePointIndex >= safePoints.Length-1)
                safePointIndex = 0;

            return;
        }

        animator.SetBool("moving", true);
        agent.destination = safePoints[safePointIndex].position;
    }

    public override void Attack()
    {
        attackRate = Random.Range(0.0f, 1.0f);

        transform.LookAt(player.transform);

        if (attackRate > skillRate)
        {
            if (timer1 >= cd1)
            {            
                Attack_01();
                timer1 = 0;
                return;
            }
        }
        else if (timer2 >= cd2)
        {
            Attack_02();
            timer2 = 0;
            return;
        }
        
        Attack_03();
    }

    private void Attack_01()
    {
        isAttacking = true;
        animator.SetTrigger("attack_01");
        AudioManager.instance.PlaySound(spell1Sound);
    }

    private void Attack_01_Damage()
    {
        spell1.SetActive(true);
        spell1.transform.position = player.transform.position;
        animator.SetBool("attacking", true);

        Invoke("FinishAttack_01", spell1.GetComponentInChildren<ParticleSystem>().main.duration*0.8f);
    }

    private void FinishAttack_01()
    {
        spell1.SetActive(false);
        animator.SetBool("attacking", false);
        isAttacking = false;
    }

    private void Attack_02()
    {        
        isAttacking = true;
        animator.SetTrigger("attack_02");
        AudioManager.instance.PlaySound(spell2Sound);
    }

    private void Attack_02_Damage()
    {
        spell2.SetActive(true);
        spell2.transform.position = player.transform.position;
        animator.SetBool("attacking", true);

        Invoke("FinishAttack_02", spell2.GetComponentInChildren<ParticleSystem>().main.duration*0.8f);
    }

    private void FinishAttack_02()
    {
        spell2.SetActive(false);
        animator.SetBool("attacking", false);
        isAttacking = false;
    }

    private void Attack_03()
    {        
        isAttacking = true;
        animator.SetTrigger("attack_03");
    }

    private void Attack_03_Damage()
    {
        spell3.SetActive(true);
        spell3.transform.position = player.transform.position;
        animator.SetBool("attacking", true);

        AudioManager.instance.PlaySound(spell3Sound);

        Invoke("FinishAttack_03", spell3.GetComponentInChildren<ParticleSystem>().main.duration*0.8f);
    }

    private void FinishAttack_03()
    {
        spell3.SetActive(false);
        animator.SetBool("attacking", false);
        isAttacking = false;
    }

    protected override bool InAttackAnimation()
    {
        return animator.GetBool("attacking") || 
            animator.GetBool("attack_01") || 
            animator.GetBool("attack_02") || 
            animator.GetBool("attack_03");
    }
}

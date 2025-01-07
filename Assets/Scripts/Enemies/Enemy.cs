using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Player Component
    protected GameObject player;

    // Enemy Stat
    public string enemyName;
    [SerializeField] private float activeRange = 40;
    [SerializeField] private float detectRange;
    [SerializeField] private float attackRange;
    [SerializeField] protected float speed;
    [SerializeField] private int exp;
    [SerializeField] private int cash;

    // Player In Range
    public GameObject playerInAttackRange;

    protected float stunTime = 0;

    [SerializeField] protected float attackCD;
    [SerializeField] protected float attackTimer = Mathf.Infinity;

    protected bool isAttacking = false;
    protected bool isDead = false;

    // Component
    protected CharacterController controller;
    protected Animator animator;

    private MouseUI mouseUI;

    
    protected virtual void Start()
    {
        exp = GetComponent<Stat>().level * 5;
        cash = GetComponent<Stat>().level * 20;
        player = GameObject.FindGameObjectWithTag("Player");
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mouseUI = GameObject.FindGameObjectWithTag("CursorUI").GetComponent<MouseUI>();
    }

    private void OnEnable() {
        isDead = false;
        FinishAttack();
        exp = GetComponent<Stat>().level * 5;
        cash = GetComponent<Stat>().level * 20;
    }

    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;

        if (GetComponent<HP>().defeat)
        {
            if (!isDead) 
                Dead();
            return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) >= activeRange)
        {
            gameObject.SetActive(false);
            return;
        }

        // Stun
        if (stunTime > 0)
        {
            Stun();
            return;
        }
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

    protected virtual bool InAttackAnimation()
    {
        return animator.GetBool("meleeAttack");
    }

    protected bool PlayerInRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= detectRange;
    }

    protected bool PlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= attackRange + player.GetComponent<CharacterController>().radius;
    }

    public virtual void Attack()
    {}

    protected virtual float DamageCalculator()
    {
        return GetComponent<Stat>().GetStr();
    }

    void Dead()
    {
        isDead = true;

        player.GetComponent<Stat>().AddExp(exp);
        player.GetComponent<CharacterInventory>().AddCash(cash);
        exp = 0;
        cash = 0;

        player.GetComponent<CharacterCombat>().opponent = null;
        
        animator.SetBool("stun", false);
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);

        CharacterMovement.isAttacking = false;

        if (gameObject.GetComponent<NavMeshAgent>())
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        }
    }

    public bool getDeadStatus()
    {
        return isDead;
    }

    private void OnMouseOver()
    {
        if (!isDead)
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
    }
}
